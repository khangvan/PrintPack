using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PrintPack
{
    public partial class FrmMessageBox : Form
    {
        public string msg;
        public FrmMessageBox()
        {
           
            InitializeComponent();
            msg = "";
        }
        public FrmMessageBox(string mesg)
        {
            InitializeComponent();
            msg = mesg;
        }

        private void FrmMessageBox_Load(object sender, EventArgs e)
        {
            //textBox1.Text = msg;
            textBox1.AppendText(msg);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
