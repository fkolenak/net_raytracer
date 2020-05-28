using System;

namespace RayTracer
{
    /// <summary>
    /// Vector class
    /// </summary>
    public struct Vector
    {
        //Vector atributes
        public float x, y, z, w;

        /// <summary>
        /// Create a vector object
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        public Vector(float x , float y, float z, float w = 1)
        {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
        }

        /// <summary>
        /// Create a new vector based on the given vector (copies it)
        /// </summary>
        /// <param name="v">Vector to copy</param>
        public Vector(Vector v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = v.w;
        }

        /// <summary>
        /// Cross product of vectors
        /// </summary>
        /// <param name="c1">First vektor</param>
        /// <param name="c2">Second vektor</param>
        /// <returns>Resulting vector </returns>
        public static Vector CrossProduct(Vector c1,Vector c2)
        {
            return new Vector(c1.y * c2.z - c1.z * c2.y, c1.z * c2.x - c1.x * c2.z, c1.x * c2.y - c1.y * c2.x);
        }
        /// <summary>
        /// Dot product of two vectors (Skalarni nasobeni)
        /// </summary>
        /// <param name="c1">First vektor</param>
        /// <param name="c2">Second vektor</param>
        /// <returns>Resulting vector</returns>
        public static float DotProduct(Vector c1, Vector c2)
        {
            return (float)(c1.x * c2.x + c1.y * c2.y + c1.z * c2.z);
        }

        /// <summary>
        /// Normalize vector
        /// </summary>
        public void Normalize()
        {
            float delka = Magnitude();
            if (delka != 0)
            {
                this.x = (float)(this.x / delka);
                this.y = (float)(this.y / delka);
                this.z = (float)(this.z / delka);
            }
        }

        /// <summary>
        /// Length of vector (Magnitude)
        /// </summary>
        /// <returns></returns>
        public float Magnitude()
        {
            return (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
        }



        /// <summary>
        /// Overload to add vectors
        /// </summary>
        /// <param name="c1">First vector</param>
        /// <param name="c2">Second vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector operator +(Vector c1, Vector c2)
        {
            return new Vector(c1.x + c2.x, c1.y + c2.y, c1.z + c2.z);
        }

        /// <summary>
        /// Overload of -
        /// </summary>
        /// <param name="c1">Point</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting point</returns>
        public static Point operator +(Point c1, Vector c2)
        {
            return new Point(c1.X + c2.x, c1.Y + c2.y, c1.Z + c2.z);
        }

        /// <summary>
        /// Overload of -
        /// </summary>
        /// <param name="c1">Vector</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector operator -(Vector c1, Vector c2)
        {
            return new Vector(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
        }

        /// <summary>
        /// Overload of -
        /// </summary>
        /// <param name="c1">Vector</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting point</returns>
        public static Point operator -(Point c1, Vector c2)
        {
            return new Point(c1.X - c2.x, c1.Y - c2.y, c1.Z - c2.z);
        }


        /// <summary>
        /// Overload of *
        /// </summary>
        /// <param name="c1">Cislo, kterym nasobime</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector operator *(float c1, Vector c2)
        {
            
            return new Vector(c1 * c2.x, c1 * c2.y, c1 * c2.z);
        }

        /// <summary>
        /// Overload of *
        /// </summary>
        /// <param name="c1">Vector</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector operator *(Vector c1, Vector c2)
        {
            return new Vector(c1.x * c2.x, c1.y * c2.y, c1.z * c2.z);
        }

        /// <summary>
        /// Overload of *
        /// </summary>
        /// <param name="c1">Cislo, kterym nasobime</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector operator *(int c1, Vector c2)
        {
            
            return new Vector(c1 * c2.x, c1 * c2.y, c1 * c2.z);
        }

        /// <summary>
        /// Overload of *
        /// </summary>
        /// <param name="c1">Cislo, kterym nasobime</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector operator *(double c1, Vector c2)
        {


            return new Vector((float)c1 * c2.x, (float)c1 * c2.y, (float)c1 * c2.z);
        }
        /// <summary>
        /// Overload of *
        /// </summary>
        /// <param name="c1">Bod, kterym nasobime</param>
        /// <param name="c2">Vector</param>
        /// <returns>Resulting vector</returns>
        public static Vector operator *(Point c1, Vector c2)
        {
            return new Vector(c1.X * c2.x, c1.Y * c2.y, c1.Z * c2.z);
        }

        /// <summary>
        /// To string
        /// </summary>
        override public string ToString()
        {
            return ("{" + this.x + "\t," + this.y + "\t," + this.z+ "}");
        }
    }
}
