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
    public partial class FrmTinforNPackingSNProcessing : Form
    {
        public FrmTinforNPackingSNProcessing()
        {
            InitializeComponent();
        }

        private void t_InformationBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.t_InformationBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.fFCPackingDataSet);

        }

        private void FrmTinforNPackingSNProcessing_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FFCPackingDataSet.PackingRecordProcessingDataTable dt = new FFCPackingDataSet.PackingRecordProcessingDataTable();
            FFCPackingDataSetTableAdapters.PackingRecordProcessingTableAdapter da = new FFCPackingDataSetTableAdapters.PackingRecordProcessingTableAdapter();
            da.Fill(dt, textBox1.Text.Trim());
            dataGridView1.DataSource = dt;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            FFCPackingDataSet.PackingRecordProcessingDataTable dt = new FFCPackingDataSet.PackingRecordProcessingDataTable();
            FFCPackingDataSetTableAdapters.PackingRecordProcessingTableAdapter da = new FFCPackingDataSetTableAdapters.PackingRecordProcessingTableAdapter();
            da.DeleteQuery(textBox1.Text.Trim());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FFCPackingDataSet.T_InformationDataTable dt = new FFCPackingDataSet.T_InformationDataTable();
            FFCPackingDataSetTableAdapters.T_InformationTableAdapter da = new FFCPackingDataSetTableAdapters.T_InformationTableAdapter();
            dt = da.GetData(textBox1.Text);
            dataGridView1.DataSource = dt;
        }
    }
}
