using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace db
{
    public partial class Form3 : Form
    {
        Form1 f = new Form1();
        string mail;
        public Form3(string mailTxt)
        {
            InitializeComponent();
            this.mail = mailTxt;
        }
        public delegate void PassControl(object sender);

        private void ClearEverything()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MailAddress to = new MailAddress(mail);
            MailAddress from = new MailAddress("---");
            MailMessage message = new MailMessage(from, to);

            message.Subject = textBox1.Text;
            message.Body = textBox2.Text;

            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("---", "---"),
                EnableSsl = true
            };
            client.Send(message);
            MessageBox.Show("Письмо было отправленно родителю на почту ");
            ClearEverything();
        }
    }
}
