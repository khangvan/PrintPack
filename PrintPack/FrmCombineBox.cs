using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.IO;
using System.Management;
using System.Drawing.Printing;
using System.Configuration;





namespace PrintPack
{
    public partial class FrmCombineBox : Form
    {
        int sosntoidatren1nhan = 15;
        public FrmCombineBox()
        {
            InitializeComponent();
        }
        int countPackSN;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //update process

            //get box number
            string strboxnumber = GetBOXNUMBER().ToUpper();
            txtBOX.Text = strboxnumber;
            // add current SN to listbox
            foreach (var sn in SNs)
            {
                FFCPackingDataSet.PackingRecordDataTable dt = new FFCPackingDataSet.PackingRecordDataTable();
                FFCPackingDataSetTableAdapters.PackingRecordTableAdapter da = new FFCPackingDataSetTableAdapters.PackingRecordTableAdapter();

                da.UpdateQueryNewBox(strboxnumber, DateTime.Now, sn.Serial.ToString(), txtPO.Text.Trim());


            }
            //update remove list-> unpack box getbox = po unpack
            string RPKbox = "";
            foreach (var sn in SNsRemove)
            {
                FFCPackingDataSet.PackingRecordDataTable dt = new FFCPackingDataSet.PackingRecordDataTable();
                FFCPackingDataSetTableAdapters.PackingRecordTableAdapter da = new FFCPackingDataSetTableAdapters.PackingRecordTableAdapter();

                string strPORepack = txtPO.Text.Trim();
                RPKbox = "RPK" + strPORepack.Substring(strPORepack.Length - 6, 6);

                da.UpdateQueryNewBox(RPKbox, DateTime.Now, sn.Serial.ToString(), txtPO.Text.Trim());


            }

            //do print



            DoPrint(SNs, strboxnumber);
            

            DoPrint(SNsRemove, RPKbox);

            KhoitaoSNnLuoi();
        }

        private void DoPrint(List<clsSerialInput> ListSNs, string Boxnumber )
        {

            #region Khởi tạo vị trí đặt nhãn
            //string LocatedFolder =@"\\vnmsrv859\bformats\PREPRINT\";
            //string LocatedFolder = Application.StartupPath+ @"\\Label\";// AP DUNG CHO LOCAL FILE

            string LocatedFolder = @"C:\PrintPackStation\Label\FFC\";

            string nhanOverPack = @"APAC-OverPackContent.btw";
            string nhanOverPackContent = @"APAC-OverPack.btw";

            nhanOverPack = LocatedFolder + nhanOverPack;
            nhanOverPackContent = LocatedFolder + nhanOverPackContent;
            //get info Order
            string Model = txtPN.Text.ToUpper();
            //Model ="ABCD";
            //get date
            //get description
            //get sn
            int Packnumber = ListSNs.Count;
            #endregion


            #region Đếm_Số_SN_trong_box_để_in_ra_OVerPackLabel

            List<clsSerialInput> SortedList = ListSNs.OrderBy(o => o.Serial).ToList();


            //var listsn = SNs.Select(x => x.Serial).ToList();//ok
            var listsn = SortedList.Select(x => x.Serial).ToList();

            string[] myArray = listsn.ToArray();
            countPackSN = 0;
            foreach (string value in myArray)
            {
                if (value != "") ;
                ++countPackSN;
            }
            #endregion

            #region Gán số thông tin cho Overpack và IN
            //print overpack
            string datetimenow = DateTime.Now.Day.ToString() + " " + DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + DateTime.Now.Year.ToString();

            LabelPrint.GanDuongDanBTlabel(nhanOverPack);
            LabelPrint.GanShareNameWithValueBTlabel("SPART", txtPN.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("PARTREV", txtRev.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("DESC", txtDes.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("ZDATE", datetimenow/*DateTime.Now.ToShortDateString()*/);
            LabelPrint.GanShareNameWithValueBTlabel("AMT", countPackSN.ToString());
            LabelPrint.GanShareNameWithValueBTlabel("PRODORDER", this.txtPO.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("MANPACK", this.cboPackingPlace.Text.ToString());
            LabelPrint.GanShareNameWithValueBTlabel("CORIGIN", this.cboManuafacturingIn.Text.ToUpper());

            LabelPrint.GanShareNameWithValueBTlabel("BOX", Boxnumber);


            LabelPrint.GansoluongNhancanin(1);

            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            #endregion

            #region Chia số SN theo số lượng trên 15SNperlabel
            ///Input
            ///Output
            #region Tính số trang SN cần in_Input: Số record tổng cộng, số SNperLabel; Output: số trang (đã được RoundUP)
            ///
            int npage = (countPackSN + sosntoidatren1nhan - 1) / sosntoidatren1nhan;//(countPackSN/sosntoidatren1nhan)+1; //+1 cho test thôi nha

            #endregion
            //for (int i=0; i< npage; i++)
            //{
            //    var listtoprint = listsn.Skip(i*sosntoidatren1nhan).Take(sosntoidatren1nhan).ToList();
            //}

            //}
            #endregion

            #region Gán thông tin cho nhãn OverpackContent_nhãn SN và IN
            //print sn
            LabelPrint.GanDuongDanBTlabel(nhanOverPackContent);
            List<string> lstget = LabelPrint.GetListFieldNameFromBTlabel();
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
                        LabelPrint.GanShareNameWithValueBTlabel("SPSN" + n, "");
                        LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + n, "");

                    }

                }
                // gan cac cot co value
                for (int j = 0; j <= sosntrentungtrang - 1; j++)
                {

                    if (myArray[0].Length != 0)
                    {
                        LabelPrint.GanShareNameWithValueBTlabel("SPSN" + j, myArray[j]);
                        LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + j, Model + "-" + myArray[j]);
                        #region Combine data cho nhãn 2D
                        if (myArray[j] != "")
                        {

                            PN2D += Model + "-" + myArray[j];
                            PN2D += ",";
                        }
                        #endregion

                    }

                }
                LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIALALL", PN2D.Substring(0, PN2D.Length - 1));

                LabelPrint.GansoluongNhancanin(1);

                LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
                LabelPrint.ThucHienIn();
            }
            #endregion


