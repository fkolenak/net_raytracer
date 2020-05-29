using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    /// <summary>
    /// Represents scene - wraps all objects
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Scene camera
        /// </summary>
        public Camera camera;
        /// <summary>
        /// Max depth
        /// </summary>
        public int maxDepth = 6;
        /// <summary>
        /// Minimal intensity of ray
        /// </summary>
        public float minIntensity = 0.001f;
        /// <summary>
        /// How many time was intersection calculated
        /// </summary>
        public int intersectionCalculationCount;
        /// <summary>
        /// Background color
        /// </summary>
        public Color backgroundColor = new Color(0,0,0.25f);
        /// <summary>
        /// All objects of the scene
        /// </summary>
        public List<AObject> allObjects;
        /// <summary>
        /// Light
        /// </summary>
        public Light light;
        /// <summary>
        /// Minimal and maximal distance (not used)
        /// </summary>
        public int minDistance = 1, maxDistance = 100;
        /// <summary>
        /// Window dimensions
        /// </summary>
        public int width = 1280, height = 720;
        /// <summary>
        /// Constructor
        /// </summary>
        public Scene()
        {
            intersectionCalculationCount = 0;
            allObjects = new List<AObject>();
        }
        /// <summary>
        /// Set light of the scene
        /// </summary>
        /// <param name="light">light</param>
        public void SetLight(Light light)
        {
            this.light = light;
        }

        /// <summary>
        /// Set camera
        /// </summary>
        /// <param name="camera">camera</param>
        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        /// <summary>
        /// Set background color
        /// </summary>
        /// <param name="r">red</param>
        /// <param name="g">green</param>
        /// <param name="b">blue</param>
        public void SetBackgroundColor(float r, float g, float b)
        {
            this.backgroundColor = new Color(r,g,b);
        }
        /// <summary>
        /// Set min and max distance
        /// </summary>
        /// <param name="min">min distance</param>
        /// <param name="max">max distance</param>
        public void SetMinMaxDistance(int min, int max)
        {
            this.minDistance = min;
            this.maxDistance = max;
        }

        /// <summary>
        /// Set max depth
        /// </summary>
        /// <param name="maxDepth">Max depth.</param>
        public void SetMaxDepth(int maxDepth)
        {
            this.maxDepth = maxDepth;
        }
        /// <summary>
        /// Set backgroundColor
        /// </summary>
        /// <param name="c">color</param>
        public void SetBackgroundColor(Color c)
        {
            this.backgroundColor = c;
        }
        /// <summary>
        /// Set min intensity of ray
        /// </summary>
        /// <param name="minIntensity">Min intensity.</param>
        internal void SetMinIntensity(float minIntensity)
        {
            this.minIntensity = minIntensity;
        }
    }
}
