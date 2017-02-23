using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ACSEE.NET;
using System.Drawing;
//using System.Windows.Forms;
using System.Diagnostics;





using System.Xml;
using System.Data;
using System.Collections;

using System.Data.SqlClient;
using System.Configuration;
using clsPrintVariable;
using BartenderLibrary;

using System.IO;
using System.Drawing.Printing;
using Microsoft.VisualBasic;
using PrintPack;
using System.Deployment.Application;
                


namespace PrintPack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()

        {
           

             InitializeComponent();
             //ConditiontoPackingVerify objController = new ConditiontoPackingVerify();


             ///testonly
             //string testpo = "000100583348";
             //BusinessPackingRecord.DeleteSerialbyPO(testpo);
             //BusinessPackingRecord.ReloadSerialbyPO(testpo);
             ///testonly
            try
            {
                

                #region Khoi_tao_connection_btformatpath
                sqlConnection1 = new SqlConnection();
                strSqlConnection1 = ConfigurationManager.AppSettings.Get("ACSEECONNECTION").ToString();
                sqlConnection2 = new SqlConnection();
                strSqlConnection2 = ConfigurationManager.AppSettings.Get("ACSEECLIENTSTATECONNECTION").ToString();
                sqlConnection3 = new SqlConnection();
                strSqlConnection3 = ConfigurationManager.AppSettings.Get("ACSEESTATECONNECTION").ToString();
                sqlConnection4 = new SqlConnection();
                strSqlConnection4_608FFCPACKING = ConfigurationManager.AppSettings.Get("FFCPACKINGCONNECTION").ToString();

               strSAPAddress = ConfigurationManager.AppSettings.Get("SAPHOMELOCATION").ToString();

                strPrintSMTwACSOverPackContent = ConfigurationManager.AppSettings.Get("PrintSMTwACSOverPackContent").ToString();
                strPrintSMTwoACSOverPackContent = ConfigurationManager.AppSettings.Get("PrintSMTwoACSOverPackContent").ToString();
                strPrintBASEOverPack = ConfigurationManager.AppSettings.Get("PrintBASEOverPack").ToString();
                strPrintBASEOverPackContent = ConfigurationManager.AppSettings.Get("PrintBASEOverPackContent").ToString();
                strPrintDLMOverPack = ConfigurationManager.AppSettings.Get("PrintDLMOverPack").ToString();
                strPrintDLMOverPackVN = ConfigurationManager.AppSettings.Get("PrintDLMOverPackVN").ToString();
                strPrintDLMOverPackContent = ConfigurationManager.AppSettings.Get("PrintDLMOverPackContent").ToString();
                strPrintFRUwACSOverPackContent = ConfigurationManager.AppSettings.Get("PrintFRUwACSOverPackContent").ToString();
                strPrintFRUwoACSOverPackContent = ConfigurationManager.AppSettings.Get("PrintFRUwoACSOverPackContent").ToString();
                strPrintFFCOverPack = ConfigurationManager.AppSettings.Get("PrintFFCOverPack").ToString();
                strPrintFFCOverPackVN = ConfigurationManager.AppSettings.Get("PrintFFCOverPackVN").ToString();
                strPrintFFCOverPackContent = ConfigurationManager.AppSettings.Get("PrintFFCOverPackContent").ToString();
                strPrintFFCOverPackContentVN = ConfigurationManager.AppSettings.Get("PrintFFCOverPackContentVN").ToString();
                strPrintFFCCustomer = ConfigurationManager.AppSettings.Get("PrintFFCCustomer").ToString();
                strPrintFFCCustomerVN = ConfigurationManager.AppSettings.Get("PrintFFCCustomerVN").ToString();
                strPrintFFCHPCustomer = ConfigurationManager.AppSettings.Get("PrintFFCHPCustomer").ToString();
                bool IsPrintPackContinue = true;

                //add Dec_10

                //add Dec_10




                strDetailCustomerAddress = ConfigurationManager.AppSettings.Get("DetailCustomerAddress").ToString();
                strDetailStation = ConfigurationManager.AppSettings.Get("DetailStation").ToString();

                iMaxSerialsPerOrder = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsAtATime").ToString().Trim());
                iMaxSerialsFFC = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsFFC").ToString().Trim());
                iMaxSerialsDLM = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsDLM").ToString().Trim());
                iMaxSerialsBASE = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsBASE").ToString().Trim());
                
                #endregion
                PopulateInstalledPrintersCombo(); //add list printer default 

                btLabel = new BTLabel(strSqlConnection1,
                                        strSqlConnection2,
                                        strSqlConnection3);

                ClearSNofPOcheck4Base();
                //Version ver = null;
                
                //if (ApplicationDeployment.IsNetworkDeployed)
                //{
                //    ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;//new Version(Application.ProductVersion);
                //}
                //else
                //{
                //    //ver = new Version(Application.pro);
                //}

                

                //Version myVersion;


                //Text = string.Format("ClickOnce published Version: v{0}.{1}.{2}.{3}", myVersion.Major, myVersion.Minor, myVersion.Build, myVersion.Revision);
                Title = String.Format("ACS EE Print Pack Station 2015 | Version: " + "{0:#}.{1:#} | Released Date: 01 Oct 2016",  "3", "1");

                reports = new StationReporting();
                reports.DoUpdate();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            #region sp_ame_check_box_number
            if (!boxRework)
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    try
                    {
                        sqlConnection4.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = sqlConnection4;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ame_CheckBoxNumber";
                        SqlDataReader rec = cmd.ExecuteReader();
                        rec.Read();
                        string result = rec["Result"].ToString().Trim();
                        if (result.Equals("OK"))
                        {
                            rec.NextResult();
                            rec.Read();
                            pyear = rec["PYear"].ToString().Trim();
                        }
                        else
                        {
                            MessageBox.Show("Vượt quá 1000 thùng 1 ngày, liên hệ kỹ sư", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            this.Close();
                        }
                        sqlConnection4.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error Check Box Number:" + ex.Message);
                    }
                }
            } 
            #endregion
        }
        private PrintDocument printDoc = new PrintDocument();
        private void PopulateInstalledPrintersCombo()
        {
            //this.cboPrinterList.Dock = DockStyle.Top;
            //Controls.Add(this.cboPrinterList);

            // Add list of installed printers found to the combo box. 
            // The pkInstalledPrinters string will be used to provide the display string. 
            int i;
            string pkInstalledPrinters;

            for (i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                this.cboPrinterList.Items.Add(pkInstalledPrinters);
                if (printDoc.PrinterSettings.IsDefaultPrinter)
                {
                    this.cboPrinterList.Text = printDoc.PrinterSettings.PrinterName;
                }
            }
        }
        public string StationType = ConfigurationManager.AppSettings.Get("Stationtype").ToString();
        public string PackingType = ConfigurationManager.AppSettings.Get("PackingType").ToString();

        public Dictionary<string, clsPOSerials> dictPOInformation ;
        public string strCurrentMaterial ="";

        public string strSqlConnection1;
        public string strSqlConnection2;
        public string strSqlConnection3;
        public string strSqlConnection4_608FFCPACKING;

        public static string strSAPAddress;

        public System.Data.SqlClient.SqlConnection sqlConnection1;
        public System.Data.SqlClient.SqlConnection sqlConnection2;
        public System.Data.SqlClient.SqlConnection sqlConnection3;
        public System.Data.SqlClient.SqlConnection sqlConnection4;

        public int iMaxSerialsPerOrder=1000;
        public int iMaxSerialsFFC = 15;
        public int iMaxSerialsDLM = 15;//10
        public int iMaxSerialsBASE = 15;//20
        public Boolean bFromSalesOrder = false;

        public string strDetailCustomerAddress;
        public string strDetailStation;

        public string strSTATION;
        public string strREPRINT;
        public string strSHOWMODEL;
        public string strADDR01;
        public string strADDR02;
        public string strADDR03;
        public string strADDR04;
        public string strADDR05;
        public string strADDR06;
        public string strADDR07;
        public string strADDR08;
        public string strADDR09;

        public string strPrintOverPack;
        public string strPrintOverPackVN;
        public string strPrintOverPackContent;
        public string strPrintCustomer;

        public string strPrintSMTwACSOverPackContent;
        public string strPrintSMTwoACSOverPackContent;
        public string strPrintBASEOverPack;
        public string strPrintBASEOverPackContent;
        public string strPrintDLMOverPack;
        public string strPrintDLMOverPackVN;
        public string strPrintDLMOverPackContent;
        public string strPrintFRUwACSOverPackContent;
        public string strPrintFRUwoACSOverPackContent;
        public string strPrintFFCOverPack;
        public string strPrintFFCOverPackVN;
        public string strPrintFFCOverPackContent;
        public string strPrintFFCOverPackContentVN;
        public string strPrintFFCCustomer;
        public string strPrintFFCCustomerVN;
        public string strPrintFFCHPCustomer;
        public string strCOO;

        public string strPOMaterial;
        public string strPONumber;
        public string strPORev;
        public string strSTDRev;
        public string strPOMaterialbak;
        public string strPONumberbak;
        public string strPORevbak;
        public DateTime TestRequestDate;

        public int intPOQuantity;
        public int intPOPacked;
        public int intPORun;
        public clsPOSalesOrderInfo objSalesOrderInfo;

        public BTLabel btLabel;
        
        public Boolean boxRework = false;
        public string box = "NA";
        public string pyear = "NA";
        public Boolean dataExist = false;
        public string strBoxReworkresult = "Cancel";
        public string strProductMap = "NG";
        public Boolean bolHaveInfo = false;
        public SortedList<string, ucSerialNumber> slRePrint = new SortedList<string, ucSerialNumber>();
        public List<clsSerialInput> DSPackedSN_RePrint = new List<clsSerialInput>();

        public Boolean bolRePrint = false;

        public Boolean isModelSN = false;
        public Boolean isSNonly = false;
        public Boolean isModelEnterSN = false;
        public Boolean isModelSN2D = false;

        public string strModeChooseFromGroupbox = "";

        public bool IsHaveSNListofPO = false;

        StationReporting reports = new StationReporting();
        public void SetSNofPOcheck4Base()
        {
            IsHaveSNListofPO = true;
        }
        public void ClearSNofPOcheck4Base()
        {
            IsHaveSNListofPO = false;
        }

        /// <summary>
        /// Chọn mode nhập vào
        /// 1-Sn only
        /// 2-Model-SN
        /// 3-Model Enter Sn
        /// 
        /// </summary>
        public void GetInputSNmode()
        {
            strModeChooseFromGroupbox="2";//default model-sn
            if (radioButton_SNonly.IsChecked==true) strModeChooseFromGroupbox = "1";
            if (radioButton_ModelSN.IsChecked == true) strModeChooseFromGroupbox = "2";
            if(radioButton_ModelEnterSN.IsChecked == true) strModeChooseFromGroupbox = "3";
            if (radioButton_ModelSN2D.IsChecked == true) strModeChooseFromGroupbox = "4";
        }


        public List<clsSerialInput> SNs;
       // public List<clsPackingInformation> PackingInformation;
       public  clsPackingInformation aPK;

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           
             aPK = new clsPackingInformation();

            string result = "NG";

            #region CheckifProdOrdr is number
            if (!(string.IsNullOrEmpty(txtProdOrder.Text)))

            txtProdOrder.Text = txtProdOrder.Text.ToString().ToUpper().Trim().PadLeft(12, '0');

            string input = txtProdOrder.Text.Trim();
            bool isPOnumberinput = input.Length >5 && input.All(char.IsDigit);

            #endregion

         

            if (isPOnumberinput)
            {

                GetInputSNmode();
                result = CheckProdOrder_Setup(); // check tum lum

                            

                 if (result.Equals("OK"))
                {

                    RunSequence();
                }
            }
            else //leng textbox1 <5
            {

                MessageBox.Show("Nhap dung so Production Order", "Chu y !");
            }
        }

        public string PrintCustomerLabel(MainWindow myMain)
        {
            bool bIsHPModel = false;
            try
            {

                using (sqlConnection3 = new SqlConnection(strSqlConnection3))
                {
                    sqlConnection3.Open();
                    if (sqlConnection3.State.Equals(ConnectionState.Open))
                    {
                        try
                        {
                            SqlCommand cmdCheckForHPModel = sqlConnection3.CreateCommand();
                            cmdCheckForHPModel.CommandType = CommandType.StoredProcedure;
                            cmdCheckForHPModel.CommandText = "ame_HP_CheckForHPModel"; ;

                            cmdCheckForHPModel.Parameters.Add("@model", SqlDbType.Char, 20);
                            cmdCheckForHPModel.Parameters["@model"].Value = strCurrentMaterial.Trim() ;
                            cmdCheckForHPModel.Parameters["@model"].Direction = ParameterDirection.Input;

                            SqlDataReader rd = cmdCheckForHPModel.ExecuteReader();
                            if (rd.HasRows)
                            {
                                rd.Read();
                                string strIsHPModel = rd[0].ToString().Trim();
                                if ( strIsHPModel.Equals("HP"))
                                {
                                    bIsHPModel = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error checking for HP Model=" + ex.Message);
                        }
                    }
                }
                if (bIsHPModel == true)
                {
                    string strSOItem = myMain.objSalesOrderInfo.strSalesItem.Trim().PadLeft(6,'0') ;

                    myMain.btLabel.setPrintFile(1, myMain.strPrintFFCHPCustomer);
                    myMain.btLabel.findFieldandSubstitute(1, "SONUM", myMain.objSalesOrderInfo.strSalesOrder.Trim() + strSOItem);
                    myMain.btLabel.findFieldandSubstitute(1, "PONUM", myMain.objSalesOrderInfo.strCustomerPurchaseOrder.Trim());
                    myMain.btLabel.findFieldandSubstitute(1, "CUSTPART", myMain.objSalesOrderInfo.strCustomerMaterial.Trim());
                    myMain.btLabel.doPrint(1, false, false);
                }
                else
                {

                    //myMain.btLabel.setPrintFile(1, myMain.strPrintCustomer);
                    ///*
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR01", "Attn:" + myMain.objSalesOrderInfo.strAttn.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR02", myMain.objSalesOrderInfo.strCustName1.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR03", myMain.objSalesOrderInfo.strStreetAddress.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR04", myMain.objSalesOrderInfo.strCity.Trim() + "," + myMain.objSalesOrderInfo.strStateRegion.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR05", myMain.objSalesOrderInfo.strPostalCode.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR06", myMain.objSalesOrderInfo.strCountry.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "PONUM", myMain.objSalesOrderInfo.strCustomerPurchaseOrder.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "CUSTPART", myMain.objSalesOrderInfo.strCustomerMaterial.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "PRODORDER", myMain.strPONumber);
                    //*/

                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR01", strADDR01);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR02", strADDR02);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR03", strADDR03);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR04", strADDR04);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR05", strADDR05);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR06", strADDR06);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR07", strADDR07);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR08", strADDR08);
                    //myMain.btLabel.findFieldandSubstitute(1, "ADDR09", strADDR09);
                    //myMain.btLabel.findFieldandSubstitute(1, "PONUM", myMain.objSalesOrderInfo.strCustomerPurchaseOrder.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "CUSTPART", myMain.objSalesOrderInfo.strCustomerMaterial.Trim());
                    //myMain.btLabel.findFieldandSubstitute(1, "PRODORDER", myMain.objSalesOrderInfo.strSalesOrder.Trim().PadLeft(10,'0') + myMain.objSalesOrderInfo.strSalesItem.Trim().PadLeft(6,'0'));
                    if (strSHOWMODEL.Equals("YES")) myMain.btLabel.findFieldandSubstitute(1, "SPART", myMain.objSalesOrderInfo.strMaterial.Trim());
                    else if (strSHOWMODEL.Equals("NO")) myMain.btLabel.findFieldandSubstitute(1, "SPART", "");
                    myMain.btLabel.doPrint(1, false, false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error printing Customer Label=" + ex.Message);
            }
            return "OK";
        }

        public string PrintBoxLabel(MainWindow myMain, int iAmt)
        {
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList oNodes;
            //XmlNode aNode;
            string strDateTimeholder;
            string strModelDescription ="";
            string strModelCUSTOMERPN = "";

            try
            {
                if (getSAPDescription(myMain.strPOMaterial.Trim()).Equals("NG"))
                {
                    strDateTimeholder = DateTime.Today.Year.ToString();
                    strDateTimeholder += DateTime.Today.Month.ToString().PadLeft(2, '0');
                    strDateTimeholder += DateTime.Today.Day.ToString().PadLeft(2, '0');
                    sp = new SAPPost("Z_BAPI_BOM_PULL_LEVEL");
                    sp.setProperty("MATERIAL_NUMBER", myMain.strPOMaterial.Trim());
                    sp.setProperty("VALID_FROM", strDateTimeholder);
                    sp.setProperty("VALID_TO", strDateTimeholder);

                    mySX = sp.Post(strSAPAddress);
                    xmlDoc = mySX.getXDOC();
                    oNodes = xmlDoc.GetElementsByTagName("MAT_DESC");
                    strModelDescription = oNodes.Item(0).InnerText.Trim();
                    writeSAPDescription(myMain.strPOMaterial.Trim(), strModelDescription);
                }
                else
                {
                    PrintPack.SP_Processing.MySqlConn connect = new SP_Processing.MySqlConn(PhanMem.chuoi_ket_noivnmsrv601_FFCPacking);
                    
                    DataSet da =connect.ExecSProcDS("ame_getSAPDescription",myMain.strPOMaterial.Trim());

                    //strModelDescription = getSAPDescription(myMain.strPOMaterial.Trim());
                    strModelDescription = da.Tables[0].Rows[0][0].ToString();
                    strModelCUSTOMERPN = da.Tables[1].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Printing Box Label:" + ex.Message);
            }
          
            try
            {
              

                myMain.btLabel.setPrintFile(2, myMain.strPrintOverPackContent);
               

                myMain.btLabel.findFieldandSubstitute(2, "SPART", myMain.strPOMaterial.Trim());
                myMain.btLabel.findFieldandSubstitute(2, "DESC", strModelDescription);
                myMain.btLabel.findFieldandSubstitute(2, "PRODORDER", myMain.strPONumber.Trim());
                //if (strProductMap.Equals("BASE")) 
                myMain.btLabel.findFieldandSubstitute(2, "PARTREV", strPORev);
                myMain.btLabel.findFieldandSubstitute(2, "AMT", iAmt.ToString().Trim());
                //myMain.btLabel.findFieldandSubstitute(2,"ZDATE", DateTime.Now.ToString("d MMM yyyy").Trim());
                myMain.btLabel.findFieldandSubstitute(2,"ZDATE", getDateTime());
                myMain.btLabel.findFieldandSubstitute(2, "BOX", box);
                myMain.btLabel.findFieldandSubstitute(2, "MANPACK", strCOO);
                myMain.btLabel.findFieldandSubstitute(2, "CORIGIN", "");

                #region Add field for Customer Partnumber
                //BartenderBusiness LabelPrint = new BartenderBusiness();
                //LabelPrint.GanDuongDanBTlabel(myMain.strPrintOverPackContent);
                //List<string> DSSHARENAME = new List<string>();//
                //DSSHARENAME = LabelPrint.LayListSharename();




                //foreach (string i in DSSHARENAME)
                //{
                //    if (i.ToString().Trim() == "CSPNName")
                //    {
                if (string.IsNullOrEmpty(strModelCUSTOMERPN))
                {

                    myMain.btLabel.findFieldandSubstitute(2, "CSPNName", "");
                }
                else
                {
                    strModelCUSTOMERPN = strModelCUSTOMERPN.Trim();
                    myMain.btLabel.findFieldandSubstitute(2, "CSPNName", "Customer Part Number");
                }

                //}
                //if (i.ToString().Trim() == "CSPN")
                //{
                myMain.btLabel.findFieldandSubstitute(2, "CSPN", strModelCUSTOMERPN);
                //    }
                //}

                #endregion
         

                myMain.btLabel.doPrint(2, false, false);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Printing Box Label=" + ex.Message);
            }
            return "OK";
        }
        public string strModelDescription = "";
        

        public void getSAPDescription1(string SAPModel)
        {
#if DEBUG
            Console.WriteLine("Lay ten mo ta, Input SAP Model");
#endif
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList oNodes;
            //XmlNode aNode;
            string strDateTimeholder;
            strModelDescription = "";
           

            try
            {
                if (getSAPDescription(SAPModel).Equals("NG"))
                {
                    strDateTimeholder = DateTime.Today.Year.ToString();
                    strDateTimeholder += DateTime.Today.Month.ToString().PadLeft(2, '0');
                    strDateTimeholder += DateTime.Today.Day.ToString().PadLeft(2, '0');
                    sp = new SAPPost("Z_BAPI_BOM_PULL_LEVEL");
                    sp.setProperty("MATERIAL_NUMBER", SAPModel);
                    sp.setProperty("VALID_FROM", strDateTimeholder);
                    sp.setProperty("VALID_TO", strDateTimeholder);

                    mySX = sp.Post(strSAPAddress);
                    xmlDoc = mySX.getXDOC();
                    oNodes = xmlDoc.GetElementsByTagName("MAT_DESC");
                    strModelDescription = oNodes.Item(0).InnerText.Trim();
                    writeSAPDescription(SAPModel, strModelDescription);

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error get SAP Description 1:" + ex.Message);
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Init_VN();
            Init_USA();
        }
         
        private void Init_USA()   
        {
            try
            {
//                XmlDocument xmlDoc = new XmlDocument(); //* create an xml document object. 
//                xmlDoc.Load("defaultConfig.xml");

//                XmlNode xmlCustomerOnOff = xmlDoc.SelectSingleNode("ROOT/CUSTOMERLABELCONFIG");

                //XmlNodeList girlAddress = xmlDoc.GetElementsByTagName("gAddress"); 

                string strCustomerLabelDefault ="";
                string strBoxLabelDefault="";
                string strOverPackLabelDefault="";


                #region CHUYEN ANH VIET- KHONG DUNG
                //string strEnglishPhrase ="" ;
                //string strForeignPhrase ="" ;
                //if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                //{
                //    label1SK.Visibility = System.Windows.Visibility.Visible;
                //    if (strSTATION.Equals("PO"))
                //    {
                //        label1.Content = "Production Order:";
                //        label1SK.Content = "(" + getForeignPhrase("PRODORDER", ref strEnglishPhrase, ref strForeignPhrase) + ")";
                //    }
                //    else if (strSTATION.Equals("SAP"))
                //    {
                //        label1.Content = "SAP Model Name:";
                //        label1SK.Content = "(" + getForeignPhrase("MODEL", ref strEnglishPhrase, ref strForeignPhrase) + ")";
                //    }

                //    labelSKCustomer.Visibility = System.Windows.Visibility.Visible;
                //    labelSKCustomer.Content = "(" + getForeignPhrase("PRINTCUSTLABEL", ref strEnglishPhrase, ref strForeignPhrase) + ")";

                //    labelSKNumber.Visibility = System.Windows.Visibility.Visible;
                //    labelSKNumber.Content = "(" + getForeignPhrase("NUMBEROFLABELS", ref strEnglishPhrase, ref strForeignPhrase) + ")";

                //    labelSKIndividual.Visibility = System.Windows.Visibility.Visible;
                //    labelSKIndividual.Content = "(" + getForeignPhrase("PRINTBOXLABEL", ref strEnglishPhrase, ref strForeignPhrase) + ")";

                //    labelSKPrintOverpack.Visibility = System.Windows.Visibility.Visible;
                //    labelSKPrintOverpack.Content = "(" + getForeignPhrase("PRINTOVERPACK", ref strEnglishPhrase, ref strForeignPhrase) + ")";

                //    labelSKNumberPerOverPack.Visibility = System.Windows.Visibility.Visible;
                //    labelSKNumberPerOverPack.Content = "(" + getForeignPhrase("NUMBERPEROVERPACK", ref strEnglishPhrase, ref strForeignPhrase) + ")";

                //    button1.Content += "  (" + getForeignPhrase("PROCEED", ref strEnglishPhrase, ref strForeignPhrase) + ")";

                //    button2.Content += "  (" + getForeignPhrase("EXIT", ref strEnglishPhrase, ref strForeignPhrase) + ")";

                //}

                strCustomerLabelDefault = ConfigurationManager.AppSettings.Get("CUSTOMERLABELSELECTED").ToString();
                strBoxLabelDefault = ConfigurationManager.AppSettings.Get("BOXLABELSELECTED").ToString();
                strOverPackLabelDefault = ConfigurationManager.AppSettings.Get("OVERPACKLABELSELECTED").ToString();
                
                #endregion
                if (strCustomerLabelDefault.Equals("ON"))
                {
                    checkBox_PrintCusLabel.IsChecked = true;
                }
                else
                {
                    checkBox_PrintCusLabel.IsChecked = false;
                }

                if (strBoxLabelDefault.Equals("ON"))
                {
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                }
                else
                {
                    checkBox_PrintIndividualBoxLabels.IsChecked = false;
                }

                if (strOverPackLabelDefault.Equals("ON"))
                {
                    checkBox_PrintOverPackLabel.IsChecked = true;
                }
                else
                {
                    checkBox_PrintOverPackLabel.IsChecked = false;
                }
                for (int i = 0; i < 50; i++)
                {
                    comboBox2.Items.Add(i);
                }

                comboBox2.Text = "1";

                for (int i = 0; i < 21; i++)
                {
                    comboBox1.Items.Add(i);
                }
                comboBox1.Text = iMaxSerialsFFC.ToString().Trim() ;
                txtProdOrder.Focus();
                txtProdOrder.SelectAll();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Loaded error=" + ex.Message);
            }
        }

        private void Init_VN()
        {
            string line;

            #region Khởi tạo COO for OverpackConten

            cboCOO.Items.Add("Manufactured in Vietnam");
            cboCOO.Items.Add("Packaging in Vietnam");
            
            #endregion

            StreamReader TextFile = new StreamReader(strDetailStation);



            while ((line = TextFile.ReadLine()) != null)
            {
                if (line.Trim().Equals("")) continue;
                if (line.Trim().Substring(0, 1).Equals(";")) continue;
                string text=line.Trim();
                int pos = text.IndexOf("=",0);

                StationType= StationType.Trim();
                //switch (text.Substring(0,pos))
                switch (StationType)
                {
                    case "STATION":
                        strSTATION = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "REPRINT":
                        strREPRINT = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                }
#if DEBUG
                Console.WriteLine("StationType: "+ StationType);
#endif
            }

            PackingType = PackingType.Trim();

            switch (PackingType)//switch (strSTATION)
            {
                case "WAREHOUSE":
                    this.Hide();
                    WareHouse wareHouse = new WareHouse();
                    wareHouse.ShowDialog();
                    this.Close();
                    break;
                case "PO":
                    lblRevision.Visibility = Visibility.Hidden;
                    txtREVision.Visibility=Visibility.Hidden;
                    txtREVision.Text="";
                    if (StationType == "REPRINT")
                        ///*if (strREPRINT.Equals("TRUE"))*/ button4.Visibility = Visibility.Visible;
                        //else button4.Visibility = Visibility.Hidden;
                        button4.Visibility = Visibility.Visible;
                    break;
            }

#if DEBUG
            Console.WriteLine("PackingType: " + PackingType);
#endif

            TextFile = new StreamReader(strDetailCustomerAddress);
            while ((line = TextFile.ReadLine()) != null)
            {
                if (line.Trim().Equals("")) continue;
                if (line.Trim().Substring(0, 1).Equals(";")) continue;
                string text = line.Trim();
                int pos = text.IndexOf("=", 0);
                switch (text.Substring(0, pos))
                {
                    case "SHOWMODEL":
                        strSHOWMODEL = text.Substring(pos + 1,text.Length - pos - 2);
                        break;
                    case "ADDR01":
                        strADDR01 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR02":
                        strADDR02 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR03":
                        strADDR03 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR04":
                        strADDR04 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR05":
                        strADDR05 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR06":
                        strADDR06 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR07":
                        strADDR07 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR08":
                        strADDR08 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                    case "ADDR09":
                        strADDR09 = text.Substring(pos + 1, text.Length - pos - 2);
                        break;
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void checkBox1_Click(object sender, RoutedEventArgs e)
        {
            bool iSchecked = checkBox_PrintCusLabel.IsChecked.Value;
        }

        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            comboBox2.IsEnabled = true;
        }

        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBox2.IsEnabled = false;
        }

        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {
            comboBox1.IsEnabled = true;
        }

        private void checkBox3_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBox1.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            btLabel.quitBTApp();
            reports.DoCloseStation();
        }

        /*private void button3_Click(object sender, RoutedEventArgs e)
        {
            string strDate = DateTime.Now.ToString("d MMM yyyy");
            MessageBox.Show(strDate);
        }*/

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        
    {
            if (e.Key == Key.Enter)
            {

                RoutedEventArgs newEventArgs = new RoutedEventArgs(Button.ClickEvent);
                button1.RaiseEvent(newEventArgs);
            }
        }

        private string getPOInformation()
        {
#if DEBUG
            Console.WriteLine("Lay thong tin PO");
#endif
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            //XmlNodeList oNodes;
            XmlNode atestNode;
            long lRetCode = -1;
            try
            {
                //Get PO SalesOrder Info
                sp = new SAPPost("ZRFC_SEND_PODATA_ACS");
                sp.setProperty("AUFNR", strPONumber);

                mySX = sp.Post(strSAPAddress);

                xmlDoc = mySX.getXDOC();

                atestNode = xmlDoc.GetElementsByTagName("RETURN_CODE").Item(0);
                if (atestNode != null)
                {
                    if (atestNode.InnerText.ToString().Trim().Length > 0)
                    {
                        lRetCode = Int32.Parse(atestNode.InnerText.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on getting PO BOM=" + ex.Message);
                return "NG";
            }

            try
            {
                if (lRetCode == 0)
                {
                    objSalesOrderInfo = new clsPOSalesOrderInfo();
                    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strSalesOrder = atestNode.InnerText.ToString().Trim();
                    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER_ITEM").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strSalesItem = atestNode.InnerText.ToString().Trim();
                    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_MATERIAL").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strCustomerMaterial = atestNode.InnerText.ToString().Trim();
                    atestNode = xmlDoc.GetElementsByTagName("MATERIAL").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0) objSalesOrderInfo.strMaterial = atestNode.InnerText.ToString().Trim();
                        else objSalesOrderInfo.strMaterial = "";
                    }
                    else objSalesOrderInfo.strMaterial = "";
                    atestNode = xmlDoc.GetElementsByTagName("DESCRIPTION").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0) objSalesOrderInfo.strDescription = atestNode.InnerText.ToString().Trim();
                        else objSalesOrderInfo.strDescription = "";
                    }
                    else objSalesOrderInfo.strDescription = "";
                    atestNode = xmlDoc.GetElementsByTagName("GRAVITY_ZONE").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0) objSalesOrderInfo.lGravityZone = Int32.Parse(atestNode.InnerText.ToString());
                        else objSalesOrderInfo.lGravityZone = -1;
                    }
                    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_PO").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strCustomerPurchaseOrder = atestNode.InnerText.ToString().Trim();
                    atestNode = xmlDoc.GetElementsByTagName("ATTN").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strAttn = atestNode.InnerText.ToString();
                    else objSalesOrderInfo.strAttn = "";

                    // new fields start here
                    atestNode = xmlDoc.GetElementsByTagName("CUSTNAME1").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strCustName1 = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strCustName1 = "";
                    atestNode = xmlDoc.GetElementsByTagName("STREETADDRESS").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strStreetAddress = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strStreetAddress = "";
                    atestNode = xmlDoc.GetElementsByTagName("CITY").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strCity = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strCity = "";
                    atestNode = xmlDoc.GetElementsByTagName("REGION").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strStateRegion = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strStateRegion = "";
                    atestNode = xmlDoc.GetElementsByTagName("POSTALCODE").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strPostalCode = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strPostalCode = "";
                    atestNode = xmlDoc.GetElementsByTagName("DESTINATIONCODE").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strDestinationCode = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strDestinationCode = "";
                    atestNode = xmlDoc.GetElementsByTagName("OTDDATE").Item(0);
                    if (atestNode != null) objSalesOrderInfo.dtOTDDate = DateTime.Parse(atestNode.InnerText.ToString());
                    else objSalesOrderInfo.dtOTDDate = DateTime.Parse("");
                    atestNode = xmlDoc.GetElementsByTagName("QTY").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0) objSalesOrderInfo.lQty = Int32.Parse(atestNode.InnerText.ToString());
                    }
                    else objSalesOrderInfo.lQty = 100000;
                    atestNode = xmlDoc.GetElementsByTagName("VENDOR").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strVendor = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strVendor = "";
                    atestNode = xmlDoc.GetElementsByTagName("COUNTRY").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strCountry = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strCountry = "";
                    atestNode = xmlDoc.GetElementsByTagName("HIERARCHY").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strHierarchy = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strHierarchy = "";
                    atestNode = xmlDoc.GetElementsByTagName("BLOCKINGCODE").Item(0);
                    if (atestNode != null) objSalesOrderInfo.strBlockingCode = atestNode.InnerText.ToString().Trim();
                    else objSalesOrderInfo.strBlockingCode = "";
                    if (objSalesOrderInfo.strSalesOrder.Trim().Length > 0 || objSalesOrderInfo.strCustName1.Trim().Length > 0)
                    {
                        bFromSalesOrder = true;
                        this.checkBox_PrintCusLabel.IsChecked = true;
                    }
                    else
                    {
                        bFromSalesOrder = false;
                        this.checkBox_PrintCusLabel.IsChecked = false;
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
                MessageBox.Show("Error on getting PO BOM fields=" + ex.Message);
                return "NG";
            }
            return "OK";
        }

        private string getPOInformationBASE()
        {
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            //XmlNodeList oNodes;
            //XmlNode atestNode;
            //long lRetCode = -1;
            //int intPOQuantity1;

            try
            {
                //Get PO SalesOrder Info
                sp = new SAPPost("ZRFC_SEND_PODATA_ACS");
                sp.setProperty("AUFNR", strPONumber);

                mySX = sp.Post(strSAPAddress);

                xmlDoc = mySX.getXDOC();

                DataTable myTable = mySX.getDataTable("PODATA_ACS");
                int iRows = myTable.Rows.Count;
                if (iRows > 0)
                {
                    strPOMaterial = myTable.Rows[0]["MATNR"].ToString().Trim();
                    intPOQuantity = Int32.Parse(myTable.Rows[0]["MENGE"].ToString().Trim());
                    /*
                    for (int i = 0; i < iRows; i++)
                    {
                        try
                        {
                            intPOQuantity1 = Convert.ToInt32(myTable.Rows[i]["MENGE"].ToString().Trim());
                            if (intPOQuantity == 0 || intPOQuantity > intPOQuantity1) intPOQuantity = intPOQuantity1;
                        }
                        catch (FormatException)
                        {
                            continue;
                        }
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on getting PO BOM=" + ex.Message);
                return "NG";
            }
            return "OK";
        }

        private string getSAPModel(string strPONumber_input)
        {
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            //XmlNodeList oNodes;
            XmlNode atestNode;
            long lRetCode = -1;
            string strMaterial="";
            
            try
            {
                //Get PO SalesOrder Info
                sp = new SAPPost("ZRFC_SEND_PODATA_ACS");
                sp.setProperty("AUFNR", strPONumber_input);

                mySX = sp.Post(strSAPAddress);

                xmlDoc = mySX.getXDOC();

                atestNode = xmlDoc.GetElementsByTagName("RETURN_CODE").Item(0);

                if (atestNode != null)
                {
                    if (atestNode.InnerText.ToString().Trim().Length > 0)
                    {
                        lRetCode = Int32.Parse(atestNode.InnerText.ToString());
#if DEBUG
                        Console.WriteLine("Model la: "+ lRetCode);
#endif
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on getting SAP Model=" + ex.Message);
            }

            try
            {
                if (lRetCode == 0)
                {
                    atestNode = xmlDoc.GetElementsByTagName("MATERIAL").Item(0);
                    if (atestNode != null)
                    {
                        if (atestNode.InnerText.ToString().Trim().Length > 0) strMaterial = atestNode.InnerText.ToString().Trim();
                        else strMaterial = "";
                    }
                    else strMaterial = "";
                } //  if (lRetCode == 0)
                else
                {
                    strMaterial = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on getting PO BOM fields=" + ex.Message);
            }
            return strMaterial;
        }

        private string getProductMap(string SAPModel)
        {
            string result="NG";
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getProductMap";
                    cmd.Parameters.Add("@SAPModel", SqlDbType.Char, 30);
                    cmd.Parameters["@SAPModel"].Value = SAPModel;
                    cmd.Parameters["@SAPModel"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    result = rec["Result"].ToString().Trim();
                    sqlConnection4.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error get Product Map:" + ex.Message);
            }
            return result;
        }

        private string getProductMapDetail (string SAPModel, ref string strDes)
        {
            string result = "NG";
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getproductmapdetail";
                    cmd.Parameters.Add("@SAPModel", SqlDbType.Char, 30);
                    cmd.Parameters["@SAPModel"].Value = SAPModel;
                    cmd.Parameters["@SAPModel"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    result = rec["Result"].ToString().Trim();
                    strDes = rec["Description"].ToString().Trim();
                    sqlConnection4.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error get Product Map:" + ex.Message);
            }
            return result;
        }

        private string getSAPDescription(string SAPModel)
        {
            #region Lấy Sap description từ SAP model_update ProductMap
            string result = "NG";
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getSAPDescription";
                    cmd.Parameters.Add("@SAPModel", SqlDbType.Char, 30);
                    cmd.Parameters["@SAPModel"].Value = SAPModel;
                    cmd.Parameters["@SAPModel"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    result = rec["Result"].ToString().Trim();
                    sqlConnection4.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error get SAP Description:" + ex.Message);
            }
            return result; 
            #endregion
        }

        private void writeSAPDescription(string SAPModel, string Description)
        {
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_writeSAPDescription";
                    cmd.Parameters.Add("@SAPModel", SqlDbType.Char, 30);
                    cmd.Parameters["@SAPModel"].Value = SAPModel;
                    cmd.Parameters["@SAPModel"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Description", SqlDbType.Char, 200);
                    cmd.Parameters["@Description"].Value = Description;
                    cmd.Parameters["@Description"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    sqlConnection4.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error write SAP Description:" + ex.Message);
            }
        }

        private void Insert_ProductMap(string SAPModel,string ProductMap, string Description, string ProductLine, string MPN)
        {
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    


                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_Insert_ProductMap";

                    cmd.Parameters.Add("@SAPModel", SqlDbType.Char, 30);
                    cmd.Parameters["@SAPModel"].Value = SAPModel;
                    cmd.Parameters["@SAPModel"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("@ProductMap", SqlDbType.Char, 30);
                    cmd.Parameters["@ProductMap"].Value = ProductMap;
                    cmd.Parameters["@ProductMap"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("@Description", SqlDbType.Char, 200);
                    cmd.Parameters["@Description"].Value = Description;
                    cmd.Parameters["@Description"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("@ProductLine", SqlDbType.Char, 30);
                    cmd.Parameters["@ProductLine"].Value = ProductLine;
                    cmd.Parameters["@ProductLine"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("@MPN", SqlDbType.Char, 20);
                    cmd.Parameters["@MPN"].Value = MPN;
                    cmd.Parameters["@MPN"].Direction = ParameterDirection.Input;

                    SqlDataReader rec = cmd.ExecuteReader();
                    sqlConnection4.Close();

#if DEBUG
                    Console.WriteLine("Insert Done " + SAPModel);
#endif
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error write Product Map:" + ex.Message);
            }
        }

        public void getPrintFilesLabel(string strProductGroup)
        {
            switch (strProductGroup)
            {
                
                #region caseFFC
                case "FFC":
                    strPrintCustomer = strPrintFFCCustomerVN;
                    strPrintOverPackContent = strPrintFFCOverPackContentVN;
                    strPrintOverPack=strPrintFFCOverPack;

                    checkBox_PrintCusLabel.IsChecked = true;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = true;
                    break;
                #endregion
                #region caseDLM
                case "DLM":
                    goto case "FFC";
                    strPrintCustomer = strPrintFFCCustomerVN;
                    strPrintOverPackContent = strPrintFFCOverPackContentVN;
                    strPrintOverPack=strPrintFFCOverPack;
                    checkBox_PrintCusLabel.IsChecked = true;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = true;
                    //strPrintOverPackContent = strPrintDLMOverPackContent;
                    //strPrintOverPack=strPrintDLMOverPack;
                    //strPrintOverPackVN = strPrintDLMOverPackVN;
                    //checkBox_PrintCusLabel.IsChecked = false;
                    //checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    //checkBox_PrintOverPackLabel.IsChecked = true;
                    break;
                #endregion
                #region caseBASE
                case "BASE":
                    goto case "FFC";
                    strPrintCustomer = strPrintFFCCustomerVN;
                    strPrintOverPackContent = strPrintFFCOverPackContentVN;
                    strPrintOverPack=strPrintFFCOverPack;
                    checkBox_PrintCusLabel.IsChecked = true;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = true;
                    //strPrintOverPackContent = strPrintBASEOverPackContent;
                    //strPrintOverPack = strPrintBASEOverPack;
                    //checkBox_PrintCusLabel.IsChecked = false;
                    //checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    //checkBox_PrintOverPackLabel.IsChecked = true;
                    break;
                #endregion
                #region caseFRUwACS
                case "FRUwACS":
                    strPrintOverPackContent = strPrintFRUwACSOverPackContent;
                    checkBox_PrintCusLabel.IsChecked = false;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = false;
                    break;
                #endregion
                #region caseFRUwoACS
                case "FRUwoACS":
                    strPrintOverPackContent = strPrintFRUwoACSOverPackContent;
                    checkBox_PrintCusLabel.IsChecked = false;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = true;
                    break;
                #endregion
            
                #region caseSMTwACS
                case "SMTwACS":
                    strPrintOverPackContent = strPrintSMTwACSOverPackContent;
                    checkBox_PrintCusLabel.IsChecked = false;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = false;
                    break;
                #endregion
                #region caseSMTwoACS
                case "SMTwoACS":
                    strPrintOverPackContent = strPrintSMTwoACSOverPackContent;
                    checkBox_PrintCusLabel.IsChecked = false;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = false;
                    break;
                #endregion
            }
        }
        
        /// <summary>
        /// gán đường dẫn cho folder FFC
        /// Plan to change fixed placed 15 Oct
        /// </summary>
        public void getPrintFilesLabel()
        {
            
            //do check file exist and copy

            //do get file
                    strPrintCustomer = strPrintFFCCustomerVN;
                    strPrintOverPackContent = strPrintFFCOverPackContent;
                    strPrintOverPack = strPrintFFCOverPack;
            //default 
                    checkBox_PrintCusLabel.IsChecked = true;
                    checkBox_PrintIndividualBoxLabels.IsChecked = true;
                    checkBox_PrintOverPackLabel.IsChecked = true;
                   

        }

        public void getPOInfo(string PONumber)
        {
            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();
                SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderInfo"; ;
                cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = PONumber;
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                SqlDataReader rec = cmdGetProdOrderSerials.ExecuteReader();
                rec.Read();
                if (rec[0].ToString().Equals("OK"))
                {
                    rec.NextResult();
                    rec.Read();
                    strPONumber = rec["T_ProdOrder"].ToString().Trim();
                    strProductMap = rec["T_ProductMap"].ToString().Trim();
                    strPOMaterial = rec["T_Material"].ToString().Trim();
                    strPORev = rec["T_Revision"].ToString().Trim();
                    intPOQuantity = Int32.Parse(rec["T_Quantity"].ToString().Trim());
                    intPOPacked = Int32.Parse(rec["T_Packed"].ToString().Trim());
                    strCurrentMaterial = strPOMaterial;
                    sqlConnection4.Close();
                }
            }
        }

        private string getDateTime()
        {
            string result = "NG";
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getDateTime";
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    result = rec["Result"].ToString().Trim();
                    sqlConnection4.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error get Date Time:" + ex.Message);
            }
            return result;
        }
        private static void TraceStepDoing(string strTrace)
        {
#if DEBUG

            Console.WriteLine(strTrace);
#endif
        }
        private string CheckProdOrder_Setup()
        {

            string result="NG";
            string result1="NG";

            strPONumber = txtProdOrder.Text.ToString().Trim().PadLeft(12, '0');



            #region doc_thong_tinPO_trentable_T_information_via_ame_T_getProdOrderInfo
            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();

                #region Kiem tra Prod Order trong T_information
                result = "Kiem tra ProdOrder trong T_information";

                SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderInfo"; ;
                cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = strPONumber;
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                SqlDataReader rec = cmdGetProdOrderSerials.ExecuteReader();
                rec.Read();

                result = rec[0].ToString(); 
                #endregion


                if (result.Equals("OK"))
                {

                    TraceStepDoing("Doc thong tin Tinformation ");

                    bolHaveInfo = true;
                    rec.NextResult();
                    rec.Read();
                    strPONumber = rec["T_ProdOrder"].ToString().Trim();
                    strProductMap = rec["T_ProductMap"].ToString().Trim();
                    strPOMaterial = rec["T_Material"].ToString().Trim();
                    strPORev = rec["T_Revision"].ToString().Trim();
                    txtREVision.Text = strPORev;
                    intPOQuantity = Int32.Parse(rec["T_Quantity"].ToString().Trim());
                    intPOPacked = Int32.Parse(rec["T_Packed"].ToString().Trim());
                    DateTime.TryParse(rec["TestRequireDate"].ToString(), out TestRequestDate);
                    

                    // lam tiep neu no la 1/1/2001 no check date


                    sqlConnection4.Close();

                    //getpacking information in detail
                    FFCPackingDataSet.GetPackingInformationDataTable dt = new FFCPackingDataSet.GetPackingInformationDataTable();
                    FFCPackingDataSetTableAdapters.GetPackingInformationTableAdapter da = new FFCPackingDataSetTableAdapters.GetPackingInformationTableAdapter();

                    dt = da.GetData(strPONumber);
                    int count = dt.Count();
                    if (count == 1)
                    {
                        
                        
                        foreach (DataRow dr in dt.Rows)
                        {
                            aPK.POnumber= dr["T_ProdOrder"].ToString().Trim();
                            aPK.POMaterial = dr["SAPModel"].ToString().Trim();
                            aPK.PODesc = dr["Description"].ToString().Trim();
                            aPK.Rev = dr["T_Revision"].ToString().Trim();
                           
                        }
                    }
    
                    //getpacking information in detail

                    if (intPOQuantity == 0)
                    {
                        MessageBox.Show("So luong PO=0. Lỗi: cần kiểm tra số lượng PO hoặc setup"," Liên hệ kỹ sư kiem tra");
                        txtProdOrder.Focus();
                        txtProdOrder.SelectAll();
                        //Get_Model_Des_Rev_fromPO();
                        //pull SN tu sap
                        //string strQtytemp="0";
                        //while (strQtytemp == "0")
                        //{
                        //    strQtytemp = Interaction.InputBox("So luong PO=0. Lỗi: cần kiểm tra số lượng PO hoặc nhap so luong: ", "Xac nhan So luong PO", strQtytemp, 10, 10);
                        //}

                        //T_Information_Update_POqty(Convert.ToInt16(strQtytemp));
                        return "NG";
                    }

                    if (intPOQuantity <= intPOPacked)
                    {
                        MessageBox.Show("PO Complete ! Đơn hàng đã hoàn tất! " + intPOPacked.ToString(), " Liên hệ kỹ sư kiem tra");
                        txtProdOrder.Focus();
                        txtProdOrder.SelectAll();
                        //Get_Model_Des_Rev_fromPO();
                        //pull SN tu sap
                        //string strQtytemp="0";
                        //while (strQtytemp == "0")
                        //{
                        //    strQtytemp = Interaction.InputBox("So luong PO=0. Lỗi: cần kiểm tra số lượng PO hoặc nhap so luong: ", "Xac nhan So luong PO", strQtytemp, 10, 10);
                        //}

                        //T_Information_Update_POqty(Convert.ToInt16(strQtytemp));
                        return "NG";
                    }
                }
                else
                {

                    TraceStepDoing("Chua co thong tin tren T Information ");
                    bolHaveInfo = false;

                    Get_Model_Des_Rev_fromPO();
                    
                    string strPOREVtemp = "";
                    while (strPOREVtemp == "")
                    {
                        strPOREVtemp = Interaction.InputBox("Model tren SAP dang co Rev: ", "Nhap Revision", strSTDRev, 10, 10);
                    }
                    strPORev = strPOREVtemp.ToUpper();

                    GetListSN(strPONumber);//cho FFC
                    //backup plan -> khi lỗi số SN không thuộc PO, thực thiện reload số SN trong PO

                    int POqty = dictPOInformation.Count();
                    bool IsConfirm = false;

                    if (POqty > 1)
                    { 
                        //no need to confirm
                        IsConfirm = true;
                    }


                    if ((IsConfirm == false) || (POqty==0))
                    {
                        POqty =Convert.ToInt16(Interaction.InputBox("Vui long nhap so luong PO: ", "Nhap so luong PO", "0", 10, 10));
                        IsConfirm = true;
                    }
                    intPOQuantity = POqty;

                    
                    strProductMap = getProductMap(strPOMaterial.Trim()).ToUpper();
                    TraceStepDoing("Get Productmap" + strProductMap);
                   

                    #region SetInputForm

                    if (strProductMap.Equals("BASE")) radioButton_SNonly.IsChecked = true;
                        

                    #endregion

                    Add_T_Information_Order();


                }


                #region  Check và InsertProductMap
                if (true)
                {

                    if (strProductMap.Equals("NG") || strProductMap.Equals(""))
                    {
                    loopproductmap:
                        strProductMap = Interaction.InputBox("Vui long nhap loai san pham FFC, DLM, BASE", "New model? Nhập ProductMap cho lần setup đầu tiên", "", 10, 10);
                         SP_Processing.MySqlConn cn = new SP_Processing.MySqlConn(PhanMem.chuoi_ket_noivnmsrv601_FFCPacking);
                        string isrightProduct = cn.ExecSProcDS("ame_Get_ProductVerify",strProductMap).Tables[0].Rows[0][0].ToString();
                        if (isrightProduct=="OK")
                        {
                            Get_Model_Des_Rev_fromPO();
                            Insert_ProductMap(strPOMaterial, strProductMap, strModelDescription, "", "");
                            FFCPackingDataSetTableAdapters.GetPackingInformationTableAdapter da = new FFCPackingDataSetTableAdapters.GetPackingInformationTableAdapter();
                            da.UpdateQueryProductMapbyPO(strProductMap, strPONumber);
                        }
                        else
                        {
                            MessageBox.Show("Nhập đúng thông tin yêu cầu,Plz");
                            goto loopproductmap;
                        }

                        // writeSAPDescription(strPOMaterial, strModelDescription);

                    }

                } 
                #endregion
            } 
            #endregion

            #region Gan bt duong dan va load SN
            try
            {
                TraceStepDoing("Lay duong dan nhan");//consider lay nhan khi in?
                //getPrintFilesLabel(strProductMap);
                getPrintFilesLabel(); 
                

                #region 303_Check PO SN_Pull vao he thong

                TraceStepDoing("Lay SN vao he thong");
                if (strProductMap.Equals("FFC") || strProductMap.Equals("DLM")||strProductMap.Equals("BASE"))
                {
                //GetListSN(strPONumber);
                GetListSNwhenTinformationComplete(strPONumber);
                }
                #endregion
//                #region khong dung

//                switch (strProductMap)
//                {
//                    #region caseFFC
//                    case "FFC":


//                        using (sqlConnection1 = new SqlConnection(strSqlConnection1))
//                        {
//#if DEBUG
//                            Console.Write("Lay so SN trong tffc_serial");
//#endif
//                            sqlConnection1.Open();
//                            SqlCommand cmdGetProdOrderSerials = sqlConnection1.CreateCommand();
//                            cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
//                            cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderSerials_byProd"; ;
//                            cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
//                            cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = strPONumber;
//                            cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
//                            SqlDataReader mySerials = cmdGetProdOrderSerials.ExecuteReader();
//                            mySerials.Read();
//                            if (mySerials[0].ToString().Equals("OK"))
//                            {
//#if DEBUG
//                                Console.Write("Lay so SN trong tffc_serial-Loading only");
//#endif
//                                mySerials.NextResult();
//                                dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
//                                while (mySerials.Read())
//                                {
//                                    clsPOSerials aPOSerial = new clsPOSerials();
//                                    aPOSerial.strPONumber = strPONumber;
//                                    aPOSerial.strSerial = mySerials["TFFC_SerialNumber"].ToString().Trim();
//                                    aPOSerial.strMaterial = mySerials["TFFC_Material"].ToString().Trim();
//                                    strCurrentMaterial = aPOSerial.strMaterial.Trim();
//                                    dictPOInformation.Add(aPOSerial.strSerial.Trim(), aPOSerial);
//                                    strPOMaterial = aPOSerial.strMaterial;
//                                }
//                            }
//                            else
//                            {
//#if DEBUG
//                                Console.Write("Lay so SN trong tffc_serial-Load from SAP");
//#endif
//                                result1 = GetListSN(strPONumber);
//                                if (result1.Equals("NG"))
//                                {
//                                    MessageBox.Show("Error getting PO Serial");
//                                    return "NG";
//                                }
//                            }
//                            result = getPOInformation();

//                            if (result.Equals("OK"))
//                            {
//                                strPONumber = strPONumber.Trim();
//                                strPOMaterial = strPOMaterial.Trim();
//                                if (!bolHaveInfo) Add_T_Information_Order();
//                            }
//                            else
//                            {
//                                MessageBox.Show("Error getting PO BOM");
//                                return "NG";
//                            }
//                        }
//                        break;
//                    #endregion
//                    #region caseDLM
//                    case "DLM":
//                        result1 = RunSubMain();
//                        if (result1.Equals("OK"))
//                        {
//                            strPONumber = strPONumber.Trim();
//                            strPOMaterial = strPOMaterial.Trim();
//                            if (!bolHaveInfo) Add_T_Information_Order();
//                        }
//                        else
//                        {
//                            MessageBox.Show("Error getting PO BOM");
//                            return "NG";
//                        }
//                        break;
//                    #endregion
//                    #region caseFRUwACS
//                    case "FRUwACS":
//                        break;
//                    #endregion
//                    #region caseFRUwoACS
//                    case "FRUwoACS":
//                        strPONumber = strPONumber.Trim();
//                        if (strPOMaterial != "" && strPOMaterial != null) strPOMaterial = strPOMaterial.Trim();
//                        if (!bolHaveInfo)
//                        {
//                            if (strPOMaterial == "" || strPOMaterial == null)
//                            {
//                                getPOInformationBASE();
//                            }
//                            Add_T_Information_Order();
//                        }
//                        break;
//                    #endregion
//                    #region caseBASE
//                    case "BASE":
//                        result1 = RunSubMain();
//                        if (result1.Equals("OK"))
//                        {
//                            strPONumber = strPONumber.Trim();
//                            if (strPOMaterial != "" && strPOMaterial != null) strPOMaterial = strPOMaterial.Trim();
//                            if (!bolHaveInfo)
//                            {
//                                if (strPOMaterial == "" || strPOMaterial == null)
//                                {
//                                    getPOInformationBASE();
//                                }
//                                Add_T_Information_Order();
//                            }
//                        }
//                        else
//                        {
//                            MessageBox.Show("Error getting PO BOM");
//                            return "NG";
//                        }
//                        break;
//                    #endregion
//                    #region caseSMTwACS
//                    case "SMTwACS":
//                        break;
//                    #endregion
//                    #region caseSMTwoACS
//                    case "SMTwoACS":
//                        break;
//                    #endregion
//                }
//                #endregion
                
            }
            catch (Exception ex)
            {
                if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                {
                    string strEnglishphrase = "";
                    string strForeignphrase = "";
                    MessageBox.Show("Error getting serial numbers(" + getForeignPhrase("ERRORGETSERIAL", ref strEnglishphrase, ref strForeignphrase) + ")-" + ex.Message);
                    return "NG";
                }
                else
                {
                    MessageBox.Show("Error getting serial numbers-" + ex.Message);
                    return "NG";
                }
            }
            return "OK"; 
            #endregion
        }

        /// <summary>
        /// Input PO, Out lấy Model, Description, rev
        /// </summary>
        private void Get_Model_Des_Rev_fromPO()
        {
            string Model = "";
            getModelfromPOno(strPONumber, out Model);
            strPOMaterial = Model.Trim();
            //Model
            string des = "";
            string rev = "";
            PullModelDescription2string(strPOMaterial, out des, out rev);
            strModelDescription = des.Trim();
            strSTDRev = rev;
        }

        private void RunSequence()
        {
            #region Set for COO on OverpackContent
            strCOO = cboCOO.Text;
            if (string.IsNullOrEmpty(strCOO))
            {
                strCOO = "Manufactured in Vietnam";
            }
            
            #endregion

            if (this.checkBox_PrintOverPackLabel.IsChecked == true)
            {
                PrintPackSerials aPrintPackSerials = new PrintPackSerials(this);

                Nullable<bool> printboxresults = aPrintPackSerials.ShowDialog();
                txtProdOrder.Focus();
                txtProdOrder.SelectAll();
                //lblRevision.Visibility = Visibility.Hidden;
                //txtREVision.Text = "";
                //txtREVision.Visibility = Visibility.Hidden;
            }
            else
            {
                if (this.checkBox_PrintCusLabel.IsChecked == true)
                {
                    try
                    {
                        string strReturn = this.PrintCustomerLabel(this);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (this.checkBox_PrintIndividualBoxLabels.IsChecked == true)
                {
                    try
                    {
                        int iAmt = Int32.Parse(comboBox1.SelectedValue.ToString());
                        string strReturn = this.PrintBoxLabel(this, iAmt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        //var listsn = SNs.Select(x => x.Serial).ToList();
//        private string[] myArray; 
//        private string GetlistSNtoArry (string strProdOrder)//= listsn.ToArray();
//    {

//        SAPXML mySX;
//        SAPPost sp;
//        XmlDocument xmlDoc = new XmlDocument();
//        XmlNodeList oNodes;
//        XmlNode atestNode;
//        long lRetCode = -1;

//        using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
//        {
//            sqlConnection4.Open();
//            SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
//            cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
//            cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderSerials_byProd"; ;
//            cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
//            cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = strProdOrder;
//            cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
//            //cmdGetProdOrderSerials.Parameters.Add("@ProductMap", SqlDbType.Char, 20);
//            //cmdGetProdOrderSerials.Parameters["@ProductMap"].Value = strProductMap_;
//            //cmdGetProdOrderSerials.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
//            SqlDataReader mySerials = cmdGetProdOrderSerials.ExecuteReader();
//            mySerials.Read();
//            if (mySerials[0].ToString().Equals("OK"))
//            {
//                mySerials.NextResult();
//                dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
//                while (mySerials.Read())
//                {

                    


//                    clsPOSerials aPOSerial = new clsPOSerials();
//                    aPOSerial.strPONumber = strPONumber;
//#if DEBUG
//                        Console.WriteLine("**Luu ý trong tru?ng h?p các các case khác");
//#endif

//                    aPOSerial.strSerial = mySerials["TFFC_SerialNumber"].ToString().Trim();
//                    aPOSerial.strMaterial = mySerials["TFFC_Material"].ToString().Trim();

//                    strCurrentMaterial = aPOSerial.strMaterial.Trim();
//                    dictPOInformation.Add(aPOSerial.strSerial.Trim(), aPOSerial);
//                    if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;
//                }
//                sqlConnection4.Close();
//                return "OK";
//            }
//            else  // try RFC to get numbers
//            {
//                if (true)//(!bolHaveInfo)
//                {
//                    try
//                    {
//                        sp = new SAPPost("ZRFC_SEND_POSERIALDATA_ACS");
//                        sp.setProperty("AUFNR", strPONumber);

//                        mySX = sp.Post(strSAPAddress);

//                        xmlDoc = mySX.getXDOC();

//                        DataTable myTable = mySX.getDataTable("ZSERIALNR_ACS");
//                        int iRows = myTable.Rows.Count;
//                        if (iRows > 0)
//                        {
//                            dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();

//                            for (int i = 0; i < iRows; i++)
//                            {
//                                clsPOSerials aPOSerial = new clsPOSerials();
//                                aPOSerial.strPONumber = strPONumber;
//                                aPOSerial.strMaterial = myTable.Rows[i]["MATNR"].ToString().Trim();
//                                aPOSerial.strSerial = myTable.Rows[i]["SERNR"].ToString().Trim();

//                                dictPOInformation.Add(aPOSerial.strSerial, aPOSerial);
//                                if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;
//                                strCurrentMaterial = aPOSerial.strMaterial.Trim();

//                                if (aPOSerial.strSerial.Trim() != "")
//                                {
//                                    try
//                                    {
//                                        using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
//                                        {
//                                            sqlConnection4.Open();
//                                            SqlCommand cmd = new SqlCommand();
//                                            cmd.Connection = sqlConnection4;
//                                            cmd.CommandType = CommandType.StoredProcedure;
//                                            cmd.CommandText = "ame_T_addSerialToTFFC_serialnumbers";//"ame_T_addSerialToConsume";
//                                            //cmd.Parameters.Add("@ProductMap", SqlDbType.Char, 30);
//                                            //cmd.Parameters["@ProductMap"].Value = strProductMap;
//                                            //cmd.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
//                                            cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
//                                            cmd.Parameters["@ProdOrder"].Value = aPOSerial.strPONumber;
//                                            cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
//                                            cmd.Parameters.Add("@Material", SqlDbType.Char, 30);
//                                            cmd.Parameters["@Material"].Value = aPOSerial.strMaterial;
//                                            cmd.Parameters["@Material"].Direction = ParameterDirection.Input;
//                                            cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
//                                            cmd.Parameters["@Serial"].Value = aPOSerial.strSerial;
//                                            cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
//                                            SqlDataReader rec = cmd.ExecuteReader();
//                                            sqlConnection4.Close();
//                                        }
//                                    }
//                                    catch (SqlException ex)
//                                    {
//                                        MessageBox.Show("Error add " + strProductMap + " Serial into server:" + ex.Message);
//                                        return "NG";
//                                    }
//                                }
//                            }
//                        }
//                        return "OK";
//                    }
//                    catch (Exception ex)
//                    {
//                        if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
//                        {
//                            string strEnglishphrase = "";
//                            string strForeignphrase = "";
//                            MessageBox.Show("Error on RFC to get serial numbers (" + getForeignPhrase("ERRORONSERIALRFC", ref strEnglishphrase, ref strForeignphrase) + ") =" + ex.Message);
//                        }
//                        else
//                        {
//                            MessageBox.Show("Error on RFC to get serial numbers=" + ex.Message);
//                        }
//                    }
//                }
//                else
//                {
//                    return "OK";
//                }
//            }
//        }
//        return "NG";
//    }

         
        /// <summary>
        /// Lấy số List SN theo PO để check SN packing có thuộc PO này không
        /// </summary>
        /// <param name="strPONumber_"></param>
        /// <returns></returns>

        private string GetListSN(string strPONumber_)
        {
#if DEBUG
            Console.WriteLine("*Lay so SN theo Production Order");
#endif

            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList oNodes;
            XmlNode atestNode;
            long lRetCode = -1;

            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();
                SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderSerials_byProd"; ;
                cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = strPONumber_;
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                //cmdGetProdOrderSerials.Parameters.Add("@ProductMap", SqlDbType.Char, 20);
                //cmdGetProdOrderSerials.Parameters["@ProductMap"].Value = strProductMap_;
                //cmdGetProdOrderSerials.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                SqlDataReader mySerials = cmdGetProdOrderSerials.ExecuteReader();
                mySerials.Read();
                if (mySerials[0].ToString().Equals("OK"))
                {
                    mySerials.NextResult();
                    dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
                    while (mySerials.Read())
                    {
                        clsPOSerials aPOSerial = new clsPOSerials();
                        aPOSerial.strPONumber = strPONumber;


                        aPOSerial.strSerial = mySerials["TFFC_SerialNumber"].ToString().Trim();
                        aPOSerial.strMaterial = mySerials["TFFC_Material"].ToString().Trim();

                        strCurrentMaterial = aPOSerial.strMaterial.Trim();
                        dictPOInformation.Add(aPOSerial.strSerial.Trim(), aPOSerial);
                        if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;


                        
                    }

                    
                    foreach (var pair in dictPOInformation)
                    {

#if DEBUG
                        Console.WriteLine("{0}", pair.Key);
#endif
                        SetSNofPOcheck4Base();
                    
                    }
                    //////Khang add to check for each dictionary-end

                    sqlConnection4.Close();
                    return "OK";

                    


                     }
                else  // try RFC to get numbers
                {
                    if (true)//(!bolHaveInfo)
                    {

                        TraceStepDoing("Add sN vao he thong lay tu sap");
                        try
                        {
                            sp = new SAPPost("ZRFC_SEND_POSERIALDATA_ACS");
                            sp.setProperty("AUFNR", strPONumber);

                            mySX = sp.Post(strSAPAddress);

                            xmlDoc = mySX.getXDOC();

                            DataTable myTable = mySX.getDataTable("ZSERIALNR_ACS");
                            int iRows = myTable.Rows.Count;
                            if (iRows > 0)
                            {
                                dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();

                                for (int i = 0; i < iRows; i++)
                                {
                                    clsPOSerials aPOSerial = new clsPOSerials();
                                    aPOSerial.strPONumber = strPONumber;
                                    aPOSerial.strMaterial = myTable.Rows[i]["MATNR"].ToString().Trim();
                                    aPOSerial.strSerial = myTable.Rows[i]["SERNR"].ToString().Trim();

                                    dictPOInformation.Add(aPOSerial.strSerial, aPOSerial);
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
                                            MessageBox.Show("Error add " + strProductMap + " Serial into server:" + ex.Message);
                                            return "NG";
                                        }
                                    }
                                }
                            }
                            return "OK";
                        }
                        catch (Exception ex)
                        {
                            if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                            {
                                string strEnglishphrase = "";
                                string strForeignphrase = "";
                                MessageBox.Show("Error on RFC to get serial numbers (" + getForeignPhrase("ERRORONSERIALRFC", ref strEnglishphrase, ref strForeignphrase) + ") =" + ex.Message);
                            }
                            else
                            {
                                MessageBox.Show("Error on RFC to get serial numbers=" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        return "OK";
                    }
                }
            }
            return "NG";
        }

        public string GetListSNbyPO(string strPONumber_)
        {
#if DEBUG
            Console.WriteLine("*Lay so SN theo Production Order");
#endif

            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList oNodes;
            XmlNode atestNode;
            long lRetCode = -1;

            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();
                SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderSerials_byProd"; ;
                cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = strPONumber_;
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                //cmdGetProdOrderSerials.Parameters.Add("@ProductMap", SqlDbType.Char, 20);
                //cmdGetProdOrderSerials.Parameters["@ProductMap"].Value = strProductMap_;
                //cmdGetProdOrderSerials.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                SqlDataReader mySerials = cmdGetProdOrderSerials.ExecuteReader();
                mySerials.Read();
                if (mySerials[0].ToString().Equals("OK"))
                {
                    mySerials.NextResult();
                    dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
                    while (mySerials.Read())
                    {
                        clsPOSerials aPOSerial = new clsPOSerials();
                        aPOSerial.strPONumber = strPONumber;


                        aPOSerial.strSerial = mySerials["TFFC_SerialNumber"].ToString().Trim();
                        aPOSerial.strMaterial = mySerials["TFFC_Material"].ToString().Trim();

                        strCurrentMaterial = aPOSerial.strMaterial.Trim();
                        dictPOInformation.Add(aPOSerial.strSerial.Trim(), aPOSerial);
                        if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;



                    }


                    foreach (var pair in dictPOInformation)
                    {

#if DEBUG
                        Console.WriteLine("{0}", pair.Key);
#endif
                        SetSNofPOcheck4Base();

                    }
                    //////Khang add to check for each dictionary-end

                    sqlConnection4.Close();
                    return "OK";




                }
                else  // try RFC to get numbers
                {
                    if (true)//(!bolHaveInfo)
                    {

                        TraceStepDoing("Add sN vao he thong lay tu sap");
                        try
                        {
                            sp = new SAPPost("ZRFC_SEND_POSERIALDATA_ACS");
                            sp.setProperty("AUFNR", strPONumber);

                            mySX = sp.Post(strSAPAddress);

                            xmlDoc = mySX.getXDOC();

                            DataTable myTable = mySX.getDataTable("ZSERIALNR_ACS");
                            int iRows = myTable.Rows.Count;
                            if (iRows > 0)
                            {
                                dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();

                                for (int i = 0; i < iRows; i++)
                                {
                                    clsPOSerials aPOSerial = new clsPOSerials();
                                    aPOSerial.strPONumber = strPONumber;
                                    aPOSerial.strMaterial = myTable.Rows[i]["MATNR"].ToString().Trim();
                                    aPOSerial.strSerial = myTable.Rows[i]["SERNR"].ToString().Trim();

                                    dictPOInformation.Add(aPOSerial.strSerial, aPOSerial);
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
                                            MessageBox.Show("Error add " + strProductMap + " Serial into server:" + ex.Message);
                                            return "NG";
                                        }
                                    }
                                }
                            }
                            return "OK";
                        }
                        catch (Exception ex)
                        {
                            if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                            {
                                string strEnglishphrase = "";
                                string strForeignphrase = "";
                                MessageBox.Show("Error on RFC to get serial numbers (" + getForeignPhrase("ERRORONSERIALRFC", ref strEnglishphrase, ref strForeignphrase) + ") =" + ex.Message);
                            }
                            else
                            {
                                MessageBox.Show("Error on RFC to get serial numbers=" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        return "OK";
                    }
                }
            }
            return "NG";
        }


        private string GetListSNwhenTinformationComplete(string strPONumber_)
        {
            try
            {
#if DEBUG
                Console.WriteLine("*Lay so SN theo Production Order");
#endif

                SAPXML mySX;
                SAPPost sp;
                XmlDocument xmlDoc = new XmlDocument();
                XmlNodeList oNodes;
                XmlNode atestNode;
                long lRetCode = -1;

                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    sqlConnection4.Open();
                    SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                    cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                    cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderSerials_byProd"; ;
                    cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
                    cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = strPONumber_;
                    cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                    //cmdGetProdOrderSerials.Parameters.Add("@ProductMap", SqlDbType.Char, 20);
                    //cmdGetProdOrderSerials.Parameters["@ProductMap"].Value = strProductMap_;
                    //cmdGetProdOrderSerials.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                    SqlDataReader mySerials = cmdGetProdOrderSerials.ExecuteReader();
                    mySerials.Read();
                    if (mySerials[0].ToString().Equals("OK"))
                    {
                        mySerials.NextResult();
                        dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
                        while (mySerials.Read())
                        {
                            clsPOSerials aPOSerial = new clsPOSerials();
                            aPOSerial.strPONumber = strPONumber;


                            aPOSerial.strSerial = mySerials["TFFC_SerialNumber"].ToString().Trim();
                            aPOSerial.strMaterial = mySerials["TFFC_Material"].ToString().Trim();

                            strCurrentMaterial = aPOSerial.strMaterial.Trim();
                            dictPOInformation.Add(aPOSerial.strSerial.Trim(), aPOSerial);
                            if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;

                        }

                              foreach (var pair in dictPOInformation)
                        {

#if DEBUG
                            Console.WriteLine("{0}", pair.Key);
#endif
                            SetSNofPOcheck4Base();

                        }
                        //////Khang add to check for each dictionary-end

                        sqlConnection4.Close();
                        return "OK";
                        
                    }
                    else //none  // try RFC to get numbers
                    {
                        #region khongdung

                        //if (true)//(!bolHaveInfo)
                        //{

                        //    TraceStepDoing("Add sN vao he thong lay tu sap");
                        //    try
                        //    {
                        //        sp = new SAPPost("ZRFC_SEND_POSERIALDATA_ACS");
                        //        sp.setProperty("AUFNR", strPONumber);

                        //        mySX = sp.Post(strSAPAddress);

                        //        xmlDoc = mySX.getXDOC();

                        //        DataTable myTable = mySX.getDataTable("ZSERIALNR_ACS");
                        //        int iRows = myTable.Rows.Count;
                        //        if (iRows > 0)
                        //        {
                        //            dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();

                        //            for (int i = 0; i < iRows; i++)
                        //            {
                        //                clsPOSerials aPOSerial = new clsPOSerials();
                        //                aPOSerial.strPONumber = strPONumber;
                        //                aPOSerial.strMaterial = myTable.Rows[i]["MATNR"].ToString().Trim();
                        //                aPOSerial.strSerial = myTable.Rows[i]["SERNR"].ToString().Trim();

                        //                dictPOInformation.Add(aPOSerial.strSerial, aPOSerial);
                        //                if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;
                        //                strCurrentMaterial = aPOSerial.strMaterial.Trim();

                        //                if (aPOSerial.strSerial.Trim() != "")
                        //                {
                        //                    try
                        //                    {
                        //                        using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                        //                        {

                        //                            sqlConnection4.Open();
                        //                            SqlCommand cmd = new SqlCommand();
                        //                            cmd.Connection = sqlConnection4;
                        //                            cmd.CommandType = CommandType.StoredProcedure;
                        //                            cmd.CommandText = "ame_T_addSerialToTFFC_serialnumbers";//"ame_T_addSerialToConsume";
                        //                            //cmd.Parameters.Add("@ProductMap", SqlDbType.Char, 30);
                        //                            //cmd.Parameters["@ProductMap"].Value = strProductMap;
                        //                            //cmd.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                        //                            cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                        //                            cmd.Parameters["@ProdOrder"].Value = aPOSerial.strPONumber;
                        //                            cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                        //                            cmd.Parameters.Add("@Material", SqlDbType.Char, 30);
                        //                            cmd.Parameters["@Material"].Value = aPOSerial.strMaterial;
                        //                            cmd.Parameters["@Material"].Direction = ParameterDirection.Input;
                        //                            cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                        //                            cmd.Parameters["@Serial"].Value = aPOSerial.strSerial;
                        //                            cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                        //                            SqlDataReader rec = cmd.ExecuteReader();
                        //                            sqlConnection4.Close();
                        //                        }
                        //                    }
                        //                    catch (SqlException ex)
                        //                    {
                        //                        MessageBox.Show("Error add " + strProductMap + " Serial into server:" + ex.Message);
                        //                        return "NG";
                        //                    }
                        //                }
                        //            }
                        //        }
                        //        return "OK";
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                        //        {
                        //            string strEnglishphrase = "";
                        //            string strForeignphrase = "";
                        //            MessageBox.Show("Error on RFC to get serial numbers (" + getForeignPhrase("ERRORONSERIALRFC", ref strEnglishphrase, ref strForeignphrase) + ") =" + ex.Message);
                        //        }
                        //        else
                        //        {
                        //            MessageBox.Show("Error on RFC to get serial numbers=" + ex.Message);
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    return "OK";
                        //} 
                        #endregion
                    }
                }
                return "NG";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
              
            }
            return "NG";
        }


        private string RunSubMain()
        {
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList oNodes;
            XmlNode atestNode;
            long lRetCode = -1;

            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();
                SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                cmdGetProdOrderSerials.CommandText = "ame_T_getProdOrderSerials"; ;
                cmdGetProdOrderSerials.Parameters.Add("@ProdOrder", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Value = strPONumber;
                cmdGetProdOrderSerials.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                cmdGetProdOrderSerials.Parameters.Add("@ProductMap", SqlDbType.Char, 20);
                cmdGetProdOrderSerials.Parameters["@ProductMap"].Value = strProductMap;
                cmdGetProdOrderSerials.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                SqlDataReader mySerials = cmdGetProdOrderSerials.ExecuteReader();
                mySerials.Read();
                if (mySerials[0].ToString().Equals("OK"))
                {
                    mySerials.NextResult();
                    dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
                    while (mySerials.Read())
                    {
                        clsPOSerials aPOSerial = new clsPOSerials();
                        aPOSerial.strPONumber = strPONumber;
#if DEBUG
                        Console.WriteLine("**Lưu ý trong trường hợp các các case khác");
#endif

                        aPOSerial.strSerial = mySerials["TFFC_SerialNumber"].ToString().Trim();
                        aPOSerial.strMaterial = mySerials["TFFC_Material"].ToString().Trim();

                        #region B?n g?c c?a G?u
                        //switch (strProductMap)
                        //{
                            

                        //    case "FFC":
                        //        aPOSerial.strSerial = mySerials["TFFC_SerialNumber"].ToString().Trim();
                        //        aPOSerial.strMaterial = mySerials["TFFC_Material"].ToString().Trim();
                        //        break;
                        //    case "DLM":
                        //        aPOSerial.strSerial = mySerials["TDLM_SerialNumber"].ToString().Trim();
                        //        aPOSerial.strMaterial = mySerials["TDLM_Material"].ToString().Trim();
                        //        break;
                        //    case "BASE":
                        //        aPOSerial.strSerial = mySerials["TBASE_SerialNumber"].ToString().Trim();
                        //        aPOSerial.strMaterial = mySerials["TBASE_Material"].ToString().Trim();
                        //        break; 
                           

                         
                               
                          
                        //  default:
                        //        goto case "FFC";
                        //        break;

                        //}
                        #endregion
                        strCurrentMaterial = aPOSerial.strMaterial.Trim();
                        dictPOInformation.Add(aPOSerial.strSerial.Trim(), aPOSerial);
                        if (aPOSerial.strMaterial != "" && aPOSerial.strMaterial != null) strPOMaterial = aPOSerial.strMaterial;
                    }
                    sqlConnection4.Close();
                    return "OK";
                }
                else  // try RFC to get numbers
                {
                    if (!bolHaveInfo)
                    {
                        try
                        {
                            sp = new SAPPost("ZRFC_SEND_POSERIALDATA_ACS");
                            sp.setProperty("AUFNR", strPONumber);

                            mySX = sp.Post(strSAPAddress);

                            xmlDoc = mySX.getXDOC();

                            DataTable myTable = mySX.getDataTable("ZSERIALNR_ACS");
                            int iRows = myTable.Rows.Count;
                            if (iRows > 0)
                            {
                                dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();

                                for (int i = 0; i < iRows; i++)
                                {
                                    clsPOSerials aPOSerial = new clsPOSerials();
                                    aPOSerial.strPONumber = strPONumber;
                                    aPOSerial.strMaterial = myTable.Rows[i]["MATNR"].ToString().Trim();
                                    aPOSerial.strSerial = myTable.Rows[i]["SERNR"].ToString().Trim();

                                    dictPOInformation.Add(aPOSerial.strSerial, aPOSerial);
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
                                                cmd.CommandText = "ame_T_addSerialToConsume";
                                                cmd.Parameters.Add("@ProductMap", SqlDbType.Char, 30);
                                                cmd.Parameters["@ProductMap"].Value = strProductMap;
                                                cmd.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
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
                                            MessageBox.Show("Error add " + strProductMap + " Serial into server:" + ex.Message);
                                            return "NG";
                                        }
                                    }
                                }
                            }
                            return "OK";
                        }
                        catch (Exception ex)
                        {
                            if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                            {
                                string strEnglishphrase = "";
                                string strForeignphrase = "";
                                MessageBox.Show("Error on RFC to get serial numbers (" + getForeignPhrase("ERRORONSERIALRFC", ref strEnglishphrase, ref strForeignphrase) + ") =" + ex.Message);
                            }
                            else
                            {
                                MessageBox.Show("Error on RFC to get serial numbers=" + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        return "OK";
                    }
                }
            }
            return "NG";
        }

        
    private void T_Information_Update_POqty(int valueinputmanual)
        {
            if (intPOQuantity == 0) intPOQuantity = valueinputmanual;
            
            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection4;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ame_T_Information_Update_POqty";

                cmd.Parameters.Add("@T_ProdOrder", SqlDbType.Char, 20);
                cmd.Parameters["@T_ProdOrder"].Value = strPONumber;
                cmd.Parameters["@T_ProdOrder"].Direction = ParameterDirection.Input;

                cmd.Parameters.Add("@T_Quantity", SqlDbType.Int, 4);
                cmd.Parameters["@T_Quantity"].Value = intPOQuantity;
                cmd.Parameters["@T_Quantity"].Direction = ParameterDirection.Input;
               
                SqlDataReader rec = cmd.ExecuteReader();
                sqlConnection4.Close();
            }
        }

        private void Add_T_Information_Order()
        {
            if (intPOQuantity==0) intPOQuantity = dictPOInformation.Count;
            
            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection4;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ame_T_addProdOrderInfo";

                cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                cmd.Parameters["@ProdOrder"].Value = strPONumber;
                cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@ProductMap", SqlDbType.Char, 30);
                cmd.Parameters["@ProductMap"].Value = strProductMap;
                cmd.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Material", SqlDbType.Char, 30);
                cmd.Parameters["@Material"].Value = strPOMaterial;
                cmd.Parameters["@Material"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Revision", SqlDbType.Char, 30);
                //if (strProductMap != "BASE" && strProductMap != "FRUwoACS") strPORev = "N/A"; //dec 10 enable record rev cho FFC
                cmd.Parameters["@Revision"].Value = strPORev;                
                cmd.Parameters["@Revision"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int, 4);
                cmd.Parameters["@Quantity"].Value = intPOQuantity;
                cmd.Parameters["@Quantity"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Packed", SqlDbType.Int, 4);
                cmd.Parameters["@Packed"].Value = 0;
                cmd.Parameters["@Packed"].Direction = ParameterDirection.Input;

                
                SqlDataReader rec = cmd.ExecuteReader();
                sqlConnection4.Close();
            }
        }

        private void Add_T_Information_Order_withoutQty()
        {
            if (intPOQuantity == 0) intPOQuantity = dictPOInformation.Count;


            using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
            {
                sqlConnection4.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection4;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ame_T_addProdOrderInfo";
                cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                cmd.Parameters["@ProdOrder"].Value = strPONumber;
                cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@ProductMap", SqlDbType.Char, 30);
                cmd.Parameters["@ProductMap"].Value = "";// strProductMap;
                cmd.Parameters["@ProductMap"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Material", SqlDbType.Char, 30);
                cmd.Parameters["@Material"].Value = strPOMaterial;
                cmd.Parameters["@Material"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Revision", SqlDbType.Char, 30);
                //if (strProductMap != "BASE" && strProductMap != "FRUwoACS") strPORev = "N/A"; //dec 10 enable record rev cho FFC
                cmd.Parameters["@Revision"].Value = strPORev;
                cmd.Parameters["@Revision"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Quantity", SqlDbType.Int, 4);
                cmd.Parameters["@Quantity"].Value = 0;// intPOQuantity;
                cmd.Parameters["@Quantity"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@Packed", SqlDbType.Int, 4);
                cmd.Parameters["@Packed"].Value = 0;
                cmd.Parameters["@Packed"].Direction = ParameterDirection.Input;
                SqlDataReader rec = cmd.ExecuteReader();
                sqlConnection4.Close();
            }
        }

        public string getForeignPhrase(string strPhraseKey, ref string strEnglishPhrase, ref string strForeignPhrase)
        {
            string strReturnPhrase = "";

            strEnglishPhrase = "" ;
            strForeignPhrase="" ;

            using (sqlConnection1 = new SqlConnection(strSqlConnection1))
            {
                sqlConnection1.Open();
                if (sqlConnection1.State.Equals(ConnectionState.Open))
                {
                    try
                    {
                        SqlCommand cmdGetPhrase = sqlConnection1.CreateCommand();
                        cmdGetPhrase.CommandType = CommandType.StoredProcedure;
                        cmdGetPhrase.CommandText = "ame_get_foreignphrase"; ;


                        cmdGetPhrase.Parameters.Add("@phrasekey", SqlDbType.Char, 20);
                        cmdGetPhrase.Parameters["@phrasekey"].Value = strPhraseKey;
                        cmdGetPhrase.Parameters["@phrasekey"].Direction = ParameterDirection.Input;


                        SqlDataReader myPhrase = cmdGetPhrase.ExecuteReader();

                        myPhrase.Read();
                        strEnglishPhrase = myPhrase["PHRASE_ENGLISH"].ToString().Trim();
                        strForeignPhrase = myPhrase["PHRASE_FOREIGN"].ToString().Trim();
                        strReturnPhrase = strForeignPhrase;

                    }
                    catch (Exception ex)
                    {
                        strEnglishPhrase = "";
                        strForeignPhrase = "";
                        strReturnPhrase = "";
                    }
                }
            }


            return strReturnPhrase;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            string result = "NG";
            //MessageBoxResult result1 = new MessageBoxResult();
            //result1 = MessageBox.Show("Bạn có chắc chắn muốn Rework không?","Thông báo",MessageBoxButton.OKCancel,MessageBoxImage.Exclamation);
            //if (result1 == MessageBoxResult.OK)
             if (true)
            {   
                BoxRework BoxRework = new BoxRework(this);
                Nullable<bool> getboxrework = BoxRework.ShowDialog();
                if (strBoxReworkresult.Equals("OK"))
                {
                    boxRework = true;
                    txtProdOrder.Text = strPONumber;
                    txtProdOrder.Focus();
                    txtProdOrder.SelectAll();

                    result = CheckProdOrder_Setup();
                    if (result.Equals("OK"))
                    {
                        RunSequence();
                    }
                }       
            }
            else //Cancel
            {
                return;
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            string result = "NG";

            if (e.Key == Key.Enter)
            {
                if (txtREVision.Text == "")
                {
                    MessageBox.Show("Input Revision");
                    txtREVision.Focus();
                }
                else
                {
                    strPORev = txtREVision.Text.Trim().ToUpper();
                    txtREVision.Text = strPORev;
                    result = CheckProdOrder_Setup();
                    if (result.Equals("OK"))
                    {
                        RunSequence();
                    }
                }
            }
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            bolRePrint = true;
            PrintPackSerials aPrintPackSerials = new PrintPackSerials(this);
            aPrintPackSerials.DoRePrint(this);
            aPrintPackSerials.Close();
            bolRePrint = false;
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
                //if (lRetCode == 0)
                //{
                //    objSalesOrderInfo = new clsPOSalesOrderInfo();
                //    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strSalesOrder = atestNode.InnerText.ToString();
                //    }

                //    atestNode = xmlDoc.GetElementsByTagName("SALES_ORDER_ITEM").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strSalesItem = atestNode.InnerText.ToString();
                //    }

                //    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_MATERIAL").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strCustomerMaterial = atestNode.InnerText.ToString();
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("MATERIAL").Item(0);
                //    if (atestNode != null)
                //    {
                //        if (atestNode.InnerText.ToString().Trim().Length > 0)
                //        {
                //            objSalesOrderInfo.strMaterial = atestNode.InnerText.ToString();
                //        }
                //        else
                //        {
                //            objSalesOrderInfo.strMaterial = "";
                //        }
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strMaterial = "";
                //    }



                //    atestNode = xmlDoc.GetElementsByTagName("DESCRIPTION").Item(0);
                //    if (atestNode != null)
                //    {
                //        if (atestNode.InnerText.ToString().Trim().Length > 0)
                //        {
                //            objSalesOrderInfo.strDescription = atestNode.InnerText.ToString();
                //        }
                //        else
                //        {
                //            objSalesOrderInfo.strDescription = "";
                //        }
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strDescription = "";
                //    }



                //    atestNode = xmlDoc.GetElementsByTagName("GRAVITY_ZONE").Item(0);
                //    if (atestNode != null)
                //    {
                //        if (atestNode.InnerText.ToString().Trim().Length > 0)
                //        {
                //            objSalesOrderInfo.lGravityZone = Int32.Parse(atestNode.InnerText.ToString());
                //        }
                //        else
                //        {
                //            objSalesOrderInfo.lGravityZone = -1;
                //        }
                //    }
                //    else
                //    {
                //    }



                //    atestNode = xmlDoc.GetElementsByTagName("CUSTOMER_PO").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strCustomerPurchaseOrder = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("ATTN").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strAttn = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strAttn = "";
                //    }

                //    // new fields start here
                //    atestNode = xmlDoc.GetElementsByTagName("CUSTNAME1").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strCustName1 = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strCustName1 = "";
                //    }



                //    atestNode = xmlDoc.GetElementsByTagName("STREETADDRESS").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strStreetAddress = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strStreetAddress = "";
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("CITY").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strCity = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strCity = "";
                //    }



                //    atestNode = xmlDoc.GetElementsByTagName("REGION").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strStateRegion = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strStateRegion = "";
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("POSTALCODE").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strPostalCode = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strPostalCode = "";
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("DESTINATIONCODE").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strDestinationCode = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strDestinationCode = "";
                //    }



                //    atestNode = xmlDoc.GetElementsByTagName("OTDDATE").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.dtOTDDate = DateTime.Parse(atestNode.InnerText.ToString());
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.dtOTDDate = DateTime.Parse("");
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("QTY").Item(0);
                //    if (atestNode != null)
                //    {
                //        if (atestNode.InnerText.ToString().Trim().Length > 0)
                //        {
                //            objSalesOrderInfo.lQty = Int32.Parse(atestNode.InnerText.ToString());
                //        }
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.lQty = 100000;
                //    }




                //    atestNode = xmlDoc.GetElementsByTagName("VENDOR").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strVendor = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strVendor = "";
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("COUNTRY").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strCountry = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strCountry = "";
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("HIERARCHY").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strHierarchy = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strHierarchy = "";
                //    }


                //    atestNode = xmlDoc.GetElementsByTagName("BLOCKINGCODE").Item(0);
                //    if (atestNode != null)
                //    {
                //        objSalesOrderInfo.strBlockingCode = atestNode.InnerText.ToString();
                //    }
                //    else
                //    {
                //        objSalesOrderInfo.strBlockingCode = "";
                //    }






                //    if (objSalesOrderInfo.strSalesOrder.Trim().Length > 0 || objSalesOrderInfo.strCustName1.Trim().Length > 0)
                //    {
                //        bFromSalesOrder = true;
                //        //this.checkBox1.IsChecked = true;
                //    }
                //    else
                //    {
                //        bFromSalesOrder = false;
                //        //this.checkBox1.IsChecked = false;
                //    }




                //} //  if (lRetCode == 0)
                //else
                //{
                //    objSalesOrderInfo = new clsPOSalesOrderInfo();
                //    objSalesOrderInfo.strSalesOrder = "";
                //    objSalesOrderInfo.strSalesItem = "";
                //    objSalesOrderInfo.strCustomerMaterial = "";
                //    objSalesOrderInfo.strMaterial = "";
                //    objSalesOrderInfo.strDescription = "";
                //    objSalesOrderInfo.lGravityZone = -1;
                //    objSalesOrderInfo.strCustomerPurchaseOrder = "";
                //    objSalesOrderInfo.strAttn = "";
                //    objSalesOrderInfo.strCustName1 = "";
                //    objSalesOrderInfo.strStreetAddress = "";
                //    objSalesOrderInfo.strCity = "";
                //    objSalesOrderInfo.strStateRegion = "";
                //    objSalesOrderInfo.strPostalCode = "";
                //    objSalesOrderInfo.strDestinationCode = "";
                //    objSalesOrderInfo.dtOTDDate = DateTime.Parse("1/1/2001");
                //    objSalesOrderInfo.lQty = 100000;
                //    objSalesOrderInfo.strVendor = "";
                //    objSalesOrderInfo.strCountry = "";
                //    objSalesOrderInfo.strHierarchy = "";
                //    objSalesOrderInfo.strBlockingCode = "";
                //}
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Error on getting PO BOM fields=" + ex.Message);
            }



        }

        /// <summary>
        /// Lấy model từ số PO
        /// Test thử OK chưa
        /// </summary>
        public static void getModelfromPOno(string strPONumber, out string modelname)
        {
            modelname = "";
            //dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
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

        private void txtProdOrder_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txtREVision.Text = "";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            WareHouse wareHouse = new WareHouse();
            wareHouse.ShowDialog();
            //Frmtest frm = new Frmtest();
            //FrmCombineBox frm = new FrmCombineBox();
            //frm.ShowDialog();
            

            this.Close();
        }

        private void cboCOO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
