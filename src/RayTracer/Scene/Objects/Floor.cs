using System;

namespace RayTracer
{
    /// <summary>
    /// Represents floor object (rectangle with chess patern)
    /// </summary>
    public class Floor : AObject
    {
        //Floor edges
        public Point a, b, c, d;

        //Normal of floor
        public Vector normal;
        //Vectors for floor description
        public Vector p, q;
        // Coefficient d 
        public float koefD;
        /// <summary>
        /// For chess pattern
        /// </summary>
        public int M = 20, N = 20;
        /// <summary>
        /// Colors of chess pattern
        /// </summary>
        public Color color1, color2;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">floor ID</param>
        /// <param name="a">Edge A</param>
        /// <param name="c">Edge C</param>
        public Floor(int ID, Point a, Point c)
        {
            this.ID = ID;
            this.a = a;
            this.c = c;

            this.b = new Point
            {
                X = c.X,
                Y = a.Y,
                Z = a.Z
            };

            this.d = new Point
            {
                X = a.X,
                Y = c.Y,
                Z = c.Z
            };


            CalculatVectors();
            normal = Vector.CrossProduct(q, p);
            koefD = -((normal.x * d.X) + (normal.y * d.Y) + (normal.z * d.Z));

            this.kd = 0.8f;
            this.ks = 0.2f;

        }

        /// <summary>
        /// Floor translation
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

            CalculatVectors();
            normal = Vector.CrossProduct(q, p);
            koefD = -((normal.x * d.X) + (normal.y * d.Y) + (normal.z * d.Z));
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
        /// Roatation
        /// </summary>
        /// <param name="angle">angle</param>
        public void rotateX(float angle)
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


            CalculatVectors();
            normal = Vector.CrossProduct(q, p);
            koefD = -((normal.x * d.X) + (normal.y * d.Y) + (normal.z * d.Z));

        }

        /// <summary>
        /// Recalculates vectors p and q
        /// </summary>
        public void CalculatVectors()
        {
            q = new Vector(c.X - d.X, c.Y - d.Y, c.Z - d.Z);
            p = new Vector(a.X - d.X, a.Y - d.Y, a.Z - d.Z);
        }


        /// <summary>
        /// Set color of floor - not needed
        /// </summary>
        /// <param name="r">RED</param>
        /// <param name="g">GREEN</param>
        /// <param name="b">BLUE</param>
        public override void SetColor(float r, float g, float b)
        {

        }

        public void SetColors(Color color1, Color color2)
        {
            this.color1 = color1;
            this.color2 = color2;
        }
        /// <summary>
        /// Find out if there is a intersection, if so return it
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>intersection</returns>
        public override Intersection GetIntersection(Ray ray)
        {
            Intersection pr = new Intersection();

            Vector normalFloor = normal;
            float nDir = Vector.DotProduct(normalFloor, ray.direction);

            if (Math.Abs(nDir) < 0.001) return null;

            else
            {
                float nA = ((normalFloor.x * ray.startPoint.X) + (normalFloor.y * ray.startPoint.Y) + (normalFloor.z * ray.startPoint.Z));
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
        /// <param name="intersectionPoint">Intersection point</param>
        /// <returns>Intersection object</returns>
        private Intersection InitializeIntersection(Ray ray, float t, Point intersectionPoint)
        {
            Intersection p = new Intersection();
            p.color = color;
            p.t = t;
            p.pointOfIntersection = intersectionPoint;
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
        /// <param name="kD">Diffusion</param>
        /// <param name="kS">Reflection</param>
        /// <param name="kT">Transparency</param>
        public override void SetProperties(float kD, float kS, float kT)
        {
            this.kd = kD;
            this.ks = kS;
            this.kt = kT;
        }
        /// <summary>
        /// Get color of floor
        /// </summary>
        /// <param name="b">point of interrest</param>
        /// <returns>color</returns>
        public override Color GetColor(Point b)
        {
            if (M == 0 && N == 0) return color;

            double x/* = (a.X - b.X) / (c.X - a.X)*/;
            double y/* = (c.Z - b.Z) / (c.Z - a.Z)*/;
            Vector pr = new Vector(b.X - d.X, b.Y - d.Y, b.Z - d.Z);
            x = Vector.DotProduct(p, pr) / Vector.DotProduct(p, p);
            y = Vector.DotProduct(q, pr) / Vector.DotProduct(q, q);

            int xx = Convert.ToInt16(((Math.Floor(x * 100 / (100 / M))) + (Math.Floor(y * 100 / (100 / N)))) % 2); //1 or 0 for black and white

            //xx = Convert.ToInt16((x * M + y * N) % 2); // Makes stripes :] ... Happy accident

            if (xx == 0)
            {
                return color1;
            }
            else return color2;
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
