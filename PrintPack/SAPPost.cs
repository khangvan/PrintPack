using System;
using System.Xml;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
//using ACStools;

namespace ACSEE.NET
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class SAPPost : IDisposable
    {
        private bool disposed = false;
        public XmlNode root;
        public XmlDocument xDoc;
        public string strOuterXML;
        public WebClient http;

        public DataSet ds;

        public SAPPost()
        {
            
            this.xDoc = new XmlDocument();
            this.http = new WebClient();
        }


        public SAPPost(string rootNodeName)
        {
            this.xDoc = new XmlDocument();
            this.http = new WebClient();

            this.xDoc.LoadXml("<?xml version=\"1.0\" ?><" + rootNodeName.Trim() + "/>");
            this.root = this.xDoc.DocumentElement;

        }

        public XmlDocument getXDOC()
        {
            return this.xDoc;
        }

        public void setProperty(string nodename, string value)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name.ToString());
            }

            XmlElement elem = this.xDoc.CreateElement(nodename);
            elem.InnerText = value.Trim();
            this.root.AppendChild(elem);
        }


        public void SetTable(String tableName, DataTable tableData)
        {

            XmlNode node;

            XmlElement elem;

            XmlElement baseElement;



            if (this.disposed)
            {

                throw new ObjectDisposedException(this.GetType().Name);

            }



            node = this.xDoc.SelectSingleNode("//" + this.root.Name + "/" + tableName);

            if (node != null)
            {

                node.RemoveAll();

            }

            else
            {

                this.root.AppendChild(this.xDoc.CreateElement(tableName));

                node = this.xDoc.SelectSingleNode("//" + this.root.Name + "/" + tableName);

            }



            for (int i = 0; i < tableData.Rows.Count; i++)
            {

                baseElement = this.xDoc.CreateElement("item");

                for (int c = 0; c < tableData.Columns.Count; c++)
                {

                    elem = this.xDoc.CreateElement(tableData.Columns[c].ColumnName);

                    elem.InnerText = tableData.Rows[i][c].ToString(); // .ItemArray[c].ToString();

                    baseElement.AppendChild(elem);

                }

                node.AppendChild(baseElement);

            }



        }


        public SAPXML Post(string URI, string rootName)
        {

            NameValueCollection data = new NameValueCollection();
            byte[] b;
            string s;
            string what;
            int j;

            ds = new DataSet();

            what = this.xDoc.OuterXml;

            data.Add("XDoc", this.xDoc.OuterXml);
            b = http.UploadValues(URI, "POST", data);
            s = Encoding.ASCII.GetString(b);





            j = 3;

            j = j + 5;


            return new SAPXML(s, rootName);

        }


        public SAPXML Post(string URI)
        {
            NameValueCollection data = new NameValueCollection();
            byte[] b;
            string s;
            string what;


            ds = new DataSet();

            what = this.xDoc.OuterXml;

            data.Add("XDoc", this.xDoc.OuterXml);
            b = http.UploadValues(URI, "POST", data);
            s = Encoding.ASCII.GetString(b);

            return new SAPXML(s, "ROOT");
        }


        protected string mState;
        public string State
        {
            get
            {
                return this.xDoc.OuterXml;
            }
            set
            {
                this.xDoc.LoadXml(value);
                this.root = this.xDoc.DocumentElement;
            }
        }

        ~SAPPost()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (this.disposed == false)
            {

                if (disposing)
                {
                    http.Dispose();
                    this.root.RemoveAll();
                    this.xDoc.RemoveAll();
                    // dispose managed resources
                }
                // dispose unmanaged resources
            }
            disposed = true;
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }



}
