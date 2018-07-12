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
    public static class Params
    {

        public const string Default_SlotImage = "_default_slot_light";
        public const string Default_InvSlotImage = "_default_slot";

        public const string Default_SelectionImage = "_selection";
        public const string Default_CloseButtonImage = "close_button_up";
        public const string Default_CloseButton2Image = "close_button_down";

        private static Image slotDefaultImage;
        public static string slotImageName;
        private static Image invSlotDefaultImage;
        public static string invSlotImageName;
        private static Image selectionDefaultImage;
        public static string selectionImageName;
        private static Image closeButtonDefaultImage;
        public static string closeButtonImageName;
        private static Image closeButton2DefaultImage;
        public static string closeButton2ImageName;

        internal static void AllToDefault()
        {
            SlotToDefault();
            InvSlotToDefault();
            SelectionToDefault();
            CloseButtonToDefault();
            CloseButton2ToDefault();
        }

        internal static void Initialization()
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

        public static void SlotToDefault()
        {
            slotDefaultImage = Resources._default_slot_light;
            slotImageName = "_default_slot_light";
            Global.TurnToDefault(typeof(Slot), slotDefaultImage, slotImageName);
        }

        public static void SetSlotImage(Image target, string name)
        {
            slotDefaultImage = target;
            slotImageName = name;
            Global.TurnToDefault(typeof(Slot), slotDefaultImage, slotImageName);
        }

        internal static void LoadSlotImage(string filename)
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

        public static Image GetSlotImage(out string name)
        {
            name = slotImageName;
            return slotDefaultImage;
        }

        public static void InvSlotToDefault()
        {
            invSlotDefaultImage = Resources._default_slot_light;
            invSlotImageName = "_default_slot";
            Global.TurnToDefault(typeof(InvSlot), invSlotDefaultImage, invSlotImageName);
        }

        public static void SetInvSlotImage(Image target, string name)
        {
            invSlotDefaultImage = target;
            invSlotImageName = name;
            Global.TurnToDefault(typeof(InvSlot), invSlotDefaultImage, invSlotImageName);
        }

        internal static void LoadInvSlotImage(string filename)
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

        public static Image GetInvSlotImage(out string name)
        {
            name = invSlotImageName;
            return invSlotDefaultImage;
        }

        public static void SelectionToDefault()
        {
            selectionDefaultImage = Resources._selection;
            selectionImageName = "_selection";
            Global.TurnSlotsSelectionToDefault(selectionDefaultImage);
        }

        public static void SetSelectionImage(Image target, string name)
        {
            selectionDefaultImage = target;
            selectionImageName = name;
            Global.TurnSlotsSelectionToDefault(selectionDefaultImage);
        }

        internal static void LoadSelectionImage(string filename)
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

        public static Image GetSelectionImage(out string name)
        {
            name = selectionImageName;
            return selectionDefaultImage;
        }

        public static void CloseButtonToDefault()
        {
            closeButtonDefaultImage = Resources.close_button_up;
            closeButtonImageName = "close_button_up";
            Global.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        public static void SetCloseButtonImage(Image target, string name)
        {
            closeButtonDefaultImage = target;
            closeButtonImageName = name;
            Global.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        internal static void LoadCloseButtonImage(string filename)
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

        public static Image GetCloseButtonImage(out string name)
        {
            name = closeButtonImageName;
            return closeButtonDefaultImage;
        }

        public static void CloseButton2ToDefault()
        {
            closeButton2DefaultImage = Resources.close_button_down;
            closeButton2ImageName = "close_button_down";
            Global.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        public static void SetCloseButton2Image(Image target, string name)
        {
            closeButton2DefaultImage = target;
            closeButton2ImageName = name;
            Global.TurnCloseButtonsToDefault(closeButtonDefaultImage, closeButtonImageName, closeButton2DefaultImage, closeButton2ImageName);
        }

        internal static void LoadCloseButton2Image(string filename)
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

        public static Image GetCloseButton2Image(out string name)
        {
            name = closeButton2ImageName;
            return closeButton2DefaultImage;
        }
    }
}
