using System;
using System.Drawing;
using System.Windows.Forms;

namespace SalesSystem.GUI
{
    public partial class frmBanHang : Form
    {
        // Khai báo các điều khiển
        private Label lblMaSP, lblSoLuong, lblNhanVien, lblKhachHang, lblTongTien;
        private TextBox txtMaSP, txtSoLuong;
        private ComboBox cboNhanVien, cboKhachHang;
        private Button btnThem, btnXoa, btnThanhToan, btnLamMoi;
        private DataGridView dgvGioHang;
        private Label lblTongTienValue;

        public frmBanHang()
        {
            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            // Cấu hình Form
            this.Text = "Quản lý Bán Hàng";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Tạo các nhãn
            lblMaSP = new Label { Text = "Mã SP:", Location = new Point(20, 20), AutoSize = true };
            lblSoLuong = new Label { Text = "Số lượng:", Location = new Point(20, 60), AutoSize = true };
            lblNhanVien = new Label { Text = "Nhân viên:", Location = new Point(20, 100), AutoSize = true };
            lblKhachHang = new Label { Text = "Khách hàng:", Location = new Point(20, 140), AutoSize = true };
            lblTongTien = new Label { Text = "Tổng tiền:", Location = new Point(500, 380), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };

            // Tạo ô nhập liệu
            txtMaSP = new TextBox { Location = new Point(100, 18), Size = new Size(200, 25) };
            txtSoLuong = new TextBox { Location = new Point(100, 58), Size = new Size(200, 25) };

            // Tạo ô chọn
            cboNhanVien = new ComboBox { Location = new Point(100, 98), Size = new Size(200, 25) };
            cboKhachHang = new ComboBox { Location = new Point(100, 138), Size = new Size(200, 25) };

            // Tạo nút bấm
            btnThem = new Button { Text = "Thêm vào giỏ", Location = new Point(20, 190), Size = new Size(130, 40) };
            btnXoa = new Button { Text = "Xóa", Location = new Point(170, 190), Size = new Size(130, 40) };
            btnLamMoi = new Button { Text = "Làm mới", Location = new Point(20, 250), Size = new Size(280, 40) };
            btnThanhToan = new Button { Text = "Thanh Toán", Location = new Point(500, 410), Size = new Size(250, 40), BackColor = Color.LightGreen };

            // Tạo bảng giỏ hàng
            dgvGioHang = new DataGridView { Location = new Point(330, 20), Size = new Size(400, 350), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvGioHang.Columns.Add("MaSP", "Mã SP");
            dgvGioHang.Columns.Add("TenSP", "Tên SP");
            dgvGioHang.Columns.Add("SoLuong", "Số lượng");
            dgvGioHang.Columns.Add("DonGia", "Đơn giá");
            dgvGioHang.Columns.Add("ThanhTien", "Thành tiền");

            // Hiển thị tổng tiền
            lblTongTienValue = new Label { Text = "0 VNĐ", Location = new Point(600, 380), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.Red };

            // Thêm tất cả vào Form
            this.Controls.AddRange(new Control[] {
                lblMaSP, lblSoLuong, lblNhanVien, lblKhachHang, lblTongTien,
                txtMaSP, txtSoLuong,
                cboNhanVien, cboKhachHang,
                btnThem, btnXoa, btnLamMoi, btnThanhToan,
                dgvGioHang, lblTongTienValue
            });

            // Gắn sự kiện
            btnThem.Click += BtnThem_Click;
            btnXoa.Click += BtnXoa_Click;
            btnLamMoi.Click += BtnLamMoi_Click;
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập vào không được để trống
            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã SP!");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text) || !int.TryParse(txtSoLuong.Text, out int soLuong) || soLuong <= 0)
            {
                MessageBox.Show("Vui lòng nhập Số lượng hợp lệ!");
                return;
            }

            // === Thêm dòng mới vào bảng giỏ hàng ===
            // Ở đây mình ví dụ Tên SP và Đơn giá tạm, bạn có thể lấy từ CSDL sau
            string tenSP = "Sản phẩm " + txtMaSP.Text;
            decimal donGia = 10000; // ← Thay bằng giá thực từ CSDL
            decimal thanhTien = soLuong * donGia;

            dgvGioHang.Rows.Add(
                txtMaSP.Text,
                tenSP,
                soLuong,
                donGia.ToString("N0"),
                thanhTien.ToString("N0")
            );

            // === Cập nhật Tổng tiền ===
            CapNhatTongTien();

            // === Xóa ô nhập liệu sau khi thêm ===
            txtMaSP.Clear();
            txtSoLuong.Clear();

            MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng!");
        }

        // === Thêm hàm tính tổng tiền ===
        private void CapNhatTongTien()
        {
            decimal tongTien = 0;
            foreach (DataGridViewRow row in dgvGioHang.Rows)
            {
                if (row.Cells["ThanhTien"].Value != null)
                {
                    string s = row.Cells["ThanhTien"].Value.ToString().Replace(",", "");
                    if (decimal.TryParse(s, out decimal thanhTien))
                        tongTien += thanhTien;
                }
            }
            lblTongTienValue.Text = tongTien.ToString("N0") + " VNĐ";
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.SelectedRows.Count > 0)
                dgvGioHang.Rows.RemoveAt(dgvGioHang.SelectedRows[0].Index);
        }

        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaSP.Clear();
            txtSoLuong.Clear();
            cboNhanVien.SelectedIndex = -1;
            cboKhachHang.SelectedIndex = -1;
        }
    }
}
