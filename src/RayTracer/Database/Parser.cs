using System;
using System.Collections.Generic;
using System.Globalization;
using RayTracer.Database;
using System.Data;

namespace RayTracer
{
    /// <summary>
    /// Parser
    /// </summary>
    class Parser
    {
        private static NumberFormatInfo f = CultureInfo.InvariantCulture.NumberFormat;

        public Parser() { }
        /// <summary>
        /// Load scene from given dataset
        /// </summary>
        /// <param name="dataSet">dataset</param>
        /// <returns></returns>
        public List<Scene> LoadScene(DataSet dataSet)
        {
            SceneDatabase sceneDatabase = new SceneDatabase();
            sceneDatabase.Merge(dataSet);

            List<Scene> sceneList = new List<Scene>();
            DataRow[] rows = sceneDatabase._Scene.Select();

            for (int i = 0; i < rows.Length; i++)
            {
                Scene scene = LoadScene(rows[i]);

                DataView cameraView = new DataView(sceneDatabase.Camera);
                DataView LightView = new DataView(sceneDatabase.Light);

                Camera camera = ParseCamera(cameraView, scene.ID);
                Light light = ParseLight(LightView, scene.ID);

                scene.SetCamera(camera);
                scene.SetLight(light);

                LoadObjects(sceneDatabase, scene);
                sceneList.Add(scene);
            }



            return sceneList;
        }
        /// <summary>
        /// Loads scene object from given dataset row
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private Scene LoadScene(DataRow dataRow)
        {
            Scene scene = new Scene();
            scene.ID = (int)dataRow["id"];
            scene.maxDepth = (int)dataRow["maxDepth"];
            scene.SetMinIntensity(float.Parse(dataRow["minIntensity"].ToString()));
            scene.SetBackgroundColor(MakeColor(dataRow["backgroundColor"].ToString()));
            return scene;
        }
        /// <summary>
        /// Loads objects from scene database
        /// </summary>
        /// <param name="sceneDatabase">scene database</param>
        /// <param name="scene">scene</param>
        private void LoadObjects(SceneDatabase sceneDatabase, Scene scene)
        {
            DataView blockView = new DataView(sceneDatabase.Block);
            DataView floorView = new DataView(sceneDatabase.Floor);
            DataView sphereView = new DataView(sceneDatabase.Sphere);

            List<Block> blocks = ParseBlocks(blockView, scene.ID);
            Floor floor = ParseFloor(floorView, scene.ID);
            List<Sphere> spheres = ParseSpheres(sphereView, scene.ID);


            foreach (Block block in blocks)
            {
                scene.allObjects.Add(block);
            }
            scene.allObjects.Add(floor);
            foreach (Sphere sphere in spheres)
            {
                scene.allObjects.Add(sphere);
            }
        }
        /// <summary>
        /// Parses scenes from sphere table view
        /// </summary>
        /// <param name="sphereView">sphere table view</param>
        /// <param name="sceneID">scene ID</param>
        /// <returns></returns>
        private List<Sphere> ParseSpheres(DataView sphereView, int sceneID)
        {
            int sphereID;
            Point location;
            float diffusion, reflection, transparency, diameter, refraction;
            Color color;
            List<Sphere> spheres = new List<Sphere>();
            for (int i = 0; i < sphereView.Count; i++)
            {
                if (sceneID == Convert.ToInt32(sphereView[i]["sceneID"]))
                {
                    sphereID = Convert.ToInt32(sphereView[i]["id"]);
                    location = MakePoint((string)sphereView[i]["location"]);
                    color = MakeColor((string)sphereView[i]["color"]);
                    diffusion = float.Parse(sphereView[i]["diffusion"].ToString());
                    reflection = float.Parse(sphereView[i]["reflection"].ToString());
                    transparency = float.Parse(sphereView[i]["transparency"].ToString());
                    diameter = float.Parse(sphereView[i]["diameter"].ToString());
                    refraction = float.Parse(sphereView[i]["refraction"].ToString());
                    
                    Sphere sphere = new Sphere(sphereID, location, diameter);
                    sphere.SetColor(color);
                    sphere.SetProperties(diffusion, reflection, transparency);
                    sphere.SetRefraction(refraction);
                    spheres.Add(sphere);
                }
            }
            return spheres;
        }
        /// <summary>
        /// Parses floor object from floor table view
        /// </summary>
        /// <param name="floorView">floor table view</param>
        /// <param name="sceneID">scene ID</param>
        /// <returns></returns>
        private Floor ParseFloor(DataView floorView, int sceneID)
        {
            int floorID;
            Point locationA, locationB;
            float diffusion, reflection, transparency;
            Color color1, color2;
            for (int i = 0; i < floorView.Count; i++)
            {
                if (sceneID == Convert.ToInt32(floorView[i]["sceneID"]))
                {
                    floorID = Convert.ToInt32(floorView[i]["id"]);
                    locationA = MakePoint((string)floorView[i]["locationA"]);
                    locationB = MakePoint((string)floorView[i]["locationB"]);
                    color1 = MakeColor((string)floorView[i]["color1"]);
                    color2 = MakeColor((string)floorView[i]["color2"]);
                    diffusion = float.Parse(floorView[i]["diffusion"].ToString());
                    reflection = float.Parse(floorView[i]["reflection"].ToString());
                    transparency = float.Parse(floorView[i]["transparency"].ToString());
                    Floor floor = new Floor(floorID, locationA, locationB);
                    floor.SetColors(color1, color2);
                    floor.SetProperties(diffusion, reflection, transparency);
                    return floor;
                }
            }
            return null;
        }
        /// <summary>
        /// Parses block objects from block table view
        /// </summary>
        /// <param name="blockView">block table view</param>
        /// <param name="sceneID">scene ID</param>
        /// <returns></returns>
        private List<Block> ParseBlocks(DataView blockView, int sceneID)
        {
            int blockID;
            Point locationA, locationB;
            float diffusion, reflection, transparency;
            Color color;
            List<Block> blocks = new List<Block>();
            for (int i = 0; i < blockView.Count; i++)
            {
                if (sceneID == Convert.ToInt32(blockView[i]["sceneID"]))
                {
                    blockID = Convert.ToInt32(blockView[i]["id"]);
                    locationA = MakePoint((string)blockView[i]["locationA"]);
                    locationB = MakePoint((string)blockView[i]["locationB"]);
                    color = MakeColor((string)blockView[i]["color"]);
                    diffusion = float.Parse(blockView[i]["diffusion"].ToString());
                    reflection = float.Parse(blockView[i]["reflection"].ToString());
                    transparency = float.Parse(blockView[i]["transparency"].ToString());
                    Block block = new Block(blockID, locationA, locationB);
                    block.SetColor(color);
                    block.SetProperties(diffusion,reflection, transparency);
                    blocks.Add(block);
                }
            }
            return blocks;
        }
        /// <summary>
        /// Parses light object from light table view
        /// </summary>
        /// <param name="lightView">light table view</param>
        /// <param name="sceneID">scene ID</param>
        /// <returns></returns>
        private Light ParseLight(DataView lightView, int sceneID)
        {
            int lightID;
            Point location;
            float intensity;
            Color color;

            for (int i = 0; i < lightView.Count; i++)
            {
                if (sceneID == Convert.ToInt32(lightView[i]["sceneID"]))
                {
                    lightID = Convert.ToInt32(lightView[i]["id"]);
                    location = MakePoint((string)lightView[i]["location"]);
                    color = MakeColor((string)lightView[i]["color"]);
                    intensity = float.Parse(lightView[i]["intensity"].ToString());
                    return new Light(lightID, location, color, intensity);
                }
            }
            return null;
        }
        /// <summary>
        /// Parses camera object from camera table view 
        /// </summary>
        /// <param name="cameraView">camera table view</param>
        /// <param name="sceneID">scene ID</param>
        /// <returns></returns>
        private Camera ParseCamera(DataView cameraView, int sceneID)
        {
            int cameraID;
            Point location;
            Vector direction, up;
            double fovy;

            for (int i = 0; i < cameraView.Count; i++)
            {
                if (sceneID == Convert.ToInt32(cameraView[i]["sceneID"])) {
                    cameraID = Convert.ToInt32(cameraView[i]["id"]);
                    location = MakePoint((string)cameraView[i]["location"]);
                    direction = MakeVector((string)cameraView[i]["direction"]);
                    up = MakeVector((string)cameraView[i]["up"]);
                    fovy = float.Parse(cameraView[i]["fovy"].ToString());
                    return new Camera(cameraID, location, direction, up, fovy);
                }
            }
            return null;
        }

