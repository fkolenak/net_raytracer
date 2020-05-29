namespace RayTracer
{
    /// <summary>
    /// Represents point white light
    /// </summary>
    public class Light
    {
        //Position of light
        public float xPos, yPos, zPos , intensity;
        // Color of light
        public Color color;
        //ID
        public int ID;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID of light</param>
        /// <param name="xPos">X coordinate</param>
        /// <param name="yPos">Y coordinate</param>
        /// <param name="zPos">Z coordinate</param>
        /// <param name="intensity">Light intensity</param>
        public Light(int ID, float xPos, float yPos, float zPos, float intensity = 1)
        {
            this.ID = ID;
            this.xPos = xPos;
            this.yPos = yPos;
            this.zPos = zPos;
            this.intensity = intensity;
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="location">location</param>
        /// <param name="intensity">intensity</param>
        public Light(int ID, Point location, float intensity = 1) : this(ID, location.X, location.Y, location.Z, intensity)
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="location">location</param>
        /// <param name="color">color</param>
        /// <param name="intensity">intensity</param>
        public Light(int ID, Point location, Color color, float intensity = 1) : this(ID, location.X, location.Y, location.Z, intensity)
        {
            this.color = color;
        }
    }
}
