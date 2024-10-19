using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using Ini;
namespace ticariotomasyon
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public string baglanticümlesi;
        public Form1()
        {
            InitializeComponent();
        }
        static IniFile iniayar = new IniFile(Application.StartupPath.ToString() + "\\Hatırla.ini");
        public void baglan()
        {
            OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|Data.mdb;Jet OLEDB:Database Password=");
            baglanti.Open();
            baglanticümlesi = baglanti.ConnectionString.ToString();
            OleDbCommand sorgu = new OleDbCommand("SELECT * FROM kullanicilar WHERE kuladi='" + textEdit1.Text + "' AND kulsifre='" + textEdit2.Text + "'", baglanti);
            OleDbDataReader oku = sorgu.ExecuteReader();
            if (oku.Read())
            {
                AnaForm aç = new AnaForm(baglanticümlesi, oku["kulid"].ToString());
                AnaForm.ActiveForm.Visible = false;
                aç.ShowDialog();
            }
            else
            {
                MessageBox.Show("kullanıcı adı veye şifre hatalı");
            }
            oku.Close();
            baglanti.Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (checkEdit1.Checked == true)
            {
                iniayar.IniWriteValue("Hatırla", "Kullanıcı Adı", textEdit1.Text);
                iniayar.IniWriteValue("Hatırla", "Kullanıcı Şifresi", textEdit2.Text);
            }
            else
            {
                iniayar.IniWriteValue("Hatırla", "Kullanıcı Adı", "");
                iniayar.IniWriteValue("Hatırla", "Kullanıcı Şifresi", "");
            }
            hatırlayükle();
            baglan();
        }
        public void hatırlayükle()
        {
            if (iniayar.IniReadValue("Hatırla", "Kullanıcı Adı") == "")
            {
                checkEdit1.Checked = false;
            }
            else
            {
                checkEdit1.Checked = true;
                textEdit1.Text = iniayar.IniReadValue("Hatırla", "Kullanıcı Adı");
                textEdit2.Text = iniayar.IniReadValue("Hatırla", "Kullanıcı Şifresi");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hatırlayükle();
        }
    }
}
