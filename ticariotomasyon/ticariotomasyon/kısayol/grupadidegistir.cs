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
    public partial class grupadidegistir : DevExpress.XtraEditors.XtraForm
    {
        public kısayol ksylfrm;
        string baglanticumlecigi;
        public grupadidegistir(string sqlcumlecigi)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
        }
        public OleDbConnection baglanti= new OleDbConnection();
        private void grupadidegistir_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString=baglanticumlecigi.ToString();
            grupadı();
        }
        public void grupadı()
        {
            try
            {
                baglanti.Open();
                OleDbCommand veri = new OleDbCommand("SELECT * FROM hızlısatısgrupadi", baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    textEdit1.Text = oku["grupadi1"].ToString();
                    textEdit2.Text = oku["grupadi2"].ToString();
                    textEdit3.Text = oku["grupadi3"].ToString();
                    textEdit4.Text = oku["grupadi4"].ToString();

                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                baglanti.Close();
            }

       }
        public void guncelle()
        {
            try
            {
                baglanti.Close();
                string sqlguncelle="UPDATE hızlısatısgrupadi SET grupadi1=@grupadi1,grupadi2=@grupadi2,grupadi3=@grupadi3,grupadi4=@grupadi4";
                OleDbCommand komut=new OleDbCommand(sqlguncelle,baglanti);
                baglanti.Open();
                komut.Parameters.AddWithValue("grupadi1",textEdit1.Text);
                komut.Parameters.AddWithValue("grupadi2",textEdit2.Text);
                komut.Parameters.AddWithValue("grupadi3",textEdit3.Text);
                komut.Parameters.AddWithValue("grupadi4",textEdit4.Text);
                komut.ExecuteNonQuery();
                baglanti.Close();

            }
            catch
            {
                XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                baglanti.Close();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if(groupControl1.Text==""||groupControl2.Text==""||groupControl3.Text==""||groupControl4.Text=="")
            {
                XtraMessageBox.Show("(*) ile belirtilmiş alanlar boş geçilemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                guncelle();
                ksylfrm.kısayolderle(sender,e);
                Close();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}