using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.FSharp.Collections;

namespace AutoPopSqlGenerator
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string issueNum = this.tbIssueNum.Text;
            if (issueNum == null || issueNum == string.Empty)
            {
                MessageBox.Show("Please enter an issue number.");
                return;
            }

            string path = this.tbAutopopPathEntry.Text;
            if (path == null || path == string.Empty)
            {
                MessageBox.Show("Please enter an Autopop Path.");
                return;
            }

            List<string[]> responses = new List<string[]>();
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                string[] temp = new string[3];
                int i = 0;
                foreach(DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null)
                    {
                        temp[i] = cell.Value.ToString();
                    }
                    else
                    {
                        temp[i] = "";
                    }
                    i++;
                }

                if (temp[0] != ""
                        || temp[1] != ""
                            || temp[2] != "")
                {
                    responses.Add(temp);
                }
            }

            string sqlPathScript = AutoPopSqlGeneratorFSharp.SqlGenerators.GenerateNodePathSql(path);
            string oraclePathScript = AutoPopSqlGeneratorFSharp.SqlGenerators.GenerateNodePathOracle(path);
            string sqlInsertResponseScript = null;
            string oracleInsertResponseScript = null;
            if (responses.Count > 0)
            {
                sqlInsertResponseScript = AutoPopSqlGeneratorFSharp.SqlGenerators.GenerateResponseInsertSql(path, SeqModule.ToList(responses));
                oracleInsertResponseScript = AutoPopSqlGeneratorFSharp.SqlGenerators.GenerateResponseInsertOracle(path, SeqModule.ToList(responses));
            } 

            if (sqlInsertResponseScript != null)
            {
                sqlPathScript += sqlInsertResponseScript;
            }

            if (oracleInsertResponseScript != null)
            {
                oraclePathScript += oracleInsertResponseScript;
            }

            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowDialog();
            var folderPath = folderBrowser.SelectedPath;

            StreamWriter sw = new StreamWriter(folderPath + "\\" + issueNum + "_S_Autopopulate.sql");
            sw.WriteLine(sqlPathScript);
            sw.Close();

            sw = new StreamWriter(folderPath + "\\" + issueNum + "_O_Autopopulate.sql");
            sw.WriteLine(oraclePathScript);
            sw.Close();

            MessageBox.Show("Export success!");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
