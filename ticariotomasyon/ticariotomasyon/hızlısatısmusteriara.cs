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
    public partial class hızlısatısmusteriara : DevExpress.XtraEditors.XtraForm
    {
        public AnaForm hızlstsfrm;
        string baglanticumlecigi;
        public hızlısatısmusteriara(string sqlcumlecigi)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;

        }
        public OleDbConnection baglanti = new OleDbConnection();
        private void hızlısatısmusteriara_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            listele();
        }
        public void listele()
        {
            DataTable dt = new DataTable();
            string sorgu = "SELECT* FROM musteriler WHERE aktiflik like '1' ORDER BY id DESC ";
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            dt.Columns.Add("Müşteri Adı", Type.GetType("System.String"));
            dt.Columns.Add("T.C. Kimlik No", Type.GetType("System.String"));
            dt.Columns.Add("Meslek", Type.GetType("System.String"));
            dt.Columns.Add("Telefon", Type.GetType("System.String"));
            dt.Columns.Add("Kan grubu", Type.GetType("System.String"));
            dt.Columns.Add("Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("Adres", Type.GetType("System.String"));
            dt.Columns.Add("İş Adres", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
          

            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["adsoyad"].ToString();
                dr[1] = oku["tckimlik"].ToString();
                dr[2] = oku["meslek"].ToString();
                dr[3] = oku["cep"].ToString();
                dr[4] = oku["kangrubu"].ToString();
                dr[5] = oku["aciklama"].ToString();
                dr[6] = oku["adres"].ToString();                
                dr[7] = oku["isadres"].ToString();
                dr[8] = oku["id"].ToString();
              
                dt.Rows.Add(dr);
            }
            gridControl2.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView2.Columns["id"].Visible = false;

        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            AnaForm veri = (AnaForm)Application.OpenForms["AnaForm"];
            veri.musteriid.Text = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "id"));
            veri.textEdit2.Text = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Müşteri Adı"));
            veri.veresiyeislem();
            Close();
        }
    }
}