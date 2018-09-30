using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SterlingForexService
{
    class MSQconn
    {

        private SqlConnection conn;
        private SqlCommand sql;
        public int num_rows;
        public int returnValue;
        public bool persist = false;

        public string errmsg;

        public MSQconn(string connstring)
        {
            conn = new SqlConnection();
            conn.ConnectionString = connstring;
            conn.Open();
        }

        public void SetSQL(string query)
        {
            sql = new SqlCommand();
            sql.Connection = conn;
            sql.CommandText = query;
            sql.CommandType = CommandType.Text;
        }

        public void SetProcedure(string query)
        {
            sql = new SqlCommand();
            sql.Connection = conn;
            sql.CommandText = query;
            sql.CommandType = CommandType.StoredProcedure;
        }

        public void AddParam(string key, object val)
        {
            sql.Parameters.AddWithValue(key, val);
           
        }

       
        public DataSet Query(string tblName)
        {
            num_rows = -1;
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter res = new SqlDataAdapter();
                res.SelectCommand = sql;
                res.TableMappings.Add("Table", tblName);
                res.Fill(ds);
                num_rows = ds.Tables[tblName].Rows.Count;
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
            }
            Close();
            return ds;
        }

        public int Query()
        {
            SqlParameter prm = new SqlParameter();
            prm.SqlDbType = SqlDbType.Int;
            prm.Direction = ParameterDirection.ReturnValue;
            sql.Parameters.Add(prm);
            returnValue = 0;
            int j = 0;
            try
            {
                j = sql.ExecuteNonQuery();
                returnValue = Convert.ToInt32(prm.Value);
                Close();
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
            }
            return j;
        }

        public DataSet Select()
        {
            num_rows = -1;
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter res = new SqlDataAdapter();
                res.SelectCommand = sql;
                res.TableMappings.Add("Table", "recs");
                res.Fill(ds);
                num_rows = ds.Tables["recs"].Rows.Count;

            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
            }
            Close();
            return ds;
        }

        public int Delete()
        {
            int j = 0;
            try
            {
                j = sql.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
            }
            return j;
        }

        public int Update()
        {
            int j = 0;
            try
            {
                j = sql.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
            }
            return j;
        }

        public object Insert()
        {
            sql.CommandText += "; select @@IDENTITY ";
            object j = 0;
            try
            {
                j = sql.ExecuteScalar();
                Close();
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
            }
            return j;
        }

        public string SelectScalar()
        {
            string j = "";
            try
            {
                j = Convert.ToString(sql.ExecuteScalar());
                Close();
            }
            catch (Exception ex)
            {
                errmsg = ex.ToString();
            }
            return j;
        }

        public void Close()
        {
            if (!persist)
            {
                if (conn != null) conn.Close();
            }
        }

        public void CloseAll()
        {
            if (conn != null) conn.Close();
        }


    }
}
