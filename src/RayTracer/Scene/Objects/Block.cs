using System;
using System.Collections.Generic;

namespace RayTracer
{
    /// <summary>
    /// Object represents a block
    /// </summary>
    public class Block : AObject
    {
        /// <summary>
        /// Points of a block
        /// </summary>
        public Point a, b, c, d, a1, b1, c1, d1;
        /// <summary>
        /// Rectangles of block
        /// </summary>
        private readonly List<Rectangle> rectangles;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of block</param>
        /// <param name="min">Point a / Min</param>
        /// <param name="max">Point c1 / Max</param>
        public Block(int ID, Point min, Point max)
        {
            this.ID = ID;
            rectangles = new List<Rectangle>();
            SetPosition(min, max);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="depth">depth</param>
        public Block(double x, double y, double z, float width, float height, float depth): this(-1, new Point((float)x, (float)y, (float)z), new Point((float) x + width, (float) y + height, (float) z + depth))
        {}

        /// <summary>
        /// Sets position
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="depth">depth</param>
        public void SetPosition(float x, float y, float z, float width, float height, float depth)
        {
            this.SetPosition(new Point((float)x, (float)y, (float)z), new Point(x + width, y + height, z + depth));
        }

        /// <summary>
        /// Sets position
        /// </summary>
        /// <param name="min">Point a / Min</param>
        /// <param name="max">Point c1 / Max</param>
        public void SetPosition(Point min, Point max)
        {
            Rectangle rectangle1, rectangle2, rectangle3, rectangle4, rectangle5, rectangle6;
            rectangles.Clear();
            a = min;
            c1 = max;

            b = new Point(c1.X, a.Y, a.Z);
            c = new Point(c1.X, c1.Y, a.Z);
            d = new Point(a.X, c1.Y, a.Z);

            a1 = new Point(a.X, a.Y, c1.Z);
            b1 = new Point(c1.X, a.Y, c1.Z);
            d1 = new Point(a.X, c1.Y, c1.Z);

            rectangle1 = new Rectangle(1, a, c);
            rectangle2 = new Rectangle(2, b, c1);
            rectangle3 = new Rectangle(3, b1, d1);
            rectangle4 = new Rectangle(4, a1, d);
            rectangle5 = new Rectangle(5, c, d1);
            rectangle6 = new Rectangle(6, a, b1);
            rectangles.Add(rectangle1);
            rectangles.Add(rectangle2);
            rectangles.Add(rectangle3);
            rectangles.Add(rectangle4);
            rectangles.Add(rectangle5);
            rectangles.Add(rectangle6);

            this.kd = 1f;
            this.ks = 0f;
            this.kt = 0f;
        }

        

        public float getHeight()
        {
            return Math.Abs(d.Y - a.Y);
        }

        public float getWidth()
        {
            return Math.Abs(b.X - a.X);
        }

        public float getDepth()
        {
            return Math.Abs(c1.Z - a.Z);
        }

        /// <summary>
        /// Translation of object
        /// </summary>
        /// <param name="x">Change in X by x</param>
        /// <param name="y">Change in Y by y</param>
        /// <param name="z">Change in Z by z</param>
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

            this.a1.X += x;
            this.a1.Y += y;
            this.a1.Z += z;
            this.b1.X += x;
            this.b1.Y += y;
            this.b1.Z += z;
            this.c1.X += x;
            this.c1.Y += y;
            this.c1.Z += z;
            this.d1.X += x;
            this.d1.Y += y;
            this.d1.Z += z;

            foreach(Rectangle o in rectangles)
            {
                o.Translate(x, y, z);
            }
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
            foreach(Rectangle o in rectangles)
            {
                o.color = new Color(r, g, b);
            }
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
            foreach (Rectangle o in rectangles)
            {
                o.SetProperties(kD,kS,kT);
            }
        }


        /// <summary>
        /// Gets intersection with this object if it exists
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>Intersection if exists, null otherwise</returns>
        public override Intersection GetIntersection(Ray ray)
        {
            Intersection pr = new Intersection();
            pr.t = float.MaxValue;
            Intersection pom;
            foreach (Rectangle o in rectangles)
            {
                pom = o.GetIntersection(ray);
                if(pom != null)
                {
                    if(pom.t < pr.t)
                    {
                        pr = pom;
                    }
                }


                pom = null;
            }

            if (pr.t == float.MaxValue)
            {
                return null;
            }
            else
            {
                
                return pr;
            }
        }

        /// <summary>
        /// Initialize intersection object
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <param name="t">Parameter t t</param>
        /// <param name="pointOfIntersection">The point where the intersection is</param>
        /// <returns>Intersection</returns>
        private Intersection InitializeIntersection(Ray ray, float t, Point pointOfIntersection)
        {
            Intersection p = new Intersection();
            p.color = color;
            p.t = t;
            p.pointOfIntersection = pointOfIntersection;
            p.kd = kd;
            p.ks = ks;
            p.kt = kt;
            p.ray = ray;

            return p;
        }

        /// <summary>
        /// Color getter
        /// </summary>
        /// <param name="b"></param>
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
        /// <summary>
        /// Set color
        /// </summary>
        /// <param name="color">color</param>
        internal void SetColor(Color color)
        {
            this.color = color;
        }
    }
}
