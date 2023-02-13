using SaleProject.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaleProject
{
    public partial class formSale : Form
    {
        private const bool V = true;

        public formSale()
        {
            InitializeComponent();
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode = textBox2.Text.Trim();

                if (barcode.Length <= 2)
                {
                    textBox6.Text = barcode;
                    textBox2.Clear();
                    textBox2.Focus();
                }
                else
                {
                    DatabaseEntities db = new DatabaseEntities();
                    if (db.Table.Any(x => x.Barkod == barcode))
                    {
                        var foundproduct = db.Table.FirstOrDefault(x => x.Barkod == barcode);
                        var rowcount = dataGridView1.Rows.Count;
                        var miktar = Convert.ToDouble(textBox6.Text);
                        bool isadded = false;
                        MessageBox.Show("Bulunan Ürün Adı: " + foundproduct.Urunad);


                        if (dataGridView1.Rows.Count >= 0)
                        {
                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (dataGridView1.Rows[i].Cells["Barcode"].Value.ToString() == foundproduct.Barkod)
                                {
                                    dataGridView1.Rows[i].Cells["Miktar"].Value = Convert.ToDouble(dataGridView1.Rows[i].Cells["Miktar"].Value) + miktar;
                                    dataGridView1.Rows[i].Cells["Toplam"].Value = Convert.ToDouble(dataGridView1.Rows[i].Cells["Toplam"].Value) +  miktar * Convert.ToDouble(foundproduct.SatisFiyat);

                                    isadded = true;
                                }
                            }
                        }
   

                        if (!isadded)
                        {
                            dataGridView1.Rows.Add();
                            dataGridView1.Rows[rowcount].Cells["ÜrünAdi"].Value = foundproduct.Urunad.ToString();
                            dataGridView1.Rows[rowcount].Cells["Miktar"].Value = miktar;
                            dataGridView1.Rows[rowcount].Cells["Fiyat"].Value = foundproduct.SatisFiyat;
                            dataGridView1.Rows[rowcount].Cells["Barcode"].Value = foundproduct.Barkod;
                            dataGridView1.Rows[rowcount].Cells["Toplam"].Value = miktar * Convert.ToDouble(foundproduct.SatisFiyat);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu barkoda sahip ürün ekli değil!");
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {
                dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void formSale_Load(object sender, EventArgs e)
        {

        }
    }
}
