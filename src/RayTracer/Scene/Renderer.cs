using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RayTracer
{
    class Renderer
    {
        /// <summary>
        /// Constant to maintain darkness of shadow 
        /// </summary>
        private const float Ia = 0.075f;
        /// <summary>
        /// Power
        /// </summary>
        private const int power = 100;

        /// <summary>
        /// Scene to render
        /// </summary>
        private Scene s;
        /// <summary>
        /// Scene width
        /// </summary>
        private int width;
        /// <summary>
        /// Scene height
        /// </summary>
        private int height;
        /// <summary>
        /// Objects to render
        /// </summary>
        private List<AObject> renderedObjects;

        public Renderer(Scene s, List<AObject> renderedObjects)
        {
            this.s = s;
            this.renderedObjects = renderedObjects;
            this.width = s.width;
            this.height = s.height;
        }

        /// <summary>
        /// Render and create window with set pixels
        /// </summary>
        /// <returns>Rendered window</returns>
        public Window Render()
        {
            Window window = new Window(s.width, s.height);

            Point cameraPosition = GetCamera().location;

            float h = (float)(2 * Math.Tan((Math.PI / 180) * (GetCamera().fovy / 2)));

            float w = (h * width) / (height);
            Point S = cameraPosition + GetCamera().direction;  //Get position of point S

            Point origin = S + (((0.5 * h) * GetCamera().up) + ((-0.5 * w) * GetCamera().right)); // Find 0,0 on canvas

            float dx = w / width;
            float dy = h / height;

            Parallel.For(0, height, i =>      //Cycle
            {
                for (int j = 0; j < width; j++)    //Cycle
                {
                    Intersection p;
                    Color c;

                    Point pixel = origin + j * dx * GetCamera().right - i * (dy * GetCamera().up);        //Position of rendered pixel
                    Vector direction = new Vector(pixel.X - cameraPosition.X, pixel.Y - cameraPosition.Y, pixel.Z - cameraPosition.Z);   //Actual direction
                    direction.Normalize();

                    Ray ray = new Ray(cameraPosition, direction); //Make ray

                    p = null;
                    for (int k = 0; k < renderedObjects.Count; k++) //For each object check if there is intersection
                    {
                        s.intersectionCalculationCount++;
                        var intersection = renderedObjects[k].GetIntersection(ray);

                        if (intersection != null)
                        {
                            intersection.indexOfCrossedObj = k;
                        }
                        if (p == null)
                        {
                            if (intersection != null && intersection.t >= 0.001)
                            {
                                p = intersection;
                            }
                        }
                        else if (intersection != null)
                        {
                            if (p.t > intersection.t && intersection.t >= 0.001)
                            {
                                p = intersection;
                            }
                        }
                    }
                    if (p != null)
                    {
                        ray.p = p;
                        ray.rayIntensity = 1;
                        c = getColor(ray, 1); //Get color of given pixel
                    }
                    else
                    {
                        c = s.backgroundColor;
                    }
                    window.SetPixel(i, j, c);       //Set color of given pixel
                }
            });
            return window;
        }

        /// <summary>
        /// Get color of given pixel
        /// </summary>
        /// <param name="ray">Ray (that we 'shoot' with)</param>
        /// <param name="generation">generation</param>
        /// <returns>Pixel color</returns>
        private Color getColor(Ray ray, int generation)
        {
            Color c = new Color();
            Intersection p = ray.p;
            if (generation < s.maxDepth && ray.rayIntensity > s.minIntensity)
            {
                float Id = 0;
                float Is = 0;

                c = renderedObjects[p.indexOfCrossedObj].GetColor(p.pointOfIntersection);

                Ray ray2 = new Ray(p.pointOfIntersection, new Vector(s.light.xPos - (p.pointOfIntersection.X), s.light.yPos - (p.pointOfIntersection.Y), s.light.zPos - (p.pointOfIntersection.Z))); // Paprsek z pruseciku do svetla

                Intersection swadowIntersection = null;



                if (!isShadow(p.indexOfCrossedObj, ray2, renderedObjects, out swadowIntersection)) // If object is not in shadow calculates its shaded color
                {
                    Id = phong_D(ray2, p.normal);
                    if (Id < Ia)
                    {
                        Id = Ia;
                    }
                    else if (Id > 1)
                    {
                        Id = 1;
                    }
                }
                else
                {
                    Id = Ia * (1 + swadowIntersection.kt); // The more transpanet the object is the more lighter the shadow will be
                    
                }

                Vector bounce = bounceVector(p);   //Directional vector of the ray that will bounce

                if (generation == 1)
                {
                    float koef = Vector.DotProduct(ray2.direction, bounce);

                    if (koef < 0) koef *= -1;
                    else koef = 0;


                    Is = (float)Math.Pow(koef, power);     //Shine part

                }
                Color reflected = new Color(0, 0, 0);
                Ray reflectionRay = new Ray(p.pointOfIntersection, -1 * bounce);
                reflectionRay.direction.Normalize();
                if (p.ks != 0)      //Calculation for mirror part
                {
                    Intersection pr = GetIntersection(reflectionRay);

                    if (pr != null)
                    {
                        reflectionRay.refractionIndex = renderedObjects[pr.indexOfCrossedObj].ktrefraction;
                        reflectionRay.p = pr;
                        reflectionRay.rayIntensity = ray.rayIntensity * p.ks;
                        reflected = getColor(reflectionRay, generation + 1);
                    }
                    else reflected = s.backgroundColor;

                }

                Color refracted = new Color(0, 0, 0);
                if (p.kt != 0)        //Calculation for refraction.
                {
                    ray.isIn = p.isIn;
                    p.normal.Normalize();
                    p.normalEnd.Normalize();
                    p.n1 = 1;
                    p.n2 = 1f;
                    Vector t;
                    float cos1;
                    float cos2;
                    if (!ray.isIn)      //Check if the ray is in objecy
                    {
                        p.n1 = 1;
                        p.n2 = renderedObjects[p.indexOfCrossedObj].ktrefraction;

                        cos1 = -Vector.DotProduct(p.normal, ray.direction);
                    }
                    else
                    {
                        p.n1 = renderedObjects[p.indexOfCrossedObj].ktrefraction;
                        p.n2 = 1;
                        cos1 = -Vector.DotProduct(-1 * p.normal, ray.direction);
                    }
                    cos2 = (float)Math.Sqrt(1 - (p.n1 / p.n2) * (p.n1 / p.n2) * (1 - cos1 * cos1));
                    t = (p.n1 / p.n2) * ray.direction + ((p.n1 * cos1) / p.n2 - cos2) * (p.normal);

                    Ray refractedRay = new Ray(p.pointOfIntersection, t);
                    refractedRay.direction.Normalize();
                    refractedRay.p = GetIntersection(refractedRay);
                    if (refractedRay.p != null)
                    {
                        refractedRay.p.normal.Normalize();
                        refractedRay.p.normalEnd.Normalize();
                        refractedRay.rayIntensity = ray.rayIntensity * p.kt;

                        refracted = getColor(refractedRay, generation + 1);
                    }
                    else refracted = s.backgroundColor;
                }

                if (generation == 1) c = ray.rayIntensity * ((p.kd * Id + Is) * c + p.ks * reflected + p.kt * refracted); // Calculation for first generation

                else c = ray.rayIntensity * ((2f * p.kd * Id) * c + p.ks * reflected + p.kt * refracted); // Calculation for second, third... not accounting the shiny component and lighten the diffuse component 2x    
                
                //Normalize the color.
                if (c.r > 1) c.r = 1;
                if (c.g > 1) c.g = 1;
                if (c.b > 1) c.b = 1;
                if (c.r < 0) c.r = 0;
                if (c.g < 0) c.g = 0;
                if (c.b < 0) c.b = 0;
            }
            return c;
        }

        /// <summary>
        /// Get used camera
        /// </summary>
        /// <returns>camera</returns>
        private Camera GetCamera()
        {
            return s.camera;
        }

        /// <summary>
        /// Creates bounced vector (directing from intersection) from given input vector (direction towards intersection)
        /// </summary>
        /// <param name="p">Input vector</param>
        /// <returns>bounced vector</returns>
        private Vector bounceVector(Intersection p)
        {
            Vector smer = p.ray.direction;
            smer.Normalize();
            Vector n = new Vector(p.normal);
            n.Normalize();

            Vector en = 2 * Vector.CrossProduct(smer, n);
            Vector v = Vector.CrossProduct(en, n) + smer;
            return v;
        }

        /// <summary>
        /// Calculate the constant by which we multiply the color
        /// </summary>
        /// <param name="ray2">Ray towards the light</param>
        /// <param name="normalInIntersection">normal in intersection</param>
        /// <returns></returns>
        private static float phong_D(Ray ray2, Vector normalInIntersection)
        {
            Vector direction = new Vector(ray2.direction);
            direction.Normalize();
            Vector copyN = new Vector(normalInIntersection);
            copyN.Normalize();

            return Vector.DotProduct(direction, copyN);
        }

        /// <summary>
        /// Gets intersection with this object if it exists
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>Intersection if exists, null otherwise</returns>
        private Intersection GetIntersection(Ray ray)
        {
            Intersection p = null;
            for (int k = 0; k < renderedObjects.Count; k++) //for every object check intersection
            {
                s.intersectionCalculationCount++;
                var intersection = renderedObjects[k].GetIntersection(ray);

                if (intersection != null)
                {
                    intersection.indexOfCrossedObj = k;
                }
                if (p == null)
                {
                    if (intersection != null && intersection.t >= 0.001)
                    {
                        p = intersection;
                    }
                }
                else if (intersection != null)
                {
                    if (p.t > intersection.t && intersection.t >= 0.001)
                    {
                        p = intersection;
                    }
                }
            }

            return p;
        }

        /// <summary>
        /// Find out if the location of intersection is in the shade
        /// </summary>
        /// <param name="indexObj">object index</param>
        /// <param name="ray">ray to check</param>
        /// <param name="renderingObjects">objects to check</param>
        /// <param name="p">intersection</param>
        /// <returns>if its in shadow</returns>
        private Boolean isShadow(int indexObj, Ray ray, List<AObject> renderingObjects, out Intersection p)
        {
            p = null;
            Vector directinControl = new Vector(ray.direction);



            for (int i = 0; i < renderingObjects.Count; i++) //Check intersection for every object
            {
                s.intersectionCalculationCount++;
                var intersection = renderingObjects[i].GetIntersection(ray);


                if (intersection != null)
                {
                    if (directinControl.Magnitude() < intersection.t)       //Check if the object is not behind light
                    {
                        continue;
                    }
                    if (intersection.t > 0.001 && intersection.kt != 1)
                    {
                        if (intersection.t2 < 0.001 && intersection.t2 > -0.001)
                        {
                            p = intersection;
                            break;
                        }
                        if (p == null)
                        {
                            p = intersection;
                            continue;
                        }
                        if (p.kt >= intersection.kt)
                        {
                            p = intersection;
                        }
                    }

                }
            }
            if (p != null)
            {
                if (p.t2 < 0.001 && p.t2 > -0.001 && p.t > 0.001) p = null;
            }

            if (p == null) return false;
            else return true;
        }
    }
}
