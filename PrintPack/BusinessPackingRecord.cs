using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using ACSEE.NET;
//using NetACS;

namespace PrintPack
{
    public class BusinessPackingRecord
    {
        public static string strSqlConnection4_608FFCPACKING = ConfigurationManager.AppSettings.Get("FFCPACKINGCONNECTION").ToString();
        public System.Data.SqlClient.SqlConnection sqlConnection4;
        public BusinessPackingRecord()
        {
           // strSqlConnection4_608FFCPACKING = ConfigurationManager.AppSettings.Get("FFCPACKINGCONNECTION").ToString();
        }

        public DataTable PackingGetDatabyBOX(string BoxNumber)
        {
             using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();

                #region Kiem tra Prod Order trong T_information
                string result = "Kiem tra ProdOrder trong T_information";
                
                SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                cmdGetProdOrderSerials.CommandText = "PackingRecord_GetByBox";

                cmdGetProdOrderSerials.Parameters.Add("@BoxNumber", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@BoxNumber"].Value = BoxNumber;
                cmdGetProdOrderSerials.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                
                SqlDataReader rec = cmdGetProdOrderSerials.ExecuteReader();
                rec.Read();

                result = rec[0].ToString();
                #endregion


                if (result.Equals("OK"))
                {

                    //TraceStepDoing("Doc thong Tinformation ");


                    //rec.NextResult();
                    //rec.Read();
                    //strPN = rec["T_ProdOrder"].ToString().Trim();
                    //strProductmap = rec["T_ProductMap"].ToString().Trim();
                    //strPN = rec["T_Material"].ToString().Trim();
                    //strRev = rec["T_Revision"].ToString().Trim();
                    //StrDes = "";
                    //string tmpProductMap = getProductMapDetail(strPN, ref StrDes);
                    //intPOQuantity = Int32.Parse(rec["T_Quantity"].ToString().Trim());
                    //intPOPacked = Int32.Parse(rec["T_Packed"].ToString().Trim());
                    sqlConnection4.Close();

                }

                DataTable d = new DataTable();
                d.Load(cmdGetProdOrderSerials.ExecuteReader());
                cmdGetProdOrderSerials.Dispose();
                
                sqlConnection4.Close();
                return d;



            }
        }


        private void ReadInforFromT_Information(string PO, ref string strPN, ref string strRev, ref string StrDes, ref string strProductmap)
        {


            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();

                #region Kiem tra Prod Order trong T_information
                string result = "Kiem tra ProdOrder trong T_information";
                PO = (PO).PadLeft(12, '0');

                SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderInfo"; ;
                cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = PO;
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                SqlDataReader rec = cmdGetProdOrderSerials.ExecuteReader();
                rec.Read();

                result = rec[0].ToString();
                #endregion


                if (result.Equals("OK"))
                {

                    //TraceStepDoing("Doc thong Tinformation ");


                    rec.NextResult();
                    rec.Read();
                    strPN = rec["T_ProdOrder"].ToString().Trim();
                    strProductmap = rec["T_ProductMap"].ToString().Trim();
                    strPN = rec["T_Material"].ToString().Trim();
                    strRev = rec["T_Revision"].ToString().Trim();
                    StrDes = "";
                    //string tmpProductMap = getProductMapDetail(strPN,ref StrDes); 
                    //intPOQuantity = Int32.Parse(rec["T_Quantity"].ToString().Trim());
                    //intPOPacked = Int32.Parse(rec["T_Packed"].ToString().Trim());
                    sqlConnection4.Close();

                }





            }
        }

        public static void DeleteSerialbyPO(string POnumber)
        {

            SP_Processing.MySqlConn mycon = new SP_Processing.MySqlConn(strSqlConnection4_608FFCPACKING);
            mycon.ExecSProc("usp_TFFC_SerialNumbersDelete", POnumber.PadLeft(12,'0'));

                   


        }

        public static void CheckSNListandReloadifProblem(string POnumber)
        {

            POnumber = POnumber.PadLeft(12, '0');
            //do check SN đúng theo PO
            SP_Processing.MySqlConn mycon = new SP_Processing.MySqlConn(strSqlConnection4_608FFCPACKING);
           // DataTable dt= mycon.ExecSProcDS("ame_CheckSNDLSS", POnumber.PadLeft(12, '0')).Tables["Table"];
            string result = mycon.ExecSProcDS("ame_CheckSNDLSS", POnumber.PadLeft(12, '0')).Tables["Table"].Rows[0][0].ToString();

            //if not, please load

            if (!(result.Substring(0,2)=="OK"))
            {
                //load SN
                ReloadSerialbyPO(POnumber);
                UpdateT_InformartionwPOqty(POnumber);
                MessageBox.Show("Hoàn tất update data SN!");

            }
            else
            {
                //Order good
                MessageBox.Show("Load SN bình thường. No Action!");
            }


        }

