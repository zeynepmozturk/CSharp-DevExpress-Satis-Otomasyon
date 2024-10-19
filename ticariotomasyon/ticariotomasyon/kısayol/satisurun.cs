using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using DevExpress.XtraEditors;

namespace ticariotomasyon
{
    public partial class satisurun : UserControl
    {
        public AnaForm hızlstsfrm;
        string baglanticumlecigi;
        public satisurun(string sqlcumlecigi)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
        }
        public string urunidimiz
        {
            get { return ürünadid; }
            set { ürünadid = value; ürünadid = value; }
        }
        public string urunadimiz
        {
            get { return ürünid; }
            set { ürünid = value; ürünid = value; }
        }
        public string urunfiyatidimiz
        {
            get { return ürünfiyat; }
            set { ürünfiyat = value; ürünfiyat = value; }
        }
        string ürünid, ürünadid;
        string ürünfiyat;
        public OleDbConnection baglanti = new OleDbConnection();
        public void vericek1()
        {
            try
            {
                baglanti.Close();
                baglanti.Open();
                string sql = "SELECT Stokkarti.stokadi,Stokkarti.satisfiyat,Stokkarti.resim,hızlısatısurunler.urunid,hızlısatısurunler.grupid " +
                    "FROM Stokkarti INNER JOIN hızlısatısurunler ON Stokkarti.id=hızlısatısurunler.urunid " +
                    "WHERE (((hızlısatısurunler.urunid) Like '" + urunidimiz + "') AND ((hızlısatısurunler.grupid) Like 0))";
                OleDbCommand veri = new OleDbCommand(sql, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    double x = Convert.ToDouble(oku["satisfiyat"].ToString());
                    ürünid = oku["urunid"].ToString();
                    ürünadı.Text = oku["stokadi"].ToString();
                    ürünadid = oku["stokadi"].ToString();
                    urunidimiz = oku["urunid"].ToString();
                    fiyat.Text = x.ToString("#,##0.00");
                    ürünfiyat = oku["satisfiyat"].ToString();
                    if (oku["resim"].ToString() == "")
                    {
                        resim.Image = ımageList1.Images[0];
                    }
                    else
                    {
                        resim.Image = Image.FromFile(Application.StartupPath + "\\stokartıresim\\" + oku["resim"].ToString());

                    }


                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                baglanti.Close();
            }
        }
        public void vericek2()
        {
            try
            {
                baglanti.Close();
                baglanti.Open();
                string sql = "SELECT Stokkarti.stokadi,Stokkarti.satisfiyat,Stokkarti.resim,hızlısatısurunler.urunid,hızlısatısurunler.grupid " +
                    "FROM Stokkarti INNER JOIN hızlısatısurunler ON Stokkarti.id=hızlısatısurunler.urunid " +
                    "WHERE (((hızlısatısurunler.urunid) Like '" + urunidimiz + "') AND ((hızlısatısurunler.grupid) Like 1))";
                OleDbCommand veri = new OleDbCommand(sql, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    double x = Convert.ToDouble(oku["satisfiyat"].ToString());
                    ürünid = oku["urunid"].ToString();
                    ürünadı.Text = oku["stokadi"].ToString();
                    ürünadid = oku["stokadi"].ToString();
                    urunidimiz = oku["urunid"].ToString();
                    fiyat.Text = x.ToString("#,##0.00");
                    ürünfiyat = oku["satisfiyat"].ToString();
                    if (oku["resim"].ToString() == "")
                    {
                        resim.Image = ımageList1.Images[0];
                    }
                    else
                    {
                        resim.Image = Image.FromFile(Application.StartupPath + "\\stokartıresim\\" + oku["resim"].ToString());

                    }


                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                baglanti.Close();
            }
        }
        public void vericek3()
        {
            try
            {
                baglanti.Close();
                baglanti.Open();
                string sql = "SELECT Stokkarti.stokadi,Stokkarti.satisfiyat,Stokkarti.resim,hızlısatısurunler.urunid,hızlısatısurunler.grupid " +
                    "FROM Stokkarti INNER JOIN hızlısatısurunler ON Stokkarti.id=hızlısatısurunler.urunid " +
                    "WHERE (((hızlısatısurunler.urunid) Like '" + urunidimiz + "') AND ((hızlısatısurunler.grupid) Like 2))";
                OleDbCommand veri = new OleDbCommand(sql, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    double x = Convert.ToDouble(oku["satisfiyat"].ToString());
                    ürünid = oku["urunid"].ToString();
                    ürünadı.Text = oku["stokadi"].ToString();
                    ürünadid = oku["stokadi"].ToString();
                    urunidimiz = oku["urunid"].ToString();
                    fiyat.Text = x.ToString("#,##0.00");
                    ürünfiyat = oku["satisfiyat"].ToString();
                    if (oku["resim"].ToString() == "")
                    {
                        resim.Image = ımageList1.Images[0];
                    }
                    else
                    {
                        resim.Image = Image.FromFile(Application.StartupPath + "\\stokartıresim\\" + oku["resim"].ToString());

                    }


                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                baglanti.Close();
            }
        }
        public void vericek4()
        {
            try
            {
                baglanti.Close();
                baglanti.Open();
                string sql = "SELECT Stokkarti.stokadi,Stokkarti.satisfiyat,Stokkarti.resim,hızlısatısurunler.urunid,hızlısatısurunler.grupid " +
                    "FROM Stokkarti INNER JOIN hızlısatısurunler ON Stokkarti.id=hızlısatısurunler.urunid " +
                    "WHERE (((hızlısatısurunler.urunid) Like '" + urunidimiz + "') AND ((hızlısatısurunler.grupid) Like 3))";
                OleDbCommand veri = new OleDbCommand(sql, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    double x = Convert.ToDouble(oku["satisfiyat"].ToString());
                    ürünid = oku["urunid"].ToString();
                    ürünadı.Text = oku["stokadi"].ToString();
                    ürünadid = oku["stokadi"].ToString();
                    urunidimiz = oku["urunid"].ToString();
                    fiyat.Text = x.ToString("#,##0.00");
                    ürünfiyat = oku["satisfiyat"].ToString();
                    if (oku["resim"].ToString() == "")
                    {
                        resim.Image = ımageList1.Images[0];
                    }
                    else
                    {
                        resim.Image = Image.FromFile(Application.StartupPath + "\\stokartıresim\\" + oku["resim"].ToString());

                    }


                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                baglanti.Close();
            }
        }
        private void satisurun_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            vericek1();
            vericek2();
            vericek3();
            vericek4();
        }
        public void satispublic(string idimiz)
        {
            AnaForm frm = (AnaForm)Application.OpenForms["AnaForm"];
            frm.showsatis(idimiz);
        }

        private void resim_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.satispublic(ürünid);
        }
        public void sil()
        {
            try
            {
                baglanti.Close();
                OleDbCommand komut = new OleDbCommand("DELETE FROM hızlısatısurunler WHERE urunid LIKE '"+ürünid+"'",baglanti);
                baglanti.Open();
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
            catch
            {
                XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            sil();
            AnaForm deger= (AnaForm)Application.OpenForms["AnaForm"];
            deger.urunlistele1();
            deger.urunlistele2();
            deger.urunlistele3();
            deger.urunlistele4();
            
        }

        private void resim_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menü.ShowPopup(MousePosition);
            }
            else if (e.Button == MouseButtons.Left)
            {
                this.satispublic(ürünid);
            }
        }

        private void ürünadı_Click(object sender, EventArgs e)
        {

        }

    }
}
