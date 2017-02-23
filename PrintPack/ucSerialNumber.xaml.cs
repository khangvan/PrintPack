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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintPack
{
    /// <summary>
    /// Interaction logic for ucSerialNumber.xaml
    /// </summary>
    public partial class ucSerialNumber : UserControl
    {
        public MainWindow objMainWindow;
        public event EventHandler SerialItemComplete;

        public ucSerialNumber()
        {
            InitializeComponent();
        }


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
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Enter)
            {
                textBox1.Text = textBox1.Text.ToUpper();

                SerialItemComplete(this, new EventArgs());
//                KeyEventArgs tabEvent = new KeyEventArgs(e.KeyboardDevice, e.InputSource, e.Timestamp, Key.Tab);
                
//                this.RaiseEvent(tabEvent) ;
            }
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
                pn = strPNtocheck;
                string strSNtocheck = str_i_input.Substring(astart, aget);
                sn = strSNtocheck;

                //if (strPNtocheck.ToUpper() != strmodelstd.ToUpper())
                //{
                //    return " Model không dúng ! Kiem tra:" + strPNtocheck;
                //}

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
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
