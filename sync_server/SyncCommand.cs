using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace sync_clientWPF
{
	class SyncCommand
	{
		public enum CommandSet { NONE, START, LOGIN, AUTHORIZED, UNAUTHORIZED, NEWUSER, EDIT, DEL, NEW, FILE, GET, RESTORE, ENDSYNC, CHECK, ENDCHECK, ACK, NOSYNC, VERSION, CHECKVERSION, GETVERSIONS };
		/*
			 		TYPE	|  data[0]  |  data[1]  |  data[2]  |  data[3]  |
			----------------+-----------+-----------+-----------+-----------+
			START			| directory |			|			|			|
			LOGIN			| username  | password  |			|			|
			AUTHORIZED		|			|			|			|			|
			UNAUTHORIZED	|			|			|			|			|
			NEWUSER			| username  | password  | directory |			|
			EDIT			| filename  | filesize  |			|			|
			DEL				| filename  |			|			|			|
			NEW				| filename  | filesize  |			|			|
			FILE			| filename  |			|			|			|
			GET				| filename  |			|			|			|
			RESTORE			| version   |			|			|			|
			ENDSYNC			|			|			|			|			|
			CHECK			| filename  | checksum  |			|			|
			ENDCHECK		|			|			|			|			|
			ACK				|			|			|			|			|
			NOSYNC			|			|			|			|			|
		    VERSION			| version   | numFiles  | timestamp |			|
		   	CHECKVERSION	| filename  | operation |			|			|
		 
		 */

		private CommandSet type;
		private string[] data = new string[4];

		public SyncCommand(CommandSet type) : this(type, new string[] { }) { }
		public SyncCommand(CommandSet type, string arg1) : this(type, new string[] { arg1 }) { }
		public SyncCommand(CommandSet type, string arg1, string arg2) : this(type, new string[] { arg1, arg2 }) { }
		public SyncCommand(CommandSet type, string arg1, string arg2, string arg3) : this(type, new string[] { arg1, arg2, arg3 }) { }
		public SyncCommand(CommandSet type, string arg1, string arg2, string arg3, string arg4) : this(type, new string[] { arg1, arg2, arg3, arg4 }) { }
		[JsonConstructor]
		public SyncCommand(CommandSet type, string[] args)
		{
			this.type = type;
			int i = 0;
			foreach (string s in args)
			{
				this.data[i++] = s;
			}
		}

		[JsonConstructor]
		public SyncCommand(CommandSet Type, string Directory, string FileName, Int64 Version, byte[] Checksum, string Username, string Password, Int64 FileSize, string Operation, Int64 NumFiles, string Timestamp)
		{
			type = Type;
			switch (Type)
			{
				case CommandSet.START:
					data[0] = Directory;
					break;
				case CommandSet.LOGIN:
					data[0] = Username;
					data[1] = Password;
					break;
				case CommandSet.NEWUSER:
					data[0] = Username;
					data[1] = Password;
					data[2] = Directory;
					break;
				case CommandSet.EDIT:
					data[0] = FileName;
					data[1] = FileSize.ToString();
					break;
				case CommandSet.DEL:
					data[0] = FileName;
					break;
				case CommandSet.NEW:
					data[0] = Directory;
					data[1] = FileSize.ToString();
					break;
				case CommandSet.FILE:
					data[0] = FileName;
					break;
				case CommandSet.GET:
					data[0] = FileName;
					break;
				case CommandSet.RESTORE:
					data[0] = Version.ToString();
					break;
				case CommandSet.CHECK:
					data[0] = FileName;
					data[1] = System.Text.Encoding.ASCII.GetString(Checksum);
					break;
				case CommandSet.VERSION:
					data[0] = Version.ToString();
					data[1] = NumFiles.ToString();
					data[2] = Timestamp;
					break;
				case CommandSet.CHECKVERSION:
					data[0] = FileName;
					data[1] = Operation;
					break;
			}
		}
		public String convertToString()
		{
			return JsonConvert.SerializeObject(this);
		}
		public static SyncCommand convertFromString(String jsonString)
		{
			return JsonConvert.DeserializeObject<SyncCommand>(jsonString);
		}
		public static int searchJsonEnd(String jsonText)
		{
			// TODO struttura debole
			bool quotes = false;
			for (int i = 0; i < jsonText.Length; i++)
			{
				if (jsonText[i] == '"' && jsonText[i - 1] != '\\')
				{
					quotes = !quotes;
				}
				else
				{
					if (jsonText[i] == '}' && quotes == false)
					{
						return i;
					}
				}
			}

			return -1;
		}

		// Property definition
		public CommandSet Type
		{
			get { return type; }
		}
		public String Directory
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.START:
						return data[0];
					case CommandSet.NEWUSER:
						return data[2];
					default:
						return null;
				}
			}
		}
		public String FileName
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.EDIT:
						return data[0];
					case CommandSet.DEL:
						return data[0];
					case CommandSet.NEW:
						return data[0];
					case CommandSet.FILE:
						return data[0];
					case CommandSet.GET:
						return data[0];
					case CommandSet.CHECK:
						return data[0];
					case CommandSet.CHECKVERSION:
						return data[0];
					default:
						return null;
				}
			}
		}
		public Int64 Version
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.RESTORE:
						return Int64.Parse(data[0]);
					case CommandSet.VERSION:
						return Int64.Parse(data[0]);
					default:
						return -1;
				}
			}
		}
		public byte[] Checksum
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.RESTORE:
						return System.Text.Encoding.ASCII.GetBytes(data[1]);
					default:
						return null;
				}
			}
		}
		public String Username
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.LOGIN:
						return data[0];
					case CommandSet.NEWUSER:
						return data[0];
					default:
						return null;
				}
			}
		}
		public String Password
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.LOGIN:
						return data[1];
					case CommandSet.NEWUSER:
						return data[1];
					default:
						return null;
				}
			}
		}

		public Int64 FileSize
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.EDIT:
						return Int64.Parse(data[1]);
					case CommandSet.NEW:
						return Int64.Parse(data[1]);
					default:
						return -1;
				}
			}
		}

		public CommandSet Operation
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.CHECKVERSION:
						return (CommandSet)Int64.Parse(data[1]);
					default:
						return CommandSet.NONE;
				}
			}
		}
		public Int64 NumFiles
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.VERSION:
						return Int64.Parse(data[1]);
					default:
						return -1;
				}
			}
		}
		public string Timestamp
		{
			get
			{
				switch (this.type)
				{
					case CommandSet.VERSION:
						return data[2];
					default:
						return null;
				}
			}
		}

	}
}
