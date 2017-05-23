namespace AutoPopSqlGenerator
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbAutopopPathEntry = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.AutopopNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutopopResponseCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AutopopAddlResponseCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.tbIssueNum = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Autpop Path (example: MN > Documents > IEPMeetingNotice) :";
            // 
            // tbAutopopPathEntry
            // 
            this.tbAutopopPathEntry.Location = new System.Drawing.Point(125, 25);
            this.tbAutopopPathEntry.Name = "tbAutopopPathEntry";
            this.tbAutopopPathEntry.Size = new System.Drawing.Size(530, 20);
            this.tbAutopopPathEntry.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(12, 553);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AutopopNameCol,
            this.AutopopResponseCol,
            this.AutopopAddlResponseCol});
            this.dataGridView1.Location = new System.Drawing.Point(12, 51);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(643, 496);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // AutopopNameCol
            // 
            this.AutopopNameCol.HeaderText = "Name";
            this.AutopopNameCol.Name = "AutopopNameCol";
            // 
            // AutopopResponseCol
            // 
            this.AutopopResponseCol.HeaderText = "Response";
            this.AutopopResponseCol.MinimumWidth = 250;
            this.AutopopResponseCol.Name = "AutopopResponseCol";
            this.AutopopResponseCol.Width = 250;
            // 
            // AutopopAddlResponseCol
            // 
            this.AutopopAddlResponseCol.HeaderText = "Addl Response";
            this.AutopopAddlResponseCol.MinimumWidth = 250;
            this.AutopopAddlResponseCol.Name = "AutopopAddlResponseCol";
            this.AutopopAddlResponseCol.Width = 250;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Enter Issue Number:";
            // 
            // tbIssueNum
            // 
            this.tbIssueNum.Location = new System.Drawing.Point(16, 25);
            this.tbIssueNum.Name = "tbIssueNum";
            this.tbIssueNum.Size = new System.Drawing.Size(100, 20);
            this.tbIssueNum.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 588);
            this.Controls.Add(this.tbIssueNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.tbAutopopPathEntry);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Autopopulate Sql Generator";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbAutopopPathEntry;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AutopopNameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AutopopResponseCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AutopopAddlResponseCol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbIssueNum;
    }
}

