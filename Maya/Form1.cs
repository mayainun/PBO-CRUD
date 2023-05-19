using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Maya
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class DatabaseHelpers
        {
            string connString = "Host=localhost;Username=postgres;Password=1;Database=Apotik";
            NpgsqlConnection conn;

            public void connect()
            {
                if (conn == null)
                {
                    conn = new NpgsqlConnection(connString);
                }
                conn.Open();
            }

            public DataTable getData(string sql)
            {
                DataTable table = new DataTable();
                connect();
                try
                {
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, conn);
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    conn.Close();
                }
                return table;
            }

            public void exc(String sql)
            {
                connect();
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch
                {

                }
                finally
                {
                    conn.Close();
                }
            }
        }
        DatabaseHelpers db = new DatabaseHelpers();

        private void Form1_Load(object sender, EventArgs e)
        {
            DataObat();
        }

        private void DataObat()
        {
            string sql = "select * from obat";
            dataGridView1.DataSource = db.getData(sql);
            dataGridView1.Columns["id_obat"].HeaderText = "ID Obat";
            dataGridView1.Columns["Nama"].HeaderText = "Nama";
            dataGridView1.Columns["Jenis"].HeaderText = "Jenis";
            dataGridView1.Columns["Harga"].HeaderText = "Harga";
            dataGridView1.Columns["Stock"].HeaderText = "Stock";
            dataGridView1.Columns["Edit"].DisplayIndex = 6;
            dataGridView1.Columns["Delete"].DisplayIndex = 6;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "INSERT FILM", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string sql = $"INSERT INTO obat(id_obat, nama, jenis, harga, stock) VALUES ({textBox1.Text},'{textBox2.Text}','{textBox3.Text}',{textBox4.Text},{textBox5.Text})";
                MessageBox.Show("Berhasil");
                db.exc(sql);
                DataObat();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            button1.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Edit")
            {
                button1.Enabled = false;
                textBox1.Enabled = false;
                textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["id_obat"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["nama"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["jenis"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells["harga"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells["stock"].Value.ToString();
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {
                var id_obat = dataGridView1.Rows[e.RowIndex].Cells["id_obat"].Value.ToString();
                string sql = $"delete from obat where id_obat = {id_obat}";

                DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "DELETE OBAT", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MessageBox.Show("Berhasil!");
                    db.exc(sql);
                    DataObat();
                    button3.PerformClick();
                }
                else if (dialogResult == DialogResult.No)
                {
                    MessageBox.Show("Gagal!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("APAKAH ANDA YAKIN?", "UPDATE OBAT", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string sql = $"update obat set nama = '{textBox2.Text}', jenis = '{textBox3.Text}', harga = '{textBox4.Text}', stock = '{textBox5.Text}' where id_obat = {textBox1.Text}";
                MessageBox.Show("Berhasil!");
                db.exc(sql);
                DataObat();
                button3.PerformClick();
            }
            else if (dialogResult == DialogResult.No)
            {
                MessageBox.Show("Gagal!");
            }
        }
    }
}
