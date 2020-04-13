using System;

namespace RayTracer
{
    /// <summary>
    /// Represents intersection with a body
    /// </summary>
    class Intersection
    {
        /// <summary>
        /// Parameter for intersecting beam (where is intersection)
        /// </summary>
        public float t, t2;
        /// <summary>
        /// Normal at the point of intersection
        /// </summary>
        public Vector normal, normalEnd;
        /// <summary>
        /// Indicates whether it is in the object
        /// </summary>
        public Boolean isIn = false;
        /// <summary>
        /// The objects index of the crossed object
        /// </summary>
        public int indexOfCrossedObj;
        /// <summary>
        /// Beam that created intersection
        /// </summary>
        public Ray ray;
        /// <summary>
        /// Color at the point of intersection
        /// </summary>
        public Color color;
        /// <summary>
        /// Refractive index
        /// </summary>
        public float n1, n2;
        /// <summary>
        /// absorption (diffusion)
        /// </summary>
        public float kd;
        /// <summary>
        /// Point of intersection
        /// </summary>
        public Point pointOfIntersection, pointOfIntersection2;
        
        /// <summary>
        /// odraz
        /// </summary>
        public float ks;
        /// <summary>
        /// reflection
        /// </summary>
        public float kt;
        /// <summary>
        /// Default constructor
        /// </summary>
        public Intersection() { }
    }
}
