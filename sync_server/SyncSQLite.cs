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
			File.Delete(this.databaseFile);
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
			this.executeQuery("CREATE TABLE users ( id int NOT NULL AUTO_INCREMENT,username VARCHAR(30) NOT NULL, password VARCHAR(30) NOT NULL, client_dir VARCHAR(80) NOT NULL, PRIMARY KEY (id))");
			//this.executeQuery("insert into highscores (name, score) values ('Me', 9001)");
		}

		public bool authenticateUser(String username, String password)
		{
			bool authenticated = false;
			SQLiteCommand command = new SQLiteCommand("SELECT * FORM users WHERE username = @username AND password = @password", connection);
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
			SQLiteCommand command = new SQLiteCommand("SELECT directory FORM users WHERE username = @username", connection);
			command.Parameters.AddWithValue("username", username);
			SQLiteDataReader reader = command.ExecuteReader();
			return false;
		}

		public bool newUser(String username, String password)
		{	

			this.executeQuery("CREATE TABLE user_@param ( id int NOT NULL AUTO_INCREMENT,username VARCHAR(30) NOT NULL, password VARCHAR(30) NOT NULL, client_dir VARCHAR(80) NOT NULL, PRIMARY KEY (id))", username);
			return false;
		}

		public bool deleteUser(String username)
		{
			return false;
		}

		public int getUserLastVersion(String username)
		{
			return 0;
		}

		public List<FileChecksum> getUserFiles(String username, int version)
		{
			return null;
		}

		public void setUserFiles(String username, int version, List<FileChecksum> fileList)
		{

		}
	}
}
