using System;
using System.Windows.Forms;

namespace WindowsLib {
    public partial class ScheduleWindow : Form {
        public ScheduleWindow() {
            InitializeComponent();
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                foreach (Schedule schedule in dbContext.Schedules) {
                    foreach (Employee employee in dbContext.Employees) {
                        if (employee.Employee_Id == schedule.Employee_Id) {
                            ScheduleGridView.Rows.Add(schedule.DayName, employee.Name, schedule.WorkTime, schedule.Day_Id);
                        }
                    }
                }
            }
        }

        private void ScheduleOKButton_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
