using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSProject.DAL
{
    class UserDataBaseFunc
    {
        private string OSPsql = "Server=localhost;Port=5433;User Id=postgres;Password=2020;Database=OldSchoolPGDataBase";
        public string ReturnConnectionString() {
            return OSPsql;
        }
        public void FuncCheckLogin(string login, string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    int newId = FuncCreateID(connectionString);

                    cmd.CommandText = $"INSERT INTO OSUser VALUES ({newId}, '{login}','TestPass123','SysTest', 'default', 0)";
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    FuncDeleteByID(newId, connectionString);
                }
            }
        }
        public void FuncCheckPass(string login, string pass, string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    int newId = FuncCreateID(connectionString);

                    cmd.CommandText = $"INSERT INTO OSUser VALUES ({newId}, '{login}','{pass}','SysTest', 'default', 0)";
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    FuncDeleteByID(newId, connectionString);
                }
            }
        }
        public int FuncCreateID(string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = $"SELECT MAX(id) FROM OSUser";
                    int CreateNewIdValue = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Parameters.Clear();
                    CreateNewIdValue += 1;
                    return CreateNewIdValue;
                }
            }
        }
        public void FuncDeleteByID(int i, string connectionString)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = $"DELETE FROM OSUser WHERE id={i};";
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }

            }
        }
        public void CreateRating(string connectionString, int someid)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    using (NpgsqlCommand cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        int vc = 0;
                        cmd.CommandText = $"SELECT COUNT(*) FROM OSGame";
                        cmd.ExecuteNonQuery();
                        vc = Convert.ToInt32(cmd.ExecuteScalar());
                        cmd.Parameters.Clear();
                        int[] idarray = new int[vc];
                        for (int i = 0; i < vc; i++)
                        {
                            cmd.CommandText = $"SELECT id FROM( SELECT id, ROW_NUMBER() OVER(ORDER BY id) AS RowNum FROM OSGame) AS rowsel WHERE rowsel.RowNum = {i + 1}";
                            idarray[i] = Convert.ToInt32(cmd.ExecuteScalar());
                            cmd.Parameters.Clear();

                            cmd.CommandText = $"INSERT INTO OSRating VALUES ({idarray[i]}, {someid}, 0)";
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("Error in CRFU! Some information was wrong");
                    return;
                }
            }
        }
    }
}
