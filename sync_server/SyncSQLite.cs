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
				File.Delete(this.databaseFile);
			}
		}

		private int executeQuery(String query)
		{
			SQLiteCommand command = new SQLiteCommand(query, connection);
			return command.ExecuteNonQuery();
		}
		private int executeQuery(String query, String param1)
		{
			// Example: "SELECT something FROM tabletop WHERE color = @param1"
			SQLiteCommand command = new SQLiteCommand(query, connection);
			command.Parameters.AddWithValue("param1", param1);
			return command.ExecuteNonQuery();
		}

		private void initDatabaseStructure()
		{
			this.executeQuery("CREATE TABLE users (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, username TEXT NOT NULL UNIQUE, password TEXT NOT NULL, user_dir TEXT NOT NULL);");
			//this.executeQuery("INSERT INTO users (username, password, client_dir) VALUES ('admin', 'admin', '/')");
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

		public bool checkUserDirectory(String username, String directory)
		{
			SQLiteCommand command = new SQLiteCommand("SELECT directory FROM users WHERE username = @username", connection);
			command.Parameters.AddWithValue("username", username);
			SQLiteDataReader reader = command.ExecuteReader();
			return (reader.Read() && reader["client_dir"] == directory);
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
			// todo create a table with the right name
			this.executeQuery("CREATE TABLE user_1 (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, version INTEGER NOT NULL, server_file TEXT NOT NULL UNIQUE, client_file TEXT NOT NULL, checksum TEXT NOT NULL);");
			return true;
		}

		public bool deleteUser(String username)
		{
			return (this.executeQuery("DELETE FROM users WHERE username = @param1;", username) == 1);
		}

		public int getUserLastVersion(String username)
		{
			// todo write the function
			return 0;
		}

		public List<FileChecksum> getUserFiles(String username, int version)
		{
			// todo write the function
			return null;
		}

		public void setUserFiles(String username, int version, List<FileChecksum> fileList)
		{
			// todo write the function
		}
	}
}
