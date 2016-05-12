using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows; // Application
using System.Net;
using System.Collections.Specialized;

namespace UploadClientForStudent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string UPLOAD_URL = ""; //JettyES Server Upload URL

        private void Form1_Load(object sender, EventArgs e)
        {
            SchoolComboBox.SelectedIndex = 0; //To Selected "中庄國中"

            //file selected filter
            openFileDialog_ZIP.Filter = "Compress File(*.zip;*.rar)|*.zip;*.rar;";
            openFileDialog_ZIP.FilterIndex = 1;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        public string filePath = @"";
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog_ZIP.FileName = "";
            if (openFileDialog_ZIP.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog_ZIP.FileName;
                //change text in form
                label_file_path.Text = filePath;
            }
        }

        private bool checkFormIsDone()
        {
            if (FullName.Text.Length >= 2 && PhoneNumber.Text.Length >= 9 && ClassName.Text.Length > 2 && filePath.Length > 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void KeyPress_Only_Number(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void UploadFileClick(object sender, EventArgs e)
        {
            if (checkFormIsDone())
            {
                string _FullName = FullName.Text;
                string _PhoneNumber = PhoneNumber.Text;
                string _SchoolName = SchoolComboBox.SelectedItem.ToString();
                string _ClassName = ClassName.Text;

                
                WebClient myWebClient = new WebClient();
                myWebClient.Proxy = System.Net.WebRequest.DefaultWebProxy;
                myWebClient.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                myWebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("FullName", _FullName);
                parameters.Add("PhoneNumber", _PhoneNumber);
                parameters.Add("SchoolName", _SchoolName);
                parameters.Add("ClassName", _ClassName);
                myWebClient.QueryString = parameters;
                var responseBytes = myWebClient.UploadFile(UPLOAD_URL, filePath);
                string response = Encoding.ASCII.GetString(responseBytes);


                MessageBox.Show("進行上傳");
            }
            else
            {
                MessageBox.Show("表單尚未完成!");
            }
        }
    }
}
