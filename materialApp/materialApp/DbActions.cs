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
        /// <summary>
        ///         USER
        /// </summary>

        public DataSet LoadAllUsers()
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

        public void AddUser(EditUserStruct userStruct)
        {
            string year = DateTime.Now.Year.ToString().Substring(2, 2);

            MySqlConnection mSql = new MySqlConnection(connMainStr);
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
                cmd.Parameters.AddWithValue("@fname", userStruct.f_name);
                cmd.Parameters.AddWithValue("@sname", userStruct.s_name);
                cmd.Parameters.AddWithValue("@address", userStruct.address);
                cmd.Parameters.AddWithValue("@tel", userStruct.tel);
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
            MySqlConnection mSql = new MySqlConnection(connMainStr);
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

        public void UpdateUser(EditUserStruct userStruct)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "UPDATE user SET first_name = @f_name, second_name = @s_name, address = @address, telephone = @tel WHERE year = @keyy AND _numbers = @keyn";
                cmd.Parameters.AddWithValue("@s_name", userStruct.s_name);
                cmd.Parameters.AddWithValue("@f_name", userStruct.f_name);
                cmd.Parameters.AddWithValue("@address", userStruct.address);
                cmd.Parameters.AddWithValue("@tel", userStruct.tel);
                cmd.Parameters.AddWithValue("@keyy", userStruct.keyy);
                cmd.Parameters.AddWithValue("@keyn", userStruct.keyn);
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

        public DataSet SearchForUserNames(EditUserStruct userStruct)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;
            int counter = 0;
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "Select * from user WHERE ";
                if (userStruct.f_name != "")
                {
                    counter++;
                    cmd.CommandText += "first_name = @fname ";
                    cmd.Parameters.AddWithValue("@fname", userStruct.f_name);
                }
                if (userStruct.s_name != "")
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "second_name = @sname ";
                    cmd.Parameters.AddWithValue("@sname", userStruct.s_name);
                    counter++;
                }
                if (userStruct.address != "")
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "address = @address ";
                    cmd.Parameters.AddWithValue("@address", userStruct.address);
                    counter++;
                }
                if (userStruct.tel != "")
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "telephone = @phone ";
                    cmd.Parameters.AddWithValue("@phone", userStruct.tel);
                    counter++;
                }
                if (userStruct.keyy != "")
                {
                    if (counter != 0) cmd.CommandText += "AND ";
                    cmd.CommandText += "year = @year AND _numbers = @numbers";
                    cmd.Parameters.AddWithValue("@year", userStruct.keyy);
                    cmd.Parameters.AddWithValue("@numbers", userStruct.keyn);
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
        public string LoadSaveSpecificItemDescription(string id, bool val, string desc)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();

            string ret = "";
            if (val) //save
            {
                try
                {
                    MySqlCommand cmd = mSql.CreateCommand();
                    cmd.CommandText = "UPDATE item SET description = @desc WHERE id = @id";
                    cmd.Parameters.AddWithValue("@desc", desc);
                    cmd.Parameters.AddWithValue("@id", id);
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
            else //get
            {
                
                try
                {
                    DataSet data = new DataSet();
                    MySqlCommand cmd = mSql.CreateCommand();
                    cmd.CommandText = "Select * from item where id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(data);
                    ret = data.Tables[0].Rows[0]["description"].ToString();
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
            return ret;
        }

        public void UpdateItemTimes(string id, int type)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                if (type == 0) //init
                {
                    cmd.CommandText = "UPDATE item SET sold_at = @time, used_card = @card, returned_at = @ret, paid_at = @paid, archived_at = @arch WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@card", 0);
                    cmd.Parameters.AddWithValue("@time", "");
                    cmd.Parameters.AddWithValue("@ret", "");
                    cmd.Parameters.AddWithValue("@paid", "");
                    cmd.Parameters.AddWithValue("@arch", "");
                }
                else if (type == 1) //sellCard
                {
                    cmd.CommandText = "UPDATE item SET sold_at = @time, used_card = @card, archived_at = @arch, returned_at = @ret, paid_at = @paid WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ret", "");
                    cmd.Parameters.AddWithValue("@card", 1);
                    cmd.Parameters.AddWithValue("@paid", "");
                    cmd.Parameters.AddWithValue("@arch", "");
                }
                else if (type == 2) //sellCash
                {
                    cmd.CommandText = "UPDATE item SET sold_at = @time, used_card = @card, archived_at = @arch, returned_at = @ret, paid_at = @paid WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@ret", "");
                    cmd.Parameters.AddWithValue("@card", 0);
                    cmd.Parameters.AddWithValue("@paid", "");
                    cmd.Parameters.AddWithValue("@arch", "");
                }
                else if (type == 3)//item_return
                {
                    cmd.CommandText = "UPDATE item SET returned_at = @time, archived_at = @arch, used_card = @card, paid_at = @paid, sold_at = @sold WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", DateTime.Now);
                    cmd.Parameters.AddWithValue("@paid", "");
                    cmd.Parameters.AddWithValue("@sold", "");
                    cmd.Parameters.AddWithValue("@card", 0);
                    cmd.Parameters.AddWithValue("@arch", "");
                }
                else if (type == 4) //paidCard
                {
                    cmd.CommandText = "UPDATE item SET returned_at = @time, archived_at = @arch, used_card = @card, paid_at = @paid WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", "");
                    cmd.Parameters.AddWithValue("@paid", DateTime.Now);
                    cmd.Parameters.AddWithValue("@card", 1);
                    cmd.Parameters.AddWithValue("@arch", "");
                }
                else if (type == 5) //paidCash
                {
                    cmd.CommandText = "UPDATE item SET returned_at = @time, used_card = @card, archived_at = @arch, paid_at = @paid WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", "");
                    cmd.Parameters.AddWithValue("@paid", DateTime.Now);
                    cmd.Parameters.AddWithValue("@card", 0);
                    cmd.Parameters.AddWithValue("@arch", "");
                }
                else if (type == 6) //Archivovat
                {
                    cmd.CommandText = "UPDATE item SET returned_at = @time, sold_at = @sold, used_card = @card, archived_at = @arch, paid_at = @paid WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@time", "");
                    cmd.Parameters.AddWithValue("@paid", "");
                    cmd.Parameters.AddWithValue("@card", 0);
                    cmd.Parameters.AddWithValue("@arch", DateTime.Now);
                    cmd.Parameters.AddWithValue("@sold", "");
                }
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

        public bool LoadSpecificItemPaidType(string id)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
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
                int.TryParse(data.Tables[0].Rows[0]["used_card"].ToString(), out k);
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
            MySqlConnection mSql = new MySqlConnection(connMainStr);
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



        public DataSet SearchForItems(EditUserStruct userStruct) // TO DO  !
        {

            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data = new DataSet();
            int count = 0;
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();

                if (userStruct.keyy != "")
                {
                    //vrati vsetky itemy kde key a keyn == databazovym
                    cmd.CommandText = "Select * from item WHERE user_year = @keyy AND user_numbers = @keyn";
                    cmd.Parameters.AddWithValue("@keyy", userStruct.keyy);
                    cmd.Parameters.AddWithValue("@keyn", userStruct.keyn);
                    count++;
                }
                else
                {
                   ;
                    MySqlCommand uCmd = mSql.CreateCommand();
                    uCmd.CommandText = "Select * from user WHERE ";
                    if (userStruct.s_name != "" && userStruct.f_name != "") //TO DO Ak je zly vyber
                    {
                        uCmd.CommandText += "first_name = @fname AND second_name = @sname";
                        uCmd.Parameters.AddWithValue("@fname", userStruct.f_name);
                        uCmd.Parameters.AddWithValue("@sname", userStruct.s_name);
                    }
                    else if (userStruct.s_name != "")
                    {
                        uCmd.CommandText += "second_name = @sname";
                        uCmd.Parameters.AddWithValue("@sname", userStruct.s_name);
                    }
                    else
                    {
                        uCmd.CommandText += "first_name = @fname";
                        uCmd.Parameters.AddWithValue("@fname", userStruct.f_name);
                    }

                    MySqlDataAdapter myadapter = new MySqlDataAdapter(uCmd);
                    DataSet userD = new DataSet();
                    myadapter.Fill(userD);
                    List<string> validIds = new List<string>();
                    string id;

                    foreach (DataRow row in userD.Tables[0].Rows)
                    {
                        if (userStruct.s_name != "" && userStruct.f_name != "")
                        {
                            if (row["first_name"].ToString() == userStruct.f_name && row["second_name"].ToString() == userStruct.s_name)
                            {
                                id = row["year"] + "-" + row["_numbers"];
                                if (!validIds.Contains(id)) validIds.Add(id);
                            }
                        }
                        else if (userStruct.s_name != "")
                        {
                            if (row["second_name"].ToString() == userStruct.s_name)
                            {
                                id = row["year"] + "-" + row["_numbers"];
                                if (!validIds.Contains(id)) validIds.Add(id);
                            }
                        }
                        else
                        {
                            if (row["first_name"].ToString() == userStruct.f_name)
                            {
                                id = row["year"] + "-" + row["_numbers"];
                                if (!validIds.Contains(id)) validIds.Add(id);
                            }
                        }
                    }

                    if (validIds.Count() == 0)
                    {
                        validIds.Add(userStruct.f_name + "-" + userStruct.s_name);
                    }
                         

                    cmd.CommandText = "Select * from item WHERE ";
                    count = 0;
                    foreach (string str in validIds)
                    {
                        if (count != 0)
                        {
                            cmd.CommandText += "OR ";
                        }
                        cmd.CommandText += "(user_year = @year AND user_numbers = @numbers)";
                        cmd.Parameters.AddWithValue("@year", str.Substring(0, 2));
                        cmd.Parameters.AddWithValue("@numbers", str.Substring(3, 3));
                        count++;
                    }
                }

                if (count != 0)
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    data = new DataSet();
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


        public void AddItem(EditItemStruct itemStruct)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "INSERT INTO item(user_year, user_numbers, name, description, size, price, photo, created_at) VALUES (@user_year, @user_numbers, @name, @description, @size, @price, @photo, @created_at)";
                cmd.Parameters.AddWithValue("@user_year", itemStruct.keyy);
                cmd.Parameters.AddWithValue("@user_numbers", itemStruct.keyn);
                cmd.Parameters.AddWithValue("@description", itemStruct.description);
                cmd.Parameters.AddWithValue("@size", itemStruct.size);
                cmd.Parameters.AddWithValue("@price", itemStruct.price);
                cmd.Parameters.AddWithValue("@name", itemStruct.name);
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

        public void UpdateItem(EditItemStruct itemStruct)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "UPDATE item SET description = @desc, size = @size, price = @price, name = @name";
                cmd.Parameters.AddWithValue("@desc", itemStruct.description);
                cmd.Parameters.AddWithValue("@size", itemStruct.size);
                cmd.Parameters.AddWithValue("@price", itemStruct.price);
                cmd.Parameters.AddWithValue("@name", itemStruct.name);
               /* if (itemStruct.created_at.ToString() != "1/1/0001 12:00:00 AM")
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
                } */
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

        public DataSet LoadSpecificItem(string key)
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


        public DataSet SearchForItemsByName(string name)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM item WHERE name = @name";
                cmd.Parameters.AddWithValue("@name", name);
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

        public DataSet LoadAllLogs()
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
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

        public DataSet LoadLogByDay(DateTime day)
        {
            MySqlConnection mSql = new MySqlConnection(connMainStr);
            mSql.Open();
            DataSet data;

            try
            {
                MySqlCommand cmd = mSql.CreateCommand();
                cmd.CommandText = "SELECT * FROM log WHERE created_at = @day";
                cmd.Parameters.AddWithValue("@day", day);
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
            MySqlConnection mSql = new MySqlConnection(connMainStr);
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
