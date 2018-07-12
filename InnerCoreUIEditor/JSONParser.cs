﻿using InnerCoreUIEditor.Controls;
using NCalc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    static class JSONParser
    {
        public static void Parse(string gui)
        {
            //int standartPosition = FindEntrance(gui, "standart:");
            //ParseStandart(gui);
            string _params = GetBigField(gui, "params");
            ParseParams(_params);
            string standart = GetBigField(gui, "standart");
            ParseStandart(standart);
            gui = Clear(gui);
            string drawing = GetBigField(gui, "drawing", '[', ']');
            ParseDrawing(drawing);
            string elements = GetBigField(gui, "elements");            
            ParseElements(elements);
            CombineDrawingsWithElements();
        }

        private static void ParseParams(string _params)
        {
            _params = Clear(_params);
            string type = GetField(_params, "slot").Replace("\"", "");
            if (type != "") Params.LoadSlotImage(type);
            type = GetField(_params, "invSlot").Replace("\"", "");
            if (type != "") Params.LoadInvSlotImage(type);
            type = GetField(_params, "selection").Replace("\"", "");
            if (type != "") Params.LoadSelectionImage(type);
            type = GetField(_params, "closeButton").Replace("\"", "");
            if (type != "") Params.LoadCloseButtonImage(type);
            type = GetField(_params, "closeButton2").Replace("\"", "");
            if (type != "") Params.LoadCloseButton2Image(type);
        }

        private static void ParseStandart(string standart)
        {
            standart = Clear(standart);
            string header = GetBigField(standart, "header");
            header = GetField(GetField(header, "text"), "text").Replace("\"", "");
            if (header != "") Global.SetHeaderText(header);
            if (GetField(GetField(header, "text"), "hideButton") == "true") Global.innerHeader.SetButtonVisibilty(true);

            string inventory = GetBigField(standart, "inventory");
            inventory = GetField(inventory, "standart");
            if (inventory == "true") Global.DrawInventorySlots();
            else if(Global.inventoryDrawed)Global.RemoveInventorySlots();

            string background = GetBigField(standart, "background");
            string bg_standart = GetField(background, "standart");
            if (bg_standart != "true")
            {
                string bg_color = GetField(background, "color");
                if (bg_color != "") Global.SetGlobalColor(bg_color);
                string bg_bitmap = GetField(background, "bitmap");
                if (bg_bitmap != "") Global.SetGlobalBackground(bg_bitmap.Replace("\"", ""));
            }

            string minHeight = GetField(standart, "minHeight");
            if (int.TryParse(minHeight, out int height)) Global.ChangeHeight(height);
            else Global.ChangeHeight(Global.defaultHeight);
        }

        private static void CombineDrawingsWithElements()
        {
            //save
            foreach(Control c in Global.panelWorkspace.Controls)
            {
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                if (c.GetType() == typeof(InnerBitmap))
                {
                    InnerBitmap innerBitmap = (InnerBitmap)c;
                    ((PictureBox)innerBitmap.Controls[0]).Image = ImageBlend.MergeWithPanel(new Bitmap(((PictureBox)innerBitmap.Controls[0]).Image), new Point(c.Location.X + Global.panelWorkspace.AutoScrollPosition.X, c.Location.Y + Global.panelWorkspace.AutoScrollPosition.Y));
                    foreach (Control c2 in Global.panelWorkspace.Controls)
                    {
                        if (c2.GetType() != typeof(Scale)) continue;                        
                        Scale scale = (Scale)c2;
                        if (innerBitmap.elementName == scale.elementName) continue;
                        if(scale.Location == innerBitmap.Location)
                        {
                            Image front = null;
                            foreach(Control c3 in scale.Controls)
                            {
                                if (c3.GetType() != typeof(PictureBox)) continue;
                                if (c3.Name == "pictureBox1") front = ((PictureBox)c3).Image;
                            }
                            Image back = ((PictureBox)innerBitmap.Controls[0]).Image;
                            back = ImageBlend.ResizeImage(back, (int)(back.Width * scale.scale), (int)(back.Height * scale.scale));
                            //ImageBlend.Blend(back, front);
                            scale.ApplyMask(new Bitmap(back));
                        }
                    }
                }
            }
            Global.panelWorkspace.Refresh();
        }

        private static void ParseDrawing(string drawing)
        {
            string element = "";
            int readState = -1; //0 - имя, 1 - элемент, 2 - все прочитано
            for (int k = 0; k < drawing.Length; k++)
            {
                if (drawing[k] == '{')
                {
                    readState = 1;
                }
                else
                if (drawing[k] == '}')
                {
                    readState = 0;
                }
                else
                {
                    element += drawing[k];
                }
                if (readState == 0)
                {
                    VisualizeDrawing(element);
                    element = "";
                }
            }
        }

        private static void VisualizeDrawing(string element)
        {
            if (element == "") return;
            string type = GetClearField(element, "type");
            switch (type)
            {
                case "bitmap":
                    {
                        InnerBitmap bitmap = new InnerBitmap();
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = Global.X / 2;
                        x += Global.panelWorkspace.AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = Global.Y / 2;
                        y += Global.panelWorkspace.AutoScrollPosition.Y;
                        float scale;
                        if (!float.TryParse(GetClearField(element, "scale"), NumberStyles.Any, CultureInfo.InvariantCulture, out scale)) scale = 1;
                        string imageName = GetClearField(element, "bitmap");
                        if (imageName == "") imageName = bitmap.imageName.Split('.')[0];
                        bitmap.Apply(x, y, scale, imageName);                        
                        Global.panelWorkspace.Controls.Add(bitmap);
                        break;
                    }

                default:

                    break;
            }
        }
        
        private static string GetBigField(string gui, string obj)
        {
            return GetBigField(gui, obj, '{', '}');
        }

        private static string GetBigField(string gui, string v, char opener, char closer)
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
                        if (gui[k] == closer) counter--;
                        if (counter > 0)answer += gui[k];
                        if (gui[k] == opener) counter++;
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
            answer = answer.Replace("\r", "");
            answer = answer.Replace("\t", "");
            answer = answer.Replace(" ", "");
            return answer;
        }

        private static void ParseElements(string elements)
        {
            string element = "";
            string name = "";
            int readState = 0; //0 - имя, 1 - элемент, 2 - все прочитано
            int bracketCounter = 0; //Количество незакрытых скобов
            for (int k = 0; k < elements.Length; k++)
            {
                if (elements[k] == '{')
                {
                    bracketCounter++;
                    if (readState == 0) readState = 1;
                }
                else
                {
                    if(elements[k] == '}')
                    {
                        bracketCounter--;
                        if (readState == 1) readState = 2;
                        else if (readState == 0) continue;
                    }
                    if(readState == 0)
                    {
                        name += elements[k];
                    }

                }
                if(readState != 0)element += elements[k];
                if (readState == 2 && bracketCounter==0)
                {
                    name = ClearName(name);
                    VisualizeElement(name.Replace(" ", ""), element);
                    element = "";
                    name = "";
                    readState = 0;
                }
            }
        }

        private static string ClearName(string name)
        {
            name = Clear(name);
            name = name.Replace("\"", "");
            name = name.Replace(",", "");
            name = name.Replace(":", "");
            return name;
        }

        private static void VisualizeElement(string name, string element)
        {
            string type = GetClearField(element, "type");
            switch (type)
            {
                case "slot":
                    { 
                        Slot slot = new Slot();
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x))x = Global.X / 2;
                        x += Global.panelWorkspace.AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = Global.Y / 2;
                        y += Global.panelWorkspace.AutoScrollPosition.Y;
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
                    }

                case "invSlot":
                    {
                        InvSlot slot = new InvSlot();
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = Global.X / 2;
                        x += Global.panelWorkspace.AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = Global.Y / 2;
                        y += Global.panelWorkspace.AutoScrollPosition.Y;
                        int size;
                        if (!int.TryParse(GetClearField(element, "size").Split('.')[0], out size)) size = slot.Width;
                        string ImageName = GetClearField(element, "bitmap");
                        if (ImageName == "") ImageName = slot.ImageName.Split('.')[0];
                        string Clicker = GetField(element, "clicker");
                        int index;
                        if (!int.TryParse(GetClearField(element, "y").Split('.')[0], out index)) index = 0;
                        slot.Apply(name, x, y, size, ImageName,index);
                        Global.panelWorkspace.Controls.Add(slot);
                        break;
                    }

                case "button":
                    {
                        InnerButton slot = new InnerButton();
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = Global.X / 2;
                        x += Global.panelWorkspace.AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = Global.Y / 2;
                        y += Global.panelWorkspace.AutoScrollPosition.Y;
                        float scale;
                        if (!float.TryParse(GetClearField(element, "scale"), NumberStyles.Any, CultureInfo.InvariantCulture, out scale)) scale = 1;
                        string activeImageName = GetClearField(element, "bitmap");
                        if (activeImageName == "") activeImageName = slot.activeImageName.Split('.')[0];
                        string activeImage2 = GetClearField(element, "bitmap2");
                        if (activeImage2 == "") activeImage2 = activeImageName;
                        string Clicker = GetField(element, "clicker");
                        slot.Apply(name, x, y, scale, activeImage2, activeImageName, Clicker);
                        Global.panelWorkspace.Controls.Add(slot);
                        break;
                    }

                case "text":
                    {
                        InnerText text = new InnerText();
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = Global.X / 2;
                        x += Global.panelWorkspace.AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = Global.Y / 2;
                        y += Global.panelWorkspace.AutoScrollPosition.Y;
                        int width;
                        if (!int.TryParse(GetClearField(element, "width").Split('.')[0], out width)) width = text.Width;
                        int height;
                        if (!int.TryParse(GetClearField(element, "height").Split('.')[0], out height)) height = text.Height;
                        string _text = GetField(element, "text");
                        string Clicker = GetField(element, "clicker");
                        text.Apply(name, x, y, width, height, _text);
                        Global.panelWorkspace.Controls.Add(text);
                        break;
                    }

                case "image":
                    {
                        InnerImage image = new InnerImage();
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = Global.X / 2;
                        x += Global.panelWorkspace.AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = Global.Y / 2;
                        y += Global.panelWorkspace.AutoScrollPosition.Y;
                        float scale;
                        if (!float.TryParse(GetClearField(element, "scale"), NumberStyles.Any, CultureInfo.InvariantCulture, out scale)) scale = 1;
                        string imageName = GetClearField(element, "bitmap");
                        if (imageName == "") imageName = image.imageName.Split('.')[0];
                        string Clicker = GetField(element, "clicker");
                        string overlayImageName = GetClearField(element, "overlay");
                        if (overlayImageName == "")
                        {
                            image.Apply(name, x, y, scale, imageName, Clicker);
                        }
                        else
                        {
                            float overlayScale;
                            if (!float.TryParse(GetClearField(element, "overlayScale"), out overlayScale))
                            {
                                if (!float.TryParse(GetClearField(element, "overlay_scale"), out overlayScale))
                                {
                                    overlayScale = scale;
                                }
                            }
                            string offset = GetClearField(element, "overlayOffset");
                            int offsetX;
                            if (!int.TryParse(GetClearField(offset, "x").Split('.')[0], out offsetX)) offsetX = 0;
                            int offsetY;
                            if (!int.TryParse(GetClearField(offset, "x").Split('.')[0], out offsetY)) offsetY = 0;
                            image.Apply(name, x, y, scale, imageName, overlayImageName, new Point(offsetX, offsetY), overlayScale, Clicker);
                        }
                        Global.panelWorkspace.Controls.Add(image);
                        break;
                    }

                case "scale":
                    {
                        Scale _scale = new Scale();
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = Global.X / 2;
                        x += Global.panelWorkspace.AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = Global.Y / 2;
                        y += Global.panelWorkspace.AutoScrollPosition.Y;
                        float scale;
                        string _sc = GetClearField(element, "scale");
                        if (!float.TryParse(_sc, NumberStyles.Any, CultureInfo.InvariantCulture, out scale)) scale = 1;
                        string imageName = GetClearField(element, "bitmap");
                        if (imageName == "") imageName = _scale.imageName.Split('.')[0];
                        string Clicker = GetField(element, "clicker");
                        int side;
                        if (!int.TryParse(GetClearField(element, "direction").Split('.')[0], out side)) side = 0;
                        bool invert;
                        if (!bool.TryParse(GetClearField(element, "invert").Split('.')[0], out invert)) invert = false;
                        string overlayImageName = GetClearField(element, "overlay");
                        if (overlayImageName == "")
                        {
                            _scale.Apply(name, x, y, scale, imageName, side, invert, Clicker);
                        }
                        else
                        {
                            float overlayScale;
                            if (!float.TryParse(GetClearField(element, "overlayScale"), out overlayScale))
                            {
                                if (!float.TryParse(GetClearField(element, "overlay_scale"), out overlayScale))
                                {
                                    overlayScale = scale;
                                }
                            }
                            string offset = GetClearField(element, "overlayOffset");
                            int offsetX;
                            if (!int.TryParse(GetClearField(offset, "x").Split('.')[0], out offsetX)) offsetX = 0;
                            int offsetY;
                            if (!int.TryParse(GetClearField(offset, "x").Split('.')[0], out offsetY)) offsetY = 0;
                            _scale.Apply(name, x, y, scale, imageName, side, invert, overlayImageName, new Point(offsetX, offsetY), overlayScale, Clicker);
                        }
                        Global.panelWorkspace.Controls.Add(_scale);
                        break;
                    }

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
                    int bracketsCounter = 0;
                    for (int k = i + j; k < element.Length; k++)
                    {
                        if (element[k] == ':') counter++;
                        else if (element[k] == ',' || element[k] == '}') counter--;
                        else if (element[k] == '(') bracketsCounter++;
                        else if (element[k] == ')') bracketsCounter--;
                        if (counter == 0 && bracketsCounter == 0) break;
                        else answer += element[k];
                    }
                    break;
                }
            }
            return answer;
        }

        internal static void Save(string filename)
        {
            string standart = "standart: \n{";
            string headerText = Global.innerHeader.GetText();
            if(headerText != "")
            {
                standart += "\n\theader: { text: { text: \"" + headerText + "\"},";
                if (Global.innerHeader.GetButtonVisibility())
                    standart += "\nhideButton: true,";
                standart += "},";
            }
            if(Global.inventoryDrawed)
            {
                standart += "\n\tinventory: { standart: true},";
            }
            standart += "\n\tbackground: ";
            if(Global.panelWorkspace.BackgroundImage!=null)
            {
                standart += "{bitmap: \"" + Global.BackgroundImageName +"\"}";
            } else
            {
                Color bg_color = Global.panelWorkspace.BackColor;
                if (bg_color == Color.FromArgb(114, 106, 112))
                {
                    standart += "{standart: true}";
                } else
                {
                    standart += "{android.graphics.Color.rgb(" + bg_color.R + ',' + bg_color.G + ',' + bg_color.B + ")}";
                }
            }

            standart += "\n\t\tminHeight: " + Global.Y + ",";

            standart += "\n},\n";

            string _params = "params: \n{";

            if(Params.slotImageName != Params.Default_SlotImage)
            {
                _params += "\n\tslot: \"" + Params.slotImageName.Replace(".png", "") + "\",";
            }

            if (Params.invSlotImageName != Params.Default_InvSlotImage)
            {
                _params += "\n\tinvSlot: \"" + Params.invSlotImageName.Replace(".png", "") + "\",";
            }

            if (Params.selectionImageName != Params.Default_SelectionImage)
            {
                _params += "\n\tselection: \"" + Params.selectionImageName.Replace(".png", "") + "\",";
            }

            if (Params.closeButtonImageName != Params.Default_CloseButtonImage)
            {
                _params += "\n\tcloseButton: \"" + Params.closeButtonImageName.Replace(".png", "") + "\",";
            }

            if (Params.closeButton2ImageName != Params.Default_CloseButton2Image)
            {
                _params += "\n\tcloseButton: \"" + Params.closeButton2ImageName.Replace(".png", "") + "\",";
            }

            _params += "\n},\n";

            string drawing = "drawing: \n[";
            foreach (Control _c in Global.panelWorkspace.Controls)
            {
                if (_c.GetType() != typeof(InnerBitmap)) continue;
                InnerBitmap c = (InnerBitmap)_c;
                drawing += c.MakeOutput() + ',';
            }
            drawing += "\n],\n";

            string elements = "elements: \n{";

            foreach (Control _c in Global.panelWorkspace.Controls)
            {
                if (_c.GetType() == typeof(Label) || _c.GetType() == typeof(InnerHeader) || _c.GetType() == typeof(InnerBitmap)) continue;
                InnerControl c = (InnerControl)_c;
                if (c.hidden) continue;
                elements += c.MakeOutput() + ',';
            }

            elements += "\n}";

            File.WriteAllText(filename, standart + _params + drawing + elements);
        }
    }
}
