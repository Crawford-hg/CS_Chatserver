using Microsoft.Data.Sqlite;
using System.Security.Cryptography;
namespace ChatServer
{
    public class dataHandler
    {
        SqliteConnection connection;
        public dataHandler() {
            string connectionString = "Filename=userdb.db";
            this.connection = new SqliteConnection(connectionString);
            EnsureTableExists();
            save();
        }

        public void insert(string user, string pass)
        {

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                // Handle the case where user or pass is empty
                Console.WriteLine("User or password is empty.");
                return;
            }

            //EnsureTableExists();

            string insertQuery = "INSERT INTO USERDATA (uname, pass) VALUES (@Value1, @Value2)";


            SqliteCommand command = new SqliteCommand(insertQuery, this.connection);
            command.Parameters.AddWithValue("@Value1", user);
            command.Parameters.AddWithValue("@Value2", _hash_string(pass));

            connection.Open();
            int rows = command.ExecuteNonQuery();

            // Close the connection after executing the query
            connection.Close();
            save();
        }


        public void select(string user, string pass) {
            string selectQuery = "SELECT * FROM USERDATA WHERE uname = @Value1 AND pass = @Value2";

            SqliteCommand command = new SqliteCommand(selectQuery, this.connection);
            command.Parameters.AddWithValue("@Value1", user);
            command.Parameters.AddWithValue("@Value2", _hash_string(pass));



            connection.Open();

            

            if (command.ExecuteReader().Read())
            {
                return;
            }
            else {
                throw new ArgumentException("Username or password is incorrect");
            }

        }

        public void EnsureTableExists()
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='USERDATA'";
                var result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                {
                    // The table does not exist, so create it
                    CreateTable();
                }
            }
            connection.Close();
        }


        private void CreateTable()
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "CREATE TABLE USERDATA (uname TEXT NOT NULL PRIMARY KEY, pass TEXT NOT NULL)";
                command.ExecuteNonQuery();
            }
            connection.Close();
        }


        private void save() {
            connection.Open();
            SqliteTransaction transaction = connection.BeginTransaction();
            try
            {
                transaction.Commit();
            }
            catch(Exception e) {
                transaction.Rollback();
                Console.WriteLine("Error on db save " + e);
            }
            connection.Close();
        }


        private string _hash_string(string str) {

            MD5 hash = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
            byte[] hashBytes = hash.ComputeHash(inputBytes);




            return Convert.ToHexString(hashBytes);

        }


    }


}
