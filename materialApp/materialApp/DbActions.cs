using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace materialApp
{
    class DbActions
    {
        private string connMainStr;

        public DbActions()
        {
            connMainStr = "Server=127.0.0.1; port = 3306; Database=wpfdata;Uid=root;Convert Zero Datetime=True";
        }

        public DataSet LoadData()
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM user";
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

        public void EditUserData(EditUserStruct userStruct)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "UPDATE user SET first_name = @f_name, second_name = @s_name, address = @address, telephone = @tel WHERE year = @keyy AND _numbers = @keyn";
                cmd.Parameters.AddWithValue("@s_name",userStruct.s_name);
                cmd.Parameters.AddWithValue("@f_name", userStruct.f_name);
                cmd.Parameters.AddWithValue("@address", userStruct.address);
                cmd.Parameters.AddWithValue("@tel", userStruct.tel);
                cmd.Parameters.AddWithValue("@keyy", userStruct.keyy);
                cmd.Parameters.AddWithValue("@keyn", userStruct.keyn);
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
            //return error/sucess
        }

        public void AddItem(EditItemStruct itemStruct)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "INSERT INTO item(user_year, user_numbers, description, size, price, photo, created_at) VALUES (@user_year, @user_numbers, @description, @size, @price, @photo, @created_at)";
                cmd.Parameters.AddWithValue("@user_year", itemStruct.keyy);
                cmd.Parameters.AddWithValue("@user_numbers", itemStruct.keyn);
                cmd.Parameters.AddWithValue("@description", itemStruct.description);
                cmd.Parameters.AddWithValue("@size", itemStruct.size);
                cmd.Parameters.AddWithValue("@price", itemStruct.price);
                cmd.Parameters.AddWithValue("@photo", itemStruct.photo);
                cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
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

        public void EditItemData(EditItemStruct itemStruct)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "UPDATE item SET description = @desc, size = @size, price = @price";
                cmd.Parameters.AddWithValue("@desc", itemStruct.description);
                cmd.Parameters.AddWithValue("@size", itemStruct.size);
                cmd.Parameters.AddWithValue("@price", itemStruct.price);
                if (itemStruct.created_at.ToString() != "1/1/0001 12:00:00 AM")
                {
                    cmd.CommandText += ", created_at = @created_at";
                    cmd.Parameters.AddWithValue("@created_at", itemStruct.created_at);
                }
                if (itemStruct.sold_at.ToString() != "1/1/0001 12:00:00 AM")
                {
                    cmd.CommandText += ", sold_at = @sold_at";
                    cmd.Parameters.AddWithValue("@sold_at", itemStruct.sold_at);
                }
                if (itemStruct.returned_at.ToString() != "1/1/0001 12:00:00 AM")
                {
                    cmd.CommandText += ", returned_at = @returned_at";
                    cmd.Parameters.AddWithValue("@returned_at", itemStruct.returned_at);
                }
                if (itemStruct.paid_at.ToString() != "1/1/0001 12:00:00 AM")
                {
                    cmd.CommandText += ", paid_at = @paid_at";
                    cmd.Parameters.AddWithValue("@paid_at", itemStruct.paid_at);
                }
                if (itemStruct.photo != "")
                {
                    cmd.CommandText += ", photo = @photo";
                    cmd.Parameters.AddWithValue("@photo", itemStruct.photo);
                }
                cmd.CommandText += " WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", itemStruct.id);
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

        public DataSet LoadItemData(string key)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM item WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", key);
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

        public DataSet LoadUserData(string keyy, string keyn)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM item WHERE user_year = @year AND user_numbers = @numbers";
                cmd.Parameters.AddWithValue("@year", keyy);
                cmd.Parameters.AddWithValue("@numbers", keyn);
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

        public DataSet LoadSearchedNamesData(string name, string sName)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            MySqlCommand cmd = mSql.CreateCommand();

            if(name == "")
            {
                cmd.CommandText = "SELECT * from user WHERE second_name = @name";
                cmd.Parameters.AddWithValue("@name", sName);
            } 
            else if(sName == "")
            {
                cmd.CommandText = "SELECT * from user WHERE first_name = @name";
                cmd.Parameters.AddWithValue("@name", name);
            }
            else
            {
                cmd.CommandText = "SELECT * from user WHERE first_name = @name AND second_name = @sName";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@sName", sName);
            }

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            data = new DataSet();
            adapter.Fill(data);

            return data;
        }
    }
}
