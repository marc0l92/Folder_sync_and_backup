using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
	class SyncSQLite
	{
		private const String defaultDatabaseFile = "MyDatabase.sqlite";
		private String databaseFile;
		private SQLiteConnection connection;
		private SQLiteCommand command;
		private String query;

		public SyncSQLite() : this(defaultDatabaseFile) { }
		public SyncSQLite(String databaseFile)
		{
			bool newDatabase = false;
			this.databaseFile = databaseFile;
			if (!File.Exists(this.databaseFile))
			{
				SQLiteConnection.CreateFile(this.databaseFile);
				newDatabase = true;
			}
			connection = new SQLiteConnection("Data Source="+this.databaseFile+";Version=3;");
			connection.Open();

			if(newDatabase){
				this.initDatabaseStructure();
			}
		}

		public void destroyDatabase()
		{
			File.Delete(this.databaseFile);
		}

		private void initDatabaseStructure(){
			query = "CREATE TABLE highscores (name VARCHAR(20), score INT)";
			command = new SQLiteCommand(query, connection);
			command.ExecuteNonQuery();
			query = "insert into highscores (name, score) values ('Me', 9001)";
			command = new SQLiteCommand(query, connection);
			command.ExecuteNonQuery();
			query = "insert into highscores (name, score) values ('Myself', 6000)";
			command = new SQLiteCommand(query, connection);
			command.ExecuteNonQuery();
			query = "insert into highscores (name, score) values ('And I', 9001)";
			command = new SQLiteCommand(query, connection);
			command.ExecuteNonQuery();
		}

		public bool authenticateUser(String username, String password)
		{
			query = "select * from highscores order by score desc";
			command = new SQLiteCommand(query, connection);
			SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
				Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
			return false;
		}

		public bool checkUserDirectory(String username, String directory)
		{
			return false;
		}

		public bool createUser(String username, String password)
		{
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
