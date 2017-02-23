using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//using vnmsrv601;
using ACSEE.NET;
using System.Xml;
using PrintPack;
using System.Data;
//using ACSoneTOOL.DBML;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
//using vnmsrv601;



namespace PrintPack
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts a List to a datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                dt.Columns.Add(property.Name, property.PropertyType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
   
    /// <summary>
    /// 
    /// </summary>
    /// 
  

   public  class PhanMem
    {
     
       // public static string chuoi_vnmsrv606 = ConfigurationManager.ConnectionStrings["chuoi_ket_noivnmsrv606"].ConnectionString;
       // public static string chuoi_ket_noivnmsrv606_ACSEE = ConfigurationManager.ConnectionStrings["chuoi_ket_noivnmsrv606_ACSEE"].ConnectionString;
       // public static string chuoi_ket_noivnmsrv608_FFCPacking = ConfigurationManager.ConnectionStrings["chuoi_ket_noivnmsrv608_FFCPacking"].ConnectionString;
       public static string chuoi_ket_noivnmsrv601_FFCPacking = ConfigurationManager.ConnectionStrings["chuoi_ket_noivnmsrv601_FFCPacking"].ConnectionString;
       // public static string chuoi_ket_noivnmsrv608_PTR = ConfigurationManager.ConnectionStrings["chuoi_ket_noivnmsrv608_PTR"].ConnectionString;
       // public static string chuoi_ket_noivnmsrv608_TestLog = ConfigurationManager.ConnectionStrings["chuoi_ket_noivnmsrv608_TestLog"].ConnectionString;
       // public static string chuoi_ket_noi_acs_ACSEE = ConfigurationManager.ConnectionStrings["chuoi_ket_noi_acs_ACSEE"].ConnectionString;
       // public static string chuoi_ket_noi_acs_ACSEE_EUG = ConfigurationManager.ConnectionStrings["chuoi_ket_noi_acs_ACSEE_EUG"].ConnectionString;
       // public static string chuoi_ket_noi_acs_ACSEESTATE = ConfigurationManager.ConnectionStrings["chuoi_ket_noi_acs_ACSEESTATE"].ConnectionString;
        public static string chuoi_ket_noi_acs_ACSClientState = ConfigurationManager.ConnectionStrings["chuoi_ket_noi_acs_ACSClientState"].ConnectionString;
        public static string chuoi_ket_noi_baocaosucodb = ConfigurationManager.ConnectionStrings["chuoi_ket_noi_baocaosucodb"].ConnectionString;
       
       // public static string btformatonserver = ConfigurationManager.AppSettings["btformatonserver"];

       // public static string btformatLocacted2printSN = ConfigurationManager.AppSettings["btformatLocacted2printSN"];
       // public static string listation4setuplabel = ConfigurationManager.AppSettings["listation4setuplabel"];
       //public static string btformatLocactedSingleBOXbig = ConfigurationManager.AppSettings["btformatLocactedSingleBOXbig"];
       //public static string btformatLocactedSingleBOXsmall = ConfigurationManager.AppSettings["btformatLocactedSingleBOXsmall"];
       ////link bt format
       // public static string btACSFFClabel = ConfigurationManager.AppSettings["btACSFFClabel"];

        //string connStr = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;

        //public static FFCPacking db608 = new FFCPacking(PhanMem.chuoi_ket_noivnmsrv601_FFCPacking);
        //public static FFCPacking db601 = new FFCPacking(PhanMem.chuoi_ket_noivnmsrv601_FFCPacking);
       // public static vnmacsdb.ACSEEClientState dbacsClientState = new vnmacsdb.ACSEEClientState(PhanMem.chuoi_ket_noi_acs_ACSClientState);


        public static Dictionary<string, clsPOSerials> dictPOInformation;


        public static clsPOSalesOrderInfo objSalesOrderInfo;
        public static string strPONumber;
        public static Boolean bFromSalesOrder = false;

        //public static DataTable ConvertListToDataTable2<T>(List<T> list)
        //{
        //    DataTable dt = new DataTable();

        //    foreach (PropertyInfo info in typeof(T).GetProperties())
        //    {
        //        dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        //    }
        //    foreach (T t in list)
        //    {
        //        DataRow row = dt.NewRow();
        //        foreach (PropertyInfo info in typeof(T).GetProperties())
        //        {
        //            row[info.Name] = info.GetValue(t, null);
        //        }
        //        dt.Rows.Add(row);
        //    }
        //    return dt;
        //}
        private static void CheckFileExistthenCopy(string Source, string Des)
        {
            // Using File
            if (!(File.Exists(Des)))
            {
                File.Copy(Source, Des);
            }


        }
        private static void CheckFolderExistthenCreate(string Des)
        {

            // Using File
            if (!(Directory.Exists(Des)))
            {
                Directory.CreateDirectory(Des);
            }
        }
        public static ArrayList DataSetToArrayList(int ColumnIndex, DataTable dataTable)
        {
            ArrayList output = new ArrayList();
            try
{
	
	
	            foreach (DataRow row in dataTable.Rows)
	                output.Add(row[ColumnIndex]);
	
	            return output;
}
catch (System.Exception ex)
{
 
}
            return output;
        }
        public static DataTable myJoinMethod(DataTable LeftTable, DataTable RightTable,
             String LeftPrimaryColumn, String RightPrimaryColumn)
        {
            //first create the datatable columns 
            DataSet mydataSet = new DataSet();
            mydataSet.Tables.Add("  ");
            DataTable myDataTable = mydataSet.Tables[0];

            //add left table columns 
            DataColumn[] dcLeftTableColumns = new DataColumn[LeftTable.Columns.Count];
            LeftTable.Columns.CopyTo(dcLeftTableColumns, 0);

            foreach (DataColumn LeftTableColumn in dcLeftTableColumns)
            {
                if (!myDataTable.Columns.Contains(LeftTableColumn.ToString()))
                    myDataTable.Columns.Add(LeftTableColumn.ToString());
            }

            //now add right table columns 
            DataColumn[] dcRightTableColumns = new DataColumn[RightTable.Columns.Count];
            RightTable.Columns.CopyTo(dcRightTableColumns, 0);

            foreach (DataColumn RightTableColumn in dcRightTableColumns)
            {
                if (!myDataTable.Columns.Contains(RightTableColumn.ToString()))
                {
                    if (RightTableColumn.ToString() != RightPrimaryColumn)
                        myDataTable.Columns.Add(RightTableColumn.ToString());
                }
            }

            //add left-table data to mytable 
            foreach (DataRow LeftTableDataRows in LeftTable.Rows)
            {
                myDataTable.ImportRow(LeftTableDataRows);
            }

            ArrayList var = new ArrayList(); //this variable holds the id's which have joined 

            ArrayList LeftTableIDs = new ArrayList();
            LeftTableIDs = DataSetToArrayList(0, LeftTable);

            //import righttable which having not equal Id's with lefttable 
            foreach (DataRow rightTableDataRows in RightTable.Rows)
            {
                if (LeftTableIDs.Contains(rightTableDataRows[0]))
                {
                    string wherecondition = "[" + myDataTable.Columns[0].ColumnName + "]='"
                            + rightTableDataRows[0].ToString() + "'";
                    DataRow[] dr = myDataTable.Select(wherecondition);
                    int iIndex = myDataTable.Rows.IndexOf(dr[0]);

                    foreach (DataColumn dc in RightTable.Columns)
                    {
                        if (dc.Ordinal != 0)
                            myDataTable.Rows[iIndex][dc.ColumnName.ToString().Trim()] =
                    rightTableDataRows[dc.ColumnName.ToString().Trim()].ToString();
                    }
                }
                else
                {
                    int count = myDataTable.Rows.Count;
                    DataRow row = myDataTable.NewRow();
                    row[0] = rightTableDataRows[0].ToString();
                    myDataTable.Rows.Add(row);
                    foreach (DataColumn dc in RightTable.Columns)
                    {
                        if (dc.Ordinal != 0)
                            myDataTable.Rows[count][dc.ColumnName.ToString().Trim()] =
                    rightTableDataRows[dc.ColumnName.ToString().Trim()].ToString();
                    }
                }
            }

            return myDataTable;
        }

        public static DataTable PullBOMbyPO2datatable(string strPONumber, string strModel)
        {
            DataTable goctbl = null;
            DataTable tbl = null;
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();

            string strAddress = "";

            XmlNode atestNode;
            XmlNode atestNode1;
            long lRetCode = -1;
            try
            {
                //connect


                if (strPONumber.ToString().Trim().Length > 5)
                {

                    strPONumber = strPONumber.ToString().PadLeft(12, '0');

                    //getPOInformation();
                    //MessageBox.Show("OK");
                }


                //Get PO SalesOrder Info
                sp = new SAPPost("ZRFC_SEND_PODATA_ACS");
                sp.setProperty("AUFNR", strPONumber);

                strAddress = "http://home/saplink/PRD/default.asp";
                mySX = sp.Post(strAddress);


                xmlDoc = mySX.getXDOC();



                atestNode = xmlDoc.GetElementsByTagName("RETURN_CODE").Item(0);
                atestNode1 = xmlDoc.GetElementsByTagName("item").Item(0);

                //DataSet tbl = new DataSet();
                //tbl = ConverttYourXmlNodeToDataSet(atestNode1);
                //goctbl= tbl.Tables[0];



                if (atestNode != null)
                {
                    if (atestNode.InnerText.ToString().Trim().Length > 0)
                    {




                        XmlNodeList BOM = xmlDoc.SelectNodes("//item");

                        int i = 0;
                        //tbl.Rows.Clear();

                        // DataSet tbl = ConverttYourXmlNodeToDataSet(atestNode);
                        goctbl = ConvertXmlNodeListToDataTable(BOM);

                        //tbl.Columns.Add("ProdOrder", typeof(string));
                        //tbl.Columns.Add("Material", typeof(string));
                        //tbl.Columns.Add("Partnumber", typeof(string));
                        //tbl.Columns.Add("REV", typeof(string));
                        //tbl.Columns.Add("Description", typeof(string));
                        //tbl.Columns.Add("MENGE", typeof(string));

                        foreach (XmlNode node in BOM)
                        {

                            XmlNode ProdOrder = node.SelectSingleNode("AUFNR");
                            XmlNode Material = node.SelectSingleNode("MATNR");
                            XmlNode Partnumber = node.SelectSingleNode("IDNRK");
                            XmlNode REV = node.SelectSingleNode("REVLV");
                            XmlNode Description = node.SelectSingleNode("MAKTX");
                            XmlNode MENGE = node.SelectSingleNode("MENGE");
                            XmlNode DATUV = node.SelectSingleNode("DATUV");
                            XmlNode DATIB = node.SelectSingleNode("DATIB");
                            string PullingBOMdate = "123";//DateTime.Now.ToShortDateString();


                            string[] row1 = new string[] { ProdOrder.InnerText, Material.InnerText, Partnumber.InnerText, REV.InnerText, Description.InnerText, MENGE.InnerText };
                            tbl.Rows.Add(row1);

                            //do insert 
                            string strDateTimeholder = "";
                            strDateTimeholder = DateTime.Today.Year.ToString();

                            strDateTimeholder += DateTime.Today.Month.ToString().PadLeft(2, '0');
                            strDateTimeholder += DateTime.Today.Day.ToString().PadLeft(2, '0');

                            //if (Material.InnerText.Trim() == strModel.Trim())
                            //{
                            //    string Stationname = "Audit01";
                            //    string BOMtype = "bPO";
                            //    InsertBOMLevelStation(
                            //        PhanMem.chuoi_ket_noivnmsrv608_PTR,
                            //        ProdOrder.InnerText,
                            //        Stationname,
                            //        Material.InnerText,
                            //        Partnumber.InnerText,
                            //        Description.InnerText,
                            //        REV.InnerText,
                            //        MENGE.InnerText,
                            //        strDateTimeholder,
                            //       strDateTimeholder,
                            //        BOMtype,
                            //        strDateTimeholder
                            //        );
                            //}

                            i++;

                        }
                    }

                }





            }


            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            return goctbl;
        }
        public static DataTable PullBOMstd2datatable(string strPONumber, string strModel)
        {
            DataTable goctbl = null;
            DataTable tbl = null;
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();

            string strAddress = "";

            XmlNode atestNode;
            XmlNode atestNode1;
            long lRetCode = -1;
            try
            {
                //connect


                if (strPONumber.ToString().Trim().Length > 5)
                {

                    strPONumber = strPONumber.ToString().PadLeft(12, '0');

                    //getPOInformation();
                    //MessageBox.Show("OK");
                }
                string strDateTimeholder = "";
                strDateTimeholder = DateTime.Today.Year.ToString();

                strDateTimeholder += DateTime.Today.Month.ToString().PadLeft(2, '0');
                strDateTimeholder += DateTime.Today.Day.ToString().PadLeft(2, '0');

                //Get PO SalesOrder Info
                sp = new SAPPost("Z_BAPI_BOM_PULL_LEVEL");
                sp.setProperty("WERKS", "3400");
                sp.setProperty("MATERIAL_NUMBER", strModel);
                sp.setProperty("VALID_FROM", strDateTimeholder);
                sp.setProperty("VALID_TO", strDateTimeholder);

                strAddress = "http://home/saplink/PRD/default.asp";
                mySX = sp.Post(strAddress);


                xmlDoc = mySX.getXDOC();



                atestNode = xmlDoc.GetElementsByTagName("ZBOM01").Item(0);
                atestNode1 = xmlDoc.GetElementsByTagName("item").Item(0);

                //DataSet tbl = new DataSet();
                //tbl = ConverttYourXmlNodeToDataSet(atestNode1);
                //goctbl= tbl.Tables[0];



                if (atestNode != null)
                {
                    if (atestNode.InnerText.ToString().Trim().Length > 0)
                    {




                        XmlNodeList BOM = xmlDoc.SelectNodes("//item");

                        int i = 0;
                        //tbl.Rows.Clear();

                        // DataSet tbl = ConverttYourXmlNodeToDataSet(atestNode);
                        goctbl = ConvertXmlNodeListToDataTable(BOM);

                        //tbl.Columns.Add("ProdOrder", typeof(string));
                        //tbl.Columns.Add("Material", typeof(string));
                        //tbl.Columns.Add("Partnumber", typeof(string));
                        //tbl.Columns.Add("REV", typeof(string));
                        //tbl.Columns.Add("Description", typeof(string));
                        //tbl.Columns.Add("MENGE", typeof(string));

                        foreach (XmlNode node in BOM)
                        {

                            //XmlNode ProdOrder = node.SelectSingleNode("AUFNR");
                            XmlNode Material = node.SelectSingleNode("MATNR");
                            XmlNode Partnumber = node.SelectSingleNode("IDNRK");
                            XmlNode REV = node.SelectSingleNode("REVLV");
                            XmlNode Description = node.SelectSingleNode("MAKTX");
                            XmlNode MENGE = node.SelectSingleNode("MENGE");
                            XmlNode DATUV = node.SelectSingleNode("DATUV");
                            XmlNode DATIB = node.SelectSingleNode("DATIB");
                            XmlNode Level = node.SelectSingleNode("STUFE");
                            string PullingBOMdate = "123";//DateTime.Now.ToShortDateString();


                             string[] row1 = new string[] { Material.InnerText.Trim(), Partnumber.InnerText.Trim(), REV.InnerText.Trim(), Description.InnerText.Trim(), MENGE.InnerText.Trim() };
                            tbl.Rows.Add(row1);

                            //do insert 
                            //string strDateTimeholder2 = "";
                            //strDateTimeholder = DateTime.Today.Year.ToString();

                            //strDateTimeholder2 += DateTime.Today.Month.ToString().PadLeft(2, '0');
                            //strDateTimeholder2 += DateTime.Today.Day.ToString().PadLeft(2, '0');

                            //if ((Material.InnerText.Trim() == strModel.Trim()) /*&& (Level.InnerText.Trim()=="1")*/)
                            //{
                            //    string Stationname = "Audit01";
                            //    string BOMtype = "std";
                            //    InsertBOMLevelStation(
                            //        PhanMem.chuoi_ket_noivnmsrv608_PTR,
                            //        strPONumber,
                            //        Stationname,
                            //        Material.InnerText,
                            //        Partnumber.InnerText,
                            //        Description.InnerText,
                            //        REV.InnerText,
                            //        MENGE.InnerText,
                            //        strDateTimeholder,
                            //       strDateTimeholder,
                            //        BOMtype,
                            //        strDateTimeholder
                            //        );
                            //}

                            i++;

                        }
                    }

                }





            }


            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return goctbl;
        }
        public static DataTable PullSN2datatable(string strPONumber)
        {
            DataTable goctbl = null;
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();

            string strAddress = "";

            XmlNode atestNode;
            XmlNode atestNode1;
            long lRetCode = -1;
            try
            {
                //connect


                if (strPONumber.ToString().Trim().Length > 5)
                {

                    strPONumber = strPONumber.ToString().PadLeft(12, '0');

                    //getPOInformation();
                    //MessageBox.Show("OK");
                }


                //Get PO SalesOrder Info
                sp = new SAPPost("ZRFC_SEND_POSERIALDATA_ACS");
                sp.setProperty("AUFNR", strPONumber);

                strAddress = "http://home/saplink/PRD/default.asp";
                mySX = sp.Post(strAddress);


                xmlDoc = mySX.getXDOC();



                atestNode = xmlDoc.GetElementsByTagName("RETURN_CODE").Item(0);
                atestNode1 = xmlDoc.GetElementsByTagName("item").Item(0);

                //DataSet tbl = new DataSet();
                //tbl = ConverttYourXmlNodeToDataSet(atestNode1);
                //goctbl= tbl.Tables[0];



                if (atestNode != null)
                {
                    if (atestNode.InnerText.ToString().Trim().Length > 0)
                    {




                        XmlNodeList BOM = xmlDoc.SelectNodes("//item");

                        int i = 0;
                        //tbl.Rows.Clear();

                        // DataSet tbl = ConverttYourXmlNodeToDataSet(atestNode);
                        goctbl = ConvertXmlNodeListToDataTable(BOM);

                        //tbl.Columns.Add("ProdOrder", typeof(string));
                        //tbl.Columns.Add("Material", typeof(string));
                        //tbl.Columns.Add("Partnumber", typeof(string));
                        //tbl.Columns.Add("REV", typeof(string));
                        //tbl.Columns.Add("Description", typeof(string));
                        //tbl.Columns.Add("MENGE", typeof(string));

                        //foreach (XmlNode node in BOM)
                        //{

                        //    XmlNode ProdOrder = node.SelectSingleNode("AUFNR");
                        //    XmlNode Material = node.SelectSingleNode("MATNR");
                        //    XmlNode Partnumber = node.SelectSingleNode("IDNRK");
                        //    XmlNode REV = node.SelectSingleNode("REVLV");
                        //    XmlNode Description = node.SelectSingleNode("MAKTX");
                        //    XmlNode MENGE = node.SelectSingleNode("MENGE");

                        //    string[] row1 = new string[] { ProdOrder.InnerText, Material.InnerText, Partnumber.InnerText, REV.InnerText, Description.InnerText, MENGE.InnerText };
                        //    tbl.Rows.Add(row1);





                        //    i++;

                        //}
                    }

                }





            }


            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            return goctbl;
        }


 public static void ForceBOMbymodel(string strModel)
 {
        try
            {//1.tao moi connection+gan string
                SqlConnection con = new SqlConnection(PhanMem.chuoi_ket_noi_acs_ACSClientState);
                //con.ConnectionString = "Server="+PlantComboBox.Text+"acsdb.eng.pscnet.com;Database=ACSEEClientState;User ID=reports;Password=reports;Trusted_Connection=False";
                //con.ConnectionString = @"File Name = C:\DLM ACS\connection.udl";

                //2. Sql query
               
                con.Open();
                //create command
                SqlCommand cmd = con.CreateCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.ame_force_bom_pull";

                cmd.Parameters.Add("@SAPModel", SqlDbType.VarChar, 20).Value = strModel;
                cmd.Parameters["@SAPModel"].Direction = ParameterDirection.Input;
                //cmd.Parameters.Add("@model", SqlDbType.VarChar, 20).Value = txtmodel.Text;
                //cmd.Parameters["@model"].Direction = ParameterDirection.Input;
                //cmd.ExecuteReader();

                int result = cmd.ExecuteNonQuery();

               // MessageBox.Show("Force repull BOM: "+result );
                //3. my adatater


                con.Close();
      


            }
            catch (Exception exp)
            {
               // MessageBox.Show(exp.Message);
            }

}

 public static DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
 {
     DataTable dt = new DataTable();
     int TempColumn = 0;
     foreach (XmlNode node in xnl.Item(0).ChildNodes)
     {
         TempColumn++; DataColumn dc = new DataColumn(node.Name, System.Type.GetType("System.String"));
         if (dt.Columns.Contains(node.Name))
         { dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString()); }
         else { dt.Columns.Add(dc); }
     } int ColumnsCount = dt.Columns.Count;
     for (int i = 0; i < xnl.Count; i++)
     { DataRow dr = dt.NewRow(); for (int j = 0; j < ColumnsCount; j++) { dr[j] = xnl.Item(i).ChildNodes[j].InnerText; } dt.Rows.Add(dr); } return dt;
 }


 public static DataTable PullBOM2datatable(string strPONumber)
 {
     DataTable goctbl = null;
     SAPXML mySX;
     SAPPost sp;
     XmlDocument xmlDoc = new XmlDocument();

     string strAddress = "";

     XmlNode atestNode;
     XmlNode atestNode1;
     long lRetCode = -1;
     try
     {
         //connect


         if (strPONumber.ToString().Trim().Length > 5)
         {

             strPONumber = strPONumber.ToString().PadLeft(12, '0');

             //getPOInformation();
             //MessageBox.Show("OK");
         }


         //Get PO SalesOrder Info
         sp = new SAPPost("ZRFC_SEND_PODATA_ACS");
         sp.setProperty("AUFNR", strPONumber);

         strAddress = "http://home/saplink/PRD/default.asp";
         mySX = sp.Post(strAddress);


         xmlDoc = mySX.getXDOC();



         atestNode = xmlDoc.GetElementsByTagName("RETURN_CODE").Item(0);
         atestNode1 = xmlDoc.GetElementsByTagName("item").Item(0);

         //DataSet tbl = new DataSet();
         //tbl = ConverttYourXmlNodeToDataSet(atestNode1);
         //goctbl= tbl.Tables[0];



         if (atestNode != null)
         {
             if (atestNode.InnerText.ToString().Trim().Length > 0)
             {




                 XmlNodeList BOM = xmlDoc.SelectNodes("//item");

                 int i = 0;
                 //tbl.Rows.Clear();

                 // DataSet tbl = ConverttYourXmlNodeToDataSet(atestNode);
                 goctbl = ConvertXmlNodeListToDataTable(BOM);

                 //tbl.Columns.Add("ProdOrder", typeof(string));
                 //tbl.Columns.Add("Material", typeof(string));
                 //tbl.Columns.Add("Partnumber", typeof(string));
                 //tbl.Columns.Add("REV", typeof(string));
                 //tbl.Columns.Add("Description", typeof(string));
                 //tbl.Columns.Add("MENGE", typeof(string));

                 //foreach (XmlNode node in BOM)
                 //{

                 //    XmlNode ProdOrder = node.SelectSingleNode("AUFNR");
                 //    XmlNode Material = node.SelectSingleNode("MATNR");
                 //    XmlNode Partnumber = node.SelectSingleNode("IDNRK");
                 //    XmlNode REV = node.SelectSingleNode("REVLV");
                 //    XmlNode Description = node.SelectSingleNode("MAKTX");
                 //    XmlNode MENGE = node.SelectSingleNode("MENGE");

                 //    string[] row1 = new string[] { ProdOrder.InnerText, Material.InnerText, Partnumber.InnerText, REV.InnerText, Description.InnerText, MENGE.InnerText };
                 //    tbl.Rows.Add(row1);





                 //    i++;

                 //}
             }

         }





     }


     catch (Exception ex)
     {
         //MessageBox.Show(ex.Message);
     }

     return goctbl;
 }
 
 public static string GetcontentCellvalue(DataGridViewCellEventArgs e, DataGridView datagridcheck)
 {
     string valueis=null;
     valueis = (datagridcheck.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
     //get cell value
     
       
     return valueis;

 }

 public static string GetcontentRANGECellvalue(DataGridViewCellEventArgs e, DataGridView datagridcheck)
 {
     

     //get string of 2 or 3 cell
     string stringis = null;
     stringis = (datagridcheck.Rows[0].Cells[2].Value.ToString()) + (datagridcheck.Rows[0].Cells[3].Value.ToString())
         + (datagridcheck.Rows[0].Cells[4].Value.ToString());

     return stringis;

 }
 public static void POnumberPadto12(TextBox txtboxname)
 {
     if (txtboxname.Text.ToString().Trim().Length > 5)
     {

         txtboxname.Text = txtboxname.Text.ToString().Trim().PadLeft(12, '0');

         //getPOInformation();
         //MessageBox.Show("OK");
     }
 }

 public static void getModelfromPOno(string strPONumber, out string modelname)
 {
     modelname = "";
     dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
     SAPXML mySX;
     SAPPost sp;
     XmlDocument xmlDoc = new XmlDocument();
     


     //strPONumber = txtPO.Text;

     string strAddress = "";

     XmlNode atestNode;
     XmlNode atestNode1;
     long lRetCode = -1;
     try
     {
         //Get PO SalesOrder Info
         sp = new SAPPost("ZRFC_SEND_PODATA_ACS");
         sp.setProperty("AUFNR", strPONumber);

         strAddress = "http://home/saplink/PRD/default.asp";
         mySX = sp.Post(strAddress);


         xmlDoc = mySX.getXDOC();



         atestNode = xmlDoc.GetElementsByTagName("RETURN_CODE").Item(0);
         atestNode1 = xmlDoc.GetElementsByTagName("MATERIAL").Item(0);//
         //MessageBox.Show(atestNode1.InnerText);
         modelname = atestNode1.InnerText;
     
     }




     catch (Exception ex)
     {
         //MessageBox.Show("Error on getting PO BOM=" + ex.Message);
     }
 }

        public static void PullBOM2datatable(string strPONumber, out DataTable tbl)
        {
            tbl = null;
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();

            string strAddress = "";

            XmlNode atestNode;
            XmlNode atestNode1;
            long lRetCode = -1;
            try
            {
                //connect
                

                if (strPONumber.ToString().Trim().Length > 5)
                {

                    strPONumber = strPONumber.ToString().PadLeft(12, '0');

                    //getPOInformation();
                    //MessageBox.Show("OK");
                }


                //Get PO SalesOrder Info
                sp = new SAPPost("ZRFC_SEND_PODATA_ACS");
                sp.setProperty("AUFNR", strPONumber);

                strAddress = "http://home/saplink/PRD/default.asp";
                mySX = sp.Post(strAddress);


                xmlDoc = mySX.getXDOC();



                atestNode = xmlDoc.GetElementsByTagName("RETURN_CODE").Item(0);
                atestNode1 = xmlDoc.GetElementsByTagName("item").Item(0);


                if (atestNode != null)
                {
                    if (atestNode.InnerText.ToString().Trim().Length > 0)
                    {



                        
                        XmlNodeList BOM = xmlDoc.SelectNodes("//item");
                        int i = 0;
                        //tbl.Rows.Clear();


                        tbl.Columns.Add("ProdOrder", typeof(string));
                        tbl.Columns.Add("Material", typeof(string));
                        tbl.Columns.Add("Partnumber", typeof(string));
                        tbl.Columns.Add("REV", typeof(string));
                        tbl.Columns.Add("Description", typeof(string));
                        tbl.Columns.Add("MENGE", typeof(string));

                        foreach (XmlNode node in BOM)
                        {

                            XmlNode ProdOrder = node.SelectSingleNode("AUFNR");
                            XmlNode Material = node.SelectSingleNode("MATNR");
                            XmlNode Partnumber = node.SelectSingleNode("IDNRK");
                            XmlNode REV = node.SelectSingleNode("REVLV");
                            XmlNode Description = node.SelectSingleNode("MAKTX");
                            XmlNode MENGE = node.SelectSingleNode("MENGE");

                            string[] row1 = new string[] { ProdOrder.InnerText, Material.InnerText, Partnumber.InnerText, REV.InnerText, Description.InnerText, MENGE.InnerText };
                            tbl.Rows.Add(row1);


                            


                            i++;

                        }
                    }

                }


               

                
            }


            catch (Exception ex)
            {
                
            }


            try
            {
                if (lRetCode == 0)
                {
                    objSalesOrderInfo = new clsPOSalesOrderInfo();
                    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strSalesOrder = atestNode.InnerText.ToString();
                    }

                    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER_ITEM").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strSalesItem = atestNode.InnerText.ToString();
                    }

                    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_MATERIAL").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCustomerMaterial = atestNode.InnerText.ToString();
                    }


                    atestNode = xmlDoc.GetElementsByTagName("MATERIAL").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.strMaterial = atestNode.InnerText.ToString();
                        }
                        else
                        {
                            objSalesOrderInfo.strMaterial = "";
                        }
                    }
                    else
                    {
                        objSalesOrderInfo.strMaterial = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("DESCRIPTION").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.strDescription = atestNode.InnerText.ToString();
                        }
                        else
                        {
                            objSalesOrderInfo.strDescription = "";
                        }
                    }
                    else
                    {
                        objSalesOrderInfo.strDescription = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("GRAVITY_ZONE").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.lGravityZone = Int32.Parse(atestNode.InnerText.ToString());
                        }
                        else
                        {
                            objSalesOrderInfo.lGravityZone = -1;
                        }
                    }
                    else
                    {
                    }



                    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_PO").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCustomerPurchaseOrder = atestNode.InnerText.ToString();
                    }
                    else
                    {
                    }


                    atestNode = xmlDoc.GetElementsByTagName("ATTN").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strAttn = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strAttn = "";
                    }

                    // new fields start here
                    atestNode = xmlDoc.GetElementsByTagName("CUSTNAME1").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCustName1 = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strCustName1 = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("STREETADDRESS").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strStreetAddress = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strStreetAddress = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("CITY").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCity = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strCity = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("REGION").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strStateRegion = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strStateRegion = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("POSTALCODE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strPostalCode = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strPostalCode = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("DESTINATIONCODE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strDestinationCode = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strDestinationCode = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("OTDDATE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.dtOTDDate = DateTime.Parse(atestNode.InnerText.ToString());
                    }
                    else
                    {
                        objSalesOrderInfo.dtOTDDate = DateTime.Parse("");
                    }


                    atestNode = xmlDoc.GetElementsByTagName("QTY").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.lQty = Int32.Parse(atestNode.InnerText.ToString());
                        }
                    }
                    else
                    {
                        objSalesOrderInfo.lQty = 100000;
                    }




                    atestNode = xmlDoc.GetElementsByTagName("VENDOR").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strVendor = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strVendor = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("COUNTRY").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCountry = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strCountry = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("HIERARCHY").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strHierarchy = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strHierarchy = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("BLOCKINGCODE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strBlockingCode = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strBlockingCode = "";
                    }






                    if (objSalesOrderInfo.strSalesOrder.Trim().Length > 0 || objSalesOrderInfo.strCustName1.Trim().Length > 0)
                    {
                        bFromSalesOrder = true;
                        //this.checkBox1.IsChecked = true;
                    }
                    else
                    {
                        bFromSalesOrder = false;
                        //this.checkBox1.IsChecked = false;
                    }




                } //  if (lRetCode == 0)
                else
                {
                    objSalesOrderInfo = new clsPOSalesOrderInfo();
                    objSalesOrderInfo.strSalesOrder = "";
                    objSalesOrderInfo.strSalesItem = "";
                    objSalesOrderInfo.strCustomerMaterial = "";
                    objSalesOrderInfo.strMaterial = "";
                    objSalesOrderInfo.strDescription = "";
                    objSalesOrderInfo.lGravityZone = -1;
                    objSalesOrderInfo.strCustomerPurchaseOrder = "";
                    objSalesOrderInfo.strAttn = "";
                    objSalesOrderInfo.strCustName1 = "";
                    objSalesOrderInfo.strStreetAddress = "";
                    objSalesOrderInfo.strCity = "";
                    objSalesOrderInfo.strStateRegion = "";
                    objSalesOrderInfo.strPostalCode = "";
                    objSalesOrderInfo.strDestinationCode = "";
                    objSalesOrderInfo.dtOTDDate = DateTime.Parse("1/1/2001");
                    objSalesOrderInfo.lQty = 100000;
                    objSalesOrderInfo.strVendor = "";
                    objSalesOrderInfo.strCountry = "";
                    objSalesOrderInfo.strHierarchy = "";
                    objSalesOrderInfo.strBlockingCode = "";
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        public static void PullModelDescription2string(string model, out string desp, out string rev)
        {
            model = model.ToUpper();
            rev = "";
            desp = "";
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            //XmlNodeList oNodes;


            //string strModel = txtGetmodel.Text;

            string strAddress = "";

            XmlNode atestNode;
            XmlNode atestNode1;
            XmlNode atestNode2;
            XmlNode atestNode3;

            long lRetCode = -1;
            string strDateTimeholder;
            try
            {

                strDateTimeholder = DateTime.Today.Year.ToString();

                strDateTimeholder += DateTime.Today.Month.ToString().PadLeft(2, '0');
                strDateTimeholder += DateTime.Today.Day.ToString().PadLeft(2, '0');
                //Get PO SalesOrder Info
                sp = new SAPPost("Z_BAPI_BOM_PULL_LEVEL");
                sp.setProperty("MATERIAL_NUMBER", model);
                sp.setProperty("VALID_FROM", strDateTimeholder);
                sp.setProperty("VALID_TO", strDateTimeholder);

                strAddress = "http://home/saplink/PRD/default.asp";
                mySX = sp.Post(strAddress);


                xmlDoc = mySX.getXDOC();

                //xmlDoc = mySX.getXDOC();
                //oNodes = xmlDoc.GetElementsByTagName("MAT_DESC");
                //strTopDescription = oNodes.Item(0).InnerText;
                //oNodes = xmlDoc.GetElementsByTagName("TOP_REVLV");
                //strTopRev = oNodes.Item(0).InnerText;


                atestNode = xmlDoc.GetElementsByTagName("ZBOM01").Item(0);
                atestNode1 = xmlDoc.GetElementsByTagName("item").Item(0);

                atestNode2 = xmlDoc.GetElementsByTagName("MAT_DESC").Item(0);
                atestNode3 = xmlDoc.GetElementsByTagName("TOP_REVLV").Item(0);

                desp = atestNode2.InnerText;
                rev = atestNode3.InnerText;

            }




            catch (Exception ex)
            {
                //MessageBox.Show("Error on getting PO BOM=" + ex.Message);
            }


            try
            {
                if (lRetCode == 0)
                {
                    objSalesOrderInfo = new clsPOSalesOrderInfo();
                    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strSalesOrder = atestNode.InnerText.ToString();
                    }

                    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER_ITEM").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strSalesItem = atestNode.InnerText.ToString();
                    }

                    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_MATERIAL").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCustomerMaterial = atestNode.InnerText.ToString();
                    }


                    atestNode = xmlDoc.GetElementsByTagName("MATERIAL").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.strMaterial = atestNode.InnerText.ToString();
                        }
                        else
                        {
                            objSalesOrderInfo.strMaterial = "";
                        }
                    }
                    else
                    {
                        objSalesOrderInfo.strMaterial = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("DESCRIPTION").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.strDescription = atestNode.InnerText.ToString();
                        }
                        else
                        {
                            objSalesOrderInfo.strDescription = "";
                        }
                    }
                    else
                    {
                        objSalesOrderInfo.strDescription = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("GRAVITY_ZONE").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.lGravityZone = Int32.Parse(atestNode.InnerText.ToString());
                        }
                        else
                        {
                            objSalesOrderInfo.lGravityZone = -1;
                        }
                    }
                    else
                    {
                    }



                    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_PO").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCustomerPurchaseOrder = atestNode.InnerText.ToString();
                    }
                    else
                    {
                    }


                    atestNode = xmlDoc.GetElementsByTagName("ATTN").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strAttn = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strAttn = "";
                    }

                    // new fields start here
                    atestNode = xmlDoc.GetElementsByTagName("CUSTNAME1").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCustName1 = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strCustName1 = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("STREETADDRESS").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strStreetAddress = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strStreetAddress = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("CITY").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCity = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strCity = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("REGION").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strStateRegion = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strStateRegion = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("POSTALCODE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strPostalCode = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strPostalCode = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("DESTINATIONCODE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strDestinationCode = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strDestinationCode = "";
                    }



                    atestNode = xmlDoc.GetElementsByTagName("OTDDATE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.dtOTDDate = DateTime.Parse(atestNode.InnerText.ToString());
                    }
                    else
                    {
                        objSalesOrderInfo.dtOTDDate = DateTime.Parse("");
                    }


                    atestNode = xmlDoc.GetElementsByTagName("QTY").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0)
                        {
                            objSalesOrderInfo.lQty = Int32.Parse(atestNode.InnerText.ToString());
                        }
                    }
                    else
                    {
                        objSalesOrderInfo.lQty = 100000;
                    }




                    atestNode = xmlDoc.GetElementsByTagName("VENDOR").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strVendor = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strVendor = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("COUNTRY").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strCountry = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strCountry = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("HIERARCHY").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strHierarchy = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strHierarchy = "";
                    }


                    atestNode = xmlDoc.GetElementsByTagName("BLOCKINGCODE").Item(0);
                    if (atestNode != null)
                    {
                        objSalesOrderInfo.strBlockingCode = atestNode.InnerText.ToString();
                    }
                    else
                    {
                        objSalesOrderInfo.strBlockingCode = "";
                    }






                    if (objSalesOrderInfo.strSalesOrder.Trim().Length > 0 || objSalesOrderInfo.strCustName1.Trim().Length > 0)
                    {
                        bFromSalesOrder = true;
                        //this.checkBox1.IsChecked = true;
                    }
                    else
                    {
                        bFromSalesOrder = false;
                        //this.checkBox1.IsChecked = false;
                    }




                } //  if (lRetCode == 0)
                else
                {
                    objSalesOrderInfo = new clsPOSalesOrderInfo();
                    objSalesOrderInfo.strSalesOrder = "";
                    objSalesOrderInfo.strSalesItem = "";
                    objSalesOrderInfo.strCustomerMaterial = "";
                    objSalesOrderInfo.strMaterial = "";
                    objSalesOrderInfo.strDescription = "";
                    objSalesOrderInfo.lGravityZone = -1;
                    objSalesOrderInfo.strCustomerPurchaseOrder = "";
                    objSalesOrderInfo.strAttn = "";
                    objSalesOrderInfo.strCustName1 = "";
                    objSalesOrderInfo.strStreetAddress = "";
                    objSalesOrderInfo.strCity = "";
                    objSalesOrderInfo.strStateRegion = "";
                    objSalesOrderInfo.strPostalCode = "";
                    objSalesOrderInfo.strDestinationCode = "";
                    objSalesOrderInfo.dtOTDDate = DateTime.Parse("1/1/2001");
                    objSalesOrderInfo.lQty = 100000;
                    objSalesOrderInfo.strVendor = "";
                    objSalesOrderInfo.strCountry = "";
                    objSalesOrderInfo.strHierarchy = "";
                    objSalesOrderInfo.strBlockingCode = "";
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show("Error on getting PO BOM fields=" + ex.Message);
            }



        }
        //COPPY RECORD THEN INSSER

        public static List<object> ThucHienLenhACCESS_hamchung(string chuoiketnoi, string lenh)
        {
            OleDbDataReader kq = null;
            OleDbConnection conoledb = new OleDbConnection(chuoi_ket_noi_baocaosucodb);
            try
            {
                
                conoledb.Open();
                
                OleDbCommand bolenh = new OleDbCommand(lenh, conoledb);

                bolenh.CommandText = lenh;
                kq = bolenh.ExecuteReader();

                if (kq.FieldCount == 0)
                {
                    // return kq;
                }
                return kq.Cast<object>().ToList();
            }
            catch (Exception ex)
            {
                kq = null;

            }


            conoledb.Close();
            return kq.Cast<object>().ToList();
        }

        public static List<object> ThucHienLenh_hamchung(string chuoiketnoi, string lenh)
        {

            List<object> a = null;

            SqlDataReader kq = null;
            SqlConnection cn = new SqlConnection(chuoiketnoi);
            try
            {
                if (cn.State == System.Data.ConnectionState.Closed) cn.Open();
                SqlCommand bolenh = cn.CreateCommand();
                //DbCommand bolenh = PhanMem.dbacsClientState.Connection.CreateCommand();
                bolenh.CommandText = lenh;
                kq = bolenh.ExecuteReader();
                try
                {
                    if (kq.FieldCount != 0)
                    {
                        // return kq;
                        //return kq.Cast<object>().ToList();
                        a = kq.Cast<object>().ToList();
                    }
                }
                catch (Exception ex)
                {
                    //kq = null;
                    // return;

                }



            }
            catch (Exception ex)
            {
               // kq = null;

            }


            cn.Close();

            //return kq.Cast<object>().ToList();
            return a;

        }
        //public static DataTable ToDataTable<T>(this IList<T> data)
        //{
        //    //PropertyDescriptorCollection properties =
        //    //    TypeDescriptor.GetProperties(typeof(T));
        //    DataTable table = new DataTable();
        //    //foreach (PropertyDescriptor prop in properties)
        //    //    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        //    //foreach (T item in data)
        //    //{
        //    //    DataRow row = table.NewRow();
        //    //    foreach (PropertyDescriptor prop in properties)
        //    //        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //    //    table.Rows.Add(row);
        //    //}
        //    return table;
        //}
        public static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            // Add columns.
            for (int i = 0; i < columns; i++)
            {
                table.Columns.Add();
            }

            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }

            return table;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {//ok
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
        public static int ThucHienLenhCapNhat_hamchung(string chuoiketnoi, string lenh)
        {

            int kq = -1;
            SqlConnection cn = new SqlConnection(chuoiketnoi);
            try
            {
                if (cn.State == System.Data.ConnectionState.Closed) cn.Open();
                SqlCommand bolenh = cn.CreateCommand();
                bolenh.CommandText = lenh;
                kq = bolenh.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                kq = -1;
                MessageBox.Show(ex.Message);

            }
            finally
            {
                cn.Close();
            }
            return kq;

        }

    
        public static float ThucHienLenhTinhToanACCESS_hamchung(string chuoiketnoi, string lenh)
        {
            float kq = -1;
            OleDbConnection cn = new OleDbConnection(chuoiketnoi);
            try
            {
                if (cn.State == System.Data.ConnectionState.Closed) cn.Open();
                OleDbCommand bolenh = cn.CreateCommand();
                bolenh.CommandText = lenh;
                kq = float.Parse(bolenh.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                kq = -1;

            }
            finally
            {
                cn.Close();
            }
            return kq;

        }
        public static float ThucHienLenhTinhToan_hamchung(string chuoiketnoi, string lenh)
        {
            float kq = -1;
            SqlConnection cn = new SqlConnection(chuoiketnoi);
            try
            {
                if (cn.State == System.Data.ConnectionState.Closed) cn.Open();
                SqlCommand bolenh = cn.CreateCommand();
                bolenh.CommandText = lenh;
                kq = float.Parse(bolenh.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                kq = -1;

            }
            finally
            {
                cn.Close();
            }
            return kq;

        }

        public static string ThucHienLenh_1giatritrave_hamchung(string chuoiketnoi, string lenh)
        {
            string kq = "";
            SqlConnection cn = new SqlConnection(chuoiketnoi);
            try
            {
                if (cn.State == System.Data.ConnectionState.Closed) cn.Open();
                SqlCommand bolenh = cn.CreateCommand();
                bolenh.CommandText = lenh;
                kq = (bolenh.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                kq = "";

            }
            finally
            {
                cn.Close();
            }
            return kq;

        }

        public static void readtxtfile2richtextbox(string strLocation, RichTextBox rtb)
        {

            try
            {
                StreamReader mo_file = new StreamReader(strLocation);
                rtb.Text = mo_file.ReadToEnd();
                mo_file.Close();
                //cuon den bottom
                rtb.SelectionStart = rtb.Text.Length;
                rtb.ScrollToCaret();
                rtb.Refresh();
            }
            catch (System.Exception ex)
            {

            }
            //
        }
        public static string ThucHienLenhACCESS_1giatritrave_hamchung(string chuoiketnoi, string lenh)
        {
            string kq = "";
            OleDbConnection cn = new OleDbConnection(chuoiketnoi);
            try
            {
                if (cn.State == System.Data.ConnectionState.Closed) cn.Open();
                OleDbCommand bolenh = cn.CreateCommand();
                bolenh.CommandText = lenh;
                kq = (bolenh.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                kq = "";

            }
            finally
            {
                cn.Close();
            }
            return kq;

        }
      

    }
}
