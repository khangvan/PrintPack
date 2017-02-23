using System;
using System.Xml;
using System.Data;
using System.Collections;
//using ACStools;

namespace ACSEE.NET
{
    /// <summary>
    /// Summary description for SAPXML.
    /// </summary>
    public class SAPXML : IDisposable
    {
        private bool disposed = false;
        private string root = string.Empty;
        private XmlDocument xDoc;

        public SAPXML()
        {
            //
            // TODO: Done- Add constructor logic here
            //
        }

        public XmlDocument getXDOC()
        {

            return this.xDoc;
        }

        public SAPXML(string xmlDocument, string rootNodename)
        {

            this.xDoc = new XmlDocument();
            this.xDoc.LoadXml(xmlDocument);
            this.root = "//" + rootNodename.Trim();

        }


        public string getRoot()
        {
            return this.root;
        }


        public ACSXMLDescription getXMLasTree()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name.ToString());
            }
            DataTable dt = new DataTable();
            //			DataRow dr ;

            XmlNodeList nodeList = xDoc.ChildNodes;

            ACSXMLDescription firstNode = new ACSXMLDescription();
            ACSXMLDescription currentNode;

            currentNode = firstNode;

            for (int i = 0; i < nodeList.Count; i++)
            {

                XmlNode myItem = nodeList.Item(i);

                if (i > 0)
                {


                    if (i == 1)
                    {

                        currentNode = firstNode;
                    }
                    else
                    {
                        ACSXMLDescription newNode = new ACSXMLDescription();
                        currentNode.nextNode = newNode;
                        currentNode = newNode;
                    }

                    currentNode.name = myItem.Name.ToString();


                    currentNode.bIsMethod = true;
                    if (myItem.HasChildNodes)
                    {
                        XmlNodeList node2List = myItem.ChildNodes;

                        int ji = node2List.Count;
                        currentNode.subChildren = new ArrayList(55);

                        try
                        {
                            for (int k = 0; k < node2List.Count; k++)
                            {
                                XmlNode subItem = node2List.Item(k);

                                ACSXMLDescription paramNode = new ACSXMLDescription();
                                paramNode.name = subItem.Name.ToString();
                                paramNode.value = subItem.InnerText.ToString();
                                int v = currentNode.subChildren.Add(paramNode);
                            }
                        }
                        catch (Exception ex)
                        {
                            string mess = ex.Message.ToString();
                            i = 23;
                        }
                    }
                    String strInner = myItem.InnerText.ToString();
                    int ki = 3;
                    int j = ki + 3;
                }
            }

            return firstNode;


        }




        public DataTable getTopNodeList()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name.ToString());
            }
            DataTable dt = new DataTable();
            //			DataRow dr ;

            XmlNodeList nodeList = xDoc.ChildNodes;

            ACSXMLDescription firstNode = new ACSXMLDescription();
            ACSXMLDescription currentNode;

            currentNode = firstNode;

            for (int i = 0; i < nodeList.Count; i++)
            {

                XmlNode myItem = nodeList.Item(i);

                if (i > 0)
                {


                    if (i == 1)
                    {

                        currentNode = firstNode;
                    }
                    else
                    {
                        ACSXMLDescription newNode = new ACSXMLDescription();
                        currentNode.nextNode = newNode;
                        currentNode = newNode;
                    }

                    currentNode.name = myItem.Name.ToString();


                    currentNode.bIsMethod = true;
                    if (myItem.HasChildNodes)
                    {
                        XmlNodeList node2List = myItem.ChildNodes;

                        int ji = node2List.Count;
                        currentNode.subChildren = new ArrayList(55);

                        try
                        {
                            for (int k = 0; k < node2List.Count; k++)
                            {
                                XmlNode subItem = node2List.Item(k);

                                ACSXMLDescription paramNode = new ACSXMLDescription();
                                paramNode.name = subItem.Name.ToString();
                                paramNode.value = subItem.InnerText.ToString();
                                int v = currentNode.subChildren.Add(paramNode);
                            }
                        }
                        catch (Exception ex)
                        {
                            string mess = ex.Message.ToString();
                            i = 23;
                        }
                    }
                    String strInner = myItem.InnerText.ToString();
                    int ki = 3;
                    int j = ki + 3;
                }
            }

            return dt;


        }

        public DataTable getNodeList()
        {

            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name.ToString());
            }

            DataTable dt = new DataTable();
            DataRow dr;
            XmlNodeList nodeList = xDoc.SelectSingleNode(this.root).ChildNodes;

            dt.Columns.Add("NodeName", System.Type.GetType("System.String"));
            dt.Columns.Add("IsDataTable", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("DataType", typeof(Type));

            for (int i = 0; i < nodeList.Count; i++)
            {
                dr = dt.NewRow();
                dr["NodeName"] = nodeList.Item(i).Name;
                if (nodeList.Item(i).ChildNodes.Item(0).HasChildNodes)
                {
                    dr["IsDataTable"] = true;
                    System.Type atype = System.Type.GetType("System.Data.DataTable");
                    //			dr["DataType"] = System.Type.GetType("System.Data.DataTable") ;
                    dr["DataType"] = typeof(DataTable);
                }
                else
                {
                    dr["IsDataTable"] = false;

                    if (nodeList.Item(i).Attributes["Type"] == null)
                    {
                        //int y = 3;
                    }
                    else
                    {
                        dr["DataType"] = this.ADOtoType(Int16.Parse(nodeList.Item(i).Attributes["Type"].Value.ToString()));
                    }
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public string getAProperty(string nodeName)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name.ToString());
            }

            return this.xDoc.SelectSingleNode(this.root + "//" + nodeName).InnerText;
        }


        public DataTable getFirstDataTable(string nodeName)
        {

            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name.ToString());
            }

            DataTable dt = new DataTable();
            XmlNodeList nodeList = xDoc.SelectSingleNode(this.root + "//" + nodeName).ChildNodes;

            nodeList = nodeList.Item(0).ChildNodes;

            if (nodeList.Count == 0)
            {
                dt = null;
            }
            else
            {
                DataRow dr;
                int i;
                int c;

                for (i = 0; i < nodeList.Item(0).ChildNodes.Count; i++)
                {
                    //				dt.Columns.Add(nodeList.Item(0).ChildNodes.Item(i).Name, 2) ;
                    int x = nodeList.Item(0).ChildNodes.Count;
                    XmlAttributeCollection myatts = nodeList.Item(0).ChildNodes.Item(i).Attributes;
                    dt.Columns.Add(nodeList.Item(0).ChildNodes.Item(i).Name, this.ADOtoType(UInt16.Parse(nodeList.Item(0).ChildNodes.Item(i).Attributes.Item(0).Value)));
                }


                for (i = 0; i < nodeList.Count; i++)
                {
                    dr = dt.NewRow();
                    for (c = 0; c < nodeList.Item(i).ChildNodes.Count; c++)
                    {
                        string x = nodeList.Item(i).ChildNodes.Item(c).InnerText.Trim();
                        if (x == string.Empty)
                        {
                            dr[nodeList.Item(i).ChildNodes.Item(c).Name] = System.DBNull.Value;

                        }
                        else
                        {
                            dr[nodeList.Item(i).ChildNodes.Item(c).Name] = nodeList.Item(i).ChildNodes.Item(c).InnerText.Trim();
                        }
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;

        }

        public DataTable getDataTable(string nodeName)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name.ToString());
            }

            DataTable dt = new DataTable();
            XmlNodeList nodeList = xDoc.SelectSingleNode(this.root + "//" + nodeName).ChildNodes;

            if (nodeList.Count == 0)
            {
                dt = null;
            }
            else
            {
                DataRow dr;
                int i;
                int c;

                for (i = 0; i < nodeList.Item(0).ChildNodes.Count; i++)
                {
                    //				dt.Columns.Add(nodeList.Item(0).ChildNodes.Item(i).Name, 2) ;
                    int x = nodeList.Item(0).ChildNodes.Count;
                    XmlAttributeCollection myatts = nodeList.Item(0).ChildNodes.Item(i).Attributes;
                    dt.Columns.Add(nodeList.Item(0).ChildNodes.Item(i).Name, this.ADOtoType(UInt16.Parse(nodeList.Item(0).ChildNodes.Item(i).Attributes.Item(0).Value)));
                }


                for (i = 0; i < nodeList.Count; i++)
                {
                    dr = dt.NewRow();
                    for (c = 0; c < nodeList.Item(i).ChildNodes.Count; c++)
                    {
                        string x = nodeList.Item(i).ChildNodes.Item(c).InnerText.Trim();
                        if (x == string.Empty)
                        {
                            dr[nodeList.Item(i).ChildNodes.Item(c).Name] = System.DBNull.Value;

                        }
                        else
                        {
                            dr[nodeList.Item(i).ChildNodes.Item(c).Name] = nodeList.Item(i).ChildNodes.Item(c).InnerText.Trim();
                        }
                    }
                    dt.Rows.Add(dr);
                }

            }
            return dt;
        }





        public static int TypetoADO(System.Type xy)
        {
            int t = 130;


            switch (xy.ToString())
            {
                case "System.Int32":
                    t = 3;

                    break;

                case "System.DateTime":
                    t = 7;
                    break;


                case "System.Decimal":
                    t = 6;
                    break;


                case "System.Double":
                    t = 5;
                    break;

                case "System.Single":
                    t = 4;
                    break;

                case "System.Byte":
                    t = 17;
                    break;


                case "System.UInt16":
                    t = 18;
                    break;

                case "System.UInt32":
                    t = 19;
                    break;

                case "System.UInt64":
                    t = 21;
                    break;

                case "System.String":
                    t = 130;
                    break;
            }


            return t;
        }


        public System.Type ADOtoType(int value)
        {
            System.Type t = null;



            switch (value)
            {
                case 3:
                    t = System.Type.GetType("System.Int32");
                    break;

                case 7:
                    t = System.Type.GetType("System.DateTime");
                    break;

                case 133:
                    t = System.Type.GetType("System.DateTime");
                    break;

                case 6:
                    t = System.Type.GetType("System.Decimal");
                    break;

                case 14:
                    t = System.Type.GetType("System.Decimal");
                    break;

                case 5:
                    t = System.Type.GetType("System.Double");
                    break;

                case 4:
                    t = System.Type.GetType("System.Single");
                    break;

                case 17:
                    t = System.Type.GetType("System.Byte");
                    break;


                case 18:
                    t = System.Type.GetType("System.UInt16");
                    break;

                case 19:
                    t = System.Type.GetType("System.UInt32");
                    break;

                case 21:
                    t = System.Type.GetType("System.UInt64");
                    break;

                case 130:
                    t = System.Type.GetType("System.String");
                    break;

                case 134:
                    t = System.Type.GetType("System.String");

                    break;
            }


            return t;
        }
        ~SAPXML()
        {
            Dispose(false);
        }

        protected void Dispose(bool disposing)
        {
            if (this.disposed == false)
            {
                if (disposing)
                {
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
