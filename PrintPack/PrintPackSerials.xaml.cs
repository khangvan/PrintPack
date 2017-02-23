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
using System.Threading;
//using BartenderLibrary;
using System.Configuration;
using PrintPack;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using ACSEE.NET;


namespace PrintPack
{
    /// <summary>
    /// Interaction logic for PrintPackSerials.xaml
    /// </summary>
    public partial class PrintPackSerials : Window
    {
        public MainWindow objMainWindow;

        public PrintPackSerials()
        {
            InitializeComponent();
        }

        public PrintPackSerials(MainWindow objMyMainWindow)
            : this()
        {
            objMainWindow = objMyMainWindow;

            DSPackedSN = new List<clsSerialInput>();
            if (objMainWindow.bolRePrint == false)
            {
                objMainWindow.dataExist = false;

                if (objMainWindow.strPORev.Equals("N/A"))
                {
                    label1.Content = "Enter Model-Serial";
                }
                else
                {
                    if (objMainWindow.strProductMap.Equals("BASE"))
                    {
                        label1.Content = "Enter " + objMainWindow.strPOMaterial + "-Serial  Revision: " + objMainWindow.strPORev;
                    }
                    else
                    {
                        label1.Content = "Enter " + objMainWindow.strPOMaterial + "- Revision: " + objMainWindow.strPORev;
                    }
                }
                if (!objMainWindow.boxRework)
                {
                    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
                    {
                        #region ame_checkboxnumber
                        try
                        {
                            objMainWindow.sqlConnection4.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = objMainWindow.sqlConnection4;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "ame_CheckBoxNumber";
                            SqlDataReader rec = cmd.ExecuteReader();
                            rec.Read();
                            string result = rec["Result"].ToString().Trim();
                            if (result.Equals("OK"))
                            {
                                rec.NextResult();
                                rec.Read();
                                objMainWindow.pyear = rec["PYear"].ToString().Trim();
                            }
                            else
                            {
                                MessageBox.Show("Vượt quá 1000 thùng 1 ngày, liên hệ kỹ sư", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            objMainWindow.sqlConnection4.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error Check Box Number:" + ex.Message);
                        }
                        #endregion
                    }
                    string strCurrentbox = objMainWindow.box.Trim();
                    string strNewbox= Do_GetNewBOXNumber();

                    if (strNewbox == strCurrentbox)
                    {
                        //re get new box
                        strNewbox = Do_GetNewBOXNumber();
                    }
                    else
                    { 
                         
                    }

                    objMainWindow.box = strNewbox.Trim();

                }
                if (objMainWindow.strProductMap.Equals("FRUwoACS"))
                {
                    if (GetPartRun2(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box) == false)
                    {
                        MessageBox.Show("PO Completed");
                    }
                }
                else
                {
                    TraceStepDoing("Update so luong PO cho CN");
                    //if (GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box) == false)
                    if (GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box) == false)
                    {
                        MessageBox.Show("PO Completed: " + objMainWindow.intPOPacked);

                    }
                }
                TraceStepDoing("Lay box thanh cong _show box" + objMainWindow.box);
                label2.Content = "BOX: " + objMainWindow.box;
            }
        }

        private string Do_GetNewBOXNumber()
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    
                    string strNewbox = "NA";


                    TraceStepDoing("Step_Lay box number");


                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_GetBoxNumber";
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    //objMainWindow.box = rec["BoxNumber"].ToString().Trim();
                    strNewbox=rec["BoxNumber"].ToString().Trim();
                    

                    objMainWindow.sqlConnection4.Close();
                    return strNewbox;

                }
                   
