using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ticariotomasyon
{
    public partial class hızlısatısalınanpara : DevExpress.XtraEditors.XtraForm
    {
        public AnaForm hızlıstsfrm;
        public hızlısatısalınanpara()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            AnaForm veri = (AnaForm)Application.OpenForms["AnaForm"];
            veri.alınanpara.Text = textEdit1.Text;             
            veri.paraüstüderle();
            Close();

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void hızlısatısalınanpara_Load(object sender, EventArgs e)
        {
            textEdit1.Text = "0,00";
        }
      
        private void simpleButton1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar !=','))
            {
                e.Handled=true;
            }
        }
    }
}