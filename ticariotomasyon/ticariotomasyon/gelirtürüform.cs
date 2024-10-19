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
    public partial class gelirtürüform : DevExpress.XtraEditors.XtraForm
    {
        string baglanticumlecigi;
        public gelirtürüform(string sqlcumlecigi)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
        }
        public OleDbConnection baglanti = new OleDbConnection();
        public void listele()
        {
            //try
            //{
                baglanti.Close();
                string sorgu = "SELECT * FROM gelirgidertürü WHERE tip like '0'"+
                    "ORDER BY id DESC";
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("Gelir Türü", Type.GetType("System.String"));
                dt.Columns.Add("id", Type.GetType("System.String"));
                while (oku.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = oku["gelirgidertürü"].ToString();
                    dr[1] = oku["id"].ToString();
                    dt.Rows.Add(dr);
                }
                gridControl4.DataSource = dt;
                oku.Close();
                baglanti.Close();
                gridView4.Columns["id"].Visible = false;

            //}
            //catch
            //{
            //    baglanti.Close();
            //}
        }

        private void gelirtürüform_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            listele();
        }

        public void kaydet()
        {
            if (textEdit1.Text == "")
            {
                XtraMessageBox.Show("(*)ile belirtilen alanlar boş geçilemez lütfen doldurun...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                try
                {
                    baglanti.Open();
                    OleDbCommand komut = baglanti.CreateCommand();
                    komut.CommandText = "insert into gelirgidertürü (tip,gelirgidertürü)" + "values (@tip,@gelirgidertürü)";
                    komut.Parameters.Add("tip", OleDbType.VarChar).Value = "0";
                    komut.Parameters.Add("gelirgidertürü", OleDbType.VarChar).Value = textEdit1.Text;

                    if (komut.ExecuteNonQuery() == 1)
                    {
                        XtraMessageBox.Show("kayıt işlemi gerçekleştirilmiştir...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                        gelirform veri = (gelirform)Application.OpenForms["gelirform"];
                        veri.gidertürülistele();

                    }
                    else
                    {
                        XtraMessageBox.Show("kayıt yapılamadı...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                }
                catch
                {
                    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        
        }
        string gidertürüid;
        public void guncelle()
        {
            try
            {
                baglanti.Close();
                string sqlguncelle = "UPDATE gelirgidertürü SET gelirgidertürü=@gelirgidertürü WHERE id like '" + gidertürüid + "'";
                OleDbCommand komut = new OleDbCommand(sqlguncelle, baglanti);
                baglanti.Open();
                komut.Parameters.AddWithValue("gelirgidertürü", textEdit1.Text);
                komut.ExecuteNonQuery();
                if (komut.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("kayıt düzeltildi...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    gelirform veri = (gelirform)Application.OpenForms["gelirform"];
                    veri.gidertürülistele();
                    Close();
                }
                else
                {
                    XtraMessageBox.Show("kayıt düzeltilemedi...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                Close();
            }
            catch
            {
                baglanti.Close();
                XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (simpleButton1.Text == "Ekle")
            {
                kaydet();
            }
            else if (simpleButton1.Text == "Güncelle")
            {
                guncelle();
                textEdit1.Text = "";
                simpleButton1.Text = "Ekle";
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            textEdit1.Text = Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "Gelir Türü"));
            gidertürüid = Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "id"));
            simpleButton1.Text = "Güncelle";
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                baglanti.Open();
                OleDbCommand komut = baglanti.CreateCommand();
                komut.CommandText = "DELETE FROM gelirgidertürü WHERE id like '" + Convert.ToString(gridView4.GetRowCellValue(gridView4.FocusedRowHandle, "id")) + "'";
                if (komut.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("kayıt silindi...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    gelirform veri = (gelirform)Application.OpenForms["gelirform"];
                    veri.gidertürülistele();
                }
                else
                {
                    XtraMessageBox.Show("kayıt silinmedi...", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                listele();
            }
            catch
            {
                XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu", "Uyarı...", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void gridControl4_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && gridView4.SelectedRowsCount == 1)
            {
                popupMenu1.ShowPopup(MousePosition);
            }
        }

    }
}