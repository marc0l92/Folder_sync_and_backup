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
		private const String DEFAULT_DATABASE_FILE = "MyDatabase.sqlite";
		private String databaseFile;
		private SQLiteConnection connection;

		public SyncSQLite() : this(DEFAULT_DATABASE_FILE) { }
		public SyncSQLite(String databaseFile)
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

		private int executeQuery(String query)
		{
			SQLiteCommand command = new SQLiteCommand(query, connection);
			return command.ExecuteNonQuery();
		}

		private int executeQuery(String query, Object param1)
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

		public bool authenticateUser(String username, String password)
		{
			bool authenticated = false;
			SQLiteCommand command = new SQLiteCommand("SELECT * FROM users WHERE username = @username AND password = @password", connection);
			command.Parameters.AddWithValue("username", username);
			command.Parameters.AddWithValue("password", password);
			SQLiteDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				// there at least a row
				authenticated = true;
			}
			reader.Close();
			return authenticated;
		}

		public Int64 checkUserDirectory(String username, String directory)
		{
			SQLiteCommand command = new SQLiteCommand("SELECT directory FROM users WHERE username = @username", connection);
			command.Parameters.AddWithValue("username", username);
			SQLiteDataReader reader = command.ExecuteReader();
			if (reader.Read() && reader["client_dir"] == directory)
			{
				return (Int64)reader["id"];
			}
			else
			{
				return -1;
			}
		}

		public bool newUser(String username, String password, String directory)
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
				return false;
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
			this.executeQuery("CREATE TABLE user_" + lastId + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, version INTEGER NOT NULL, server_file TEXT NOT NULL UNIQUE, client_file TEXT NOT NULL, checksum TEXT NOT NULL);");
			return true;
		}

		public bool deleteUser(int userId)
		{
			if(this.executeQuery("DELETE FROM users WHERE id = @param1;", userId) == 1){
				this.executeQuery("DROP TABLE user_"+userId);
				return true;
			}
			return false;
		}

		public Int64 getUserLastVersion(Int64 userId)
		{
			SQLiteCommand command = new SQLiteCommand("SELECT MAX(version) AS max_version FROM user_" + userId, connection);
			SQLiteDataReader reader = command.ExecuteReader();
			if (reader.Read())
			{
				return (Int64)reader["max_version"];
			}
			else
			{
				return -1;
			}
		}

		public List<FileChecksum> getUserFiles(Int64 userId, int version)
		{
			List<FileChecksum> userFiles = new List<FileChecksum>();
			SQLiteCommand command = new SQLiteCommand("SELECT * FROM user_" + userId + " WHERE version = " + version, connection);
			SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				userFiles.Add(new FileChecksum((String)reader["server_file"], (String)reader["client_file"], (String)reader["checksum"]));
			}
			return userFiles;
		}

		public void setUserFiles(Int64 userId, int version, List<FileChecksum> fileList)
		{
			SQLiteCommand command;
			foreach (FileChecksum file in fileList)
			{
				command = new SQLiteCommand("INSERT INTO user_" + userId + " (version, server_file, client_file, checksum) VALUES (@version, @server_file, @client_file, @checksum);", connection);
				command.Parameters.AddWithValue("version", version);
				command.Parameters.AddWithValue("server_file", file.FileNameServer);
				command.Parameters.AddWithValue("client_file", file.FileNameClient);
				command.Parameters.AddWithValue("checksum", file.Checksum);
				command.ExecuteNonQuery();
			}
		}
	}
}
