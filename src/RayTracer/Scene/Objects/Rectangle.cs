using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Represents rectangle object
    /// </summary>
    class Rectangle : AObject
    {
        //Edges of rectangle
        public Point a, b, c, d;

        //Normal of rectangle
        public Vector normal;
        //Vectors for rectangle description
        public Vector p, q;
        // Coefficient d 
        public float koefD;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID rectangle</param>
        /// <param name="a">Edge A</param>
        /// <param name="c">Edge C</param>
        public Rectangle(int ID, Point a, Point c)
        {
            this.ID = ID;
            this.a = a;
            this.c = c;

            this.b = new Point();
            this.b.X = c.X;
            this.b.Y = a.Y;
            this.b.Z = a.Z;

            this.d = new Point();
            this.d.X = a.X;
            this.d.Y = c.Y;
            this.d.Z = c.Z;

            this.kd = 1f;
            this.ks = 0f;
            this.kt = 0f;
            CalculateVectors();
        }

        /// <summary>
        /// Rectangle translation
        /// </summary>
        /// <param name="x">Change in X</param>
        /// <param name="y">Change in Y</param>
        /// <param name="z">Change in Z</param>
        public override void Translate(float x, float y, float z)
        {
            this.a.X += x;
            this.a.Y += y;
            this.a.Z += z;
            this.b.X += x;
            this.b.Y += y;
            this.b.Z += z;
            this.c.X += x;
            this.c.Y += y;
            this.c.Z += z;
            this.d.X += x;
            this.d.Y += y;
            this.d.Z += z;

            CalculateVectors();

        }

        //public void rotateZ(float angle)
        //{
        //    this.a.X = (float)(this.a.X * Math.Cos((Math.PI / 180) * angle) - this.a.Y * Math.Sin((Math.PI / 180) * angle));
        //    this.a.Y = (float)(this.a.X * Math.Sin((Math.PI / 180) * angle) + this.a.Y * Math.Cos((Math.PI / 180) * angle));
        //    this.b.X = (float)(this.b.X * Math.Cos((Math.PI / 180) * angle) - this.b.Y * Math.Sin((Math.PI / 180) * angle));
        //    this.b.Y = (float)(this.b.X * Math.Sin((Math.PI / 180) * angle) + this.b.Y * Math.Cos((Math.PI / 180) * angle));
        //    this.c.X = (float)(this.c.X * Math.Cos((Math.PI / 180) * angle) - this.c.Y * Math.Sin((Math.PI / 180) * angle));
        //    this.c.Y = (float)(this.c.X * Math.Sin((Math.PI / 180) * angle) + this.c.Y * Math.Cos((Math.PI / 180) * angle));
        //    this.d.X = (float)(this.d.X * Math.Cos((Math.PI / 180) * angle) - this.d.Y * Math.Sin((Math.PI / 180) * angle));
        //    this.d.Y = (float)(this.d.X * Math.Sin((Math.PI / 180) * angle) + this.d.Y * Math.Cos((Math.PI / 180) * angle));
        //}

        //public void rotateY(float angle)
        //{
        //    this.a.Z = (float)(this.a.Z * Math.Cos((Math.PI / 180) * angle) - this.a.X * Math.Sin((Math.PI / 180) * angle));
        //    this.a.X = (float)(this.a.Z * Math.Sin((Math.PI / 180) * angle) + this.a.X * Math.Cos((Math.PI / 180) * angle));
        //    this.b.Z = (float)(this.b.Z * Math.Cos((Math.PI / 180) * angle) - this.b.X * Math.Sin((Math.PI / 180) * angle));
        //    this.b.X = (float)(this.b.Z * Math.Sin((Math.PI / 180) * angle) + this.b.X * Math.Cos((Math.PI / 180) * angle));
        //    this.c.Z = (float)(this.c.Z * Math.Cos((Math.PI / 180) * angle) - this.c.X * Math.Sin((Math.PI / 180) * angle));
        //    this.c.X = (float)(this.c.Z * Math.Sin((Math.PI / 180) * angle) + this.c.X * Math.Cos((Math.PI / 180) * angle));
        //    this.d.Z = (float)(this.d.Z * Math.Cos((Math.PI / 180) * angle) - this.d.X * Math.Sin((Math.PI / 180) * angle));
        //    this.d.X = (float)(this.d.Z * Math.Sin((Math.PI / 180) * angle) + this.d.X * Math.Cos((Math.PI / 180) * angle));
        //}
        /// <summary>
        /// Rotation around X axis
        /// </summary>
        /// <param name="angle">Angle</param>
        public void RotateX(float angle)
        {
            float m1 = this.a.Y;
            float m2 = this.a.Z;
            float m3 = this.b.Y;
            float m4 = this.b.Z;
            float m5 = this.c.Y;
            float m6 = this.c.Z;
            float m7 = this.d.Y;
            float m8 = this.d.Z;

            this.a.Y = (float)(m1 * Math.Cos((Math.PI / 180) * angle) - m2 * Math.Sin((Math.PI / 180) * angle));
            this.a.Z = (float)(m1 * Math.Sin((Math.PI / 180) * angle) + m2 * Math.Cos((Math.PI / 180) * angle));
            this.b.Y = (float)(m3 * Math.Cos((Math.PI / 180) * angle) - m4 * Math.Sin((Math.PI / 180) * angle));
            this.b.Z = (float)(m3 * Math.Sin((Math.PI / 180) * angle) + m4 * Math.Cos((Math.PI / 180) * angle));
            this.c.Y = (float)(m5 * Math.Cos((Math.PI / 180) * angle) - m6 * Math.Sin((Math.PI / 180) * angle));
            this.c.Z = (float)(m5 * Math.Sin((Math.PI / 180) * angle) + m6 * Math.Cos((Math.PI / 180) * angle));
            this.d.Y = (float)(m7 * Math.Cos((Math.PI / 180) * angle) - m8 * Math.Sin((Math.PI / 180) * angle));
            this.d.Z = (float)(m7 * Math.Sin((Math.PI / 180) * angle) + m8 * Math.Cos((Math.PI / 180) * angle));


            CalculateVectors();
        }

        /// <summary>
        /// Calculate vectors p a q
        /// </summary>
        public void CalculateVectors()
        {
            q = new Vector(c.X - d.X, c.Y - d.Y, c.Z - d.Z);
            p = new Vector(a.X - d.X, a.Y - d.Y, a.Z - d.Z);
            if (ID.Equals("OD4") || ID.Equals("OD2") || ID.Equals("OD5") || ID.Equals("OD6"))
            {
                normal = Vector.CrossProduct(q, p);
            }
            else
            {
                normal = Vector.CrossProduct(p, q);
            }

            koefD = -((normal.x * d.X) + (normal.y * d.Y) + (normal.z * d.Z));
        }


        /// <summary>
        /// Set color
        /// </summary>
        /// <param name="r">RED</param>
        /// <param name="g">GREEN</param>
        /// <param name="b">BLUE</param>
        public override void SetColor(float r, float g, float b)
        {
            color = new Color(r, g, b);
        }

        /// <summary>
        /// Find out if there is a intersection, if so return it
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>intersection</returns>
        public override Intersection GetIntersection(Ray ray)
        {
            Intersection pr = new Intersection();

            Vector normalOfFloor = normal;


            float nDir = Vector.DotProduct(normalOfFloor, ray.direction);

            if (Math.Abs(nDir) < 0.001) return null;

            else
            {
                float nA = ((normalOfFloor.x * ray.startPoint.X) + (normalOfFloor.y * ray.startPoint.Y) + (normalOfFloor.z * ray.startPoint.Z));
                float t = -((nA + koefD) / nDir);



                if (t > 0.001)
                {
                    Point intersection = new Point(ray.startPoint.X + t * ray.direction.x, ray.startPoint.Y + t * ray.direction.y, ray.startPoint.Z + t * ray.direction.z);
                    Vector b = new Vector(intersection.X - d.X, intersection.Y - d.Y, intersection.Z - d.Z);
                    float L = ((p.x / p.Magnitude() * b.x) + (p.y / p.Magnitude() * b.y) + (p.z / p.Magnitude() * b.z));
                    float g = ((q.x / q.Magnitude() * b.x) + (q.y / q.Magnitude() * b.y) + (q.z / q.Magnitude() * b.z));

                    float S = L / p.Magnitude();
                    float S1 = g / q.Magnitude();
                    if (S <= 1 && S >= 0 && S1 <= 1 && S1 >= 0)
                    {
                        pr = InitializeIntersection(ray, t, intersection);

                    }
                    else return null;
                }
                else return null;

            }






            return pr;
        }

        /// <summary>
        /// Initialize intersection object
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <param name="t">Parameter t</param>
        /// <param name="interSectionPoint">Point of interrest</param>
        /// <returns>Intersection object</returns>
        private Intersection InitializeIntersection(Ray ray, float t, Point interSectionPoint)
        {
            Intersection p = new Intersection();
            p.color = color;
            p.t = t;
            p.t2 = t;
            p.pointOfIntersection = interSectionPoint;
            p.normal = normal;
            p.ray = ray;
            p.kd = kd;
            p.ks = ks;
            p.kt = kt;
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
        /// <param name="b">point</param>
        /// <returns>color</returns>
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
