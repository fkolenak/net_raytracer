using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using static RayTracer.Database.SceneDatabase;

namespace RayTracer.Database
{
    class DatabaseHandler
    {
        private static string FILENAME = "database.xml";
        public SceneDatabase Load(string path)
        {
            DataSet sceneDatabase = new DataSet();
            DataTableHelper.ReadXmlIntoDataSet(sceneDatabase, path + FILENAME);
            return (SceneDatabase) sceneDatabase;
        }

        public void Save(DataSet sceneDatabase, string path)
        {
            DataTableHelper.WriteDataSetToXML(sceneDatabase, path + FILENAME);
        }

        public static void CreateTestProject(SceneDatabase sceneDatabase)
        {
            int sceneID = InsertScene(sceneDatabase);
            sceneDatabase.Floor.Rows.Add(new Object[] { null, sceneID, "-10;0;-10", "10;0;10", 0.6, 0.4, 0, "1;1;1", "0;0;0" });
            sceneDatabase.Sphere.Rows.Add(new Object[] { null, sceneID, "0;2;0", 2, 0.375,0.125,0.5, "0.25;0.5;1" });
            sceneDatabase.Block.Rows.Add(new Object[] { null, sceneID, "-5;0.75;-5", "5;3.75;-6", 1, 0, 0, "1;0.5;0.25" });
            //sceneDatabase.Camera.Rows.Add(new Object[] { null, sceneID, "0;15;0", "0;-1;0", "0;0;1", 60 }); // Top down look

            sceneDatabase.Camera.Rows.Add(new Object[] { null, sceneID, "5.3368;8.0531;9.8769", "-0.38363;-0.42482;-0.82", "-0.16485;0.90515;-0.391826", 60});
            sceneDatabase.Light.Rows.Add(new Object[] { null, sceneID, "-15;10;20", 1, "0;0;0" });


            sceneDatabase.AcceptChanges();
        }

        public static int InsertScene(SceneDatabase sceneDatabase)
        {
            SceneRow row = sceneDatabase._Scene.NewSceneRow();
            sceneDatabase._Scene.Rows.Add(row);
            sceneDatabase._Scene.AcceptChanges();

            return row.id;
        }
    }



    // Use WriteXml method to export the dataset.  
    static class DataTableHelper
    {
        public static void WriteDataSetToXML(DataSet dataset, String xmlFileName)
        {
            using (FileStream fsWriterStream = new FileStream(xmlFileName, FileMode.Create))
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(fsWriterStream, Encoding.Unicode))
                {
                    dataset.WriteXml(xmlWriter, XmlWriteMode.WriteSchema);
                    Console.WriteLine("Write {0} to the File {1}.", dataset.DataSetName, xmlFileName);
                    Console.WriteLine();
                }
            }
        }

        // Use GetXml method to get the XML data of the dataset and then export to the file.  
        public static void GetXMLFromDataSet(DataSet dataset, String xmlFileName)
        {
            using (StreamWriter writer = new StreamWriter(xmlFileName))
            {
                writer.WriteLine(dataset.GetXml());
                Console.WriteLine("Get Xml data from {0} and write to the File {1}.", dataset.DataSetName, xmlFileName);
                Console.WriteLine();
            }
        }

        // Use ReadXml method to import the dataset from the dataset.  
        public static void ReadXmlIntoDataSet(DataSet newDataSet, String xmlFileName)
        {
            using (FileStream fsReaderStream = new FileStream(xmlFileName, FileMode.Open))
            {
                using (XmlTextReader xmlReader = new XmlTextReader(fsReaderStream))
                {
                    newDataSet.ReadXml(xmlReader, XmlReadMode.ReadSchema);
                }
            }
        }

        // Display the columns and value of DataSet.  
        public static void ShowDataSet(DataSet dataset)
        {
            foreach (DataTable table in dataset.Tables)
            {
                Console.WriteLine("Table {0}:", table.TableName);
                ShowDataTable(table);
            }
        }

        // Display the columns and value of DataTable.  
        private static void ShowDataTable(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
            {
                Console.Write("{0,-14}", col.ColumnName);
            }
            Console.WriteLine("{0,-14}", "");

            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        if (col.DataType.Equals(typeof(DateTime)))
                        {
                            Console.Write("{0,-14:d}", row[col, DataRowVersion.Original]);
                        }
                        else if (col.DataType.Equals(typeof(Decimal)))
                        {
                            Console.Write("{0,-14:C}", row[col, DataRowVersion.Original]);
                        }
                        else
                        {
                            Console.Write("{0,-14}", row[col, DataRowVersion.Original]);
                        }
                    }
                }
                else
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        if (col.DataType.Equals(typeof(DateTime)))
                        {
                            Console.Write("{0,-14:d}", row[col]);
                        }
                        else if (col.DataType.Equals(typeof(Decimal)))
                        {
                            Console.Write("{0,-14:C}", row[col]);
                        }
                        else
                        {
                            Console.Write("{0,-14}", row[col]);
                        }
                    }
                }
                Console.WriteLine("{0,-14}", "");
            }
        }

        // Display the columns of DataSet.  
        public static void ShowDataSetSchema(DataSet dataSet)
        {
            Console.WriteLine("{0} contains the following tables:", dataSet.DataSetName);
            foreach (DataTable table in dataSet.Tables)
            {
                Console.WriteLine("   Table {0} contains the following columns:", table.TableName);
                ShowDataTableSchema(table);
            }
        }

        // Display the columns of DataTable  
        private static void ShowDataTableSchema(DataTable table)
        {
            String columnString = "";
            foreach (DataColumn col in table.Columns)
            {
                columnString += col.ColumnName + "   ";
            }
            Console.WriteLine(columnString);
        }
    }
}
