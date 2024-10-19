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
    public partial class urunkayıt : DevExpress.XtraEditors.XtraForm
    {
        string baglanticümlecigi, kulllaniciid, stokid, durum;
        public urunkayıt(string sqlcumlecgi, string kullaniciidsi, string stokidimiz, string durumidimiz)
        {
            InitializeComponent();
            baglanticümlecigi = sqlcumlecgi;
            kulllaniciid = kullaniciidsi;
            stokid = stokidimiz;
            durum = durumidimiz;
        }
        OleDbConnection baglanti = new OleDbConnection();
        OpenFileDialog myFileDialog= new OpenFileDialog();
        string profilresim = "";
        public void kaydet()
        {
            DateTime dt = DateTime.Now;
            DateTime datestring = DateTime.Now;
            string saat = datestring.Hour.ToString();
            string dakika = datestring.Minute.ToString();
            string saniye = datestring.Second.ToString();
            string sn = saat + "." + dakika + "." + saniye;
            string t = dt.ToString("dd.MM.yyyy");
            if (textEdit1.Text == "" || textEdit5.Text == null)
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
                        string resimyolu = Application.StartupPath + "\\stokkartıresim\\" + t + "." + sn + "." + myFileDialog.SafeFileName;
                        FileInfo fi = new FileInfo(myFileDialog.FileName);
                        fi.CopyTo(resimyolu);
                        profilresim = t + "" + sn + "." + myFileDialog.SafeFileName;
                    }
                    baglanti.Open();
                    OleDbCommand komut = baglanti.CreateCommand();
                    komut.CommandText = "insert into Stokkarti(stokadi,barkod,stokcinsi,alisfiyat,satisfiyat,tedarikci,tedarikcitel,tedarikciaciklama,tedarikciadres,kayittar,stoktipi,depo,resim,aktiflik)" + "values(@stokadi,@barkod,@stokcinsi,@alisfiyat,@satisfiyat,@tedarikci,@tedarikcitel,@tedarikciaciklama,@tedarikciadres,@kayittar,@stoktipi,@depo,@resim,@aktiflik)";
                    komut.Parameters.Add("stokadi", OleDbType.VarChar).Value = textEdit1.Text;
                    komut.Parameters.Add("barkod", OleDbType.VarChar).Value = textEdit2.Text;
                    komut.Parameters.Add("stokcinsi", OleDbType.VarChar).Value = lookUpEdit2.EditValue;
                    komut.Parameters.Add("alisfiyat", OleDbType.VarChar).Value = textEdit4.Text;
                    komut.Parameters.Add("satisfiyat", OleDbType.VarChar).Value = textEdit5.Text;
                    komut.Parameters.Add("tedarikci", OleDbType.VarChar).Value = textEdit3.Text;
                    komut.Parameters.Add("tedarikcitel", OleDbType.VarChar).Value = textEdit6.Text;
                    komut.Parameters.Add("tedarikciaciklama", OleDbType.VarChar).Value = memoEdit2.Text;
                    komut.Parameters.Add("tedarikciadres", OleDbType.VarChar).Value = memoEdit1.Text;
                    komut.Parameters.Add("kayittar", OleDbType.VarChar).Value = DateTime.Now.ToString("dd/mm/yyyy");
                    if(radioGroup1.SelectedIndex.ToString()=="0")
                    {
                        komut.Parameters.Add("stoktipi", OleDbType.VarChar).Value = "Sarfiyat";
                    }
                    else  if(radioGroup1.SelectedIndex.ToString()=="1")
                    {
                        komut.Parameters.Add("stoktipi", OleDbType.VarChar).Value = "Demirbaş";
                    }
                    komut.Parameters.Add("depo", OleDbType.VarChar).Value = lookUpEdit1.EditValue;
                    komut.Parameters.Add("resim", OleDbType.VarChar).Value = profilresim.ToString();
                    komut.Parameters.Add("aktiflik", OleDbType.VarChar).Value = "1";
                    if(komut.ExecuteNonQuery()==1)
                    {
                        XtraMessageBox.Show("Kayıt işlemi gerçekleştirilmiştir...","bilgi");
                        textEdit1.Text="";
                        textEdit2.Text="";
                        textEdit4.Text="";
                        textEdit5.Text="";
                        textEdit3.Text="";
                        textEdit6.Text="";
                        memoEdit2.Text="";
                        memoEdit1.Text="";

                    }
                    else{
                        XtraMessageBox.Show("kayıt yapılamadı","bilgi");
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
        public void depolistele()
        {
            string sorgu = "SELECT id,depo FROM Stokdepo";
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("id",typeof(string));
            dt.Columns.Add("depo",typeof(string));
            dt.Load(oku);
            lookUpEdit1.Properties.ValueMember = "id";
            lookUpEdit1.Properties.DisplayMember = "depo";
            lookUpEdit1.Properties.DataSource = dt;
            oku.Close();
            baglanti.Close();
            

        }
        public void stokcinsilistele()
        {
            string sorgu = "SELECT id,stokcinsi FROM Stokcinsi";
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("stokcinsi", typeof(string));
            dt.Load(oku);
            lookUpEdit2.Properties.ValueMember = "id";
            lookUpEdit2.Properties.DisplayMember = "stokcinsi";
            lookUpEdit2.Properties.DataSource = dt;
            oku.Close();
            baglanti.Close();


        }
        string resimyolumuz,resimadi;
        public void guncelle()
        {
            DateTime dt = DateTime.Now;
            DateTime dateString = DateTime.Now;
            string saat = dateString.Hour.ToString();
            string dakika = dateString.Minute.ToString();
            string saniye = dateString.Second.ToString();
            string sn = saat + "." + dakika + "." + saniye + ".";
            string tarih = dt.ToString("dd.MM.yyyy");
            if (textEdit1.Text == "" || textEdit5.Text == null)
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

                            string resimyolu = Application.StartupPath + "\\stokartıresim\\" + tarih + " " + sn + "." + myFileDialog.SafeFileName;
                            FileInfo fi = new FileInfo(myFileDialog.FileName);
                            fi.CopyTo(resimyolu);
                            profilresim = tarih + " " + sn + "." + myFileDialog.SafeFileName;
                        }
                    }
                    catch
                    {

                    }
                    baglanti.Close();
                    baglanti.Open();
                    OleDbCommand sorgu = new OleDbCommand("UPDATE Stokkarti SET stokadi=@stokadi,barkod=@barkod,stokcinsi=@stokcinsi,  " +
                               "alisfiyat=@alisfiyat,satisfiyat=@satisfiyat,tedarikci=@tedarikci,tedarikcitel=@tedarikcitel,tedarikciaciklama=@tedarikciaciklama,tedarikciadres=@tedarikciadres,stoktipi=@stoktipi,depo=@depo,resim=@resim " +
                               "WHERE id like'" + stokid.ToString() + "'", baglanti);

                    sorgu.Parameters.AddWithValue("stokadi", textEdit1.Text);
                    sorgu.Parameters.AddWithValue("barkod", textEdit2.Text);
                    sorgu.Parameters.AddWithValue("stokcinsi", lookUpEdit2.EditValue);
                    sorgu.Parameters.AddWithValue("alisfiyat", textEdit4.Text);
                    sorgu.Parameters.AddWithValue("satisfiyat", textEdit5.Text);
                    sorgu.Parameters.AddWithValue("tedarikci", textEdit3.Text);
                    sorgu.Parameters.AddWithValue("tedarikcitel", textEdit6.Text);
                    sorgu.Parameters.AddWithValue("tedarikciaciklama", memoEdit2.Text);
                    sorgu.Parameters.AddWithValue("tedarikciadres", memoEdit1.Text);
                    if (radioGroup1.SelectedIndex == 0)
                    {
                        sorgu.Parameters.AddWithValue("stoktipi", "Sarfiyat");
                    }
                    else if (radioGroup1.SelectedIndex == 1)
                    {
                        sorgu.Parameters.AddWithValue("stoktipi", "Demirbaş");
                    }
                    sorgu.Parameters.AddWithValue("depo", lookUpEdit1.EditValue);
                    sorgu.Parameters.AddWithValue("resim", profilresim.ToString());


                    if (sorgu.ExecuteNonQuery() == 1)
                    {
                        XtraMessageBox.Show("Güncelleme işlemi gerçekleştirilmiştir", "Bilgi...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        textEdit1.Text = "";
                        textEdit2.Text = "";
                        textEdit4.Text = "";
                        textEdit5.Text = "";
                        textEdit3.Text = "";
                        textEdit6.Text = "";
                        memoEdit2.Text = "";
                        memoEdit1.Text = "";
                        simpleButton1.Text = "Kaydet";

                    }
                    else
                    {
                        XtraMessageBox.Show("güncelleme yapılamadı", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    baglanti.Close();
                }
                catch
                {
                    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu.", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
        
            }
        }
        public void vericek()
        {
            try
            {
                string sorgu = "SELECT * FROM Stokkarti WHERE id like'" + stokid.ToString() + "'";
                baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    textEdit1.Text = oku["stokadi"].ToString();
                    textEdit2.Text = oku["barkod"].ToString();
                    lookUpEdit2.EditValue = oku["stokcinsi"].ToString();
                    textEdit4.Text = oku["alisfiyat"].ToString();
                    textEdit5.Text = oku["satisfiyat"].ToString();
                    textEdit3.Text = oku["tedarikci"].ToString();
                    textEdit6.Text = oku["tedarikcitel"].ToString();
                    memoEdit2.Text = oku["tedarikciaciklama"].ToString();
                    memoEdit1.Text = oku["tedarikciadres"].ToString();
                     if (oku["stoktipi"].ToString() == "Sarfiyat")
                     {
                         radioGroup1.SelectedIndex = 0;
                     }
                     else if (oku["stoktipi"].ToString() == "Demirbaş")
                     {
                         radioGroup1.SelectedIndex = 1;
                     }
                     lookUpEdit1.EditValue = oku["depo"].ToString();
                     pictureEdit1.Image = GetCopyImage(Application.StartupPath + "\\stokartıresim\\" + oku["resim"].ToString() + "");
                     resimyolumuz = Application.StartupPath + "\\stokartıresim\\" + oku["resim"].ToString() + "";
                     resimadi = oku["resim"].ToString();



                }
                oku.Close();
                baglanti.Close();

            }
            catch
            {
                baglanti.Close();

            }
        }
        public Image GetCopyImage(string path)
        {
            using (Image im = Image.FromFile(path))
            {
                Bitmap bm = new Bitmap(im);
                return bm;
            }
        }
        
       
        private void urunkayıt_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticümlecigi.ToString();
            depolistele();
            stokcinsilistele();
            if (durum.ToString() == "1")
            {
                simpleButton1.Text = "Kaydet";
            }
            else if (durum.ToString() == "2")
            {
                simpleButton1.Text = "Güncelle";
                vericek();
            }
            
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            stokdepo ac = new stokdepo(baglanticümlecigi,kulllaniciid);
            ac.ShowDialog();

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            stokcinsi ac = new stokcinsi(baglanticümlecigi,kulllaniciid);
            ac.ShowDialog();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (durum.ToString() == "1")
            {
                kaydet();
            }
            else if (durum.ToString() == "2")
            {                
                guncelle();
                Close();
            }

        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

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