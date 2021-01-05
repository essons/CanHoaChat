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
            this.mResult = new MetroFramework.Controls.MetroPanel();
            this.pbTram = new System.Windows.Forms.PictureBox();
            this.mlBack = new MetroFramework.Controls.MetroLink();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbTram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // mResult
            // 
            this.mResult.BackColor = System.Drawing.Color.Transparent;
            this.mResult.HorizontalScrollbarBarColor = true;
            this.mResult.HorizontalScrollbarHighlightOnWheel = false;
            this.mResult.HorizontalScrollbarSize = 8;
            this.mResult.Location = new System.Drawing.Point(20, 69);
            this.mResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mResult.Name = "mResult";
            this.mResult.Size = new System.Drawing.Size(1680, 794);
            this.mResult.TabIndex = 2;
            this.mResult.UseCustomBackColor = true;
            this.mResult.VerticalScrollbarBarColor = true;
            this.mResult.VerticalScrollbarHighlightOnWheel = false;
            this.mResult.VerticalScrollbarSize = 9;
            // 
            // pbTram
            // 
            this.pbTram.Location = new System.Drawing.Point(1169, 13);
            this.pbTram.Name = "pbTram";
            this.pbTram.Size = new System.Drawing.Size(328, 59);
            this.pbTram.TabIndex = 3;
            this.pbTram.TabStop = false;
            // 
            // mlBack
            // 
            this.mlBack.Image = global::CanHoaChat.Properties.Resources.icons8_back_64;
            this.mlBack.ImageSize = 32;
            this.mlBack.Location = new System.Drawing.Point(80, 22);
            this.mlBack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mlBack.Name = "mlBack";
            this.mlBack.Size = new System.Drawing.Size(67, 42);
            this.mlBack.TabIndex = 1;
            this.mlBack.UseSelectable = true;
            this.mlBack.Click += new System.EventHandler(this.mlBack_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(167, 16);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(846, 50);
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1707, 864);
            this.Controls.Add(this.pbTram);
            this.Controls.Add(this.mlBack);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.mResult);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1707, 864);
            this.MinimumSize = new System.Drawing.Size(1705, 814);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(18, 60, 18, 16);
            this.ShadowType = MetroFramework.Forms.MetroFormShadowType.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbTram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroLink mlBack;
        private MetroFramework.Controls.MetroPanel mResult;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pbTram;
    }
}

