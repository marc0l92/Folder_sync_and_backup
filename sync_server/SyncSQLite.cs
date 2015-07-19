using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
    public class SyncSQLite
    {
        public class UserVersions
        {
            public Int64 userId{get; set;}
            public string username { get; set; }
            public Int64 versionCount { get; set; }
        }

        private const string DEFAULT_DATABASE_FILE = "MyDatabase.sqlite";
        private string databaseFile;
        private SQLiteConnection connection;

        public SyncSQLite() : this(DEFAULT_DATABASE_FILE) { }
        public SyncSQLite(string databaseFile)
        {
            bool newDatabase = false;
            this.databaseFile = databaseFile;
            if (!File.Exists(this.databaseFile))
            {
                SQLiteConnection.CreateFile(this.databaseFile);
                newDatabase = true;
            }
            connection = new SQLiteConnection("Data Source=" + this.databaseFile + ";Version=3;");
            connection.Open();

            if (newDatabase)
            {
                this.initDatabaseStructure();
            }
        }

        public void closeConnection()
        {
            connection.Close();
            connection.Dispose();
        }
        public void destroyDatabase()
        {
            if (File.Exists(this.databaseFile))
            {
                connection.Close();
                try
                {
                    File.Delete(this.databaseFile);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
        }

        private int executeQuery(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, connection);
            return command.ExecuteNonQuery();
        }

        private int executeQuery(string query, Object param1)
        {
            // Example: "SELECT something FROM tabletop WHERE color = @param1"
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("param1", param1);
            return command.ExecuteNonQuery();
        }

        private void initDatabaseStructure()
        {
            this.executeQuery("CREATE TABLE users (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, username TEXT NOT NULL UNIQUE, password TEXT NOT NULL, user_dir TEXT NOT NULL);");
        }

        public Int64 authenticateUser(string username, string password)
        {
            Int64 userId = -1;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM users WHERE username = @username AND password = @password", connection);
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                // there at least a row
                userId = (Int64)reader["id"];
            }
            reader.Close();
            return userId;
        }

        public Int64 checkUserDirectory(string username, string directory)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM users WHERE username = @username", connection);
            command.Parameters.AddWithValue("username", username);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string db_dir = (string)reader["user_dir"];
                if (directory == db_dir)
                {
                    return (Int64)reader["id"];
                }

            }
            return -1;
        }

        public Int64 newUser(string username, string password, string directory)
        {
            // test if there is an user with the same username
            bool usernameAlereadyUsed;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM users WHERE username = @username", connection);
            command.Parameters.AddWithValue("username", username);
            SQLiteDataReader reader = command.ExecuteReader();
            usernameAlereadyUsed = reader.Read();
            reader.Close();
            if (usernameAlereadyUsed)
            {
                return -1;
            }
            // create a new user
            command = new SQLiteCommand("INSERT INTO users (username, password, user_dir) VALUES (@username, @password, @directory)", connection);
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);
            command.Parameters.AddWithValue("directory", directory);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("select last_insert_rowid()", connection);
            Int64 lastId = (Int64)command.ExecuteScalar();

            // todo create a table with the right name
            this.executeQuery("CREATE TABLE user_" + lastId + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, version INTEGER NOT NULL, server_file TEXT NOT NULL, client_file TEXT NOT NULL, checksum BLOB NOT NULL, timestamp TEXT NOT NULL);");
            return lastId;
        }

        public bool deleteUser(Int64 userId)
        {
            if (this.executeQuery("DELETE FROM users WHERE id = @param1;", userId) == 1)
            {
                this.executeQuery("DROP TABLE user_" + userId);
                return true;
            }
            return false;
        }

        public Int64 getUserMinMaxVersion(Int64 userId, ref Int64 maxVersion)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT IFNULL(MAX(version), 0) AS max_version, IFNULL(MIN(version), 0) AS min_version FROM user_" + userId, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                maxVersion = (Int64)reader["max_version"];
                return (Int64)reader["min_version"];
            }
            else
            {
                maxVersion = -1;
                return -1;
            }
        }

        public List<FileChecksum> getUserFiles(Int64 userId, Int64 version, string serverBaseDir)
        {
            List<FileChecksum> userFiles = new List<FileChecksum>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM user_" + userId + " WHERE version = @version;", connection);
            command.Parameters.AddWithValue("version", version);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                userFiles.Add(new FileChecksum(serverBaseDir + (string)reader["server_file"], (string)reader["server_file"], (string)reader["client_file"], (byte[])reader["checksum"], version, (string)reader["timestamp"]));
            }
            reader.Close();
            return userFiles;
        }

        public void setUserFiles(Int64 userId, Int64 version, List<FileChecksum> fileList)
        {
            SQLiteCommand command;
            string current_timestamp = string.Format("{0:dd-MM-yyyy h-mm-ss-tt}", DateTime.Now);
            foreach (FileChecksum file in fileList)
            {
                command = new SQLiteCommand("INSERT INTO user_" + userId + " (version, server_file, client_file, checksum, timestamp) VALUES (@version, @server_file, @client_file, @checksum, @timestamp);", connection);
                command.Parameters.AddWithValue("version", version);
                command.Parameters.AddWithValue("server_file", file.FileNameServerDB);
                command.Parameters.AddWithValue("client_file", file.FileNameClient);
                command.Parameters.AddWithValue("checksum", file.ChecksumBytes);
                command.Parameters.AddWithValue("timestamp", current_timestamp);
                command.ExecuteNonQuery();
            }

        }

        public List<UserVersions> getUsersList()
        {
            UserVersions item;
            List<UserVersions> userList = new List<UserVersions>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM users", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                item = new UserVersions();
                item.username = (string)reader["username"];
                item.userId = (Int64)reader["id"];
                userList.Add(item);
            }
            reader.Close();
            for (int i =0; i<userList.Count;i++ )
            {
                command = new SQLiteCommand("SELECT IFNULL(COUNT( DISTINCT version) , 0) AS count FROM user_" + userList[i].userId , connection);
                reader = command.ExecuteReader();
                if (reader.Read())
                    userList[i].versionCount = (Int64) reader["count"];
                else
                    userList[i].versionCount = 0;

                reader.Close();
            }
            return userList;
        }

        public List<FileChecksum> getFileVersions(Int64 userId, string filename, string serverBaseDir)
        {
            List<FileChecksum> userFiles = new List<FileChecksum>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM user_" + userId + " WHERE client_file = @filename;", connection);
            command.Parameters.AddWithValue("filename", filename);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                userFiles.Add(new FileChecksum(serverBaseDir + (string)reader["server_file"], (string)reader["server_file"], (string)reader["client_file"], (byte[])reader["checksum"], (Int64)reader["version"], (string)reader["timestamp"]));
            }
            reader.Close();
            return userFiles;
        }

        public void deleteVersion(Int64 userId, Int64 version)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM user_" + userId + " WHERE version = @version;", connection);
            command.Parameters.AddWithValue("version", version);
            command.ExecuteNonQuery();
        }

        public FileChecksum getFileChecksum(Int64 userId, string filename, Int64 version, string serverBaseDir)
        {
            FileChecksum fileChecksum = null;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM user_" + userId + " WHERE client_file = @filename", connection);
            command.Parameters.AddWithValue("filename", filename);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                fileChecksum = new FileChecksum(serverBaseDir + (string)reader["server_file"], (string)reader["server_file"], (string)reader["client_file"], (byte[])reader["checksum"], (Int64)reader["version"], (string)reader["timestamp"]);
                reader.Close();
            }
            return fileChecksum;
        }
    }
}
