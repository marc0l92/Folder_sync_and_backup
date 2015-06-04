using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_server
{
	class SyncCommand
	{
		public enum CommandSet { START, LOGIN, AUTHORIZED, UNAUTHORIZED, EDIT, DEL, NEW, FILE, GET, RESTORE, ENDSYNC, CHECK, ENDCHECK };
		private CommandSet type;
		private String directory;
		private String fileName;
		public int version;
		public String checksum;
		public String username, passwrod;
		public String fileContent;

		public SyncCommand(CommandSet type, String arg1) : this(type, new String[] { arg1 }) { }
		public SyncCommand(CommandSet type, String arg1, String arg2) : this(type, new String[] { arg1, arg2 }) { }
		public SyncCommand(CommandSet type, String[] args)
		{
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
				case CommandSet.EDIT:
					if (args.Length != 1) throw new Exception("Wrong params count");
					fileName = args[0];
					break;
				case CommandSet.DEL:
					if (args.Length != 1) throw new Exception("Wrong params count");
					fileName = args[0];
					break;
				case CommandSet.NEW:
					if (args.Length != 1) throw new Exception("Wrong params count");
					fileName = args[0];
					break;
				case CommandSet.FILE:
					if (args.Length != 1) throw new Exception("Wrong params count");
					fileContent = args[0];
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
					checksum = args[1];
					break;
				case CommandSet.ENDCHECK:
					if (args.Length != 0) throw new Exception("Wrong params count");
					break;
				default:
					throw new Exception("Command not implemented");
			}
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
				if (this.type == CommandSet.START)
					return directory;
				else
					throw new Exception("Command type not compatible");
			}
		}
		public String FileName
		{
			get
			{
				if (this.type == CommandSet.EDIT || this.type == CommandSet.DEL || this.type == CommandSet.NEW || this.type == CommandSet.GET || this.type == CommandSet.CHECK)
					return fileName;
				else
					throw new Exception("Command type not compatible");
			}
		}
		public int Version
		{
			get
			{
				if (this.type == CommandSet.RESTORE)
					return version;
				else
					throw new Exception("Command type not compatible");
			}
		}
		public String Checksum
		{
			get
			{
				if (this.type == CommandSet.CHECK)
					return checksum;
				else
					throw new Exception("Command type not compatible");
			}
		}
		public String FileContent
		{
			get
			{
				if (this.type == CommandSet.FILE)
					return fileContent;
				else
					throw new Exception("Command type not compatible");
			}
		}
		public String Username
		{
			get
			{
				if (this.type == CommandSet.LOGIN)
					return username;
				else
					throw new Exception("Command type not compatible");
			}
		}
		public String Password
		{
			get
			{
				if (this.type == CommandSet.LOGIN)
					return passwrod;
				else
					throw new Exception("Command type not compatible");
			}
		}
	}
}
