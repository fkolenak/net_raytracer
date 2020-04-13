using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Object that represents camera object
    /// </summary>
    class Camera
    {
        /// <summary>
        /// Field of view
        /// </summary>
        public double fovy;

        /// <summary>
        /// Location of camera
        /// </summary>
        public Point location;

        /// <summary>
        /// Directional vector of camera
        /// </summary>
        public Vector direction, up, right;

        /// <summary>
        /// ID of camera
        /// </summary>
        public int ID;

        /// <summary>
        /// Constructor of camera
        /// </summary>
        /// <param name="ID">ID of camera</param>
        /// <param name="location">Location of camera</param>
        /// <param name="direction">Direction vector</param>
        /// <param name="up">Vector that indicates up direction</param>
        /// <param name="fovy">Field of view</param>
        public Camera(int ID, Point location, Vector direction, Vector up, double fovy)
        {
            this.ID = ID;
            this.fovy = fovy;
            this.location = location;
            this.direction = direction;
            this.up = up;
            
            this.right = -1*Vector.CrossProduct(up,direction);
        }

    }
}
