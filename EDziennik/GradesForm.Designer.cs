namespace EDziennik
{
    partial class GradesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvStudents = new System.Windows.Forms.DataGridView();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.lblFirstName = new System.Windows.Forms.Label();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblClass = new System.Windows.Forms.Label();
            this.cmbSubject = new System.Windows.Forms.ComboBox();
            this.dgvGrades = new System.Windows.Forms.DataGridView();
            this.cmbGradeValue = new System.Windows.Forms.ComboBox();
            this.cmbGradeWeight = new System.Windows.Forms.ComboBox();
            this.dtpGradeDate = new System.Windows.Forms.DateTimePicker();
            this.txtGradeNote = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvStudents
            // 
            this.dgvStudents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStudents.Location = new System.Drawing.Point(0, 0);
            this.dgvStudents.MultiSelect = false;
            this.dgvStudents.Name = "dgvStudents";
            this.dgvStudents.ReadOnly = true;
            this.dgvStudents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStudents.Size = new System.Drawing.Size(790, 150);
            this.dgvStudents.TabIndex = 0;
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(12, 192);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(26, 13);
            this.lblFirstName.TabIndex = 1;
            this.lblFirstName.Text = "Imie";
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(94, 192);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(53, 13);
            this.lblLastName.TabIndex = 2;
            this.lblLastName.Text = "Nazwisko";
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Location = new System.Drawing.Point(182, 192);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(79, 13);
            this.lblBirthDate.TabIndex = 3;
            this.lblBirthDate.Text = "Data urodzenia";
            // 
            // lblClass
            // 
            this.lblClass.AutoSize = true;
            this.lblClass.Location = new System.Drawing.Point(268, 192);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(33, 13);
            this.lblClass.TabIndex = 4;
            this.lblClass.Text = "Klasa";
            // 
            // cmbSubject
            // 
            this.cmbSubject.FormattingEnabled = true;
            this.cmbSubject.Location = new System.Drawing.Point(15, 223);
            this.cmbSubject.Name = "cmbSubject";
            this.cmbSubject.Size = new System.Drawing.Size(121, 21);
            this.cmbSubject.TabIndex = 5;
            // 
            // dgvGrades
            // 
            this.dgvGrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrades.Location = new System.Drawing.Point(0, 250);
            this.dgvGrades.Name = "dgvGrades";
            this.dgvGrades.ReadOnly = true;
            this.dgvGrades.Size = new System.Drawing.Size(810, 150);
            this.dgvGrades.TabIndex = 6;
            // 
            // cmbGradeValue
            // 
            this.cmbGradeValue.FormattingEnabled = true;
            this.cmbGradeValue.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.cmbGradeValue.Location = new System.Drawing.Point(15, 428);
            this.cmbGradeValue.Name = "cmbGradeValue";
            this.cmbGradeValue.Size = new System.Drawing.Size(121, 21);
            this.cmbGradeValue.TabIndex = 7;
            // 
            // cmbGradeWeight
            // 
            this.cmbGradeWeight.FormattingEnabled = true;
            this.cmbGradeWeight.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cmbGradeWeight.Location = new System.Drawing.Point(166, 428);
            this.cmbGradeWeight.Name = "cmbGradeWeight";
            this.cmbGradeWeight.Size = new System.Drawing.Size(121, 21);
            this.cmbGradeWeight.TabIndex = 8;
            // 
            // dtpGradeDate
            // 
            this.dtpGradeDate.Location = new System.Drawing.Point(15, 487);
            this.dtpGradeDate.Name = "dtpGradeDate";
            this.dtpGradeDate.Size = new System.Drawing.Size(200, 20);
            this.dtpGradeDate.TabIndex = 9;
            // 
            // txtGradeNote
            // 
            this.txtGradeNote.Location = new System.Drawing.Point(271, 487);
            this.txtGradeNote.Name = "txtGradeNote";
            this.txtGradeNote.Size = new System.Drawing.Size(100, 20);
            this.txtGradeNote.TabIndex = 10;
            // 
            // GradesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 546);
            this.Controls.Add(this.txtGradeNote);
            this.Controls.Add(this.dtpGradeDate);
            this.Controls.Add(this.cmbGradeWeight);
            this.Controls.Add(this.cmbGradeValue);
            this.Controls.Add(this.dgvGrades);
            this.Controls.Add(this.cmbSubject);
            this.Controls.Add(this.lblClass);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.dgvStudents);
            this.Name = "GradesForm";
            this.Text = "GradesForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStudents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrades)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvStudents;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.Label lblClass;
        private System.Windows.Forms.ComboBox cmbSubject;
        private System.Windows.Forms.DataGridView dgvGrades;
        private System.Windows.Forms.ComboBox cmbGradeValue;
        private System.Windows.Forms.ComboBox cmbGradeWeight;
        private System.Windows.Forms.DateTimePicker dtpGradeDate;
        private System.Windows.Forms.TextBox txtGradeNote;
    }
}