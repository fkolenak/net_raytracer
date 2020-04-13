using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Represents unification of objects
    /// </summary>
    class Unification : AObject
    {
        /// <summary>
        /// Unification objects
        /// </summary>
        AObject a, b;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="a">Object a</param>
        /// <param name="b">Object b</param>
        public Unification(int ID, AObject a, AObject b)
        {
            this.ID = ID;
            this.a = a;
            this.b = b;
        }

        /// <summary>
        /// Get color
        /// </summary>
        /// <param name="b">point of interrest</param>
        /// <returns>color</returns>
        public override Color GetColor(Point b)
        {
            return this.color;
        }
        /// <summary>
        /// Set color to all unified objects
        /// </summary>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public override void SetColor(float r, float g, float b)
        {
            this.color = new Color(r,g,b);
            this.a.SetColor(r,g,b);
            this.b.SetColor(r,g,b);
        }
        /// <summary>
        /// Set properties to all objects
        /// </summary>
        /// <param name="kD">diffusion</param>
        /// <param name="kS">Reflection</param>
        /// <param name="kT">Transparency</param>
        public override void SetProperties(float kD, float kS, float kT)
        {
            this.kd = kD;
            this.ks = kS;
            this.kt = kT;
            this.a.SetProperties(kD, kS, kT);
            this.b.SetProperties(kD, kS, kT);
        }
        /// <summary>
        /// Set refraction index to all objects
        /// </summary>
        /// <param name="refraction">refraction</param>
        public override void SetRefraction(float refraction)
        {
            this.ktrefraction = refraction;
            this.a.SetRefraction(refraction);
            this.b.SetRefraction(refraction);
            
        }
        /// <summary>
        /// Translation
        /// </summary>
        /// <param name="x">Move by X</param>
        /// <param name="y">Move by Y</param>
        /// <param name="z">Move by Z</param>
        public override void Translate(float x, float y, float z)
        {
            a.Translate(x,y,z);
            b.Translate(x,y,z);
        }
        /// <summary>
        /// Get intersection
        /// </summary>
        /// <param name="ray">Ray</param>
        /// <returns>Intersection if found, null otherwise</returns>
        public override Intersection GetIntersection(Ray ray)
        {
            Intersection p1 = a.GetIntersection(ray);
            Intersection p2 = b.GetIntersection(ray);
            if (p1 != null && p2 != null)
            {
                if (p1.t > 0.001 && p2.t > 0.001 && p1.t2 > 0.001 && p2.t2 > 0.001)
                {
                    if (p1.t < p2.t && p1.t2 > p2.t2) return p1;// p2 in
                    else if (p2.t < p1.t && p2.t2 > p1.t2) return p2;// p1 in
                    else if (p1.t < p2.t && p1.t2 < p2.t2 && p1.t < p2.t2)           // overlay p1 sooner
                    {
                        p1.t2 = p2.t2;
                        p1.normalEnd = p2.normalEnd;
                        p1.pointOfIntersection2 = p2.pointOfIntersection2;
                        return p1;
                    }
                    else if (p2.t < p1.t && p2.t2 < p1.t2 && p2.t < p1.t2)           // overlay p2 sooner
                    {
                        p2.t2 = p1.t2;
                        p2.normalEnd = p1.normalEnd;
                        p2.pointOfIntersection2 = p1.pointOfIntersection2;
                        return p2;
                    }
                    else if (p1.t < p2.t && p1.t2 < p2.t) return p1; //p1 sooner
                    else if (p2.t < p1.t && p2.t2 < p1.t) return p2; //p2 sooner
                }
                else
                {
                    if (p2.t2 < 0.001)
                    {
                        if (p2.t < p1.t && p2.t < p1.t2)
                        {
                            p2.isIn = true;
                            return p2;
                        }
                        else if (p2.t > p1.t && p2.t < p1.t2)
                        {

                            p2.t = p1.t2;
                            p2.normal = p1.normalEnd;
                            p2.pointOfIntersection = p1.pointOfIntersection2;
                            p2.isIn = true;
                            return p2;
                        }
                        else if (p2.t > p1.t && p2.t > p1.t2)
                        {
                            p2.isIn = true;
                            return p2;
                        }
                        else if(p1.t2 < 0.001 && p2.t < p1.t)
                        {
                            p2.t = p1.t;
                            p2.normal = p1.normal;
                            p2.pointOfIntersection = p1.pointOfIntersection;
                            p2.isIn = true;
                            return p2;
                        }
                        p2.isIn = true;
                        return p2;
                    }
                    if (p1.t2 < 0.001)
                    {

                        if (p1.t < p2.t && p1.t < p2.t2)
                        {
                            p1.isIn = true;
                            return p1;
                        }
                        else if (p1.t > p2.t && p1.t < p2.t2)
                        {

                            p1.t = p2.t2;
                            p1.normal = p2.normalEnd;
                            p1.pointOfIntersection = p2.pointOfIntersection2;
                            p1.isIn = true;
                            return p1;
                        }
                        else if (p1.t > p2.t && p1.t > p2.t2)
                        {
                            p1.isIn = true;
                            return p1;
                        }
                        else if (p2.t2 < 0.001 && p1.t < p2.t)
                        {
                            p1.t = p2.t;
                            p1.normal = p2.normal;
                            p1.pointOfIntersection = p2.pointOfIntersection;
                            p1.isIn = true;
                            return p1;
                        }
                        p1.isIn = true;

                        return p1;
                    }
                }
            }
            else if (p1 == null && p2 != null)
            {
                if (p2.t2 < 0.001)
                {
                    p2.isIn = true;
                    return p2;
                }
                else return p2;
            }
            else if (p2 == null && p1 != null) 
                if (p1.t2 < 0.001)
                {
                    p1.isIn = true;
                    return p1;
                }
                else return p1;
            else return null;
            return null;
        }

    }
}
