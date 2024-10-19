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
    public partial class giderform : DevExpress.XtraEditors.XtraForm
    {
        string baglanticumlecigi, guncellemedurumu, giderid, kasaid;
        public giderform(string sqlcumlecigi, string gncldurum, string id, string kasaidimiz)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
            guncellemedurumu = gncldurum;
            giderid = id;
            kasaid = kasaidimiz;

        }
        public OleDbConnection baglanti = new OleDbConnection();
        public void gelirtürülistele()
        {
            try
            {
                string sorgu = "select id ,gelirgidertürü AS Gidertürü FROM gelirgidertürü WHERE tip like '1' ORDER BY id ASC";
                baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(string));
                dt.Columns.Add("Gidertürü", typeof(string));
                dt.Load(oku);
                lookUpEdit1.Properties.ValueMember = "id";
                lookUpEdit1.Properties.DisplayMember = "Gidertürü";
                lookUpEdit1.Properties.DataSource = dt;
                oku.Close();
                baglanti.Close();

            }
            catch
            {
                baglanti.Close();
            }
        }

        private void giderform_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            dateEdit1.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            dateEdit1.Text = DateTime.Now.ToString("dd.MM.yyyy");
            gelirtürülistele();
            if (guncellemedurumu == "1")
            {
                listele();
                simpleButton1.Text = "Güncelle";
            }
            else
            {
                simpleButton2.Text = "Kaydet";
            }
        }
        public void kaydet()
        {
            //try
            //{
            baglanti.Open();
            OleDbCommand komut = baglanti.CreateCommand();
            komut.CommandText = "insert into kasabankahareketleri (kasaid,kasagiristar,islemkod,durum,kasatutar,tahakuktip,acıklama,kasatip)" + "values (@kasaid,@kasagiristar,@islemkod,@durum,@kasatutar,@tahakuktip,@acıklama,@kasatip)";
            komut.Parameters.Add("kasaid", OleDbType.VarChar).Value = kasaid.ToString();
            komut.Parameters.Add("kasagiristar", OleDbType.VarChar).Value = dateEdit1.Text;
            komut.Parameters.Add("islemkod", OleDbType.VarChar).Value = textEdit2.Text;
            komut.Parameters.Add("durum", OleDbType.VarChar).Value = "GİDER";
            komut.Parameters.Add("kasatutar", OleDbType.VarChar).Value = Convert.ToDecimal(textEdit1.Text);
            komut.Parameters.Add("tahakuktip", OleDbType.VarChar).Value = lookUpEdit1.EditValue;
            komut.Parameters.Add("acıklama", OleDbType.VarChar).Value = memoEdit1.Text;
            komut.Parameters.Add("kasatip", OleDbType.VarChar).Value = "3";
            if (komut.ExecuteNonQuery() == 1)
            {
                XtraMessageBox.Show("kayıt işlemi gerçekleştirilmiştir...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                textEdit1.Text = "0";
                memoEdit1.Text = "";
                textEdit2.Text = "";
                AnaForm veri = (AnaForm)Application.OpenForms["AnaForm"];
                veri.kasalistesi();
                Close();
            }
            else
            {
                XtraMessageBox.Show("kayıt yapılamadı...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            //}
            //catch
            //{
            //    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            //}

        }
        public void guncelle()
        {
            try
            {
                baglanti.Close();
                string sqlguncelle = "UPDATE kasabankahareketleri SET kasagiristar=@kasagiristar,islemkod=@islemkod,kasatutar=@kasatutar,tahakuktip=@tahakuktip,acıklama=@acıklama" +
                    "WHERE id like '" + giderid.ToString() + "'";
                OleDbCommand komut = new OleDbCommand(sqlguncelle, baglanti);
                baglanti.Open();
                komut.Parameters.AddWithValue("kasagiristar", dateEdit1.Text);
                komut.Parameters.AddWithValue("islemkod", textEdit2.Text);
                komut.Parameters.AddWithValue("kasatutar", Convert.ToDecimal(textEdit1.Text));
                komut.Parameters.AddWithValue("tahakuktip", lookUpEdit1.EditValue);
                komut.Parameters.AddWithValue("acıklama", memoEdit1.Text);
                if (komut.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("kayıt düzeltildi...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    AnaForm veri = (AnaForm)Application.OpenForms["AnaForm"];
                    veri.kasalistesi();
                    Close();
                }
                else
                {
                    XtraMessageBox.Show("kayıt düzeltilemedi...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                Close();
            }
            catch
            {
                baglanti.Close();
                XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        public void listele()
        {
            try
            {
                baglanti.Close();
                string sorgu = "SELECT * FROM kasabankahareketleri WHERE id like '" + giderid.ToString() + "'";
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    double borcdegiisken = Convert.ToDouble(oku["kasatutar"].ToString());
                    dateEdit1.Text = oku["kasagiristar"].ToString();
                    textEdit2.Text = oku["islemkod"].ToString();
                    textEdit1.Text = borcdegiisken.ToString("#,##0.00");
                    lookUpEdit1.EditValue = oku["tahakuktip"].ToString();
                    memoEdit1.Text = oku["acıklama"].ToString();
                }
                oku.Close();
                baglanti.Close();

            }
            catch
            {
                baglanti.Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (guncellemedurumu == "0")
            {
                kaydet();
            }
            else if (guncellemedurumu == "1")
            {
                guncelle();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            gidertürüform ac = new gidertürüform(baglanticumlecigi);
            ac.ShowDialog();
            Close(); 
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            AnaForm veri = (AnaForm)Application.OpenForms["AnaForm"];
            veri.kasalistesitariharası();
            Close();
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            decimal x;
            x = Convert.ToDecimal(textEdit1.Text);
            x = Math.Round(x, 2);
            textEdit1.Text = Convert.ToString(x);
        }

        private void giderform_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnaForm veri = (AnaForm)Application.OpenForms["AnaForm"];
            veri.kasalistesitariharası();
        }

     
    }
}