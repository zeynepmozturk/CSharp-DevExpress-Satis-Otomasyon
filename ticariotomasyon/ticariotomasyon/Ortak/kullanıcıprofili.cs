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
    public partial class kullanıcıprofili : DevExpress.XtraEditors.XtraForm
    {
        string baglanticümlecigi, kulid;
        public kullanıcıprofili(string baglanticümlecigim, string kulidimiz)
        {
            InitializeComponent();
            baglanticümlecigi = baglanticümlecigim;
            kulid = kulidimiz;
        }
        OleDbConnection baglanti = new OleDbConnection();
        private void kullanıcıprofili_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticümlecigi.ToString();
            vericek();
        }
        public void vericek()
        {
            try
            {
                string sorgu = "SELECT*FROM kullanicilar WHERE kulid like '" + kulid.ToString() + "'";
                baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu,baglanti);
                OleDbDataReader oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    labelControl7.Text = oku["kulisim"].ToString() + " " + oku["kulsoyisim"].ToString();
                    labelControl8.Text = oku["kule_mail"].ToString();
                    labelControl9.Text = oku["kulil"].ToString();
                    labelControl10.Text = oku["kulilce"].ToString();
                    labelControl11.Text = oku["kulgsm"].ToString();
                    labelControl12.Text = oku["kuladres"].ToString();
                    if (oku["kulresim"].ToString() == "" && oku["cinsiyet"].ToString() == "Kadın")
                    {
                        pictureEdit1.Image = Image.FromFile(Application.StartupPath + "\\profil\\kadın.png");
                    }
                    else if (oku["kulresim"].ToString() == "" && oku["cinsiyet"].ToString() == "Erkek")
                    {
                        pictureEdit1.Image = Image.FromFile(Application.StartupPath + "\\profil\\bay.jpeg");
                    }
                    else
                    {
                        pictureEdit1.Image = Image.FromFile(Application.StartupPath + "\\profil\\" + oku["kulresim"].ToString());
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            sifredegis ac = new sifredegis(baglanticümlecigi,kulid);
            ac.ShowDialog();
        }
    }
}