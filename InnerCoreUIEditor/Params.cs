using InnerCoreUIEditor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InnerCoreUIEditor
{
    public  class Params
    {

        public const string Default_SlotImage = "_default_slot_light";
        public const string Default_InvSlotImage = "_default_slot";

        public const string Default_SelectionImage = "_selection";
        public const string Default_CloseButtonImage = "close_button_up";
        public const string Default_CloseButton2Image = "close_button_down";

        private  Image slotDefaultImage;
        public  string slotImageName;
        private  Image invSlotDefaultImage;
        public  string invSlotImageName;
        private  Image selectionDefaultImage;
        public  string selectionImageName;
        private  Image closeButtonDefaultImage;
        public  string closeButtonImageName;
        private  Image closeButton2DefaultImage;
        public  string closeButton2ImageName;

        InnerTabPage innerTabPage;

        public Params(InnerTabPage innerTabPage)
        {
            this.innerTabPage = innerTabPage;
            AllToDefault();
        }

        internal  void AllToDefault()
        {
            SlotToDefault();
            InvSlotToDefault();
            SelectionToDefault();
            CloseButtonToDefault();
            CloseButton2ToDefault();
        }

        internal  void Initialization()
        {
            slotDefaultImage = Resources._default_slot_light;
            slotImageName = "_default_slot_light";

            invSlotDefaultImage = Resources._default_slot_light;
            invSlotImageName = "_default_slot";

            selectionDefaultImage = Resources._selection;
            selectionImageName = "_selection";

            closeButtonDefaultImage = Resources.close_button_up;
            closeButtonImageName = "close_button_up";

            closeButton2DefaultImage = Resources.close_button_down;
            closeButton2ImageName = "close_button_down";
        }

        public  void SlotToDefault()
        {
            slotDefaultImage = Resources._default_slot_light;
            slotImageName = "_default_slot_light";
            innerTabPage.TurnToDefault(typeof(Slot), slotDefaultImage, slotImageName);
        }

        public  void SetSlotImage(Image target, string name)
        {
            slotDefaultImage = target;
            slotImageName = name;
            innerTabPage.TurnToDefault(typeof(Slot), slotDefaultImage, slotImageName);
        }

        internal  void LoadSlotImage(string filename)
        {
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + filename + ".png";
            slotImageName = filename + ".png";
            try
            {
                slotDefaultImage = Bitmap.FromFile(path);
            }catch(Exception)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                SlotToDefault();
            }
        }

        public  Image GetSlotImage(out string name)
        {
            name = slotImageName;
            return slotDefaultImage;
        }

        public  void InvSlotToDefault()
        {
            invSlotDefaultImage = Resources._default_slot_light;
            invSlotImageName = "_default_slot";
            innerTabPage.TurnToDefault(typeof(InvSlot), invSlotDefaultImage, invSlotImageName);
        }

        public  void SetInvSlotImage(Image target, string name)
        {
            invSlotDefaultImage = target;
            invSlotImageName = name;
            innerTabPage.TurnToDefault(typeof(InvSlot), invSlotDefaultImage, invSlotImageName);
        }

        internal  void LoadInvSlotImage(string filename)
        {
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + filename + ".png";
            invSlotImageName = filename + ".png";
            try
            {
                invSlotDefaultImage = Bitmap.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                InvSlotToDefault();
            }
        }

        public  Image GetInvSlotImage(out string name)
        {
            name = invSlotImageName;
            return invSlotDefaultImage;
        }

        public  void SelectionToDefault()
        {
            selectionDefaultImage = Resources._selection;
            selectionImageName = "_selection";
            innerTabPage.TurnSlotsSelectionToDefault(selectionDefaultImage);
        }

        public  void SetSelectionImage(Image target, string name)
        {
            selectionDefaultImage = target;
            selectionImageName = name;
            innerTabPage.TurnSlotsSelectionToDefault(selectionDefaultImage);
        }

        internal  void LoadSelectionImage(string filename)
        {
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + filename + ".png";
            selectionImageName = filename + ".png";
            try
            {
                selectionDefaultImage = Bitmap.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                SelectionToDefault();
            }
        }

        public  Image GetSelectionImage(out string name)
        {
            name = selectionImageName;
            return selectionDefaultImage;
        }

        public  void CloseButtonToDefault()
        {
            closeButtonDefaultImage = Resources.close_button_up;
            closeButtonImageName = "close_button_up";
            innerTabPage.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        public  void SetCloseButtonImage(Image target, string name)
        {
            closeButtonDefaultImage = target;
            closeButtonImageName = name;
            innerTabPage.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        internal  void LoadCloseButtonImage(string filename)
        {
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + filename + ".png";
            closeButtonImageName = filename + ".png";
            try
            {
                closeButtonDefaultImage = Bitmap.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                CloseButtonToDefault();
            }
        }

        public  Image GetCloseButtonImage(out string name)
        {
            name = closeButtonImageName;
            return closeButtonDefaultImage;
        }

        public  void CloseButton2ToDefault()
        {
            closeButton2DefaultImage = Resources.close_button_down;
            closeButton2ImageName = "close_button_down";
            innerTabPage.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        public  void SetCloseButton2Image(Image target, string name)
        {
            closeButton2DefaultImage = target;
            closeButton2ImageName = name;
            innerTabPage.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        internal  void LoadCloseButton2Image(string filename)
        {
            string path = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\gui\" + filename + ".png";
            closeButton2ImageName = filename + ".png";
            try
            {
                closeButton2DefaultImage = Bitmap.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует файл " + path + ". Добавьте его и загрузите заново");
                CloseButton2ToDefault();
            }
        }

        public  Image GetCloseButton2Image(out string name)
        {
            name = closeButton2ImageName;
            return closeButton2DefaultImage;
        }

        internal bool IsSlotDefault()
        {
            return slotImageName == Default_SlotImage;
        }

        internal bool IsInvSlotDefault()
        {
            return invSlotImageName == Default_InvSlotImage;
        }

        internal bool IsSelectionDefault()
        {
            return selectionImageName == Default_SelectionImage;
        }

        internal bool IsCloseButtonDefault()
        {
            return closeButtonImageName == Default_CloseButtonImage;
        }

        internal bool IsCloseButton2Default()
        {
            return closeButton2ImageName == Default_CloseButton2Image;
        }
    }
}
