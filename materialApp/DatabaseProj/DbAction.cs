using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;

namespace DatabaseProj
{

    public class DbActions
    {
        public DbActions()
        {
        }

        public DataSet LoadAllUsers()
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        public void AddUser(User userStruct)
        {
            string year = DateTime.Now.Year.ToString().Substring(2, 2);

            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM user WHERE year = @year";
                cmd.Parameters.AddWithValue("@year", year);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet data = new DataSet();
                adapter.Fill(data);

                int pickId = 100;
                bool breaker = false;
                while (breaker == false)
                {
                    breaker = true;
                    foreach (DataRow row in data.Tables[0].Rows)
                    {
                        if (row["_numbers"].ToString() == pickId.ToString())
                        {
                            pickId++;
                            breaker = false;
                        }
                    }
                }

                cmd = mSql.CreateCommand();
                cmd.CommandText = "INSERT INTO user (year, _numbers, first_name, second_name, address, telephone, created_at) VALUES (@year, @num, @fname, @sname, @address, @tel, @date)";
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@num", pickId);
                cmd.Parameters.AddWithValue("@fname", userStruct.FName);
                cmd.Parameters.AddWithValue("@sname", userStruct.SName);
                cmd.Parameters.AddWithValue("@address", userStruct.Address);
                cmd.Parameters.AddWithValue("@tel", userStruct.Id);
                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }
        }


