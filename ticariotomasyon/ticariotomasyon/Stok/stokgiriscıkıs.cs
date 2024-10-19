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
    public partial class stokgiriscıkıs : DevExpress.XtraEditors.XtraForm
    {
        string baglanticumlecigi, kullaniciid, stokid;
        public stokgiriscıkıs(string sqlcumlecigi,string kullaniciidsi,string stokidimiz)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
            kullaniciid = kullaniciidsi;
            stokid = stokidimiz;


        }
        public OleDbConnection baglanti = new OleDbConnection();
        private void stokgiriscıkıs_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            dateEdit1.Properties.DisplayFormat.FormatString = "dd.MM.yyyy";
            dateEdit1.Text = DateTime.Now.ToString("dd.MM.yyyy");
            comboBoxEdit1.Text = "Giriş";
            comboBoxEdit1.Properties.Items.Add("Giriş");
            comboBoxEdit1.Properties.Items.Add("Çıkış");
        }
        public void kaydet()
        {
            if (textEdit1.Text == "" || comboBoxEdit1.Text == "" || dateEdit1.Text == "")
            {
                XtraMessageBox.Show("yıldız(*) ile gösterilen alanlar boş geçilmez \n Lütfen bilgileri tam giriniz...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                try
                {
                    baglanti.Open();
                    OleDbCommand komut = baglanti.CreateCommand();
                    komut.CommandText = "insert into StokHareketi (Stokid,hareketturu,miktar,aciklama,tarih)" + "values(@Stokid,@hareketturu,@miktar,@aciklama,@tarih)";
                    komut.Parameters.Add("Stokid",OleDbType.VarChar).Value=stokid;
                    komut.Parameters.Add("hareketturu", OleDbType.VarChar).Value = comboBoxEdit1.Text;
                    komut.Parameters.Add("miktar", OleDbType.VarChar).Value = textEdit1.Text;
                    komut.Parameters.Add("aciklama", OleDbType.VarChar).Value = memoEdit1.Text;
                    komut.Parameters.Add("tarih", OleDbType.VarChar).Value = dateEdit1.Text;
                    if (komut.ExecuteNonQuery() == 1)
                    {
                        XtraMessageBox.Show("Kayıt işlemi gerçekleştirilmiştir...", "bilgi");
                        Close();
                    }
                    else
                    {
                        XtraMessageBox.Show("kayıt yapılamadı", "hata");
                    }
                    baglanti.Close();

                }
                catch
                {
                    baglanti.Close();
                    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu", "Uyarı...");
                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            kaydet();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}