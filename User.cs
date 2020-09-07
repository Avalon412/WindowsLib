using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsLib
{    
    public partial class User : Form
    {
        public static int ActiveUserId;

        public User()
        {
            InitializeComponent();                        
            for (int i = 0; i < 31; i++) {
                ChangeDateBox.Items.Add(i + 1);
            }
            for (int i = 0; i < 12; i++) {
                ChangeMonthBox.Items.Add(i + 1);
            }
            for (int i = 1940; i < 2020; i++) {
                ChangeYearBox.Items.Add(i + 1);
            }
            for (int i = 0; i < 31; i++) {
                ChangeWDateBox.Items.Add(i + 1);
            }
            for (int i = 0; i < 12; i++) {
                ChangeWMonthBox.Items.Add(i + 1);
            }
            for (int i = 1940; i < 2020; i++) {
                ChangeWYearBox.Items.Add(i + 1);
            }
            for (int i = 0; i < 31; i++) {
                DayCombobox.Items.Add(i + 1);
            }
            for (int i = 0; i < 12; i++) {
                MonthCombobox.Items.Add(i + 1);
            }
            for (int i = 1940; i < 2020; i++) {
                YearCombobox.Items.Add(i + 1);
            }
            DayCombobox.Text = "1";
            MonthCombobox.Text = "1";
            YearCombobox.Text = "2020";
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                foreach (Employee employee in dbContext.Employees) {
                    WorkersComboBox.Items.Add(employee.Employee_Id + " - " + employee.Name);
                }
                foreach (Schedule day in dbContext.Schedules) {
                    foreach (Employee employee in dbContext.Employees) {
                        if (employee.Employee_Id == day.Employee_Id) {
                            ScheduleGridView.Rows.Add(day.DayName, employee.Name, day.WorkTime, day.Day_Id);
                        }
                    }
                }
            }
            FormClosed += new FormClosedEventHandler(global_FormClosed);
        }

        public void global_FormClosed(object sender, EventArgs e) {
            Application.Exit();
        }

        private void ToolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {
            
        }

        private void SearchBooksButton_Click(object sender, EventArgs e) {
            List<Book> searchedBooks = new List<Book>();
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                List<Book> books = dbContext.Books.ToList();                
                foreach (var book in books) {
                    if ((book.Name.Contains(BookNameTextBox.Text) && BookNameTextBox.Text != null && BookNameTextBox.Text != "") ||
                        (book.Autor.Contains(BookAutorTextBox.Text) && BookAutorTextBox.Text != null && BookAutorTextBox.Text != "") ||
                        (book.Genre.Contains(BookGenreTextBox.Text)) && BookGenreTextBox.Text != null && BookGenreTextBox.Text != "") {
                        searchedBooks.Add(book);
                    }
                }                
            }
            SearchResultGrid.DataSource = searchedBooks;
            int cellcount = SearchResultGrid.ColumnCount - 1;
            SearchResultGrid.Columns.Remove(SearchResultGrid.Columns[cellcount]);            
        }

        List<Book> selectedBooks = new List<Book>();
        private void AddToListButton_Click(object sender, EventArgs e) {
            if (SearchResultGrid.SelectedRows.Count == 0) return;
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                List<Book> books = dbContext.Books.ToList();
                int id = Convert.ToInt32(SearchResultGrid.SelectedRows[0].Cells[0].Value);
                foreach (Book book in books) {
                    if (book.Book_Id == id) {
                        selectedBooks.Add(book);
                    }
                }
                OrderListGrid.DataSource = null;
                OrderListGrid.DataSource = selectedBooks;                
                if (OrderListGrid.ColumnCount != SearchResultGrid.ColumnCount) {
                    int cellcount = OrderListGrid.ColumnCount - 1;
                    OrderListGrid.Columns.Remove(OrderListGrid.Columns[cellcount]);
                }
                
            }
            
        }

        private void RemoveFromListButton_Click(object sender, EventArgs e) {
            if (OrderListGrid.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Are you sure you want to delete this book from the list?", "Removing confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                int id = Convert.ToInt32(OrderListGrid.SelectedRows[0].Cells[0].Value);
                Book toDelete = null;
                foreach (Book book in selectedBooks) {
                    if (book.Book_Id == id) {                    
                        toDelete = book;
                    }
                }
                selectedBooks.Remove(toDelete);
                OrderListGrid.DataSource = null;
                OrderListGrid.DataSource = selectedBooks;
            }
        }

        private void SendOrderButton_Click(object sender, EventArgs e) {
            if (OrderListGrid.Rows.Count == 0) return;
            if (MessageBox.Show("Are you sure you want to order this books?", "Order confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                    ReaderOrder order = dbContext.ReaderOrders.Add(new ReaderOrder() { Reader_Id = ActiveUserId });
                    dbContext.SaveChanges();
                    foreach (Book book in selectedBooks) {
                        dbContext.Orders.Add(new Order() { ReaderOrder_Id = order.Order_Id, Book_Id = book.Book_Id, OrderDate = DateTime.Now, IsExpired = false, IsReturned = false });
                    }
                    dbContext.SaveChanges();
                    MessageBox.Show("Your order number is " + order.Order_Id, "Order confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }
        }

        private void ShowMyBooksButton_Click(object sender, EventArgs e) {
            UsersBooksGrid.Rows.Clear();
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                List<ReaderOrder> readerOrders = new List<ReaderOrder>();
                foreach (ReaderOrder order in dbContext.ReaderOrders) {
                    if(order.Reader_Id == ActiveUserId) {
                        readerOrders.Add(order);
                    }
                }                
                List<Order> orders = new List<Order>();
                foreach (ReaderOrder readerOrder in readerOrders) {
                    foreach (Order order in dbContext.Orders) {
                        if (readerOrder.Order_Id == order.ReaderOrder_Id) {
                            TimeSpan expireTime = DateTime.Now - order.OrderDate;
                            if (expireTime.Days > 14 && order.IsReturned == false) {
                                order.IsExpired = true;                               
                            }
                            orders.Add(order);
                        }
                    }
                }
                dbContext.SaveChanges();
                foreach (Order order in orders) {
                    foreach(Book book in dbContext.Books) {
                        if (order.Book_Id == book.Book_Id) {
                            UsersBooksGrid.Rows.Add(book.Name, book.Autor, book.Genre, order.OrderDate.ToShortDateString(), order.IsExpired, order.IsReturned);
                        }
                    }
                }
            }
        }

        private void ShowPasswordBox_CheckedChanged(object sender, EventArgs e) {
            ChangePassBox.UseSystemPasswordChar = !ShowPasswordBox.Checked;
            ChangePassConfirmBox.UseSystemPasswordChar = !ShowPasswordBox.Checked;
        }

        private void ChangePasswordButton_Click(object sender, EventArgs e) {
            bool isChanged = false;
            if (ChangePassBox.Text != ChangePassConfirmBox.Text) {
                MessageBox.Show("Passwords aren`t matches", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
                return;
            }
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                Reader reader = dbContext.Readers.SingleOrDefault(r => r.Reader_Id == ActiveUserId);
                if (reader != null) {
                    reader.Password = ChangePassBox.Text;
                    dbContext.SaveChanges();
                    isChanged = true;
                    if (isChanged) MessageBox.Show("Password changed successfuly", "Success", MessageBoxButtons.OK);
                    ChangePassBox.Clear();
                    ChangePassConfirmBox.Clear();
                }
            }
        }

        private void ViewAllButton_Click(object sender, EventArgs e) {
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                List<Book> books = dbContext.Books.ToList();
                SearchResultGrid.DataSource = books;
                int cellcount = SearchResultGrid.ColumnCount - 1;
                SearchResultGrid.Columns.Remove(SearchResultGrid.Columns[cellcount]);
            }
        }

        private void ChangeInfoButton_Click(object sender, EventArgs e) {
            bool isChanged = false;
            string newName = ChangeNameTextbox.Text;
            string newPhoneNumber = ChangePhonetextbox.Text;
            DateTime newBirthDate = DateTime.Parse(ChangeDateBox.Text + " " + ChangeMonthBox.Text + " " + ChangeYearBox.Text);
            string phoneNumberPattern = @"[+]{1}\b\d{12}\b";
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                Reader reader = dbContext.Readers.SingleOrDefault(r => r.Reader_Id == ActiveUserId);
                if (reader != null) {
                    if (newName != null && newName.Trim().Length > 0) {
                        reader.Name = newName;
                    }
                    if (newPhoneNumber != null && newPhoneNumber.Trim().Length > 0 && Regex.IsMatch(newPhoneNumber, phoneNumberPattern)) {
                        reader.PhoneNumber = newPhoneNumber;
                    } else {
                        MessageBox.Show("Incorect form of phone number", "Error");
                        return;
                    }
                    if (newBirthDate != null) {
                        reader.BirthDate = newBirthDate;
                    }
                }
                dbContext.SaveChanges();
                NameLabel.Text = reader.Name;
                PhoneNumberLabel.Text = reader.PhoneNumber;
                BirthDateLabel.Text = "Date of birth " + reader.BirthDate.ToShortDateString();
                isChanged = true;
            }            
            if (isChanged) MessageBox.Show("Information changed successfuly", "Success", MessageBoxButtons.OK);
            ChangeNameTextbox.Clear();
            ChangePhonetextbox.Clear();
        }

        private void ShowWPassBox_CheckedChanged(object sender, EventArgs e) {
            ChangeWPassTextbox.UseSystemPasswordChar = !ShowWPassBox.Checked;
            ConfirmChangeWPassTextbox.UseSystemPasswordChar = !ShowWPassBox.Checked;
        }

        private void ChangeWPassButton_Click(object sender, EventArgs e) {
            bool isChanged = false;
            if (ChangeWPassTextbox.Text != ConfirmChangeWPassTextbox.Text) {
                MessageBox.Show("Passwords aren`t matches", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                Employee employee = dbContext.Employees.SingleOrDefault(w => w.Employee_Id == ActiveUserId);
                if (employee != null) {
                    employee.Password = ChangeWPassTextbox.Text;
                    dbContext.SaveChanges();
                    isChanged = true;
                    if (isChanged) MessageBox.Show("Password changed successfuly", "Success", MessageBoxButtons.OK);
                    ChangeWPassTextbox.Clear();
                    ConfirmChangeWPassTextbox.Clear();
                }
            }
        }

        private void ChangeWInfoButton_Click(object sender, EventArgs e) {
            bool isChanged = false;
            string newName = ChangeWNameTextbox.Text;
            string newPhone = ChangeWPhoneTextbox.Text;
            DateTime newBirthDate = DateTime.Parse(ChangeWDateBox.Text + " " + ChangeWMonthBox.Text + " " + ChangeWYearBox.Text);
            string phoneNumberPattern = @"[+]{1}\b\d{12}\b";
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                Employee employee = dbContext.Employees.SingleOrDefault(w => w.Employee_Id == ActiveUserId);
                if (employee != null) {
                    if (newName != null && newName.Trim().Length > 0) {
                        employee.Name = newName;
                    }
                    if (newPhone != null && newPhone.Trim().Length > 0) {
                        if (Regex.IsMatch(newPhone, phoneNumberPattern)) {
                            employee.PhoneNumber = newPhone;
                        } else {
                            MessageBox.Show("Incorrect form of phone number", "Error");
                            return;
                        }
                    }
                    if (newBirthDate != null) {
                        employee.BirthDate = newBirthDate;
                    }
                }
                dbContext.SaveChanges();
                WorkerNameLabel.Text = employee.Name;
                WorkerPhoneLabel.Text = employee.PhoneNumber;
                WorkerBirtdateLabel.Text = "Date of birth " + employee.BirthDate.ToShortDateString();
                isChanged = true;
            }
            if (isChanged) MessageBox.Show("Information changed successfuly", "Success", MessageBoxButtons.OK);
            ChangeWNameTextbox.Clear();
            ChangeWPhoneTextbox.Clear();
        }

        private void ShowOrderButton_Click(object sender, EventArgs e) {
            OrdersViewGrid.Rows.Clear();
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                List<ReaderOrder> readerOrders = new List<ReaderOrder>();
                int num;
                if (OrderIDTextbox.Text != null && OrderIDTextbox.Text.Trim().Length > 0 && int.TryParse(OrderIDTextbox.Text, out num)) {
                    foreach (ReaderOrder readerOrder in dbContext.ReaderOrders) {
                        if (readerOrder.Order_Id == Convert.ToInt32(OrderIDTextbox.Text)) {
                            readerOrders.Add(readerOrder);
                        }
                    }
                } else if (OrderIDTextbox.Text == null || OrderIDTextbox.Text.Trim().Length == 0) {
                    foreach (ReaderOrder readerOrder in dbContext.ReaderOrders) {
                        readerOrders.Add(readerOrder);
                    }
                }
                List<Order> orders = new List<Order>();
                foreach (ReaderOrder readerOrder in readerOrders) {
                    foreach (Order order in dbContext.Orders) {
                        if (readerOrder.Order_Id == order.ReaderOrder_Id) {
                            TimeSpan expireTime = DateTime.Now - order.OrderDate;
                            if (expireTime.Days > 14 && order.IsReturned == false) {
                                order.IsExpired = true;
                            }
                            orders.Add(order);
                        }
                    }
                }
                dbContext.SaveChanges();
                foreach (Order order in orders) {
                    foreach (Book book in dbContext.Books) {
                        if (order.Book_Id == book.Book_Id) {
                            foreach (ReaderOrder readerOrder in readerOrders) {
                                if (readerOrder.Order_Id == order.ReaderOrder_Id) {
                                    foreach (Reader reader in dbContext.Readers) {
                                        if (readerOrder.Reader_Id == reader.Reader_Id) {
                                            OrdersViewGrid.Rows.Add(order.ReaderOrder_Id, reader.Name, reader.CardNumber, book.Name, book.Autor, 
                                                order.OrderDate.ToShortDateString(), order.IsExpired, order.IsReturned, order.LineItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MarkAsReadButton_Click(object sender, EventArgs e) {
            if (OrdersViewGrid.SelectedRows.Count == 0) return;
            int itemCell = OrdersViewGrid.ColumnCount - 1;
            int bookId = Convert.ToInt32(OrdersViewGrid.SelectedRows[0].Cells[itemCell].Value);
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                foreach(Order order in dbContext.Orders) {
                    if (order.LineItem == bookId) {
                        order.IsReturned = true;
                    }
                }
                dbContext.SaveChanges();
                OrdersViewGrid.SelectedRows[0].Cells[itemCell - 1].Value = true;
                OrdersViewGrid.Refresh();
            }
        }

        private void ReaderRadioButton_CheckedChanged(object sender, EventArgs e) {
            UpdateSalaryTextbox.Enabled = false;
            NewWorkerAddButton.Enabled = false;
            LibrarianFullNameTextbox.Enabled = false;
            PhoneNumberTextbox.Enabled = false;
            DayCombobox.Enabled = false;
            MonthCombobox.Enabled = false;
            YearCombobox.Enabled = false;
            ContractNumberTextbox.Enabled = false;
            SalaryTextbox.Enabled = false;
            LibrarianGridView.Visible = false;
            ReaderGridView.Visible = true;
        }

        private void LibrarianRadioButton_CheckedChanged(object sender, EventArgs e) {
            UpdateSalaryTextbox.Enabled = true;
            NewWorkerAddButton.Enabled = true;
            LibrarianFullNameTextbox.Enabled = true;
            PhoneNumberTextbox.Enabled = true;
            DayCombobox.Enabled = true;
            MonthCombobox.Enabled = true;
            YearCombobox.Enabled = true;
            ContractNumberTextbox.Enabled = true;
            SalaryTextbox.Enabled = true;
            LibrarianGridView.Visible = true;
            ReaderGridView.Visible = false;
        }

        private void FindBooksToEditButton_Click(object sender, EventArgs e) {
            BooksToEditGrid.Rows.Clear();
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                List<Book> books = dbContext.Books.ToList();
                foreach (Book book in books) {
                    if ((book.Name.Contains(EditBookTitleTextbox.Text) && EditBookTitleTextbox.Text != null && EditBookTitleTextbox.Text.Trim().Length > 0) ||
                        (book.Autor.Contains(EditAutorTextbox.Text) && EditAutorTextbox.Text != null && EditAutorTextbox.Text.Trim().Length > 0) ||
                        (book.Genre.Contains(EditGenreTextbox.Text) && EditGenreTextbox.Text != null && EditGenreTextbox.Text.Trim().Length > 0) ||
                        (book.Year.Contains(EditYearTextbox.Text) && EditYearTextbox.Text != null && EditYearTextbox.Text.Trim().Length > 0) ||
                        (book.Publication.Contains(EditPublicationTextbox.Text) && EditPublicationTextbox.Text != null && EditPublicationTextbox.Text.Trim().Length > 0)) {
                        BooksToEditGrid.Rows.Add(book.Name, book.Autor, book.Genre, book.Year, book.Publication, book.Book_Id);
                    }
                    if ((EditBookTitleTextbox.Text == null || EditBookTitleTextbox.Text.Trim().Length == 0) &&
                        (EditAutorTextbox.Text == null || EditAutorTextbox.Text.Trim().Length == 0) &&
                        (EditGenreTextbox.Text == null || EditGenreTextbox.Text.Trim().Length == 0) &&
                        (EditYearTextbox.Text == null || EditYearTextbox.Text.Trim().Length == 0) &&
                        (EditPublicationTextbox.Text == null || EditPublicationTextbox.Text.Trim().Length == 0)) {
                        BooksToEditGrid.Rows.Add(book.Name, book.Autor, book.Genre, book.Year, book.Publication, book.Book_Id);
                    }
                }

            }
        }

        private void AddNewBookButton_Click(object sender, EventArgs e) {
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                if ((EditBookTitleTextbox.Text != null && EditBookTitleTextbox.Text.Trim().Length > 0) &&
                        (EditAutorTextbox.Text != null && EditAutorTextbox.Text.Trim().Length > 0) &&
                        (EditGenreTextbox.Text != null && EditGenreTextbox.Text.Trim().Length > 0) &&
                        (EditYearTextbox.Text != null && EditYearTextbox.Text.Trim().Length > 0) &&
                        (EditPublicationTextbox.Text != null && EditPublicationTextbox.Text.Trim().Length > 0)) {
                    int num;
                    if (!int.TryParse(EditYearTextbox.Text, out num)) {
                        MessageBox.Show("Incorrect year!", "Error");
                        return;
                    }
                    dbContext.Books.Add(new Book { Name = EditBookTitleTextbox.Text, Autor = EditAutorTextbox.Text, Genre = EditGenreTextbox.Text,
                                                   Year = EditYearTextbox.Text, Publication = EditPublicationTextbox.Text });
                    dbContext.SaveChanges();
                    MessageBox.Show("Book was added", "Success", MessageBoxButtons.OK);
                } else {
                    MessageBox.Show("Some data wasnt filled. Please fill all data details", "Book wasn`t added");
                }
            }
        }

        private void RemoveBookButton_Click(object sender, EventArgs e) {
            if (BooksToEditGrid.SelectedRows.Count == 0) return;
            using (LibraryDBEntities dBContext = new LibraryDBEntities()) {
                List<Book> books = dBContext.Books.ToList();
                int id = Convert.ToInt32(BooksToEditGrid.SelectedRows[0].Cells[5].Value);
                foreach (Book book in books) {
                    if (book.Book_Id == id) {
                        dBContext.Books.Remove(book);
                    }
                }
                dBContext.SaveChanges();
                DataGridViewRow rowToDelete = BooksToEditGrid.SelectedRows[0];
                BooksToEditGrid.Rows.Remove(rowToDelete);
                BooksToEditGrid.ClearSelection();
            }
        }

        private void UpdateBookButton_Click(object sender, EventArgs e) {
            bool isChanged = false;
            string newBookTitle = EditBookTitleTextbox.Text;
            string newBookAutor = EditAutorTextbox.Text;
            string newBookGenre = EditGenreTextbox.Text;
            string newBookYear = EditYearTextbox.Text;
            string newBookPublication = EditPublicationTextbox.Text;
            using (LibraryDBEntities dbContex = new LibraryDBEntities()) {
                if (BooksToEditGrid.SelectedRows.Count == 0) return;
                int id = Convert.ToInt32(BooksToEditGrid.SelectedRows[0].Cells[5].Value);
                Book book = dbContex.Books.SingleOrDefault(b => b.Book_Id == id);
                if (book != null) {
                    if(newBookTitle != null && newBookTitle.Trim().Length > 0) {
                        book.Name = newBookTitle;
                        BooksToEditGrid.SelectedRows[0].Cells[0].Value = newBookTitle;
                    }
                    if (newBookAutor != null && newBookAutor.Trim().Length > 0) {
                        book.Autor = newBookAutor;
                        BooksToEditGrid.SelectedRows[0].Cells[1].Value = newBookAutor;
                    }
                    if (newBookGenre != null && newBookGenre.Trim().Length > 0) {
                        book.Genre = newBookGenre;
                        BooksToEditGrid.SelectedRows[0].Cells[2].Value = newBookGenre;
                    }
                    if (newBookYear != null && newBookYear.Trim().Length > 0) {
                        int num;
                        if (!int.TryParse(EditYearTextbox.Text, out num)) {
                            MessageBox.Show("Incorrect year!", "Error");
                            return;
                        }
                        book.Year = newBookYear;
                        BooksToEditGrid.SelectedRows[0].Cells[3].Value = newBookYear;
                    }
                    if (newBookPublication != null && newBookPublication.Trim().Length > 0) {
                        book.Publication = newBookPublication;
                        BooksToEditGrid.SelectedRows[0].Cells[4].Value = newBookPublication;
                    }
                    isChanged = true;
                    dbContex.SaveChanges();
                }
                BooksToEditGrid.Refresh();
                EditBookTitleTextbox.Clear();
                EditAutorTextbox.Clear();
                EditGenreTextbox.Clear();
                EditYearTextbox.Clear();
                EditPublicationTextbox.Clear();
                if (isChanged) MessageBox.Show("Information changed successfuly", "Success", MessageBoxButtons.OK);
            }
        }

        private void EditScheduleButton_Click(object sender, EventArgs e) {
            bool isChanged = false;
            if (ScheduleGridView.SelectedRows.Count == 0) return;
            if (WorkersComboBox.Text.Trim().Length == 0 || WorkersComboBox.Text == null) return;
            if (WorkTimeTextbox.Text.Trim().Length == 0 || WorkTimeTextbox.Text == null) return;            
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                int day_IdCell = ScheduleGridView.ColumnCount - 1;
                int name_Cell = ScheduleGridView.ColumnCount - 3;
                int day_Id = Convert.ToInt32(ScheduleGridView.SelectedRows[0].Cells[day_IdCell].Value);
                string[] arr = WorkersComboBox.Text.Split('-');
                int workerID = Convert.ToInt32(arr[0].Trim());
                Schedule day = dbContext.Schedules.SingleOrDefault(d => d.Day_Id == day_Id);
                foreach(Employee employee in dbContext.Employees) {
                    if (employee.Employee_Id == workerID) {
                        day.Employee_Id = workerID;
                        day.WorkTime = WorkTimeTextbox.Text;
                    }
                }
                dbContext.SaveChanges();
                isChanged = true;                
                Employee newScheduleLibrarian = dbContext.Employees.SingleOrDefault(w => w.Employee_Id == workerID);
                ScheduleGridView.SelectedRows[0].Cells[name_Cell].Value = newScheduleLibrarian.Name;
                ScheduleGridView.SelectedRows[0].Cells[name_Cell + 1].Value = WorkTimeTextbox.Text;
                ScheduleGridView.Refresh();
                if (isChanged) MessageBox.Show("Schedule was edited", "Success", MessageBoxButtons.OK);
            }
        }

        private void ShowScheduleButton_Click(object sender, EventArgs e) {
            ScheduleWindow window = new ScheduleWindow();
            window.Show();
        }

        private void NewWorkerAddButton_Click(object sender, EventArgs e) {
            bool IsNameOk = false;
            bool IsPhoneOk = false;
            bool IsContractOk = false;
            bool IsSalaryOk = false;
            int num;
            string newLibrarianName = LibrarianFullNameTextbox.Text;
            string newLibrarianPhone = PhoneNumberTextbox.Text;
            string phoneNumberPattern = @"[+]{1}\b\d{12}\b";
            DateTime newLibrarianBirthDate = DateTime.Parse(DayCombobox.Text + " " + MonthCombobox.Text + " " + YearCombobox.Text);
            string newLibrarianContractNumber = ContractNumberTextbox.Text;
            string newLibrarianSalary = SalaryTextbox.Text;
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                Employee employee = new Employee();
                Job job = dbContext.Jobs.SingleOrDefault(j => j.Job_Id == 2);
                if (newLibrarianName != null && newLibrarianName.Trim().Length > 0) {
                    employee.Name = newLibrarianName;
                    IsNameOk = true;
                }
                if (newLibrarianPhone != null && newLibrarianPhone.Trim().Length > 0) {
                    if (!Regex.IsMatch(newLibrarianPhone, phoneNumberPattern)) {
                        MessageBox.Show("Wrong mobile phone number", "Error", MessageBoxButtons.OK);
                        return;
                    }
                    employee.PhoneNumber = newLibrarianPhone;
                    IsPhoneOk = true;
                }
                if (newLibrarianContractNumber != null && newLibrarianContractNumber.Trim().Length > 0) {
                    Employee employExist = dbContext.Employees.SingleOrDefault(emp => emp.Login == newLibrarianContractNumber);
                    if (employExist == null) {
                        employee.Login = newLibrarianContractNumber;
                        IsContractOk = true;
                    } else {
                        MessageBox.Show("Such contract is already registered", "Error", MessageBoxButtons.OK);
                        return;
                    }
                }
                if (newLibrarianSalary != null && newLibrarianSalary.Trim().Length > 0) {
                    if (!int.TryParse(newLibrarianSalary, out num)) {
                        MessageBox.Show("Wrong characters in 'Salary' field", "Error", MessageBoxButtons.OK);
                        return;
                    }
                    if (Convert.ToInt32(newLibrarianSalary) < job.Min_Salary) {
                        MessageBox.Show("Sallary is too small", "Error", MessageBoxButtons.OK);
                        return;
                    } else if (Convert.ToInt32(newLibrarianSalary) > job.Max_Salary) {
                        MessageBox.Show("Sallary is too big", "Error", MessageBoxButtons.OK);
                        return;
                    } else {
                        employee.Salary = Convert.ToInt32(newLibrarianSalary);
                        IsSalaryOk = true;
                    }
                }
                if (!(IsNameOk && IsPhoneOk && IsContractOk && IsSalaryOk)) {
                    MessageBox.Show("Some fields are empty. Please check your information", "Error", MessageBoxButtons.OK);
                    return;
                }
                employee.BirthDate = newLibrarianBirthDate;
                employee.HireDate = DateTime.Now;
                employee.Job_Id = 2;
                employee.Password = "123456789";
                dbContext.Employees.Add(employee);
                dbContext.SaveChanges();
                LibrarianFullNameTextbox.Clear();
                PhoneNumberTextbox.Clear();
                ContractNumberTextbox.Clear();
                SalaryTextbox.Clear();
                MessageBox.Show("New Librarian added", "Error", MessageBoxButtons.OK);
            }
        }

        private void ViewAllPeopleButton_Click(object sender, EventArgs e) {
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                if (ReaderRadioButton.Checked) {
                    ReaderGridView.Rows.Clear();
                    foreach (Reader reader in dbContext.Readers) {
                        ReaderGridView.Rows.Add(reader.Name, reader.PhoneNumber, reader.CardNumber, reader.BirthDate.ToShortDateString(), reader.DateOfSigningIn.ToShortDateString());
                    }
                }
                if (LibrarianRadioButton.Checked) {
                    LibrarianGridView.Rows.Clear();
                    foreach (Employee employee in dbContext.Employees) {
                        string position = "Unknown";
                        switch (employee.Job_Id) {
                            case 1:
                                position = "Head Librarian";
                                break;
                            case 2:
                                position = "Librarian";
                                break;
                        }
                        LibrarianGridView.Rows.Add(employee.Name, employee.Login, employee.PhoneNumber, employee.BirthDate.ToShortDateString(),
                            employee.HireDate.ToShortDateString(), position, employee.Salary);
                    }
                }
            }
        }

        private void FindPersoneButton_Click(object sender, EventArgs e) {
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                if (ReaderRadioButton.Checked) {
                    ReaderGridView.Rows.Clear();
                    foreach (Reader reader in dbContext.Readers) {
                        if (UpdateLoginTextbox.Text == null || UpdateLoginTextbox.Text.Trim().Length == 0) return;
                        if (reader.CardNumber.Contains(UpdateLoginTextbox.Text)) {
                            ReaderGridView.Rows.Add(reader.Name, reader.PhoneNumber, reader.CardNumber, reader.BirthDate.ToShortDateString(), reader.DateOfSigningIn.ToShortDateString());
                        }
                    }
                }
                if (LibrarianRadioButton.Checked) {
                    LibrarianGridView.Rows.Clear();
                    foreach (Employee employee in dbContext.Employees) {
                        if (UpdateLoginTextbox.Text == null || UpdateLoginTextbox.Text.Trim().Length == 0) return;
                        if (employee.Login.Contains(UpdateLoginTextbox.Text)) {
                            string position = "Unknown";
                            switch (employee.Job_Id) {
                                case 1:
                                    position = "Head Librarian";
                                    break;
                                case 2:
                                    position = "Librarian";
                                    break;
                            }
                            LibrarianGridView.Rows.Add(employee.Name, employee.Login, employee.PhoneNumber, employee.BirthDate.ToShortDateString(),
                            employee.HireDate.ToShortDateString(), position, employee.Salary);
                        }
                    }
                }
            }
        }

        private void ChangePersonInfoButton_Click(object sender, EventArgs e) {
            bool isChanged = false;
            using (LibraryDBEntities dbContext = new LibraryDBEntities()) {
                if (ReaderRadioButton.Checked) {
                    if (ReaderGridView.SelectedRows.Count == 0) return;
                    string cardNumber = ReaderGridView.SelectedRows[0].Cells[2].Value.ToString();
                    string loginPattern = @"[A-Z]{2}[-]{1}\b\d{6}\b";
                    if (!Regex.IsMatch(UpdateLoginTextbox.Text, loginPattern)) {
                        MessageBox.Show("Invalid readers card identification", "Sign up error");
                        return;
                    }
                    foreach (Reader readerOld in dbContext.Readers) {
                        if (readerOld.CardNumber == UpdateLoginTextbox.Text) {
                            MessageBox.Show("Such card already registered", "Error", MessageBoxButtons.OK);
                            return;
                        }
                    }
                    Reader reader = dbContext.Readers.SingleOrDefault(r => r.CardNumber == cardNumber);
                    if (reader != null) {
                        reader.CardNumber = UpdateLoginTextbox.Text;
                        dbContext.SaveChanges();
                        isChanged = true;
                    }
                    ReaderGridView.SelectedRows[0].Cells[2].Value = UpdateLoginTextbox.Text;
                    ReaderGridView.Refresh();
                    if (isChanged) MessageBox.Show("Login changed successfuly", "Success", MessageBoxButtons.OK);
                    UpdateLoginTextbox.Clear();
                }
                if (LibrarianRadioButton.Checked) {
                    if (LibrarianGridView.SelectedRows.Count == 0) return;
                    string contractNumber = LibrarianGridView.SelectedRows[0].Cells[1].Value.ToString();                    
                    foreach (Employee employeeOld in dbContext.Employees) {
                        if (employeeOld.Login == UpdateLoginTextbox.Text) {
                            MessageBox.Show("Such contract already registered", "Error", MessageBoxButtons.OK);
                            return;
                        }
                    }
                    Employee employee = dbContext.Employees.SingleOrDefault(emp => emp.Login == contractNumber);
                    if (employee != null) {
                        if (UpdateSalaryTextbox.Text != null && UpdateSalaryTextbox.Text.Trim().Length > 0) {
                            Job job = dbContext.Jobs.SingleOrDefault(j => j.Job_Id == 2);
                            if (Convert.ToInt32(UpdateSalaryTextbox.Text) < job.Min_Salary) {
                                MessageBox.Show("Sallary is too small", "Error", MessageBoxButtons.OK);
                                return;
                            }
                            else if (Convert.ToInt32(UpdateSalaryTextbox.Text) > job.Max_Salary) {
                                MessageBox.Show("Sallary is too big", "Error", MessageBoxButtons.OK);
                                return;
                            }
                            else {
                                employee.Salary = Convert.ToInt32(UpdateSalaryTextbox.Text);
                                LibrarianGridView.SelectedRows[0].Cells[6].Value = UpdateSalaryTextbox.Text;
                                dbContext.SaveChanges();
                            }
                        }
                        if (UpdateLoginTextbox.Text == null || UpdateLoginTextbox.Text.Trim().Length == 0) return;                        
                        employee.Login = UpdateLoginTextbox.Text;
                        LibrarianGridView.SelectedRows[0].Cells[1].Value = UpdateLoginTextbox.Text;
                        dbContext.SaveChanges();
                        isChanged = true;
                        LibrarianGridView.Refresh();
                        if (isChanged) MessageBox.Show("Information changed successfuly", "Success", MessageBoxButtons.OK);
                        UpdateSalaryTextbox.Clear();
                        UpdateLoginTextbox.Clear();
                    }                    
                }
            }            
        }
    }
}