        public DataSet LoadSpecificUser(string keyy, string keyn)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM user WHERE year = @year AND _numbers = @numbers";
                cmd.Parameters.AddWithValue("@year", keyy);
                cmd.Parameters.AddWithValue("@numbers", keyn);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        public Item LoadSpecificItem2(string key)
        {
            DataRow row = LoadSpecificItem(key).Tables[0].Rows[0];
            Item i = new Item
            {
                Description = row["description"].ToString(),
                Price = Double.Parse(row["price"].ToString()),
                Size = row["size"].ToString(),
                Name = row["name"].ToString(),
                Archived = row["archived"].ToString() == "True",
                State = int.Parse(row["stav"].ToString()),
                Id = int.Parse(row["id"].ToString()),
                UserNumber = int.Parse(row["user_numbers"].ToString()),
                UserYear = int.Parse(row["user_year"].ToString()),
                Photo = row["photo"].ToString()
            };
            return i;
        }
        public DataSet LoadSpecificItem(string key)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
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
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        public void UpdateUser2(User oldUser)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "UPDATE user SET first_name = @f_name, second_name = @s_name, address = @address, telephone = @tel WHERE year = @keyy AND _numbers = @keyn";
                cmd.Parameters.AddWithValue("@s_name",oldUser.SName);
                cmd.Parameters.AddWithValue("@f_name",oldUser.FName);
                cmd.Parameters.AddWithValue("@address",oldUser.Address);
                cmd.Parameters.AddWithValue("@tel",oldUser.Phone);
                cmd.Parameters.AddWithValue("@keyy", oldUser.IdYear);
                cmd.Parameters.AddWithValue("@keyn", oldUser.IdNumber);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }
        }

        public DataSet SearchForUserNames(User userStruct)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;
            int counter = 0;
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "Select * from user WHERE ";
                if (userStruct.FName != "")
                {
                    counter++;
                    cmd.CommandText += "first_name = @fname ";
                    cmd.Parameters.AddWithValue("@fname", userStruct.FName);
                }
                if (userStruct.SName != "")
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "second_name = @sname ";
                    cmd.Parameters.AddWithValue("@sname", userStruct.SName);
                    counter++;
                }
                if (userStruct.Address != "")
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "address = @address ";
                    cmd.Parameters.AddWithValue("@address", userStruct.Address);
                    counter++;
                }
                if (userStruct.Phone != 0)
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "telephone = @phone ";
                    cmd.Parameters.AddWithValue("@phone", userStruct.Phone);
                    counter++;
                }
                if (userStruct.IdYear != 0)
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "year = @year AND _numbers = @numbers";
                    cmd.Parameters.AddWithValue("@year", userStruct.IdYear);
                    cmd.Parameters.AddWithValue("@numbers", userStruct.IdNumber);
                    counter++;
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        ///<summary>
        ///         ITEM
        ///</summary>
        ///

        public void UpdateSpecificItem(string id, int type, string newVal)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                if (type == 0) //desription
                {
                    cmd.CommandText = "UPDATE item SET description = @desc WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@desc", newVal);
                }
                else if (type == 1) //size
                {
                    cmd.CommandText = "UPDATE item SET size = @size WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@size", newVal);
                }
                else if (type == 2) //price
                {
                    cmd.CommandText = "UPDATE item SET price = @price WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@price", newVal);
                }
                else if (type == 3)//name
                {
                    cmd.CommandText = "UPDATE item SET name = @name WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", newVal);
                }
                else if (type == 4) //photo
                {
                    cmd.CommandText = "UPDATE item SET photo = @photo WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@photo", newVal);
                }
                else if (type == 5) //state
                {
                    cmd.CommandText = "UPDATE item SET stav = @state WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@state", newVal);
                }
                else if (type == 6) //Archivovat
                {
                    cmd.CommandText = "UPDATE item SET archived = @archived WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    if (newVal == "True") newVal = "1";
                    else newVal = "0";
                    cmd.Parameters.AddWithValue("@archived", newVal);
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }
        }

        public bool LoadSpecificItemPaidType(string id) 
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;
            bool ret = false;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "Select * from item WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);

                int k;
                int.TryParse(data.Tables[0].Rows[0]["stav"].ToString(), out k);
                if (k == 1) ret = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return ret;
        }

        public DataSet LoadAllItems()
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "Select * from item";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }



        public DataSet SearchForItems(User userStruct, ref DataSet userSet) 
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data = new DataSet();
            int count = 0;
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * from user WHERE";
                if (userStruct.IdYear != 0)
                {
                    cmd.CommandText += " year = @year AND _numbers = @numbers";
                    cmd.Parameters.AddWithValue("@year", userStruct.IdYear);
                    cmd.Parameters.AddWithValue("@numbers", userStruct.IdNumber);
                    count++;
                }
                if (userStruct.FName != "")
                {
                    if (count > 0)
                    {
                        cmd.CommandText += " AND";
                    }
                    cmd.CommandText += " first_name = @fname";
                    cmd.Parameters.AddWithValue("@fname", userStruct.FName); 
                    count++;
                }
                if (userStruct.SName != "")
                {
                    if (count > 0)
                    {
                        cmd.CommandText += " AND";
                    }
                    cmd.CommandText += " second_name = @sname";
                    cmd.Parameters.AddWithValue("@sname", userStruct.SName);
                    count++;
                }
                if (count > 0)
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    userSet = new DataSet();
                    adapter.Fill(userSet);
                    List<string> validIds = new List<string>();
                    foreach (DataRow row in userSet.Tables[0].Rows)
                    {
                        validIds.Add(row["year"].ToString() + "-" + row["_numbers"].ToString());
                    }
                    if (validIds.Count == 0)    
                    {
                        cmd = mSql.CreateCommand();
                        cmd.CommandText = "SELECT * from item WHERE name = @name";
                        cmd.Parameters.AddWithValue("@name", "audasjknasdkdsasd");
                        adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(data);
                        return data;
                    }

                    cmd = mSql.CreateCommand();
                    cmd.CommandText = "SELECT * from item WHERE";
                    count = 0;
                    foreach (string ids in validIds)
                    {
                        if (count != 0)
                        {
                            cmd.CommandText += " OR";
                        }
                        cmd.CommandText += " (user_year = @u_year" +count +" AND user_numbers = @u_num" + count + ")";
                        cmd.Parameters.AddWithValue("@u_year" + count, ids.Substring(0, 2));
                        cmd.Parameters.AddWithValue("@u_num" + count, ids.Substring(3, 3));
                        count++;
                    }

                    if (userStruct.Address != "")
                    {
                        if (count != 0)
                        {
                            cmd.CommandText += " AND";
                        }
                        cmd.CommandText += " name = @name";
                        cmd.Parameters.AddWithValue("@name", userStruct.Address);
                        adapter = new MySqlDataAdapter(cmd);
                        data = new DataSet();
                        adapter.Fill(data);
                    }
                    else
                    {
                        data = LoadAllItems();
                    }
                }
                else
                {
                    userSet = LoadAllUsers();

                    cmd = mSql.CreateCommand();
                    cmd.CommandText += "SELECT * from item WHERE name = @name";
                    cmd.Parameters.AddWithValue("@name", userStruct.Address);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(data);
                }                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        public void AddItem(Item item)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            if (item.Photo == "") item.Photo = "";
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "INSERT INTO item(user_year, user_numbers, name, description, size, price, photo, stav, archived) VALUES (@user_year, @user_numbers, @name, @description, @size, @price, @photo, @stav, @archived)";
                cmd.Parameters.AddWithValue("@user_year", item.UserYear);
                cmd.Parameters.AddWithValue("@user_numbers", item.UserNumber);
                cmd.Parameters.AddWithValue("@description", item.Description);
                cmd.Parameters.AddWithValue("@size", item.Size);
                cmd.Parameters.AddWithValue("@price", item.Price);
                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@photo", item.Photo);
                cmd.Parameters.AddWithValue("@stav", 0);
                cmd.Parameters.AddWithValue("@archived", 0);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }
        }

        public DataSet SearchForItemsByName(string name, string year, string number)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM item WHERE name = @name AND user_year = @year AND user_numbers = @numbers";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@numbers", number);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        public DataSet SearchForItemsByUserkeys(string keyy, string keyn)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
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

        public DataSet LoadAllLogs()
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM log";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        public int GetLastItemId()
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();

            DataSet data;
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * from item ORDER BY id DESC LIMIT 0, 1";
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
                return Convert.ToInt32(data.Tables[0].Rows[0]["id"]);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

        }

        public void AddLog(string itemId, string userId, int type, string changeText)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "INSERT INTO log(item_id, user_id, type, change_text, time) VALUES(@item_id, @user_id, @type, @change_text, @time)" ;
                cmd.Parameters.AddWithValue("@item_id", itemId);
                cmd.Parameters.AddWithValue("@user_id", userId);
                cmd.Parameters.AddWithValue("@type", type);
                cmd.Parameters.AddWithValue("@change_text", changeText);
                cmd.Parameters.AddWithValue("@time", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

        }

        public DataSet SearchForLogs(string item_id, string user_id, string type, DateTime day)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM log WHERE";
                int count = 0;
                if (item_id != "")
                {
                    count++;
                    cmd.CommandText += " item_id = @i_id";
                    cmd.Parameters.AddWithValue("@i_id", item_id);
                }
                if (user_id != "")
                {
                    if (count > 0) cmd.CommandText += " AND";
                    cmd.CommandText += " user_id = @u_id";
                    cmd.Parameters.AddWithValue("@u_id", user_id);
                    count++;
                }
                if (type != "")
                {
                    if (count > 0) cmd.CommandText += " AND";
                    cmd.CommandText += " type = @type";
                    cmd.Parameters.AddWithValue("@type", type);
                    count++;
                }
                if (day.ToShortDateString() != "1/1/0001")
                {
                    if (count > 0) cmd.CommandText += " AND";
                    cmd.CommandText += " time BETWEEN @daystart AND @dayend";
                    string[] dates = day.ToShortDateString().Split('/');
                    string date = dates[2] + "/" + dates[0] + "/" + dates[1];
                    cmd.Parameters.AddWithValue("@daystart", date + " 00:00:00");
                    cmd.Parameters.AddWithValue("@dayend", date + " 23:59:59");
                }

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }

            return data;
        }

        public int GetNumberOfItemsForUser(string year, string numbers)
        {
            MySqlConnection mSql = new MySqlConnection(Settings.ConnectionString);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "Select * from item where user_year = @year and user_numbers =@numbers";
                cmd.Parameters.AddWithValue("@year", year);
                cmd.Parameters.AddWithValue("@numbers", numbers);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                data = new DataSet();
                adapter.Fill(data);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mSql.State == ConnectionState.Open) mSql.Close();
            }
            //TO DO pocitam len tie ktore su na sklade, ci vsetky?
            return data.Tables[0].Rows.Count;
        }
    }
}
