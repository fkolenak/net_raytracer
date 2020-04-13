using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Abstract object
    /// </summary>
    abstract class AObject
    {
        /// <summary>
        /// Color
        /// </summary>
        public Color color;
        /// <summary>
        /// Absorption (diffusion)
        /// </summary>
        public float kd = 1;
        /// <summary>
        /// Reflection
        /// </summary>
        public float ks = 0;
        /// <summary>
        /// Transparency
        /// </summary>
        public float kt = 0;
        /// <summary>
        /// transparency-refractive index
        /// </summary>
        public float ktrefraction = 1;
        /// <summary>
        /// ID objektu
        /// </summary>
        public int ID;
        /// <summary>
        /// Find intersection
        /// </summary>
        public abstract Intersection GetIntersection(Ray ray);
        /// <summary>
        /// Set Color
        /// </summary>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public abstract void SetColor(float r, float g, float b);
        /// <summary>
        /// Get object color
        /// </summary>
        /// <param name="b"></param>
        public abstract Color GetColor(Point b);

        /// <summary>
        /// Set refraction
        /// </summary>
        /// <param name="refraction">refraction</param>
        public abstract void SetRefraction(float refraction);
        /// <summary>
        /// Translation of the object
        /// </summary>
        /// <param name="x">Move by x</param>
        /// <param name="y">Move by y</param>
        /// <param name="z">Move by z</param>
        public abstract void Translate(float x, float y, float z);
        /// <summary>
        /// Set properties of the objects
        /// </summary>
        /// <param name="kD"></param>
        /// <param name="kS"></param>
        /// <param name="kT"></param>
        public abstract void SetProperties(float kD, float kS, float kT);
    }
}
