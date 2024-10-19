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
using System.Media;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid;

namespace ticariotomasyon
{
    public partial class AnaForm : DevExpress.XtraEditors.XtraForm
    {
        string baglanticümlecigi, kulid;
        public AnaForm(string sqlSorgu,string kulidm)
        {
            InitializeComponent();
            baglanticümlecigi = sqlSorgu;
            kulid = kulidm;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle|=0x20000;
                cp.ExStyle |=0x02000000;
                 return base.CreateParams;
            }
        }
        public OleDbConnection baglanti = new OleDbConnection();
        public void aktifmusterilistele()
        {
            DataTable dt = new DataTable();
            string sorgu = "SELECT* FROM musteriler WHERE aktiflik like '1' ORDER BY id DESC ";
            baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            dt.Columns.Add("Adı Soyadı", Type.GetType("System.String"));
            dt.Columns.Add("T.C. Kimlik", Type.GetType("System.String"));
            dt.Columns.Add("Telefon", Type.GetType("System.String"));
            dt.Columns.Add("Kan Grubu", Type.GetType("System.String"));
            dt.Columns.Add("Adres", Type.GetType("System.String"));
            dt.Columns.Add("Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("E_mail", Type.GetType("System.String"));
            dt.Columns.Add("Meslek", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
            dt.Columns.Add("aktiflik", Type.GetType("System.String"));

            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["adsoyad"].ToString();
                dr[1] = oku["tckimlik"].ToString();
                dr[2] = oku["cep"].ToString();
                dr[3] = oku["kangrubu"].ToString();
                dr[4] = oku["adres"].ToString();
                dr[5] = oku["aciklama"].ToString();
                dr[6] = oku["eposta"].ToString();
                dr[7] = oku["meslek"].ToString();
                dr[8] = oku["id"].ToString();
                dr[9] = oku["aktiflik"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl3.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView3.Columns["id"].Visible = false;
            gridView3.Columns["aktiflik"].Visible = false;


        }
        public void pasifmusterilistele()
        {
            DataTable dt = new DataTable();
            string sorgu = "SELECT* FROM musteriler WHERE aktiflik like '0' ORDER BY id DESC ";
            baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            dt.Columns.Add("Adı Soyadı", Type.GetType("System.String"));
            dt.Columns.Add("T.C. Kimlik", Type.GetType("System.String"));
            dt.Columns.Add("Telefon", Type.GetType("System.String"));
            dt.Columns.Add("Kan Grubu", Type.GetType("System.String"));
            dt.Columns.Add("Adres", Type.GetType("System.String"));
            dt.Columns.Add("Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("E_mail", Type.GetType("System.String"));
            dt.Columns.Add("Meslek", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
            dt.Columns.Add("aktiflik", Type.GetType("System.String"));

            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["adsoyad"].ToString();
                dr[1] = oku["tckimlik"].ToString();
                dr[2] = oku["cep"].ToString();
                dr[3] = oku["kangrubu"].ToString();
                dr[4] = oku["adres"].ToString();
                dr[5] = oku["aciklama"].ToString();
                dr[6] = oku["eposta"].ToString();
                dr[7] = oku["meslek"].ToString();
                dr[8] = oku["id"].ToString();
                dr[9] = oku["aktiflik"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl3.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView3.Columns["id"].Visible = false;
            gridView3.Columns["aktiflik"].Visible = false;


        }
        private void AnaForm_Load(object sender, EventArgs e)
        {
            baglanti.ConnectionString = baglanticümlecigi.ToString();
            satistablosu();
            odemetipigetir();
            aktifkasa();
            grupadı();
            urunlistele1();
            urunlistele2();
            urunlistele3();
            urunlistele4();
            simpleButton2.Enabled = false;
            kasalistesitariharası();

        }
        public void totaltariharası()
        {
            try
            {
                string vFDate, vTDate;
                vFDate = baslangıctar.Value.ToString("MM-dd-yyyy");
                vTDate = bitistar.Value.ToString("MM-dd-yyyy");
                string sorgu = "SELECT Sum(IIf([durum]='GELİR',[kasatutar],0)) AS giris1, Sum(IIf([durum]='GİDER',[kasatutar],0)) AS cikis1, Sum(IIf([durum]='GELİR',[kasatutar],0))  - Sum(IIf([durum]='GİDER',[kasatutar],0)) AS kalan FROM kasabankahareketleri WHERE ( ((kasabankahareketleri.kasaid) Like '" + kasaid + "')) AND kasagiristar BETWEEN #" + vFDate + "# AND #" + vTDate + "#";
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    double girispara = Convert.ToDouble(oku["giris1"].ToString());
                    double cikispara = Convert.ToDouble(oku["cikis1"].ToString());
                    double bakiye = Convert.ToDouble(oku["kalan"].ToString());
                    girislabel.Text = girispara.ToString("#,##0.00");
                    giderlabel.Text = cikispara.ToString("#,##0.00");
                    bakiyelabel.Text = bakiye.ToString("#,##0.00");
                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                //XtraMessageBox.Show("Veri Tabanında Bir Hata Oluştu \n lütfen Programı Kapatıp Tekrar Deneyiniz.....!", "HATA....", MessageBoxButtons.OK, MessageBoxIcon.Error);             
                baglanti.Close();
                girislabel.Text = "0,00";
                giderlabel.Text = "0,00";
                bakiyelabel.Text = "0,00";
            }

        }

        public void kasalistesitariharası()
        {
            string vfDate, vTDate;
            vfDate = baslangıctar.Value.ToString("MM-dd-yyyy");
            vTDate = bitistar.Value.ToString("MM-dd-yyyy");
            string sorgu = "SELECT kasabankahareketleri.id,kasabankahareketleri.kasaid,kasabankahareketleri.kasagiristar,kasabankahareketleri.islemkod, " +
                "kasabankahareketleri.durum,kasabankahareketleri.kasatutar,gelirgidertürü.gelirgidertürü,kasabankahareketleri.acıklama, " +
                "kasabankahareketleri.kasatip,kasabankahareketleri.virmanid, " +
                "kasabankahareketleri.tahakuktip " +
                "FROM gelirgidertürü INNER JOIN kasabankahareketleri ON gelirgidertürü.id=kasabankahareketleri.tahakuktip " +
                "WHERE kasaid LIKE '" + kasaid + "' AND kasagiristar BETWEEN #" + vfDate + "# AND #" + vTDate + "#" +
                "ORDER BY kasabankahareketleri.id DESC";
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Tarih", Type.GetType("System.String"));
            dt.Columns.Add("Durum", Type.GetType("System.String"));
            dt.Columns.Add("Türü", Type.GetType("System.String"));
            dt.Columns.Add("Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("Tutar", Type.GetType("System.String"));
            dt.Columns.Add("id", Type.GetType("System.String"));
            dt.Columns.Add("kasaid", Type.GetType("System.String"));
            dt.Columns.Add("kasatip" ,Type.GetType("System.String"));
            dt.Columns.Add("virmanid", Type.GetType("System.String"));
            while(oku.Read())
            {
                double gidertutardeğişkeni = Convert.ToDouble(oku["kasatutar"].ToString());
                DataRow dr = dt.NewRow();
                dr[0] = oku["kasagiristar"].ToString();
                dr[1] = oku["durum"].ToString();
                dr[2] = oku["gelirgidertürü"].ToString();
                dr[3] = oku["acıklama"].ToString();
                dr[4] = gidertutardeğişkeni.ToString("#,##0.00");
                dr[5] = oku["id"].ToString();
                dr[6] = oku["kasaid"].ToString();
                dr[7] = oku["kasatip"].ToString();
                dr[8]=oku["virmanid"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl4.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView4.Columns["id"].Visible = false;
            gridView4.Columns["kasaid"].Visible = false;
            gridView4.Columns["kasatip"].Visible = false;
            gridView4.Columns["virmanid"].Visible = false;
            gridView4.IndicatorWidth = 40;
            totaltariharası();

                
        }
        public void odemetipigetir()
        {
            try
            {
                OleDbCommand veri = new OleDbCommand("SELECT id,kasabankatanımı  FROM kasabankatanımı ORDER BY id DESC", baglanti);
                OleDbDataReader oku = null;
                baglanti.Open();
                oku = veri.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("id", typeof(string));
                dt.Columns.Add("kasabankatanımı", typeof(string));
                dt.Load(oku);
                lookUpEdit1.Properties.ValueMember = "id";
                lookUpEdit1.Properties.DisplayMember = "kasabankatanımı";
                lookUpEdit1.Properties.DataSource = dt;
                oku.Close();
                baglanti.Close();
            }
            catch
            {

            }

        }
        int kasaid;
        public void aktifkasa()
        {
            try
            {
                string sorgu = "SELECT *FROM kasabankatanımı WHERE  varsayılan LIKE '1'";
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    kasaid = Convert.ToInt32(oku["id"].ToString());
                    lookUpEdit1.EditValue = oku["id"].ToString();
                    kasanınadı.Text = oku["kasabankatanımı"].ToString();
                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {

            }



        }
        public void satistablosu()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Ürün Adı", Type.GetType("System.String"));
            dt.Columns.Add("Miktar", Type.GetType("System.String"));
            dt.Columns.Add("Fiyat", Type.GetType("System.String"));
            dt.Columns.Add("Tutar", Type.GetType("System.Object"));
            dt.Columns.Add("id", Type.GetType("System.Object"));
            gridControl2.DataSource = dt;
            gridView2.Columns["id"].Visible = false;

        }
        public void aktifurunlistesi()
        {
            string sorgu = "SELECT* FROM Stokkarti WHERE aktiflik like '1' ORDER BY id desc";
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = komut.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Stok Adı", Type.GetType("System.String"));
            dt.Columns.Add("Barkod", Type.GetType("System.String"));
            dt.Columns.Add("Stok Cinsi", Type.GetType("System.String"));
            dt.Columns.Add("Ürünün Alış Fiyatı", Type.GetType("System.String"));
            dt.Columns.Add("Ürünün Satış Fiyatı", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi Telefon", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi Adres", Type.GetType("System.String"));
            dt.Columns.Add("Ürün Kayıt Tarihi", Type.GetType("System.String"));
            dt.Columns.Add("Ürün Stok Tipi", Type.GetType("System.String"));
            dt.Columns.Add("Depo", Type.GetType("System.String"));
            dt.Columns.Add("İD", Type.GetType("System.String"));
            dt.Columns.Add("aktiflik", Type.GetType("System.String"));

            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["stokadi"].ToString();
                dr[1] = oku["barkod"].ToString();
                dr[2] = oku["stokcinsi"].ToString();
                dr[3] = oku["alisfiyat"].ToString();
                dr[4] = oku["satisfiyat"].ToString();
                dr[5] = oku["tedarikci"].ToString();
                dr[6] = oku["tedarikcitel"].ToString();
                dr[7] = oku["tedarikciaciklama"].ToString();
                dr[8] = oku["tedarikciadres"].ToString();
                dr[9] = oku["kayittar"].ToString();
                dr[10] = oku["stoktipi"].ToString();
                dr[11] = oku["depo"].ToString();
                dr[12] = oku["id"].ToString();
                dr[13] = oku["aktiflik"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl1.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView1.Columns["İD"].Visible = false;
            gridView1.Columns["aktiflik"].Visible = false;



        }
        public void pasifurunlistesi()
        {
            string sorgu = "SELECT* FROM Stokkarti WHERE aktiflik like '0' ORDER BY id desc";
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = komut.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Stok Adı", Type.GetType("System.String"));
            dt.Columns.Add("Barkod", Type.GetType("System.String"));
            dt.Columns.Add("Stok Cinsi", Type.GetType("System.String"));
            dt.Columns.Add("Ürünün Alış Fiyatı", Type.GetType("System.String"));
            dt.Columns.Add("Ürünün Satış Fiyatı", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi Telefon", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("Tedarikçi Adres", Type.GetType("System.String"));
            dt.Columns.Add("Ürün Kayıt Tarihi", Type.GetType("System.String"));
            dt.Columns.Add("Ürün Stok Tipi", Type.GetType("System.String"));
            dt.Columns.Add("Depo", Type.GetType("System.String"));
            dt.Columns.Add("İD", Type.GetType("System.String"));
            dt.Columns.Add("aktiflik", Type.GetType("System.String"));

            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["stokadi"].ToString();
                dr[1] = oku["barkod"].ToString();
                dr[2] = oku["stokcinsi"].ToString();
                dr[3] = oku["alisfiyat"].ToString();
                dr[4] = oku["satisfiyat"].ToString();
                dr[5] = oku["tedarikci"].ToString();
                dr[6] = oku["tedarikcitel"].ToString();
                dr[7] = oku["tedarikciaciklama"].ToString();
                dr[8] = oku["tedarikciadres"].ToString();
                dr[9] = oku["kayittar"].ToString();
                dr[10] = oku["stoktipi"].ToString();
                dr[11] = oku["depo"].ToString();
                dr[12] = oku["id"].ToString();
                dr[13] = oku["aktiflik"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl1.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView1.Columns["İD"].Visible = false;
            gridView1.Columns["aktiflik"].Visible = false;



        }
        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            kullanıcıprofili ac = new kullanıcıprofili(baglanticümlecigi,kulid);
            ac.ShowDialog();
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            profilguncelleme aç = new profilguncelleme(baglanticümlecigi, kulid);
            aç.ShowDialog();
        }

     
        private void navButton3_ElementClick_1(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            urunkayıt ac = new urunkayıt(baglanticümlecigi, kulid, null, "1");
            ac.ShowDialog();
        }

        private void navButton2_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            aktifurunlistesi();
        }

        private void navButton4_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            pasifurunlistesi();
        }

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            DialogResult sonuc = new DialogResult();
            sonuc = XtraMessageBox.Show("Program kapatılacaktır çıkmak istediğinizden emin misiniz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            if (sonuc == DialogResult.No)
            {

            }
            if (sonuc == DialogResult.Yes)
            {
                this.Close();
                Application.Exit();
            }
        }

      
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string datapath = Environment.CurrentDirectory + @"/Data.mdb";
                string dosyaadı = DateTime.Now.ToString("dd.MM.yyyy");
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string backuppath = fbd.SelectedPath.ToString();
                    File.Copy(datapath, backuppath + @"/" + dosyaadı + " " + "backup.mdb", true);
                    XtraMessageBox.Show("Veri tabanı backup'u alınmıştır.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }

            }
            catch
            {

            }
        }
        public void veresiyeislem()
        {
            simpleButton10.Visible = true;
            simpleButton2.Visible = true;
            simpleButton2.Enabled = true;
            simpleButton1.Enabled = false;
            simpleButton3.Enabled = false;
            textEdit3.Enabled = false;
        }
        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            urunkayıt ac = new urunkayıt(baglanticümlecigi, kulid, Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "İD")), "2");
            ac.ShowDialog();
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                baglanti.Open();
                OleDbCommand sorgu = new OleDbCommand("UPDATE Stokkarti SET aktiflik=@aktiflik WHERE id like'" + Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "İD")) + "'", baglanti);
                sorgu.Parameters.AddWithValue("aktiflik", "1");
                if (sorgu.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("Kayıt aktif edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                }
                else
                {
                    XtraMessageBox.Show("Kayıt aktif edilemedi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                pasifurunlistesi();
            }
            catch
            {

            }
        }

        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                baglanti.Open();
                OleDbCommand sorgu = new OleDbCommand("UPDATE Stokkarti SET aktiflik=@aktiflik WHERE id like'" + Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "İD")) + "'", baglanti);
                sorgu.Parameters.AddWithValue("aktiflik", "0");
                if (sorgu.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("Kayıt pasif edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                }
                else
                {
                    XtraMessageBox.Show("Kayıt pasif edilemedi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                aktifurunlistesi();
            }
            catch
            {

            }
        }

        private void gridControl1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && gridView1.SelectedRowsCount == 1)
            {
                if (Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "aktiflik")) == "1")
                {
                    barButtonItem13.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barButtonItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                }
                else if (Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "aktiflik")) == "0")
                {
                    barButtonItem13.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                popupMenu2.ShowPopup(MousePosition);
            }
        }
        string id = "";
        string stokadi;
        double satisfiyatim;
       
        public void barkod()
        {
           
                DateTime dtm = DateTime.Now;
                string t = dtm.ToString("dd.MM.yyyy");
                if (textEdit4.Text == "" || textEdit5.Text == "")
                {
                    XtraMessageBox.Show("Barkot ve Miktar Alanları Boş Geçilemez", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    baglanti.Close();
                }
                else
                {
                    string sorgu = "SELECT* FROM Stokkarti WHERE barkod like '" + textEdit4.Text + "'";
                    baglanti.Open();
                    OleDbCommand komut = new OleDbCommand(sorgu, baglanti);
                    OleDbDataReader oku = null;
                    oku = komut.ExecuteReader();
                    while (oku.Read())
                    {
                        id = oku["id"].ToString();
                        double satis = Convert.ToDouble(oku["satisfiyat"].ToString());
                        satisfiyatim = Convert.ToDouble(satis.ToString("#,##0.00"));
                        stokadi = oku["stokadi"].ToString();
                    }
                    oku.Close();
                    baglanti.Close();
                    if (satisfiyatim.ToString() == "" || id.ToString() == "" || satisfiyatim == 0)
                    {
                        XtraMessageBox.Show("Barkod numaralı ürün bulunamadı...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        baglanti.Close();
                    }
                    else
                    {
                        double bakiye = satisfiyatim * Convert.ToDouble(textEdit5.Text);
                        DataTable dt = gridControl2.DataSource as DataTable;
                        DataRow newRow = dt.NewRow();
                        newRow[0] = stokadi;
                        newRow[1] = textEdit5.Text;
                        newRow[2] = satisfiyatim.ToString("#,##0.00");
                        newRow[3] = bakiye.ToString("#,##0.00");
                        newRow[4] = id.ToString();
                        dt.Rows.InsertAt(newRow, 0);
                        gridView2.FocusedRowHandle = 0;
                        id = "";
                        stokadi = "";
                        satisfiyatim = 0;
                        double toplam = 0;
                        for (int i = 0; i < gridView2.RowCount; i++)
                        {
                            toplam += Convert.ToDouble(gridView2.GetRowCellValue(i, "Tutar").ToString());
                        }
                        textEdit1.Text = toplam.ToString("#,##0.00");

                    }
                    baglanti.Close();
                }
           
          
        }
        public void stokhareketi()
        {
            DateTime dtm = DateTime.Now;
            string t = dtm.ToString("dd.MM.yyyy");
            baglanti.Open();
            for (int i = 0; i < gridView2.RowCount;i++ )
            {
                OleDbCommand komut = baglanti.CreateCommand();
                komut.CommandText = "insert into Stokhareketi (Stokid,hareketturu,miktar,tarih,direksatisdönenid)" + "values(@Stokid,@hareketturu,@miktar,@tarih,@direksatisdönenid)";
                komut.Parameters.Add("Stokid", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i ,"id").ToString());
                komut.Parameters.Add("hareketturu", OleDbType.VarChar).Value = "Çıkış";
                komut.Parameters.Add("miktar", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i, "Miktar").ToString());
                komut.Parameters.Add("tarih", OleDbType.VarChar).Value = t.ToString();
                komut.Parameters.Add("direksatisdönenid", OleDbType.VarChar).Value = donenid;
                komut.ExecuteNonQuery();
                
            }
            baglanti.Close();
        }
        public void hızlısatıskasahareketi()
        {
            baglanti.Open();
            for (int i = 0; i < gridView2.RowCount; i++)
            {
                OleDbCommand komut = baglanti.CreateCommand();
                komut.CommandText = "insert into hızlısatıskasahareketi (Kasaid,ürünadı,miktar,fiyat)" + "values(@Kasaid,@ürünadı,@miktar,@fiyat)";
                komut.Parameters.Add("Kasaid", OleDbType.VarChar).Value = donenid;
                komut.Parameters.Add("ürünadı", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i, "Ürün Adı").ToString());
                komut.Parameters.Add("miktar", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i, "Miktar").ToString());
                komut.Parameters.Add("fiyat", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i, "Fiyat").ToString());
                komut.ExecuteNonQuery();

            }
            baglanti.Close();
        }
        int donenid;
        public void Direksatış()
        {
            DateTime dtm = DateTime.Now;
            string t = dtm.ToString("dd.MM.yyyy");
            if (textEdit1.Text == "0" || textEdit1.Text == "0.00" || textEdit1.Text == "0,00")
            {
                XtraMessageBox.Show("Girilen tahsilat sıfır olamaz...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                try
                {
                    OleDbCommand komut = baglanti.CreateCommand();
                    komut.CommandText = "insert into kasabankahareketleri(kasaid,kasagiristar,durum,kasatutar,tahakuktip,acıklama,kasatip)" + "values(@kasaid,@kasagiristar,@durum,@kasatutar,@tahakuktip,@acıklama,@kasatip)";
                    string komut2 = "Select @@Identity";
                    komut.Parameters.Add("kasaid", OleDbType.VarChar).Value = lookUpEdit1.EditValue;
                    komut.Parameters.Add("kasagiristar", OleDbType.VarChar).Value = t.ToString();
                    komut.Parameters.Add("durum", OleDbType.VarChar).Value ="Gelir";
                    komut.Parameters.Add("kasatutar", OleDbType.VarChar).Value = textEdit1.Text;
                    komut.Parameters.Add("tahakuktip", OleDbType.VarChar).Value = "4";
                    komut.Parameters.Add("acıklama", OleDbType.VarChar).Value = "Direkt Satış(Nakit)";
                    komut.Parameters.Add("kasatip", OleDbType.VarChar).Value = "0";
                    baglanti.Open();
                    if(komut.ExecuteNonQuery()==1)
                    {
                        komut.CommandText = komut2;
                        donenid = (int)komut.ExecuteScalar();
                        baglanti.Close();
                        stokhareketi();
                        baglanti.Close();
                        hızlısatıskasahareketi();                        
                        baglanti.Close();
                        textEdit5.Text = "1";
                        textEdit3.Text = "0,00";
                        textEdit1.Text = "0,00";
                        textEdit4.Text = "";
                        gridControl2.DataSource = null;
                        gridView2.Columns.Clear();
                        satistablosu();
                    }
                    try
                    {
                        SoundPlayer ac = new SoundPlayer(Application.StartupPath+"\\kasa.wav");
                        ac.Play();
                    }
                    catch
                    {
                        baglanti.Close();
                    }
                    
                }
                catch
                {
                    XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }
        public void urunlistele1()
        {
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.VerticalScroll.Enabled = true;
            flowLayoutPanel1.VerticalScroll.Visible = true;
            flowLayoutPanel1.AutoScroll = true;
            baglanti.Close();
            baglanti.Open();
            OleDbCommand veri = new OleDbCommand("SELECT * FROM hızlısatısurunler where grupid like '0'",baglanti);
            OleDbDataReader oku = null;
            oku=veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id", typeof(int));
            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["urunid"].ToString();
                dt.Rows.Add(dr);
                DataView dw = dt.DefaultView;
                dw.Sort = "id asc";
                dt = dw.ToTable();
            }
            foreach(DataRow row in dt.Rows)
            {
                satisurun item = new satisurun(baglanticümlecigi);
                item.hızlstsfrm = this;
                item.urunidimiz = row["id"].ToString();
                flowLayoutPanel1.Controls.Add(item);
            }
            oku.Close();
            baglanti.Close();
        }
        public void urunlistele2()
        {
            flowLayoutPanel2.Controls.Clear();
            flowLayoutPanel2.VerticalScroll.Enabled = true;
            flowLayoutPanel2.VerticalScroll.Visible = true;
            flowLayoutPanel2.AutoScroll = true;
            baglanti.Close();
            baglanti.Open();
            OleDbCommand veri = new OleDbCommand("SELECT * FROM hızlısatısurunler where grupid like '1'", baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id", typeof(int));
            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["urunid"].ToString();
                dt.Rows.Add(dr);
                DataView dw = dt.DefaultView;
                dw.Sort = "id asc";
                dt = dw.ToTable();
            }
            foreach (DataRow row in dt.Rows)
            {
                satisurun item = new satisurun(baglanticümlecigi);
                item.hızlstsfrm = this;
                item.urunidimiz = row["id"].ToString();
                flowLayoutPanel2.Controls.Add(item);
            }
            oku.Close();
            baglanti.Close();
        }
        public void urunlistele3()
        {
            flowLayoutPanel3.Controls.Clear();
            flowLayoutPanel3.VerticalScroll.Enabled = true;
            flowLayoutPanel3.VerticalScroll.Visible = true;
            flowLayoutPanel3.AutoScroll = true;
            baglanti.Close();
            baglanti.Open();
            OleDbCommand veri = new OleDbCommand("SELECT * FROM hızlısatısurunler where grupid like '2'", baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id", typeof(int));
            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["urunid"].ToString();
                dt.Rows.Add(dr);
                DataView dw = dt.DefaultView;
                dw.Sort = "id asc";
                dt = dw.ToTable();
            }
            foreach (DataRow row in dt.Rows)
            {
                satisurun item = new satisurun(baglanticümlecigi);
                item.hızlstsfrm = this;
                item.urunidimiz = row["id"].ToString();
                flowLayoutPanel3.Controls.Add(item);
            }
            oku.Close();
            baglanti.Close();
        }
        public void urunlistele4()
        {
            flowLayoutPanel4.Controls.Clear();
            flowLayoutPanel4.VerticalScroll.Enabled = true;
            flowLayoutPanel4.VerticalScroll.Visible = true;
            flowLayoutPanel4.AutoScroll = true;
            baglanti.Close();
            baglanti.Open();
            OleDbCommand veri = new OleDbCommand("SELECT * FROM hızlısatısurunler where grupid like '3'", baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id", typeof(int));
            while (oku.Read())
            {
                DataRow dr = dt.NewRow();
                dr[0] = oku["urunid"].ToString();
                dt.Rows.Add(dr);
                DataView dw = dt.DefaultView;
                dw.Sort = "id asc";
                dt = dw.ToTable();
            }
            foreach (DataRow row in dt.Rows)
            {
                satisurun item = new satisurun(baglanticümlecigi);
                item.hızlstsfrm = this;
                item.urunidimiz = row["id"].ToString();
                flowLayoutPanel4.Controls.Add(item);
            }
            oku.Close();
            baglanti.Close();
        }
        public void showsatis(string idimiz)
        {
            //try
            //{
                    baglanti.Close();
                    string sorgu = "SELECT Stokkarti.id,Stokkarti.stokadi,Stokkarti.barkod,Stokcinsi.stokcinsi,Stokkarti.alisfiyat,Stokkarti.satisfiyat, " +
                        "Stokkarti.tedarikciadres,Stokkarti.stoktipi,Stokdepo.depo,Stokkarti.aktiflik " +
                        "FROM Stokdepo INNER JOIN (Stokcinsi INNER JOIN Stokkarti ON Stokcinsi.id=Stokkarti.stokcinsi) ON Stokdepo.id=Stokkarti.depo WHERE aktiflik like '1' AND stoktipi like 'Sarfiyat' AND Stokkarti.id like '"+idimiz+"' " +
                        "ORDER BY Stokkarti.id DESC";
                    baglanti.Open();
                    OleDbCommand komut = new OleDbCommand(sorgu, baglanti);
                    OleDbDataReader oku = null;
                    oku = komut.ExecuteReader();
                    while (oku.Read())
                    {
                        id = oku["id"].ToString();
                        double satis = Convert.ToDouble(oku["satisfiyat"].ToString());
                        satisfiyatim = Convert.ToDouble(satis.ToString("#,##0.00"));
                        stokadi = oku["stokadi"].ToString();
                    }
                    oku.Close();
                    baglanti.Close();

                        double bakiye = satisfiyatim * Convert.ToDouble(textEdit5.Text);
                        DataTable dt = gridControl2.DataSource as DataTable;
                        DataRow newRow = dt.NewRow();
                        newRow[0] = stokadi;
                        newRow[1] = textEdit5.Text;
                        newRow[2] = satisfiyatim.ToString("#,##0.00");
                        newRow[3] = bakiye.ToString("#,##0.00");
                        newRow[4] = id.ToString();
                        dt.Rows.InsertAt(newRow, 0);
                        gridView2.FocusedRowHandle = 0;
                        id = "";
                        stokadi = "";
                        satisfiyatim = 0;
                        double toplam = 0;
                        for (int i = 0; i < gridView2.RowCount; i++)
                        {
                            toplam += Convert.ToDouble(gridView2.GetRowCellValue(i, "Tutar").ToString());
                        }
                        textEdit1.Text = toplam.ToString("#,##0.00");
                        baglanti.Close();

                //}
                // catch
                // {
                //     XtraMessageBox.Show("bilinmeyen bir nedenden dolayı hata oluştu...", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                // }
            }
            
        
        private void textEdit4_EditValueChanged(object sender, EventArgs e)
        {
           
           
        }

        private void textEdit4_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
               barkod();
            }
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Direksatış();
        }

        private void gridControl2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && gridView2.SelectedRowsCount == 1)
            {
                satismenu.ShowPopup(MousePosition);
            }
        }

        private void barButtonItem14_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
                if (gridView2.SelectedRowsCount>0)
                {
                    gridView2.DeleteSelectedRows();
                    double toplam = 0;
                    for (int i = 0; i < gridView2.RowCount; i++)
                    {
                        toplam += Convert.ToDouble(gridView2.GetRowCellValue(i, "Tutar").ToString());
                    }
                    textEdit1.Text = toplam.ToString("#,##0.00");

                }
                else
                {
                    MessageBox.Show("Lütfen silinecek satırı seçin!");
                }
            
        }

        private void barButtonItem15_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            stokgiriscıkıs ac = new stokgiriscıkıs(baglanticümlecigi,kulid,Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "İD")));
            ac.ShowDialog();
        }

        private void barButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            stokhareketi ac = new stokhareketi(baglanticümlecigi, kulid, Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "İD")));
            ac.ShowDialog();
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {

        }
        public void paraüstüderle()
        {
            try
            {
                double toplam = Convert.ToDouble(alınanpara.Text);
                double paraüstünüz = toplam - Convert.ToDouble(textEdit1.Text);
                alınanpara.Text = toplam.ToString("#,##0.00");
                labelControl3.Text = paraüstünüz.ToString("#,##0.00");

            }
            catch
            {
            }
        }
        private void simpleButton8_Click(object sender, EventArgs e)
        {
            hızlısatısalınanpara ac = new hızlısatısalınanpara();
            ac.hızlıstsfrm = this;
            ac.ShowDialog();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            alınanpara.Text = "0,00";
            labelControl3.Text = "0,00";
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(alınanpara.Text) + Convert.ToDouble("5,00");
                double paraüstümüz = toplam - Convert.ToDouble(textEdit1.Text);
                alınanpara.Text = toplam.ToString("#,##0.00");
                labelControl3.Text = paraüstümüz.ToString("#,##0.00");

            }
            catch
            {

            }
        }

        private void pictureEdit2_Click(object sender, EventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(alınanpara.Text) + Convert.ToDouble("10,00");
                double paraüstümüz = toplam - Convert.ToDouble(textEdit1.Text);
                alınanpara.Text = toplam.ToString("#,##0.00");
                labelControl3.Text = paraüstümüz.ToString("#,##0.00");

            }
            catch
            {

            }
        }

        private void pictureEdit3_Click(object sender, EventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(alınanpara.Text) + Convert.ToDouble("20,00");
                double paraüstümüz = toplam - Convert.ToDouble(textEdit1.Text);
                alınanpara.Text = toplam.ToString("#,##0.00");
                labelControl3.Text = paraüstümüz.ToString("#,##0.00");

            }
            catch
            {

            }
        }

        private void pictureEdit6_Click(object sender, EventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(alınanpara.Text) + Convert.ToDouble("50,00");
                double paraüstümüz = toplam - Convert.ToDouble(textEdit1.Text);
                alınanpara.Text = toplam.ToString("#,##0.00");
                labelControl3.Text = paraüstümüz.ToString("#,##0.00");

            }
            catch
            {

            }
        }

        private void pictureEdit5_Click(object sender, EventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(alınanpara.Text) + Convert.ToDouble("100,00");
                double paraüstümüz = toplam - Convert.ToDouble(textEdit1.Text);
                alınanpara.Text = toplam.ToString("#,##0.00");
                labelControl3.Text = paraüstümüz.ToString("#,##0.00");

            }
            catch
            {

            }
        }

        private void pictureEdit4_Click(object sender, EventArgs e)
        {
            try
            {
                double toplam = Convert.ToDouble(alınanpara.Text) + Convert.ToDouble("200,00");
                double paraüstümüz = toplam - Convert.ToDouble(textEdit1.Text);
                alınanpara.Text = toplam.ToString("#,##0.00");
                labelControl3.Text = paraüstümüz.ToString("#,##0.00");

            }
            catch
            {

            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            kısayol ac = new kısayol(baglanticümlecigi);
            ac.hızlıfrm = this;
            ac.ShowDialog();
        }
        public void grupadı()
        {
            try
            {
                OleDbCommand veri = new OleDbCommand("SELECT * FROM hızlısatısgrupadi", baglanti);
                OleDbDataReader oku = null;
                baglanti.Open();
                oku = veri.ExecuteReader();
                while(oku.Read())
                {
                    xtraTabPage1.Text = oku["grupadi1"].ToString();
                    xtraTabPage2.Text = oku["grupadi2"].ToString();
                    xtraTabPage3.Text = oku["grupadi3"].ToString();
                    xtraTabPage4.Text = oku["grupadi4"].ToString();

                }
            }
            catch
            {
            }
        }
        public void kısayolderle(object sender, EventArgs e)
        {
            grupadı();
        }

        private void navButton1_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            musteeriekle ac = new musteeriekle(baglanticümlecigi, kulid, "", "0");
            ac.ShowDialog();
        }

        private void navButton6_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            aktifmusterilistele();
        }

        private void navButton7_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            pasifmusterilistele();
        }

        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            musteeriekle ac = new musteeriekle(baglanticümlecigi, kulid, Convert.ToString(gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "id")), "1");
            ac.ShowDialog();
        }

        private void gridControl3_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && gridView3.SelectedRowsCount == 1)
            {
                if (Convert.ToString(gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "aktiflik")) == "1")
                {
                    
                    barButtonItem20.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barButtonItem19.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

                }
                else if (Convert.ToString(gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "aktiflik")) == "0")
                {
                    barButtonItem20.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    barButtonItem19.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                popupMenu3.ShowPopup(MousePosition);
            }
        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                baglanti.Open();
                OleDbCommand sorgu = new OleDbCommand("UPDATE musteriler SET aktiflik=@aktiflik WHERE id like'" + Convert.ToString(gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "id")).ToString() + "'", baglanti);
                sorgu.Parameters.AddWithValue("aktiflik", "1");
                if (sorgu.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("Kayıt aktif edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                }
                else
                {
                    XtraMessageBox.Show("Kayıt aktif edilemedi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();
                
            }
            catch
            {

            }
            baglanti.Close();
            pasifmusterilistele();
        }

        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                baglanti.Open();
                OleDbCommand sorgu = new OleDbCommand("UPDATE musteriler SET aktiflik=@aktiflik WHERE id like'" + Convert.ToString(gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "id")).ToString() + "'", baglanti);
                sorgu.Parameters.AddWithValue("aktiflik", "0");
                if (sorgu.ExecuteNonQuery() == 1)
                {
                    XtraMessageBox.Show("Kayıt aktif edildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                }
                else
                {
                    XtraMessageBox.Show("Kayıt aktif edilemedi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                }
                baglanti.Close();

            }
            catch
            {

            }
            baglanti.Close();
            aktifmusterilistele();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            hızlısatısmusteriara ac = new hızlısatısmusteriara(baglanticümlecigi);
            ac.hızlstsfrm = this;
            ac.ShowDialog();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            simpleButton10.Visible = false;
            simpleButton2.Enabled = false;
            simpleButton1.Enabled = true;
            simpleButton3.Enabled = true;
            textEdit3.Enabled = true;
            musteriid.Text = "";
            textEdit2.Text = "";
        }
        public void hızlısatısveresiyebakiye()
        {
            baglanti.Open();
            DateTime dtm = DateTime.Now;
            string t = dtm.ToString();
            OleDbCommand komut = baglanti.CreateCommand();
            komut.CommandText = "insert into hızlısatısveresiyebakiye (hızlısatısveresiyeid,borc,tahsilat,bakiye,tarih,durum)" + "values (@hızlısatısveresiyeid,@borc,@tahsilat,@bakiye,@tarih,@durum)";
            komut.Parameters.Add("hızlısatısveresiyeid", OleDbType.VarChar).Value = dönenid;
            komut.Parameters.Add("borc", OleDbType.VarChar).Value = Convert.ToDouble(textEdit1.Text);
            komut.Parameters.Add("tahsilat", OleDbType.VarChar).Value = "0,00";
            komut.Parameters.Add("bakiye", OleDbType.VarChar).Value = Convert.ToDouble(textEdit1.Text);
            komut.Parameters.Add("tarih", OleDbType.VarChar).Value = t.ToString();
            komut.Parameters.Add("durum", OleDbType.VarChar).Value = "borç";
            komut.ExecuteNonQuery();
            baglanti.Close();

        }
        public void hızlısatıskasahareketiveresiye()
        {
            baglanti.Open();
            for (int i = 0; i < gridView2.RowCount; i++)
            {
                OleDbCommand komut = baglanti.CreateCommand();
                komut.CommandText = "insert into Hızlısatıskasahareketi (Veresiyeid,ürünadı,miktar,fiyat)" + "values (@Veresiyeid,@ürünadı,@miktar,@fiyat)";
                komut.Parameters.Add("Veresiyeid", OleDbType.VarChar).Value = dönenid;
                komut.Parameters.Add("ürünadı", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i,"Ürün Adı")).ToString();
                komut.Parameters.Add("miktar", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i, "Miktar")).ToString();
                komut.Parameters.Add("fiyat", OleDbType.VarChar).Value = Convert.ToString(gridView2.GetRowCellValue(i, "Tutar")).ToString();
                komut.ExecuteNonQuery();
            }
            baglanti.Close();
        }
        int dönenid;
        public void direksatısveresiye()
        {
            
            DateTime dtm = DateTime.Now;
            string t = dtm.ToString();
            if (musteriid.Text == "")
            {
                XtraMessageBox.Show("Veresiye Satış için lütfen müşteri seçiniz...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                if (textEdit1.Text == "" || textEdit1.Text == "0,00")
                {
                    XtraMessageBox.Show("girilen tahsilat sıfır olamaz", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    try
                    {
                        OleDbCommand komut = baglanti.CreateCommand();
                        komut.CommandText = "INSERT INTO hızlısatısveresiye (musteriid,tarih)" + "values (@musteriid,@tarih)";
                        string komut2 = "Select @@Identity";
                        komut.Parameters.Add("musteriid", OleDbType.VarChar).Value = musteriid.Text;
                        komut.Parameters.Add("tarih", OleDbType.VarChar).Value = t.ToString();
                        baglanti.Open();
                        if (komut.ExecuteNonQuery() == 1)
                        {
                            komut.CommandText = komut2;
                            dönenid = (int)komut.ExecuteScalar();
                            baglanti.Close();
                            hızlısatısveresiyebakiye();
                            baglanti.Close();
                            stokhareketi();
                            baglanti.Close();
                            hızlısatıskasahareketiveresiye();
                            baglanti.Close();
                            textEdit5.Text = "1";
                            textEdit3.Text = "0,00";
                            textEdit1.Text = "0,00";
                            textEdit4.Text = "";
                            alınanpara.Text = "0,00";
                            labelControl3.Text = "0,00";
                            simpleButton10.Visible = false;
                            simpleButton2.Enabled = false;
                            simpleButton1.Enabled = true;
                            simpleButton3.Enabled = true;
                            textEdit3.Enabled = true;
                            textEdit2.Text = "";
                            musteriid.Text = "";
                            gridControl2.DataSource = null;
                            gridView2.Columns.Clear();
                            satistablosu();
                            try
                            {
                                SoundPlayer ac = new SoundPlayer(Application.StartupPath + "\\kasa.wav");
                                ac.Play();
                            }
                            catch
                            {
                                baglanti.Close();
                            }



                        }



                    }
                    catch
                    {
                        baglanti.Close();
                    }

                }
            }
        }
        public void hızlısatıskasahareketiindirim()
        {
            baglanti.Open();
            OleDbCommand komut = baglanti.CreateCommand();
            komut.CommandText = "insert into Hızlısatıskasahareketi (Kasaid,ürünadı,miktar,fiyat)" + "values (@Kasaid,@ürünadı,@miktar,@fiyat)";
            komut.Parameters.Add("Veresiyeid", OleDbType.VarChar).Value = dönenid;
            komut.Parameters.Add("ürünadı", OleDbType.VarChar).Value = textEdit3.Text+"TL İskondolu Satış(Nakit)";
            komut.Parameters.Add("miktar", OleDbType.VarChar).Value = "1";
            komut.Parameters.Add("fiyat", OleDbType.VarChar).Value = "-"+textEdit3.Text;
            komut.ExecuteNonQuery();
            baglanti.Close();
        }
        public void direksatışindirimli()
        {
            DateTime dtm = DateTime.Now;
            string t = dtm.ToString();
            if (textEdit1.Text == "0" || textEdit1.Text == "0,00")
            {
                XtraMessageBox.Show("girilen tahsilat sıfır olamaz...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                try
                {
                    OleDbCommand komut = baglanti.CreateCommand();
                    komut.CommandText="INSERT INTO kasabankahareketleri (kasaid,kasagiristar,durum,kasatutar,tahakuktip,acıklama,kasatip)"+"values (@kasaid,@kasagiristar,@durum,@kasatutar,@tahakuktip,@acıklama,@kasatip)";
                    string komut2 = "Select @@Identity";
                    komut.Parameters.Add("kasaid", OleDbType.VarChar).Value = lookUpEdit1.EditValue;
                    komut.Parameters.Add("kasagiristar", OleDbType.VarChar).Value = t.ToString();
                    komut.Parameters.Add("durum", OleDbType.VarChar).Value = "Gelir";
                    komut.Parameters.Add("kasatutar", OleDbType.VarChar).Value = Convert.ToDouble(textEdit1.Text)-Convert.ToDouble(textEdit3.Text);
                    komut.Parameters.Add("tahakuktip", OleDbType.VarChar).Value = "4";
                    komut.Parameters.Add("acıklama", OleDbType.VarChar).Value = textEdit3.Text+"TL iskondolu satış(Nakit)";
                    komut.Parameters.Add("kasatip", OleDbType.VarChar).Value = "0";
                    baglanti.Open();
                    if(komut.ExecuteNonQuery()==1)
                    {
                        komut.CommandText = komut2;
                        dönenid = (int)komut.ExecuteScalar();
                        baglanti.Close();
                        stokhareketi();
                        baglanti.Close();
                        hızlısatıskasahareketi();
                        baglanti.Close();
                        hızlısatıskasahareketiindirim();
                        baglanti.Close();
                        textEdit5.Text = "1";
                        textEdit3.Text = "0,00";
                        textEdit1.Text = "0,00";
                        textEdit4.Text = "";
                        alınanpara.Text = "0,00";
                        labelControl3.Text = "0,00";
                        gridControl2.DataSource = null;
                        gridView2.Columns.Clear();
                        satistablosu();
                        try
                        {
                            SoundPlayer ac = new SoundPlayer(Application.StartupPath + "\\kasa.wav");
                            ac.Play();
                        }
                        catch
                        {
                            baglanti.Close();
                        }
                    }
                    


                }
                catch
                {
                }
            }

        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
             direksatısveresiye();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (textEdit3.Text == "" || textEdit3.Text == "0" || textEdit3.Text == "0,00")
            {
                XtraMessageBox.Show("indirim miktar sıfır olamaz...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            else
            {
                direksatışindirimli();
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            kasalistesitariharası();
        }
        public void kasalistesi()
        {

            string sorgu = "SELECT kasabankahareketleri.id,kasabankahareketleri.kasaid,kasabankahareketleri.kasagiristar,kasabankahareketleri.islemkod, " +
               "kasabankahareketleri.durum,kasabankahareketleri.kasatutar,gelirgidertürü.gelirgidertürü,kasabankahareketleri.acıklama, " +
               "kasabankahareketleri.kasatip,kasabankahareketleri.virmanid, " +
               "kasabankahareketleri.tahakuktip " +
               "FROM gelirgidertürü INNER JOIN kasabankahareketleri ON gelirgidertürü.id=kasabankahareketleri.tahakuktip " +
               "WHERE kasaid LIKE '" + kasaid + "' "+
               "ORDER BY kasabankahareketleri.id DESC";
            if (baglanti.State != ConnectionState.Open) baglanti.Open();
            OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
            OleDbDataReader oku = null;
            oku = veri.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Tarih", Type.GetType("System.String"));
            dt.Columns.Add("Durum", Type.GetType("System.String"));
            dt.Columns.Add("Türü", Type.GetType("System.String"));
            dt.Columns.Add("Açıklama", Type.GetType("System.String"));
            dt.Columns.Add("Tutar", Type.GetType("System.Object"));
            dt.Columns.Add("id", Type.GetType("System.Double"));           
            dt.Columns.Add("kasaid", Type.GetType("System.Double"));
            dt.Columns.Add("kasatip", Type.GetType("System.String"));
            dt.Columns.Add("virmanid", Type.GetType("System.String"));
          
            while (oku.Read())
            {
                double gidertutardeğişkeni = Convert.ToDouble(oku["kasatutar"].ToString());
                DataRow dr = dt.NewRow();
                dr[0] = oku["kasagiristar"].ToString();
                dr[1] = oku["durum"].ToString();
                dr[2] = oku["gelirgidertürü"].ToString();
                dr[3] = oku["acıklama"].ToString();
                dr[4] = gidertutardeğişkeni.ToString("#,##0.00");
                dr[5] = oku["id"].ToString();
                dr[6] = oku["kasaid"].ToString();
                dr[7] = oku["kasatip"].ToString();
                dr[8] = oku["virmanid"].ToString();
                dt.Rows.Add(dr);
            }
            gridControl4.DataSource = dt;
            oku.Close();
            baglanti.Close();
            gridView4.Columns["id"].Visible = false;          
            gridView4.Columns["kasaid"].Visible = false;
            gridView4.Columns["kasatip"].Visible = false;
            gridView4.Columns["virmanid"].Visible = false;           
            gridView4.IndicatorWidth = 40;
            totaltümü();
        }
        public void kasaderle(object sender, EventArgs e)
        {
            aktifkasa();
            kasalistesitariharası();
        }
        public void totaltümü()
        {
            try
            {
                string sorgu = "SELECT Sum(IIf([durum]='GELİR',[kasatutar],0)) AS giris1, Sum(IIf([durum]='GİDER',[kasatutar],0)) AS cikis1, Sum(IIf([durum]='GELİR',[kasatutar],0))  - Sum(IIf([durum]='GİDER',[kasatutar],0)) AS kalan FROM kasabankahareketleri WHERE  (((kasabankahareketleri.kasaid) Like '" + kasaid + "'))";
                if (baglanti.State != ConnectionState.Open) baglanti.Open();
                OleDbCommand veri = new OleDbCommand(sorgu, baglanti);
                OleDbDataReader oku = null;
                oku = veri.ExecuteReader();
                while (oku.Read())
                {
                    double girispara = Convert.ToDouble(oku["giris1"].ToString());
                    double cikispara = Convert.ToDouble(oku["cikis1"].ToString());
                    double bakiye = Convert.ToDouble(oku["kalan"].ToString());
                    girislabel.Text = girispara.ToString("#,##0.00");
                    giderlabel.Text = cikispara.ToString("#,##0.00");
                    bakiyelabel.Text = bakiye.ToString("#,##0.00");
                }
                oku.Close();
                baglanti.Close();
            }
            catch
            {
                //XtraMessageBox.Show("Veri Tabanında Bir Hata Oluştu \n lütfen Programı Kapatıp Tekrar Deneyiniz.....!", "HATA....", MessageBoxButtons.OK, MessageBoxIcon.Error);             
                baglanti.Close();
                girislabel.Text = "0,00";
                giderlabel.Text = "0,00";
                bakiyelabel.Text = "0,00";
            }

        }
        private void simpleButton19_Click(object sender, EventArgs e)
        {
            kasalistesi();
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            link.Component = gridControl4;
            link.Landscape = true;
            link.PageHeaderFooter = true;
            link.ShowPreview();  
        }

        private void simpleButton21_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx |RichText File (.rtf)|*.rtf |Pdf File (.pdf)|*.pdf |Html File (.html)|*.html";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtenstion = new FileInfo(exportFilePath).Extension;
                    switch (fileExtenstion)
                    {
                        case ".xls":
                            gridControl4.ExportToXls(exportFilePath);
                            break;

                        case ".xlsx":
                            gridControl4.ExportToXlsx(exportFilePath);

                            break;
                        case ".rtf":
                            gridControl4.ExportToRtf(exportFilePath);

                            break;
                        case ".pdf":
                            gridControl4.ExportToPdf(exportFilePath);
                            break;

                        case ".html":
                            gridControl4.ExportToHtml(exportFilePath);
                            break;

                        case ".mht":
                            gridControl4.ExportToMht(exportFilePath);
                            break;

                        default:
                            break;

                    }

                }

            }
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            kasabankatanımı ac = new kasabankatanımı(baglanticümlecigi);
            ac.kasafrm = this;
            ac.ShowDialog();

        }

        private void kasanınadı_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            gelirform ac = new gelirform(baglanticümlecigi, "0", kulid, kasaid.ToString());
            ac.ShowDialog();
        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void backstageViewTabItem5_SelectedChanged(object sender, DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs e)
        {

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            giderform ac = new giderform(baglanticümlecigi, "0", kulid, kasaid.ToString());
            ac.ShowDialog();
            
        }

        }

    }
