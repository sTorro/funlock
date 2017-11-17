namespace Funlock {
    partial class FormMain {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(
                        FormMain));
            this.ButtonKill = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.ProcessList = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            //
            // ButtonKill
            //
            this.ButtonKill.Location = new System.Drawing.Point(12, 184);
            this.ButtonKill.Name = "ButtonKill";
            this.ButtonKill.Size = new System.Drawing.Size(117, 37);
            this.ButtonKill.TabIndex = 0;
            this.ButtonKill.Text = "&Kill \'em!";
            this.ButtonKill.UseVisualStyleBackColor = true;
            this.ButtonKill.Click += new System.EventHandler(this.BtnKill_Click);
            //
            // ButtonCancel
            //
            this.ButtonCancel.Location = new System.Drawing.Point(362, 184);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(117, 37);
            this.ButtonCancel.TabIndex = 2;
            this.ButtonCancel.Text = "&Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            //
            // ProcessList
            //
            this.ProcessList.FormattingEnabled = true;
            this.ProcessList.Location = new System.Drawing.Point(12, 9);
            this.ProcessList.Name = "ProcessList";
            this.ProcessList.Size = new System.Drawing.Size(467, 169);
            this.ProcessList.TabIndex = 3;
            this.ProcessList.SelectedIndexChanged += new System.EventHandler(this.ProcessList_SelectedIndexChanged);
            //
            // FormMain
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 230);
            this.Controls.Add(this.ProcessList);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonKill);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(327, 144);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "funlock";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonKill;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.CheckedListBox ProcessList;
    }
}

