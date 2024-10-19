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
    public partial class kısayol : DevExpress.XtraEditors.XtraForm
    {
        public AnaForm hızlıfrm;
        string baglanticumlecigi;
        public kısayol(string sqlcumlecigi)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
        }
        public OleDbConnection baglanti=new OleDbConnection();
        private void kısayol_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString=baglanticumlecigi.ToString();
            grupadı();
        }
        public void grupadı()
        {
            try
            {
                baglanti.Open();
                OleDbCommand veri=new OleDbCommand("SELECT * FROM hızlısatısgrupadi",baglanti);
                OleDbDataReader oku=null;
                oku=veri.ExecuteReader();
                while(oku.Read())
                {
                    radioGroup1.Properties.Items[0].Description=oku["grupadi1"].ToString();
                    radioGroup1.Properties.Items[1].Description=oku["grupadi2"].ToString();
                    radioGroup1.Properties.Items[2].Description=oku["grupadi3"].ToString();
                    radioGroup1.Properties.Items[3].Description=oku["grupadi4"].ToString();
                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                baglanti.Close();
            }

        }
        public void kaydet()
        {
            try
            {
            DateTime dt=new DateTime();
            string t=dt.ToString("dd.MM.yyyy");
            if(labelControl1.Text=="")
            {
                 XtraMessageBox.Show("Lütfen kısa yola eklemek istediğiniz ürünü seçin... " , "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                OleDbCommand komut=baglanti.CreateCommand();
                komut.CommandText="insert into hızlısatısurunler (grupid,urunid)"+"values(@grupid,@urunid)";
                komut.Parameters.AddWithValue("grupid",radioGroup1.SelectedIndex.ToString());
                komut.Parameters.Add("urunid",OleDbType.VarChar).Value=labelControl1.Text;
                baglanti.Open();
                if(komut.ExecuteNonQuery()==1)
                {
                     XtraMessageBox.Show("Kayıt işlemi gerçekleştirilmiştir...", "Bilgi...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    textEdit1.Text="";
                    labelControl1.Text="";
                }
                else
                {
                     XtraMessageBox.Show("kayıt işlemi gerçekleştirilemedi...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    baglanti.Close();
                }
            }
            }
            catch
            {
                 XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
           kaydet();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            grupadidegistir ac=new grupadidegistir(baglanticumlecigi);
            ac.ksylfrm=this;
            ac.ShowDialog();
        }
        public void kısayolderle(object sender, EventArgs e)
        {
            grupadı();
        }
        private void kısayol_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnaForm deger=(AnaForm)Application.OpenForms["AnaForm"];
            deger.urunlistele1();
            deger.urunlistele2();
            deger.urunlistele3();
            deger.urunlistele4();
            hızlıfrm.kısayolderle(sender, e);

       

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            AnaForm deger = (AnaForm)Application.OpenForms["AnaForm"];
            deger.urunlistele1();
            deger.urunlistele2();
            deger.urunlistele3();
            deger.urunlistele4();
            hızlıfrm.kısayolderle(sender, e);
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            kısayolürünara ac = new kısayolürünara(baglanticumlecigi);
            ac.ksylfrm = this;
            ac.ShowDialog();
        }

    }
}