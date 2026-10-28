using System;
using System.Windows.Forms;

namespace Bai4._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetupFormDefaults();
        }

        private void SetupFormDefaults()
        {
            // 1. Phím Enter kích hoạt btnLogin, phím Esc kích hoạt btnExit
            this.AcceptButton = btnLogin;
            this.CancelButton = btnExit;

            // 2. Mặc định ẩn mật khẩu (*)
            txtPassword.UseSystemPasswordChar = true;
            chkShowPassword.Checked = false;
        }

        // --- Xử lý sự kiện CheckBox Ẩn / Hiện mật khẩu ---
        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        // --- Hàm kiểm tra dữ liệu đầu vào (Validation) ---
        private bool ValidateInputs()
        {
            bool isValid = true;

            // Xóa sạch thông báo lỗi cũ
            errorProvider1.Clear();

            // Kiểm tra trống Tên đăng nhập
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                errorProvider1.SetError(txtLogin, "Vui lòng nhập tên đăng nhập!");
                isValid = false;
            }

            // Kiểm tra trống Mật khẩu
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Vui lòng nhập mật khẩu!");
                isValid = false;
            }

            return isValid;
        }

        // --- Sự kiện click nút Đăng nhập ---
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            string username = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"
                SELECT MatKhauHash
                FROM TaiKhoan
                WHERE TenDangNhap = @username
                LIMIT 1";

                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(
                        sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        object result = cmd.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            MessageBox.Show(
                                "Tên đăng nhập hoặc mật khẩu không đúng!",
                                "Đăng nhập thất bại",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }

                        string storedHash = result.ToString();

                        if (PasswordHasher.VerifyPassword(password, storedHash))
                        {
                            MessageBox.Show(
                                "Đăng nhập thành công!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(
                                "Tên đăng nhập hoặc mật khẩu không đúng!",
                                "Đăng nhập thất bại",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể kết nối hoặc truy vấn database!\nChi tiết: "
                    + ex.Message,
                    "Lỗi database",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // --- Sự kiện click nút Thoát ---
        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận",
                                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        // --- Các hàm hướng sự kiện bị lặp từ Designer về hàm chính ---
        private void chkShowPassword_CheckedChanged_1(object sender, EventArgs e)
        {
            chkShowPassword_CheckedChanged(sender, e);
        }

        private void btnLogin_Click_1(object sender, EventArgs e)
        {
            btnLogin_Click(sender, e);
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            btnExit_Click(sender, e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}