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
            this.panelWorkspace = new System.Windows.Forms.Panel();
            this.panelExplorer = new System.Windows.Forms.Panel();
            this.panelProperties = new System.Windows.Forms.Panel();
            this.panelElements = new System.Windows.Forms.Panel();
            this.slotAdder1 = new InnerCoreUIEditor.SlotAdder();
            this.panelOptions = new System.Windows.Forms.Panel();
            this.panelElements.SuspendLayout();
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
            this.panelProperties.Visible = false;
            // 
            // panelElements
            // 
            this.panelElements.AutoScroll = true;
            this.panelElements.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelElements.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelElements.Controls.Add(this.slotAdder1);
            this.panelElements.Location = new System.Drawing.Point(0, 34);
            this.panelElements.Name = "panelElements";
            this.panelElements.Size = new System.Drawing.Size(850, 108);
            this.panelElements.TabIndex = 2;
            // 
            // slotAdder1
            // 
            this.slotAdder1.Location = new System.Drawing.Point(68, 6);
            this.slotAdder1.Name = "slotAdder1";
            this.slotAdder1.Size = new System.Drawing.Size(60, 99);
            this.slotAdder1.TabIndex = 0;
            // 
            // panelOptions
            // 
            this.panelOptions.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panelOptions.Location = new System.Drawing.Point(0, 0);
            this.panelOptions.Name = "panelOptions";
            this.panelOptions.Size = new System.Drawing.Size(849, 34);
            this.panelOptions.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 588);
            this.Controls.Add(this.panelProperties);
            this.Controls.Add(this.panelOptions);
            this.Controls.Add(this.panelElements);
            this.Controls.Add(this.panelExplorer);
            this.Controls.Add(this.panelWorkspace);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panelElements.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelWorkspace;
        private System.Windows.Forms.Panel panelExplorer;
        private System.Windows.Forms.Panel panelProperties;
        private System.Windows.Forms.Panel panelElements;
        private System.Windows.Forms.Panel panelOptions;
        private SlotAdder slotAdder1;
    }
}