        public static void CheckSNperLoadedListandReloadifProblem( string SN,string POnumber)
        {

            POnumber = POnumber.PadLeft(12, '0');
            //do check SN đúng theo PO
            SP_Processing.MySqlConn mycon = new SP_Processing.MySqlConn(strSqlConnection4_608FFCPACKING);
            // DataTable dt= mycon.ExecSProcDS("ame_CheckSNDLSS", POnumber.PadLeft(12, '0')).Tables["Table"];
            string result = mycon.ExecSProcDS("ame_CheckSNvsTFFCCONTROL",SN.ToString(), POnumber.PadLeft(12, '0')).Tables["Table"].Rows[0][0].ToString().Trim();

            //if not, please load

            if ((result.Substring(0, 2) == "OK"))
            {
                //Order good
                MessageBox.Show("Load SN bình thường. No Action!");

            }
            else
            {
                DeleteSerialbyPO(POnumber);
   
                //load SN
                ReloadSerialbyPO(POnumber);
                UpdateT_InformartionwPOqty(POnumber);
                MessageBox.Show("Hoàn tất update data SN!");
            }


        }

        public static void UpdateT_InformartionwPOqty(string strPONumber)
        {
            strPONumber = strPONumber.PadLeft(12, '0');
            //do check SN đúng theo PO
            SP_Processing.MySqlConn mycon = new SP_Processing.MySqlConn(strSqlConnection4_608FFCPACKING);
            // DataTable dt= mycon.ExecSProcDS("ame_CheckSNDLSS", POnumber.PadLeft(12, '0')).Tables["Table"];
            mycon.ExecSProc("[ame_T_Information_Update_POqty]", strPONumber.PadLeft(12, '0'));

        }


        public static void ReloadSerialbyPO(string strPONumber)
        {

            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList oNodes;
            XmlNode atestNode;
            long lRetCode = -1;
            string strPOMaterial;
            string strCurrentMaterial;

            //load to list form saplink

            SqlConnection sqlConnection4;

            
            try
            {
                sp = new SAPPost("ZRFC_SEND_POSERIALDATA_ACS");
                sp.setProperty("AUFNR", strPONumber.PadLeft(12,'0'));

                mySX = sp.Post(MainWindow.strSAPAddress);

                xmlDoc = mySX.getXDOC();

                DataTable myTable = mySX.getDataTable("ZSERIALNR_ACS");
                
                int iRows = myTable.Rows.Count;
                if (iRows > 0)
                {
                    

                    for (int i = 0; i < iRows; i++)
                    {
                        clsPOSerials aPOSerial = new clsPOSerials();
                        aPOSerial.strPONumber = strPONumber;
                        aPOSerial.strMaterial = myTable.Rows[i]["MATNR"].ToString().Trim();
                        aPOSerial.strSerial = myTable.Rows[i]["SERNR"].ToString().Trim();

                        
                        if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;
                        strCurrentMaterial = aPOSerial.strMaterial.Trim();

                        if (aPOSerial.strSerial.Trim() != "")
                        {
                            try
                            {
                                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                                {

                                    sqlConnection4.Open();
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = sqlConnection4;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "ame_T_addSerialToTFFC_serialnumbers";//"ame_T_addSerialToConsume";
                                    //cmd.Parameters.Add("@ProductMap", SqlDbType.Char, 30);
                                    //cmd.Parameters["@ProductMap"].Value = strProductMap;
                                    //cmd.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                                    cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                                    cmd.Parameters["@ProdOrder"].Value = aPOSerial.strPONumber;
                                    cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                                    cmd.Parameters.Add("@Material", SqlDbType.Char, 30);
                                    cmd.Parameters["@Material"].Value = aPOSerial.strMaterial;
                                    cmd.Parameters["@Material"].Direction = ParameterDirection.Input;
                                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                                    cmd.Parameters["@Serial"].Value = aPOSerial.strSerial;
                                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                                    SqlDataReader rec = cmd.ExecuteReader();
                                    sqlConnection4.Close();
                                }
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show("Error add  Serial into server:" + ex.Message);
                                //return "NG";
                            }
                        }
                    }
                }
                //return "OK";
            }
            catch (Exception ex)
            {
                if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                {
                    string strEnglishphrase = "";
                    string strForeignphrase = "";
                    MessageBox.Show("Error on RFC to get serial numbers (" + ex.Message);
                }
                else
                {
                    MessageBox.Show("Error on RFC to get serial numbers=" + ex.Message);
                }
            }

            //writ to tffc_serial
            
        }


        public static void UpdatePackedRecordbyPO(string POnumber)
        {

            ///count sn in tffc_serialnumber.
            
            ///count sn in packingdata.

        }
    }
}