                catch (SqlException ex)
                {
                    MessageBox.Show("Error Get Box Number:" + ex.Message);
                    return "NA";
                }
            }
        }


        //public static string stringGetboxnumber()
        //{
        //    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
        //    {
        //        try
        //        {


        //            TraceStepDoing("Step_Lay box number");

        //            objMainWindow.sqlConnection4.Open();
        //            SqlCommand cmd = new SqlCommand();
        //            cmd.Connection = objMainWindow.sqlConnection4;
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandText = "ame_GetBoxNumber";
        //            SqlDataReader rec = cmd.ExecuteReader();
        //            rec.Read();
        //            stringGetboxnumber = rec["BoxNumber"].ToString().Trim();
        //            objMainWindow.sqlConnection4.Close();
        //        }
        //        catch (SqlException ex)
        //        {
        //            MessageBox.Show("Error Get Box Number:" + ex.Message);
        //        }
        //    }
        //}
        private static void TraceStepDoing(string strTrace)
        {
#if DEBUG

            Console.WriteLine(strTrace);
#endif
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ucSerialNumber firstSerial = new ucSerialNumber();

            if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
            {
                string strEnglishphrase = "";
                string strForeignphrase = "";
                button2.Content += " (" + objMainWindow.getForeignPhrase("PRINT", ref strEnglishphrase, ref strForeignphrase) + ")";

                //                MessageBox.Show("Serial number already entered! (" + objMainWindow.getForeignPhrase("SERIALNUMBERALREADY", ref strEnglishphrase, ref strForeignphrase) + ")");
            }



            for (int i = 1; i <= objMainWindow.iMaxSerialsPerOrder; i++)
            {
                ucSerialNumber aSerial = new ucSerialNumber();

                aSerial.label2.Content = i.ToString();
                aSerial.TabIndex = i;
                aSerial.textBox1.Text = " ";
                //  aSerial.textBox1.Text = i.ToString();
                aSerial.textBox1.GotKeyboardFocus += TextBoxGotKeyboardFocus;
                aSerial.textBox1.LostKeyboardFocus += TextBoxLostKeyboardFocus;
                aSerial.SerialItemComplete += this.HandleSerialNumberEntered;
                //aSerial.IsEnabled = false;

                this.stackPanel1.Children.Add(aSerial);

                if (i == 1)
                {
                    //aSerial.IsEnabled = true;
                    firstSerial = aSerial;
                    //                    Keyboard.Focus(aSerial);
                    FocusHelper.Focus(firstSerial.textBox1);
                }

            }
            //            bool bFocus = firstSerial.Focus();
        }

        private void TextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox source = e.Source as TextBox;

            if (source != null)
            {
                // Change the TextBox color when it obtains focus.
                //source.IsEnabled = true;
                source.Background = Brushes.LightBlue;
                source.SelectAll();
                // Clear the TextBox.
            }
        }

        private void TextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox source = e.Source as TextBox;

            if (source != null)
            {
                // Change the TextBox color when it loses focus.
                //source.IsEnabled = false;
                source.Background = Brushes.White;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }

        public bool IsSerialAlreadyEntered(string strLastSerial, ucSerialNumber enteredSerial)
        {
            int i = 0;
            try
            {
                for (i = 0; i < objMainWindow.iMaxSerialsPerOrder; i++)
                {
                    ucSerialNumber aSerialEntry = (ucSerialNumber)(this.stackPanel1.Children[i]);
                    if ((aSerialEntry.textBox1.Text.ToString().Trim().Length > 0) && (aSerialEntry != enteredSerial))
                    {
                        if (aSerialEntry.textBox1.Text.ToString().Trim().Equals(strLastSerial))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public bool IsSerialBelongProdorder(string strLastSerial, string isbelongProdOrdercheck)
        {
            int i = 0;
            try
            {
                objMainWindow.sqlConnection4.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = objMainWindow.sqlConnection4;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "ame_GetProdOrderfromSN";
                cmd.Parameters.Add("@@Serialnumberinput", SqlDbType.Char, 15);
                cmd.Parameters["@@Serialnumberinput"].Value = strLastSerial;



                cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@Return_Value", SqlDbType.Int, 4);
                cmd.Parameters["@Return_Value"].Direction = ParameterDirection.ReturnValue;

                SqlDataReader rec = cmd.ExecuteReader();
                //objMainWindow.intPORun = Int32.Parse(rec["@Return_Value"].ToString());
                string GetProdOrder = cmd.Parameters["@ProdOrder"].Value.ToString();

                if (GetProdOrder == isbelongProdOrdercheck)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public bool IsSerialBelongProdorder_inmemory(string strLastSerial)
        {
            int i = 0;
            bool checkresult = false;
            try
            {
                foreach (var pair in objMainWindow.dictPOInformation)
                {

                    if (strLastSerial.Trim() == pair.Key)
                    {
#if DEBUG
                        Console.WriteLine("YES: {0}", pair.Key);
#endif
                        checkresult = true;
                        return checkresult;
                    }
                    else
                    {
                        checkresult = false;
                    }

                }
                return checkresult;






            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public bool IsSerialModel(string Model, string Serial, ref string Return)
        {
            using (objMainWindow.sqlConnection1 = new SqlConnection(objMainWindow.strSqlConnection1))
            {
                try
                {
                    objMainWindow.sqlConnection1.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection1;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "user_check_SerialModel";
                    cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                    cmd.Parameters["@Model"].Value = Model;
                    cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                    cmd.Parameters["@Serial"].Value = Serial;
                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    string temp = rec["Result"].ToString().Trim();
                    rec.NextResult();
                    rec.Read();
                    //string temp1 = rec["TestDateTime"].ToString().Trim();
                    DateTime Testdate = new DateTime();
                    DateTime.TryParse(rec["TestDateTime"].ToString(), out Testdate);
                    switch (temp)
                    {
                        case "NoData":
                            Return = "No data on ACS";
                            return false;
                        case "ModelIncorrect":
                            Return = "Model and Serial are not match";
                            return false;
                        case "Fail":
                            Return = "Kết quả test : FAIL";
                            return false;
                        case "OK":
                            if (IsDateTestVerify(objMainWindow.TestRequestDate,Testdate))
                            {
                                Return = "OK";
                                return true;
                            }
                            else
                            {
                                Return = string.Format("NG-Need to retest again \r\n TestRequestDate: {0} \r\n Actual Test Date: {1}", objMainWindow.TestRequestDate, Testdate);
                                return false;
                            }
                            
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }

        public bool IsDateTestVerify(DateTime requesdate, DateTime testdate)
        {
            TimeSpan difference = testdate - requesdate;
            if (difference.TotalDays > 0)
            {
                // Bingo!
                return true;
            }
            else
            {

                return false;
            }

        }

        public bool IsSerialAlreadyBox(Boolean Rework, string strBoxRework, string PONumber, string Model,
            string strLastSerial, ref string box, ref string po)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!Rework)
                    {
                        cmd.CommandText = "ame_CheckSerial";
                        cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                        cmd.Parameters["@PONumber"].Value = PONumber;
                        cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                        cmd.Parameters["@Model"].Value = Model;
                        cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                        cmd.Parameters["@Serial"].Value = strLastSerial;
                        cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    }
                    else
                    {
                        cmd.CommandText = "ame_CheckSerialRework";
                        cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                        cmd.Parameters["@PONumber"].Value = PONumber;
                        cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                        cmd.Parameters["@Model"].Value = Model;
                        cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                        cmd.Parameters["@Serial"].Value = strLastSerial;
                        cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("@BoxRework", SqlDbType.Char, 30);
                        cmd.Parameters["@BoxRework"].Value = strBoxRework;
                        cmd.Parameters["@BoxRework"].Direction = ParameterDirection.Input;
                    }
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    if (rec["Result"].ToString().Trim().Equals("OK"))
                    {
                        objMainWindow.sqlConnection4.Close();
                        return false;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("NG"))
                    {
                        rec.NextResult();
                        rec.Read();
                        box = rec["Box"].ToString().Trim();
                        po = "NA";
                        //MessageBox.Show("Đã packing trong box: "+box);

                        PrintPack.FrmMessageBox frm = new FrmMessageBox("Đã packing trong box: " + box);
                        frm.ShowDialog();
                        objMainWindow.sqlConnection4.Close();
                        return true;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("NGP"))
                    {
                        rec.NextResult();
                        rec.Read();
                        box = rec["Box"].ToString().Trim();
                        po = rec["PO"].ToString().Trim();
                        //MessageBox.Show("Đã packing trong box: " + box);
                        PrintPack.FrmMessageBox frm = new FrmMessageBox("Đã packing trong box: " + box);
                        frm.ShowDialog();
                        objMainWindow.sqlConnection4.Close();
                        return true;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("Dang Packing"))
                    {
                        rec.NextResult();
                        rec.Read();
                        box = rec["Box"].ToString().Trim();
                        po = rec["PO"].ToString().Trim();
                        objMainWindow.sqlConnection4.Close();
                        PrintPack.FrmMessageBox frm = new FrmMessageBox("Ðang packing trong box: " + box);
                        frm.ShowDialog();
                        return true;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return true;
        }

        public void LockSerial(string PONumber, string Model, string Serial, string BoxNumber, string PYear)
        {


            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_tmpPackingRecord";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = PONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                    cmd.Parameters["@Model"].Value = Model;
                    cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                    cmd.Parameters["@Serial"].Value = Serial;
                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@PYear", SqlDbType.Char, 30);
                    cmd.Parameters["@PYear"].Value = PYear;
                    cmd.Parameters["@PYear"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                    SetDataExist();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SetDataExist()
        {
            objMainWindow.dataExist = true;
        }



        public void DoUpdate1SNfromtemp2PackingRecord(string PONumber, string Model, string Serial, string BoxNumber, string PYear, string PAckingStation)
        {


            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_tmp2PackingRecord";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = PONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                    cmd.Parameters["@Model"].Value = Model;
                    cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                    cmd.Parameters["@Serial"].Value = Serial;
                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@PYear", SqlDbType.Char, 30);
                    cmd.Parameters["@PYear"].Value = PYear;
                    cmd.Parameters["@PYear"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                    objMainWindow.dataExist = true;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void DoUpdate1SN2_tempPackingRecord(string PONumber, string Model, string Serial, string BoxNumber, string PYear, string PAckingStation)
        {


            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_tmpPackingRecord";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = PONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                    cmd.Parameters["@Model"].Value = Model;
                    cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                    cmd.Parameters["@Serial"].Value = Serial;
                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@PYear", SqlDbType.Char, 30);
                    cmd.Parameters["@PYear"].Value = PYear;
                    cmd.Parameters["@PYear"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                    objMainWindow.dataExist = true;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        /// <summary>
        /// Update số lượng đóng gói/ tổng số lượng PO
        /// </summary>
        /// <param name="PONumber"></param>
        /// <param name="Rework"></param>
        /// <param name="BoxNumber"></param>
        /// <returns></returns>
        public bool GetPartRun(string PONumber, Boolean Rework, string BoxNumber)
        {


            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getPartRun";
                    cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                    cmd.Parameters["@ProdOrder"].Value = PONumber;
                    cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Rework", SqlDbType.Char, 1);
                    if (Rework) cmd.Parameters["@Rework"].Value = "1";
                    else cmd.Parameters["@Rework"].Value = "0";
                    cmd.Parameters["@Rework"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Return_Value", SqlDbType.Int, 4);
                    cmd.Parameters["@Return_Value"].Direction = ParameterDirection.ReturnValue;
                    //SqlDataReader rec = cmd.ExecuteReader();
                    ////objMainWindow.intPORun = Int32.Parse(rec["@Return_Value"].ToString());
                    //objMainWindow.intPORun = Int32.Parse(cmd.Parameters["@Return_Value"].Value.ToString());

                    //label3.Content = "PACKED: " + objMainWindow.intPORun + "/" + objMainWindow.intPOQuantity;

                    label3.Content = "PACKED: " + objMainWindow.intPOPacked + "/" + objMainWindow.intPOQuantity;

                    objMainWindow.sqlConnection4.Close();
                    if (objMainWindow.intPOPacked < objMainWindow.intPOQuantity) return true;
                    else return false;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }

        private void UpdateQtyPackedInformation()
        {
            label3.Content = "PACKED: " + objMainWindow.intPORun + "/" + objMainWindow.intPOQuantity;
        }

        public bool GetPartRun2(string PONumber, Boolean Rework, string BoxNumber)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getPartRun2";
                    cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                    cmd.Parameters["@ProdOrder"].Value = PONumber;
                    cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Rework", SqlDbType.Char, 1);
                    if (Rework) cmd.Parameters["@Rework"].Value = "1";
                    else cmd.Parameters["@Rework"].Value = "0";
                    cmd.Parameters["@Rework"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Return_Value", SqlDbType.Int, 4);
                    cmd.Parameters["@Return_Value"].Direction = ParameterDirection.ReturnValue;
                    SqlDataReader rec = cmd.ExecuteReader();
                    //objMainWindow.intPORun = Int32.Parse(rec["@Return_Value"].ToString());
                    objMainWindow.intPORun = Int32.Parse(cmd.Parameters["@Return_Value"].Value.ToString());

                    label3.Content = "PACKED: " + objMainWindow.intPORun + "/" + objMainWindow.intPOQuantity;
                    objMainWindow.sqlConnection4.Close();
                    if (objMainWindow.intPORun < objMainWindow.intPOQuantity) return true;
                    else return false;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }



        public void UpdatePartRun2TInformation(int value)
        {

            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_UpdatePartRun2TInformation";
                    cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                    cmd.Parameters["@ProdOrder"].Value = objMainWindow.strPONumber;
                    cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;

                    cmd.Parameters.Add("@result", SqlDbType.Int);
                    cmd.Parameters["@result"].Value = value;
                    cmd.Parameters["@result"].Direction = ParameterDirection.Input;

                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void UpdatePartRun()
        {

            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_updatePartRun";
                    cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                    cmd.Parameters["@ProdOrder"].Value = objMainWindow.strPONumber;
                    cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void UpdatePartRun2()
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_updatePartRun2";
                    cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                    cmd.Parameters["@ProdOrder"].Value = objMainWindow.strPONumber;
                    cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #region NotUsed
        public string GetLoci(string strACSSerial, out string strORTStatus,
           out string strORTBin, out string strORTStart, out string strPSCSN,
            out string strAssmSN, out string strSAPModel, out long lSieveByte,
            out long lUnitStnByte, out long lLineByte,
            out long lPlantByte, out string strNext_Station)
        {

            string strResult = "";

            strORTStatus = "N";
            strORTBin = "";
            strORTStart = "";
            strPSCSN = "";
            strAssmSN = "";
            strSAPModel = "";
            lSieveByte = 0;
            lUnitStnByte = 0;
            lLineByte = 0;
            lPlantByte = 0;
            // strStation = "";
            strNext_Station = "";
            /*
                        SqlConnection sqlConnectionACSEEState;

                        try
                        {
                            using (sqlConnectionACSEEState = new SqlConnection(frmMainForm.strSqlConnection3))
                            {
                                sqlConnectionACSEEState.Open();
                                if (sqlConnectionACSEEState.State.Equals(ConnectionState.Open))
                                {
                                    try
                                    {
                                        SqlCommand cmdGetLoci = sqlConnectionACSEEState.CreateCommand();
                                        cmdGetLoci.CommandType = CommandType.StoredProcedure;
                                        cmdGetLoci.CommandText = "ame_get_loci";


                                        cmdGetLoci.Parameters.Add("@acssn", SqlDbType.Char, 20);
                                        cmdGetLoci.Parameters["@acssn"].Value = strACSSerial;
                                        cmdGetLoci.Parameters["@acssn"].Direction = ParameterDirection.Input;

                                        using (SqlDataReader rd = cmdGetLoci.ExecuteReader())
                                        {
                                            if (rd.HasRows)
                                            {
                                                rd.Read();

                                                strResult = rd[0].ToString();
                                                if (strResult.Trim().Equals("OK"))
                                                {
                                                    rd.NextResult() ;
                                                    rd.Read();
                                                    string tryit = rd["SAP_Model"].ToString().Trim();
                                                    strORTStatus = rd["ORT_Status"].ToString().Trim() ;
                                                    strORTBin = rd["ORT_Bin"].ToString().Trim() ;
                                                    strORTStart = rd["ORT_Start"].ToString().Trim() ;
                                                    strPSCSN = rd["PSC_Serial"].ToString().Trim() ;
    
                                                    strAssmSN = rd["Assembly_ACSSN"].ToString().Trim() ;
                                                    strSAPModel = rd["SAP_Model"].ToString().Trim() ;
                                                    lSieveByte = Int32.Parse(rd["SieveByte"].ToString()) ;
                                                    lUnitStnByte = Int32.Parse(rd["UnitStnByte"].ToString()) ;
                                                    lLineByte = Int32.Parse(rd["LineByte"].ToString() ) ;
                                                    lPlantByte = Int32.Parse(rd["PlantByte"].ToString()) ;
                                                    strNext_Station = rd["Next_Station_Name"].ToString().Trim();
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        return strResult;
             */

            return "OK";
        }

        public string CheckDBLoci(string strPSCSN)
        {
            /*
                                    brework = false;
                        SqlConnection sqlConnectionState;
                        bool bCorrectStation = false;


                        string strORTStatus;
                        string strORTBin;
                        string strORTStart;
                        string strPSCSN;
                        string strAssmSN;
                        string strSAPModel;
                        long lSieveByte;
                        long lUnitStnByte;
                        long lLineByte;
                        long lPlantByte;
                        string strNext_Station = "";
                        using (sqlConnectionState = new SqlConnection(strSqlConnection))
                        {
                            sqlConnectionState.Open();
                            if (sqlConnectionState.State.Equals(ConnectionState.Open))
                            {
                                try
                                {

                                    IEnumerable<DataRow> acsquery =
                from kicker in dtKickerTable.AsEnumerable()
                where kicker.Field<string>("TFFC_KICKER_Model").Trim() == strModel.Trim()
                select kicker;

                                    DataTable dtResult = acsquery.CopyToDataTable<DataRow>();
                                    string strCurrentStation = dtResult.Rows[0]["TFFC_KICKER_Station"].ToString();
                                    if (dtResult.Rows.Count > 0)
                                    {
            //                            string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\\\svksrv722.psc.pscnet.com\\prd\\dldb\\NetPro\\Tests.mdb;Persist Security Info=True;Jet OLEDB:Database Password=callisto";

                                        string strAccessConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" ; //\\\\svksrv722.psc.pscnet.com\\prd\\dldb\\NetPro\\Tests.mdb;Persist Security Info=True;Jet OLEDB:Database Password=callisto";
                                        strAccessConn += dtResult.Rows[0]["TFFC_KICKER_DBPath"].ToString().Trim();
                                        strAccessConn += ";Persist Security Info=True;";

                                        string strStationQuery = "SELECT Station_Name, Machine_Name, FactoryGroup_Mask, ProductGroup_Mask, Order_Value, Perform_Test from [STATIONS]";
                                        OleDbConnection myAccessConn = null;
                                        try
                                        {
                                            myAccessConn = new OleDbConnection(strAccessConn);
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);
                                        }

                                        DataSet dsStations = new DataSet();

                                        OleDbCommand myAccessCommand = new OleDbCommand(strStationQuery, myAccessConn);
                                        OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);

                                        myAccessConn.Open();
                                        myDataAdapter.Fill(dsStations, "STATIONS");
                            
                                        DataTable dtStations = dsStations.Tables[0];
                                        for (int i = 0; i < dtStations.Rows.Count; i++)
                                        {
                                            clsStationInfo objStationInfo = new clsStationInfo();
                                            objStationInfo.strStationName = dtStations.Rows[i]["Station_Name"].ToString();
                                            objStationInfo.strMachineName = dtStations.Rows[i]["Machine_Name"].ToString();
                                            objStationInfo.iFactoryGroup_Mask = Int32.Parse(dtStations.Rows[i]["FactoryGroup_Mask"].ToString()) ;
                                            objStationInfo.iProductGroup_Mask = Int32.Parse(dtStations.Rows[i]["ProductGroup_Mask"].ToString());
                                            objStationInfo.iOrder_Value = Int32.Parse(dtStations.Rows[i]["Order_Value"].ToString()) ;
                                            objStationInfo.strPerform_Test = dtStations.Rows[i]["Perform_Test"].ToString();
                                            frmMainForm.listStations.Add(objStationInfo);
                                        }

                                        myAccessConn.Close();



                                        string strLociReturn;

                                        strLociReturn = GetLoci(strSerial, out strORTStatus, out strORTBin,
                                            out strORTStart, out strPSCSN, out strAssmSN,
                                            out strSAPModel, out lSieveByte, out lUnitStnByte,
                                            out lLineByte, out lPlantByte, out strNext_Station);


                                        if (Char.IsNumber((char)(strNext_Station[strNext_Station.Length - 1])) == true)
                                        {
                                            strNext_Station = strNext_Station.Substring(0, strNext_Station.Length - 1);
                                        }

                          

                                        if (Char.IsNumber((char)(strStation[strStation.Length - 1])) == true)
                                        {
                                            strStation = strStation.Substring(0, strStation.Length - 1);
                                        }

                                        if ( strNext_Station.Trim().Equals(strNext_Station.Trim()))
                                        {
                                            return "OK" ;
                                        }

                                        clsStationInfo objCurrentStation = new clsStationInfo();
                                        for (int k = 0; k < frmMainForm.listStations.Count; k++)
                                        {
                                            if (frmMainForm.listStations[k].strStationName.Trim().Equals(strStation))
                                            {
                                                objCurrentStation.iProductGroup_Mask = frmMainForm.listStations[k].iProductGroup_Mask;
                                                objCurrentStation.iOrder_Value = frmMainForm.listStations[k].iOrder_Value;
                                            }
                                        }

                                        for (int j = 0; j < frmMainForm.listStations.Count; j++)
                                        {
                                            string strTempStn;

                                            strTempStn = frmMainForm.listStations[j].strStationName.Trim();
                                            if (Char.IsNumber((char)(strTempStn[strTempStn.Length - 1])) == true)
                                            {
                                                strTempStn = strTempStn.Substring(0, strTempStn.Length - 1);
                                            }
                                            if (strTempStn.Trim().Equals(strNext_Station))
                                            {
                                                if ((objCurrentStation.iProductGroup_Mask & frmMainForm.listStations[j].iProductGroup_Mask) >0 )
                                                {
                                                    if (objCurrentStation.iOrder_Value <= frmMainForm.listStations[j].iOrder_Value)
                                                    {
                                                        bCorrectStation = true;
                                                        if (objCurrentStation.iOrder_Value < frmMainForm.listStations[j].iOrder_Value)
                                                        {
                                                            brework = true;

                                                        }
                                                        return "OK";
                                                    }
                                                }

                                            }



                                        }


                                    }
                                    else
                                    {
                                        MessageBox.Show("No rows in Kicker for NextStationLoci");
                                    }

                    


                        



                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }


            */




            return "OK";
        }
        #endregion

        public static int ReturnCharFound(string strchuoiinput, string findvalue)
        {
            return strchuoiinput.IndexOf(findvalue);
        }
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        private string Check_Model_SN_by_Conditions(string str_i_input, ref string pn, ref string sn)
        {
            //string result = "NG";
            try
            {
                #region Check PartNumber to make sure PN and PN-SN correct
                ///if PN not correct then end
                ///ReturnCharFound(ReverseString(txtSNinput.Text.Trim()), "-"); => l?y ký t? s? sn t? các ký t? cu?i
                ///
                int aget = ReturnCharFound(ReverseString(str_i_input), "-");

                int astart = str_i_input.Length - aget;
                int bstart = str_i_input.Length - astart;

                string strPNtocheck = str_i_input.Substring(0, astart - 1);
                //pn = strPNtocheck;
                string strSNtocheck = str_i_input.Substring(astart, aget);
                sn = strSNtocheck;

                if (strPNtocheck.ToUpper() != pn.ToUpper())
                {
                    return " Model không đúng ! Kiem tra: " + strPNtocheck;
                }

                //if (strSNtocheck.Length != SNlength)
                //{
                //    return " SN không d? ho?c du ký t? ! Ki?m tra s? SN: " + strSNtocheck;
                //}

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();

            }

                #endregion
        }

        private static string CheckIfValueExistInArray(string stringToCheck, string[] stringArray)
        {
            string kq = "OK";
            //string stringToCheck = "GHI";
            //string[] stringArray = { "ABC", "DEF", "GHI", "JKL" };
            foreach (string x in stringArray)
            {

                if (x.Equals(stringToCheck))
                {
                    MessageBox.Show("Tìm thấy số Serial trùng ..." + x);
                    //CheckIfValueExistInArray = 

                    kq = "Trung so !";
                }

            }
            return kq;
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
        public string strinputway = "";
        public int QtyPackedInbox = 0;





        public void AddSNvaoDS(string PONumber, string Model, string Serial, string BoxNumber, string PYear, string PackingStation)
        {
            clsSerialInput SNinput = new clsSerialInput();
            SNinput.Order = PONumber;
            SNinput.Partnumber = Model;
            SNinput.Serial = Serial;
            SNinput.BoxNo = BoxNumber;
            SNinput.Packingdate = PYear;
            SNinput.PackStation = PackingStation;
            DSPackedSN.Add(SNinput);
        }

        public void DoUpdateDSPackedSN2PackingTable()
        {
            foreach (clsSerialInput dong in DSPackedSN)
            {
                string PONumber = dong.Order;
                string Model = dong.Partnumber;
                string Serial = dong.Serial;
                string BoxNumber = dong.BoxNo;
                string PYear = dong.Packingdate;
                string PackingStation = dong.PackStation;

                DoUpdate1SNfromtemp2PackingRecord(PONumber, Model, Serial, BoxNumber, PYear, PackingStation);


            }

            //uddate T_information


        }
        public void DoUpdateDSPackedSN2_tempPackingTable()
        {
            foreach (clsSerialInput dong in DSPackedSN)
            {
                string PONumber = dong.Order;
                string Model = dong.Partnumber;
                string Serial = dong.Serial;
                string BoxNumber = dong.BoxNo;
                string PYear = dong.Packingdate;
                string PackingStation = dong.PackStation;

                DoUpdate1SN2_tempPackingRecord(PONumber, Model, Serial, BoxNumber, PYear, PackingStation);


            }

            //uddate T_information


        }

        public List<clsSerialInput> DSPackedSN = new List<clsSerialInput>();


        public void HandleSerialNumberEntered(object sender, EventArgs e)
        {

            string strSerialNumberEntered;
            string strModelEntered;
            string strJustSerialNumber;
            ucSerialNumber mySerial;
            ucSerialNumber mySerialProdOrder;
            Boolean temp;

            Boolean ischecktestrecord = true;
            Boolean ischeckpacked = true;
            Boolean ischeckbelogProdOrder = true;




            #region Lay cac yeu cau kiem tra cho sf

            ConditiontoPackingVerify KiemSoatTheoProductMap = new ConditiontoPackingVerify();
            KiemSoatTheoProductMap.GetProductMap(objMainWindow.strProductMap);


            KiemSoatTheoProductMap.strInputMode = objMainWindow.strModeChooseFromGroupbox;
            strinputway = KiemSoatTheoProductMap.strInputMode;



            #endregion


            try
            {


                mySerial = (ucSerialNumber)sender;
                int iIndex = mySerial.TabIndex;
                string boxexist = "NA";
                string poexist = "NA";
                strSerialNumberEntered = mySerial.textBox1.Text.ToString().Trim().ToUpper();


                // if data input empty => do update data
                if ((strSerialNumberEntered.Trim().Length == 0) || (strSerialNumberEntered.Trim().Equals("=")))
                {
                    
                    mySerial.textBox1.Text = "";
                    if (objMainWindow.dataExist)
                    {
                        UpdateDataThenDoPrint();
                    }
                    else
                    {
                        ClosePrintPackSerials();
                    }
                }
                else // do check 
                {
                    goto newversion; //oldversion;
                //goto oldversion;
                newversion:
                    #region KhangAdd 27 July

                    #region Define Mode input to seperate Model+SN
                    TraceStepDoing("Chon mode nhap vao SN, Model,-SN, haloge..");
                    strModelEntered = objMainWindow.strPOMaterial.Trim();// pre-assign
                    string strInput = strSerialNumberEntered.Trim();

                    //check các mode input

                    if (strinputway == "2")
                    {


                        string pn = strModelEntered;
                        string sn = "";
                        string result_checkInputMode = Check_Model_SN_by_Conditions(strInput, ref pn, ref sn);

                        if (result_checkInputMode.Equals("OK"))
                        {
                            strinputway = "2";
                            objMainWindow.isModelSN = true;
                            strSerialNumberEntered = sn;
                            strModelEntered = pn;//change now

                        }
                        else
                        {
                            MessageBox.Show(result_checkInputMode + " Hoac Kiểm tra mode SN only ");
                            return;

                        }
                    }
                    else if (strinputway == "1") //Sn only
                    {
                        strinputway = "1";
                        objMainWindow.isSNonly = true;
                        strSerialNumberEntered = strInput;
                        strModelEntered = objMainWindow.strPOMaterial.Trim(); //nochange
                    }
                    else if (strinputway == "3") //halogen mode
                    {
                        if (strSerialNumberEntered.Trim() == objMainWindow.strPOMaterial.Trim())
                        {
                            mySerial.textBox1.Text = "";
                            return;
                        }
                        if (strSerialNumberEntered.Trim() != objMainWindow.strPOMaterial.Trim())
                        {
                            strSerialNumberEntered = strInput;
                            strModelEntered = objMainWindow.strPOMaterial.Trim(); //nochange
                        }
                    }


                    #endregion

                    #region Checkif_SN_input_OK
                    if (KiemSoatTheoProductMap.IsCheckInputAlready)
                    {//case combine
                        //if (KiemSoatTheoProductMap.strInputMode)
                        TraceStepDoing("Kiem trâ da nhap vao box truoc do chua");
                        string strSNModel_Entercheck = "";
                        if (strinputway == "1" || strinputway == "3") //halogen + sn only
                        {
                            strSNModel_Entercheck = strSerialNumberEntered;//base
                        }
                        else if (strinputway == "2")
                        {
                            strSNModel_Entercheck = strModelEntered + "-" + strSerialNumberEntered; //ffc
                        }



                        if (IsSerialAlreadyEntered(strSNModel_Entercheck, mySerial))
                        {
                            // MessageBox.Show("Serial number already entered!");
                            PrintPack.FrmMessageBox frm = new FrmMessageBox("SN đã được nhập vào rồi !");
                            frm.ShowDialog();
                            return;
                        }
                    }
                    #endregion
                    #region CheckPASStestrecord_OK
                    if (KiemSoatTheoProductMap.IsCheckTestLog)
                    {
                        TraceStepDoing("Kiem tra testlog");
                        string resultiftestPASS = "";
                        if (!IsSerialModel(objMainWindow.strPOMaterial, strSerialNumberEntered, ref resultiftestPASS))
                        {
                            //MessageBox.Show(resultiftestPASS);
                            PrintPack.FrmMessageBox frm = new FrmMessageBox("Ket qua Test/TestResult: " + resultiftestPASS);
                            frm.ShowDialog();
                            return;
                        }

                    }
                    else
                    {//ko check testrecord
                        TraceStepDoing("KO Kiem tra testlog");
                    }
                    #endregion

                    #region Check if SN pack in somebox
                    if (KiemSoatTheoProductMap.IsCheckPacked)
                    {
                        TraceStepDoing("Kiem tra da dong goi o box truoc do");
                        if (!IsSerialAlreadyBox(objMainWindow.boxRework, objMainWindow.box, objMainWindow.strPONumber,
                                                       objMainWindow.strPOMaterial, strSerialNumberEntered.Trim(), ref boxexist, ref poexist) == false)
                        {
                            MessageBox.Show("Số SN đã đóng gói rồi! dùng tool check lại số Box number");
                            return;
                        }
                    }
                    #endregion
                    #region Check if belong ProdOrder

                    if (KiemSoatTheoProductMap.IsCheckSNbelongProdOrder || objMainWindow.IsHaveSNListofPO)
                    {
                        TraceStepDoing("Kiem tra SN co thuoc PO ko");
                        //do check SN belong SN
                        if (!IsSerialBelongProdorder_inmemory(strSerialNumberEntered))
                        {
                            PrintPack.FrmMessageBox frm = new FrmMessageBox("Serial " + strSerialNumberEntered + " khong thuoc ProdOrder");
                            frm.ShowDialog();

                            
                            //BusinessPackingRecord.CheckSNListandReloadifProblem(objMainWindow.txtProdOrder.Text);
                            //MessageBox.Show("Chương trình sẽ lấy lại số SN theo order để đảm bảo đủ số SN và Planner ko có thay đổi số SN theo PO, nhấn OK để tiếp tục, thử lại số SN này");
                    
                            return;
                        }


                        //if (!(IsSerialBelongProdorder(strSerialNumberEntered, objMainWindow.strPOMaterial.Trim())))
                        //{
                        //    MessageBox.Show("Serial " + strSerialNumberEntered+ "khong thuoc ProdOrder");
                        //    return;
                        //}
                    }

                    #endregion

                    #region Check if consumed Config FFC

                    if (KiemSoatTheoProductMap.IsCheckConsumedConfig)
                    {
                        TraceStepDoing("Kiem tra SN consume at FFC config tffc_serialnumber");
                        //do check SN belong SN
                        //if (!IsSerialBelongProdorder_inmemory(strSerialNumberEntered))
                        //{
                        //    PrintPack.FrmMessageBox frm = new FrmMessageBox("Serial " + strSerialNumberEntered + " khong thuoc ProdOrder");
                        //    frm.ShowDialog();
                        //    //MessageBox.Show("Serial " + strSerialNumberEntered + " khong thuoc ProdOrder");
                        //    return;
                        //}
                        PrintPack.ACS_EEDataSet.TFFC_SerialNumbersDataTable dt = new ACS_EEDataSet.TFFC_SerialNumbersDataTable();
                        PrintPack.ACS_EEDataSetTableAdapters.TFFC_SerialNumbersTableAdapter da = new ACS_EEDataSetTableAdapters.TFFC_SerialNumbersTableAdapter();
                        int iresult = 0;
                        iresult = Convert.ToInt16(da.CheckSerialifConsumedConfig(strSerialNumberEntered));
                        //foreach (DataRow dr in dt.Rows)
                        //    (
                        //        //iresult = dr["Column1"].ToString().Trim();
                        //     )

                        if (iresult > 0)//has consumed
                        {
                            //MessageBox.Show("OK to go");
                        }
                        else // not conssumed
                        {
                            PrintPack.FrmMessageBox frm = new FrmMessageBox("Serial " + strSerialNumberEntered + " khong pass Config! Vui long kiem tra lai");
                            frm.ShowDialog();
                            return;
                        }




                    }

                    #endregion


                    #region Check Poste SN

                    bool boolresult = false;


                    #region ChecK Model can check POSTE SN
                    PrintPack.TestLogDB.FFC_POSTESN_CONDITIONS1DataTable dtchecksn = new TestLogDB.FFC_POSTESN_CONDITIONS1DataTable();
                    PrintPack.TestLogDBTableAdapters.FFC_POSTESN_CONDITIONS1TableAdapter dachecksn = new TestLogDBTableAdapters.FFC_POSTESN_CONDITIONS1TableAdapter();

                    dtchecksn = dachecksn.GetDataBySAPmodel(strModelEntered);
                    int icountdtchecksn = dtchecksn.Count;
                    if (icountdtchecksn == 0)
                    { //model ko can check
                    }
                    else
                    {//model can check 
                        //check SN đúng format string
                        foreach (DataRow dr in dtchecksn.Rows)
                        {
                            //string sap_model = dr["sap_model"].ToString();
                            string Prefix = dr["Prefix"].ToString().Trim();
                            int ilength = Prefix.Length;
                            #region GetPOSTE SN

                            PrintPack.TestLogDB.FFC_POSTESN1DataTable dt0 = new TestLogDB.FFC_POSTESN1DataTable();
                            PrintPack.TestLogDBTableAdapters.FFC_POSTESN1TableAdapter da0 = new TestLogDBTableAdapters.FFC_POSTESN1TableAdapter();
                            dt0 = da0.GetDataBySN(strSerialNumberEntered);
                            int countdt0 = dt0.Count;
                            if (countdt0 == 0)
                            { //khoong ton tai sn or sn chua conssume 
                                PrintPack.FrmMessageBox frm = new FrmMessageBox("Kiểm tra: Số POSTE SN chưa nhập cho số SN " + strSerialNumberEntered + "ở trạm trước");
                                frm.ShowDialog();
                                //MessageBox.Show("Serial " + strSerialNumberEntered + " khong thuoc ProdOrder");
                                return;
                            }
                            else // co so SN
                            {
                                foreach (DataRow dr0 in dt0.Rows)
                                {
                                    string strPOSSN2check = dr0["PosteSN"].ToString();
                                    if (strPOSSN2check.Substring(0, ilength) == Prefix)
                                    {
                                        boolresult = true;
                                    }
                                    else
                                    {
                                        boolresult = false;
                                        PrintPack.FrmMessageBox frm = new FrmMessageBox("Số POSTE SN sai format");
                                        frm.ShowDialog();
                                        //MessageBox.Show("Serial " + strSerialNumberEntered + " khong thuoc ProdOrder");
                                        return;
                                    }

                                }

                            }
                            #endregion
                        }
                    }
                    #endregion










                    #endregion
                    #region Do Lock TempData
                    if (true)
                    {
                        goto option2;
                    option1:
                        #region option1: add temp->ffcpack


                        //luu tam o tmpPackingRecord
                        LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strSerialNumberEntered,
                            objMainWindow.box, objMainWindow.pyear);

                        //new fuction add local
                        AddSNvaoDS(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strSerialNumberEntered,
                            objMainWindow.box, objMainWindow.pyear, "NoStation");
                        #endregion
                        //temp = GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box);
                        goto endNdonext;
                    option2:
                        #region option2: add local->temp->ffcpack


                        //luu tam o tmpPackingRecord
                        //LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strSerialNumberEntered,
                        //    objMainWindow.box, objMainWindow.pyear);

                        //new fuction add local
                        AddSNvaoDS(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strSerialNumberEntered,
                            objMainWindow.box, objMainWindow.pyear, "NoStation");

                        SetDataExist();
                        #endregion

                        goto endNdonext;

                    endNdonext:
                        QtyPackedInbox = QtyPackedInbox + 1;
                        int currentPack = objMainWindow.intPOPacked + QtyPackedInbox;

                        #region Docheck If Full PO, update Packing information
                        if (currentPack > objMainWindow.intPOQuantity)
                        {
                            MessageBox.Show("PO Full/ PO đóng gói đủ số lượng, vui lòng chuyển PO khác!");
                            return;
                        }
                        else
                        {
                            label3.Content = "Box Qty : " + QtyPackedInbox + " | Packed/PO Qty: " + currentPack + "/" + objMainWindow.intPOQuantity;
                        }

                        #endregion

                        UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;

                        ((TextBox)(elementWithFocus2)).IsEnabled = false;

                        FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                        TraversalRequest request = new TraversalRequest(focusDirection);
                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                        //((TextBox)(elementWithFocus)).IsEnabled = true;
                        elementWithFocus.MoveFocus(request);
                    }
                    else
                    {
                        if (poexist == "NA") MessageBox.Show("Serial number dã trong box: " + boxexist);
                        else MessageBox.Show("Serial number dang du?c packing trong po: " + poexist + " và box: " + boxexist);
                    }
                    UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;

                    //((TextBox)(elementWithFocus1)).SelectAll();
                    ((TextBox)(elementWithFocus1)).Text = "";




                    #endregion

                    return;


                oldversion:
                    switch (objMainWindow.strProductMap)
                    {
                        #region caseFFC
                        case "FFC":
                            if (strSerialNumberEntered.Trim().Length < 13)
                            {
                                if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                {
                                    string strEnglishphrase = "";
                                    string strForeignphrase = "";
                                    MessageBox.Show("Model - Serial Number not entered (" + objMainWindow.getForeignPhrase("MODELSERIALNOTENTER", ref strEnglishphrase, ref strForeignphrase) + ")");
                                }
                                else
                                {
                                    MessageBox.Show("Model - Serial Number not entered!!");
                                }
                                return;
                            }

                            if (objMainWindow.dictPOInformation.Count > 0)
                            {
                                int iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                if (iMaterialLength + 1 < strSerialNumberEntered.Length)
                                {
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1);
                                    strModelEntered = strSerialNumberEntered.Substring(0, iMaterialLength);
                                }
                                else
                                {
                                    strJustSerialNumber = strSerialNumberEntered.Substring(strSerialNumberEntered.Length - 9);
                                    strModelEntered = strSerialNumberEntered.Substring(0, strSerialNumberEntered.Length - 10);
                                }
                            }
                            else
                            {
                                strJustSerialNumber = strSerialNumberEntered.Substring(strSerialNumberEntered.Length - 9);
                                strModelEntered = strSerialNumberEntered.Substring(0, strSerialNumberEntered.Length - 10);
                            }

                            if (strModelEntered == objMainWindow.strCurrentMaterial)
                            {
                                if (objMainWindow.dictPOInformation.ContainsKey(strJustSerialNumber.Trim()))
                                {
                                    if (GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box) == true)
                                    {
                                        if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                                        {
                                            if (IsSerialAlreadyBox(objMainWindow.boxRework, objMainWindow.box, objMainWindow.strPONumber,
                                                objMainWindow.strPOMaterial, strJustSerialNumber.Trim(), ref boxexist, ref poexist) == false)
                                            {
                                                LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strJustSerialNumber.Trim(),
                                                    objMainWindow.box, objMainWindow.pyear);
                                                temp = GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box);

                                                UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;

                                                ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                                FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                                TraversalRequest request = new TraversalRequest(focusDirection);
                                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                                //((TextBox)(elementWithFocus)).IsEnabled = true;
                                                elementWithFocus.MoveFocus(request);
                                            }
                                            else
                                            {
                                                if (poexist == "NA") MessageBox.Show("Serial number đã trong box: " + boxexist);
                                                else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);
                                            }
                                            //elementWithFocus1 = Keyboard.FocusedElement as UIElement;

                                            //((TextBox)(elementWithFocus1)).SelectAll();
                                            //((TextBox)(elementWithFocus1)).Text = "";
                                        }
                                        else
                                        {
                                            if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                            {
                                                string strEnglishphrase = "";
                                                string strForeignphrase = "";
                                                MessageBox.Show("Serial number already entered! (" + objMainWindow.getForeignPhrase("SERIALNUMBERALREADY", ref strEnglishphrase, ref strForeignphrase) + ")");
                                            }
                                            else
                                            {
                                                MessageBox.Show("Serial number already entered!");
                                            }
                                            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                            //((TextBox)(elementWithFocus)).SelectAll();
                                            //((TextBox)(elementWithFocus)).Text = "";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("PO Full");
                                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                        //((TextBox)(elementWithFocus)).SelectAll();
                                        //((TextBox)(elementWithFocus)).Text = ""; 
                                    }
                                }
                                else
                                {
                                    if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                    {
                                        string strEnglishphrase = "";
                                        string strForeignphrase = "";
                                        MessageBox.Show("Serial Number is not in Production Order! (" + objMainWindow.getForeignPhrase("SERIALNOTINPRODORDER", ref strEnglishphrase, ref strForeignphrase) + ")");
                                    }
                                    else
                                    {

                                        MessageBox.Show("Serial Number is not in Production Order!");
                                    }
                                    /*                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                                        TraversalRequest request = new TraversalRequest(focusDirection);
                                    */
                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                                    //((TextBox)(elementWithFocus)).SelectAll();
                                    ((TextBox)(elementWithFocus)).Text = "";
                                    //                    elementWithFocus.MoveFocus(request);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Model is not match in Production Order!");
                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
                            break;
                        #endregion
                        #region caseDLM
                        case "DLM":
                            if (strSerialNumberEntered.Trim().Length < 13)
                            {
                                if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                {
                                    string strEnglishphrase = "";
                                    string strForeignphrase = "";
                                    MessageBox.Show("Model - Serial Number not entered (" + objMainWindow.getForeignPhrase("MODELSERIALNOTENTER", ref strEnglishphrase, ref strForeignphrase) + ")");
                                }
                                else
                                {
                                    MessageBox.Show("Model - Serial Number not entered!!");
                                }
                                return;
                            }

                            if (objMainWindow.dictPOInformation.Count > 0)
                            {
                                int iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                if (iMaterialLength + 1 < strSerialNumberEntered.Length)
                                {
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1);
                                    strModelEntered = strSerialNumberEntered.Substring(0, iMaterialLength);
                                }
                                else
                                {
                                    strJustSerialNumber = strSerialNumberEntered.Substring(strSerialNumberEntered.Length - 9);
                                    strModelEntered = strSerialNumberEntered.Substring(0, strSerialNumberEntered.Length - 10);
                                }
                            }
                            else
                            {
                                strJustSerialNumber = strSerialNumberEntered.Substring(strSerialNumberEntered.Length - 9);
                                strModelEntered = strSerialNumberEntered.Substring(0, strSerialNumberEntered.Length - 10);
                            }


                            if (strModelEntered == objMainWindow.strCurrentMaterial)
                            {
                                if (objMainWindow.dictPOInformation.ContainsKey(strJustSerialNumber.Trim()))
                                {
                                    if (GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box) == true)
                                    {
                                        if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                                        {
                                            if (IsSerialAlreadyBox(objMainWindow.boxRework, objMainWindow.box, objMainWindow.strPONumber,
                                                objMainWindow.strPOMaterial, strJustSerialNumber.Trim(), ref boxexist, ref poexist) == false)
                                            {
                                                LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strJustSerialNumber.Trim(),
                                                    objMainWindow.box, objMainWindow.pyear);
                                                temp = GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box);
                                                UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;

                                                ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                                FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                                TraversalRequest request = new TraversalRequest(focusDirection);
                                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                                //((TextBox)(elementWithFocus)).IsEnabled = true;
                                                elementWithFocus.MoveFocus(request);
                                            }
                                            else
                                            {
                                                if (poexist == "NA") MessageBox.Show("Serial number đã trong box: " + boxexist);
                                                else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);
                                            }
                                            //UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;

                                            //((TextBox)(elementWithFocus1)).SelectAll();
                                            //((TextBox)(elementWithFocus1)).Text = "";
                                        }
                                        else
                                        {
                                            if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                            {
                                                string strEnglishphrase = "";
                                                string strForeignphrase = "";
                                                MessageBox.Show("Serial number already entered! (" + objMainWindow.getForeignPhrase("SERIALNUMBERALREADY", ref strEnglishphrase, ref strForeignphrase) + ")");
                                            }
                                            else
                                            {
                                                MessageBox.Show("Serial number already entered!");
                                            }
                                            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                            //((TextBox)(elementWithFocus)).SelectAll();
                                            ((TextBox)(elementWithFocus)).Text = "";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("PO Full");
                                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                        //((TextBox)(elementWithFocus)).SelectAll();
                                        ((TextBox)(elementWithFocus)).Text = "";
                                    }
                                }
                                else
                                {
                                    if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                    {
                                        string strEnglishphrase = "";
                                        string strForeignphrase = "";
                                        MessageBox.Show("Serial Number is not in Production Order! (" + objMainWindow.getForeignPhrase("SERIALNOTINPRODORDER", ref strEnglishphrase, ref strForeignphrase) + ")");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Serial Number is not in Production Order!");
                                    }
                                    /*                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                                        TraversalRequest request = new TraversalRequest(focusDirection);
                                    */
                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                                    //((TextBox)(elementWithFocus)).SelectAll();
                                    ((TextBox)(elementWithFocus)).Text = "";
                                    //                    elementWithFocus.MoveFocus(request);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Model is not match in Production Order!");
                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
                            break;
                        #endregion
                        #region caseFRUwACS
                        case "FRUwACS":
                            break;
                        #endregion
                        #region caseFRUwoACS
                        case "FRUwoACS":
                            if (strSerialNumberEntered.Trim().Length < 9)
                            {
                                if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                {
                                    string strEnglishphrase = "";
                                    string strForeignphrase = "";
                                    MessageBox.Show("Serial Number not entered (" + objMainWindow.getForeignPhrase("MODELSERIALNOTENTER", ref strEnglishphrase, ref strForeignphrase) + ")");
                                }
                                else
                                {
                                    MessageBox.Show("Serial Number not entered!!");
                                }
                                return;
                            }

                            strJustSerialNumber = strSerialNumberEntered;
                            if (strJustSerialNumber == objMainWindow.strPOMaterial)
                            {
                                LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strJustSerialNumber.Trim(),
                                                   objMainWindow.box, objMainWindow.pyear);
                                temp = GetPartRun2(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box);

                                UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;

                                ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                TraversalRequest request = new TraversalRequest(focusDirection);
                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                //((TextBox)(elementWithFocus)).IsEnabled = true;
                                elementWithFocus.MoveFocus(request);
                            }
                            else
                            {
                                //elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                //((TextBox)(elementWithFocus1)).SelectAll();
                                //((TextBox)(elementWithFocus1)).Text = "";
                            }
                            break;
                        #endregion
                        #region caseBASE
                        case "BASE":
                            string result = "";
                            if (strSerialNumberEntered.Trim().Length < 9)
                            {
                                if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                {
                                    string strEnglishphrase = "";
                                    string strForeignphrase = "";
                                    MessageBox.Show("Serial Number not entered (" + objMainWindow.getForeignPhrase("MODELSERIALNOTENTER", ref strEnglishphrase, ref strForeignphrase) + ")");
                                }
                                else
                                {
                                    MessageBox.Show("Serial Number not entered!!");
                                }
                                return;
                            }

                            strJustSerialNumber = strSerialNumberEntered;
                            if (strJustSerialNumber.Substring(0, 1).Equals("G"))
                            {
                                if (objMainWindow.dictPOInformation.ContainsKey(strJustSerialNumber.Trim()))
                                {
                                    if (GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box) == true)
                                    {
                                        if (IsSerialModel(objMainWindow.strPOMaterial, strSerialNumberEntered, ref result))
                                        {
                                            if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                                            {
                                                if (IsSerialAlreadyBox(objMainWindow.boxRework, objMainWindow.box, objMainWindow.strPONumber,
                                                    objMainWindow.strPOMaterial, strJustSerialNumber.Trim(), ref boxexist, ref poexist) == false)
                                                {
                                                    LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strJustSerialNumber.Trim(),
                                                        objMainWindow.box, objMainWindow.pyear);
                                                    temp = GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box);

                                                    UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;

                                                    ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                                    TraversalRequest request = new TraversalRequest(focusDirection);
                                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                                    //((TextBox)(elementWithFocus)).IsEnabled = true;
                                                    elementWithFocus.MoveFocus(request);
                                                }
                                                else
                                                {
                                                    if (poexist == "NA") MessageBox.Show("Serial number đã trong box: " + boxexist);
                                                    else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);
                                                }
                                                // elementWithFocus1 = Keyboard.FocusedElement as UIElement;

                                                //((TextBox)(elementWithFocus1)).SelectAll();
                                                //((TextBox)(elementWithFocus1)).Text = "";
                                            }
                                            else
                                            {
                                                if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                                {
                                                    string strEnglishphrase = "";
                                                    string strForeignphrase = "";
                                                    MessageBox.Show("Serial number already entered! (" + objMainWindow.getForeignPhrase("SERIALNUMBERALREADY", ref strEnglishphrase, ref strForeignphrase) + ")");
                                                }
                                                else
                                                {

                                                    MessageBox.Show("Serial number already entered!");
                                                }
                                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                                //((TextBox)(elementWithFocus)).SelectAll();
                                                //((TextBox)(elementWithFocus)).Text = "";
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show(result);
                                            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                                            //((TextBox)(elementWithFocus)).SelectAll();
                                            ((TextBox)(elementWithFocus)).Text = "";
                                            //elementWithFocus.MoveFocus(request);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("PO Full");
                                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                        //((TextBox)(elementWithFocus)).SelectAll();
                                        ((TextBox)(elementWithFocus)).Text = "";
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Serial Number is not in Production Order!");

                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                    //((TextBox)(elementWithFocus)).SelectAll();
                                    ((TextBox)(elementWithFocus)).Text = "";
                                }
                            }
                            else
                            {
                                if (GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box) == true)
                                {
                                    if (IsSerialModel(objMainWindow.strPOMaterial, strSerialNumberEntered, ref result))
                                    {
                                        if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                                        {
                                            if (IsSerialAlreadyBox(objMainWindow.boxRework, objMainWindow.box, objMainWindow.strPONumber,
                                                objMainWindow.strPOMaterial, strJustSerialNumber.Trim(), ref boxexist, ref poexist) == false)
                                            {
                                                LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strJustSerialNumber.Trim(),
                                                    objMainWindow.box, objMainWindow.pyear);
                                                temp = GetPartRun(objMainWindow.strPONumber, objMainWindow.boxRework, objMainWindow.box);
                                                UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;

                                                ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                                FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                                TraversalRequest request = new TraversalRequest(focusDirection);
                                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                                //((TextBox)(elementWithFocus)).IsEnabled = true;
                                                elementWithFocus.MoveFocus(request);
                                            }
                                            else
                                            {
                                                if (poexist == "NA") MessageBox.Show("Serial number đã trong box: " + boxexist);
                                                else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);
                                            }
                                            //elementWithFocus1 = Keyboard.FocusedElement as UIElement;

                                            //((TextBox)(elementWithFocus1)).SelectAll();
                                            //((TextBox)(elementWithFocus1)).Text = "";
                                        }
                                        else
                                        {
                                            if (!ConfigurationManager.AppSettings.Get("runLocation").ToString().Trim().Equals("EUGENE"))
                                            {
                                                string strEnglishphrase = "";
                                                string strForeignphrase = "";
                                                MessageBox.Show("Serial number already entered! (" + objMainWindow.getForeignPhrase("SERIALNUMBERALREADY", ref strEnglishphrase, ref strForeignphrase) + ")");
                                            }
                                            else
                                            {
                                                MessageBox.Show("Serial number already entered!");
                                            }
                                            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                            //((TextBox)(elementWithFocus)).SelectAll();
                                            ((TextBox)(elementWithFocus)).Text = "";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(result);
                                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                                        //((TextBox)(elementWithFocus)).SelectAll();
                                        ((TextBox)(elementWithFocus)).Text = "";
                                        //elementWithFocus.MoveFocus(request);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("PO Full");
                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                    //((TextBox)(elementWithFocus)).SelectAll();
                                    ((TextBox)(elementWithFocus)).Text = "";
                                }
                            }
                            break;
                        #endregion
                        #region caseSMTwACS
                        case "SMTwACS":
                            break;
                        #endregion
                        #region caseSMTwoACS
                        case "SMTwoACS":
                            break;
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private void UpdateDataThenDoPrint()
        {
            //    goto option2;
            //option1:
            //    #region option1: temp->packing

            //    //tmp->packingrecord
            //    TraceStepDoing("UpdateFinishData:temp -> packingdata");
            //    DoUpdateFinishPackingData(); //tranfer temptbl 2 packingdata //danh gia 2 tuan from 12 oct then remove


            //    #endregion
            //    goto endoption;

            //option2:
            #region option2: local-> temp->packing

            //local/ds ->tmp packingrecord
            TraceStepDoing("local -> Packingrecord" + DSPackedSN.Count());
            DoUpdateDSPackedSN2_tempPackingTable();

            //tmp -> packing
            TraceStepDoing("UpdateFinishData:temp -> packingdata");
            DoUpdateFinishPackingData(); //tranfer temptbl 2 packingdata //danh gia 2 tuan from 12 oct then remove


            #endregion



            #region In nhan

            TraceStepDoing("Thuc hien lenh in");
            DoPrint();

            TraceStepDoing("Update thong tin PO vao T Information");
            UpdatePartRun2TInformation(objMainWindow.intPOPacked + QtyPackedInbox);
            //UpdatePartRun(); // cu cua Gau

            this.Close();
            #endregion
        }

        BartenderBusiness LabelPrint = new BartenderBusiness();
        public void DoPrint()
        {
            #region In nhãn Shipping label
            //if (objMainWindow.checkBox_PrintCusLabel.IsChecked.Value == true)
            //{
            //    // khong in nhan
            //    try
            //    {
            //        string strReturn = objMainWindow.PrintCustomerLabel(objMainWindow);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //} 
            #endregion
            int iCounter = 0;

            #region In nhan OverPack (thông tin trên thùng- không phải list số SN)
            if (objMainWindow.checkBox_PrintIndividualBoxLabels.IsChecked == true)
            {
                try
                {
                    //for (int i = 0; i < objMainWindow.iMaxSerialsPerOrder; i++)
                    //{
                    //    //ucSerialNumber aSerialEntry = (ucSerialNumber)(this.stackPanel1.Children[i]);
                    //    //if (aSerialEntry.textBox1.Text.ToString().Trim().Length > 0)
                    //    //{
                    //    //    iCounter++;
                    //    //}
                    //}
                    iCounter = DSPackedSN.Count();

                    objMainWindow.aPK.BoxQty = iCounter.ToString();
                    objMainWindow.aPK.Boxno = objMainWindow.box;

                    string strReturn = objMainWindow.PrintBoxLabel(objMainWindow, iCounter);


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            #endregion



            #region In nhãn List SN

            if (objMainWindow.checkBox_PrintOverPackLabel.IsChecked == true)
            {
                #region Box Rework
                if (objMainWindow.boxRework)
                {
                    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
                    {
                        try
                        {
                            objMainWindow.sqlConnection4.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = objMainWindow.sqlConnection4;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "ame_DeleteBoxRework";
                            cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                            cmd.Parameters["@PONumber"].Value = objMainWindow.strPONumber;
                            cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                            cmd.Parameters.Add("@BoxRework", SqlDbType.Char, 30);
                            cmd.Parameters["@BoxRework"].Value = objMainWindow.box;
                            cmd.Parameters["@BoxRework"].Direction = ParameterDirection.Input;
                            SqlDataReader rec = cmd.ExecuteReader();
                            objMainWindow.sqlConnection4.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }

                }
                #endregion

                if (objMainWindow.strProductMap.Equals("FRUwoACS"))
                {
                    UpdateRecord2(objMainWindow.box);
                    objMainWindow.boxRework = false;
                    UpdatePartRun2();
                }
                else
                {

                    //DoUpdateFinishPackingData();

                    SortedList<string, ucSerialNumber> slSortedItems = new SortedList<string, ucSerialNumber>();
                    objMainWindow.slRePrint.Clear();
                    objMainWindow.DSPackedSN_RePrint.Clear();


                    try
                    {
                        //int iPageCounter = 0;
                        //int iPages = 0;
                        //                   int iCounter = 0;
                        //         string [] arSerials  = new string[1000] ;
                        objMainWindow.btLabel.setPrintFile(3, objMainWindow.strPrintOverPack);
                        objMainWindow.btLabel.clearAllFieldValues(3);

                        DoPrintOverPack2(DSPackedSN, objMainWindow.box);

                        objMainWindow.DSPackedSN_RePrint = DSPackedSN;
                        //DSPackedSN.Clear();
                        //xong
                        return;


                        for (int i = 0; i < objMainWindow.iMaxSerialsPerOrder; i++)
                        {
                            try
                            {
                                ucSerialNumber aSerialEntry = (ucSerialNumber)(this.stackPanel1.Children[i]);
                                if (aSerialEntry.textBox1.Text.ToString().Trim().Length > 0)
                                {
                                    //                             arSerials[iCounter++] = aSerialEntry.textBox1.Text.ToString().Trim();
                                    if (strinputway == "1" || strinputway == "3") //halogen + sn only
                                    {
                                        slSortedItems.Add(string.Format("{0}-{1}", objMainWindow.strPOMaterial, aSerialEntry.textBox1.Text.ToString().Trim()), aSerialEntry);
                                    }
                                    else if (strinputway == "2")
                                    {
                                        slSortedItems.Add(aSerialEntry.textBox1.Text.ToString().Trim(), aSerialEntry);
                                    }


                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Indexing error i= " + i.ToString() + " iCounter=" + iCounter.ToString());
                            }
                        }

                        objMainWindow.slRePrint = slSortedItems;
                        /*
                        string strSerialFieldName;
                        string strSerialFieldNameNo;
                        string strSerialFieldRoot = ConfigurationManager.AppSettings.Get("OverPackFieldRoot").ToString();
                        string strSerialFieldRootNo = ConfigurationManager.AppSettings.Get("OverPackFieldRootNo").ToString();
                        string strTwoDFieldName = ConfigurationManager.AppSettings.Get("OverPackTwoDField").ToString();
                        */
                        DoPrintOverPack(slSortedItems);

                        objMainWindow.boxRework = false;
                        //UpdatePartRun();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            #endregion
        }

        private void DoUpdateFinishPackingData()
        {
            UpdateRecord(objMainWindow.strPONumber, objMainWindow.box); // include clear temprecord
            //ClearTmpRecord(objMainWindow.strPONumber, objMainWindow.box);
        }


        /// <summary>
        /// In nhan ListSN
        /// Sorlist da duoc chuan bi truoc do voi cau hinh Model-SN
        /// </summary>
        /// <param name="slSortedItems"></param>
        private void DoPrintOverPack2(List<clsSerialInput> DSSN_canin, string boxnumber)
        {
            //int iCounter = DSsSNinpit.Count();
            //int iPageCounter = 0;
            int iPages = 0;
            string strSerialFieldName;
            string strSerialFieldNameNo;
            string strSerialFieldRoot = ConfigurationManager.AppSettings.Get("OverPackFieldRoot").ToString();//pn-sn0
            string strSerialFieldRootNo = ConfigurationManager.AppSettings.Get("OverPackFieldRootNo").ToString(); //sp0
            string strTwoDFieldName = ConfigurationManager.AppSettings.Get("OverPackTwoDField").ToString();//2d

            #region Khang add in nhan list SN
            #region Khởi tạo vị trí đặt nhãn

            string Model = objMainWindow.strPOMaterial.ToUpper();//txtPN.Text.ToUpper();
            //Model ="ABCD";
            //get date
            //get description
            //get sn
            int Packnumber = DSSN_canin.Count;
            #endregion


            #region Đếm_Số_SN_trong_box_để_in_ra_OVerPackLabel

            List<clsSerialInput> SortedList = DSSN_canin.OrderBy(o => o.Serial).ToList();


            //var listsn = SNs.Select(x => x.Serial).ToList();//ok
            var listsn = SortedList.Select(x => x.Serial).ToList();

            string[] myArray = listsn.ToArray();
            int countPackSN = Packnumber;//0;
            //foreach (string value in myArray)
            //{
            //    if (value != "") ;
            //    ++countPackSN;
            //}
            #endregion

            #region KO DUNG- Gán số thông tin cho Overpack và IN
            ////print overpack
            //string datetimenow = DateTime.Now.Day.ToString() + " " + DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + DateTime.Now.Year.ToString();

            //LabelPrint.GanDuongDanBTlabel(nhanOverPack);
            //LabelPrint.GanShareNameWithValueBTlabel("SPART", txtPN.Text.ToUpper());
            //LabelPrint.GanShareNameWithValueBTlabel("PARTREV", txtRev.Text.ToUpper());
            //LabelPrint.GanShareNameWithValueBTlabel("DESC", txtDes.Text.ToUpper());
            //LabelPrint.GanShareNameWithValueBTlabel("ZDATE", datetimenow/*DateTime.Now.ToShortDateString()*/);
            //LabelPrint.GanShareNameWithValueBTlabel("AMT", countPackSN.ToString());
            //LabelPrint.GanShareNameWithValueBTlabel("PRODORDER", this.txtTO.Text.ToUpper());
            //LabelPrint.GanShareNameWithValueBTlabel("MANPACK", this.cboPackingPlace.Text.ToString());
            //LabelPrint.GanShareNameWithValueBTlabel("CORIGIN", this.cboManuafacturingIn.Text.ToUpper());

            //LabelPrint.GanShareNameWithValueBTlabel("BOX", txtBOX.Text);


            //LabelPrint.GansoluongNhancanin(1);

            //LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            //LabelPrint.ThucHienIn();

            #endregion

            #region Chia số SN theo số lượng trên 15SNperlabel
            ///Input
            ///Output
            #region Tính số trang SN cần in_Input: Số record tổng cộng, số SNperLabel; Output: số trang (đã được RoundUP)
            ///
            int sosntoidatren1nhan = 15;

            int npage = (countPackSN + sosntoidatren1nhan - 1) / sosntoidatren1nhan;//(countPackSN/sosntoidatren1nhan)+1; //+1 cho test thôi nha

            #endregion

            #endregion

            #region Gán thông tin cho nhãn OverpackContent_nhãn SN và IN
            //print sn
            //LabelPrint.GanDuongDanBTlabel(nhanOverPackContent);
            //List<string> lstget = LabelPrint.GetListFieldNameFromBTlabel();
            //int i = 0;
            //loop to print page
            for (int k = 0; k < npage; k++)
            {
                var listtoprint = listsn.Skip(k * sosntoidatren1nhan).Take(sosntoidatren1nhan).ToList();
                myArray = listtoprint.ToArray();
                int sosntrentungtrang = myArray.Count();

                string PN2D = "";
                //gan all arry = "" empty
                for (int n = 0; n <= 15 - 1; n++)
                {

                    if (myArray[0].Length != 0)
                    {
                        //LabelPrint.GanShareNameWithValueBTlabel("SPSN" + n, "");
                        //LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + n, "");

                        objMainWindow.btLabel.findFieldandSubstitute(3, "SPSN" + n, "");
                        objMainWindow.btLabel.findFieldandSubstitute(3, "PARTCONSERIAL" + n, "");
                    }

                }
                // gan cac cot co value
                for (int j = 0; j <= sosntrentungtrang - 1; j++)
                {

                    if (myArray[0].Length != 0)
                    {
                        //LabelPrint.GanShareNameWithValueBTlabel("SPSN" + j, myArray[j]);
                        //LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + j, Model + "-" + myArray[j]);

                        objMainWindow.btLabel.findFieldandSubstitute(3, "SPSN" + j, myArray[j]);
                        objMainWindow.btLabel.findFieldandSubstitute(3, "PARTCONSERIAL" + j, Model + "-" + myArray[j]);

                        #region Combine data cho nhãn 2D
                        if (myArray[j] != "")
                        {

                            PN2D += Model + "-" + myArray[j];
                            PN2D += ",";
                        }
                        #endregion

                    }

                }
                //LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIALALL", PN2D.Substring(0, PN2D.Length - 1));
                objMainWindow.btLabel.findFieldandSubstitute(3, "PARTCONSERIALALL", PN2D.Substring(0, PN2D.Length - 1));

                objMainWindow.btLabel.findFieldandSubstitute(3, "BOX", boxnumber);

                objMainWindow.btLabel.doPrint(3, false, false);

                //LabelPrint.GansoluongNhancanin(1);

                //LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
                //LabelPrint.ThucHienIn();
            }
            #endregion

            #endregion




            //note     objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);

            // objMainWindow.btLabel.doPrint(3, false, false);
            //in nhan xong =>clear data
            //clear 2d+SNonly+Model-SN

            //DSPackedSN.Clear();

            for (int n = 0; n <= 15 - 1; n++)
            {
                objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, "");//2d
                objMainWindow.btLabel.findFieldandSubstitute(3, "SPSN" + n, "");
                objMainWindow.btLabel.findFieldandSubstitute(3, "PARTCONSERIAL" + n, "");
            }
        }



        /// <summary>
        /// In nhan ListSN
        /// Sorlist da duoc chuan bi truoc do voi cau hinh Model-SN
        /// </summary>
        /// <param name="slSortedItems"></param>
        private void DoPrintOverPack(SortedList<string, ucSerialNumber> slSortedItems)
        {
            int iCounter = slSortedItems.Count();
            //int iPageCounter = 0;
            int iPages = 0;
            string strSerialFieldName;
            string strSerialFieldNameNo;
            string strSerialFieldRoot = ConfigurationManager.AppSettings.Get("OverPackFieldRoot").ToString();
            string strSerialFieldRootNo = ConfigurationManager.AppSettings.Get("OverPackFieldRootNo").ToString();
            string strTwoDFieldName = ConfigurationManager.AppSettings.Get("OverPackTwoDField").ToString();


            if (iCounter % objMainWindow.iMaxSerialsFFC == 0) iPages = (iCounter / objMainWindow.iMaxSerialsFFC);
            else iPages = (iCounter / objMainWindow.iMaxSerialsFFC) + 1;

            for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
            {
                // so trang

                int iItemsForThisPage;
                int iStartingIndex;
                iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsFFC;
                if (iPageNumber == iPages)
                {
                    iItemsForThisPage = iCounter - iStartingIndex;
                }
                else
                {
                    iItemsForThisPage = objMainWindow.iMaxSerialsFFC;
                }

                #region gán thông tin 2D
                string strFieldValue;
                string strTwoDFieldValue = "";
                for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                {
                    strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();

                    string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                    strFieldValue = strSortedFieldValue;
                    if (jPrintPageIndex > 0)
                    {
                        strTwoDFieldValue += "," + strFieldValue;
                    }
                    else
                    {
                        strTwoDFieldValue = strFieldValue;
                    }
                    objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                }


                objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, strTwoDFieldValue);// gán nhãn 2D 
                #endregion


                string strFieldValueNo;
                for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                {
                    // gan list sn
                    strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();
                    strSerialFieldNameNo = strSerialFieldRootNo + jPrintPageIndex.ToString().Trim();

                    string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                    strFieldValue = strSortedFieldValue.Substring(objMainWindow.strPOMaterial.Length + 1);
                    strFieldValueNo = ((jPrintPageIndex + 1) + ((iPageNumber - 1) * objMainWindow.iMaxSerialsFFC)).ToString();

                    //objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                    //objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, strFieldValueNo);
                    objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, strFieldValue);

                }
                objMainWindow.btLabel.doPrint(3, false, false);
                //in nhan xong =>clear data
                //clear 2d+SNonly+Model-SN



                for (int n = 0; n <= 15 - 1; n++)
                {
                    objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, "");//2d
                    objMainWindow.btLabel.findFieldandSubstitute(3, "SPSN" + n, "");
                    objMainWindow.btLabel.findFieldandSubstitute(3, "PARTCONSERIAL" + n, "");
                }
            }

            return;

            //khong dung phan duoi


            switch (objMainWindow.strProductMap)
            {
                case "FFC":
                    if (iCounter % objMainWindow.iMaxSerialsFFC == 0) iPages = (iCounter / objMainWindow.iMaxSerialsFFC);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsFFC) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        // so trang

                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsFFC;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsFFC;
                        }

                        #region gán thông tin 2D
                        string strFieldValue;
                        string strTwoDFieldValue = "";
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue;
                            if (jPrintPageIndex > 0)
                            {
                                strTwoDFieldValue += "," + strFieldValue;
                            }
                            else
                            {
                                strTwoDFieldValue = strFieldValue;
                            }
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                        }


                        objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, strTwoDFieldValue);// gán nhãn 2D 
                        #endregion


                        string strFieldValueNo;
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            // gan list sn
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue.Substring(objMainWindow.strPOMaterial.Length + 1);
                            strFieldValueNo = ((jPrintPageIndex + 1) + ((iPageNumber - 1) * objMainWindow.iMaxSerialsFFC)).ToString();

                            //objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                            //objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, strFieldValueNo);
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, strFieldValue);

                        }
                        objMainWindow.btLabel.doPrint(3, false, false);
                        //in nhan xong
                    }




                    break;
                case "DLM":
                    if (iCounter % objMainWindow.iMaxSerialsFFC == 0) iPages = (iCounter / objMainWindow.iMaxSerialsFFC);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsFFC) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsFFC;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsFFC;
                        }

                        string strFieldValue;
                        string strTwoDFieldValue = "";
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue;
                            if (jPrintPageIndex > 0)
                            {
                                strTwoDFieldValue += "," + strFieldValue;
                            }
                            else
                            {
                                strTwoDFieldValue = strFieldValue;
                            }
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                        }
                        objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, strTwoDFieldValue);
                        objMainWindow.btLabel.doPrint(3, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsFFC; k++)
                        {
                            strSerialFieldName = strSerialFieldRoot + k.ToString().Trim();
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, "");
                        }
                        objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, "");
                    }

                    objMainWindow.btLabel.setPrintFile(4, objMainWindow.strPrintOverPackVN);
                    objMainWindow.btLabel.clearAllFieldValues(4);

                    if (iCounter % objMainWindow.iMaxSerialsDLM == 0) iPages = (iCounter / objMainWindow.iMaxSerialsDLM);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsDLM) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsDLM;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsDLM;
                        }

                        string strFieldValue;
                        string strFieldValueNo;
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue.Substring(objMainWindow.strPOMaterial.Length + 1);
                            strFieldValueNo = ((jPrintPageIndex + 1) + ((iPageNumber - 1) * objMainWindow.iMaxSerialsDLM)).ToString();

                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldName, strFieldValue);
                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldNameNo, strFieldValueNo);
                        }
                        objMainWindow.btLabel.doPrint(4, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsDLM; k++)
                        {
                            strSerialFieldName = strSerialFieldRoot + k.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + k.ToString().Trim();
                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldName, "");
                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldNameNo, "");
                        }
                    }
                    break;
                case "BASE":
                    if (iCounter % objMainWindow.iMaxSerialsBASE == 0) iPages = (iCounter / objMainWindow.iMaxSerialsBASE);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsBASE) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsBASE;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsBASE;
                        }

                        string strFieldValue;
                        string strFieldValueNo;
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue;
                            strFieldValueNo = ((jPrintPageIndex + 1) + ((iPageNumber - 1) * objMainWindow.iMaxSerialsBASE)).ToString();

                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, strFieldValueNo);
                        }
                        objMainWindow.btLabel.doPrint(3, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsBASE; k++)
                        {
                            strSerialFieldName = strSerialFieldRoot + k.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + k.ToString().Trim();
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, "");
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, "");
                        }
                    }
                    break;
            }
        }

        /*
        private void DoPrePrintOverPack(SortedList<string, ucSerialNumber> slSortedItems, MainWindow objMainWindow)
        {
            int iCounter = slSortedItems.Count();
            int iPageCounter = 0;
            int iPages = 0;
            string strSerialFieldName;
            string strSerialFieldNameNo;
            string strSerialFieldRoot = ConfigurationManager.AppSettings.Get("OverPackFieldRoot").ToString();
            string strSerialFieldRootNo = ConfigurationManager.AppSettings.Get("OverPackFieldRootNo").ToString();
            string strTwoDFieldName = ConfigurationManager.AppSettings.Get("OverPackTwoDField").ToString();

            switch (objMainWindow.strProductMap)
            {
                case "FFC":
                    if (iCounter % objMainWindow.iMaxSerialsFFC == 0) iPages = (iCounter / objMainWindow.iMaxSerialsFFC);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsFFC) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsFFC;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsFFC;
                        }

                        string strFieldValue;
                        string strTwoDFieldValue = "";
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue;
                            if (jPrintPageIndex > 0)
                            {
                                strTwoDFieldValue += "," + strFieldValue;
                            }
                            else
                            {
                                strTwoDFieldValue = strFieldValue;
                            }
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                        }
                        objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, strTwoDFieldValue);
                        objMainWindow.btLabel.doPrint(3, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsFFC; k++)
                        {
                            strSerialFieldName = strSerialFieldRoot + k.ToString().Trim();
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, "");
                        }
                        objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, "");
                    }
                    break;
                case "DLM":
                    if (iCounter % objMainWindow.iMaxSerialsFFC == 0) iPages = (iCounter / objMainWindow.iMaxSerialsFFC);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsFFC) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsFFC;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsFFC;
                        }

                        string strFieldValue;
                        string strTwoDFieldValue = "";
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue;
                            if (jPrintPageIndex > 0)
                            {
                                strTwoDFieldValue += "," + strFieldValue;
                            }
                            else
                            {
                                strTwoDFieldValue = strFieldValue;
                            }
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                        }
                        objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, strTwoDFieldValue);
                        objMainWindow.btLabel.doPrint(3, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsFFC; k++)
                        {
                            strSerialFieldName = strSerialFieldRoot + k.ToString().Trim();
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, "");
                        }
                        objMainWindow.btLabel.findFieldandSubstitute(3, strTwoDFieldName, "");
                    }

                    objMainWindow.btLabel.setPrintFile(4, objMainWindow.strPrintOverPackVN);
                    objMainWindow.btLabel.clearAllFieldValues(4);

                    if (iCounter % objMainWindow.iMaxSerialsDLM == 0) iPages = (iCounter / objMainWindow.iMaxSerialsDLM);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsDLM) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsDLM;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsDLM;
                        }

                        string strFieldValue;
                        string strFieldValueNo;
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue.Substring(strSortedFieldValue.Length - 9);
                            strFieldValueNo = ((jPrintPageIndex + 1) + ((iPageNumber - 1) * objMainWindow.iMaxSerialsDLM)).ToString();

                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldName, strFieldValue);
                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldNameNo, strFieldValueNo);
                        }
                        objMainWindow.btLabel.doPrint(4, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsDLM; k++)
                        {
                            strSerialFieldName = strSerialFieldRoot + k.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + k.ToString().Trim();
                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldName, "");
                            objMainWindow.btLabel.findFieldandSubstitute(4, strSerialFieldNameNo, "");
                        }
                    }
                    break;
                case "BASE":
                    if (iCounter % objMainWindow.iMaxSerialsBASE == 0) iPages = (iCounter / objMainWindow.iMaxSerialsBASE);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsBASE) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsBASE;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsBASE;
                        }

                        string strFieldValue;
                        string strFieldValueNo;
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue;
                            strFieldValueNo = ((jPrintPageIndex + 1) + ((iPageNumber - 1) * objMainWindow.iMaxSerialsBASE)).ToString();

                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, strFieldValueNo);
                        }
                        objMainWindow.btLabel.doPrint(3, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsBASE; k++)
                        {
                            strSerialFieldName = strSerialFieldRoot + k.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + k.ToString().Trim();
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, "");
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, "");
                        }
                    }
                    break;
            }
        }
        */

        public void DoRePrint(MainWindow objMainWindow)
        {
            if (objMainWindow.DSPackedSN_RePrint.Count() != 0)
            {
                #region In Nhan Shipping
                if (objMainWindow.checkBox_PrintCusLabel.IsChecked.Value == true)
                {
                    try
                    {
                        string strReturn = objMainWindow.PrintCustomerLabel(objMainWindow);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                #endregion
                int iCounter = objMainWindow.slRePrint.Count();
                #region In nhan OverpackContent
                if (objMainWindow.checkBox_PrintIndividualBoxLabels.IsChecked == true)
                {
                    try
                    {


                        //do pprint overpack
                        //objMainWindow.strPOMaterial = objMainWindow.strPOMaterialbak;
                        //objMainWindow.strPONumber = objMainWindow.strPONumberbak;
                        //objMainWindow.strPORev = objMainWindow.strPORevbak;
                        //new add

                        objMainWindow.strPOMaterial = objMainWindow.aPK.POMaterial;

                        objMainWindow.strPONumber = objMainWindow.aPK.POnumber;
                        objMainWindow.strPORev = objMainWindow.aPK.Rev;
                        objMainWindow.box = objMainWindow.aPK.Boxno;
                        iCounter = Convert.ToInt16(objMainWindow.aPK.BoxQty);

                        string strReturn = objMainWindow.PrintBoxLabel(objMainWindow, iCounter);
                        objMainWindow.strPOMaterial = "";
                        objMainWindow.strPONumber = "";
                        objMainWindow.strPORev = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                #endregion

                #region In nhan SN list
                if (objMainWindow.checkBox_PrintOverPackLabel.IsChecked == true)
                {
                    try
                    {
                        objMainWindow.btLabel.setPrintFile(3, objMainWindow.strPrintOverPack);
                        objMainWindow.btLabel.clearAllFieldValues(3);
                        objMainWindow.strPOMaterial = objMainWindow.aPK.POMaterial;

                        //DoPrintOverPack(objMainWindow.slRePrint);
                        DoPrintOverPack2(objMainWindow.DSPackedSN_RePrint, objMainWindow.box);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }

                #endregion
            }
        }

        private void UpdateRecord(string PONumber, string Box)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    //khai báo command
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_PackingRecord";
                    //value input parameter
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = PONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    //value input parameter 
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = Box;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;

                    //thực thi bằng ExecuteReader
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateRecord2(string Box)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_PackingRecord2";
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = Box;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ClearTmpRecord(string PONumber, string Box)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_ClearPackingRecord";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = PONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = Box;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ClosePrintPackSerials()
        {
            if (objMainWindow.boxRework)
            {//rework
                using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
                {
                    try
                    {
                        objMainWindow.sqlConnection4.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = objMainWindow.sqlConnection4;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ame_ClearBoxRework";
                        cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                        cmd.Parameters["@PONumber"].Value = objMainWindow.strPONumber;
                        cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                        cmd.Parameters["@BoxNumber"].Value = objMainWindow.box;
                        cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                        SqlDataReader rec = cmd.ExecuteReader();
                        objMainWindow.sqlConnection4.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error Clear Box Rework:" + ex.Message);
                    }
                }
            }
            else
            {//as normal
                using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
                {
                    try
                    {
                        objMainWindow.sqlConnection4.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = objMainWindow.sqlConnection4;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ame_ClearBoxNumber";
                        cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                        cmd.Parameters["@BoxNumber"].Value = objMainWindow.box;
                        cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                        SqlDataReader rec = cmd.ExecuteReader();
                        objMainWindow.sqlConnection4.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error Clear Box Number:" + ex.Message);
                    }
                }
            }
            objMainWindow.boxRework = false;
            objMainWindow.dataExist = false;
            ClearTmpRecord(objMainWindow.strPONumber, objMainWindow.box);
            if (objMainWindow.strPOMaterial != "") objMainWindow.strPOMaterialbak = objMainWindow.strPOMaterial;
            if (objMainWindow.strPONumber != "") objMainWindow.strPONumberbak = objMainWindow.strPONumber;
            if (objMainWindow.strPORev != "") objMainWindow.strPORevbak = objMainWindow.strPORev;
            objMainWindow.strPOMaterial = "";
            objMainWindow.strPONumber = "";
            objMainWindow.strPORev = "";
            objMainWindow.intPOQuantity = 0;
            objMainWindow.intPOPacked = 0;
            objMainWindow.intPORun = 0;
            this.Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (objMainWindow.dataExist)
            {
                UpdateDataThenDoPrint();
                //DoPrint();
                this.Close();
            }
            else
            {
                ClosePrintPackSerials();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ClosePrintPackSerials();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ClosePrintPackSerials();
        }
    }


    static class FocusHelper
    {
        private delegate void MethodInvoker();

        public static void Focus(UIElement element)
        {
            ThreadPool.QueueUserWorkItem(delegate(Object foo)
            {
                UIElement elem = (UIElement)foo;
                elem.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                    (MethodInvoker)delegate()
                    {
                        elem.IsEnabled = true;
                        elem.Focus();
                        Keyboard.Focus(elem);
                    });
            }, element);
        }
    }




}
                    #endregion