            MessageBox.Show(string.Format("Đã in nhãn cho {0} Serial", countPackSN));
        }

        private void DoPrintfromBOX(string Boxnumber)
        {
            IsFirstBoxEnter = true;
            AddSNSfrmbox(Boxnumber, SNs);
            List<clsSerialInput> ListSNs = SNs;

            #region Khởi tạo vị trí đặt nhãn
            //string LocatedFolder =@"\\vnmsrv859\bformats\PREPRINT\";
            //string LocatedFolder = Application.StartupPath+ @"\\Label\";// AP DUNG CHO LOCAL FILE

            string LocatedFolder = @"C:\PrintPackStation\Label\FFC\";

            string nhanOverPack = @"APAC-OverPackContent.btw";
            string nhanOverPackContent = @"APAC-OverPack.btw";

            nhanOverPack = LocatedFolder + nhanOverPack;
            nhanOverPackContent = LocatedFolder + nhanOverPackContent;
            //get info Order
            string Model = txtPN.Text.ToUpper();
            //Model ="ABCD";
            //get date
            //get description
            //get sn
            int Packnumber = ListSNs.Count;
            #endregion


            #region Đếm_Số_SN_trong_box_để_in_ra_OVerPackLabel

            List<clsSerialInput> SortedList = ListSNs.OrderBy(o => o.Serial).ToList();


            //var listsn = SNs.Select(x => x.Serial).ToList();//ok
            var listsn = SortedList.Select(x => x.Serial).ToList();

            string[] myArray = listsn.ToArray();
            countPackSN = 0;
            foreach (string value in myArray)
            {
                if (value != "") ;
                ++countPackSN;
            }
            #endregion

            #region Gán số thông tin cho Overpack và IN
            SP_Processing.MySqlConn mycon = new SP_Processing.MySqlConn(strSqlConnection4_608FFCPACKING);

            DataTable dt = mycon.ExecSProcDS("[amevn_getPOinfofromBox]", Boxnumber).Tables[0];
            if (dt.Rows.Count > 0)
            {

                string strPONumber = dt.Rows[0]["T_ProdOrder"].ToString().Trim();
                string strProductMap = dt.Rows[0]["T_ProductMap"].ToString().Trim();
                string SAP_model = dt.Rows[0]["T_Material"].ToString().Trim();
                string REV = dt.Rows[0]["T_Revision"].ToString().Trim();
                string iSLPO = (dt.Rows[0]["T_Quantity"].ToString().Trim());
                string iSLPOPACKED = (dt.Rows[0]["T_Packed"].ToString().Trim());
                string iSLBOX = (dt.Rows[0]["SL"].ToString().Trim());
                Boxnumber = Boxnumber.ToUpper();



                //print overpack


                string datetimenow = DateTime.Now.Day.ToString() + " " + DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + DateTime.Now.Year.ToString();

                LabelPrint.GanDuongDanBTlabel(nhanOverPack);
                LabelPrint.GanShareNameWithValueBTlabel("SPART", SAP_model.ToUpper());
                LabelPrint.GanShareNameWithValueBTlabel("PARTREV", REV.ToUpper());
                LabelPrint.GanShareNameWithValueBTlabel("DESC", txtDes.Text.ToUpper());
                LabelPrint.GanShareNameWithValueBTlabel("ZDATE", datetimenow/*DateTime.Now.ToShortDateString()*/);
                LabelPrint.GanShareNameWithValueBTlabel("AMT", iSLBOX.ToString());
                LabelPrint.GanShareNameWithValueBTlabel("PRODORDER", strPONumber.ToUpper());
                LabelPrint.GanShareNameWithValueBTlabel("MANPACK", this.cboPackingPlace.Text.ToString());
                LabelPrint.GanShareNameWithValueBTlabel("CORIGIN", this.cboManuafacturingIn.Text.ToUpper());

                LabelPrint.GanShareNameWithValueBTlabel("BOX", Boxnumber);

            }
            LabelPrint.GansoluongNhancanin(1);

            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            #endregion

            #region Chia số SN theo số lượng trên 15SNperlabel
            ///Input
            ///Output
            #region Tính số trang SN cần in_Input: Số record tổng cộng, số SNperLabel; Output: số trang (đã được RoundUP)
            ///
            int npage = (countPackSN + sosntoidatren1nhan - 1) / sosntoidatren1nhan;//(countPackSN/sosntoidatren1nhan)+1; //+1 cho test thôi nha

            #endregion
            //for (int i=0; i< npage; i++)
            //{
            //    var listtoprint = listsn.Skip(i*sosntoidatren1nhan).Take(sosntoidatren1nhan).ToList();
            //}

            //}
            #endregion

            #region Gán thông tin cho nhãn OverpackContent_nhãn SN và IN
            //print sn
            LabelPrint.GanDuongDanBTlabel(nhanOverPackContent);
            List<string> lstget = LabelPrint.GetListFieldNameFromBTlabel();
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
                        LabelPrint.GanShareNameWithValueBTlabel("SPSN" + n, "");
                        LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + n, "");

                    }

                }
                // gan cac cot co value
                for (int j = 0; j <= sosntrentungtrang - 1; j++)
                {

                    if (myArray[0].Length != 0)
                    {
                        LabelPrint.GanShareNameWithValueBTlabel("SPSN" + j, myArray[j]);
                        LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + j, Model + "-" + myArray[j]);
                        #region Combine data cho nhãn 2D
                        if (myArray[j] != "")
                        {

                            PN2D += Model + "-" + myArray[j];
                            PN2D += ",";
                        }
                        #endregion

                    }

                }
                LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIALALL", PN2D.Substring(0, PN2D.Length - 1));

                LabelPrint.GansoluongNhancanin(1);

                LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
                LabelPrint.ThucHienIn();
            }
            #endregion


            MessageBox.Show(string.Format("Đã in nhãn cho {0} Serial", countPackSN));
        }
        private void DoPrint2DOnly(List<clsSerialInput> ListSNs, string Boxnumber)
        {

            #region Khởi tạo vị trí đặt nhãn
            //string LocatedFolder =@"\\vnmsrv859\bformats\PREPRINT\";
            //string LocatedFolder = Application.StartupPath+ @"\\Label\";// AP DUNG CHO LOCAL FILE

            string LocatedFolder = @"C:\PrintPackStation\Label\FFC\";

            string nhanOverPack = @"APAC-OverPack.btw";
            string nhanOverPackContent =  @"APAC-OverPackContent.btw";

            nhanOverPack = LocatedFolder + nhanOverPack;
            nhanOverPackContent = LocatedFolder + nhanOverPackContent;
            //get info Order
            string Model = txtPN.Text.ToUpper();
            //Model ="ABCD";
            //get date
            //get description
            //get sn
            int Packnumber = ListSNs.Count;
            #endregion


            #region Đếm_Số_SN_trong_box_để_in_ra_OVerPackLabel

            List<clsSerialInput> SortedList = ListSNs.OrderBy(o => o.Serial).ToList();


            //var listsn = SNs.Select(x => x.Serial).ToList();//ok
            var listsn = SortedList.Select(x => x.Serial).ToList();

            string[] myArray = listsn.ToArray();
            countPackSN = 0;
            foreach (string value in myArray)
            {
                if (value != "") ;
                ++countPackSN;
            }
            #endregion

            #region Gán số thông tin cho Overpack và IN
            //print overpack
            string datetimenow = DateTime.Now.Day.ToString() + " " + DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + DateTime.Now.Year.ToString();

            LabelPrint.GanDuongDanBTlabel(nhanOverPackContent);
            LabelPrint.GanShareNameWithValueBTlabel("SPART", txtPN.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("PARTREV", txtRev.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("DESC", txtDes.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("ZDATE", datetimenow/*DateTime.Now.ToShortDateString()*/);
            LabelPrint.GanShareNameWithValueBTlabel("AMT", countPackSN.ToString());
            LabelPrint.GanShareNameWithValueBTlabel("PRODORDER", this.txtPO.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("MANPACK", this.cboPackingPlace.Text.ToString());
            LabelPrint.GanShareNameWithValueBTlabel("CORIGIN", this.cboManuafacturingIn.Text.ToUpper());

            LabelPrint.GanShareNameWithValueBTlabel("BOX", Boxnumber);


            LabelPrint.GansoluongNhancanin(1);

            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            #endregion

            #region Chia số SN theo số lượng trên 15SNperlabel
            ///Input
            ///Output
            #region Tính số trang SN cần in_Input: Số record tổng cộng, số SNperLabel; Output: số trang (đã được RoundUP)
            ///
            int npage = (countPackSN + sosntoidatren1nhan - 1) / sosntoidatren1nhan;//(countPackSN/sosntoidatren1nhan)+1; //+1 cho test thôi nha

            #endregion
            //for (int i=0; i< npage; i++)
            //{
            //    var listtoprint = listsn.Skip(i*sosntoidatren1nhan).Take(sosntoidatren1nhan).ToList();
            //}

            //}
            #endregion

            #region Gán thông tin cho nhãn OverpackContent_nhãn SN và IN
            //print sn
            LabelPrint.GanDuongDanBTlabel(nhanOverPack);
            List<string> lstget = LabelPrint.GetListFieldNameFromBTlabel();
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
                        LabelPrint.GanShareNameWithValueBTlabel("SPSN" + n, "");
                        LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + n, "");

                    }

                }
                // gan cac cot co value
                for (int j = 0; j <= sosntrentungtrang - 1; j++)
                {

                    if (myArray[0].Length != 0)
                    {
                        LabelPrint.GanShareNameWithValueBTlabel("SPSN" + j, myArray[j]);
                        LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + j, Model + "-" + myArray[j]);
                        #region Combine data cho nhãn 2D
                        if (myArray[j] != "")
                        {

                            PN2D += Model + "-" + myArray[j];
                            PN2D += ",";
                        }
                        #endregion

                    }

                }
                LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIALALL", PN2D.Substring(0, PN2D.Length - 1));

                LabelPrint.GansoluongNhancanin(1);

                LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
                LabelPrint.ThucHienIn();
            }
            #endregion


            MessageBox.Show(string.Format("Đã in nhãn cho {0} Serial", countPackSN));
        }
        private void DoPrintOverPackContent(int iQty, string Boxnumber)
        {

            #region Khởi tạo vị trí đặt nhãn
            //string LocatedFolder =@"\\vnmsrv859\bformats\PREPRINT\";
            //string LocatedFolder = Application.StartupPath+ @"\\Label\";// AP DUNG CHO LOCAL FILE

            string LocatedFolder = @"C:\PrintPackStation\Label\FFC\";

            string nhanOverPack = @"APAC-OverPackContent.btw";
            string nhanOverPackContent = @"APAC-OverPack.btw";

            nhanOverPack = LocatedFolder + nhanOverPack;
            nhanOverPackContent = LocatedFolder + nhanOverPackContent;
            //get info Order
            string Model = txtPN.Text.ToUpper();
            //Model ="ABCD";
            //get date
            //get description
            //get sn
            int Packnumber = iQty;//ListSNs.Count;
            #endregion


            //#region Đếm_Số_SN_trong_box_để_in_ra_OVerPackLabel

            //List<clsSerialInput> SortedList = ListSNs.OrderBy(o => o.Serial).ToList();


            ////var listsn = SNs.Select(x => x.Serial).ToList();//ok
            //var listsn = SortedList.Select(x => x.Serial).ToList();

            //string[] myArray = listsn.ToArray();
            //countPackSN = 0;
            //foreach (string value in myArray)
            //{
            //    if (value != "") ;
            //    ++countPackSN;
            //}
            //#endregion

            #region Gán số thông tin cho Overpack và IN
            //print overpack
            string datetimenow = DateTime.Now.Day.ToString() + " " + DateTime.Now.ToString("MMMM").Substring(0, 3) + " " + DateTime.Now.Year.ToString();

            LabelPrint.GanDuongDanBTlabel(nhanOverPack);
            LabelPrint.GanShareNameWithValueBTlabel("SPART", this.txtModelCheck.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("PARTREV", textBox6.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("DESC", textBox5.Text.ToUpper());
            LabelPrint.GanShareNameWithValueBTlabel("ZDATE", datetimenow/*DateTime.Now.ToShortDateString()*/);
            LabelPrint.GanShareNameWithValueBTlabel("AMT", iQty.ToString());
            LabelPrint.GanShareNameWithValueBTlabel("PRODORDER", "");
            LabelPrint.GanShareNameWithValueBTlabel("MANPACK", this.textBox8.ToString());
            LabelPrint.GanShareNameWithValueBTlabel("CORIGIN", this.textBox9.Text.ToUpper());

            LabelPrint.GanShareNameWithValueBTlabel("BOX", Boxnumber);


            LabelPrint.GansoluongNhancanin(1);

            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            #endregion

            //#region Chia số SN theo số lượng trên 15SNperlabel
            /////Input
            /////Output
            //#region Tính số trang SN cần in_Input: Số record tổng cộng, số SNperLabel; Output: số trang (đã được RoundUP)
            /////
            //int npage = (countPackSN + sosntoidatren1nhan - 1) / sosntoidatren1nhan;//(countPackSN/sosntoidatren1nhan)+1; //+1 cho test thôi nha

            //#endregion
            ////for (int i=0; i< npage; i++)
            ////{
            ////    var listtoprint = listsn.Skip(i*sosntoidatren1nhan).Take(sosntoidatren1nhan).ToList();
            ////}

            ////}
            //#endregion

            //#region Gán thông tin cho nhãn OverpackContent_nhãn SN và IN
            ////print sn
            //LabelPrint.GanDuongDanBTlabel(nhanOverPackContent);
            //List<string> lstget = LabelPrint.GetListFieldNameFromBTlabel();
            ////int i = 0;
            ////loop to print page
            //for (int k = 0; k < npage; k++)
            //{
            //    var listtoprint = listsn.Skip(k * sosntoidatren1nhan).Take(sosntoidatren1nhan).ToList();
            //    myArray = listtoprint.ToArray();
            //    int sosntrentungtrang = myArray.Count();

            //    string PN2D = "";
            //    //gan all arry = "" empty
            //    for (int n = 0; n <= 15 - 1; n++)
            //    {


            //        if (myArray[0].Length != 0)
            //        {
            //            LabelPrint.GanShareNameWithValueBTlabel("SPSN" + n, "");
            //            LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + n, "");

            //        }

            //    }
            //    // gan cac cot co value
            //    for (int j = 0; j <= sosntrentungtrang - 1; j++)
            //    {

            //        if (myArray[0].Length != 0)
            //        {
            //            LabelPrint.GanShareNameWithValueBTlabel("SPSN" + j, myArray[j]);
            //            LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIAL" + j, Model + "-" + myArray[j]);
            //            #region Combine data cho nhãn 2D
            //            if (myArray[j] != "")
            //            {

            //                PN2D += Model + "-" + myArray[j];
            //                PN2D += ",";
            //            }
            //            #endregion

            //        }

            //    }
            //    LabelPrint.GanShareNameWithValueBTlabel("PARTCONSERIALALL", PN2D.Substring(0, PN2D.Length - 1));

            //    LabelPrint.GansoluongNhancanin(1);

            //    LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            //    LabelPrint.ThucHienIn();
            //}
            //#endregion


            //MessageBox.Show(string.Format("Đã in nhãn cho {0} Serial", countPackSN));
        }

        //public static int nTotalRow  //total rows for next button
        //public static int pTotalRow  //total rows for previos button
        //public static int nSkkipedRows  //the steps for skkiping rows in next button
        //public static int pSkkipedRows    //the steps for skkiping rows in previous button
        //public static int Total             //total rows of table


        public List<clsSerialInput> SNs;// = new clsSerialInput();
        public List<clsSerialInput> SNsRemove;// = new clsSerialInput();
        public List<clsSerialInput> SNsAdd;// = new clsSerialInput();

        //MachineInfoBusiness thismachine = new MachineInfoBusiness();
        BartenderBusiness LabelPrint;

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

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static int ReturnCharFound(string strchuoiinput, string findvalue)
        {
            return strchuoiinput.IndexOf(findvalue);
        }

        private string Check_Model_SN_by_Conditions(string str_i_input, string strmodelstd, int SNlength)
        {

            #region Check PartNumber to make sure PN and PN-SN correct
            ///if PN not correct then end
            ///ReturnCharFound(ReverseString(txtSNinput.Text.Trim()), "-"); => lấy ký tự số sn từ các ký tự cuối
            ///
            int aget = ReturnCharFound(ReverseString(str_i_input), "-");

            int astart = str_i_input.Length - aget;
            int bstart = str_i_input.Length - astart;

            string strPNtocheck = str_i_input.Substring(0, astart - 1);
            string strSNtocheck = str_i_input.Substring(astart, aget);

            if (strPNtocheck.ToUpper() != strmodelstd.ToUpper())
            {
                return " Model không đúng ! Kiểm tra:" + strPNtocheck;
            }

            if (strSNtocheck.Length != SNlength)
            {
                return " SN không đủ hoặc dư ký tự ! Kiểm tra số SN: " + strSNtocheck;
            }

            return "OK";

            #endregion
        }
    
        private void Form1_Load(object sender, EventArgs e)
        {
         
            PopulateInstalledPrintersCombo();
           

//            PrinterList(); //ok without defaut 
               
            CheckBTformat_CopyIf_NOT_here();
            
            LabelPrint= new BartenderBusiness();
            //LabelPrint.Khoitao();

            strSqlConnection4_608FFCPACKING = ConfigurationManager.AppSettings.Get("FFCPACKINGCONNECTION").ToString();

            KhoitaoSNnLuoi();

            txtPO.Focus();
            
            
        }

        private PrintDocument printDoc = new PrintDocument();

        private void PopulateInstalledPrintersCombo()
        {
            //this.cboPrinterList.Dock = DockStyle.Top;
           // Controls.Add(this.cboPrinterList);

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
        private void PrinterList()
        {
            // POPULATE THE COMBO BOX.
            foreach (string sPrinters in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                cboPrinterList.Items.Add(sPrinters);
                
            }

            

            //// POPULATE THE LIST BOX.
            //foreach (string sPrinters in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    lstBoxPrinters.Items.Add(sPrinters);
            //}
        }
        private static void CheckBTformat_CopyIf_NOT_here()
        {
            //check format if not exist then copy//
            CheckFolderExistthenCreate(@"C:\PrintPackStation");
            CheckFolderExistthenCreate(@"C:\PrintPackStation\Label");
            CheckFolderExistthenCreate(@"C:\PrintPackStation\Label\FFC");

            CheckFileExistthenCopy(@"\\vnmsrv601\DevelopSoftware\PrintPackbyTO\Label\APAC-OverPack.btw", @"C:\PrintPackStation\Label\FFC\APAC-OverPack.btw");
            CheckFileExistthenCopy(@"\\vnmsrv601\DevelopSoftware\PrintPackbyTO\Label\APAC-OverPackContent.btw", @"C:\PrintPackStation\Label\FFC\APAC-OverPackContent.btw");
            CheckFileExistthenCopy(@"\\vnmsrv601\DevelopSoftware\PrintPackbyTO\Label\APAC-OverPackContentVN.btw", @"C:\PrintPackStation\Label\FFC\APAC-OverPackContentVN.btw");

            //check format if not exist then copy//end
        }

        private string GetBOXNUMBER()
        {
            //getboxnumber

            SqlConnection sqlConnection1 = new SqlConnection("Server=10.84.10.67\\SIPLACE_2008R2EX;Database=FFCPacking;User ID=reports;Password=reports;Trusted_Connection=False");
            SqlCommand cmd = new SqlCommand();
            Object returnValue;

            cmd.CommandText = "ame_GetBoxNumber";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            returnValue = cmd.ExecuteScalar();

            return returnValue.ToString();
            sqlConnection1.Close();

            //getboxnumber
        }

        private void KhoitaoSNnLuoi()
        {
            SNs = new List<clsSerialInput>();
            LuoiSNInput.DataSource = SNs;
            
            SNsRemove= new List<clsSerialInput>();
            DgvREmovelist.DataSource = SNsRemove;
            //SNsAdd= new List<clsSerialInput>();

            txtBOX.Text = "";
            
        }
        int i=0;
        private void txtSNinput_KeyDown(object sender, KeyEventArgs e)
        {

          
                if (e.KeyCode == Keys.Enter)
                {
                    if (chkModelEnterSerial.Checked == true && i == 0) // 
                    {
                        if (txtSNinput.Text.ToUpper().Trim() != txtPN.Text.ToUpper().Trim())
                        {
                            MessageBox.Show("Wrong Model");
                        }
                        else
                        {

                            txtSNinput.Text = "";
                            txtSNinput.Focus();
                            i = 1;//catch model
                            return;
                        }
                    }
                    else if (i == 1 || chkModelEnterSerial.Checked == false)
                    {
                        i = 0;
                        //check sn
                        if (SNs.Count <= Convert.ToInt16(cboSNperLabel.Text.Trim()) + 1) // COUNT ĐỦ 15 PCS ĐỂ IN
                        {
                            if (txtSNinput.Text.Trim() != "")
                            {

                                if (ChkSerialOnly.Checked)
                                {
                                    txtSNinput.Text = txtPN.Text.Trim().ToUpper()+"-"+ txtSNinput.Text.Trim().ToUpper();
                                }

#region Check PartNumber to make sure PN and PN-SN correct 
                                ///if PN not correct then end
                                ///ReturnCharFound(ReverseString(txtSNinput.Text.Trim()), "-"); => lấy ký tự số sn từ các ký tự cuối
                                ///
                               
//                                int aget = ReturnCharFound(ReverseString(txtSNinput.Text.Trim()), "-");

//                                int astart = txtSNinput.Text.ToUpper().Trim().Length - aget;
//                                string strPNtocheck = txtSNinput.Text.ToUpper().Substring(0, astart - 1);

//                                    if (strPNtocheck.ToUpper() != txtPN.Text.Trim().ToUpper())
//                                    {

//                                        //TAT TAM CHO HALOGEN CHAY
//                                        MessageBox.Show(strPNtocheck + " Model không đúng !");
//                                        txtSNinput.Text = "";
//                                        txtSNinput.Focus();
//                                        return;
//                                    }

//#endregion

                                   

//#region Check_số_SN 
                                   
//                               string strSNtocheck = txtSNinput.Text.ToUpper().Substring(astart, aget).Trim();


                                //HAOLENMODE

                                string strSNtocheck = txtSNinput.Text.Trim();
                                //HAOLENMODE

                                var listsn = SNs.Select(x => x.Serial).ToList();

                                string[] myArray = listsn.ToArray();
                                countPackSN = 0;
                                foreach (string value in myArray)
                                {
                                    if (value != "") ;
                                    ++countPackSN;
                                }

                                string result = CheckIfValueExistInArray(strSNtocheck/*txtSNinput.Text.Trim()*/, myArray);
                                if (result != "OK")
                                {
                                    txtSNinput.Text = "";
                                }


                                //check if exist
#endregion
                                if (result == "OK")
                                {


                                    AddSNs(strSNtocheck);

                                    ClearAndWaitNextSerialInput();

                                    UpdateQtyScanned2table();
                                }
                            }
                            else if ((txtSNinput.Text.Trim() == "") && (SNs.Count != 0))
                            {
                                //print enven not enoug qty

                                DialogResult dr = MessageBox.Show("Số lượng chưa đủ, bạn muốn in " + SNs.Count + " SN này ", "Confirm ! " + SNs.Count, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dr == DialogResult.Yes)
                                {
                                    btnPrint.PerformClick();
                                }
                            }
                        }
                        else
                        {
                            ClearAndWaitNextSerialInput();
                            MessageBox.Show("du so luong  sn tren label");
                        }

                        if ((SNs.Count == Convert.ToInt16(cboSNperLabel.Text.Trim())))
                        {
                            ClearAndWaitNextSerialInput();
                            //MessageBox.Show("=>in");
                            //do logFile


                            //do print
                            btnPrint.PerformClick();
                        }


                        List<clsSerialInput> SortedList = SNs.OrderBy(o => o.Serial).ToList();
                        LuoiSNInput.DataSource = SortedList.ToList();
                    }
                }

            }

        private void AddSNs(string strSNtocheck)
        {
            clsSerialInput aSNs = new clsSerialInput();
            aSNs.Order = txtPO.Text.Trim();
            aSNs.Partnumber = txtPN.Text.Trim().ToUpper();
            aSNs.Serial = strSNtocheck;//nhap SN đã cắt ra từ model-sn
            aSNs.Packingdate = DateTime.Now.ToShortDateString();
            aSNs.BoxNo = txtBOX.Text;
            aSNs.PackStation = "";// thismachine.MayTinhHienTaiTenGi();


            SNs.Add(aSNs); // LINQ add vào
        }

        public string strPOlast = "";
        public string strPNlast = "";
        public string strRev = "";
        public string strDes = "";

        Boolean IsFirstBoxEnter = false;





        private string AddSNSfrmbox(string strboxNumber_, List<clsSerialInput> ListSNs)
        {

            try
            {
                using (sqlConnection4 = new SqlConnection(strSqlConnection4_608FFCPACKING))
                {
                    sqlConnection4.Open();
                    SqlCommand cmdGetProdOrderSerials = sqlConnection4.CreateCommand();
                    cmdGetProdOrderSerials.CommandType = CommandType.StoredProcedure;
                    cmdGetProdOrderSerials.CommandText = "ame_T_getSerial_byBOX"; ;
                    cmdGetProdOrderSerials.Parameters.Add("@BoxNumber", SqlDbType.Char, 20);
                    cmdGetProdOrderSerials.Parameters["@BoxNumber"].Value = strboxNumber_;
                    cmdGetProdOrderSerials.Parameters["@BoxNumber"].Direction = ParameterDirection.Input;
                    //cmdGetProdOrderSerials.Parameters.Add("@PONumber", SqlDbType.Char, 20);
                    //cmdGetProdOrderSerials.Parameters["@PONumber"].Value = strboxNumber_;
                    //cmdGetProdOrderSerials.Parameters["@PONumber"].Direction = ParameterDirection.Output; ;
                    SqlDataReader mySerials = cmdGetProdOrderSerials.ExecuteReader();
                    mySerials.Read();
                    if (mySerials[0].ToString().Equals("OK"))
                    {
                        mySerials.NextResult();
                        //dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();
                        while (mySerials.Read())
                        {
                            
                            clsSerialInput aSNs = new clsSerialInput();
                            aSNs.Order = mySerials["PONumber"].ToString().Trim();
                            aSNs.Partnumber = mySerials["Model"].ToString().Trim();
                            aSNs.Serial = mySerials["Serial"].ToString().Trim();//nhap SN đã cắt ra từ model-sn
                            aSNs.Packingdate = mySerials["PackingDateTime"].ToString().Trim();
                            aSNs.BoxNo = mySerials["BoxNumber"].ToString().Trim(); ;
                            aSNs.PackStation = "";// thismachine.MayTinhHienTaiTenGi();

                            if (IsFirstBoxEnter)
                            {
                                strPOlast = mySerials["PONumber"].ToString().Trim();
                                strPNlast = mySerials["Model"].ToString().Trim();
                                IsFirstBoxEnter = false;

                                //do update information
                                string result = GetPoInformationDetail(strPOlast.PadLeft(12,'0'));
                                //
                            }
                            if (/*strPOlast != aSNs.Order ||*/ strPNlast != aSNs.Partnumber)
                            {
                                MessageBox.Show("Model  ko dung! Check lai  box "+ strboxNumber_);
                            return "NG";
                            }


                            ListSNs.Add(aSNs); // LINQ add vào
                           


                        }
                    }
                    sqlConnection4.Close();
                   
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return "NG";
                MessageBox.Show(ex.Message, "box " + strboxNumber_);
            }
        }

        private void AddSNsRemove(string strSNtocheck)
        {
            clsSerialInput aSNs = new clsSerialInput();
            aSNs.Order = strPOlast;
            aSNs.Partnumber = strPNlast;
            aSNs.Serial = strSNtocheck;//nhap SN dã c?t ra t? model-sn
            aSNs.Packingdate = DateTime.Now.ToShortDateString();
            aSNs.BoxNo = "";// txtBOX.Text;
            aSNs.PackStation = "";// thismachine.MayTinhHienTaiTenGi();


            SNsRemove.Add(aSNs); // LINQ add vào

        }
        private void AddSNsAdd(string strSNtocheck)
        {
            clsSerialInput aSNs = new clsSerialInput();
            aSNs.Order = strPOlast;
            aSNs.Partnumber = strPNlast;
            aSNs.Serial = strSNtocheck;//nhap SN dã c?t ra t? model-sn
            aSNs.Packingdate = DateTime.Now.ToShortDateString();
            aSNs.BoxNo = txtBOX.Text;
            aSNs.PackStation = "";// thismachine.MayTinhHienTaiTenGi();


            SNsAdd.Add(aSNs); // LINQ add vào
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
                    
                   kq= "Trung so !";
                }
                
            }
            return kq;
        }

        private void UpdateQtyScanned2table()
        {
            /// Main Function
            /// Update số SN lên lưới

            List<clsSerialInput> SortedList = SNs.OrderBy(o => o.Serial).ToList();
            LuoiSNInput.DataSource = SortedList.ToList();

            List<clsSerialInput> SortedList1 = SNsRemove.OrderBy(o => o.Serial).ToList();
            DgvREmovelist.DataSource = SortedList1;

            //List<clsSerialInput> SortedList2 = SNsAdd.OrderBy(o => o.Serial).ToList();
            //dgvAddnew.DataSource = SortedList2;

            lblPacked.Text = SNs.Count.ToString();
        }

        private void ClearAndWaitNextSerialInput()
        {
            txtSNinput.Text = "";
            txtSNinput.Focus();
        }

        private void txtPN_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void txtPN_Enter(object sender, EventArgs e)
        {

        }

        private void txtPN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPN.Text = txtPN.Text.ToUpper().Trim();
                string des = "";
                string rev = "";
                //ACSoneTOOL.PhanMem pm = new ACSoneTOOL.PhanMem();
                
               // PhanMem.PullModelDescription2string(this.txtPN.Text.Trim(), out des, out rev);

                this.txtDes.Text = des;

                this.txtRev.Text = rev;

                 if (this.txtDes.Text.Trim()!="")
            { this.txtSNinput.Focus(); }
            else
                 { txtPN.Focus(); }
            }
        }
        public string strSqlConnection4_608FFCPACKING ;
        public System.Data.SqlClient.SqlConnection sqlConnection4;
        

        private void txtTO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string Producmap = "";
                string PNa="";
                string Rev = "";
                string Des = "";

            ReadInforFromT_Information(txtPO.Text, ref PNa , ref Rev, ref Des, ref Producmap);
                txtPN.Text=PNa;
                 txtRev.Text=Rev;
                     txtDes.Text=Des;
                txtPN.Focus();
            }
            else
            { txtPO.Focus(); }
        }

        private void ReadInforFromT_Information(string PO, ref string strPN,ref string strRev, ref string StrDes, ref string strProductmap )
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
        private string getProductMapDetail(string SAPModel, ref string strDes)
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
                    cmd.Parameters.Add("@Description", SqlDbType.Char, 30);
                    
                    cmd.Parameters["@Description"].Direction = ParameterDirection.Output;

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

        private string GetPoInformationDetail(string PONumber)
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
                    cmd.CommandText = "ame_GetPoInformationDetail";
                    cmd.Parameters.Add("@PONumber", SqlDbType.Char, 30);
                    cmd.Parameters["@PONumber"].Value = PONumber;
                    cmd.Parameters["@PONumber"].Direction = ParameterDirection.Input;
                   

                    SqlDataReader rec = cmd.ExecuteReader();
                    rec.Read();

                    if (rec[0].ToString().Equals("OK"))
                    {
                        rec.NextResult();
                        //dictPOInformation = new System.Collections.Generic.Dictionary<string, clsPOSerials>();



                        while (rec.Read())
                        {
                            txtPO.Text = rec["T_ProdOrder"].ToString().Trim();
                            txtPN.Text = rec["T_Material"].ToString().Trim();
                            txtRev.Text = rec["T_Revision"].ToString().Trim();
                            txtDes.Text = rec["Description"].ToString().Trim();
                            //= rec["ProductMap"].ToString().Trim();

                        }
                    }
                    sqlConnection4.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error get Product Map:" + ex.Message);
            }
            return result;
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void txtRev_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRev.Text))
            {
                txtRev.Text = "N/A";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (removeSN != "")
            {
                var itemToRemove = SNs.Single(r => r.Serial == removeSN);
                AddSNsRemove(itemToRemove.Serial.ToString().Trim());

                SNs.Remove(itemToRemove);

                removeSN = "";
                button1.Text = "Revmove SN: " + " ?";

                UpdateQtyScanned2table();
            }
            else
            {
                MessageBox.Show("Chọn số SN từ danh sách số SN bên dưới !");
            }

        }

        string removeSN;
        private void LuoiSNInput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            removeSN = (this.LuoiSNInput.Rows[e.RowIndex].Cells[2].Value.ToString()).Trim();
            button1.Text = "Revmove SN: "+removeSN+ " ?";
        }

        

        private void LuoiSNInput_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        /// <summary>
        /// In test máy in và format nhãn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)

