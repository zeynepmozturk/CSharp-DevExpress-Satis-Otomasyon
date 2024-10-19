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
using System.IO;

namespace ticariotomasyon
{
    public partial class musteeriekle : DevExpress.XtraEditors.XtraForm
    {
        string baglanticumlecigi, kullaniciid, musterid,durum;
        public musteeriekle(string sqlcumlecigi,string kullaniciidsi,string musteriidmiz,string durumidimiz)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
            kullaniciid = kullaniciidsi;
            musterid = musteriidmiz;
            durum = durumidimiz;
        }
        public OleDbConnection baglanti = new OleDbConnection();
        int dönenid;
        string profilresim="";
        public Image GetCopyImage(string path)
        {
            using (Image im = Image.FromFile(path))
            {
                Bitmap bm = new Bitmap(im);
                return bm;
            }
        }
        OpenFileDialog myFileDialog = new OpenFileDialog();
        public void kaydet()
        {
            DateTime dt = DateTime.Now;
            DateTime datestring = DateTime.Now;
            string saat = datestring.Hour.ToString();
            string dakika = datestring.Minute.ToString();
            string saniye = datestring.Second.ToString();
            string sn = saat + "." + dakika + "." + saniye;
            string t = dt.ToString("dd.MM.yyyy");
            if (textEdit1.Text == "")
            {
                XtraMessageBox.Show("(*)ile belirtilmiş alanlar boş  \n geçilemez lütfen doldurunuz...", "Uyarı...");
            }
            else
            {
                //try
                //{
                if (myFileDialog.SafeFileName == "")
                {
                    if (profilresim == "")
                    {
                        profilresim = "";
                    }
                }
                else
                {
                    string resimyolu = Application.StartupPath + "\\musteriprofil\\" + t + "." + sn + "." + myFileDialog.SafeFileName;
                    FileInfo fi = new FileInfo(myFileDialog.FileName);
                    fi.CopyTo(resimyolu);
                    profilresim = t + "" + sn + "." + myFileDialog.SafeFileName;
                }
                baglanti.Open();
                OleDbCommand komut = baglanti.CreateCommand();
                komut.CommandText = "insert into musteriler(adsoyad,tckimlik,cep,eposta,dogumtar,meslek,kangrubu,adres,isadres,vergidairesi,vergino,aciklama,cinsiyet,profilresmi,aktiflik)" + "values(@adsoyad,@tckimlik,@cep,@eposta,@dogumtar,@meslek,@kangrubu,@adres,@isadres,@vergidairesi,@vergino,@aciklama,@cinsiyet,@profilresmi,@aktiflik)";
                komut.Parameters.Add("adsoyad", OleDbType.VarChar).Value = textEdit1.Text;
                komut.Parameters.Add("tckimlik", OleDbType.VarChar).Value = textEdit8.Text;
                komut.Parameters.Add("cep", OleDbType.VarChar).Value = textEdit2.Text;
                komut.Parameters.Add("eposta", OleDbType.VarChar).Value = textEdit11.Text;
                komut.Parameters.Add("dogumtar", OleDbType.VarChar).Value = textEdit3.Text;
                komut.Parameters.Add("meslek", OleDbType.VarChar).Value = textEdit7.Text;
                komut.Parameters.Add("kangrubu", OleDbType.VarChar).Value = comboBoxEdit1.Text;
                komut.Parameters.Add("adres", OleDbType.VarChar).Value = memoEdit1.Text;
                komut.Parameters.Add("isadres", OleDbType.VarChar).Value = memoEdit2.Text;
                komut.Parameters.Add("vergidairesi", OleDbType.VarChar).Value = textEdit4.Text;
                komut.Parameters.Add("vergino", OleDbType.VarChar).Value = textEdit10.Text;
                komut.Parameters.Add("aciklama", OleDbType.VarChar).Value = memoEdit3.Text;
                komut.Parameters.Add("cinsiyet", OleDbType.VarChar).Value = comboBoxEdit2.Text;
                komut.Parameters.Add("profilresmi", OleDbType.VarChar).Value = profilresim.ToString();
                komut.Parameters.Add("aktiflik", OleDbType.VarChar).Value = "1";
                if (komut.ExecuteNonQuery() == 1)
                {
                
                    XtraMessageBox.Show("Kayıt işlemi gerçekleştirilmiştir...", "bilgi");
                    textEdit1.Text = "";
                    textEdit8.Text = "";
                    textEdit2.Text = "";
                    textEdit11.Text = "";
                    textEdit3.Text = "";
                    textEdit7.Text = "";
                    textEdit4.Text = "";
                    textEdit10.Text = "";
                    comboBoxEdit1.Text = "";
                    comboBoxEdit2.Text = "";
                    memoEdit2.Text = "";
                    memoEdit1.Text = "";
                    memoEdit3.Text = "";

                }
                else
                {
                    XtraMessageBox.Show("kayıt yapılamadı", "bilgi");
                }
                baglanti.Close();
                //}
                //catch
                //{
                //    baglanti.Close();
                //     XtraMessageBox.Show("bir hata oluştu","uyarı");
                //}
            }
        }
        string resimadi,resimyolumuz;
        public void guncelle()
        {
             DateTime dt = DateTime.Now;
            DateTime dateString = DateTime.Now;
            string saat = dateString.Hour.ToString();
            string dakika = dateString.Minute.ToString();
            string saniye = dateString.Second.ToString();
            string sn = saat + "." + dakika + "." + saniye + ".";
            string tarih = dt.ToString("dd.MM.yyyy");
            if (textEdit1.Text == "")
            {
                XtraMessageBox.Show("yıldız(*) ile gösterilen alanlar boş geçilmez \n Lütfen bilgileri tam giriniz...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            }
            else
            {
                try
                {
                    try
                    {
                        if (myFileDialog.SafeFileName == "")
                        {
                            profilresim = resimadi.ToString();
                        }
                        else
                        {
                            try
                            {
                                File.Delete(resimyolumuz.ToString());
                            }
                            catch
                            {
                            }

                            string resimyolu = Application.StartupPath + "\\musteriprofil\\" + tarih + " " + sn + "." + myFileDialog.SafeFileName;
                            FileInfo fi = new FileInfo(myFileDialog.FileName);
                            fi.CopyTo(resimyolu);
                            profilresim = tarih + " " + sn + "." + myFileDialog.SafeFileName;
                        }
                    }
                    catch
                    {

                    }
                    baglanti.Close();

                    string sorgu = "UPDATE musteriler SET adsoyad=@adsoyad,tckimlik=@tckimlik,cep=@cep,  " +
                               "eposta=@eposta,dogumtar=@dogumtar,meslek=@meslek,kangrubu=@kangrubu,vergidairesi=@vergidairesi,vergino=@vergino,adres=@adres,isadres=@isadres,aciklama=@aciklama,cinsiyet=@cinsiyet,profilresmi=@profilresmi " +
                               "WHERE id like'" + musterid.ToString() + "'";
                    OleDbCommand komut = new OleDbCommand(sorgu, baglanti);
                    baglanti.Open();
                    komut.Parameters.AddWithValue("adsoyad", textEdit1.Text);
                    komut.Parameters.AddWithValue("tckimlik", textEdit8.Text);
                    komut.Parameters.AddWithValue("cep", textEdit2.Text);
                    komut.Parameters.AddWithValue("eposta", textEdit11.Text);
                    komut.Parameters.AddWithValue("dogumtar", textEdit3.Text);
                    komut.Parameters.AddWithValue("meslek", textEdit7.Text);
                    komut.Parameters.AddWithValue("kangrubu", comboBoxEdit1.Text);
                    komut.Parameters.AddWithValue("vergidairesi", textEdit4.Text);
                    komut.Parameters.AddWithValue("vergino", textEdit10.Text);
                    komut.Parameters.AddWithValue("adres", memoEdit1.Text);
                    komut.Parameters.AddWithValue("isadres", memoEdit2.Text);
                    komut.Parameters.AddWithValue("aciklama", memoEdit3.Text);
                    komut.Parameters.AddWithValue("cinsiyet", comboBoxEdit2.Text);

                    komut.Parameters.AddWithValue("profilresmi", profilresim.ToString());
                    if (komut.ExecuteNonQuery() == 1)
                    {
                        XtraMessageBox.Show("Güncelleme işlemi gerçekleştirilmiştir", "Bilgi...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        XtraMessageBox.Show("güncelleme yapılamadı", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                catch
                {
                    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu.", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
            
        }
        private void musteeriekle_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            if(durum=="0")
            {
                
            }
            else if(durum=="1")
            {
                simpleButton1.Text = "Güncelle";
                doldur();
            }
            comboBoxEdit1.Properties.Items.Add("Erkek");
            comboBoxEdit1.Properties.Items.Add("Kadın");
            comboBoxEdit2.Properties.Items.Add("A Rh+");
            comboBoxEdit2.Properties.Items.Add("A Rh-");
            comboBoxEdit2.Properties.Items.Add("B Rh+");
            comboBoxEdit2.Properties.Items.Add("B Rh-");
            comboBoxEdit2.Properties.Items.Add("AB Rh+");
            comboBoxEdit2.Properties.Items.Add("AB Rh-");
            comboBoxEdit2.Properties.Items.Add("0 Rh+");
            comboBoxEdit2.Properties.Items.Add("0 Rh-");
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (durum == "0")
            {
                kaydet();
               
            }
            else if (durum == "1")
            {
                guncelle();
            }
        }
        public void doldur()
        {
           
            try
            {
                string sorgu = "SELECT * FROM musteriler WHERE id like'" + musterid.ToString() + "'";
                baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    textEdit1.Text = oku["adsoyad"].ToString();
                    textEdit8.Text = oku["tckimlik"].ToString();
                    textEdit2.Text = oku["cep"].ToString();
                    textEdit11.Text = oku["eposta"].ToString();
                    textEdit3.Text = oku["dogumtar"].ToString();
                    textEdit7.Text = oku["meslek"].ToString();
                    comboBoxEdit1.Text = oku["kangrubu"].ToString();
                    textEdit4.Text = oku["vergidairesi"].ToString();
                    textEdit10.Text = oku["vergino"].ToString();
                    memoEdit1.Text = oku["adres"].ToString();
                    memoEdit2.Text = oku["isadres"].ToString();
                    memoEdit3.Text = oku["aciklama"].ToString();
                    comboBoxEdit2.Text = oku["cinsiyet"].ToString();
                    pictureEdit1.Image = GetCopyImage(Application.StartupPath + "\\musteriprofil\\" + oku["profilresmi"].ToString() + "");
                    resimyolumuz = Application.StartupPath + "\\musteriprofil\\" + oku["profilresmi"].ToString() + "";
                    resimadi = oku["profilresmi"].ToString();



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
            Close();
        }

        private void pictureEdit1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DialogResult dr = new DialogResult();
                myFileDialog.Filter = "(*jpg)|*.jpg|(*.png)|*.png";
                dr = myFileDialog.ShowDialog();
                pictureEdit1.Image = Image.FromFile(myFileDialog.FileName);
            }
            catch
            {

            }
        }
    }
}