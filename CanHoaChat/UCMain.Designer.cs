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
            this.components = new System.ComponentModel.Container();
            this.mCanBanTuDong = new MetroFramework.Controls.MetroTile();
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.metroTile2 = new MetroFramework.Controls.MetroTile();
            this.metroTile3 = new MetroFramework.Controls.MetroTile();
            this.pCheckNV = new System.Windows.Forms.Panel();
            this.lbError = new MetroFramework.Controls.MetroLabel();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.txtQRNV = new MetroFramework.Controls.MetroTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pCheckNV.SuspendLayout();
            this.SuspendLayout();
            // 
            // mCanBanTuDong
            // 
            this.mCanBanTuDong.ActiveControl = null;
            this.mCanBanTuDong.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mCanBanTuDong.AutoSize = true;
            this.mCanBanTuDong.Location = new System.Drawing.Point(848, 272);
            this.mCanBanTuDong.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mCanBanTuDong.Name = "mCanBanTuDong";
            this.mCanBanTuDong.Size = new System.Drawing.Size(243, 83);
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
            this.metroTile1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroTile1.Location = new System.Drawing.Point(589, 272);
            this.metroTile1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(253, 83);
            this.metroTile1.TabIndex = 0;
            this.metroTile1.Text = "Mở bồn hóa chất";
            this.metroTile1.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTile1.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTile1.UseSelectable = true;
            this.metroTile1.Click += new System.EventHandler(this.mCanTuDong_Click);
            // 
            // metroTile2
            // 
            this.metroTile2.ActiveControl = null;
            this.metroTile2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroTile2.AutoSize = true;
            this.metroTile2.Location = new System.Drawing.Point(848, 359);
            this.metroTile2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.metroTile2.Name = "metroTile2";
            this.metroTile2.Size = new System.Drawing.Size(243, 83);
            this.metroTile2.TabIndex = 0;
            this.metroTile2.Text = "Cân bán tự động 2";
            this.metroTile2.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTile2.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTile2.UseSelectable = true;
            this.metroTile2.Click += new System.EventHandler(this.metroTile2_Click);
            // 
            // metroTile3
            // 
            this.metroTile3.ActiveControl = null;
            this.metroTile3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.metroTile3.Location = new System.Drawing.Point(589, 359);
            this.metroTile3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.metroTile3.Name = "metroTile3";
            this.metroTile3.Size = new System.Drawing.Size(253, 83);
            this.metroTile3.TabIndex = 0;
            this.metroTile3.Text = "Lịch sử cân";
            this.metroTile3.TileTextFontSize = MetroFramework.MetroTileTextSize.Tall;
            this.metroTile3.TileTextFontWeight = MetroFramework.MetroTileTextWeight.Bold;
            this.metroTile3.UseSelectable = true;
            this.metroTile3.Click += new System.EventHandler(this.metroTile3_Click);
            // 
            // pCheckNV
            // 
            this.pCheckNV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pCheckNV.Controls.Add(this.lbError);
            this.pCheckNV.Controls.Add(this.metroButton1);
            this.pCheckNV.Controls.Add(this.txtQRNV);
            this.pCheckNV.Location = new System.Drawing.Point(589, 319);
            this.pCheckNV.Name = "pCheckNV";
            this.pCheckNV.Size = new System.Drawing.Size(733, 292);
            this.pCheckNV.TabIndex = 1;
            this.pCheckNV.Visible = false;
            this.pCheckNV.VisibleChanged += new System.EventHandler(this.pCheckNV_VisibleChanged);
            // 
            // lbError
            // 
            this.lbError.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.lbError.ForeColor = System.Drawing.Color.Red;
            this.lbError.Location = new System.Drawing.Point(139, 152);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(452, 40);
            this.lbError.TabIndex = 2;
            this.lbError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lbError.UseCustomBackColor = true;
            this.lbError.UseCustomForeColor = true;
            // 
            // metroButton1
            // 
            this.metroButton1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.metroButton1.FontSize = MetroFramework.MetroButtonSize.Tall;
            this.metroButton1.Location = new System.Drawing.Point(314, 206);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(124, 38);
            this.metroButton1.TabIndex = 1;
            this.metroButton1.Text = "Hủy";
            this.metroButton1.UseCustomBackColor = true;
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // txtQRNV
            // 
            // 
            // 
            // 
            this.txtQRNV.CustomButton.Image = null;
            this.txtQRNV.CustomButton.Location = new System.Drawing.Point(241, 1);
            this.txtQRNV.CustomButton.Name = "";
            this.txtQRNV.CustomButton.Size = new System.Drawing.Size(35, 35);
            this.txtQRNV.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtQRNV.CustomButton.TabIndex = 1;
            this.txtQRNV.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtQRNV.CustomButton.UseSelectable = true;
            this.txtQRNV.CustomButton.Visible = false;
            this.txtQRNV.Lines = new string[0];
            this.txtQRNV.Location = new System.Drawing.Point(237, 100);
            this.txtQRNV.MaxLength = 5;
            this.txtQRNV.Multiline = true;
            this.txtQRNV.Name = "txtQRNV";
            this.txtQRNV.PasswordChar = '\0';
            this.txtQRNV.PromptText = "Quét mã nhân viên";
            this.txtQRNV.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtQRNV.SelectedText = "";
            this.txtQRNV.SelectionLength = 0;
            this.txtQRNV.SelectionStart = 0;
            this.txtQRNV.ShortcutsEnabled = true;
            this.txtQRNV.Size = new System.Drawing.Size(277, 37);
            this.txtQRNV.TabIndex = 0;
            this.txtQRNV.UseSelectable = true;
            this.txtQRNV.WaterMark = "Quét mã nhân viên";
            this.txtQRNV.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtQRNV.WaterMarkFont = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtQRNV.TextChanged += new System.EventHandler(this.txtQRNV_TextChanged);
            this.txtQRNV.Click += new System.EventHandler(this.txtQRNV_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
            // 
            // UCMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.pCheckNV);
            this.Controls.Add(this.metroTile3);
            this.Controls.Add(this.metroTile1);
            this.Controls.Add(this.metroTile2);
            this.Controls.Add(this.mCanBanTuDong);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCMain";
            this.Size = new System.Drawing.Size(1734, 801);
            this.Load += new System.EventHandler(this.UCMain_Load);
            this.pCheckNV.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroTile mCanBanTuDong;
        private MetroFramework.Controls.MetroTile metroTile1;
        private MetroFramework.Controls.MetroTile metroTile2;
        private MetroFramework.Controls.MetroTile metroTile3;
        private System.Windows.Forms.Panel pCheckNV;
        private MetroFramework.Controls.MetroButton metroButton1;
        private MetroFramework.Controls.MetroTextBox txtQRNV;
        private MetroFramework.Controls.MetroLabel lbError;
        private System.Windows.Forms.Timer timer1;
    }
}
