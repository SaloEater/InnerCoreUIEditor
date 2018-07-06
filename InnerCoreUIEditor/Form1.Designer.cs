namespace InnerCoreUIEditor
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelWorkspace = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelExplorer = new System.Windows.Forms.Panel();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.panelElements = new System.Windows.Forms.Panel();
            this.innerBitmapAdder1 = new InnerCoreUIEditor.InnerBitmapAdder();
            this.scaleAdder1 = new InnerCoreUIEditor.ScaleAdder();
            this.innerImageAdder1 = new InnerCoreUIEditor.InnerImageAdder();
            this.innerTextAdder1 = new InnerCoreUIEditor.InnerTextAdder();
            this.closeButtonAdder1 = new InnerCoreUIEditor.CloseButtonAdder();
            this.buttonAdder1 = new InnerCoreUIEditor.ButtonAdder();
            this.invSlotAdder1 = new InnerCoreUIEditor.InvSlotAdder();
            this.slotAdder1 = new InnerCoreUIEditor.SlotAdder();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.стандартныеВозможностиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.инвентарьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заголовокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заднийФонToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.цветToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.innerHeader1 = new InnerCoreUIEditor.Controls.InnerHeader();
            this.panelWorkspace.SuspendLayout();
            this.panelElements.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWorkspace
            // 
            this.panelWorkspace.AutoScroll = true;
            this.panelWorkspace.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelWorkspace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelWorkspace.Controls.Add(this.innerHeader1);
            this.panelWorkspace.Controls.Add(this.label1);
            this.panelWorkspace.Location = new System.Drawing.Point(0, 142);
            this.panelWorkspace.Name = "panelWorkspace";
            this.panelWorkspace.Size = new System.Drawing.Size(646, 446);
            this.panelWorkspace.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1368, 984);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 0;
            // 
            // panelExplorer
            // 
            this.panelExplorer.AutoScroll = true;
            this.panelExplorer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelExplorer.Location = new System.Drawing.Point(646, 142);
            this.panelExplorer.Name = "panelExplorer";
            this.panelExplorer.Size = new System.Drawing.Size(203, 220);
            this.panelExplorer.TabIndex = 1;
            // 
            // panelProperties
            // 
            this.panelProperties.AutoScroll = true;
            this.panelProperties.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelProperties.Location = new System.Drawing.Point(646, 362);
            this.panelProperties.Name = "panelProperties";
            this.panelProperties.Size = new System.Drawing.Size(204, 226);
            this.panelProperties.TabIndex = 0;
            // 
            // panelElements
            // 
            this.panelElements.AutoScroll = true;
            this.panelElements.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelElements.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelElements.Controls.Add(this.innerBitmapAdder1);
            this.panelElements.Controls.Add(this.scaleAdder1);
            this.panelElements.Controls.Add(this.innerImageAdder1);
            this.panelElements.Controls.Add(this.innerTextAdder1);
            this.panelElements.Controls.Add(this.closeButtonAdder1);
            this.panelElements.Controls.Add(this.buttonAdder1);
            this.panelElements.Controls.Add(this.invSlotAdder1);
            this.panelElements.Controls.Add(this.slotAdder1);
            this.panelElements.Location = new System.Drawing.Point(0, 34);
            this.panelElements.Name = "panelElements";
            this.panelElements.Size = new System.Drawing.Size(850, 108);
            this.panelElements.TabIndex = 2;
            // 
            // innerBitmapAdder1
            // 
            this.innerBitmapAdder1.Location = new System.Drawing.Point(480, 4);
            this.innerBitmapAdder1.Name = "innerBitmapAdder1";
            this.innerBitmapAdder1.Size = new System.Drawing.Size(60, 99);
            this.innerBitmapAdder1.TabIndex = 7;
            // 
            // scaleAdder1
            // 
            this.scaleAdder1.Location = new System.Drawing.Point(413, 4);
            this.scaleAdder1.Name = "scaleAdder1";
            this.scaleAdder1.Size = new System.Drawing.Size(60, 99);
            this.scaleAdder1.TabIndex = 6;
            // 
            // innerImageAdder1
            // 
            this.innerImageAdder1.Location = new System.Drawing.Point(346, 4);
            this.innerImageAdder1.Name = "innerImageAdder1";
            this.innerImageAdder1.Size = new System.Drawing.Size(60, 99);
            this.innerImageAdder1.TabIndex = 5;
            // 
            // innerTextAdder1
            // 
            this.innerTextAdder1.Location = new System.Drawing.Point(279, 4);
            this.innerTextAdder1.Name = "innerTextAdder1";
            this.innerTextAdder1.Size = new System.Drawing.Size(60, 99);
            this.innerTextAdder1.TabIndex = 4;
            // 
            // closeButtonAdder1
            // 
            this.closeButtonAdder1.Location = new System.Drawing.Point(212, 4);
            this.closeButtonAdder1.Name = "closeButtonAdder1";
            this.closeButtonAdder1.Size = new System.Drawing.Size(60, 99);
            this.closeButtonAdder1.TabIndex = 3;
            // 
            // buttonAdder1
            // 
            this.buttonAdder1.Location = new System.Drawing.Point(145, 4);
            this.buttonAdder1.Name = "buttonAdder1";
            this.buttonAdder1.Size = new System.Drawing.Size(60, 99);
            this.buttonAdder1.TabIndex = 2;
            // 
            // invSlotAdder1
            // 
            this.invSlotAdder1.Location = new System.Drawing.Point(78, 4);
            this.invSlotAdder1.Name = "invSlotAdder1";
            this.invSlotAdder1.Size = new System.Drawing.Size(60, 99);
            this.invSlotAdder1.TabIndex = 1;
            // 
            // slotAdder1
            // 
            this.slotAdder1.Location = new System.Drawing.Point(11, 4);
            this.slotAdder1.Name = "slotAdder1";
            this.slotAdder1.Size = new System.Drawing.Size(60, 99);
            this.slotAdder1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem,
            this.стандартныеВозможностиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(849, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.сохранитьToolStripMenuItem.Text = "Файл";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItem1.Text = "Сохранить гпи";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(154, 22);
            this.toolStripMenuItem2.Text = "Загрузить гпи";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // стандартныеВозможностиToolStripMenuItem
            // 
            this.стандартныеВозможностиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.инвентарьToolStripMenuItem,
            this.заголовокToolStripMenuItem,
            this.заднийФонToolStripMenuItem});
            this.стандартныеВозможностиToolStripMenuItem.Name = "стандартныеВозможностиToolStripMenuItem";
            this.стандартныеВозможностиToolStripMenuItem.Size = new System.Drawing.Size(169, 20);
            this.стандартныеВозможностиToolStripMenuItem.Text = "Стандартные возможности";
            // 
            // инвентарьToolStripMenuItem
            // 
            this.инвентарьToolStripMenuItem.Name = "инвентарьToolStripMenuItem";
            this.инвентарьToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.инвентарьToolStripMenuItem.Text = "Инвентарь";
            this.инвентарьToolStripMenuItem.Click += new System.EventHandler(this.инвентарьToolStripMenuItem_Click_1);
            // 
            // заголовокToolStripMenuItem
            // 
            this.заголовокToolStripMenuItem.Name = "заголовокToolStripMenuItem";
            this.заголовокToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.заголовокToolStripMenuItem.Text = "Заголовок";
            this.заголовокToolStripMenuItem.Click += new System.EventHandler(this.заголовокToolStripMenuItem_Click);
            // 
            // заднийФонToolStripMenuItem
            // 
            this.заднийФонToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.цветToolStripMenuItem,
            this.изображениеToolStripMenuItem});
            this.заднийФонToolStripMenuItem.Name = "заднийФонToolStripMenuItem";
            this.заднийФонToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.заднийФонToolStripMenuItem.Text = "Задний фон";
            // 
            // цветToolStripMenuItem
            // 
            this.цветToolStripMenuItem.Name = "цветToolStripMenuItem";
            this.цветToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.цветToolStripMenuItem.Text = "Цвет";
            this.цветToolStripMenuItem.Click += new System.EventHandler(this.цветToolStripMenuItem_Click);
            // 
            // изображениеToolStripMenuItem
            // 
            this.изображениеToolStripMenuItem.Name = "изображениеToolStripMenuItem";
            this.изображениеToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.изображениеToolStripMenuItem.Text = "Изображение";
            this.изображениеToolStripMenuItem.Click += new System.EventHandler(this.изображениеToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // innerHeader1
            // 
            this.innerHeader1.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.innerHeader1.BackColor = System.Drawing.Color.Transparent;
            this.innerHeader1.Location = new System.Drawing.Point(0, 0);
            this.innerHeader1.Name = "innerHeader1";
            this.innerHeader1.Size = new System.Drawing.Size(1000, 40);
            this.innerHeader1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 588);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.panelElements);
            this.Controls.Add(this.panelExplorer);
            this.Controls.Add(this.panelWorkspace);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelWorkspace.ResumeLayout(false);
            this.panelWorkspace.PerformLayout();
            this.panelElements.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelWorkspace;
        private System.Windows.Forms.Panel panelExplorer;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.Panel panelElements;
        private SlotAdder slotAdder1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private InvSlotAdder invSlotAdder1;
        private System.Windows.Forms.Timer timer1;
        private ButtonAdder buttonAdder1;
        private CloseButtonAdder closeButtonAdder1;
        private InnerTextAdder innerTextAdder1;
        private InnerImageAdder innerImageAdder1;
        private ScaleAdder scaleAdder1;
        private InnerBitmapAdder innerBitmapAdder1;
        private System.Windows.Forms.ToolStripMenuItem стандартныеВозможностиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem инвентарьToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem заголовокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заднийФонToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem цветToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изображениеToolStripMenuItem;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private Controls.InnerHeader innerHeader1;
    }
}

