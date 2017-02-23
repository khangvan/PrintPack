using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data;
//using BarTender;
using System.Diagnostics;


namespace PrintPack
{
    public class BartenderBusiness
    {

        #region Khai báo ban đầu
        public BarTender.ApplicationClass btApp;
        public BarTender.Format btFormat;
        public BarTender.Messages btMsgs;

        public string strpath;

        public List<string> ListSharename;

        #endregion

        public void KillBTprocess()
        {
            Process[] prs = Process.GetProcesses();

            const int allowBT = 3;
            int i = 0;

            foreach (Process pr in prs)
            {
                if (pr.ProcessName == "bartend")
                {
                    i++;
                   bool WillKill= (i > allowBT) ? true : false;

                    if(WillKill) { pr.Kill(); }
                }
                

            }
        }
        public BartenderBusiness()
        {
            Khoitao();
        }
        protected void Khoitao()
        {
            //*** Can set up Interop.bartender properties : Embed interop type =false => TRUE se bao loi
            //KillBTprocess();

            btApp = new BarTender.ApplicationClass();
            btFormat = new BarTender.FormatClass();
            ListSharename = new List<string>();
            ListSharename = null;
        }
        public void GanDuongDanBTlabel(string path)
        {
            strpath = path;
            btFormat = btApp.Formats.Open(strpath, true, "");//reload
        }

        public List<string> LayListSharename()
        {
            List<string> lst = new List<string>();
            
            string strlistSharename = "";
          

            foreach (BarTender.SubString btSubString in btFormat.NamedSubStrings)
            {
                //MessageBox.Show("Name is: " + btSubString.Name + "\n\r Full is: " + btFormat.NamedSubStrings.GetAll(",", ":"));
                strlistSharename += btSubString.Name + " ";

            }
            strlistSharename = strlistSharename.Trim();//clear end blank=problem at get sharename from list

            //string a = PhanMem.listation4setuplabel;
            string[] words = strlistSharename.Split(' ');
            foreach (string i in words)
            {
                lst.Add(i.ToString());
            }

            return lst;
           
        }
        public void GanMayIN(string path)
        {

            btFormat.PrintSetup.Printer = path;
        }

        public void GanShareNameWithValueBTlabel(string ShareName, string AssignValue)
        {

            btFormat.SetNamedSubStringValue(ShareName, AssignValue);
        }
        public List<string> GetListFieldNameFromBTlabel()
        {
            List<string> DsShareName = new List<string>();
            
            try
            {
                
                string strlistSharename = "";
                // lblLinkBT.Text = Labelpath2print;
                //listShareName.Items.Clear();
                //loadField of bartender to data
                btFormat = btApp.Formats.Open(strpath, false, "");

                btFormat = btApp.Formats.Open(strpath, true, "");//reload
                //MessageBox.Show(btFormat.NamedSubStrings.GetAll(",", ":"));
                foreach (BarTender.SubString btSubString in btFormat.NamedSubStrings)
                {
                    //MessageBox.Show("Name is: " + btSubString.Name + "\n\r Full is: " + btFormat.NamedSubStrings.GetAll(",", ":"));
                    strlistSharename += btSubString.Name + " ";

                }
                strlistSharename = strlistSharename.Trim();//clear end blank=problem at get sharename from list

                //string a = PhanMem.listation4setuplabel;
                string[] words = strlistSharename.Split(' ');
                DsShareName.Clear();
                foreach (string i in words)
                {
                    //MessageBox.Show(i);
                    //DsShareName.Add("ok ne");
                    DsShareName.Add(i);
                    //DsShareName.Add(i.ToString());
                }

                DsShareName.Sort();
                return DsShareName;
                //loadField of bartender to data

                //load copied setup
                //txtCopied.Value = Convert.ToDecimal(btFormat.IdenticalCopiesOfLabel);
                //load copied setup
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
                return DsShareName;
            }

        }
        

        public void GansoluongNhancanin(Int32 i)
        {
            btFormat.IdenticalCopiesOfLabel = i;
        }

        public void GansoluongNhancanin(string i)
        {
            btFormat.IdenticalCopiesOfLabel = Convert.ToInt32(i);
        }

        public void ThucHienIn()
        {
            
            

            btFormat.Print(DateTime.Now.ToShortTimeString(), true, 1, out btMsgs);
        }

        public void GanSoCottrongPageSetup(Int32 iCollums)
        {

            btFormat.PageSetup.LabelColumns = iCollums;
        }
        public void GanSoDongtrongPageSetup(Int32 iRows)
        {

            btFormat.PageSetup.LabelRows = iRows;
        }
        public void HuydoituongBT()
        {
            btApp.Quit(BarTender.BtSaveOptions.btDoNotSaveChanges);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(btApp);
        }
        public void HuydoituongBT_withSave()
        {
            btApp.Quit(BarTender.BtSaveOptions.btSaveChanges);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(btApp);
        }


        
    }
}
