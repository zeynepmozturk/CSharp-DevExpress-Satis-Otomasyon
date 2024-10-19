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
    public partial class profilguncelleme : DevExpress.XtraEditors.XtraForm
    {
        string baglanticümlecigi, kulid;
        public profilguncelleme(string baglanticümlecigim, string kulidimiz)
        {
            InitializeComponent();
            kulid = kulidimiz;
            baglanticümlecigi = baglanticümlecigim;
        }
        OleDbConnection baglanti = new OleDbConnection();
        private void profilguncelleme_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticümlecigi.ToString();
            vericek();
            comboBoxEdit1.Properties.Items.Add("Adana");
            comboBoxEdit1.Properties.Items.Add("Erzincan");
            comboBoxEdit1.Properties.Items.Add("Ankara");
            comboBoxEdit1.Properties.Items.Add("Bursa");
            comboBoxEdit1.Properties.Items.Add("Balıkesir");
            


        }
        string resimyolumuz, resimadimiz;
        public void vericek()
        {
            try
            {
                string sorgu = "SELECT * FROM kullanicilar WHERE kulid like'" + kulid.ToString() + "'";
                baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    textEdit6.Text = oku["kuladi"].ToString();
                    textEdit1.Text = oku["kulisim"].ToString();
                    textEdit2.Text = oku["kulsoyisim"].ToString();
                    textEdit3.Text = oku["kule_mail"].ToString();
                    textEdit4.Text= oku["kulgsm"].ToString();
                    comboBoxEdit1.Text = oku["kulil"].ToString();
                    textEdit5.Text = oku["kulilce"].ToString();
                    memoEdit1.Text = oku["kuladres"].ToString();
                    pictureEdit1.Image = GetCopyImage(Application.StartupPath + "\\profil\\"+oku["kulresim"].ToString()+"");
                    resimyolumuz = Application.StartupPath + "\\profil\\" + oku["kulresim"].ToString() + "";
                    resimadimiz = oku["kulresim"].ToString();
                    if (oku["cinsiyet"].ToString()=="Kadın")
                    {
                        radioGroup1.SelectedIndex = 0;
                    }
                    else if (oku["cinsiyet"].ToString() == "Erkek")
                    {
                        radioGroup1.SelectedIndex = 1;
                    }
                    if(oku["yedekdurumu"].ToString()=="Alınsın")
                    {
                        checkEdit1.Checked = true;
                        labelControl14.Text = oku["yedekyolu"].ToString();
                    }
                    else if (oku["yedekdurumu"].ToString() == "Alınmasın")
                    {
                        checkEdit1.Checked = false;
                    }
                    else 
                    {
                        checkEdit1.Checked = false;
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
        public Image GetCopyImage(string path)
        {
            using (Image im = Image.FromFile(path))
            {
                Bitmap bm = new Bitmap(im);
                return bm;
            }
        }
        OpenFileDialog dialog =new OpenFileDialog();
        string profilresim;
        public void guncelle()
        {
            DateTime dt= DateTime.Now;
            DateTime dateString = DateTime.Now;
            string saat = dateString.Hour.ToString();
            string dakika = dateString.Minute.ToString();
            string saniye = dateString.Second.ToString();
            string sn = saat + "." + dakika + "." + saniye + ".";
            string tarih = dt.ToString("dd.MM.yyyy");

            if (textEdit6.Text == "" || textEdit1.Text == "" || textEdit2.Text == "")
            {
                XtraMessageBox.Show("Yıldız ile gösterilen alanlar boş geçilemez \n  Lütfen yıldızlı alanları doldurun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else
            {
                try
                {
                    if (dialog.SafeFileName == "")
                    {
                        profilresim = resimadimiz.ToString();
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
                        string resimyolu = Application.StartupPath + "\\profil\\" + tarih + " " + sn + "." + dialog.SafeFileName;
                        FileInfo fi = new FileInfo(dialog.FileName);
                        fi.CopyTo(resimyolu);
                        profilresim = tarih + " " + sn + "." + dialog.SafeFileName;
                    }
                    try
                    {
                        baglanti.Open();
                        OleDbCommand sorgu = new OleDbCommand("UPDATE kullanicilar SET kuladi=@kuladi,kulisim=@kulisim,kulsoyisim=@kulsoyisim,  " +
                            "kule_mail=@kule_mail,kulgsm=@kulgsm,kulil=@kulil,kulilce=@kulilce,kuladres=@kuladres,cinsiyet=@cinsiyet,yedekdurumu=@yedekdurumu,yedekyolu=@yedekyolu,kulresim=@kulresim  " +
                            "WHERE kulid like'" + kulid.ToString() + "'", baglanti);
                        sorgu.Parameters.AddWithValue("kuladi", textEdit6.Text);
                        sorgu.Parameters.AddWithValue("kulisim", textEdit1.Text);
                        sorgu.Parameters.AddWithValue("kulsoyisim", textEdit2.Text);
                        sorgu.Parameters.AddWithValue("kule_mail", textEdit3.Text);
                        sorgu.Parameters.AddWithValue("kulgsm", textEdit4.Text);
                        sorgu.Parameters.AddWithValue("kulil", comboBoxEdit1.Text);
                        sorgu.Parameters.AddWithValue("kulilce", textEdit5.Text);
                        sorgu.Parameters.AddWithValue("kuladres", memoEdit1.Text);
                        if (radioGroup1.SelectedIndex == 0)
                        {
                            sorgu.Parameters.AddWithValue("cinsiyet", "Kadın");
                        }
                        else if (radioGroup1.SelectedIndex == 1)
                        {
                            sorgu.Parameters.AddWithValue("cinsiyet", "Erkek");
                        }
                        if (checkEdit1.Checked == true)
                        {
                            sorgu.Parameters.AddWithValue("yedekdurumu", "Alınsın");
                            sorgu.Parameters.AddWithValue("yedekyolu", labelControl14.Text);
                        }
                        else if (checkEdit1.Checked == false)
                        {
                            sorgu.Parameters.AddWithValue("yedekdurumu", "Alınmasın");
                            sorgu.Parameters.AddWithValue("yedekyolu", "");
                        }
                        sorgu.Parameters.AddWithValue("kulresim", profilresim.ToString());
                        if (sorgu.ExecuteNonQuery() == 1)
                        {
                            XtraMessageBox.Show("Güncelleme işlemi başarılı", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            Close();
                        }
                        else
                        {
                            XtraMessageBox.Show("Güncelleme işlemi başarısız", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                        }
                        baglanti.Close();
                    }
                    catch
                    {

                    }

                }
                catch
                {

                }
            }


        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            guncelle();
        }

   
        private void pictureEdit1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                DialogResult dr = new DialogResult();
                dialog.Filter="(*jpg)|*.jpg|(*png)|*.png";
                dr = dialog.ShowDialog();
                pictureEdit1.Image = Image.FromFile(dialog.FileName);

            }
            catch
            { 

            }

        }

        private void checkEdit1_MouseDown(object sender, MouseEventArgs e)
        {
            if(checkEdit1.Checked==false)
            {
                FolderBrowserDialog fdb = new FolderBrowserDialog();
                if(fdb.ShowDialog()==DialogResult.OK)
                {
                    labelControl14.Text = fdb.SelectedPath.ToString();
                    checkEdit1.Checked = true;
                }
            }
        }
 
    }
}