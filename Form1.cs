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
using System.IO;

namespace UploadClientForStudent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string UPLOAD_URL = "http://127.0.0.1/MyUploadFile"; //JettyES Server Upload URL
        public static string FACEBOOK_URL = "http://facebook.com/";

        private void Form1_Load(object sender, EventArgs e)
        {
            SchoolComboBox.SelectedIndex = 0; //To Selected "中庄國中"
            SchoolComboBox.DropDownStyle = ComboBoxStyle.DropDownList; //Lock Edit

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


                /*WebClient myWebClient = new WebClient();
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
                string response = Encoding.ASCII.GetString(responseBytes);*/
                WebClient myWebClient = new WebClient();
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("FullName", _FullName);
                parameters.Add("PhoneNumber", _PhoneNumber);
                parameters.Add("SchoolName", _SchoolName);
                parameters.Add("ClassName", _ClassName);
                myWebClient.QueryString = parameters;
                myWebClient.UploadProgressChanged += new UploadProgressChangedEventHandler(UploadProgressCallback);
                myWebClient.UploadFileAsync(new Uri(UPLOAD_URL), filePath);
                //byte[] responseArray = myWebClient.UploadFile(UPLOAD_URL, filePath);
            }
            else
            {
                MessageBox.Show("表單尚未完成!");
            }
        }

        static int dv = 0; //because 100 percent show two times
        private void UploadProgressCallback(object sender, UploadProgressChangedEventArgs e)
        {
            // Displays the operation identifier, and the transfer progress.
            Console.WriteLine("{0}    uploaded {1} of {2} bytes. {3} % complete...",
                (string)e.UserState,
                e.BytesSent,
                e.TotalBytesToSend,
                e.ProgressPercentage);
            progressBar1.Value = e.ProgressPercentage;
            if(e.ProgressPercentage == 100)
            {
                dv++;
                if (dv == 2) {
                    string link = "http://163.15.198.172/" + PhoneNumber.Text + "-" + FullName.Text;
                    MessageBox.Show("請您把您作品的網址記下來，我們將在今日晚上到一週內公開於網站給您下載: " + Environment.NewLine + link);
                    DialogResult result = MessageBox.Show("上傳已完成，是否要關閉程式?", "上傳完成 - FotechCSIE - Student Visit Client Dialog",MessageBoxButtons.OKCancel,MessageBoxIcon.Information,MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.OK)
                    {
                        MessageBox.Show("感謝您本次在和春科大的支持，也歡迎您到我們的粉絲專頁按讚!");
                        Application.Exit();
                    }
                    dv = 0;
                }
            }
        }

        private void GOURL(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.Start(FACEBOOK_URL);
        }
    }
}
