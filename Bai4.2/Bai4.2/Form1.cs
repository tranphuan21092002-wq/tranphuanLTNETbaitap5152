using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bai4._2
{
    public partial class Form1 : Form
    {
        public class Course
        {
            public string CourseID { get; set; }
            public string CourseName { get; set; }

            public Course(string id, string name)
            {
                CourseID = id;
                CourseName = name;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nạp danh sách khóa học vào ComboBox
            List<Course> listCourses = new List<Course>
            {
                new Course("C01", "Lập trình C# WinForms"),
                new Course("C02", "Lập trình Web ASP.NET"),
                new Course("C03", "Cơ sở dữ liệu SQL Server"),
                new Course("C04", "Lập trình Python")
            };

            cboCourse.DataSource = listCourses;
            cboCourse.DisplayMember = "CourseName";
            cboCourse.ValueMember = "CourseID";
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Lay thong tin tu form
            string phone = mtxtPhone.Text.Trim();
            DateTime birthDate = dtpBirthDate.Value.Date;

            string gender = rdoMale.Checked ? "Nam"
                          : rdoNu.Checked ? "Nu"
                          : "";

            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui long nhap so dien thoai!");
                mtxtPhone.Focus();
                return;
            }

            if (gender == "")
            {
                MessageBox.Show("Vui long chon gioi tinh!");
                return;
            }

            if (cboCourse.SelectedItem == null ||
                cboCourse.SelectedValue == null)
            {
                MessageBox.Show("Vui long chon khoa hoc!");
                return;
            }

            string courseName = cboCourse.Text;
            string courseId = cboCourse.SelectedValue.ToString();

            List<string> selectedSkills = new List<string>();

            if (tienganh.Checked)
                selectedSkills.Add(tienganh.Text);

            if (laptrinh.Checked)
                selectedSkills.Add(laptrinh.Text);

            if (thietke.Checked)
                selectedSkills.Add(thietke.Text);

            string skillsText = string.Join(", ", selectedSkills);

            // Luu thong tin vao MySQL
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"INSERT INTO DangKy
                (SoDienThoai, NgaySinh, GioiTinh,
                 KhoaHoc, MaKhoaHoc, KyNang)
                VALUES
                (@phone, @birthDate, @gender,
                 @courseName, @courseId, @skills)";

                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@birthDate", birthDate);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@courseName", courseName);
                        cmd.Parameters.AddWithValue("@courseId", courseId);
                        cmd.Parameters.AddWithValue("@skills", skillsText);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Dang ky khoa hoc thanh cong! Du lieu da duoc luu vao database.",
                    "Thong bao",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Khong the luu du lieu!\nChi tiet: " + ex.Message,
                    "Loi database",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void tienganh_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}