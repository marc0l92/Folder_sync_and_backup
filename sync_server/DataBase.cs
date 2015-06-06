using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using SQLite;

namespace sync_server
{
    public class DataBase
    {
        public class File
        {
            [PrimaryKey]
            public int Id { get; set; }
            public string client_path { get; set; }
            public string server_path { get; set; }
            public int version { get; set; }
            public string checksum { get; set; }
        }




        private async void CreateTable()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("userfile");
            await conn.CreateTableAsync<file>();
        }

    }
}