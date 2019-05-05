using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace DatabaseProj
{
    public class DatabaseContext
    {
        public DatabaseContext()
        {
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(Settings.ConnectionString);
        }

        public string GetImageName(int id)
        {
            string res = "";
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Select * from item where id = @id";
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res = reader["photo"].ToString();
                    }
                }
            }

            return res;
        }

        public List <PrehladTable> GetPrehladTable()
        {
            List<PrehladTable> retData = new List<PrehladTable>();
            DataSet userData;
            DataSet itemData;
            MySqlDataAdapter adapter;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from user", conn);
                adapter = new MySqlDataAdapter(cmd);
                userData = new DataSet();
                adapter.Fill(userData);

                cmd = new MySqlCommand("Select * from item", conn);
                adapter = new MySqlDataAdapter(cmd);
                itemData = new DataSet();
                adapter.Fill(itemData);
            }

            itemData.Tables[0].Columns.Add("rok-id", typeof(string));
            itemData.Tables[0].Columns.Add("Prve meno", typeof(string));
            itemData.Tables[0].Columns.Add("Druhe meno", typeof(string));

            for (int i = itemData.Tables[0].Rows.Count -1; i >= 0; i--)
            {
                DataRow row = itemData.Tables[0].Rows[i];
                row["rok-id"] = row["user_year"] + "-" + row["user_numbers"];
                foreach(DataRow uRow in userData.Tables[0].Rows)
                {
                    if (row["user_year"].ToString() == uRow["year"].ToString())
                    {
                        if (row["user_numbers"].ToString() == uRow["_numbers"].ToString())
                        {
                            row["Prve meno"] = uRow["first_name"].ToString();
                            row["Druhe meno"] = uRow["second_name"].ToString();
                            break;
                        }
                    }
                }
                if (row["Prve meno"].ToString() == "" || row["Archived"].ToString() == "True")
                {
                    row.Delete();
                }
            }
            itemData.AcceptChanges();

            string stav;
            foreach (DataRow row in itemData.Tables[0].Rows)
            {
                if (row["stav"].ToString() == "0")
                {
                    stav = "Skladom";
                }
                else if (row["stav"].ToString() == "1")
                {
                    stav = "Predane karta";
                }
                else if (row["stav"].ToString() == "2")
                {
                    stav = "Predane hotovost";
                }
                else if (row["stav"].ToString() == "3")
                {
                    stav = "Vratene";
                }
                else if (row["stav"].ToString() == "4")
                {
                    stav = "Zaplatene karta";
                }
                else 
                {
                    stav = "Zaplatene hotovost";
                }

                retData.Add(new PrehladTable()
                {
                    id_item = row["id"].ToString(),
                    id_user = row["rok-id"].ToString(),
                    f_name = row["Prve meno"].ToString(),
                    s_name = row["Druhe meno"].ToString(),
                    item_name = row["name"].ToString(),
                    price = row["price"].ToString(),
                    size = row["size"].ToString(),
                    state = stav,
                    photo = row["photo"].ToString()
                });
            }

            return retData;
           
        }
    }
}
