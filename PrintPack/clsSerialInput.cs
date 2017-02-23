using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace PrintPack
//{
//    class clsSerialInput
//    {
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

namespace PrintPack
{
    public class clsPackingInformation
    {
        public string POnumber;
        public string POMaterial;
        public string PODesc;
        public string BoxQty;
        public string Boxno;
        public string Rev;
    }

    /// <summary>
    /// 
    /// </summary>
    /// 

    public  class clsSerialInput
    {
        private string _Partnumber;

        public string Partnumber
        {
            get { return _Partnumber; }
            set { _Partnumber = value; }
        }
        private string _Order;

        public string Order
        {
            get { return _Order; }
            set { _Order = value; }
        }
        private string _Serial;

        public string Serial
        {
            get { return _Serial; }
            set { _Serial = value; }
        }
        private string _Packingdate;

        public string Packingdate
        {
            get { return _Packingdate; }
            set { _Packingdate = value; }
        }
        private string _BoxNo;

        public string BoxNo
        {
            get { return _BoxNo; }
            set { _BoxNo = value; }
        }
        private string _PackStation;

        public string PackStation
        {
            get { return _PackStation; }
            set { _PackStation = value; }
        }

        ///add vao he thong local
        /// add vao he thong
        /// 



    }
}
