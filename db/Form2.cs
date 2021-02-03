using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace db
{
    public partial class Form2 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\karim\source\repos\db\db\Database1.mdf; Integrated Security = True");
        SqlDataAdapter adapter;
        List<string> avaldused;
        OpenFileDialog opFile;
        MailMessage message;
        MailAddress to, from;
        int i;
        string fileName, filePath;
        public Form2()
        {
            InitializeComponent();
            Mail();
            avaldused = new List<string>();
        }
        private void Mail()
        {
            DataTable table = new DataTable();
            adapter = new SqlDataAdapter("SELECT Email FROM opilane", connect);
            adapter.Fill(table);
            foreach (DataRow row in table.Rows)
            {
                comboBox1.Items.Add(row["Email"]);
            }
            connect.Close();
        }

        private void send_Click(object sender, EventArgs e)
        {
            avaldused.Add("Avaldus" + ".pdf");

            string address = comboBox1.SelectedItem.ToString();
            to = new MailAddress(address);
            from = new MailAddress("prorokadolbobeka@gmail.com");
            message = new MailMessage(from, to);

            message.Subject = textBox1.Text;
            message.Body = textBox2.Text;

            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("prorokadolbobeka@gmail.com", ""),
                EnableSsl = true
            };
            if (opFile != null && checkBox1.Checked == true)
            {
                foreach (var item in avaldused)
                {
                    message.Attachments.Add(new Attachment(item));
                }
            }
            else if (opFile != null)
            {
                foreach (string filePath in opFile.FileNames)
                {
                    if (File.Exists(filePath))
                    {
                        message.Attachments.Add(new Attachment(filePath));
                    }
                }
            }
            else if (checkBox1.Checked == true)
            {
                foreach (var item in avaldused)
                {
                    message.Attachments.Add(new Attachment(item));
                }
            }
            client.Send(message);
            MessageBox.Show("Письмо отправленно на " + address);
            Clear();
        }

        private void file_Click(object sender, EventArgs e)
        {
            opFile = new OpenFileDialog();
            opFile.ShowDialog();

            foreach (string filePath in opFile.FileNames)
            {
                if (File.Exists(filePath))
                {
                    avaldused.Add(filePath);
                    fileName = Path.GetFileName(filePath);
                    label5.Text = fileName;
                }
            }
        }


        private void Clear()
        {
            checkBox1.Checked = false;
            comboBox1.SelectedItem = null;
            textBox2.Text = "";
            textBox1.Text = "";
            avaldused.Clear();
        }
    }
}