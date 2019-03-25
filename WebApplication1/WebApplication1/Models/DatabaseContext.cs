using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WebApplication1.Models
{
    public class DatabaseContext
    {
        public string ConnectionString { get; set; }
        public DatabaseContext(string connectionString)
        {
            this.ConnectionString = connectionString; //does it need this?
        }
        
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List <ItemsTable> GetAllItemsData()
        {
            List<ItemsTable> data = new List<ItemsTable>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from polozka", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new ItemsTable()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            nazov = reader["nazov"].ToString(),
                            velkost = Convert.ToDouble(reader["velkost"]),
                            photo = reader["foto"].ToString(),
                            stav = Convert.ToBoolean(reader["stav"]),
                            prichod = reader["prichod"].ToString(),
                            odchod = reader["odchod"].ToString(),
                            cena = Convert.ToDouble(reader["cena"])
                        });
                    }
                }
            }

            return data;
        }

        public string GetImagePath(int id)
        {
            string res = "";

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "Select * from polozka Where id = @id";
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id));
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res = reader["foto"].ToString();
                    }
                }
            }

            return res;
        }

        public List <TovarTable> GetAllTovarData()
        {
            List<TovarTable> data = new List<TovarTable>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from tovar", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new TovarTable()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            nazov = reader["nazov"].ToString(),
                            velkost = Convert.ToDouble(reader["velkost"]),
                            cena = Convert.ToDouble(reader["cena"]),
                            foto = reader["foto"].ToString()
                        });
                    }
                }
            }

            return data;
        }

        public List <OsobyTable> GetAllOsobyData()
        {
            List<OsobyTable> data = new List<OsobyTable>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from osoba_zak", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(new OsobyTable()
                        {
                            id = Convert.ToInt32(reader["id"]),
                            firstName = reader["prve_meno"].ToString(),
                            secondName = reader["druhe_meno"].ToString(),
                            phoneNumber = Convert.ToInt32(reader["telefonne_cislo"]),
                            address = reader["adresa"].ToString()
                        });
                    }
                }
            }

            return data;
        }

        
    }
}
