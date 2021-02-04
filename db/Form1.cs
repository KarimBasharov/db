using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer;

namespace db
{
    public partial class Form1 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\karim\source\repos\db\db\Database1.mdf; Integrated Security = True");
        SqlCommand command;
        SqlDataAdapter adapter, adapter2;
        SaveFileDialog save;
        int Id = 0;
        int lastId;
        DateTime dob;
        public bool vanMail;
        bool vanemCheck;

        DataGridView vane = new DataGridView();
        public string parentEMail;
        public Form1()
        {
            InitializeComponent();
            DisplayData();
            vane.Size = new Size(963, 184);
            vane.Location = new Point(12, 12);
            LastId();
        }
        private BindingSource bindingSource = new BindingSource();
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                /*command = new SqlCommand("SELECT * FROM opilane WHERE GruppId = @grupp", connect);
                connect.Open();
                command.Parameters.AddWithValue("@grupp", comboBox1.SelectedIndex + 1);
                command.ExecuteNonQuery();
                connect.Close();*/
                DataTable table = new DataTable();
                adapter = new SqlDataAdapter();
                command = new SqlCommand("SELECT * FROM opilane WHERE GruppId = @grupp", connect);
                command.Parameters.AddWithValue("@grupp", SqlDbType.Int).Value = comboBox1.SelectedIndex + 1;
                adapter.SelectCommand = command;
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
        private void LastId()
        {
            connect.Open();
            command = new SqlCommand("SELECT Id FROM opilane ORDER BY Id DESC", connect);
            lastId = (Int32)command.ExecuteScalar();
            connect.Close();
        }
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                name.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                surname.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                address.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                email.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                number.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                pictureBox1.Image = Image.FromFile(@"C:\Users\karim\source\repos\db\db\foto\" + dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString());
                string v = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                comboBox1.SelectedIndex = Int32.Parse(v) - 1;

                syniaeg.Value = (DateTime)dataGridView1.Rows[e.RowIndex].Cells[8].Value;
                dob = (DateTime)dataGridView1.Rows[e.RowIndex].Cells[8].Value;
            }
            catch(Exception)
            {
                name.Text = "";
                surname.Text = "";
                address.Text = "";
                email.Text = "";
                number.Text = "";
                pictureBox1.Image = null;
                comboBox1.SelectedIndex = 0;
            }
        }

        private void lisa_button_Click(object sender, EventArgs e)
        {
            if(name.Text != "" && surname.Text != "" && address.Text != "" && number.Text != "" && syniaeg.Text != "" && email.Text != "" && comboBox1.SelectedItem != null && vanemCheck == false && email.Text.Contains("@"))
            {
                try
                {
                    command = new SqlCommand("INSERT INTO opilane(Nimi, Perekonnanimi, Adress, Number, Email, Foto, GruppId, Vanus) VALUES (@name,@surname,@adress,@number,@email,@photo,@group,@vanus)", connect);
                    connect.Open();
                    command.Parameters.AddWithValue("@name", name.Text);
                    command.Parameters.AddWithValue("@surname", surname.Text);
                    command.Parameters.AddWithValue("@adress", address.Text);
                    command.Parameters.AddWithValue("@number", number.Text);
                    command.Parameters.AddWithValue("@email", email.Text);
                    string file_pilt = name.Text + (lastId + 1) + ".jpg";
                    command.Parameters.AddWithValue("@photo", file_pilt);
                    command.Parameters.AddWithValue("@group", (comboBox1.SelectedIndex + 1));
                    command.Parameters.AddWithValue("@vanus", syniaeg.Value);
                    command.ExecuteNonQuery();
                    connect.Close();
                    comboBox1.Items.Clear();
                    DisplayData();
                    ClearData();
                    LastId();
                    MessageBox.Show("Added to the database");
                }

                catch(Exception)
                {
                    MessageBox.Show("Error!");
                }
            }

            else if (name.Text != "" && surname.Text != "" && email.Text != "" && vanemCheck == true)
            {
                command = new SqlCommand("INSERT INTO vanemad(Nimi,Perekonnanimi,Email,Opilane) VALUES (@name,@sur,@mail,@opilan)", connect);
                connect.Open();
                command.Parameters.AddWithValue("@name", name.Text);
                command.Parameters.AddWithValue("@sur", surname.Text);
                command.Parameters.AddWithValue("@mail", email.Text);
                command.Parameters.AddWithValue("@opilan", Id);
                command.ExecuteNonQuery();
                connect.Close();
                comboBox1.Items.Clear();
                DisplayParent();
                ClearData();
                MessageBox.Show("Andmed on lisatud!");
            }

            else
            {
                MessageBox.Show("Error");
            }
        }
        private void ClearData()
        {
            name.Text = "";
            surname.Text = "";
            number.Text = "";
            email.Text = "";
            address.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog OP = new OpenFileDialog();
            OP.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
            OP.InitialDirectory = Path.GetFullPath(@"C:\Users\karim\Desktop");
            if (OP.ShowDialog() == DialogResult.OK)
            {
                save = new SaveFileDialog();
                save.FileName = name.Text + ".jpg";
                save.Filter = "Image Files(*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
                save.InitialDirectory = Path.GetFullPath(@"C:\Users\karim\source\repos\db\db\foto\");

                if (save.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(OP.FileName, save.FileName);
                    save.RestoreDirectory = true;
                    pictureBox1.Image = Image.FromFile(save.FileName);
                }

            }
        }
        private void update_Click(object sender, EventArgs e)
        {
            if (name.Text != "" && surname.Text != "" && address.Text != "" && number.Text != "" && email.Text != "" && pictureBox1.Image != null)
            {
                command = new SqlCommand("UPDATE opilane SET Nimi=@name,Perekonnanimi=@surname, Email=@email, Foto=@photo, Adress=@adress, Number=@number, Vanus = @vanus WHERE Id=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@name", name.Text);
                command.Parameters.AddWithValue("@surname", surname.Text);
                command.Parameters.AddWithValue("@email", email.Text);
                command.Parameters.AddWithValue("@number", number.Text);
                command.Parameters.AddWithValue("@adress", address.Text);
                string file_pilt = name.Text + ".jpg";
                command.Parameters.AddWithValue("@photo", file_pilt);
                command.Parameters.AddWithValue("@grupp", comboBox1.SelectedIndex + 1);
                command.Parameters.AddWithValue("@vanus", syniaeg.Value);
                command.ExecuteNonQuery();
                connect.Close();
                comboBox1.Items.Clear();
                DisplayData();
                ClearData();
                MessageBox.Show("Database updated");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (Id != 0)
            {
                command = new SqlCommand("DELETE opilane WHERE Id=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", Id);
                command.ExecuteNonQuery();
                connect.Close();
                comboBox1.Items.Clear();
                DisplayData();
                ClearData();
                MessageBox.Show("Data deleted");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void sort_Click(object sender, EventArgs e)
        {
            comboBox1_SelectedIndexChanged(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true && vanMail == true)
            {
                Form3 parents = new Form3(email.Text);
                parents.Show();
            }
            else
            {
                Form2 mailll = new Form2();
                mailll.Show();
            }
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            int age = DateTime.Today.Year - syniaeg.Value.Year;

            if (checkBox1.Checked == true && name.Text != "" /*&& syniaeg.Value.AddYears(age) > DateTime.Now*/)
            {
                if (age >= 18)
                {
                    MessageBox.Show("Õpilasel on juba 18 aastat!");
                    checkBox1.Checked = false;
                }
                else
                {
                    vanemCheck = true;
                    address.Enabled = false;
                    number.Enabled = false;
                    comboBox1.Enabled = false;
                    syniaeg.Enabled = false;

                    dataGridView2.BringToFront();
                    DisplayParent();
                    comboBox1.Items.Clear();
                    vanMail = true;
                }
            }
            else
            {
                address.Enabled = true;
                number.Enabled = true;
                comboBox1.Enabled = true;
                syniaeg.Enabled = true;
                vanemCheck = false;
                dataGridView2.SendToBack();
                checkBox1.Checked = false;
                vanMail = false;
                ClearData();
                DisplayData();
            }

            //DateTime today = DateTime.Today;
            //int age = today.Year - dob.Year;

            //if (checkBox1.Checked == true && name.Text != "")
            //{


            //    if (age >= 18)
            //    {
            //        MessageBox.Show("Õpilasel on juba 18 aastat!");
            //        checkBox1.Checked = false;
            //    }
            //    else
            //    {
            //        vanemCheck = true;
            //        address.Enabled = false;
            //        number.Enabled = false;
            //        comboBox1.Enabled = false;
            //        syniaeg.Enabled = false;

            //        dataGridView2.BringToFront();
            //        DisplayParent();
            //        comboBox1.Items.Clear();
            //        vanMail = true;
            //    }


            //}
            //else
            //{
            //    address.Enabled = true;
            //    number.Enabled = true;
            //    comboBox1.Enabled = true;
            //    syniaeg.Enabled = true;
            //    vanemCheck = false;
            //    dataGridView2.SendToBack();
            //    checkBox1.Checked = false;
            //    vanMail = false;
            //    ClearData();
            //    DisplayData();
            //}
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ClearData();
            name.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            surname.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            email.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void DisplayData()
        {
            connect.Open();
            DataTable table = new DataTable();
            adapter = new SqlDataAdapter("SELECT * FROM opilane", connect);
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            adapter2 = new SqlDataAdapter("SELECT GruppNim FROM gruppid", connect);
            DataTable grupp_table = new DataTable();
            adapter2.Fill(grupp_table);
            foreach (DataRow row in grupp_table.Rows)
            {
                comboBox1.Items.Add(row["GruppNim"]);
            }
            connect.Close();
        }
        private void DisplayParent()
        {
            connect.Open();
            DataTable table = new DataTable();
            adapter2 = new SqlDataAdapter("SELECT * FROM vanemad", connect);
            command = new SqlCommand("SELECT * FROM vanemad WHERE Opilane = @opId", connect);
            command.Parameters.Add("@opId", SqlDbType.Int).Value = Id;
            adapter2.SelectCommand = command;
            adapter2.Fill(table);
            dataGridView2.DataSource = table;

            connect.Close();
        }
    }
}
