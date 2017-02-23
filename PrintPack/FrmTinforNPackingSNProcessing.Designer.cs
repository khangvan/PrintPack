namespace PrintPack
{
    partial class FrmTinforNPackingSNProcessing
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.fFCPackingDataSet = new PrintPack.FFCPackingDataSet();
            this.t_InformationBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.t_InformationTableAdapter = new PrintPack.FFCPackingDataSetTableAdapters.T_InformationTableAdapter();
            this.tableAdapterManager = new PrintPack.FFCPackingDataSetTableAdapters.TableAdapterManager();
            this.packingRecordTableAdapter = new PrintPack.FFCPackingDataSetTableAdapters.PackingRecordTableAdapter();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.packingRecordBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fFCPackingDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.t_InformationBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.packingRecordBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // fFCPackingDataSet
            // 
            this.fFCPackingDataSet.DataSetName = "FFCPackingDataSet";
            this.fFCPackingDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // t_InformationBindingSource
            // 
            this.t_InformationBindingSource.DataMember = "T_Information";
            this.t_InformationBindingSource.DataSource = this.fFCPackingDataSet;
            // 
            // t_InformationTableAdapter
            // 
            this.t_InformationTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.BoxNumberTableAdapter = null;
            this.tableAdapterManager.ComPackingRecordTableAdapter = null;
            this.tableAdapterManager.DayTableTableAdapter = null;
            this.tableAdapterManager.LabelSmallTableAdapter = null;
            this.tableAdapterManager.MonthTableTableAdapter = null;
            this.tableAdapterManager.P_recordTableAdapter = null;
            this.tableAdapterManager.PackingRecordProcessingTableAdapter = null;
            this.tableAdapterManager.PackingRecordTableAdapter = this.packingRecordTableAdapter;
            this.tableAdapterManager.ProdPackingRecordTableAdapter = null;
            this.tableAdapterManager.ProductMapTableAdapter = null;
            this.tableAdapterManager.PullSNbyBOXTableAdapter = null;
            this.tableAdapterManager.ReworkRecordTableAdapter = null;
            this.tableAdapterManager.ShippingPlanTableAdapter = null;
            this.tableAdapterManager.T_InformationTableAdapter = this.t_InformationTableAdapter;
            this.tableAdapterManager.TBASE_SerialNumbersTableAdapter = null;
            this.tableAdapterManager.TDLM_SerialNumbersTableAdapter = null;
            this.tableAdapterManager.TFFC_SerialNumbersTableAdapter = null;
            this.tableAdapterManager.tmp_ProPackingRecordTableAdapter = null;
            this.tableAdapterManager.tmpBoxNumberTableAdapter = null;
            this.tableAdapterManager.tmpBoxReworkTableAdapter = null;
            this.tableAdapterManager.tmpBoxUnPackTableAdapter = null;
            this.tableAdapterManager.tmpBTFileMappingTableAdapter = null;
            this.tableAdapterManager.tmpCRTableAdapter = null;
            this.tableAdapterManager.tmpPackingRecordTableAdapter = null;
            this.tableAdapterManager.tmpRRTableAdapter = null;
            this.tableAdapterManager.tmpUARTableAdapter = null;
            this.tableAdapterManager.tmpURTableAdapter = null;
            this.tableAdapterManager.UnPackingAllRecordTableAdapter = null;
            this.tableAdapterManager.UnPackingPartsTableAdapter = null;
            this.tableAdapterManager.UnPackingRecordTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PrintPack.FFCPackingDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // packingRecordTableAdapter
            // 
            this.packingRecordTableAdapter.ClearBeforeFill = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 154);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 2;
            // 
            // packingRecordBindingSource
            // 
            this.packingRecordBindingSource.DataMember = "PackingRecord";
            this.packingRecordBindingSource.DataSource = this.fFCPackingDataSet;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 154);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "See Packing Detail";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(305, 154);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 181);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(695, 150);
            this.dataGridView1.TabIndex = 4;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(174, 89);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(122, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "See PO";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FrmTinforNPackingSNProcessing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 444);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Name = "FrmTinforNPackingSNProcessing";
            this.Text = "FrmTinforNPackingSNProcessing";
            this.Load += new System.EventHandler(this.FrmTinforNPackingSNProcessing_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fFCPackingDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.t_InformationBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.packingRecordBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FFCPackingDataSet fFCPackingDataSet;
        private System.Windows.Forms.BindingSource t_InformationBindingSource;
        private FFCPackingDataSetTableAdapters.T_InformationTableAdapter t_InformationTableAdapter;
        private FFCPackingDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private FFCPackingDataSetTableAdapters.PackingRecordTableAdapter packingRecordTableAdapter;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.BindingSource packingRecordBindingSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button3;
    }
}