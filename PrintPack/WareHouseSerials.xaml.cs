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
using System.IO;
using System.Data.OleDb;

namespace PrintPack
{
    /// <summary>
    /// Interaction logic for PrintPackSerials.xaml
    /// </summary>
    public partial class WareHouseSerials : Window
    {
        public WareHouse objMainWindow;

        public WareHouseSerials()
        {
            InitializeComponent();
        }

        public string boxnew;
        public string box;

        public void SetLabel(Boolean State1, string Text1, Boolean State2, string Text2)
        {
            if (State1) label1.Visibility = Visibility.Visible;
            else label1.Visibility = Visibility.Hidden;
            label1.Content = Text1;
            if (State2) label2.Visibility = Visibility.Visible;
            else label2.Visibility = Visibility.Hidden;
            label2.Content = Text2;
        }

        public WareHouseSerials(WareHouse objMyMainWindow): this()
        {
            objMainWindow = objMyMainWindow;
            if (objMainWindow.bolRePrint == false)
            {
                objMainWindow.dataExist = false;
                if (objMainWindow.strProductMap == "FRUwoACS") objMainWindow.bolwoACS = true;
                box = objMainWindow.box;
                boxnew = objMainWindow.boxnew;

                switch (objMainWindow.Mode)
                {
                    case 0: //UnPacking All
                        SetLabel(true, "Enter Box Number", false, "");
                        break;
                    case 1: //Combine Packing
                        SetLabel(true, "Enter Model-Serial or Model or Serial", true, "BOX: " + box);
                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
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
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show("Error Check Box Number:" + ex.Message);
                            }
                        }
                        break;
                    case 2: //Scan Box Number
                        SetLabel(true, "Enter Box Number", false, "");
                        break;
                    case 3: //UnPacking Apart
                        if (objMainWindow.bolwoACS)
                        {
                            SetLabel(true, "Enter " + objMainWindow.strPOMaterial + "- Revision: " + objMainWindow.strPORev, true, "BOX: " + box);
                        }
                        else
                        {
                            if (objMainWindow.strProductMap == "BASE")
                            {
                                SetLabel(true, "Enter " + objMainWindow.strPOMaterial + "-Serial  Revision: " + objMainWindow.strPORev, true, "BOX: " + box);
                            }
                            else
                            {
                                SetLabel(true, "Enter Model-Serial", true, "BOX: " + box);
                            }
                        }

                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
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
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show("Error Check Box Number:" + ex.Message);
                            }
                        }
                        break;
                    case 4: //Create New Box
                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
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
                        }

                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
                            try
                            {
                                objMainWindow.sqlConnection4.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = objMainWindow.sqlConnection4;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "ame_GetBoxNumber";
                                SqlDataReader rec = cmd.ExecuteReader();
                                rec.Read();
                                box = rec["BoxNumber"].ToString().Trim();
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show("Error Get Box Number:" + ex.Message);
                            }
                        }
                        SetLabel(true, "Enter Model-Serial or Serial", true, "BOX: " + box);
                        break;
                    case 5: //Box RePrint
                        DoBoxPrint(box);
                        this.Close();
                        break;
                    case 6: //UnPacking + New Box
                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
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
                        }

                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
                            try
                            {
                                objMainWindow.sqlConnection4.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = objMainWindow.sqlConnection4;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "ame_GetBoxNumber";
                                SqlDataReader rec = cmd.ExecuteReader();
                                rec.Read();
                                boxnew = rec["BoxNumber"].ToString().Trim();
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show("Error Get Box Number:" + ex.Message);
                            }
                        }

                        if (objMainWindow.bolwoACS)
                        {
                            SetLabel(true, "Enter " + objMainWindow.strPOMaterial + "- Revision: " + objMainWindow.strPORev, true, "BOX: " + box);
                        }
                        else
                        {
                            if (objMainWindow.strProductMap == "BASE")
                            {
                                SetLabel(true, "Enter " + objMainWindow.strPOMaterial + "-Serial  Revision: " + objMainWindow.strPORev, true, "BOX: " + box);
                            }
                            else
                            {
                                SetLabel(true, "Enter Model-Serial", true, "BOX: " + box);
                            }
                        }
                        break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ucSerialNumber firstSerial = new ucSerialNumber();

            for (int i = 1; i <= objMainWindow.iMaxSerialsPerOrder; i++)
            {
                ucSerialNumber aSerial = new ucSerialNumber();

                aSerial.label2.Content = i.ToString();
                aSerial.TabIndex = i;
                aSerial.textBox1.Text = " ";
                aSerial.textBox1.GotKeyboardFocus += TextBoxGotKeyboardFocus;
                aSerial.textBox1.LostKeyboardFocus += TextBoxLostKeyboardFocus;
                aSerial.SerialItemComplete += this.HandleSerialNumberEntered;

                this.stackPanel1.Children.Add(aSerial);

                if (i == 1)
                {
                    firstSerial = aSerial;
                    FocusHelper.Focus(firstSerial.textBox1);
                }

            }
        }

        private void TextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox source = e.Source as TextBox;

            if (source != null)
            {
                source.Background = Brushes.LightBlue;
                source.SelectAll();
            }
        }

        private void TextBoxLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox source = e.Source as TextBox;

            if (source != null)
            {
                source.Background = Brushes.White;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

        }

        private DataTable GetReportBoxNumber(string BoxNumber)
        {
            DataTable dt = new DataTable();
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_GetPackingRecord";
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 7);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataAdapter rec = new SqlDataAdapter(cmd);
                    rec.Fill(dt);
                    objMainWindow.sqlConnection4.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return dt;
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

        public bool IsSerialAlreadyBox(string Model, string Serial, string BoxNumber, ref string box, ref string po)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_CheckSerialUnPack";
                    cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                    cmd.Parameters["@Model"].Value = Model;
                    cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                    cmd.Parameters["@Serial"].Value = Serial;
                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxUnPack", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxUnPack"].Value = BoxNumber;
                    cmd.Parameters["@BoxUnPack"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    if (rec["Result"].ToString().Trim().Equals("OK"))
                    {
                        objMainWindow.sqlConnection4.Close();
                        return false;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("NG"))
                    {
                        box = "NA";
                        po = "NA";
                        objMainWindow.sqlConnection4.Close();
                        return true;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("NGP"))
                    {
                        rec.NextResult();
                        rec.Read();
                        box = rec["Box"].ToString().Trim();
                        po = rec["PO"].ToString().Trim();
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

        public bool IsSerialAlreadyBoxNew(string Model, string Serial, string BoxNumber, ref string box, ref string po)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_CheckSerialUnPackNewBox";
                    cmd.Parameters.Add("@Model", SqlDbType.Char, 30);
                    cmd.Parameters["@Model"].Value = Model;
                    cmd.Parameters["@Model"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                    cmd.Parameters["@Serial"].Value = Serial;
                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxUnPack", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxUnPack"].Value = BoxNumber;
                    cmd.Parameters["@BoxUnPack"].Direction = ParameterDirection.Input;
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
                        po = rec["PO"].ToString().Trim();
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

        public bool IsBoxExist(string BoxNumber)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_CheckBoxExist";
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    if (rec["Result"].ToString().Trim().Equals("OK"))
                    {
                        objMainWindow.sqlConnection4.Close();
                        return true;
                    }
                    else
                    {
                        objMainWindow.sqlConnection4.Close();
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

        public bool IsSerialAvailable(string Serial, ref string result, ref string rPONumber, ref string rModel, ref string rSerial)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_CheckSerialAvailable";
                    cmd.Parameters.Add("@Serial", SqlDbType.Char, 30);
                    cmd.Parameters["@Serial"].Value = Serial;
                    cmd.Parameters["@Serial"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    if (rec["Result"].ToString().Trim().Equals("OK"))
                    {
                        rec.NextResult();
                        rec.Read();
                        rPONumber = rec["PONumber"].ToString().Trim();
                        rModel=rec["Model"].ToString().Trim();
                        rSerial=rec["Serial"].ToString().Trim();
                        result="OK";
                        objMainWindow.sqlConnection4.Close();
                        return true;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("Model-Serial"))
                    {
                        result="Model-Serial";
                        rPONumber = "NA";
                        rModel="NA";
                        rSerial="NA";
                        objMainWindow.sqlConnection4.Close();
                        return false;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("NG"))
                    {
                        result="NA";
                        rPONumber = "NA";
                        rModel="NA";
                        rSerial="NA";
                        objMainWindow.sqlConnection4.Close();
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

        public void LockSerial(string PONumber, string Model, string Serial, string BoxNumber, string PYear)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
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

        public void LockBox(string PONumber, string BoxNumber)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_tmpUnPackingRecord";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = PONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
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

        public void DeleteBoxUnPack(string BoxNumber, ref int PartQuantity)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_DeleteBoxUnPack";
                    cmd.Parameters.Add("@BoxUnPack", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxUnPack"].Value = BoxNumber;
                    cmd.Parameters["@BoxUnPack"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    PartQuantity = Int32.Parse(rec["PartQuantity"].ToString().Trim());
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void HandleSerialNumberEntered(object sender, EventArgs e)
        {
            string strSerialNumberEntered;
            string strJustSerialNumber="";
            ucSerialNumber mySerial;
            
            try
            {
                mySerial = (ucSerialNumber)sender;
                int iIndex = mySerial.TabIndex;
                int iMaterialLength;
                string boxexist = "NA";
                string poexist = "NA";
                string realresult = "NA";
                string realmodel = "NA";
                string realserial = "NA";
                string realpo = "NA";

                strSerialNumberEntered = mySerial.textBox1.Text.ToString().Trim().ToUpper();
                if ((strSerialNumberEntered.Trim().Length == 0) || (strSerialNumberEntered.Trim().Equals("=")))
                {
                    mySerial.textBox1.Text = "";
                    if (objMainWindow.dataExist)
                    {
                        DoPrint();
                        this.Close();
                    }
                    else
                    {
                        if (objMainWindow.Mode == 3)
                        {
                            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                            {
                                try
                                {
                                    objMainWindow.sqlConnection4.Open();
                                    SqlCommand cmd = new SqlCommand();
                                    cmd.Connection = objMainWindow.sqlConnection4;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.CommandText = "ame_DeleteBoxUnPackApartAll";
                                    cmd.Parameters.Add("@BoxUnPackApart", SqlDbType.Char, 30);
                                    cmd.Parameters["@BoxUnPackApart"].Value = box;
                                    cmd.Parameters["@BoxUnPackApart"].Direction = ParameterDirection.Input;
                                    SqlDataReader rec = cmd.ExecuteReader();
                                    objMainWindow.sqlConnection4.Close();
                                }
                                catch (SqlException ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            MessageBox.Show("Đã tách các parts ra khỏi thùng");
                        }
                        ClosePrintPackSerials();
                    }
                }
                else
                {
                    switch (objMainWindow.Mode)
                    {
                        #region UnPacking All
                        case 0:
                            if (strSerialNumberEntered.Trim().Length != 7 )
                            {
                                MessageBox.Show("Box Number not entered!!");
                                return;
                            }

                            strJustSerialNumber = strSerialNumberEntered.Trim();

                            if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                            {
                                if (IsBoxExist(strJustSerialNumber)==true)
                                {
                                    LockBox(objMainWindow.strPONumber, strJustSerialNumber);

                                    UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                    TraversalRequest request = new TraversalRequest(focusDirection);
                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                    elementWithFocus.MoveFocus(request);
                                }
                                else
                                {
                                    MessageBox.Show("Box Number is not existed");
                                            
                                    UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus1)).Text = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("Box Number already entered!");
                                
                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
                            break;
                        #endregion
                        #region Combine Packing
                        case 1:
                            if (strSerialNumberEntered.Trim().Length < 9)
                            {
                                MessageBox.Show("Model - Serial Number or Serial Number not entered !!");
                                return;
                            }
                           
                            strJustSerialNumber = strSerialNumberEntered.ToString().Trim();
                            
                            if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                            {
                                if (IsSerialAvailable(strJustSerialNumber, ref realresult, ref realpo, ref realmodel, ref realserial))
                                {
                                    if (IsSerialAlreadyBoxNew(realmodel, realserial, box, ref boxexist, ref poexist) == false)
                                    {
                                        LockSerial(realpo, realmodel, realserial, box, objMainWindow.pyear);
                                            
                                        UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;
                                        ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                        FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                        TraversalRequest request = new TraversalRequest(focusDirection);
                                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                        elementWithFocus.MoveFocus(request);
                                    }
                                    else
                                    {
                                        if (poexist == "NA") MessageBox.Show("Serial number đã trong box: " + boxexist);
                                        else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);

                                        UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                        ((TextBox)(elementWithFocus1)).Text = "";
                                    }
                                }
                                else
                                {
                                    if (realresult == "Model-Serial") MessageBox.Show("Scan Model-Serial");
                                    else MessageBox.Show("Serial number không tồn tại");

                                    UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus1)).Text = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("Serial number already entered!");
                                        
                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
                            break;
                        #endregion
                        #region Scan Box Number
                        case 2:
                             if (strSerialNumberEntered.Trim().Length != 7 )
                            {
                                MessageBox.Show("Box Number not entered!!");
                                return;
                            }

                            strJustSerialNumber = strSerialNumberEntered.Trim();

                            if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                            {
                                if (IsBoxExist(strJustSerialNumber)==true)
                                {
                                    LockBox(objMainWindow.strPONumber, strJustSerialNumber);

                                    UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                    TraversalRequest request = new TraversalRequest(focusDirection);
                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                    elementWithFocus.MoveFocus(request);
                                }
                                else
                                {
                                    MessageBox.Show("Box Number is not existed");
                                            
                                    UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus1)).Text = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("Box Number already entered!");
                                
                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
                            break;
                        #endregion
                        #region UnPacking Apart
                        case 3:
                            switch (objMainWindow.strProductMap)
                            {
                                case "FFC":
                                    if (strSerialNumberEntered.Trim().Length < 13)
                                    {
                                        MessageBox.Show("Model - Serial Number not entered!!");
                                        return;
                                    }
                                    iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1).Trim().ToUpper();
                                    objMainWindow.strPrintOverPack = objMainWindow.strPrintFFCOverPack;
                                    objMainWindow.strPrintOverPackContent = objMainWindow.strPrintFFCOverPackContentVN;
                                    break;
                                case "DLM":
                                    if (strSerialNumberEntered.Trim().Length < 13)
                                    {
                                        MessageBox.Show("Model - Serial Number not entered!!");
                                        return;
                                    }
                                    iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1).Trim().ToUpper();
                                    objMainWindow.strPrintOverPack = objMainWindow.strPrintDLMOverPack;
                                    objMainWindow.strPrintOverPackVN = objMainWindow.strPrintDLMOverPackVN;
                                    objMainWindow.strPrintOverPackContent = objMainWindow.strPrintDLMOverPackContent;
                                    break;
                                case "BASE":
                                    if (strSerialNumberEntered.Trim().Length < 9)
                                    {
                                        MessageBox.Show("Serial Number not entered!!");
                                        return;
                                    }
                                    iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1).Trim().ToUpper();
                                    objMainWindow.strPrintOverPack = objMainWindow.strPrintBASEOverPack;
                                    objMainWindow.strPrintOverPackContent = objMainWindow.strPrintBASEOverPackContent;
                                    break;
                            }

                            if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                            {
                                if (IsSerialAlreadyBox(objMainWindow.strPOMaterial, strJustSerialNumber, box, ref boxexist, ref poexist)==false)
                                {
                                    LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strJustSerialNumber.Trim(),
                                        box, objMainWindow.pyear);

                                    UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                    TraversalRequest request = new TraversalRequest(focusDirection);
                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                    elementWithFocus.MoveFocus(request);
                                }
                                else
                                {
                                    if (poexist == "NA") MessageBox.Show("Serial number không tồn tại");
                                    else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);
                                }

                                UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus1)).Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Serial number already entered!");

                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
                            break;
                        #endregion
                        #region Create New Box
                        case 4:
                            if (strSerialNumberEntered.Trim().Length < 9)
                            {
                                MessageBox.Show("Model - Serial Number or Serial Number not entered !!");
                                return;
                            }
                           
                            strJustSerialNumber = strSerialNumberEntered.ToString().Trim();
                            
                            if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                            {
                                if (IsSerialAvailable(strJustSerialNumber, ref realresult, ref realpo, ref realmodel, ref realserial))
                                {
                                    if (IsSerialAlreadyBoxNew(realmodel, realserial, box, ref boxexist, ref poexist) == false)
                                    {
                                        LockSerial(realpo, realmodel, realserial, box, objMainWindow.pyear);
                                            
                                        UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;
                                        ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                        FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                        TraversalRequest request = new TraversalRequest(focusDirection);
                                        UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                        elementWithFocus.MoveFocus(request);
                                    }
                                    else
                                    {
                                        if (poexist == "NA") MessageBox.Show("Serial number đã trong box: " + boxexist);
                                        else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);

                                        UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                        ((TextBox)(elementWithFocus1)).Text = "";
                                    }
                                }
                                else
                                {
                                    if (realresult == "Model-Serial") MessageBox.Show("Scan Model-Serial");
                                    else MessageBox.Show("Serial number không tồn tại");

                                    UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus1)).Text = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("Serial number already entered!");
                                        
                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
                            break;
                        #endregion
                        #region UnPacking + New Box
                        case 6:
                            switch (objMainWindow.strProductMap)
                            {
                                case "FFC":
                                    if (strSerialNumberEntered.Trim().Length < 13)
                                    {
                                        MessageBox.Show("Model - Serial Number not entered!!");
                                        return;
                                    }
                                    iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1).Trim().ToUpper();
                                    objMainWindow.strPrintOverPack = objMainWindow.strPrintFFCOverPack;
                                    objMainWindow.strPrintOverPackContent = objMainWindow.strPrintFFCOverPackContentVN;
                                    break;
                                case "DLM":
                                    if (strSerialNumberEntered.Trim().Length < 13)
                                    {
                                        MessageBox.Show("Model - Serial Number not entered!!");
                                        return;
                                    }
                                    iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1).Trim().ToUpper();
                                    objMainWindow.strPrintOverPack = objMainWindow.strPrintDLMOverPack;
                                    objMainWindow.strPrintOverPackVN = objMainWindow.strPrintDLMOverPackVN;
                                    objMainWindow.strPrintOverPackContent = objMainWindow.strPrintDLMOverPackContent;
                                    break;
                                case "BASE":
                                    if (strSerialNumberEntered.Trim().Length < 9)
                                    {
                                        MessageBox.Show("Serial Number not entered!!");
                                        return;
                                    }
                                    iMaterialLength = objMainWindow.strCurrentMaterial.Trim().Length;
                                    strJustSerialNumber = strSerialNumberEntered.Substring(iMaterialLength + 1).Trim().ToUpper();
                                    objMainWindow.strPrintOverPack = objMainWindow.strPrintBASEOverPack;
                                    objMainWindow.strPrintOverPackContent = objMainWindow.strPrintBASEOverPackContent;
                                    break;
                            }

                            if (IsSerialAlreadyEntered(strSerialNumberEntered, mySerial) == false)
                            {
                                if (IsSerialAlreadyBox(objMainWindow.strPOMaterial, strJustSerialNumber, box, ref boxexist, ref poexist) == false)
                                {
                                    LockSerial(objMainWindow.strPONumber, objMainWindow.strPOMaterial, strJustSerialNumber.Trim(),
                                        box, objMainWindow.pyear);

                                    UIElement elementWithFocus2 = Keyboard.FocusedElement as UIElement;
                                    ((TextBox)(elementWithFocus2)).IsEnabled = false;

                                    FocusNavigationDirection focusDirection = FocusNavigationDirection.Down;
                                    TraversalRequest request = new TraversalRequest(focusDirection);
                                    UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                    elementWithFocus.MoveFocus(request);
                                }
                                else
                                {
                                    if (poexist == "NA") MessageBox.Show("Serial number không tồn tại");
                                    else MessageBox.Show("Serial number đang được packing trong po: " + poexist + " và box: " + boxexist);
                                }

                                UIElement elementWithFocus1 = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus1)).Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Serial number already entered!");

                                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
                                ((TextBox)(elementWithFocus)).Text = "";
                            }
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

        private void DoScanBoxNumber()
        {
            DataTable dt = new DataTable();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Packing.xlsx";
            File.Copy(objMainWindow.strReportBoxNumber, path, true);
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path +
                ";Extended Properties=\"Excel 8.0;HDR=Yes\";";
            OleDbConnection con = new OleDbConnection(connectionString);

            int iCounter = 0;

            SortedList<string, ucSerialNumber> slSortedItems = new SortedList<string, ucSerialNumber>();

            try
            {
                for (int i = 0; i < objMainWindow.iMaxSerialsPerOrder; i++)
                {
                    try
                    {
                        ucSerialNumber aSerialEntry = (ucSerialNumber)(this.stackPanel1.Children[i]);
                        if (aSerialEntry.textBox1.Text.ToString().Trim().Length > 0)
                        {
                            slSortedItems.Add(aSerialEntry.textBox1.Text.ToString().Trim(), aSerialEntry);
                            iCounter++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Indexing error i= " + i.ToString() + " iCounter=" + iCounter.ToString());
                    }
                }

                for (int i = 0; i < slSortedItems.Count(); i++)
                {
                    string strBoxNumber = slSortedItems.Keys[i].ToString().Trim().ToUpper();
                    dt = GetReportBoxNumber(strBoxNumber);

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
                        string selectString = "Insert into [Sheet1$] ([BoxNumber],[PONumber],[Model],[Serial]) " +
                            "values (@BoxNumber,@PONumber,@Model,@Serial)";
                        con.Open();
                        OleDbCommand cmd = new OleDbCommand();
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@box", OleDbType.Char).Value = strBoxNumber;
                        cmd.Parameters.Add("@PONumber", OleDbType.Char).Value = PONumber;
                        cmd.Parameters.Add("@Model", OleDbType.Char).Value = Model;
                        cmd.Parameters.Add("@Serial", OleDbType.Char).Value = Serial;
                        cmd.CommandText = selectString;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                MessageBox.Show("Đã export thành công, file Packing.xlsx ngoài desktop");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DoPrint()
        {
            try
            {
                switch (objMainWindow.Mode)
                {
                   case 0: //UnPacking All
                        DoPrintUnPackingAll();
                        break;
                   case 1: //Combine Packing
                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
                            try
                            {
                                objMainWindow.sqlConnection4.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = objMainWindow.sqlConnection4;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "ame_DeleteBoxComPackApart";
                                cmd.Parameters.Add("@BoxComPackApart", SqlDbType.Char, 30);
                                cmd.Parameters["@BoxComPackApart"].Value = box;
                                cmd.Parameters["@BoxComPackApart"].Direction = ParameterDirection.Input;
                                SqlDataReader rec = cmd.ExecuteReader();
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        break;
                    case 2: //Scan Box Number
                        DoScanBoxNumber();
                        break;
                    case 3: //UnPacking Apart
                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
                            try
                            {
                                objMainWindow.sqlConnection4.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = objMainWindow.sqlConnection4;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "ame_DeleteBoxUnPackApart";
                                cmd.Parameters.Add("@BoxUnPackApart", SqlDbType.Char, 30);
                                cmd.Parameters["@BoxUnPackApart"].Value = box;
                                cmd.Parameters["@BoxUnPackApart"].Direction = ParameterDirection.Input;
                                SqlDataReader rec = cmd.ExecuteReader();
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        break;
                    case 4: //Create New Box
                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
                            try
                            {
                                objMainWindow.sqlConnection4.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = objMainWindow.sqlConnection4;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "ame_DeleteBoxNewPack";
                                cmd.Parameters.Add("@BoxNewPack", SqlDbType.Char, 30);
                                cmd.Parameters["@BoxNewPack"].Value = box;
                                cmd.Parameters["@BoxNewPack"].Direction = ParameterDirection.Input;
                                SqlDataReader rec = cmd.ExecuteReader();
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        break;
                    case 6: //UnPacking + New Box
                        using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                        {
                            try
                            {
                                objMainWindow.sqlConnection4.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd.Connection = objMainWindow.sqlConnection4;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.CommandText = "ame_DeleteBoxUnPackApartNew";
                                cmd.Parameters.Add("@BoxUnPackApart", SqlDbType.Char, 30);
                                cmd.Parameters["@BoxUnPackApart"].Value = box;
                                cmd.Parameters["@BoxUnPackApart"].Direction = ParameterDirection.Input;
                                cmd.Parameters.Add("@BoxUnPackNew", SqlDbType.Char, 30);
                                cmd.Parameters["@BoxUnPackNew"].Value = boxnew;
                                cmd.Parameters["@BoxUnPackNew"].Direction = ParameterDirection.Input;
                                SqlDataReader rec = cmd.ExecuteReader();
                                objMainWindow.sqlConnection4.Close();
                            }
                            catch (SqlException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        break;
                    }
                    if (objMainWindow.Mode == 6)
                    {
                        DoBoxPrint(boxnew);
                        DoBoxPrint(box);
                    }
                    else
                    {
                        UpdateRecord(box);
                        DoBoxPrint(box);
                    }
                    ClosePrintPackSerials();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        public void DoPrintUnPackingAll()
        {
            int iCounter = 0;
            int tmpPartQuantity = 0;

            SortedList<string, ucSerialNumber> slSortedItems = new SortedList<string, ucSerialNumber>();
            objMainWindow.slRePrint.Clear();

            try
            {
                objMainWindow.btLabel.setPrintFile(3, objMainWindow.strPrintUnPackingAll);
                objMainWindow.btLabel.clearAllFieldValues(3);

                for (int i = 0; i < objMainWindow.iMaxSerialsPerOrder; i++)
                {
                    try
                    {
                        ucSerialNumber aSerialEntry = (ucSerialNumber)(this.stackPanel1.Children[i]);
                        if (aSerialEntry.textBox1.Text.ToString().Trim().Length > 0)
                        {
                            slSortedItems.Add(aSerialEntry.textBox1.Text.ToString().Trim(), aSerialEntry);
                            iCounter++;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Indexing error i= " + i.ToString() + " iCounter=" + iCounter.ToString());
                    }
                }

                objMainWindow.slRePrint = slSortedItems;

                for (int i = 0; i < slSortedItems.Count(); i++)
                {
                    string strBoxNumber = slSortedItems.Keys[i].ToString().Trim().ToUpper();
                    DeleteBoxUnPack(strBoxNumber, ref tmpPartQuantity);
                    objMainWindow.PartQuantity += tmpPartQuantity;
                }

                DoPrintOverPack(slSortedItems, objMainWindow.PartQuantity);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DoBoxPrint(string BoxNumber)
        {
            int iCounter = 0;
            objMainWindow.box = BoxNumber;
            string[] lstPONumber = new string[0] ;
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_getBoxPrintInfo";
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    lstPONumber = new string[Int32.Parse(rec["Qty"].ToString().Trim())];
                    rec.NextResult();
                    int i = 0;
                    while (rec.Read())
                    {
                        lstPONumber[i]=rec["PONumber"].ToString().Trim();
                        i++;
                    }
                    objMainWindow.sqlConnection4.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Error Get PO Box Print:" + ex.Message);
                    this.Close();
                }
            }

            foreach (string tmpPONumber in lstPONumber)
            {
                objMainWindow.getPOInfo(tmpPONumber);
                objMainWindow.getPrintFilesLabel(objMainWindow.strProductMap);
                SortedList<string, string> slSortedItems = new SortedList<string, string>();
                using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                {
                    try
                    {
                        objMainWindow.sqlConnection4.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = objMainWindow.sqlConnection4;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ame_getBoxPrintData";
                        cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                        cmd.Parameters["@PONumber"].Value = objMainWindow.strPONumber;
                        cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                        cmd.Parameters["@BoxNumber"].Value = BoxNumber;
                        cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                        SqlDataReader rec = cmd.ExecuteReader();                        
                        while(rec.Read())
                        {
                            slSortedItems.Add(objMainWindow.strPOMaterial + "-" + rec["Serial"].ToString().Trim(), rec["Serial"].ToString().Trim());
                        }
                        objMainWindow.sqlConnection4.Close();
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Error Get PO Box Print Data:" + ex.Message);
                        this.Close();
                    }

                    try
                    {
                        iCounter = slSortedItems.Count();
                        string strReturn = objMainWindow.PrintBoxLabel(objMainWindow, iCounter);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    try
                    {
                        objMainWindow.btLabel.setPrintFile(3, objMainWindow.strPrintOverPack);
                        objMainWindow.btLabel.clearAllFieldValues(3);
                        DoBoxPrintOverPack(slSortedItems);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void DoBoxPrintOverPack(SortedList<string, string> slSortedItems)
        {
            int iCounter = slSortedItems.Count();
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

                    /*
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
                    */
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

        private void DoPrintOverPack(SortedList<string, ucSerialNumber> slSortedItems, int PartQuantity)
        {
            int iCounter = slSortedItems.Count();
            int iPages = 0;
            string strSerialFieldName;
            string strSerialFieldNameNo;
            string strSerialFieldRoot = ConfigurationManager.AppSettings.Get("OverPackFieldRoot").ToString();
            string strSerialFieldRootNo = ConfigurationManager.AppSettings.Get("OverPackFieldRootNo").ToString();
            string strTwoDFieldName = ConfigurationManager.AppSettings.Get("OverPackTwoDField").ToString();
            string strBoxQuantityField = ConfigurationManager.AppSettings.Get("BoxQuantity").ToString();
            string strPartQuantityField = ConfigurationManager.AppSettings.Get("PartQuantity").ToString();

            switch (objMainWindow.Mode)
            {
                case 0:
                    if (iCounter % objMainWindow.iMaxSerialsUnPackingAll == 0) iPages = (iCounter / objMainWindow.iMaxSerialsUnPackingAll);
                    else iPages = (iCounter / objMainWindow.iMaxSerialsUnPackingAll) + 1;

                    for (int iPageNumber = 1; iPageNumber <= iPages; iPageNumber++)
                    {
                        int iItemsForThisPage;
                        int iStartingIndex;
                        iStartingIndex = (iPageNumber - 1) * objMainWindow.iMaxSerialsUnPackingAll;
                        if (iPageNumber == iPages)
                        {
                            iItemsForThisPage = iCounter - iStartingIndex;
                        }
                        else
                        {
                            iItemsForThisPage = objMainWindow.iMaxSerialsUnPackingAll;
                        }

                        string strFieldValue;
                        string strFieldValueNo;
                        objMainWindow.btLabel.findFieldandSubstitute(3, strBoxQuantityField, iCounter.ToString());
                        objMainWindow.btLabel.findFieldandSubstitute(3, strPartQuantityField, PartQuantity.ToString());
                        for (int jPrintPageIndex = 0; jPrintPageIndex < iItemsForThisPage; jPrintPageIndex++)
                        {
                            strSerialFieldName = strSerialFieldRoot + jPrintPageIndex.ToString().Trim();
                            strSerialFieldNameNo = strSerialFieldRootNo + jPrintPageIndex.ToString().Trim();

                            string strSortedFieldValue = slSortedItems.Keys[iStartingIndex + jPrintPageIndex].ToString().Trim().ToUpper();

                            strFieldValue = strSortedFieldValue;
                            strFieldValueNo = ((jPrintPageIndex + 1) + ((iPageNumber - 1) * objMainWindow.iMaxSerialsUnPackingAll)).ToString();

                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldName, strFieldValue);
                            objMainWindow.btLabel.findFieldandSubstitute(3, strSerialFieldNameNo, strFieldValueNo);
                        }
                        objMainWindow.btLabel.doPrint(3, false, false);
                        for (int k = 0; k < objMainWindow.iMaxSerialsUnPackingAll; k++)
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

        public void DoRePrint(WareHouse objMainWindow)
        {
            int iCounter = objMainWindow.slRePrint.Count();
            if (objMainWindow.Mode == 0) //UnPackingAll
            {
                try
                {
                    objMainWindow.btLabel.setPrintFile(3, objMainWindow.strPrintUnPackingAll);
                    objMainWindow.btLabel.clearAllFieldValues(3);

                    DoPrintOverPack(objMainWindow.slRePrint, objMainWindow.PartQuantity);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (objMainWindow.Mode == 2) //Scan Box Number
            {
                return;
            }
            else
            {
                DoBoxPrint(box);
                if (objMainWindow.Mode == 6) DoBoxPrint(boxnew);
            }
        }

        private void UpdateRecord(string BoxNumber)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_PackingRecord1";
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
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

        private void ClearTmpRecord(string BoxNumber)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_ClearPackingRecord1";
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = BoxNumber;
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
            switch (objMainWindow.Mode)
            {
                case 0:
                    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                    {
                        try
                        {
                            objMainWindow.sqlConnection4.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = objMainWindow.sqlConnection4;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "ame_ClearBoxUnPack";
                            cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                            cmd.Parameters["@PONumber"].Value = objMainWindow.strPONumber;
                            cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                            SqlDataReader rec = cmd.ExecuteReader();
                            objMainWindow.sqlConnection4.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error Clear Box Rework:" + ex.Message);
                        }
                    }
                    break;
                case 3:
                    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
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
                            cmd.Parameters["@BoxNumber"].Value = box;
                            cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                            SqlDataReader rec = cmd.ExecuteReader();
                            objMainWindow.sqlConnection4.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error Clear Box Rework:" + ex.Message);
                        }
                    }
                    break;
                case 4:
                    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                    {
                        try
                        {
                            objMainWindow.sqlConnection4.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = objMainWindow.sqlConnection4;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "ame_ClearBoxNumber";
                            cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                            cmd.Parameters["@BoxNumber"].Value = box;
                            cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                            SqlDataReader rec = cmd.ExecuteReader();
                            objMainWindow.sqlConnection4.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error Clear Box Number:" + ex.Message);
                        }
                    }
                    break;
                case 6:
                    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
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
                            cmd.Parameters["@BoxNumber"].Value = box;
                            cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                            SqlDataReader rec = cmd.ExecuteReader();
                            objMainWindow.sqlConnection4.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error Clear Box Rework:" + ex.Message);
                        }
                    }

                    using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4))
                    {
                        try
                        {
                            objMainWindow.sqlConnection4.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = objMainWindow.sqlConnection4;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "ame_ClearBoxNumber";
                            cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                            cmd.Parameters["@BoxNumber"].Value = boxnew;
                            cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                            SqlDataReader rec = cmd.ExecuteReader();
                            objMainWindow.sqlConnection4.Close();
                        }
                        catch (SqlException ex)
                        {
                            MessageBox.Show("Error Clear Box Number:" + ex.Message);
                        }
                    }
                    //ClearTmpRecord(objMainWindow.boxnew);
                    break;
            }
            objMainWindow.bolwoACS = false;
            objMainWindow.dataExist = false;
            ClearTmpRecord(box);
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
                DoPrint();
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
}