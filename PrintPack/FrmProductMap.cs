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
    public partial class FrmProductMap : Form
    {
        public FrmProductMap()
        {
            InitializeComponent();
        }

        private void productMapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.productMapBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.fFCPackingDataSet);

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            this.productMapTableAdapter.Fill(this.fFCPackingDataSet.ProductMap);

        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {

        }
    }
}