{
    //string test = @"\\vnmsrv859\bformats\QD2400\z731097902UNTF.btw";
    //LabelPrint.GanDuongDanBTlabel(test);
    //LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
    //LabelPrint.ThucHienIn();
    //return;


            string LocatedFolder = @"C:\PrintPackStation\Label\FFC\";

            string nhanOverPack = @"APAC-OverPackContent.btw";
            string nhanOverPackContent = @"APAC-OverPack.btw";

            nhanOverPack = LocatedFolder + nhanOverPack;
            nhanOverPackContent = LocatedFolder + nhanOverPackContent;

            LabelPrint.GanDuongDanBTlabel(nhanOverPack);
            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            LabelPrint.GanDuongDanBTlabel(nhanOverPackContent);
            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            //LabelPrint.HuydoituongBT_withSave(); //=< in test hủy đối tượng chi

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LabelPrint.HuydoituongBT();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //load box by station
            IsFirstBoxEnter = true;
            string strListBox = "";
            foreach (var i in listBox1.Items)
            {
                
              strListBox = i.ToString();
              AddSNSfrmbox(strListBox, SNs);
            }

            listBox1.Items.Clear();
            
            UpdateQtyScanned2table();

            IsFirstBoxEnter = false;
            //get listSN by box


        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!textBox1.Text.Equals(""))
                {
                    listBox1.Items.Add(textBox1.Text.Trim());
                    textBox1.Text = "";

                    //add list SN to list
                    
                }
                else
                {
                    button3.PerformClick();
                }
                


