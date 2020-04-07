using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace btapbuoi12_21A4040123
{
    public partial class FrmQLyKSan : Form
    {
        SqlConnection con = new SqlConnection();// Khai báo đối tượng kết nối
        DataTable tablePhong; //Đối tượng lưu bảng Phòng
        public FrmQLyKSan()
        {
            InitializeComponent();
        }

        private void FrmQLyKSan_Load(object sender, EventArgs e)
        {
            con = new SqlConnection();// Khởi tạo đối tượng
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Qly_KhSan;Integrated Security=True";
            con.ConnectionString = connectionString;
            con.Open();
            loadDataToGridview();
        }
        private void loadDataToGridview()
        {
            string sql = "select * from Phong";
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);// Đối tượng DataAdapter
            tablePhong = new DataTable();
            adp.Fill(tablePhong);// Đổ dữ liệu từ DataAdapter vào bảng
            dataGridView1.DataSource = tablePhong;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaPhong.Text = dataGridView1.CurrentRow.Cells["MaPhong"].Value.ToString();
            txtTenPhong.Text = dataGridView1.CurrentRow.Cells["TenPhong"].Value.ToString();
            txtDonGia.Text = dataGridView1.CurrentRow.Cells["DonGia"].Value.ToString();

            txtMaPhong.Enabled = false;
        }
        private void ResetValues()
        {
            txtMaPhong.Text = "";
            txtTenPhong.Text = "";
            txtDonGia.Text = "";
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            btnLuu.Enabled = true;
            btnHuy.Enabled = true;
            txtMaPhong.Enabled = true;
            txtMaPhong.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
             string sql = "SELECT MaPhong FROM Phong WHERE MaPhong='" + txtMaPhong.Text + "'";
            SqlDataAdapter adp = new SqlDataAdapter(sql, con);
            DataTable tablePhong = new DataTable();
            adp.Fill(tablePhong);
            if (tablePhong.Rows.Count > 0)
            { 
                MessageBox.Show("Mã phòng đã tồn tại"); 
                return;
            }
            if (txtMaPhong.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã phòng", "Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaPhong.Focus();
                return;
            }
            if (txtTenPhong.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn cần nhập tên phòng","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtTenPhong.Focus();
                return;
            }
            if(txtDonGia.Text=="")
            {
                MessageBox.Show("Bạn cần nhập đơn giá","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                txtDonGia.Focus();
                return;
            }
            else
            {
                sql = "insert into Phong values('"+ txtMaPhong.Text + "','" + txtTenPhong.Text + "'";
                if (txtDonGia.Text != "")
                    sql = sql + "," + txtDonGia.Text.Trim();
                sql = sql + ")";
                
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.ExecuteNonQuery();
                    loadDataToGridview();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                return;
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string sql;
            if (txtMaPhong.Text == "")
            {
                MessageBox.Show("Bạn chưa chọn bản ghi nào", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                sql = "Delete from Phong where MaPhong='" + txtMaPhong.Text + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                loadDataToGridview();
                ResetValues();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string sql = "Update Phong set TenPhong='" + txtTenPhong.Text + "',DonGia='" + txtDonGia.Text + "'" +
                "where MaPhong='" + txtMaPhong.Text + "' ";
            if (txtTenPhong.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn cần nhập tên phòng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhong.Focus();
                return;
            }
            if (txtDonGia.Text == "")
            {
                MessageBox.Show("Bạn cần nhập đơn giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDonGia.Focus();
                return;
            }
            txtMaPhong.Enabled = false;
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.ExecuteNonQuery();
            loadDataToGridview();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            ResetValues();
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btnHuy.Enabled = false;
            btnLuu.Enabled = false;
            txtMaPhong.Enabled = false;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát khỏi chương trình không?", "Thông báo"
                , MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                con.Close();
            this.Close();
        }

        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) ||
            (e.KeyChar == '.') || (Convert.ToInt32(e.KeyChar) == 8) || (Convert.ToInt32(e.KeyChar) == 13))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        
        

    }
}
