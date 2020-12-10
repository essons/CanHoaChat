namespace CanHoaChat
{
    partial class UCCanTuDong
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.txtQRCode = new MetroFramework.Controls.MetroTextBox();
            this.cJ2Compolet1 = new OMRON.Compolet.CIP.CJ2Compolet(this.components);
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.metroLabel1.Location = new System.Drawing.Point(440, 336);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(165, 30);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Quét mã bồn";
            // 
            // txtQRCode
            // 
            // 
            // 
            // 
            this.txtQRCode.CustomButton.Image = null;
            this.txtQRCode.CustomButton.Location = new System.Drawing.Point(235, 1);
            this.txtQRCode.CustomButton.Name = "";
            this.txtQRCode.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txtQRCode.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txtQRCode.CustomButton.TabIndex = 1;
            this.txtQRCode.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txtQRCode.CustomButton.UseSelectable = true;
            this.txtQRCode.CustomButton.Visible = false;
            this.txtQRCode.Lines = new string[0];
            this.txtQRCode.Location = new System.Drawing.Point(614, 331);
            this.txtQRCode.MaxLength = 3;
            this.txtQRCode.Name = "txtQRCode";
            this.txtQRCode.PasswordChar = '\0';
            this.txtQRCode.PromptText = "QR Code";
            this.txtQRCode.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtQRCode.SelectedText = "";
            this.txtQRCode.SelectionLength = 0;
            this.txtQRCode.SelectionStart = 0;
            this.txtQRCode.ShortcutsEnabled = true;
            this.txtQRCode.Size = new System.Drawing.Size(280, 40);
            this.txtQRCode.TabIndex = 1;
            this.txtQRCode.UseSelectable = true;
            this.txtQRCode.WaterMark = "QR Code";
            this.txtQRCode.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txtQRCode.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.txtQRCode.TextChanged += new System.EventHandler(this.txtQRCode_TextChanged);
            // 
            // cJ2Compolet1
            // 
            this.cJ2Compolet1.Active = true;
            this.cJ2Compolet1.ConnectionType = OMRON.Compolet.CIP.ConnectionType.UCMM;
            this.cJ2Compolet1.HeartBeatTimer = 0;
            this.cJ2Compolet1.LocalPort = 2;
            this.cJ2Compolet1.PeerAddress = "10.0.14.22";
            this.cJ2Compolet1.ReceiveTimeLimit = ((long)(750));
            this.cJ2Compolet1.RoutePath = "";
            this.cJ2Compolet1.UseRoutePath = false;
            // 
            // UCCanTuDong
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtQRCode);
            this.Controls.Add(this.metroLabel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCCanTuDong";
            this.Size = new System.Drawing.Size(1538, 781);
            this.Load += new System.EventHandler(this.UCCanTuDong_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroTextBox txtQRCode;
        private OMRON.Compolet.CIP.CJ2Compolet cJ2Compolet1;
    }
}