        /// <summary>
        /// Parses coordinations from raw string (delimiter is semicolon)
        /// </summary>
        /// <param name="coordinations">raw coordinations string</param>
        /// <returns></returns>
        private string[] ParseCoordinations(string coordinations)
        {
            return coordinations.Split(';');
        }
        /// <summary>
        /// Makes point object from raw string
        /// </summary>
        /// <param name="rawPoint">raw string with coordinations</param>
        /// <returns></returns>
        private Point MakePoint(string rawPoint)
        {
            string[] parsedCoordinations = ParseCoordinations(rawPoint);
            return new Point(float.Parse(parsedCoordinations[0], f), float.Parse(parsedCoordinations[1], f), float.Parse(parsedCoordinations[2], f));
        }
        /// <summary>
        /// Makes vector object from raw string
        /// </summary>
        /// <param name="rawVector">raw string with coordinations</param>
        /// <returns></returns>
        private Vector MakeVector(string rawVector)
        {
            string[] parsedCoordinations = ParseCoordinations(rawVector);
            return new Vector(float.Parse(parsedCoordinations[0], f), float.Parse(parsedCoordinations[1], f), float.Parse(parsedCoordinations[2], f));
        }
        /// <summary>
        /// Makes color object from raw string
        /// </summary>
        /// <param name="rawColor">raw string with values</param>
        /// <returns></returns>
        private Color MakeColor(string rawColor)
        {
            string[] parsedCoordinations = ParseCoordinations(rawColor);
            return new Color(float.Parse(parsedCoordinations[0], f), float.Parse(parsedCoordinations[1], f), float.Parse(parsedCoordinations[2], f));

        }
    }
}
