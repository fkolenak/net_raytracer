using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public struct Point
    {
        //Coordinates of the point in space
        private float xval, yval, zval;
        public float w;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Point(float x, float y, float z , float w = 1)
        {
            xval = x;
            yval = y;
            zval = z;
            this.w = w;
        }

       
        
        /// <summary>
        /// Get and Set
        /// </summary>
        public float X
        {
            get
            {
                return xval;
            }
            set
            {
                xval = value;
            }
        }
        /// <summary>
        /// Get and Set
        /// </summary>
        public float Y
        {
            get
            {
                return yval;
            }
            set
            {
                yval = value;
            }
        }
        /// <summary>
        /// Get and Set
        /// </summary>
        public float Z
        {
            get
            {
                return zval;
            }
            set
            {
                zval = value;
            }
        }

        /// <summary>
        /// Override toString.
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            return ("["+this.X + ";\t" + this.Y + ";\t" + this.Z+"]");
        }
    }
}