//                Dim nwTB As New Northwind.OrdersDataTable
//Dim nwtba As New  NorthwindTableAdapters.OrdersTableAdapter
//nwTB = nwtba.GetDataByCustomerID("ALFKI")



                


            }
        }

        private void tFFC_SerialNumbersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.tFFC_SerialNumbersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.fFCPackingDataSet);

        }

        private void boxNumberTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRemoveSn_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void fillByBoxnumberToolStripButton_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    this.packingRecordTableAdapter.FillByBoxnumber(this.fFCPackingDataSet.PackingRecord);
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}

        }

        private void txtRemoveSn_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                removeSN = txtRemoveSn.Text;
                if (removeSN != "")
                {
                    var itemToRemove = SNs.Single(r => r.Serial == removeSN);
                    AddSNsRemove(itemToRemove.Serial.ToString().Trim());

                    SNs.Remove(itemToRemove);

                    removeSN = txtRemoveSn.Text= "";
                    button1.Text = "Revmove SN: " + " ?";

                    UpdateQtyScanned2table();
                }
                else
                {
                    MessageBox.Show("Chọn số SN từ danh sách số SN bên dưới !");
                }
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //var itemToRemove = SNs.Single(r => r.Serial == txtAddSN.Text);
                //AddSNs(txtAddSN.Text.ToString().Trim());

                //SNs.Remove(itemToRemove);

                UpdateQtyScanned2table();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
         
        }

        private void UpdateRepackData()
        {
            //update process

            //get box number
            string strboxnumber = GetBOXNUMBER().ToUpper();
            txtBOX.Text = strboxnumber;
            // add current SN to listbox
            foreach (var sn in SNs)
            {
                FFCPackingDataSet.PackingRecordDataTable dt = new FFCPackingDataSet.PackingRecordDataTable();
                FFCPackingDataSetTableAdapters.PackingRecordTableAdapter da = new FFCPackingDataSetTableAdapters.PackingRecordTableAdapter();

                da.UpdateQueryNewBox(strboxnumber, DateTime.Now, sn.Serial.ToString(), txtPO.Text.Trim());


            }
            //update remove list-> unpack box getbox = po unpack
            foreach (var sn in SNsRemove)
            {
                FFCPackingDataSet.PackingRecordDataTable dt = new FFCPackingDataSet.PackingRecordDataTable();
                FFCPackingDataSetTableAdapters.PackingRecordTableAdapter da = new FFCPackingDataSetTableAdapters.PackingRecordTableAdapter();

                string strPORepack = txtPO.Text.Trim();
                string RPKbox = "RPK" + strPORepack.Substring(strPORepack.Length - 6, 6);

                da.UpdateQueryNewBox(RPKbox, DateTime.Now, sn.Serial.ToString(), txtPO.Text.Trim());


            }

            //do print


            
        }

        private void txtBOX_TextChanged(object sender, EventArgs e)
        {

        }
        bool IsRepackStart = false;

        private void textBox2_KeyDown_1(object sender, KeyEventArgs e)
        {
            //todo: Repack Mode_develop here

            if (e.KeyCode == Keys.Enter)
            {
                string SNinput = textBox2.Text.Trim();
                string Boxnumber = "";
                if (SNinput != "")
                {
                    IsRepackStart = true;
                    //scan SN vao
                    FFCPackingDataSet.GetBoxnumberbySNDataTable dt = new FFCPackingDataSet.GetBoxnumberbySNDataTable();
                    FFCPackingDataSetTableAdapters.GetBoxnumberbySNTableAdapter da = new FFCPackingDataSetTableAdapters.GetBoxnumberbySNTableAdapter();

                    da.GetBoxNumberbySerial(dt, SNinput).ToString();
                    
                    foreach (DataRow dr in dt.Rows)
                    {
                       Boxnumber= dr["Boxnumber"].ToString().Trim();
                       textBox3.Text = Boxnumber;
                       textBox2.Focus();
                    }
                    //lây box
                    //get SN to new box
                    //floating SN PO
                }
                else
                {
                    //focus txtbox2
                }
            }

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // create box
            string strboxnumber = GetBOXNUMBER().ToUpper();
            txtBoxNumber2.Text = strboxnumber.Trim();

            

        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //get model description
                //get rev
                txtModelCheck.Text = txtModelCheck.Text.ToUpper().Trim();
                string des = "";
                string rev = "";
                //ACSoneTOOL.PhanMem pm = new ACSoneTOOL.PhanMem();

                PhanMem.PullModelDescription2string(txtModelCheck.Text.Trim(), out des, out rev);

                textBox5.Text = des;
                textBox6.Text = rev;
            }
        }

       public int iQty = 0;
        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtModelneedtocheck.Text = txtModelneedtocheck.Text.ToUpper();
                if (iQty == 0)
                {
                    btnDoneToPrint.Visible = false;
                }

                if (txtModelneedtocheck.Text.Trim() == txtModelCheck.Text.Trim())
                {
                    txtModelneedtocheck.Text = "";
                    btnDoneToPrint.Visible = true;
                    iQty++;
                    txtQty.Text = iQty.ToString();
                }
                else
                {
                    MessageBox.Show("Sai model");
                    txtModelneedtocheck.Text = "";
                     
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // do print
            DoPrintOverPackContent(iQty, txtBoxNumber2.Text);
            // reset iQty
            iQty = 0;
            txtQty.Text = iQty.ToString();
            txtBoxNumber2.Text = "";
            btnDoneToPrint.Visible = false;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            string box = textBox4.Text;
            //get sn list
            


            DoPrintfromBOX(box);
            textBox1.Text = "";
            //
            return;
            

            string LocatedFolder = @"C:\PrintPackStation\Label\FFC\";

            string nhanOverPack = @"APAC-OverPackContent.btw";
            string nhanOverPackContent = @"APAC-OverPack.btw";

            nhanOverPack = LocatedFolder + nhanOverPack;
            nhanOverPackContent = LocatedFolder + nhanOverPackContent;

            LabelPrint.GanDuongDanBTlabel(nhanOverPack);
            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            LabelPrint.GanDuongDanBTlabel(nhanOverPackContent);
            LabelPrint.GanMayIN(cboPrinterList.Text.Trim());
            LabelPrint.ThucHienIn();

            //LabelPrint.HuydoituongBT_withSave(); //=< in test hủy đối tượng chi

        }

        

        //private void fillToolStripButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.ame_CheckSerialUnPackTableAdapter.Fill(this.fFCPackingDataSet.ame_CheckSerialUnPack, modelToolStripTextBox.Text, serialToolStripTextBox.Text, boxUnPackToolStripTextBox.Text);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message);
        //    }

        //}
            //public void SetSerial (string strOrder  ,string strSerial, string strPackdate, string strBox,string strStation )
            //{
            //    SNs.Order = strOrder;
            //    SNs.Serial = strSerial;
            //    SNs.Packingdate = strPackdate;
            //    SNs.BoxNo = strBox;
            //    SNs.PackStation = strStation;
            //}

        //}==>khong code ben duowi dong nay


    }
}
