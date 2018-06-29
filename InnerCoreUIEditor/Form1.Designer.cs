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
            this.panelExplorer = new System.Windows.Forms.Panel();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.panelElements = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.invSlotAdder1 = new InnerCoreUIEditor.InvSlotAdder();
            this.slotAdder1 = new InnerCoreUIEditor.SlotAdder();
            this.buttonAdder1 = new InnerCoreUIEditor.ButtonAdder();
            this.panelElements.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWorkspace
            // 
            this.panelWorkspace.AutoScroll = true;
            this.panelWorkspace.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelWorkspace.Location = new System.Drawing.Point(0, 142);
            this.panelWorkspace.Name = "panelWorkspace";
            this.panelWorkspace.Size = new System.Drawing.Size(646, 446);
            this.panelWorkspace.TabIndex = 0;
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
            this.panelElements.Controls.Add(this.buttonAdder1);
            this.panelElements.Controls.Add(this.invSlotAdder1);
            this.panelElements.Controls.Add(this.slotAdder1);
            this.panelElements.Location = new System.Drawing.Point(0, 34);
            this.panelElements.Name = "panelElements";
            this.panelElements.Size = new System.Drawing.Size(850, 108);
            this.panelElements.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьToolStripMenuItem});
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
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.сохранитьToolStripMenuItem.Text = "Тест";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(132, 22);
            this.toolStripMenuItem1.Text = "Сохранить";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(132, 22);
            this.toolStripMenuItem2.Text = "Загрузить";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
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
            // buttonAdder1
            // 
            this.buttonAdder1.Location = new System.Drawing.Point(145, 4);
            this.buttonAdder1.Name = "buttonAdder1";
            this.buttonAdder1.Size = new System.Drawing.Size(60, 99);
            this.buttonAdder1.TabIndex = 2;
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
    }
}

