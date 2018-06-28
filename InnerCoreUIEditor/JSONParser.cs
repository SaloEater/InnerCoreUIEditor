using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnerCoreUIEditor
{
    static class JSONParser
    {
        public static void Parse(string gui)
        {
            //int standartPosition = FindEntrance(gui, "standart:");
            //ParseStandart(gui);
            string elements = GetBigField(gui, "elements");
            ParseElements(elements);
        }

        private static string GetBigField(string gui, string v)
        {
            string answer = "";

            for(int i = 0; i < gui.Length; i++)
            {
                int j;
                for(j = 0; j < v.Length; j++)
                {
                    if (gui[i+j] != v[j]) break;
                }
                if(j==v.Length)
                {
                    if (gui[i+j++] != ':') break;
                    int counter = 0;
                    for(int k = i + j; k < gui.Length; k++)
                    {
                        if (gui[k] == '{') counter++;
                        if (gui[k] == '}') counter--;
                        answer += gui[k];
                        if (counter == 0 && gui[k] != ' ' || counter == 0 && gui[k] == ',') break;
                    }
                    break;
                }
            }
            //answer = Clear(answer);
            return answer;
        }

        private static string Clear(string answer)
        {
            answer = answer.Replace("\n", "");
            answer = answer.Replace("\t", "");
            answer = answer.Replace(" ", "");
            return answer;
        }

        private static void ParseElements(string elements)
        {
            string element = "";
            string name = "";
            int counter = 0;
            int elementRead = 0;
            for (int k = 0; k < elements.Length; k++)
            {
                if (elements[k] == '{')
                {
                    if (counter == 1) elementRead = 1;
                    counter++;
                }
                if (elementRead == 0 && counter == 1)
                {
                    name += elements[k];
                }
                if (elementRead == 1) element += elements[k];
                if (elements[k] == '}')
                {
                    if (counter == 2) elementRead = 2;
                    counter--;
                }
                if (elementRead == 2)
                {
                    name = Clear(name);
                    VisualizeElement(name.Replace(" ", ""), element);
                    element = "";
                    name = "";
                    elementRead = 0;
                }
            }
        }

        private static void VisualizeElement(string name, string element)
        {
            string type = GetClearField(element, "type");
            switch(type)
            {
                case "slot":
                    Slot slot = new Slot();
                    int x;
                    if (!int.TryParse(GetClearField(element, "x").Split('.')[0], out x)) x = Global.X / 2;
                    int y;
                    if (!int.TryParse(GetClearField(element, "y").Split('.')[0], out y)) y = Global.Y / 2;
                    int size;
                    if (!int.TryParse(GetClearField(element, "size").Split('.')[0], out size)) size = slot.Width;
                    bool visual;
                    if (!bool.TryParse(GetClearField(element, "visual"), out visual)) visual = false;
                    string ImageName = GetClearField(element, "bitmap");
                    if (ImageName == "") ImageName = slot.ImageName;
                    string Clicker = GetField(element, "clicker");
                    slot.Apply(name, x, y, size, visual, ImageName, Clicker);
                    Global.panelWorkspace.Controls.Add(slot);
                    break;

                default:

                    break;
            }
        }

        private static string GetClearField(string element, string v)
        {
            return GetField(element, v).Replace("\"", "").Replace(" ", "");
        }

        private static string GetField(string element, string v)
        {
            string answer = "";

            for (int i = 0; i < element.Length; i++)
            {
                int j;
                for (j = 0; j < v.Length; j++)
                {
                    if (element[i + j] != v[j]) break;
                }
                if (j == v.Length)
                {
                    if (element[i + j++] != ':') continue;
                    int counter = 1;
                    for (int k = i + j; k < element.Length; k++)
                    {
                        if (element[k] == ':') counter++;
                        if (element[k] == ',' || element[k] == '}') counter--;
                        if (counter == 0) break;
                        else answer += element[k];
                    }
                    break;
                }
            }
            return answer;
        }

        internal static void Save(string filename)
        {
            string elements = "elements: \n{";

            foreach(InnerControl c in Global.panelWorkspace.Controls)
            {
                elements += c.MakeOutput();
            }

            elements += "\n}";
            File.WriteAllText(filename, elements);
        }
    }
}
