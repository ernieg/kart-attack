using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace MapEditor
{
    //A typical map object
    public struct MapObject
    {
        public string ObjectName;
        public Point Location;
        public int Size;
        public int XLength;
        public int YLength;
        public Color ObjectColor;
    };

    public class Map
    {
        public Map()
        {
            objects = new List<MapObject>();
        }

        public Map(string file)
        {
            objects = new List<MapObject>();
        }

        public void DrawMap(Graphics g, List<int> currentSelected)
        {
            for(int i = 0; i < objects.Count; i++)
            {
                if (currentSelected.Contains(i))
                {
                    SolidBrush myBrush = new SolidBrush(Color.Tomato);
                    g.FillRectangle(myBrush, new Rectangle(objects[i].Location.X, objects[i].Location.Y, objects[i].XLength * objects[i].Size, objects[i].YLength * objects[i].Size));
                    
                }
                else
                {
                    SolidBrush myBrush = new SolidBrush(objects[i].ObjectColor);
                    g.FillRectangle(myBrush, new Rectangle(objects[i].Location.X, objects[i].Location.Y, objects[i].XLength * objects[i].Size, objects[i].YLength * objects[i].Size));
                }
            }
        }

        public void SaveMap(string saveLocation)
        {
            StreamWriter writer = new StreamWriter(saveLocation);
            foreach (MapObject obj in objects)
            { 
                writer.WriteLine(obj.ObjectName + "|" + obj.ObjectColor.Name + "|" + obj.Location.X + "|" + obj.Location.Y + "|" +
                                 obj.Size + "|" + obj.XLength + "|" + obj.YLength);
            }
            writer.Close();
        }

        public void LoadMap(string file)
        {
            StreamReader reader = File.OpenText(file);
            string input = null;
            objects.Clear();
            while ((input = reader.ReadLine()) != null)
            {
                string[] split = input.Split(new Char[] { '|' });
                MapObject temp = new MapObject();
                temp.ObjectName = split[0];
                temp.ObjectColor = Color.FromName(split[1]);
                temp.Location = new Point((int)Convert.ToUInt32(split[2]), (int)Convert.ToUInt32(split[3]));
                temp.Size = (int)Convert.ToUInt32(split[4]);
                temp.XLength = (int)Convert.ToUInt32(split[5]);
                temp.YLength = (int)Convert.ToUInt32(split[6]);
                objects.Add(temp);
            }
            Console.WriteLine();
            reader.Close();
        }

        public List<MapObject> objects;
    }
}
