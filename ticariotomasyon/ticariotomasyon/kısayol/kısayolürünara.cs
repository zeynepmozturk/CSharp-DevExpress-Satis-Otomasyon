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
    public partial class kısayolürünara : DevExpress.XtraEditors.XtraForm
    {
        public kısayol ksylfrm;
        string baglanticumlecigi;
        public kısayolürünara(string sqlcumlecigi)
        {
            InitializeComponent();
            baglanticumlecigi = sqlcumlecigi;
        }
        public OleDbConnection baglanti = new OleDbConnection();
        private void kısayolürünara_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticumlecigi.ToString();
            listele();
        }
        public void listele()
        {
            string sorgu = "SELECT Stokkarti.id, Stokkarti.stokadi,Stokkarti.barkod,Stokcinsi.stokcinsi,Stokdepo.depo,Stokkarti.tedarikci,Stokkarti.tedarikcitel,Stokkarti.kayittar,Stokkarti.stoktipi,Stokkarti.aktiflik " +
                "FROM Stokdepo INNER JOIN (Stokcinsi INNER JOIN Stokkarti ON Stokcinsi.id=Stokkarti.stokcinsi)ON Stokdepo.id=Stokkarti.depo " +
                "WHERE ((Stokkarti.stoktipi Like 'Sarfiyat') AND ((Stokkarti.aktiflik) Like 1))";
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Stok Adı", Type.GetType("System.String"));
            dt.Columns.Add("Barkod No", Type.GetType("System.String"));
            dt.Columns.Add("Depo", Type.GetType("System.String"));
            dt.Columns.Add("Stok Cinsi", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi Tel", Type.GetType("System.String"));
            dt.Columns.Add("Kayıt Tarihi", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
            while(oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["stokadi"].ToString();
                dr[1] = oku["barkod"].ToString();
                dr[2] = oku["depo"].ToString();
                dr[3] = oku["stokcinsi"].ToString();
                dr[4] = oku["tedarikci"].ToString();
                dr[5] = oku["tedarikcitel"].ToString();
                dr[6] = oku["kayittar"].ToString();
                dr[7] = oku["id"].ToString();
                dt.Rows.Add(dr);
              
               
            }
            gridControl2.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView2.Columns["id"].Visible = false;

        }

        private void gridView2_DoubleClick(object sender, EventArgs e)
        {
            kısayol veri = (kısayol)Application.OpenForms["kısayol"];
            veri.labelControl1.Text = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "id"));
            veri.textEdit1.Text = Convert.ToString(gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "Stok Adı"));
            Close();
            
        }
    }
}