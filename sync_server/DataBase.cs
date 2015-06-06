using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using SQLite;
using System.Threading.Tasks;

namespace sync_server
{
    public class DataBase
    {
        public class FileRow
        {
            [SQLite.PrimaryKey, SQLite.AutoIncrement]
            public int Id { get; set; }
            public string client_path { get; set; }
            public string server_path { get; set; }
            public int version { get; set; }
            public string checksum { get; set; }
        }

        public class User
        {
            [SQLite.PrimaryKey, SQLite.AutoIncrement]
            public int Id { get; set; }
            public string client_dir { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public int version { get; set; }
        }

        private async void CreateTableFileUser(String username)
        {
            String tableName = "usr:" + username;
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(tableName);
            await conn.CreateTableAsync<FileRow>();
        }

        private async void CreateTableUser()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("users");
            await conn.CreateTableAsync<User>();
        }

        public async void InsertFile(String client_path, String server_path, int version, String checksum, String username)
        {
            String tableName = "usr:" + username;
            FileRow file = new FileRow();
            file.checksum = checksum;
            file.client_path = client_path;
            file.server_path = server_path;
            file.version = version;
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(tableName);
            await conn.InsertAsync(file);
        }
/*
        public async Task<Post> GetPost(int id)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("blog");

            var query = conn.Table<Post>().Where(x => x.Id == id);
            var result = await query.ToListAsync();

            return result.FirstOrDefault();
        }
        */
        public async Task<List<FileRow>> GetFiles(String username)
        {

            String tableName = "usr:" + username;
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(tableName);
            var query = conn.Table<FileRow>();
            var result = await query.ToListAsync();

            return result;
        }

        public async void UpdatePost(String client_path, String server_path, int version, String checksum, String username)
        {
            String tableName = "usr:" + username;
            FileRow file = new FileRow();
            file.checksum = checksum;
            file.client_path = client_path;
            file.server_path = server_path;
            file.version = version;
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(tableName);

            var query = conn.Table<FileRow>().Where(x => x.server_path == server_path, x => x.server_path == server_path);
            var result = await query.ToListAsync();
            await conn.UpdateAsync(file);
        }

    }
}