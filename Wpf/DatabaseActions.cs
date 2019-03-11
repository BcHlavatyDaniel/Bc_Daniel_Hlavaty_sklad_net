using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace Bakalaris
{
    

    class DatabaseActions
    {
        private string connMainStr;

        public DatabaseActions()
        {
           connMainStr = "Server=127.0.0.1; port = 3306; Database=bakalarkadb;Uid=root;";
        }

        public void DeleteData(DataRowView row)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "DELETE from main WHERE FirstName = @fName AND SecondName = @sName AND Stat = @dStat;";
                cmd.Parameters.AddWithValue("@fName", row.Row.ItemArray[0].ToString());
                cmd.Parameters.AddWithValue("@sName", row.Row.ItemArray[1].ToString());
                cmd.Parameters.AddWithValue("@dStat", row.Row.ItemArray[2].ToString());
                cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

        }

        public void AddData(string name, string sName, string stat, bool skladom, char type, DateTime arrivalStr, DateTime leaveStr)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "INSERT INTO main(FirstName, SecondName, Stat, StampAdded, StampToLeave, CheckedOut, Type)VALUES(@fName, @sName, @Stat, DATE(@StampAdded), DATE(@StampToLeave), @checkedOut, @typ)";
                cmd.Parameters.AddWithValue("@fName", name);
                cmd.Parameters.AddWithValue("@sName", sName);
                cmd.Parameters.AddWithValue("@Stat", stat);
                cmd.Parameters.AddWithValue("@StampAdded", arrivalStr);
                cmd.Parameters.AddWithValue("@StampToLeave", leaveStr);
                cmd.Parameters.AddWithValue("@checkedOut", skladom);
                cmd.Parameters.AddWithValue("@typ", type);
                cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if(mSql.State == ConnectionState.Open) mSql.Close();
            }

        }

        public void EditData(DataRowView row, int index)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "UPDATE main SET FirstName = @fName, SecondName = @sName, Stat = @sStat, CheckedOut = @sChecked, Type = @sType WHERE id = @sId"; //TO DO update dates
                cmd.Parameters.AddWithValue("@fName", row.Row.ItemArray[0].ToString());
                cmd.Parameters.AddWithValue("@sName", row.Row.ItemArray[1].ToString());
                cmd.Parameters.AddWithValue("@sStat", row.Row.ItemArray[2].ToString());
                cmd.Parameters.AddWithValue("@sChecked", row.Row.ItemArray[3].ToString());
                cmd.Parameters.AddWithValue("@sType", row.Row.ItemArray[4].ToString());
                cmd.Parameters.AddWithValue("@sId", row.Row.ItemArray[7].ToString());
                cmd.ExecuteNonQuery();
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }
        }

        public DataSet LoadSearchedData(string name, string sName)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            MySqlCommand cmd = mSql.CreateCommand();

            if (name == "")
            {
                cmd.CommandText = "SELECT * FROM main WHERE SecondName = @sName";
                cmd.Parameters.AddWithValue("@sName", sName);
            } else if (sName == "")
            {
                cmd.CommandText = "SELECT * FROM main WHERE FirstName = @name";
                cmd.Parameters.AddWithValue("@name", name);
            } else
            {
                cmd.CommandText = "SELECT * FROM main WHERE FirstName = @name AND SecondName = @sName";
                cmd.Parameters.AddWithValue("@sName", sName);
                cmd.Parameters.AddWithValue("@name", name);
            }

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            data = new DataSet();
            adapter.Fill(data);
            return data;
        }

        public DataSet LoadData()
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM main";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

    }
}