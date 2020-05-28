using System;

namespace RayTracer
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            /*Parser parser = new Parser();
            DatabaseHandler databaseHandler = new DatabaseHandler();
            SceneDatabase sceneDatabase = new SceneDatabase();

            DatabaseHandler.CreateTestProject(sceneDatabase);

            //databaseHandler.Save(sceneDatabase, ""); // Removed so the xml can be manipulated

            DataSet data = new DataSet();
            DataTableHelper.ReadXmlIntoDataSet(data, "database.xml");
            //DataTableHelper.ShowDataSet(data);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<Scene> scenes = parser.LoadScene(data);

            foreach (Scene scene in scenes)
            {
                Renderer renderer = new Renderer(scene, scene.allObjects);
                Console.WriteLine("Raytracing started.");
                int time = Environment.TickCount;
                RenderWindow window = renderer.Render();
                time = -time + Environment.TickCount;
                Console.WriteLine("Raytracing finished.");
                window.ShowImage();
                Console.WriteLine("Intersection calculating time: \t" + scene.intersectionCalculationCount + "\nRender time: \t\t" + time + "ms");
                Application.Run(window);
            }*/
        }
    }
}
