using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Represents one single ray
    /// </summary>
    public class Ray
    {
        /// <summary>
        /// Ray direction
        /// </summary>
        public Vector direction;
        /// <summary>
        /// Ray start position
        /// </summary>
        public Point startPoint;
        /// <summary>
        /// Is in object?
        /// </summary>
        public Boolean isIn = false;
        /// <summary>
        /// Ray number(generation)
        /// </summary>
        public int generation;
        /// <summary>
        /// Ray intensity (in what proportion the color should be added)
        /// </summary>
        public float rayIntensity = 1;
        /// <summary>
        /// Refraction index
        /// </summary>
        public float refractionIndex = 1;
        /// <summary>
        /// Intersection that the ray created
        /// </summary>
        public Intersection p;
        /// <summary>
        /// Construktor
        /// </summary>
        /// <param name="startPoint">Start of ray</param>
        /// <param name="direction">Ray direction</param>
        public Ray(Point startPoint, Vector direction)
        {
            this.startPoint = startPoint;
            this.direction = direction;
        }

        /// <summary>
        /// Construktor
        /// </summary>
        /// <param name="generation">Ray generation</param>
        /// <param name="startPoint">Start of ray</param>
        /// <param name="direction">Ray direction</param>
        public Ray(int generation, Point startPoint, Vector direction)
        {
            this.generation = generation;
            this.startPoint = startPoint;
            this.direction = direction;
        }
    }
}
