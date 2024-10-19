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
    public partial class stokdepo : DevExpress.XtraEditors.XtraForm
    {
        string baglanticümlecigi, kulllaniciid;
        public stokdepo(string sqlcumlecgi, string kullaniciidsi)
        {
            InitializeComponent();
            baglanticümlecigi = sqlcumlecgi;
            kulllaniciid = kullaniciidsi;


        }
        OleDbConnection baglanti = new OleDbConnection();
        string güncellemeid;
        private void stokdepo_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticümlecigi.ToString();
            listele();
        }
        public void kaydet()
        {
            if (textEdit1.Text == "")
            {
                XtraMessageBox.Show("(*)ile belirtilmiş alanlar boş  \n geçilemez lütfen doldurunuz...", "Uyarı...");
            }
            else
            {
                //try
                //{
                    baglanti.Open();
                    OleDbCommand komut = baglanti.CreateCommand();
                    komut.CommandText="insert into Stokdepo (depo,eklemetar)"+"values(@depo,@eklemetar)";
                    komut.Parameters.Add("depo", OleDbType.VarChar).Value = textEdit1.Text;
                    komut.Parameters.Add("eklemetar", OleDbType.VarChar).Value = DateTime.Now.ToString("dd/mm/yyyy");
                    if (komut.ExecuteNonQuery() == 1)
                    {
                        XtraMessageBox.Show("(Kayıt işlemi gerçekleştirilmiştir", "Bilgi...");
                        textEdit1.Text = "";
                        listele();
                    }
                    else
                    {
                        XtraMessageBox.Show("kayıt yapılamadı", "Hata...");
                    }
                    baglanti.Close();
                //}
                    
                //catch
                //{
                //    baglanti.Close();
                //    XtraMessageBox.Show("Bilinmeyen bir nedenden dolayı hata oluştu \n Lütfen programı kapatıp tekrar açınız", "Uyarı...");
                //}
            }
        }
        public void listele()
        {
            try
            {
                baglanti.Close();
                string sorgu= "SELECT * FROM Stokdepo ORDER BY id DESC";
                if(baglanti.State != ConnectionState.Open) baglanti.Open();
                OleDbCommand veri=new OleDbCommand(sorgu,baglanti);
                OleDbDataReader oku=null;
                oku=veri.ExecuteReader();
                DataTable dt= new DataTable();
                dt.Columns.Add("Depo Adı",Type.GetType("System.String"));
                dt.Columns.Add("Kayıt Tarihi",Type.GetType("System.String"));
                dt.Columns.Add("id",Type.GetType("System.String"));
                while(oku.Read())
                {
                    DataRow dr= dt.NewRow();
                    dr[0]=oku["depo"].ToString();
                    dr[1]=oku["eklemetar"].ToString();
                    dr[2]=oku["id"].ToString();
                    dt.Rows.Add(dr);
                }
                gridControl1.DataSource=dt;
                oku.Close();
                baglanti.Close();
                gridView1.Columns["id"].Visible=false; 
            }
            catch
            {
                baglanti.Close();
            }
        }
        public void güncelle()
        {
            if (textEdit1.Text == "")
            {
                XtraMessageBox.Show("yıldız(*) ile gösterilen alanlar boş geçilmez \n Lütfen bilgileri tam giriniz...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

            }
            else
            {
                //try
                //{
              
                    string sorgu = "UPDATE Stokdepo SET depo=@depo WHERE id like'" + güncellemeid + "'";
                    baglanti.Close();
                    OleDbCommand komut = new OleDbCommand(sorgu,baglanti);
                    komut.Parameters.AddWithValue("depo",textEdit1.Text);
                    baglanti.Open();
                    if (komut.ExecuteNonQuery() == 1)
                    {
                        XtraMessageBox.Show("Güncelleme işlemi gerçekleştirilmiştir", "Bilgi...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        textEdit1.Text = "";
                        simpleButton1.Text = "Kaydet";
                        listele();
                    }
                    else
                    {
                        XtraMessageBox.Show("güncelleme yapılamadı","Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    baglanti.Close();
                //}
                //catch
                //{
                //    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu.", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                //}

            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           textEdit1.Text = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle,"Depo Adı"));
           güncellemeid = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id"));
           simpleButton1.Text ="Güncelle";
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OleDbCommand veri = new OleDbCommand("DELETE FROM Stokdepo WHERE id like'" + Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id")) + "'", baglanti);
            baglanti.Open();
            if (veri.ExecuteNonQuery() == 1)
            {
                XtraMessageBox.Show("kayıt silindi", "Bilgi...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                listele();
                

            }
            else
            {
                XtraMessageBox.Show("kayıt silinmedi", "Hata...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            baglanti.Close();
        }

        private void gridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Right && gridView1.SelectedRowsCount==1)
            {
                popupMenu1.ShowPopup(MousePosition);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        { 
            if(simpleButton1.Text=="Kaydet")
            {
                kaydet();
                listele();

            }
            else if (simpleButton1.Text == "Güncelle")
            {
                güncelle();
                listele();

            }

        }
    
    }
}