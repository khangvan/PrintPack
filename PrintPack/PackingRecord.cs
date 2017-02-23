

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;




namespace PrintPack
{
    partial class UserAuth
    {
        public static string StrConnection = "";
        public static SqlConnection dbCNN = new SqlConnection(StrConnection);

        public static void Open ()
        {
            
                     dbCNN.Open();
        }

        public static void Close()
        {

            dbCNN.Close();
        }

        public static void Log()
        { 
        //do nothing
        }

    }
    class PackingRecord
    {

        private string ConnectionString;
        public PackingRecord(string ConnStr)
        {
            ConnectionString = ConnStr;
        }

        private string m_PONumber;
        public string PONumber
        {
            get { return m_PONumber; }
            set { m_PONumber = value; }
        }
        private string m_Model;
        public string Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }
        private string m_Serial;
        public string Serial
        {
            get { return m_Serial; }
            set { m_Serial = value; }
        }
        private string m_BoxNumber;
        public string BoxNumber
        {
            get { return m_BoxNumber; }
            set { m_BoxNumber = value; }
        }
        private string m_PackingDateTime;
        public string PackingDateTime
        {
            get { return m_PackingDateTime; }
            set { m_PackingDateTime = value; }
        }
        private string m_PYear;
        public string PYear
        {
            get { return m_PYear; }
            set { m_PYear = value; }
        }
        private string m_ID;
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }




        public void Update(string PONumber, string Model, string Serial, string BoxNumber, string PackingDateTime, string PYear, string ID)
        {
            

            SqlCommand cmd = new SqlCommand("Update_PackingRecord", UserAuth.dbCNN);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(@PONumber, PONumber);
            cmd.Parameters.AddWithValue(@Model, Model);
            cmd.Parameters.AddWithValue(@Serial, Serial);
            cmd.Parameters.AddWithValue(@BoxNumber, BoxNumber);
            cmd.Parameters.AddWithValue(@PackingDateTime, PackingDateTime);
            cmd.Parameters.AddWithValue(@PYear, PYear);
            cmd.Parameters.AddWithValue(@ID, ID);

            try
            {
                UserAuth.dbCNN.Open();
                cmd.ExecuteNonQuery();
                //UserAuth.Log(DBAction.Update, "PackingRecord");
            }
            catch
            { }
            finally
            {
                UserAuth.dbCNN.Close();
                cmd.Dispose();
            }
        }

        public void Insert(string PONumber, string Model, string Serial, string BoxNumber, string PackingDateTime, string PYear, string ID)
        {
            SqlCommand cmd = new SqlCommand("Insert_PackingRecord", UserAuth.dbCNN);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(@PONumber, PONumber);
            cmd.Parameters.AddWithValue(@Model, Model);
            cmd.Parameters.AddWithValue(@Serial, Serial);
            cmd.Parameters.AddWithValue(@BoxNumber, BoxNumber);
            cmd.Parameters.AddWithValue(@PackingDateTime, PackingDateTime);
            cmd.Parameters.AddWithValue(@PYear, PYear);
            cmd.Parameters.AddWithValue(@ID, ID);

            try
            {
                UserAuth.dbCNN.Open();
                cmd.ExecuteNonQuery();
                //UserAuth.Log(DBAction.Insert, "PackingRecord");
            }
            catch
            { }
            finally
            {
                UserAuth.dbCNN.Close();
                cmd.Dispose();
            }
        }

        public void Delete(string ID)
        {
            SqlCommand cmd = new SqlCommand("Delete_PackingRecord", UserAuth.dbCNN);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue(@ID, ID);

            try
            {
                UserAuth.dbCNN.Open();
                cmd.ExecuteNonQuery();
                //UserAuth.Log(DBAction.Delete, "PackingRecord");
            }
            catch
            { }
            finally
            {
                UserAuth.dbCNN.Close();
                cmd.Dispose();
            }
        }

        public DataSet Select()
        {
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT * FROM Select_PackingRecord", UserAuth.dbCNN);
            DataSet dts = new DataSet();
            try
            {
                UserAuth.dbCNN.Open();
                cmd.Fill(dts);
                return dts;
            }
            catch
            { }
            finally
            {
                UserAuth.dbCNN.Close();
                cmd.Dispose();
            }
            return dts;
        }
    }
}
