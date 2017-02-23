using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BartenderLibrary;
using ACSEE.NET;
using System.Xml;
using System.IO;
using System.Data.OleDb;

namespace PrintPack
{
    /// <summary>
    /// Interaction logic for WareHouse.xaml
    /// </summary>
    public partial class WareHouse : Window
    {
        //Mode 0: UnPacking All
        //Mode 1: Combine Packing
        //Mode 2: Scan Box Number
        //Mode 3: UnPacking Apart
        //Mode 4: Create New Box
        //Mode 5: Box RePrint
        //Mode 6: RePrint
        public byte Mode;
        public Boolean bolwoACS = false;

        public WareHouse()
        {
            InitializeComponent();
            Init();
            try
            {
                sqlConnection1 = new SqlConnection();
                strSqlConnection1 = ConfigurationManager.AppSettings.Get("ACSEECONNECTION").ToString();
                sqlConnection2 = new SqlConnection();
                strSqlConnection2 = ConfigurationManager.AppSettings.Get("ACSEECLIENTSTATECONNECTION").ToString();
                sqlConnection3 = new SqlConnection();
                strSqlConnection3 = ConfigurationManager.AppSettings.Get("ACSEESTATECONNECTION").ToString();
                sqlConnection4 = new SqlConnection();
                strSqlConnection4 = ConfigurationManager.AppSettings.Get("FFCPACKINGCONNECTION").ToString();

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
                strPrintUnPackingAll = ConfigurationManager.AppSettings.Get("PrintUnPackingAll").ToString();
                strPrintSmallBox = ConfigurationManager.AppSettings.Get("PrintSmallBox").ToString();

                strReportBoxNumber = ConfigurationManager.AppSettings.Get("ReportBoxNumber").ToString();
                strReportUnPackingParts = ConfigurationManager.AppSettings.Get("ReportUnPackingParts").ToString();

                iMaxSerialsPerOrder=Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsAtATime").ToString().Trim()) ;
                iMaxSerialsFFC = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsFFC").ToString().Trim());
                iMaxSerialsDLM = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsDLM").ToString().Trim());
                iMaxSerialsBASE = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsBASE").ToString().Trim());
                iMaxSerialsUnPackingAll = Int32.Parse(ConfigurationManager.AppSettings.Get("MaxSerialsUnPackingAll").ToString().Trim());

                btLabel = new BTLabel(strSqlConnection1,
                                        strSqlConnection2,
                                        strSqlConnection3);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public Dictionary<string, clsPOSerials> dictPOInformation ;
        public string strCurrentMaterial ="";

        public string strSqlConnection1;
        public string strSqlConnection2;
        public string strSqlConnection3;
        public string strSqlConnection4;

        public string strSAPAddress;

        public System.Data.SqlClient.SqlConnection sqlConnection1;
        public System.Data.SqlClient.SqlConnection sqlConnection2;
        public System.Data.SqlClient.SqlConnection sqlConnection3;
        public System.Data.SqlClient.SqlConnection sqlConnection4;

        public int iMaxSerialsPerOrder=1000;
        public int iMaxSerialsFFC = 15;
        public int iMaxSerialsDLM = 10;
        public int iMaxSerialsBASE = 20;
        public int iMaxSerialsUnPackingAll = 18;

        public string strPrintOverPack;
        public string strPrintOverPackVN;
        public string strPrintOverPackContent;
        public string strPrintCustomer;
        public string strPrintUnPackingAll;
        public string strPrintSmallBox;

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

        public string strReportBoxNumber;
        public string strReportUnPackingParts;

        public string strPOMaterial;
        public string strPONumber;
        public string strPORev;
        public string strPONumberbak;
        public string strPORevbak;
        public int intPOQuantity;
        public int intPOPacked;
        public int intPORun;

        public BTLabel btLabel;
        public string box = "NA";
        public string boxnew = "NA";
        public string pyear = "NA";
        public Boolean dataExist = false;
        public string strProductMap = "NG";
        public SortedList<string, ucSerialNumber> slBoxRePrint = new SortedList<string, ucSerialNumber>();
        public SortedList<string, ucSerialNumber> slRePrint = new SortedList<string, ucSerialNumber>();
        public Boolean bolRePrint = false;
        public int PartQuantity = 0;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //this.Close();
        }

        #region Initial
        private void Init()
        {
            SetLabel(false);
            SetLabel1(false);
            SetButton(true);
        }

        private void SetButton(Boolean State)
        {
            button1.IsEnabled = State;  //UnPacking All
            button2.IsEnabled = State;  //Combine Packing
            button3.IsEnabled = State;  //Scan Box Number
            button4.IsEnabled = State;  //UnPacking Apart
            button5.IsEnabled = State;  //Create New Box
            button6.IsEnabled = State;  //Box RePrint
            button7.IsEnabled = State;  //RePrint
            button8.IsEnabled = State;  //UnPacking + New Box
            button9.IsEnabled = State;  //UnPacking Parts
            button10.IsEnabled = State; //Box Manual
        }

        private void SetLabel(Boolean State)
        {
            if (State)
            {
                label1.Visibility = Visibility.Visible;
                textBox1.Visibility = Visibility.Visible;
                textBox1.Focus();
                textBox1.SelectAll();
            }
            else
            {
                label1.Visibility = Visibility.Hidden;
                textBox1.Visibility = Visibility.Hidden;
                textBox1.Text = "";
            }
        }

        private void SetLabel1(Boolean State)
        {
            if (State)
            {
                checkBox1.Visibility = Visibility.Visible;
                label2.Visibility = Visibility.Visible;
                textBox2.Visibility = Visibility.Visible;
                label3.Visibility = Visibility.Visible;
                textBox3.Visibility = Visibility.Visible;
                label4.Visibility = Visibility.Visible;
                textBox4.Visibility = Visibility.Visible;
                button11.Visibility = Visibility.Visible;
                button12.Visibility = Visibility.Visible;
                textBox2.Focus();
                textBox2.SelectAll();
            }
            else
            {
                checkBox1.Visibility = Visibility.Hidden;
                label2.Visibility = Visibility.Hidden;
                textBox2.Visibility = Visibility.Hidden;
                label3.Visibility = Visibility.Hidden;
                textBox3.Visibility = Visibility.Hidden;
                label4.Visibility = Visibility.Hidden;
                textBox4.Visibility = Visibility.Hidden;
                button11.Visibility = Visibility.Hidden;
                button12.Visibility = Visibility.Hidden;
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
            }
        }

        #endregion

        #region ButtonEvent
        //UnPacking All
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn trả thùng ra sản xuất không?","Thông báo",MessageBoxButton.OKCancel,MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                Mode = 0;
                strPONumber = getPOUnPack();
                WareHouseSerials aPrintPackSerials = new WareHouseSerials(this);
                Nullable<bool> printboxresults = aPrintPackSerials.ShowDialog();
                Init();
            }
            else
            {
                Init();
            }
        }

