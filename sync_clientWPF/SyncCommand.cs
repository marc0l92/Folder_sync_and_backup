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
		public enum CommandSet { START, LOGIN, AUTHORIZED, UNAUTHORIZED, NEWUSER, EDIT, DEL, NEW, FILE, GET, RESTORE, ENDSYNC, CHECK, ENDCHECK, ACK, NOSYNC};
		private CommandSet type;
		private String directory;
		private String fileName;
		private Int64 version, fileSize;
		private byte[] checksum;
		private String username, passwrod;

		public SyncCommand(CommandSet type) : this(type, new String[]{}) {}
		public SyncCommand(CommandSet type, String arg1) : this(type, new String[]{arg1}) {}
		public SyncCommand(CommandSet type, String arg1, String arg2) : this(type, new String[] { arg1, arg2 }) { }
		public SyncCommand(CommandSet type, String arg1, String arg2, String arg3) : this(type, new String[] { arg1, arg2, arg3 }) { }
		public SyncCommand(CommandSet type, String[] args){
			this.type = type;
			switch (type)
			{
				case CommandSet.START:
					if (args.Length != 1) throw new Exception("Wrong params count");
					directory = args[0];
					break;
				case CommandSet.LOGIN:
					if (args.Length != 2) throw new Exception("Wrong params count");
					username = args[0];
					passwrod = args[1];
					break;
				case CommandSet.AUTHORIZED:
					if (args.Length != 0) throw new Exception("Wrong params count");
					break;
				case CommandSet.UNAUTHORIZED:
					if (args.Length != 0) throw new Exception("Wrong params count");
					break;
				case CommandSet.NEWUSER:
					if (args.Length != 3) throw new Exception("Wrong params count");
					username = args[0];
					passwrod = args[1];
					directory = args[2];
					break;
				case CommandSet.EDIT:
					if (args.Length != 2) throw new Exception("Wrong params count");
					fileName = args[0];
					fileSize = Int64.Parse(args[1]);
					break;
				case CommandSet.DEL:
					if (args.Length != 1) throw new Exception("Wrong params count");
					fileName = args[0];
					break;
				case CommandSet.NEW:
					if (args.Length != 2) throw new Exception("Wrong params count");
					fileName = args[0];
					fileSize = Int64.Parse(args[1]);
					break;
				case CommandSet.FILE:
					if (args.Length != 1) throw new Exception("Wrong params count");
					fileName = args[0];
					break;
				case CommandSet.GET:
					if (args.Length != 1) throw new Exception("Wrong params count");
					fileName = args[0];
					break;
				case CommandSet.RESTORE:
					if (args.Length != 1) throw new Exception("Wrong params count");
					version = Convert.ToInt32(args[0]);
					break;
				case CommandSet.ENDSYNC:
					if (args.Length != 0) throw new Exception("Wrong params count");
					break;
				case CommandSet.CHECK:
					if (args.Length != 2) throw new Exception("Wrong params count");
					fileName = args[0];
					checksum = System.Text.Encoding.ASCII.GetBytes(args[1]);
					break;
				case CommandSet.ENDCHECK:
					if (args.Length != 0) throw new Exception("Wrong params count");
					break;
				case CommandSet.ACK:
					if (args.Length != 0) throw new Exception("Wrong params count");
					break;
				case CommandSet.NOSYNC:
					if (args.Length != 0) throw new Exception("Wrong params count");
					break;
				default:
					throw new Exception("Command not implemented");
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
		[JsonConstructor]
		public SyncCommand(CommandSet Type, String Directory, String FileName, Int64 Version, Int64 FileSize, byte[] Checksum, String Username, String Password)
		{
			this.type = Type;
			this.directory = Directory;
			this.fileName = FileName;
			this.version = Version;
			this.fileSize = FileSize;
			this.checksum = Checksum;
			this.username = Username;
			this.passwrod = Password;
		}
		public static int searchJsonEnd(String jsonText)
		{
			// TODO struttura debole
			bool quotes = false;
			for (int i = 0; i < jsonText.Length; i++)
			{
				if (jsonText[i] == '"' && jsonText[i-1] != '\\')
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
			get {
				if (this.type == CommandSet.START || this.type == CommandSet.NEWUSER)
					return directory;
				else
					return null;
			}
		}
		public String FileName
		{
			get
			{
				if (this.type == CommandSet.EDIT || this.type == CommandSet.DEL || this.type == CommandSet.NEW || this.type == CommandSet.GET || this.type == CommandSet.CHECK)
					return fileName;
				else
					return null;
			}
		}
		public Int64 Version
		{
			get
			{
				if (this.type == CommandSet.RESTORE)
					return version;
				else
					return -1;
			}
		}
		public byte[] Checksum
		{
			get
			{
				if (this.type == CommandSet.CHECK)
					return checksum;
				else
					return null;
			}
		}
		public String Username
		{
			get
			{
				if (this.type == CommandSet.LOGIN || this.type == CommandSet.NEWUSER)
					return username;
				else
					return null;
			}
		}
		public String Password
		{
			get
			{
				if (this.type == CommandSet.LOGIN || this.type == CommandSet.NEWUSER)
					return passwrod;
				else
					return null;
			}
		}

		public Int64 FileSize
		{
			get
			{
				if (this.type == CommandSet.NEW || this.type == CommandSet.EDIT)
					return fileSize;
				else
					return -1;
			}
		}

	}
}
