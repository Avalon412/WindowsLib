using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsLib
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {            
            textBox1.UseSystemPasswordChar = !checkBox1.Checked;
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            SignUp sign = new SignUp();
            sign.Show();
        }

        private void button1_Click(object sender, EventArgs e) {
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                List<Reader> readers = dbContext.Readers.ToList();
                List<Employee> employees = dbContext.Employees.ToList();
                Employee currentEmployee = null;
                Reader currentReader = null;

                if (!checkBox3.Checked) {
                    foreach (Reader reader in readers) {
                        if (textBox2.Text == reader.CardNumber && textBox1.Text == reader.Password) {
                            currentReader = reader;
                        }
                    }
                }

                if (checkBox3.Checked) {
                    foreach (Employee employee in employees) {
                        if (textBox2.Text == employee.Login && textBox1.Text == employee.Password) {
                            currentEmployee = employee;
                        }
                    }
                }

                if (currentReader != null) {
                    User user = new User();
                    user.UsersNameLable.Text = currentReader.Name;
                    User.ActiveUserId = currentReader.Reader_Id;
                    user.UsersCardNumberLable.Text = "Readers card number: " + currentReader.CardNumber;
                    user.NameLabel.Text = currentReader.Name;
                    user.CardNumberLabel.Text = currentReader.CardNumber;
                    user.PhoneNumberLabel.Text = currentReader.PhoneNumber;
                    user.BirthDateLabel.Text = "Date of birth " + currentReader.BirthDate.ToShortDateString();
                    user.DateOfSignupLabel.Text = "Date of Sign up " + currentReader.DateOfSigningIn.ToShortDateString();
                    user.StatusLabel.Text = "Reader";
                    user.ChangeDateBox.Text = currentReader.BirthDate.Day.ToString();
                    user.ChangeMonthBox.Text = currentReader.BirthDate.Month.ToString();
                    user.ChangeYearBox.Text = currentReader.BirthDate.Year.ToString();                   
                    for (int i = 0; i < 4; i++) {
                        TabPage pageToDelete = user.MyCabinetPage.TabPages[2];
                        user.MyCabinetPage.TabPages.Remove(pageToDelete);
                    }                    
                    user.Show();
                    this.Hide();
                } else if (!checkBox3.Checked) {
                    MessageBox.Show("Such reader does not exhist", "Reader not found");
                } else if (currentEmployee != null) {
                    User user = new User();
                    user.UsersNameLable.Text = currentEmployee.Name;
                    if (currentEmployee.Job_Id == 2) user.UsersCardNumberLable.Text = "Librarian";
                    if (currentEmployee.Job_Id == 1) user.UsersCardNumberLable.Text = "Head Librarian";
                    User.ActiveUserId = currentEmployee.Employee_Id;
                    user.WorkerNameLabel.Text = currentEmployee.Name;
                    user.WorkerPhoneLabel.Text = currentEmployee.PhoneNumber;                    
                    if (currentEmployee.Job_Id == 2) user.WorkerStatusLabel.Text = "Librarian";
                    if (currentEmployee.Job_Id == 1) user.WorkerStatusLabel.Text = "Head Librarian";
                    user.WorkerBirtdateLabel.Text = "Date of birth " + currentEmployee.BirthDate.ToShortDateString();
                    user.WorkerHiredateLabel.Text = "Hired " + currentEmployee.HireDate.ToShortDateString();
                    user.ChangeWDateBox.Text = currentEmployee.BirthDate.Day.ToString();
                    user.ChangeWMonthBox.Text = currentEmployee.BirthDate.Month.ToString();
                    user.ChangeWYearBox.Text = currentEmployee.BirthDate.Year.ToString();
                    TabPage pageToDelete1 = user.MyCabinetPage.TabPages[1];
                    user.MyCabinetPage.TabPages.Remove(pageToDelete1);
                    if (currentEmployee.Job_Id == 2) {
                        for (int i = 0; i < 3; i++) {
                            TabPage pageToDelete = user.MyCabinetPage.TabPages[2];
                            user.MyCabinetPage.TabPages.Remove(pageToDelete);
                        }
                    }
                    user.AddToListButton.Enabled = false;
                    user.RemoveFromListButton.Enabled = false;
                    user.SendOrderButton.Enabled = false;
                    user.OrderListGrid.Enabled = false;
                    user.label3.Enabled = false;
                    user.Show();
                    this.Hide();
                } else {
                    MessageBox.Show("Such worker does not exhist", "Worker not found");
                }

                
            }
        }
    }
}
