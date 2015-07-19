using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_clientWPF
{
	public class Version
	{
		private List<VersionFile> vec;
		private Int64 version = 0;
		private int newFiles=0, editFiles=0, delFiles=0;
		private string timestamp;

		public Version(Int64 ver, string timestamp)
		{
			vec = new List<VersionFile>();
			this.version = ver;
			this.timestamp = timestamp;
		}

		public void append(VersionFile vf)
		{
			switch (vf.FileOperation)
			{
				case "NEW":
					newFiles++;
					break;
				case "EDIT":
					editFiles++;
					break;
				case "DEL":
					delFiles++;
					break;
				case "NONE":
					break;
				default:
					throw new Exception("Invalid file operation");
			}
			vec.Add(vf);
		}

		public List<VersionFile> Items
		{
			get { return vec; }
		}

		public int FileCount
		{
			get { return vec.Count; }
		}
		public Int64 VersionNum
		{
			get { return this.version; }
		}
		public int NewFiles
		{
			get { return this.newFiles; }
		}
		public int EditFiles
		{
			get { return this.editFiles; }
		}
		public int DelFiles
		{
			get { return this.delFiles; }
		}
		public string Timestamp
		{
			get { return this.timestamp; }
		}
	}

	public class VersionFile
	{
		private string fileName;
		private string fileOperation; /*EDIT, NEW, DEL, NONE*/
		private string timestamp;

		public VersionFile(string fileName, string operation, string timestamp="")
		{
			this.fileOperation = operation;
			this.fileName = fileName;
			this.timestamp = timestamp;
		}

		public string FileOperation
		{
			get { return this.fileOperation; }
		}

		public string FileName
		{
			get { return this.fileName; }
		}

		public string Timestamp
		{
			get { return this.timestamp; }
		}

	}
}