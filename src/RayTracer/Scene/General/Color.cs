namespace RayTracer
{
    public struct Color
    {
        /// <summary>parts of color</summary>
        public float r, g, b;
        /// <summary>alpha / Transparency</summary>
        public float a;
        /// <summary>
        /// ID of color
        /// </summary>
        public string ID;


        /// <summary>
        /// Color RGBA
        /// </summary>
        /// <param name="r">Red.</param>
        /// <param name="g">Green.</param>
        /// <param name="b">Blue.</param>
        /// <param name="a">Transparency.</param>
        /// <param name="ID">ID of color.</param>
        public Color(float r, float g, float b, float a = 1, string ID = "Default")
        {
            this.ID = ID;
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        /// <summary>
        /// Operation +
        /// </summary>
        /// <param name="c1">color 1</param>
        /// <param name="c2">color 2</param>
        /// <returns></returns>
        public static Color operator +(Color c1, Color c2)
        {
            return new Color(c1.r + c2.r, c1.g + c2.g, c1.b + c2.b);
        }
        /// <summary>
        /// Operation -
        /// </summary>
        /// <param name="c1">color 1</param>
        /// <param name="c2">color 2</param>
        /// <returns></returns>
        public static Color operator -(Color c1, Color c2)
        {
            return new Color(c1.r - c2.r, c1.g - c2.g, c1.b - c2.b);
        }
        /// <summary>
        /// Operation *
        /// </summary>
        /// <param name="number">number value</param>
        /// <param name="c">color</param>
        /// <returns></returns>
        public static Color operator *(float number, Color c)
        {
            return new Color(number * c.r, number * c.g, number * c.b);
        }

    }
}
