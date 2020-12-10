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
            this.mCanBanTuDong = new MetroFramework.Controls.MetroTile();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.SuspendLayout();
            // 
            // mCanBanTuDong
            // 
            this.mCanBanTuDong.ActiveControl = null;
            this.mCanBanTuDong.Location = new System.Drawing.Point(762, 299);
            this.mCanBanTuDong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mCanBanTuDong.Name = "mCanBanTuDong";
            this.mCanBanTuDong.Size = new System.Drawing.Size(180, 83);
            this.mCanBanTuDong.TabIndex = 0;
            this.mCanBanTuDong.Text = "Cân bán tự động";
            this.mCanBanTuDong.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.mCanBanTuDong.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.mCanBanTuDong.UseSelectable = true;
            this.mCanBanTuDong.Click += new System.EventHandler(this.mCanBanTuDong_Click);
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Location = new System.Drawing.Point(533, 299);
            this.metroTile1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(180, 83);
            this.metroTile1.TabIndex = 0;
            this.metroTile1.Text = "Mở bồn hóa chất";
            this.metroTile1.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTile1.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTile1.UseSelectable = true;
            this.metroTile1.Click += new System.EventHandler(this.mCanTuDong_Click);
            // 
            // UCMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.metroTile1);
            this.Controls.Add(this.mCanBanTuDong);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCMain";
            this.Size = new System.Drawing.Size(1733, 720);
            this.Load += new System.EventHandler(this.UCMain_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private MetroFramework.Controls.MetroTile mCanBanTuDong;
        private MetroFramework.Controls.MetroTile metroTile1;
    }
}
