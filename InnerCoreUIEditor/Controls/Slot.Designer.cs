﻿namespace InnerCoreUIEditor
{
    partial class Slot
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBoxSlot = new System.Windows.Forms.PictureBox();
            this.pictureBoxSelection = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxSlot
            // 
            this.pictureBoxSlot.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxSlot.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSlot.Name = "pictureBoxSlot";
            this.pictureBoxSlot.Size = new System.Drawing.Size(60, 60);
            this.pictureBoxSlot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSlot.TabIndex = 1;
            this.pictureBoxSlot.TabStop = false;
            // 
            // pictureBoxSelection
            // 
            this.pictureBoxSelection.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSelection.Name = "pictureBoxSelection";
            this.pictureBoxSelection.Size = new System.Drawing.Size(60, 60);
            this.pictureBoxSelection.TabIndex = 2;
            this.pictureBoxSelection.TabStop = false;
            this.pictureBoxSelection.Visible = false;
            // 
            // Slot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBoxSelection);
            this.Controls.Add(this.pictureBoxSlot);
            this.Name = "Slot";
            this.Size = new System.Drawing.Size(60, 60);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSelection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBoxSlot;
        private System.Windows.Forms.PictureBox pictureBoxSelection;
    }
}
