using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLTest
{
	class Program
	{
		static SQLiteConnection m_dbConnection;
		static SQLiteCommand command;
		static String sql;

		static void Main(string[] args)
		{
			//Install-Package Community.CsharpSqlite.SQLiteClient
			SQLiteConnection.CreateFile("MyDatabase.sqlite");
			m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
			m_dbConnection.Open();

			sql = "CREATE TABLE highscores (name VARCHAR(20), score INT)";
			command = new SQLiteCommand(sql, m_dbConnection);
			command.ExecuteNonQuery();
			sql = "insert into highscores (name, score) values ('Me', 9001)";
			command = new SQLiteCommand(sql, m_dbConnection);
			command.ExecuteNonQuery();
			sql = "insert into highscores (name, score) values ('Myself', 6000)";
			command = new SQLiteCommand(sql, m_dbConnection);
			command.ExecuteNonQuery();
			sql = "insert into highscores (name, score) values ('And I', 9001)";
			command = new SQLiteCommand(sql, m_dbConnection);
			command.ExecuteNonQuery();

			sql = "select * from highscores order by score desc";
			command = new SQLiteCommand(sql, m_dbConnection);
			SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
				Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);

			Console.ReadLine();
		}
	}
}
