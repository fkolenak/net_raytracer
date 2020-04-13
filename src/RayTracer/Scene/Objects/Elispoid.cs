using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Represents elipsoid object
    /// </summary>
    class Elispoid : AObject
    {
        /// <summary>
        /// Parameters of elipsoid
        /// </summary>
        public float a, b, c;
        /// <summary>
        /// Middle position point
        /// </summary>
        public float x, y, z;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="a">Parameter a</param>
        /// <param name="b">Parameter b</param>
        /// <param name="c">Parameter c</param>
        public Elispoid(int ID,float a, float b, float c)
        {
            this.ID = ID;
            this.a = a;
            this.b = b;
            this.c = c;
            this.x = 0;
            this.y = 0;
            this.z = 0;

            this.kd = 0.3f;
            this.ks = 0.2f;
            this.kt = 0.5f;
        }

        /// <summary>
        /// Translation of middle point
        /// </summary>
        /// <param name="x">Move by x</param>
        /// <param name="y">Move by y</param>
        /// <param name="z">Move by z</param>
        public override void Translate(float x, float y, float z)
        {
            this.x += x;
            this.y += y;
            this.z += z;
        }

        /// <summary>
        /// Set color of elipsoid
        /// </summary>
        /// <param name="r">RED</param>
        /// <param name="g">GREEN</param>
        /// <param name="b">BLUE</param>
        public override void SetColor(float r, float g, float b)
        {
            this.color = new Color(r, g, b);
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
        /// Find out if there is a intersection, if so return it
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>intersection</returns>
        public override Intersection GetIntersection(Ray ray)
        {
            
            ray.direction.Normalize();
            ray.direction.w = 0;
            Intersection p = null;
            Vector m = new Vector(ray.direction.x * b*b*c*c ,ray.direction.y*a*a*c*c,ray.direction.z * a*a*b*b,ray.direction.w*-a*a*b*b*c*c); // Direction times matrix C
            float memberA = m.x * ray.direction.x + m.y * ray.direction.y + m.z * ray.direction.z + m.w * ray.direction.w;

            Vector AminS = new Vector(ray.startPoint.X - x, ray.startPoint.Y - y, ray.startPoint.Z - z);      //A-S ... position ray = A, S is middle of elipsouidu


            float memberB = 2 * (m.x * AminS.x + m.y * AminS.y + m.z * AminS.z + m.w * AminS.w);


            Vector AminSTimesC = new Vector(AminS.x * b * b * c * c, AminS.y * a * a * c * c, AminS.z * a * a * b * b, -1*(a * a * b * b * c * c));
            float clenC = AminSTimesC.x * AminS.x + AminSTimesC.y * AminS.y + AminSTimesC.z * AminS.z + AminSTimesC.w * AminS.w;

            float discriminant = memberB * memberB - 4 * memberA * clenC;

            float t1, t2, pom;
            
            if (discriminant > 0.000001)
            {
                t1 = (-memberB + (float)Math.Sqrt(discriminant))/(2*memberA);

                t2 = (-memberB - (float)Math.Sqrt(discriminant)) / (2 * memberA);
            
                if(t2 > 0.001 && t1 > 0.001 && t1 > t2)
                {
                    pom = t1;
                    t1 = t2;
                    t2 = pom;
                }
                if(t1>0.001)
                {
                    p = InitializeIntersection(ray, t1,t2);
                }
                else if(t2>0.001)
                {
                    p = InitializeIntersection(ray, t2,t1);
                }
            }
            else return null;



            
            return p;
        }
        /// <summary>
        /// Initialize intersection object
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <param name="t">Parameter t</param>
        /// <param name="t2">Parameter t2</param>
        /// <returns>Intersection object</returns>
        private Intersection InitializeIntersection(Ray ray, float t ,float t2)
        {
            Intersection p = new Intersection();
            p.color = color;
            p.t = t;
            p.t2 = t2;
            
            p.pointOfIntersection = new Point( ray.startPoint.X + t * ray.direction.x,  ray.startPoint.Y + t * ray.direction.y, ray.startPoint.Z + t * ray.direction.z);

            p.normal = new Vector((2 * (p.pointOfIntersection.X - x))/(a*a),( 2 * (p.pointOfIntersection.Y - y))/(b*b), (2 * (p.pointOfIntersection.Z - z))/(c*c));
            if (Vector.DotProduct(ray.direction, p.normal) > 0)
            {
                p.normal = (-1) * p.normal;
            }
            p.pointOfIntersection2 = new Point(ray.startPoint.X + t2 * ray.direction.x, ray.startPoint.Y + t2 * ray.direction.y, ray.startPoint.Z + t2 * ray.direction.z);
            p.normalEnd = new Vector((2 * (p.pointOfIntersection2.X - x)) / (a * a), (2 * (p.pointOfIntersection2.Y - y)) / (b * b), (2 * (p.pointOfIntersection2.Z - z)) / (c * c));
            if (Vector.DotProduct(ray.direction, p.normalEnd) > 0)
            {
                p.normalEnd = (-1) * p.normalEnd;
            }
            p.kd = kd;
            p.ks = ks;
            p.kt = kt;
            p.ray = ray;
            return p;
        }

        /// <summary>
        /// Get color of object
        /// </summary>
        /// <param name="b">Intersection point (not needed here)</param>
        /// <returns></returns>
        public override Color GetColor(Point b)
        {
            return color;
        }
        /// <summary>
        /// Set refraction index
        /// </summary>
        /// <param name="refraction">refraction index</param>
        public override void SetRefraction(float refraction)
        {
            this.ktrefraction = refraction;
        }
    }
}
