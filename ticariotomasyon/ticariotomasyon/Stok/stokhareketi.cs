using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.OleDb;
namespace ticariotomasyon
{
    public partial class stokhareketi : DevExpress.XtraEditors.XtraForm
    {
        string baglanticumlecigi, kullaniciid, stokid;
        public stokhareketi(string sqlcumlecigi, string kullaniciidsi, string stokidmiz)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
            kullaniciid = kullaniciidsi;
            stokid = stokidmiz;

        }
        public OleDbConnection baglanti = new OleDbConnection();
        string gün, ay, yıl, ayson, gunson;
        private void stokhareketi_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            comboBoxEdit1.Text = "Tümü";
            comboBoxEdit1.Properties.Items.Add("Tümü");
            comboBoxEdit1.Properties.Items.Add("Giriş");
            comboBoxEdit1.Properties.Items.Add("Çıkış");
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            yıl = DateTime.Now.ToString("yyyy");
            ay = "01.";
            gün = "01.";
            ayson = "12.";
            gunson = "31.";

            dateTimePicker1.Text = gün + ay + yıl;
            dateTimePicker2.Text = gunson + ayson + yıl;
            hareketler();
        }
        string sorgu = null;
        public void hareketler()
        {
            string vFDate, VTDate;
            vFDate = dateTimePicker1.Value.ToString("MM-dd-yyyy");
            VTDate = dateTimePicker2.Value.ToString("MM-dd-yyyy");
            if(comboBoxEdit1.Text=="Tümü" && checkEdit1.Checked==false)
            {
                sorgu = "SELECT Stokkarti.stokadi, Stokhareketi.hareketturu, Stokhareketi.miktar, Stokhareketi.tarih, Stokhareketi.aciklama, Stokhareketi.id, Stokkarti.id, Stokkarti.stoktipi, Stokcinsi.stokcinsi "+
                        "FROM Stokcinsi INNER JOIN (Stokhareketi INNER JOIN Stokkarti ON Stokhareketi.Stokid = Stokkarti.id) ON Stokcinsi.id = Stokkarti.stokcinsi "+
                        "WHERE (((Stokkarti.id) Like '" + stokid + "')) " +
                        "ORDER BY DateValue(Stokhareketi.tarih) DESC";

            }
            else if (comboBoxEdit1.Text == "Giriş" && checkEdit1.Checked == false)
            {
                sorgu = "SELECT Stokkarti.stokadi,Stokhareketi.hareketturu,Stokhareketi.miktar,Stokhareketi.tarih,Stokhareketi.aciklama,Stokhareketi.id,Stokkarti.id,Stokkarti.stoktipi,Stokcinsi.stokcinsi " +
                "FROM Stokcinsi INNER JOIN (Stokhareketi INNER JOIN Stokkarti ON Stokhareketi.Stokid = Stokkarti.id) ON Stokcinsi.id=Stokkarti.stokcinsi " +
                "WHERE (((Stokhareketi.hareketturu) Like 'Giriş') AND ((Stokkarti.id) Like'"+stokid+"')) " +
                "ORDER BY DateValue(Stokhareketi.tarih) DESC";

            }
            else if (comboBoxEdit1.Text == "Çıkış" && checkEdit1.Checked == false)
            {
                sorgu = "SELECT Stokkarti.stokadi,Stokhareketi.hareketturu,Stokhareketi.miktar,Stokhareketi.tarih,Stokhareketi.aciklama,Stokhareketi.id,Stokkarti.id,Stokkarti.stoktipi,Stokcinsi.stokcinsi " +
                "FROM Stokcinsi INNER JOIN (Stokhareketi INNER JOIN Stokkarti ON Stokhareketi.Stokid = Stokkarti.id) ON Stokcinsi.id=Stokkarti.stokcinsi " +
                "WHERE (((Stokhareketi.hareketturu) Like 'Çıkış') AND ((Stokkarti.id) Like'" + stokid + "')) " +
                "ORDER BY DateValue(Stokhareketi.tarih) DESC";

            }
            else if (comboBoxEdit1.Text == "Giriş" && checkEdit1.Checked == true)
            {
                sorgu = "SELECT Stokkarti.stokadi,Stokhareketi.hareketturu,Stokhareketi.miktar,Stokhareketi.tarih,Stokhareketi.aciklama,Stokhareketi.id,Stokkarti.id,Stokkarti.stoktipi,Stokcinsi.stokcinsi " +
                "FROM Stokcinsi INNER JOIN (Stokhareketi INNER JOIN Stokkarti ON Stokhareketi.Stokid = Stokkarti.id) ON Stokcinsi.id=Stokkarti.stokcinsi " +
                "WHERE (((Stokhareketi.hareketturu) Like 'Giriş') AND ((Stokkarti.id) Like'" + stokid + "') AND DateValue(Stokhareketi.tarih) BETWEEN #"+vFDate+"# AND #"+VTDate+"# ) " +
                "ORDER BY DateValue(Stokhareketi.tarih) DESC";

            }
            else if (comboBoxEdit1.Text == "Çıkış" && checkEdit1.Checked == true)
            {
                sorgu = "SELECT Stokkarti.stokadi,Stokhareketi.hareketturu,Stokhareketi.miktar,Stokhareketi.tarih,Stokhareketi.aciklama,Stokhareketi.id,Stokkarti.id,Stokkarti.stoktipi,Stokcinsi.stokcinsi " +
                "FROM Stokcinsi INNER JOIN (Stokhareketi INNER JOIN Stokkarti ON Stokhareketi.Stokid = Stokkarti.id) ON Stokcinsi.id=Stokkarti.stokcinsi " +
                "WHERE (((Stokhareketi.hareketturu) Like 'Çıkış') AND ((Stokkarti.id) Like'" + stokid + "') AND DateValue(Stokhareketi.tarih) BETWEEN #" + vFDate + "# AND #" + VTDate + "# ) " +
                "ORDER BY DateValue(Stokhareketi.tarih) DESC";

            }
            else if (comboBoxEdit1.Text == "Tümü" && checkEdit1.Checked == true)
            {
                sorgu = "SELECT Stokkarti.stokadi,Stokhareketi.hareketturu,Stokhareketi.miktar,Stokhareketi.tarih,Stokhareketi.aciklama,Stokhareketi.id,Stokkarti.id,Stokkarti.stoktipi,Stokcinsi.stokcinsi " +
                "FROM Stokcinsi INNER JOIN (Stokhareketi INNER JOIN Stokkarti ON Stokhareketi.Stokid = Stokkarti.id) ON Stokcinsi.id=Stokkarti.stokcinsi " +
                "WHERE ((Stokkarti.id) Like '" + stokid + "') AND DateValue(Stokhareketi.tarih) BETWEEN #" + vFDate + "# AND #" + VTDate + "# " +
                "ORDER BY DateValue(Stokhareketi.tarih) DESC";

            }
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Stok Adı", Type.GetType("System.String"));
            dt.Columns.Add("Cinsi", Type.GetType("System.String"));
            dt.Columns.Add("Miktar", Type.GetType("System.String"));
            dt.Columns.Add("Tipi", Type.GetType("System.String"));
            dt.Columns.Add("İşlem Türü", Type.GetType("System.String"));
            dt.Columns.Add("Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("Ekleme tarihi", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["stokadi"].ToString();
                dr[1] = oku["stokcinsi"].ToString();
                dr[2] = oku["miktar"].ToString();
                dr[3] = oku["stoktipi"].ToString();
                dr[4] = oku["hareketturu"].ToString();
                dr[5] = oku["aciklama"].ToString();
                dr[6] = oku["tarih"].ToString();
                dr[7] = oku["Stokhareketi.id"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl1.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView1.Columns["id"].Visible = false; 

           

            
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            hareketler();
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkEdit1.Checked==true)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;

            }
            else if (checkEdit1.Checked == false)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;

            }
           
        }

        
    }
}