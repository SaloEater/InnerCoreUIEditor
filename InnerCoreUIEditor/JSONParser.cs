using InnerCoreUIEditor.Controls;
using NCalc;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
     class JSONParser
    {
        Params _params;
        ExplorerPainter explorerPainter;
        InnerTabPage parentTabPage;

        public JSONParser(Params _params, ExplorerPainter explorerPainter, InnerTabPage parentTabPage)
        {
            this.parentTabPage = parentTabPage;
            this._params = _params;
            this.explorerPainter = explorerPainter;
        }

        public  void Parse(string gui)
        {
            //int standartPosition = FindEntrance(gui, "standart:");
            //ParseStandart(gui);
            gui = Clear(gui);
            string _params = GetBigField(gui, "params");
            ParseParams(_params);
            string standart = GetBigField(gui, "standart");
            ParseStandart(standart);
            string drawing = GetBigField(gui, "drawing", '[', ']');
            ParseDrawing(drawing);
            string elements = GetBigField(gui, "elements");            
            ParseElements(elements);
            CombineDrawingsWithElements();
        }

        private  void ParseParams(string _params)
        {
            string type = GetClearField(_params, "slot");
            if (type != "") this._params.LoadSlotImage(type);
            type = GetClearField(_params, "invSlot");
            if (type != "") this._params.LoadInvSlotImage(type);
            type = GetClearField(_params, "selection");
            if (type != "") this._params.LoadSelectionImage(type);
            type = GetClearField(_params, "closeButton");
            if (type != "") this._params.LoadCloseButtonImage(type);
            type = GetClearField(_params, "closeButton2");
            if (type != "") this._params.LoadCloseButton2Image(type);
        }

        private  void ParseStandart(string standart)
        {
            string header = GetBigField(standart, "header");
            string str1 = GetField(header, "text");
            header = GetClearField(str1, "text", false);
            if (header != "") parentTabPage.SetHeaderText(header);
            if (bool.TryParse(GetField(GetField(header, "text"), "hideButton"), out bool b) && b) parentTabPage.innerHeader.SetButtonVisibilty(true);

            string inventory = GetBigField(standart, "inventory");
            inventory = GetClearField(inventory, "standart", true);
            if (bool.TryParse(inventory, out b) && b)parentTabPage.DrawInventorySlots();
            else if(parentTabPage.inventoryDrawed)parentTabPage.RemoveInventorySlots();

            string background = GetBigField(standart, "background");
            string bg_standart = GetClearField(background, "standart");
            if (!bool.TryParse("bg_standart", out b) || !b)
            {
                string bg_color = GetField(background, "color");
                if (bg_color != "") parentTabPage.SetGlobalColor(bg_color);
                string bg_bitmap = GetClearField(background, "bitmap");
                if (bg_bitmap != "") parentTabPage.SetGlobalBackground(bg_bitmap.Replace("\"", ""));
            }

            string minHeight = GetClearField(standart, "minHeight");
            if (int.TryParse(minHeight, out int height)) parentTabPage.ChangeHeight(height);
            else parentTabPage.ChangeHeight(parentTabPage.defaultHeight);
        }

        private  void ParseDrawing(string drawing)
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

        private  void VisualizeDrawing(string element)
        {
            if (element == "") return;
            string type = GetClearField(element, "type");
            switch (type)
            {
                case "bitmap":
                    {
                        InnerBitmap bitmap = new InnerBitmap(explorerPainter, _params, parentTabPage);
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = parentTabPage.MaxX() / 2;
                        x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = parentTabPage.MaxY() / 2;
                        y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
                        float scale;
                        if (!float.TryParse(GetClearField(element, "scale"), NumberStyles.Any, CultureInfo.InvariantCulture, out scale)) scale = 1;
                        string imageName = GetClearField(element, "bitmap");
                        if (imageName == "") imageName = bitmap.imageName.Split('.')[0];
                        bitmap.Apply(x, y, scale, imageName);
                        bitmap.elementName = bitmap.imageName;
                        parentTabPage.GetDesktopPanel().Controls.Add(bitmap);
                        break;
                    }

                default:

                    break;
            }
        }
        
        private  string GetBigField(string gui, string obj)
        {
            return GetBigField(gui, obj, '{', '}');
        }

        private  string GetBigField(string gui, string v, char opener, char closer)
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
                        if (/*counter == 0 && gui[k] != ' ' || */counter == 0 && gui[k] == ',') break;
                    }
                    break;
                }
            }
            //answer = Clear(answer);
            return answer;
        }

        private  string Clear(string answer)
        {
            //answer = answer.Replace("\n", "");
            answer = answer.Replace("\r", "");
            //answer = answer.Replace("\t", "");
            return answer;
        }

        private  string ClearSpacing(string str)
        {
            return str.Replace(" ", "");
        }

        private  void ParseElements(string elements)
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

        private  string ClearName(string name)
        {
            name = Clear(name);
            name = name.Replace("\"", "");
            name = name.Replace(",", "");
            name = name.Replace(":", "");
            name = name.Replace("\n", "");
            name = name.Replace("\t", "");
            return name;
        }

        private  void VisualizeElement(string name, string element)
        {
            string type = GetClearField(element, "type");
            switch (type)
            {
                case "slot":
                    { 
                        Slot slot = new Slot(explorerPainter, this._params, parentTabPage);
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x))x = parentTabPage.MaxX() / 2;
                        x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = parentTabPage.MaxY() / 2;
                        y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
                        int size;
                        if (!int.TryParse(GetClearField(element, "size").Split('.')[0], out size)) size = slot.Width;
                        bool visual;
                        if (!bool.TryParse(GetClearField(element, "visual"), out visual)) visual = false;
                        string ImageName = GetClearField(element, "bitmap");
                        if (ImageName == "") ImageName = slot.ImageName;
                        string Clicker = GetField(element, "clicker");
                        slot.Apply(name, x, y, size, visual, ImageName, Clicker);
                        parentTabPage.GetDesktopPanel().Controls.Add(slot);
                        break;
                    }

                case "invSlot":
                    {
                        InvSlot slot = new InvSlot(explorerPainter, this._params, parentTabPage);
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = parentTabPage.MaxX() / 2;
                        x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = parentTabPage.MaxY() / 2;
                        y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
                        int size;
                        if (!int.TryParse(GetClearField(element, "size").Split('.')[0], out size)) size = slot.Width;
                        string ImageName = GetClearField(element, "bitmap");
                        if (ImageName == "") ImageName = slot.ImageName.Split('.')[0];
                        string Clicker = GetField(element, "clicker");
                        int index;
                        if (!int.TryParse(GetClearField(element, "y").Split('.')[0], out index)) index = 0;
                        slot.Apply(name, x, y, size, ImageName, index);
                        parentTabPage.GetDesktopPanel().Controls.Add(slot);
                        break;
                    }

                case "button":
                    {
                        InnerButton slot = new InnerButton(explorerPainter, this._params, parentTabPage);
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = parentTabPage.MaxX() / 2;
                        x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = parentTabPage.MaxY() / 2;
                        y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
                        float scale;
                        if (!float.TryParse(GetClearField(element, "scale"), NumberStyles.Any, CultureInfo.InvariantCulture, out scale)) scale = 1;
                        string activeImageName = GetClearField(element, "bitmap");
                        if (activeImageName == "") activeImageName = slot.activeImageName.Split('.')[0];
                        string activeImage2 = GetClearField(element, "bitmap2");
                        if (activeImage2 == "") activeImage2 = activeImageName;
                        string Clicker = GetField(element, "clicker");
                        slot.Apply(name, x, y, scale, activeImage2, activeImageName, Clicker);
                        parentTabPage.GetDesktopPanel().Controls.Add(slot);
                        break;
                    }

                case "text":
                    {
                        InnerText text = new InnerText(explorerPainter, this._params, parentTabPage);
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = parentTabPage.MaxX() / 2;
                        x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = parentTabPage.MaxY() / 2;
                        y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
                        int width;
                        if (!int.TryParse(GetClearField(element, "width").Split('.')[0], out width)) width = text.Width;
                        int height;
                        if (!int.TryParse(GetClearField(element, "height").Split('.')[0], out height)) height = text.Height;
                        string _text = GetField(element, "text");
                        text.Apply(name, x, y, width, height, _text);
                        parentTabPage.GetDesktopPanel().Controls.Add(text);
                        break;
                    }

                case "image":
                    {
                        InnerImage image = new InnerImage(explorerPainter, this._params, parentTabPage);
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = parentTabPage.MaxX() / 2;
                        x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = parentTabPage.MaxY() / 2;
                        y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
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
                        parentTabPage.GetDesktopPanel().Controls.Add(image);
                        break;
                    }

                case "scale":
                    {
                        Scale _scale = new Scale(explorerPainter, this._params, parentTabPage);
                        int x;
                        Expression e = new Expression(GetClearField(element, "x"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out x)) x = parentTabPage.MaxX() / 2;
                        x += parentTabPage.GetDesktopPanel().AutoScrollPosition.X;
                        int y;
                        e = new Expression(GetClearField(element, "y"));
                        if (!int.TryParse(e.Evaluate().ToString().Split(',')[0], out y)) y = parentTabPage.MaxY() / 2;
                        y += parentTabPage.GetDesktopPanel().AutoScrollPosition.Y;
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
                        parentTabPage.GetDesktopPanel().Controls.Add(_scale);
                        break;
                    }

                default:


                    break;
            }
        }

        private  string GetClearField(string element, string v)
        {
            return GetClearField(element, v, true);
        }

        private  string GetClearField(string element, string v, bool spacing)
        {
            string answer = GetField(element, v).Replace("\"", "");
            if (spacing) answer = answer.Replace(" ", "").Replace("\n", "").Replace("\t", "");
            return answer;
        }

        private  string GetField(string element, string v)
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
                    if (element[i + j] != ':') continue;
                    /*int entrance = 1;
                    int counter = 0;
                    int bracketsCounter = 0;
                    int doubleDots = 0;*/
                    char _k;
                    int elementStarted = 0;
                    int curveBrackets = -1;
                    int brackets = -1;
                    for (int k = i + j; k < element.Length; k++)
                    {
                        _k = element[k];

                        if (elementStarted == 1)
                        {

                            if (curveBrackets == -1 && brackets == -1 && _k == ',' || _k == '}' && curveBrackets == -1)
                                elementStarted = 0;


                            if (elementStarted == 0) break;

                            if (_k == '{')
                                curveBrackets = curveBrackets == -1 ? 1 : curveBrackets + 1;
                            else if (_k == '}')
                                curveBrackets = curveBrackets == -1 ? 0 : curveBrackets - 1;

                            if (_k == '(')
                                brackets = brackets == -1 ? 1 : brackets + 1;
                            else if (_k == ')')
                                brackets = brackets == -1 ? 0 : brackets - 1;

                            answer += _k;

                            if (brackets == 0 && curveBrackets == 0) break;
                        }

                        if (_k == ':')
                        {
                            elementStarted = 1;
                        }

                        /*if (_k == ':') doubleDots = 1;
                        else if (_k == '{') counter++;
                        else if (_k == '}')
                        {
                            doubleDots = 0;
                            counter--;
                            if (counter <= 0) entrance = 0;
                        }
                        else if (_k == '(') bracketsCounter++;
                        else if (_k == ')') bracketsCounter--;
                        else if (_k == ',' && counter <= 0 && bracketsCounter <= 0) entrance = 0;
                        if (doubleDots == 0 && counter <= 0 && bracketsCounter <= 0 && entrance == 0) break;
                        else answer += _k;*/
                    }
                    break;
                }
            }
            return answer;
        }

        private  void CombineDrawingsWithElements()
        {
            //save
            foreach (Control c in parentTabPage.GetDesktopPanel().Controls)
            {
                if (c.GetType() == typeof(Label) || c.GetType() == typeof(InnerHeader)) continue;
                if (c.GetType() == typeof(InnerBitmap))
                {
                    InnerBitmap innerBitmap = (InnerBitmap)c;
                    ((PictureBox)innerBitmap.Controls[0]).Image = ImageBlend.MergeWithPanel(parentTabPage.GetDesktopPanel(), new Bitmap(((PictureBox)innerBitmap.Controls[0]).Image), new Point(c.Location.X + parentTabPage.GetDesktopPanel().AutoScrollPosition.X, c.Location.Y + parentTabPage.GetDesktopPanel().AutoScrollPosition.Y));
                    foreach (Control c2 in parentTabPage.GetDesktopPanel().Controls)
                    {
                        if (c2.GetType() != typeof(Scale)) continue;
                        Scale scale = (Scale)c2;
                        if (innerBitmap.elementName == scale.elementName) continue;
                        if (scale.Location == innerBitmap.Location)
                        {
                            Image front = null;
                            foreach (Control c3 in scale.Controls)
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
            parentTabPage.GetDesktopPanel().Refresh();
        }

        internal  void Save(string filename)
        {
            string standart = "{\nstandart: \n{";
            string headerText = parentTabPage.innerHeader.GetText();
            if(headerText != "")
            {
                standart += "\n\t\theader: \n\t\t{ \n\t\t\ttext: { text: \"" + headerText.Replace("\t", "").Replace("\n", "") + "\"},";
                if (!parentTabPage.innerHeader.GetButtonVisibility())
                    standart += "\n\t\t\thideButton: false,";
                standart += "\n\t\t},";
            }
            if(parentTabPage.inventoryDrawed)
            {
                standart += "\n\t\tinventory: { standart: true},";
            }
            standart += "\n\t\tbackground: ";
            if(parentTabPage.GetDesktopPanel().BackgroundImage!=null)
            {
                standart += "{bitmap: \"" + parentTabPage.BackgroundImageName +"\"}";
            } else
            {
                Color bg_color = parentTabPage.GetDesktopPanel().BackColor;
                if (bg_color == Color.FromArgb(114, 106, 112))
                {
                    standart += "{standart: true}";
                } else
                {
                    standart += "{android.graphics.Color.rgb(" + bg_color.R + ',' + bg_color.G + ',' + bg_color.B + ")}";
                }
            }

            standart += "\n\t\tminHeight: " + parentTabPage.MaxY() + ",";

            standart += "\n},\n";

            string _params = "params: \n{";

            if(!this._params.IsSlotDefault())
            {
                _params += "\n\t\tslot: \"" + this._params.slotImageName.Replace(".png", "") + "\",";
            }

            if (!this._params.IsInvSlotDefault())
            {
                _params += "\n\t\tinvSlot: \"" + this._params.invSlotImageName.Replace(".png", "") + "\",";
            }

            if (!this._params.IsSelectionDefault())
            {
                _params += "\n\t\tselection: \"" + this._params.selectionImageName.Replace(".png", "") + "\",";
            }

            if (!this._params.IsCloseButtonDefault())
            {
                _params += "\n\t\tcloseButton: \"" + this._params.closeButtonImageName.Replace(".png", "") + "\",";
            }

            if (!this._params.IsCloseButton2Default())
            {
                _params += "\n\t\tcloseButton: \"" + this._params.closeButton2ImageName.Replace(".png", "") + "\",";
            }

            _params += "\n},\n";

            string drawing = "drawing: \n[";
            foreach (Control _c in parentTabPage.GetDesktopPanel().Controls)
            {
                if (_c.GetType() != typeof(InnerBitmap)) continue;
                InnerBitmap c = (InnerBitmap)_c;
                drawing += c.MakeOutput() + ',';
            }
            drawing += "\n],\n";

            string elements = "elements: \n{";

            foreach (Control _c in parentTabPage.GetDesktopPanel().Controls)
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
