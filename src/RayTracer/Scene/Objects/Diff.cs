using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// The class represents a body that is created by the difference of two bodies.
    /// </summary>
    class Diff : AObject
    {
        /// <summary>
        /// Body "Basis"
        /// </summary>
        AObject a;
        /// <summary>
        /// Body, which we remove
        /// </summary>
        AObject b;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of created object</param>
        /// <param name="a">Basis</param>
        /// <param name="b">Body which we remove</param>
        public Diff(int ID, AObject a, AObject b)
        {
            this.ID = ID;
            this.a = a;
            this.b = b;
        }

        /// <summary>
        /// Get color
        /// </summary>
        /// <param name="b">Point</param>
        /// <returns>Color of body</returns>
        public override Color GetColor(Point b)
        {
            return this.color;
        }

        /// <summary>
        /// Set color of body
        /// </summary>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public override void SetColor(float r, float g, float b)
        {
            this.color = new Color(r, g, b);
            this.a.SetColor(r, g, b);
        }

        /// <summary>
        /// Set properties of body
        /// </summary>
        /// <param name="kD">difuze</param>
        /// <param name="kS">odraz</param>
        /// <param name="kT">pruhlednost</param>
        public override void SetProperties(float kD, float kS, float kT)
        {
            this.kd = kD;
            this.ks = kS;
            this.kt = kT;
            this.a.SetProperties(kD, kS, kT);
            //this.b.SetProperties(kD, kS, kT);
        }
        /// <summary>
        /// Translation of body
        /// </summary>
        /// <param name="x">move by X</param>
        /// <param name="y">move by Y</param>
        /// <param name="z">move by Z</param>
        public override void Translate(float x, float y, float z)
        {
            a.Translate(x, y, z);
            b.Translate(x, y, z);
        }
        /// <summary>
        /// Get intersection
        /// </summary>
        /// <param name="ray">Ray that is intersecting</param>
        /// <returns>Intersection</returns>
        public override Intersection GetIntersection(Ray ray)
        {
            Intersection p1 = a.GetIntersection(ray);
            Intersection p2 = b.GetIntersection(ray);
            if (p1 != null && p2 != null)
            {
                if (p1.t < p2.t && p1.t2 < p2.t2) // Objects overlap
                {
                    p1.t2 = p2.t;
                    return p1;
                }
                else if (p2.t2 > p1.t2 && p2.t2 > p1.t && p2.t < p1.t && p2.t < p1.t2) return null; //Object inside
                else if (p1.t < p2.t2 && p1.t > p2.t && p1.t2 > p2.t && p1.t2 > p2.t2) // Objects overpal
                {
                    return null;
                }
                else if (p1.t < p2.t && p1.t2 < p2.t)
                    if (p2.t2 > 0.001) return p1; // Objects outside
                    else return null;
                else if (p1.t > p2.t2 && p1.t2 > p2.t2)
                    return p1; // Objects outside
                else return null;
                
            }
            else if (p2 == null) return p1;
            else return null;
            
        }
        /// <summary>
        /// Set refraction index
        /// </summary>
        /// <param name="refraction">refraction index</param>
        public override void SetRefraction(float refraction)
        {
            this.ktrefraction = refraction;
            this.a.SetRefraction(refraction);
        }
    }
}
