namespace PrintPack
{
    partial class FrmProductMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProductMap));
            System.Windows.Forms.Label sAPModelLabel;
            System.Windows.Forms.Label productMapLabel;
            System.Windows.Forms.Label descriptionLabel;
            System.Windows.Forms.Label productLineLabel;
            System.Windows.Forms.Label mPNLabel;
            this.fFCPackingDataSet = new PrintPack.FFCPackingDataSet();
            this.productMapBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.productMapTableAdapter = new PrintPack.FFCPackingDataSetTableAdapters.ProductMapTableAdapter();
            this.tableAdapterManager = new PrintPack.FFCPackingDataSetTableAdapters.TableAdapterManager();
            this.productMapBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.productMapBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.productMapDataGridView = new System.Windows.Forms.DataGridView();
            this.sAPModelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productMapDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productLineDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mPNDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sAPModelTextBox = new System.Windows.Forms.TextBox();
            this.productMapTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.productLineTextBox = new System.Windows.Forms.TextBox();
            this.mPNTextBox = new System.Windows.Forms.TextBox();
            sAPModelLabel = new System.Windows.Forms.Label();
            productMapLabel = new System.Windows.Forms.Label();
            descriptionLabel = new System.Windows.Forms.Label();
            productLineLabel = new System.Windows.Forms.Label();
            mPNLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fFCPackingDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productMapBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productMapBindingNavigator)).BeginInit();
            this.productMapBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productMapDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // fFCPackingDataSet
            // 
            this.fFCPackingDataSet.DataSetName = "FFCPackingDataSet";
            this.fFCPackingDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // productMapBindingSource
            // 
            this.productMapBindingSource.DataMember = "ProductMap";
            this.productMapBindingSource.DataSource = this.fFCPackingDataSet;
            // 
            // productMapTableAdapter
            // 
            this.productMapTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.PackingRecordTableAdapter = null;
            this.tableAdapterManager.ProdPackingRecordTableAdapter = null;
            this.tableAdapterManager.ProductMapTableAdapter = this.productMapTableAdapter;
            this.tableAdapterManager.ReworkRecordTableAdapter = null;
            this.tableAdapterManager.ShippingPlanTableAdapter = null;
            this.tableAdapterManager.T_InformationTableAdapter = null;
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
            // productMapBindingNavigator
            // 
            this.productMapBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.productMapBindingNavigator.BindingSource = this.productMapBindingSource;
            this.productMapBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.productMapBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.productMapBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.productMapBindingNavigatorSaveItem});
            this.productMapBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.productMapBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.productMapBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.productMapBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.productMapBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.productMapBindingNavigator.Name = "productMapBindingNavigator";
            this.productMapBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.productMapBindingNavigator.Size = new System.Drawing.Size(664, 25);
            this.productMapBindingNavigator.TabIndex = 0;
            this.productMapBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // productMapBindingNavigatorSaveItem
            // 
            this.productMapBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.productMapBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("productMapBindingNavigatorSaveItem.Image")));
            this.productMapBindingNavigatorSaveItem.Name = "productMapBindingNavigatorSaveItem";
            this.productMapBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.productMapBindingNavigatorSaveItem.Text = "Save Data";
            this.productMapBindingNavigatorSaveItem.Click += new System.EventHandler(this.productMapBindingNavigatorSaveItem_Click);
            // 
            // productMapDataGridView
            // 
            this.productMapDataGridView.AutoGenerateColumns = false;
            this.productMapDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.productMapDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sAPModelDataGridViewTextBoxColumn,
            this.productMapDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.productLineDataGridViewTextBoxColumn,
            this.mPNDataGridViewTextBoxColumn});
            this.productMapDataGridView.DataSource = this.productMapBindingSource;
            this.productMapDataGridView.Location = new System.Drawing.Point(12, 81);
            this.productMapDataGridView.Name = "productMapDataGridView";
            this.productMapDataGridView.Size = new System.Drawing.Size(629, 454);
            this.productMapDataGridView.TabIndex = 1;
            // 
            // sAPModelDataGridViewTextBoxColumn
            // 
            this.sAPModelDataGridViewTextBoxColumn.DataPropertyName = "SAPModel";
            this.sAPModelDataGridViewTextBoxColumn.HeaderText = "SAPModel";
            this.sAPModelDataGridViewTextBoxColumn.Name = "sAPModelDataGridViewTextBoxColumn";
            // 
            // productMapDataGridViewTextBoxColumn
            // 
            this.productMapDataGridViewTextBoxColumn.DataPropertyName = "ProductMap";
            this.productMapDataGridViewTextBoxColumn.HeaderText = "ProductMap";
            this.productMapDataGridViewTextBoxColumn.Name = "productMapDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // productLineDataGridViewTextBoxColumn
            // 
            this.productLineDataGridViewTextBoxColumn.DataPropertyName = "ProductLine";
            this.productLineDataGridViewTextBoxColumn.HeaderText = "ProductLine";
            this.productLineDataGridViewTextBoxColumn.Name = "productLineDataGridViewTextBoxColumn";
            // 
            // mPNDataGridViewTextBoxColumn
            // 
            this.mPNDataGridViewTextBoxColumn.DataPropertyName = "MPN";
            this.mPNDataGridViewTextBoxColumn.HeaderText = "MPN";
            this.mPNDataGridViewTextBoxColumn.Name = "mPNDataGridViewTextBoxColumn";
            // 
            // sAPModelLabel
            // 
            sAPModelLabel.AutoSize = true;
            sAPModelLabel.Location = new System.Drawing.Point(9, 32);
            sAPModelLabel.Name = "sAPModelLabel";
            sAPModelLabel.Size = new System.Drawing.Size(60, 13);
            sAPModelLabel.TabIndex = 2;
            sAPModelLabel.Text = "SAPModel:";
            // 
            // sAPModelTextBox
            // 
            this.sAPModelTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.productMapBindingSource, "SAPModel", true));
            this.sAPModelTextBox.Location = new System.Drawing.Point(86, 29);
            this.sAPModelTextBox.Name = "sAPModelTextBox";
            this.sAPModelTextBox.Size = new System.Drawing.Size(100, 20);
            this.sAPModelTextBox.TabIndex = 3;
            // 
            // productMapLabel
            // 
            productMapLabel.AutoSize = true;
            productMapLabel.Location = new System.Drawing.Point(9, 58);
            productMapLabel.Name = "productMapLabel";
            productMapLabel.Size = new System.Drawing.Size(71, 13);
            productMapLabel.TabIndex = 4;
            productMapLabel.Text = "Product Map:";
            // 
            // productMapTextBox
            // 
            this.productMapTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.productMapBindingSource, "ProductMap", true));
            this.productMapTextBox.Location = new System.Drawing.Point(86, 55);
            this.productMapTextBox.Name = "productMapTextBox";
            this.productMapTextBox.Size = new System.Drawing.Size(100, 20);
            this.productMapTextBox.TabIndex = 5;
            // 
            // descriptionLabel
            // 
            descriptionLabel.AutoSize = true;
            descriptionLabel.Location = new System.Drawing.Point(245, 28);
            descriptionLabel.Name = "descriptionLabel";
            descriptionLabel.Size = new System.Drawing.Size(63, 13);
            descriptionLabel.TabIndex = 6;
            descriptionLabel.Text = "Description:";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.productMapBindingSource, "Description", true));
            this.descriptionTextBox.Location = new System.Drawing.Point(322, 25);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(319, 20);
            this.descriptionTextBox.TabIndex = 7;
            // 
            // productLineLabel
            // 
            productLineLabel.AutoSize = true;
            productLineLabel.Location = new System.Drawing.Point(245, 54);
            productLineLabel.Name = "productLineLabel";
            productLineLabel.Size = new System.Drawing.Size(70, 13);
            productLineLabel.TabIndex = 8;
            productLineLabel.Text = "Product Line:";
            // 
            // productLineTextBox
            // 
            this.productLineTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.productMapBindingSource, "ProductLine", true));
            this.productLineTextBox.Location = new System.Drawing.Point(322, 51);
            this.productLineTextBox.Name = "productLineTextBox";
            this.productLineTextBox.Size = new System.Drawing.Size(100, 20);
            this.productLineTextBox.TabIndex = 9;
            // 
            // mPNLabel
            // 
            mPNLabel.AutoSize = true;
            mPNLabel.Location = new System.Drawing.Point(464, 54);
            mPNLabel.Name = "mPNLabel";
            mPNLabel.Size = new System.Drawing.Size(34, 13);
            mPNLabel.TabIndex = 10;
            mPNLabel.Text = "MPN:";
            // 
            // mPNTextBox
            // 
            this.mPNTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.productMapBindingSource, "MPN", true));
            this.mPNTextBox.Location = new System.Drawing.Point(541, 51);
            this.mPNTextBox.Name = "mPNTextBox";
            this.mPNTextBox.Size = new System.Drawing.Size(100, 20);
            this.mPNTextBox.TabIndex = 11;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 547);
            this.Controls.Add(sAPModelLabel);
            this.Controls.Add(this.sAPModelTextBox);
            this.Controls.Add(productMapLabel);
            this.Controls.Add(this.productMapTextBox);
            this.Controls.Add(descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(productLineLabel);
            this.Controls.Add(this.productLineTextBox);
            this.Controls.Add(mPNLabel);
            this.Controls.Add(this.mPNTextBox);
            this.Controls.Add(this.productMapDataGridView);
            this.Controls.Add(this.productMapBindingNavigator);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fFCPackingDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productMapBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productMapBindingNavigator)).EndInit();
            this.productMapBindingNavigator.ResumeLayout(false);
            this.productMapBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.productMapDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FFCPackingDataSet fFCPackingDataSet;
        private System.Windows.Forms.BindingSource productMapBindingSource;
        private FFCPackingDataSetTableAdapters.ProductMapTableAdapter productMapTableAdapter;
        private FFCPackingDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator productMapBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton productMapBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView productMapDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn sAPModelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productMapDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productLineDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mPNDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox sAPModelTextBox;
        private System.Windows.Forms.TextBox productMapTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox productLineTextBox;
        private System.Windows.Forms.TextBox mPNTextBox;
    }
}