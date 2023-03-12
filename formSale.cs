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
                        GetProducttoList(foundproduct, barcode, Convert.ToDouble(textBox6.Text));
                    }
                    else
                    {
                        var barkod = Convert.ToInt32(barcode.Substring(0, 2));
                        if (db.Terazi.Any(x => x.TeraziOnEk == barkod))
                        {
                            var foundproduct = db.Table.Where(x => x.Barkod == barcode.Substring(2, 5)).FirstOrDefault();
                            if (foundproduct != null)
                            {
                                var amountofkg = Convert.ToDouble(barcode.Substring(7, 5)) / 1000;
                                GetProducttoList(foundproduct, barcode.Substring(2, 5),amountofkg);
                            }
                            else
                            {
                                Console.Beep(900, 1000);
                                MessageBox.Show("KG PRODUCT");
                            }
                        }
                        else
                        {
                            Console.Beep(900, 1000);
                            MessageBox.Show("PRODUCT");

                        }

                        textBox2.Clear();
                        textBox2.Focus();
                    }
                }
            }
        }

        public void FillButtons()
        {
            var quickp = new DatabaseEntities().HızlıUrun.ToList();

            foreach (var item in quickp)
            {
                Button btn = Controls.Find("button" + (item.Id + 3), true).FirstOrDefault() as Button;
                if(btn != null)
                {
                    btn.Text = item.urunAd + Environment.NewLine + item.Fiyat + "TL";
                }
            }

        }
        public void GetProducttoList(Urun urun, string barcode, double amount)
        {
            bool isadded = false;

            if (dataGridView1.Rows.Count >= 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["Barcode"].Value.ToString() == urun.Barkod)
                    {
                        dataGridView1.Rows[i].Cells["Miktar"].Value = Convert.ToDouble(dataGridView1.Rows[i].Cells["Miktar"].Value) + amount;
                        dataGridView1.Rows[i].Cells["Toplam"].Value = Convert.ToDouble(dataGridView1.Rows[i].Cells["Toplam"].Value) + amount * Convert.ToDouble(urun.SatisFiyat);
                        textBox2.Clear();
                        textBox2.Focus();
                        isadded = true;
                    }
                }
            }

            if (!isadded)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["ÜrünAdi"].Value = urun.Urunad.ToString();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Miktar"].Value = amount;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Fiyat"].Value = urun.SatisFiyat;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Barcode"].Value = urun.Barkod;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Toplam"].Value = amount * Convert.ToDouble(urun.SatisFiyat);
                textBox2.Clear();
                textBox2.Focus();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                if (dataGridView1.Rows.Count > 0)
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void formSale_Load(object sender, EventArgs e)
        {
            FillButtons();
        }

        private void quickbutton_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            DatabaseEntities db = new DatabaseEntities();
            var bid = Convert.ToInt32(b.Name.Substring(6, 1)) - 3;
            var quickp = db.HızlıUrun.FirstOrDefault(a => a.Id == 1);
            var p = db.Table.FirstOrDefault(x=>x.Barkod == quickp.Barkod);
            if(quickp !=null)
            {
                GetProducttoList(p,p.Barkod, Convert.ToDouble(textBox6.Text));
            }
        }
    }
}
