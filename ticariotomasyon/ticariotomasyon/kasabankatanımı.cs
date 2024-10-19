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
    public partial class kasabankatanımı : DevExpress.XtraEditors.XtraForm
    {
        public AnaForm kasafrm;
        string baglanticumlecigi;
        public kasabankatanımı(string sqlcumlecigi)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
        }
        public OleDbConnection baglanti = new OleDbConnection();
        public void listele()
        {
            string sorgu = "SELECT * FROM kasabankatanımı ORDER BY id DESC";
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Kasa/Banka", Type.GetType("System.String"));
            dt.Columns.Add("Kasa/Banka Tanımı", Type.GetType("System.String"));
            dt.Columns.Add("Açılış Tarihi", Type.GetType("System.String"));
            dt.Columns.Add("Varsayılan", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["kasabanka"].ToString();
                dr[1] = oku["kasabankatanımı"].ToString();
                dr[2] = oku["acılıstar"].ToString();
                if (oku["varsayılan"].ToString() == "1")
                {
                    dr[3] = "varsayılan";
                }
                else
                {
                    dr[3] = "";
                }
                dr[4] = oku["id"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl4.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView4.Columns["id"].Visible = false;
        }
        public void varsayılanupdate()
        {
            if (checkEdit1.Checked == true)
            {
                string sqlguncelle = "UPDATE kasabankatanımı SET varsayılan=@varsayılan ";
                OleDbCommand komut = new OleDbCommand(sqlguncelle, baglanti);
                baglanti.Open();
                komut.Parameters.AddWithValue("varsayılan", "0");
                komut.ExecuteNonQuery();
                baglanti.Close();
            }
        }
        int dönenid;
        public void kaydet()
        {
            if (dateTimePicker1.Text == "" || comboBoxEdit1.Text == "" || textEdit1.Text == "")
            {
                XtraMessageBox.Show("yıldız(*) ile gösterilen alanlar boş geçilmez \n Lütfen bilgileri tam giriniz...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                //try
                //{
                    varsayılanupdate();
                    baglanti.Open();
                    OleDbCommand komut = baglanti.CreateCommand();
                    komut.CommandText = "insert into kasabankatanımı (kasabanka,kasabankatanımı,acılıstar,varsayılan)" + "values (@kasabanka,@kasabankatanımı,@acılıstar,@varsayılan)";
                    string komut2 = "Select @@Identity";
                    komut.Parameters.Add("kasabanka", OleDbType.VarChar).Value = comboBoxEdit1.Text;
                    komut.Parameters.Add("kasabankatanımı", OleDbType.VarChar).Value = textEdit1.Text;
                    komut.Parameters.Add("acılıstar", OleDbType.Date).Value = dateTimePicker1.Text;
                    if (checkEdit1.Checked == true)
                    {
                        komut.Parameters.Add("varsayılan", OleDbType.VarChar).Value = "1";

                    }
                    else
                    {
                        komut.Parameters.Add("varsayılan", OleDbType.VarChar).Value = "0";
                    }
                    if (komut.ExecuteNonQuery() == 1)
                    {
                        komut.CommandText = komut2;
                        dönenid = (int)komut.ExecuteScalar();
                        baglanti.Close();
                        if (Convert.ToDouble(textEdit2.Text) > 0)
                        {
                            baglanti.Open();
                            OleDbCommand komut3 = baglanti.CreateCommand();
                            komut3.CommandText = "insert into kasabankahareketleri (kasaid,durum,kasagiristar,kasatutar,acıklama,kasatip,tahakuktip)" + "values (@kasaid,@durum,@kasagiristar,@kasatutar,@acıklama,@kasatip,@tahakuktip)";
                            komut3.Parameters.Add("kasaid", OleDbType.VarChar).Value = dönenid;
                            komut3.Parameters.Add("durum", OleDbType.VarChar).Value = "Gelir";
                            komut3.Parameters.Add("kasagiristar", OleDbType.Date).Value = dateTimePicker1.Text;
                            komut3.Parameters.Add("kasatutar", OleDbType.VarChar).Value = textEdit2.Text;
                            komut3.Parameters.Add("acıklama", OleDbType.VarChar).Value = "Kasa Açılış Bakiyesi";
                            komut3.Parameters.Add("kasatip", OleDbType.VarChar).Value = "0";
                            komut3.Parameters.Add("tahakuktip", OleDbType.VarChar).Value = "3";
                            komut3.ExecuteNonQuery();
                            baglanti.Close();

                        }
                        XtraMessageBox.Show("kayıt işlemi gerçekleştirilmiştir...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        XtraMessageBox.Show("kayıt yapılamadı...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                //}
                //catch
                //{
                //    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                //}
            }
        }
        string kasaid;
        public void guncelle()
        {
            //try
            //{
                varsayılanupdate();
                string sqlguncelle="update kasabankatanımı SET kasabanka=@kasabanka,kasabankatanımı=@kasabankatanımı,acılıstar=@acılıstar,varsayılan=@varsayılan "+
                    " WHERE id like'"+kasaid+"'";
                OleDbCommand komut = new OleDbCommand(sqlguncelle, baglanti);
                baglanti.Open();
                komut.Parameters.AddWithValue("kasabanka", comboBoxEdit1.Text);
                komut.Parameters.AddWithValue("kasabankatanımı", textEdit1.Text);
                komut.Parameters.AddWithValue("acılıstar", dateTimePicker1.Text);
                if (checkEdit1.Checked == true)
                {
                    komut.Parameters.AddWithValue("varsayılan", 1);
                }
                else
                {
                    komut.Parameters.AddWithValue("varsayılan", 0);
                }
                if (komut.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("kayıt düzeltildi", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    XtraMessageBox.Show("kayıt yapılamadı", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                listele();



            //}
            //catch
            //{
            //    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            //}
        }
        private void kasabankatanımı_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            comboBoxEdit1.Properties.Items.Add("Kasa");
            comboBoxEdit1.Properties.Items.Add("Banka");
            listele();


        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            comboBoxEdit1.Text = Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "Kasa/Banka"));
            textEdit1.Text = Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "Kasa/Banka Tanımı"));
            dateTimePicker1.Text = Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "Açılış Tarihi"));
            if (Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "Varsayılan")) == "Varsayılan")
            {
                checkEdit1.Checked = true;
            }
            else
            {
                checkEdit1.Checked = false;
            }
            kasaid = Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "id"));
            simpleButton1.Text = "Güncelle";
            textEdit2.Enabled = false;
        }
        double ksahrksayi = 0;
        public void kasahareketsorgula()
        {
            try
            {
                string sorgu = "SELECT Sum(IIf([durum]='GELİR',[kasatutar],0)) AS giris1, Sum(IIf([durum]='GİDER',[kasatutar],0)) AS cikis1, Sum(IIf([durum]='GELİR',[kasatutar],0))  - Sum(IIf([durum]='GİDER',[kasatutar],0)) AS kalan FROM kasabankaharaketleri WHERE (((kasabankaharaketleri.kasaid) Like '" + kasaid + "')) ";
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    ksahrksayi = Convert.ToDouble(oku["toplam"].ToString());
                }
            }
            catch
            {
                baglanti.Close();
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if(simpleButton1.Text=="Kaydet")
                {
                    kaydet();
                    try
                    {
                        kasafrm.kasaderle(sender, e);
                        
                    }
                    catch
                    {

                    }
                    
                }
                else if (simpleButton1.Text == "Güncelle")
                {
                    guncelle();
                    simpleButton1.Text = "Kaydet";
                    textEdit2.Enabled = true;
                    comboBoxEdit1.Text = "";
                    textEdit1.Text = "";
                    dateTimePicker1.Text = "";
                    textEdit2.Text = "0";
                    checkEdit1.Checked = false;
                    try
                    {
                        kasafrm.kasaderle(sender, e);

                    }
                    catch
                    {

                    }
                }

            }
            catch
            {
                baglanti.Close();
                listele();
            }
        }

        

        private void gridControl4_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && gridView4.SelectedRowsCount == 1)
            {
                popupMenu1.ShowPopup(MousePosition);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            kasahareketsorgula();
            //try
            //{
            if (ksahrksayi.ToString() == "0")
            {
                baglanti.Open();
                OleDbCommand komut = new OleDbCommand("DELETE FROM hızlısatısurunler WHERE id LIKE '" + Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "id")) + "'", baglanti);
                if (komut.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("kayıt silindi", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    try
                    {
                        kasafrm.kasaderle(sender, e);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    XtraMessageBox.Show(Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "Kasa/Banka Tanımı")) + "kasasında hareket bulunmaktadır", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                listele();

            }
            else
            {
                XtraMessageBox.Show("hata oluştu", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            //}
            //catch
            //{
            //}
        }

    }
}