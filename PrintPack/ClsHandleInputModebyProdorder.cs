using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintPack
{
    /// <summary>
    /// Chọn inputmode để lấy điều kiện kiểm tra
    /// 1- Đã test
    /// 2- Đã packed trước đó (consider 1 tháng)
    /// 3- Có thuộc PO (trường hợp FFC/DLM
    /// 4- Đã nhập rồi trong kỳ đóng gói này
    /// </summary>
    class ConditiontoPackingVerify
    {
        public string ProductMap { get; set; }
        public string  strInputMode { get; set; }
        
        public bool IsCheckSNbelongProdOrder  { get; set; }
        public bool IsCheckTestLog { get; set; }
        public bool IsCheckPacked { get; set; }
        public bool IsCheckInputAlready { get; set; } // Check if input in current box
        public bool IsCheckConsumedConfig { get; set; }

        public ConditiontoPackingVerify()
        {
            IsCheckSNbelongProdOrder = false;
            IsCheckTestLog = false;
            IsCheckPacked= false;
            IsCheckConsumedConfig = false;

        }

        public void GetProductMapbyModel(string SAPModel)
        {
            

        }
        public void GetProductMap(string strProductMap)
        {
            ProductMap = strProductMap;
            if (strProductMap == "DLM" )
            //if (strProductMap == "DLM" ||strProductMap == "FFC" )
            {
                IsCheckSNbelongProdOrder = true;
                IsCheckTestLog = false;// true;// false;change 21 Oct 2016
                IsCheckPacked= true;
                IsCheckInputAlready = true;
                IsCheckConsumedConfig = false;
                
            }
            else if (strProductMap == "BASE")
            {
                IsCheckSNbelongProdOrder = false;
                IsCheckTestLog = true;
                IsCheckPacked= true;
                IsCheckInputAlready = true;
                IsCheckConsumedConfig = false;
            }
            else if (strProductMap == "BASEHALOGEN")
            {
                IsCheckSNbelongProdOrder = true;
                IsCheckTestLog = true;
                IsCheckPacked = true;
                IsCheckInputAlready = true;
                IsCheckConsumedConfig = false;
            }
            else if (strProductMap == "FFC")
            {
                IsCheckSNbelongProdOrder = true;
                IsCheckTestLog = false;
                IsCheckPacked = true;
                IsCheckInputAlready = true; // Check if input in current box
                IsCheckConsumedConfig = true;
            }
        }

        



  





    
    }
}
