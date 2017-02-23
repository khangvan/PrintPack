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

namespace PrintPack
{
    /// <summary>
    /// Interaction logic for BoxRework.xaml
    /// </summary>
    public partial class BoxRework : Window
    {
        public MainWindow objMainWindow;
        public BoxRework()
        {
            InitializeComponent();

            
            
            
          
            

        }

        public BoxRework(MainWindow objMyMainWindow) : this()
        {
            objMainWindow = objMyMainWindow;
            textBox1.Text = objMyMainWindow.txtProdOrder.Text;
            

            
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            return;

            objMainWindow.strBoxReworkresult = "Cancel";
            objMainWindow.box = "NA";
            objMainWindow.strPONumber = "";
            ClearBoxRework();
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            if (e.Key == Key.Enter)
            {
                if (textBox1.Text.ToString().Trim().Length > 5)
                {
                    
                    objMainWindow.strPONumber = textBox1.Text.ToString().PadLeft(12, '0');
                    textBox1.Text = objMainWindow.strPONumber;
                    Boolean bolresult = true;//CheckPOExist();
                    if (bolresult)
                    {
                        textBox1.IsEnabled = false;
                        textBox2.IsEnabled = true;
                        textBox2.Focus();
                    }
                    else
                    {
                        MessageBox.Show("PO không tồn tại trên hệ thống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                textBox1.IsEnabled = true;
                textBox2.Text = "";
                textBox2.IsEnabled = false;
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            if (e.Key == Key.Enter)
            {
                textBox2.Text = textBox2.Text.Trim().ToUpper();
                if (textBox2.Text.ToString().Trim().Length > 0)
                {

                    // do clear box
                    ClearPAckingRecordAndUpdateTinformation();



                    //do update partrun

                    MessageBox.Show("Hoàn tất xóa box, vui lòng đóng gói lại vào box mới !");
                    this.Close();
                    return;
                    //older code
                    objMainWindow.box = textBox2.Text.ToString().Trim();
                    string result = "NA";
                    Boolean bolresult = CheckBoxRework(ref result);
                    if (bolresult)
                    {
                        objMainWindow.strBoxReworkresult = "OK";
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        textBox2.Focus();
                        textBox2.SelectAll();
                    }
                }

                if (textBox2.Text=="")
                {
                    //update PO qty
                    UpdateTinformation();
                    MessageBox.Show("Hòan tất cập nhật số lượng PO, Vui lòng kiểm tra số lượng đã đóng gói !");

                    
                    this.Close();
                    return;
                }
                

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox1.IsEnabled = true;
            textBox2.IsEnabled = false;
            textBox1.Focus();
            textBox1.SelectAll();
        }

        #region CheckFunction
        private bool CheckBoxRework(ref string result)
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_CheckBoxRework";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = objMainWindow.strPONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxRework", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxRework"].Value = objMainWindow.box;
                    cmd.Parameters["@BoxRework"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();
                    if (rec["Result"].ToString().Trim().Equals("OK"))
                    {
                        result = "OK";
                        objMainWindow.sqlConnection4.Close();
                        return true;
                    }
                    else if (rec["Result"].ToString().Trim().Equals("NG"))
                    {
                        result = "Không tồn tại Box này";
                        objMainWindow.sqlConnection4.Close();
                        return false;
                    }
                    else
                    {
                        result = rec["Result"].ToString().Trim();
                        objMainWindow.sqlConnection4.Close();
                        return false;
                    }
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }

        private bool CheckPOExist()
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_CheckPOExist";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = objMainWindow.strPONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@Station", SqlDbType.Char, 30);
                    cmd.Parameters["@Station"].Value = objMainWindow.strSTATION;
                    cmd.Parameters["@Station"].Direction = ParameterDirection.Input;
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
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }

        private void ClearBoxRework()
        {
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
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ClearPAckingRecordAndUpdateTinformation()
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_ClearPackingRecordNUpdateTInformation";

                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = textBox1.Text.Trim();
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    cmd.Parameters["@BoxNumber"].Value = textBox2.Text.Trim();
                    cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateTinformation()
        {
            using (objMainWindow.sqlConnection4 = new SqlConnection(objMainWindow.strSqlConnection4_608FFCPACKING))
            {
                try
                {
                    objMainWindow.sqlConnection4.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = objMainWindow.sqlConnection4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "ame_UpdatePartRun";

                    cmd.Parameters.Add("@ProdOrder", SqlDbType.Char, 30);
                    cmd.Parameters["@ProdOrder"].Value = textBox1.Text.Trim();
                    cmd.Parameters["@ProdOrder"].Direction = ParameterDirection.Input;
                    //cmd.Parameters.Add("@BoxNumber", SqlDbType.Char, 30);
                    //cmd.Parameters["@BoxNumber"].Value = objMainWindow.box;
                    //cmd.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    SqlDataReader rec = cmd.ExecuteReader();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        #endregion

      

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BusinessPackingRecord.CheckSNListandReloadifProblem(textBox1.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((txtSNChecking.Text !="") || (textBox1.Text !=""))
            {

            BusinessPackingRecord.CheckSNperLoadedListandReloadifProblem(txtSNChecking.Text, textBox1.Text);
            }
            else
            {

                MessageBox.Show("Nhập số PO và SN");
            }
        }
    }
}
