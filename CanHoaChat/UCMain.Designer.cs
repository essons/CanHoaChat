namespace CanHoaChat
{
    partial class UCMain
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mCanTuDong = new MetroFramework.Controls.MetroTile();
            this.mCanBanTuDong = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // mCanTuDong
            // 
            this.mCanTuDong.ActiveControl = null;
            this.mCanTuDong.Location = new System.Drawing.Point(760, 370);
            this.mCanTuDong.Name = "mCanTuDong";
            this.mCanTuDong.Size = new System.Drawing.Size(202, 104);
            this.mCanTuDong.TabIndex = 0;
            this.mCanTuDong.Text = "Cân tự động";
            this.mCanTuDong.UseSelectable = true;
            this.mCanTuDong.Click += new System.EventHandler(this.mCanTuDong_Click);
            // 
            // mCanBanTuDong
            // 
            this.mCanBanTuDong.ActiveControl = null;
            this.mCanBanTuDong.Location = new System.Drawing.Point(1067, 370);
            this.mCanBanTuDong.Name = "mCanBanTuDong";
            this.mCanBanTuDong.Size = new System.Drawing.Size(202, 104);
            this.mCanBanTuDong.TabIndex = 0;
            this.mCanBanTuDong.Text = "Cân bán tự động";
            this.mCanBanTuDong.UseSelectable = true;
            this.mCanBanTuDong.Click += new System.EventHandler(this.mCanBanTuDong_Click);
            // 
            // UCMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mCanBanTuDong);
            this.Controls.Add(this.mCanTuDong);
            this.Name = "UCMain";
            this.Size = new System.Drawing.Size(1950, 900);
            this.Load += new System.EventHandler(this.UCMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroTile mCanTuDong;
        private MetroFramework.Controls.MetroTile mCanBanTuDong;
    }
}
