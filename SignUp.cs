using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace WindowsLib
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
            for (int i = 0; i < 31; i++) {
                comboBoxDate.Items.Add(i + 1);
            }
            for (int i = 0; i < 12; i++) {
                comboBoxMonth.Items.Add(i + 1);
            }
            for (int i = 1940; i < 2020; i++) {
                comboBoxYear.Items.Add(i + 1);
            }
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void SignUp_Load(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) {
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                Reader reader = new Reader();
                List<Reader> readers = dbContext.Readers.ToList();
                string loginPattern = @"[A-Z]{2}[-]{1}\b\d{6}\b";
                string phoneNumberPattern = @"[+]{1}\b\d{12}\b";
                bool isLoginExhist = false;
                bool isPasswordMatches = true;
                foreach (Reader match in readers) {
                    if (match.CardNumber == textBox6.Text)
                        isLoginExhist = true;
                }
                if (!Regex.IsMatch(textBox1.Text, phoneNumberPattern)) {
                    MessageBox.Show("Incorect form of phone number", "Sign up error");
                    return;
                }
                if (!Regex.IsMatch(textBox6.Text, loginPattern)) {
                    MessageBox.Show("Invalid readers card identification", "Sign up error");
                    return;
                }
                if (isLoginExhist) {
                    MessageBox.Show("This readers card is already registered", "Sign up error");
                    return;
                }
                if (textBox3.Text != textBox4.Text) {
                    MessageBox.Show("Passwords aren`t matches", "Sign up error");
                    isPasswordMatches = false;
                    return;
                }
                if (isLoginExhist == false && isPasswordMatches == true && Regex.IsMatch(textBox6.Text, loginPattern)) {
                    reader.Name = textBox2.Text;
                    reader.PhoneNumber = textBox1.Text;
                    DateTime birthDate = DateTime.Parse(comboBoxDate.Text + " " + comboBoxMonth.Text + " " + comboBoxYear.Text);
                    reader.BirthDate = birthDate;
                    reader.CardNumber = textBox6.Text;
                    reader.Password = textBox4.Text;
                    reader.DateOfSigningIn = DateTime.Now;
                    dbContext.Readers.Add(reader);
                    dbContext.SaveChanges();
                    textBox2.Clear();
                    textBox1.Clear();
                    textBox6.Clear();
                    textBox4.Clear();
                    textBox3.Clear();
                    MessageBox.Show("You were registered successfully", "Congratulations!", MessageBoxButtons.OK);
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            textBox3.UseSystemPasswordChar = !checkBox1.Checked;
            textBox4.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}
