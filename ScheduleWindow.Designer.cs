namespace WindowsLib {
    partial class ScheduleWindow {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.ScheduleGridView = new System.Windows.Forms.DataGridView();
            this.DayOfWeek = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Librarian = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Day_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScheduleOKButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // ScheduleGridView
            // 
            this.ScheduleGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ScheduleGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DayOfWeek,
            this.Librarian,
            this.WorkTime,
            this.Day_ID});
            this.ScheduleGridView.Location = new System.Drawing.Point(12, 12);
            this.ScheduleGridView.Name = "ScheduleGridView";
            this.ScheduleGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ScheduleGridView.Size = new System.Drawing.Size(413, 153);
            this.ScheduleGridView.TabIndex = 1;
            // 
            // DayOfWeek
            // 
            this.DayOfWeek.HeaderText = "Day";
            this.DayOfWeek.Name = "DayOfWeek";
            this.DayOfWeek.ReadOnly = true;
            // 
            // Librarian
            // 
            this.Librarian.HeaderText = "Librarian";
            this.Librarian.Name = "Librarian";
            this.Librarian.ReadOnly = true;
            this.Librarian.Width = 170;
            // 
            // WorkTime
            // 
            this.WorkTime.HeaderText = "Work Time";
            this.WorkTime.Name = "WorkTime";
            this.WorkTime.ReadOnly = true;
            // 
            // Day_ID
            // 
            this.Day_ID.HeaderText = "Day_ID";
            this.Day_ID.Name = "Day_ID";
            this.Day_ID.ReadOnly = true;
            this.Day_ID.Visible = false;
            // 
            // ScheduleOKButton
            // 
            this.ScheduleOKButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(116)))), ((int)(((byte)(237)))));
            this.ScheduleOKButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ScheduleOKButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ScheduleOKButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(116)))), ((int)(((byte)(238)))));
            this.ScheduleOKButton.FlatAppearance.BorderSize = 0;
            this.ScheduleOKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ScheduleOKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ScheduleOKButton.ForeColor = System.Drawing.Color.White;
            this.ScheduleOKButton.Location = new System.Drawing.Point(182, 171);
            this.ScheduleOKButton.Name = "ScheduleOKButton";
            this.ScheduleOKButton.Size = new System.Drawing.Size(70, 38);
            this.ScheduleOKButton.TabIndex = 105;
            this.ScheduleOKButton.Text = "OK";
            this.ScheduleOKButton.UseVisualStyleBackColor = false;
            this.ScheduleOKButton.Click += new System.EventHandler(this.ScheduleOKButton_Click);
            // 
            // ScheduleWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(438, 216);
            this.Controls.Add(this.ScheduleOKButton);
            this.Controls.Add(this.ScheduleGridView);
            this.Name = "ScheduleWindow";
            this.Text = "ScheduleWindow";
            ((System.ComponentModel.ISupportInitialize)(this.ScheduleGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ScheduleGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn DayOfWeek;
        private System.Windows.Forms.DataGridViewTextBoxColumn Librarian;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Day_ID;
        private System.Windows.Forms.Button ScheduleOKButton;
    }
}