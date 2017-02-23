using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Net;
using System.Text;

using System.Windows.Forms;

namespace PrintPack
{
    class StationReporting
    {
        private string _idKey;

        public string idKey
        {
            get { return _idKey; }
            set { _idKey = value; }
        }
        
        private string  _stationname;

        public string  StationName
        {
            get { return _stationname; }
            set { _stationname = value; }
        }
        

        private string _PPVersion;

        public string PPVersion
        {
            get { return _PPVersion; }
            set { _PPVersion = value; }
        }

        private string  _StartUpPath;

        public string  StartUpPath
        {
            get { return _StartUpPath; }
            set { _StartUpPath = value; }
        }
        
        private string _IP;

        public string IP
        {
            get { return _IP; }
            set { _IP = value; }
        }

        SP_Processing.MySqlConn cn = new SP_Processing.MySqlConn(PhanMem.chuoi_ket_noivnmsrv601_FFCPacking);
        public StationReporting()
        {
            //get station name
            StationName = System.Environment.MachineName; 
            //get ip
            IP = GetIP();
            //get startup part
            // Get normal filepath of this assembly's permanent directory
            var path = new Uri(
                System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().CodeBase)
                ).LocalPath;
            this.StartUpPath = path.ToString();
            //getversion
            PPVersion= GetPPVersion();

            //get key
            idKey = cn.ExecSProcDS("GetMaxKey").Tables["Table"].Rows[0][0].ToString();

            //do data
            
            
            
            
        }

        private static string GetPPVersion()
        {
            string sVersion;
            Version ver;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ver = ApplicationDeployment.CurrentDeployment.CurrentVersion;//new Version(Application.ProductVersion);
            }
            else
            {
                ver = new Version(Application.ProductVersion);
            }

            return sVersion=ver.Major.ToString()+" "+ ver.Revision.ToString();
        }
        public void DoUpdateStationWorking()
        {

        }
        public string GetIP()
        {
            string Str = "";
            Str = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Str);
            IPAddress[] addr = ipEntry.AddressList;
            return addr[addr.Length - 1].ToString();

        }

        public void DoUpdate()
        {

            
            cn.ExecSProc("DoUpdateStation", this.StationName, this.IP, PPVersion, StartUpPath, DateTime.Now.ToString(), "Start", idKey);
        }

        public void DoCloseStation()
        {

            SP_Processing.MySqlConn cn = new SP_Processing.MySqlConn(PhanMem.chuoi_ket_noivnmsrv601_FFCPacking);
            cn.ExecSProc("DoCloseStation", idKey);
        }
      
    }
}
