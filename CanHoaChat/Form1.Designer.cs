namespace CanHoaChat
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mlBack = new MetroFramework.Controls.MetroLink();
            this.mResult = new MetroFramework.Controls.MetroPanel();
            this.SuspendLayout();
            // 
            // mlBack
            // 
            this.mlBack.Image = global::CanHoaChat.Properties.Resources.icons8_back_64;
            this.mlBack.ImageSize = 32;
            this.mlBack.Location = new System.Drawing.Point(90, 27);
            this.mlBack.Name = "mlBack";
            this.mlBack.Size = new System.Drawing.Size(75, 53);
            this.mlBack.TabIndex = 1;
            this.mlBack.UseSelectable = true;
            this.mlBack.Click += new System.EventHandler(this.mlBack_Click);
            // 
            // mResult
            // 
            this.mResult.HorizontalScrollbarBarColor = true;
            this.mResult.HorizontalScrollbarHighlightOnWheel = false;
            this.mResult.HorizontalScrollbarSize = 10;
            this.mResult.Location = new System.Drawing.Point(23, 86);
            this.mResult.Name = "mResult";
            this.mResult.Size = new System.Drawing.Size(1890, 993);
            this.mResult.TabIndex = 2;
            this.mResult.VerticalScrollbarBarColor = true;
            this.mResult.VerticalScrollbarHighlightOnWheel = false;
            this.mResult.VerticalScrollbarSize = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.Controls.Add(this.mResult);
            this.Controls.Add(this.mlBack);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(1918, 1018);
            this.Name = "Form1";
            this.Text = "              Hệ thống cân hóa chất ESSONS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroLink mlBack;
        private MetroFramework.Controls.MetroPanel mResult;
    }
}