        //Combine Packing
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn gộp thùng không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                Mode = 1;
                SetLabel(true);
            }
            else
            {
                Init();
            }
        }

        //Scan Box Number
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn đọc số thùng lấy số Serial không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                if (CheckReportBoxNumberExist())
                {
                    MessageBoxResult result = new MessageBoxResult();
                    result = MessageBox.Show("File Packing.xlsx đã có ngoài Desktop, move để lưu lại hay del nếu không cần nữa", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Cancel)
                    {
                        Init();
                        return;
                    }
                }

                SetButton(false);
                Mode = 2;
                WareHouseSerials aPrintPackSerials = new WareHouseSerials(this);
                Nullable<bool> printboxresults = aPrintPackSerials.ShowDialog();
            }
            Init();
        }

        //UnPacking Apart
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn tách thùng không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                Mode = 3;
                SetLabel(true);
            }
            else
            {
                Init();
            }
        }

        //Create New Box
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn tạo thùng mới không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                Mode = 4;
                WareHouseSerials aPrintPackSerials = new WareHouseSerials(this);
                Nullable<bool> printboxresults = aPrintPackSerials.ShowDialog();
                Init();
            }
            else
            {
                Init();
            }
        }

        //Box RePrint
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn in lại nhãn thùng không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                Mode = 5;
                SetLabel(true);
            }
            else
            {
                Init();
            }
        }

        //RePrint
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            SetButton(false);
            bolRePrint = true;
            WareHouseSerials aPrintPackSerials = new WareHouseSerials(this);
            aPrintPackSerials.DoRePrint(this);
            aPrintPackSerials.Close();
            bolRePrint = false;
            Init();
        }

        //UnPacking + New Box
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn tách thùng và tạo thùng mới không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                Mode = 6;
                SetLabel(true);
            }
            else
            {
                Init();
            }
        }

        //UnPacking Parts
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn lấy các Parts chưa đóng gói không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                DoReportUnPackingParts();
            }
            Init();
        }

        //Box Manual
        private void button10_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result1 = new MessageBoxResult();
            result1 = MessageBox.Show("Bạn có chắc chắn muốn in nhãn nhỏ không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            if (result1 == MessageBoxResult.OK)
            {
                SetButton(false);
                SetLabel1(true);
            }
            else
            {
                Init();
            }
        }

        //Box Manual - Print
        private void button11_Click(object sender, RoutedEventArgs e)
        {
            PrintBoxManual(textBox2.Text, textBox3.Text, textBox4.Text);
        }

        //Box Manual - Close
        private void button12_Click(object sender, RoutedEventArgs e)
        {
            Init();
        }

        

        //Enter at Box Number textbox
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            string result="NA";
            if (e.Key == Key.Enter)
            {
                if (textBox1.Text.ToString().Trim().Length == 7)
                {
                    if (CheckBoxExist(textBox1.Text.ToString().Trim().ToUpper()))
                    {
                        box = textBox1.Text.ToString().Trim().ToUpper();
                        if (Mode != 5) getPOInfo(strPONumber);
                        if (Mode == 3 || Mode == 6)
                        {
                            if (!CheckBoxRework(strPONumber, box, ref result))
                            {
                                MessageBox.Show(result, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                textBox1.Focus();
                                textBox1.SelectAll();
                                return;
                            }
                        }

                        WareHouseSerials aPrintPackSerials = new WareHouseSerials(this);
                        if (Mode != 5)
                        {
                            Nullable<bool> printboxresults = aPrintPackSerials.ShowDialog();
                        }
                        Init();
                    }
                    else
                    {
                        MessageBox.Show("Box Number này không tồn tại");
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                }
            }
            else if (e.Key == Key.Escape)
            {
                Init();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBox2.Text = textBox2.Text.Trim().ToString().ToUpper();
                textBox3.Focus();
                textBox3.SelectAll();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBox3.Text = textBox3.Text.Trim().ToString().ToUpper();
                textBox4.Focus();
                textBox4.SelectAll();
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                textBox4.Text = textBox4.Text.Trim().ToString().ToUpper();
                button11.Focus();
            }
        }
        #endregion

        #region Function
        private bool CheckBoxExist(string BoxNumber)
        {
            using (sqlConnection4 = new SqlConnection(strSqlConnection4))
            {
                try
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_CheckBoxExist";
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    if (rec["Result"].ToString().Trim().Equals("OK"))
                    {
                        rec.NextResult();
                        rec.Read();
                        strPONumber = rec["PONumber"].ToString().Trim();
                        sqlConnection4.Close();
                        return true;
                    }
                    else
                    {
                        sqlConnection4.Close();
                        return false;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }

        private string getDateTime()
        {
            string result = "NG";
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4))
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

        private string getPOUnPack()
        {
            string result = "NG";
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4))
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getPOUnPack";
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    result = rec["Result"].ToString().Trim();
                    sqlConnection4.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error get PO Un Pack:" + ex.Message);
            }
            return result;
        }

        public void getPOInfo(string PONumber)
        {
            using (sqlConnection4 = new SqlConnection(strSqlConnection4))
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

        

        public void getPrintFilesLabel(string ProductMap)
        {
            //add cho 1 loai nhan thoi
            //do get file
            strPrintCustomer = strPrintFFCCustomerVN;
            strPrintOverPackContent = strPrintFFCOverPackContent;
            strPrintOverPack = strPrintFFCOverPack;
            return;

            //phan cu cua gau
            switch (ProductMap)
            {
                #region caseFFC
                case "FFC":
                    strPrintOverPackContent = strPrintFFCOverPackContentVN;
                    strPrintOverPack = strPrintFFCOverPack;
                    break;
                #endregion
                #region caseDLM
                case "DLM":
                    strPrintOverPackContent = strPrintDLMOverPackContent;
                    strPrintOverPack = strPrintDLMOverPack;
                    strPrintOverPackVN = strPrintDLMOverPackVN;
                    break;
                #endregion
                #region caseFRUwACS
                case "FRUwACS":
                    strPrintOverPackContent = strPrintFRUwACSOverPackContent;
                    break;
                #endregion
                #region caseFRUwoACS
                case "FRUwoACS":
                    strPrintOverPackContent = strPrintFRUwoACSOverPackContent;
                    break;
                #endregion
                #region caseBASE
                case "BASE":
                    strPrintOverPackContent = strPrintBASEOverPackContent;
                    strPrintOverPack = strPrintBASEOverPack;
                    break;
                #endregion
                #region caseSMTwACS
                case "SMTwACS":
                    strPrintOverPackContent = strPrintSMTwACSOverPackContent;
                    break;
                #endregion
                #region caseSMTwoACS
                case "SMTwoACS":
                    strPrintOverPackContent = strPrintSMTwoACSOverPackContent;
                    break;
                #endregion
            }
        }

        public string PrintBoxLabel(WareHouse myMain, int iAmt)
        {
            SAPXML mySX;
            SAPPost sp;
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList oNodes;
            string strDateTimeholder;
            string strModelDescription = "";

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
                    strModelDescription = getSAPDescription(myMain.strPOMaterial.Trim());
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
                myMain.btLabel.findFieldandSubstitute(2, "ZDATE", getDateTime());
                myMain.btLabel.findFieldandSubstitute(2, "BOX", box);

                myMain.btLabel.doPrint(2, false, false);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Printing Box Label=" + ex.Message);
            }
            return "OK";
        }

        public void PrintBoxManual(string SAPModel, string Quantity, string Copied)
        {
            try
            {
                btLabel.setPrintFile(5, strPrintSmallBox);
                btLabel.findFieldandSubstitute(5, "SPART", SAPModel);
                btLabel.findFieldandSubstitute(5, "QTY", Quantity);
                if (checkBox1.IsChecked==true) btLabel.findFieldandSubstitute(5, "COUNTRY", "MADE IN VIETNAM");
                else btLabel.findFieldandSubstitute(5, "COUNTRY", "");
                btLabel.findFieldandSubstitute(5, "ZDATE", getDateTime());
                //btLabel.setPrintFileCopies(5,Copied);
                
                btLabel.doPrint(5, false, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Printing Box Label=" + ex.Message);
            }
        }

        private bool CheckBoxRework(string PONUmber, string BoxNumber, ref string result)
        {
            using (sqlConnection4 = new SqlConnection(strSqlConnection4))
            {
                sqlConnection4.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlConnection4;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ame_CheckBoxRework";
                cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                cmd.Parameters["@PONumber"].Value = PONUmber;
                cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                cmd.Parameters.Add("@BoxRework", SqlDbType.Char, 30);
                cmd.Parameters["@BoxRework"].Value = BoxNumber;
                cmd.Parameters["@BoxRework"].Direction = ParameterDirection.Input;
                SqlDataReader rec = cmd.ExecuteReader();
                rec.Read();
                if (rec["Result"].ToString().Trim().Equals("OK"))
                {
                    result = "OK";
                    sqlConnection4.Close();
                    return true;
                }
                else if (rec["Result"].ToString().Trim().Equals("NG"))
                {
                    result = "Không tồn tại Box này";
                    sqlConnection4.Close();
                    return false;
                }
                else
                {
                    result = rec["Result"].ToString().Trim();
                    sqlConnection4.Close();
                    return false;
                }
            }
        }

        private bool CheckReportBoxNumberExist()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Packing.xlsx";
            if (File.Exists(path)) return true;
            else return false;
        }

        private DataTable GetReportUnPackingParts()
        {
            DataTable dt = new DataTable();
            using (sqlConnection4 = new SqlConnection(strSqlConnection4))
            {
                try
                {
                    sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_GetUnPackingParts";
                    SqlDataAdapter rec = new SqlDataAdapter(cmd);
                    rec.Fill(dt);
                    sqlConnection4.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
        }

        private void DoReportUnPackingParts()
        {
            DataTable dt = new DataTable();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\UnPackingParts.xlsx";
            File.Copy(strReportUnPackingParts, path, true);
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path +
                ";Extended Properties=\"Excel 8.0;HDR=Yes\";";
            OleDbConnection con = new OleDbConnection(connectionString);

            dt = GetReportUnPackingParts();

            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    string[] temp = new string[3];
                    byte j = 0;

                    foreach (var item in row.ItemArray) // Loop over the items.
                    {
                        temp[j] = item.ToString();
                        j++;
                    }
                    string PONumber = temp[0].Trim();
                    string Model = temp[1].Trim();
                    string Serial = temp[2].Trim();
                    string selectString = "Insert into [Sheet1$] ([PONumber],[Model],[Serial]) " +
                        "values (@PONumber,@Model,@Serial)";
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@PONumber", OleDbType.Char).Value = PONumber;
                    cmd.Parameters.Add("@Model", OleDbType.Char).Value = Model;
                    cmd.Parameters.Add("@Serial", OleDbType.Char).Value = Serial;
                    cmd.CommandText = selectString;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                MessageBox.Show("Đã export thành công, file UnPackingParts.xlsx ngoài desktop");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string getSAPDescription(string SAPModel)
        {
            string result = "NG";
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4))
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
        }

        private void writeSAPDescription(string SAPModel, string Description)
        {
            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4))
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
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrmCombineBox frm = new FrmCombineBox();
            frm.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FrmTinforNPackingSNProcessing frm = new FrmTinforNPackingSNProcessing();
            frm.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

      

       

      

       

        
    }
}
