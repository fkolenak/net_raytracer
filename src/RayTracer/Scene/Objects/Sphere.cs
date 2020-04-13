using System;

namespace RayTracer
{
    /// <summary>
    /// Represents a sphere
    /// </summary>
    class Sphere : AObject
    {
        //Parameters of sphere
        public float xPos, yPos, zPos, diameter;
        /// <summary>
        /// Constructor
        /// </summary>
        public Sphere()
        {
            this.ID = 1;
            this.xPos = 0;
            this.yPos = 0;
            this.zPos = 0;
            this.diameter = 1;

            this.kd = 0.3f;
            this.ks = 0.2f;
            this.kt = 0.5f;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID</param>
        public Sphere(int ID) : this()
        {
            this.ID = ID;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="xPos">X position of middle</param>
        /// <param name="yPos">Y position of middle</param>
        /// <param name="zPos">Z position of middle</param>
        public Sphere(int ID, float xPos, float yPos, float zPos) : this(ID)
        {
            this.xPos = xPos;
            this.yPos = yPos;
            this.zPos = zPos;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="xPos">X position of middle</param>
        /// <param name="yPos">Y position of middle</param>
        /// <param name="zPos">Z position of middle</param>
        /// <param name="diameter">diameter</param>
        public Sphere(int ID, float xPos, float yPos, float zPos, float diameter) : this(ID, xPos, yPos, zPos)
        {
            this.diameter = diameter;

            this.kd = 0.3f;
            this.ks = 0.2f;
            this.kt = 0.5f;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">id</param>
        /// <param name="location">location</param>
        /// <param name="diameter">diameter</param>
        public Sphere(int ID, Point location, float diameter) : this(ID, location.X, location.Y, location.Z, diameter)
        {
        }

        /// <summary>
        /// Set color
        /// </summary>
        /// <param name="r">RED</param>
        /// <param name="g">GREEN</param>
        /// <param name="b">BLUE</param>
        public override void SetColor(float r, float g, float b)
        {
            this.color = new Color(r, g, b);
        }

        /// <summary>
        /// Set color
        /// </summary>
        /// <param name="color">color</param>
        public void SetColor(Color color)
        {
            this.color = color;
        }
        /// <summary>
        /// Translate
        /// </summary>
        /// <param name="x">Move in X</param>
        /// <param name="y">Move in Y</param>
        /// <param name="z">Move in Z</param>
        public override void Translate(float x, float y, float z)
        {
            this.xPos += x;
            this.yPos += y;
            this.zPos += z;
        }

        /// <summary>
        /// Gets intersection with this object if it exists
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>Intersection if exists, null otherwise</returns>
        public override Intersection GetIntersection(Ray ray)
        {
            Intersection p = null;
            ray.direction.Normalize();

            float t1 = -1;
            float t2 = -1;
            float pom = -1;

            Vector v = new Vector(ray.startPoint.X - xPos, ray.startPoint.Y - yPos, ray.startPoint.Z - zPos);
            float multVD = 2 * Vector.DotProduct(v, ray.direction);

            if ((multVD * multVD - 4 * (Vector.DotProduct(v, v) - diameter * diameter) > 0))
            {
                float square = (float)Math.Sqrt(multVD * multVD - 4 * (Vector.DotProduct(v, v) - diameter * diameter));

                t1 = (-multVD + square) / 2;
                t2 = (-multVD - square) / 2;
                if (t2 > 0 && t1 > 0 && t1 > t2)
                {
                    pom = t1;
                    t1 = t2;
                    t2 = pom;
                }
                if (t1 > 0.001)
                {

                    p = inicializujPrusecik(ray, t1, t2);


                }
                else if (t2 > 0.001)
                {

                    p = inicializujPrusecik(ray, t2, t1);

                }
                else return null;


            }
            else return null;

            return p;
        }

        /// <summary>
        /// Initialize intersection
        /// </summary>
        /// <param name="ray">ray</param>
        /// <param name="t">t</param>
        /// <param name="t2">t2</param>
        /// <returns>Intersection object</returns>
        private Intersection inicializujPrusecik(Ray ray, float t, float t2)
        {

            Intersection p = new Intersection();
            p.color = color;
            p.t = t;
            p.t2 = t2;
            p.pointOfIntersection = new Point(ray.startPoint.X + t * ray.direction.x, ray.startPoint.Y + t * ray.direction.y, ray.startPoint.Z + t * ray.direction.z);        //Vypocte pozici pruseciku

            p.normal = new Vector(2 * (p.pointOfIntersection.X - xPos), 2 * (p.pointOfIntersection.Y - yPos), 2 * (p.pointOfIntersection.Z - zPos));
            if (Vector.DotProduct(ray.direction, p.normal) > 0)
            {
                p.normal = (-1) * p.normal;
            }
            p.pointOfIntersection2 = new Point(ray.startPoint.X + t2 * ray.direction.x, ray.startPoint.Y + t2 * ray.direction.y, ray.startPoint.Z + t2 * ray.direction.z);
            p.normalEnd = new Vector(2 * (p.pointOfIntersection2.X - xPos), 2 * (p.pointOfIntersection2.Y - yPos), 2 * (p.pointOfIntersection2.Z - zPos));
            if (Vector.DotProduct(ray.direction, p.normalEnd) > 0)
            {
                p.normalEnd = (-1) * p.normalEnd;
            }
            p.kd = kd;
            p.ks = ks;
            p.kt = kt;

            p.n2 = ktrefraction;

            p.ray = ray;
            return p;
        }

        /// <summary>
        /// Set properties
        /// </summary>
        /// <param name="kD">diffusion</param>
        /// <param name="kS">Reflection</param>
        /// <param name="kT">Transparency</param>
        public override void SetProperties(float kD, float kS, float kT)
        {
            this.kd = kD;
            this.ks = kS;
            this.kt = kT;
        }

        /// <summary>
        /// Get color
        /// </summary>
        /// <param name="b">Point of interrest</param>
        /// <returns>color</returns>
        public override Color GetColor(Point b)
        {
            return color;
        }
        /// <summary>
        /// Set refraction index
        /// </summary>
        /// <param name="refraction">refraction</param>
        public override void SetRefraction(float refraction)
        {
            this.ktrefraction = refraction;
        }
    }
}
