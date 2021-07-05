namespace CanHoaChat
{
    partial class UCLichSuCan
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ManufactureOrderNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SoThe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChemicalOrderCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChemicalOrder2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Weight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.intime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.indat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.username = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(19, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(444, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lịch sử cân trong ngày";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.ColumnHeadersHeight = 60;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ManufactureOrderNo,
            this.SoThe,
            this.ChemicalOrderCode,
            this.ChemicalOrder2,
            this.BatchNo,
            this.Weight,
            this.intime,
            this.indat,
            this.username,
            this.status});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Location = new System.Drawing.Point(25, 118);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.Size = new System.Drawing.Size(2070, 806);
            this.dataGridView1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ManufactureOrderNo
            // 
            this.ManufactureOrderNo.HeaderText = "Mã lệnh";
            this.ManufactureOrderNo.MinimumWidth = 6;
            this.ManufactureOrderNo.Name = "ManufactureOrderNo";
            this.ManufactureOrderNo.ReadOnly = true;
            // 
            // SoThe
            // 
            this.SoThe.HeaderText = "Số thẻ";
            this.SoThe.MinimumWidth = 6;
            this.SoThe.Name = "SoThe";
            this.SoThe.ReadOnly = true;
            // 
            // ChemicalOrderCode
            // 
            this.ChemicalOrderCode.HeaderText = "Mã keo";
            this.ChemicalOrderCode.MinimumWidth = 6;
            this.ChemicalOrderCode.Name = "ChemicalOrderCode";
            this.ChemicalOrderCode.ReadOnly = true;
            // 
            // ChemicalOrder2
            // 
            this.ChemicalOrder2.HeaderText = "Mã keo 2";
            this.ChemicalOrder2.MinimumWidth = 6;
            this.ChemicalOrder2.Name = "ChemicalOrder2";
            this.ChemicalOrder2.ReadOnly = true;
            // 
            // BatchNo
            // 
            this.BatchNo.HeaderText = "Số hộp";
            this.BatchNo.MinimumWidth = 6;
            this.BatchNo.Name = "BatchNo";
            this.BatchNo.ReadOnly = true;
            // 
            // Weight
            // 
            this.Weight.HeaderText = "KG";
            this.Weight.MinimumWidth = 6;
            this.Weight.Name = "Weight";
            this.Weight.ReadOnly = true;
            // 
            // intime
            // 
            this.intime.HeaderText = "Giờ làm";
            this.intime.MinimumWidth = 6;
            this.intime.Name = "intime";
            this.intime.ReadOnly = true;
            // 
            // indat
            // 
            this.indat.HeaderText = "Ngày làm";
            this.indat.MinimumWidth = 6;
            this.indat.Name = "indat";
            this.indat.ReadOnly = true;
            // 
            // username
            // 
            this.username.HeaderText = "Người làm";
            this.username.MinimumWidth = 6;
            this.username.Name = "username";
            this.username.ReadOnly = true;
            // 
            // status
            // 
            this.status.HeaderText = "Trạng thái";
            this.status.MinimumWidth = 6;
            this.status.Name = "status";
            this.status.ReadOnly = true;
            // 
            // UCLichSuCan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Name = "UCLichSuCan";
            this.Size = new System.Drawing.Size(2100, 791);
            this.Load += new System.EventHandler(this.UCLichSuCan_Load);
            this.MouseHover += new System.EventHandler(this.UCLichSuCan_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ManufactureOrderNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SoThe;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChemicalOrderCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChemicalOrder2;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Weight;
        private System.Windows.Forms.DataGridViewTextBoxColumn intime;
        private System.Windows.Forms.DataGridViewTextBoxColumn indat;
        private System.Windows.Forms.DataGridViewTextBoxColumn username;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
    }
}
