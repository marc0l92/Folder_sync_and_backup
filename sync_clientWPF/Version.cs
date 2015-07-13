using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sync_clientWPF
{
	class Version
	{
		private List<VersionFile> vec;
		private Int64 version = 0;
		private int newFiles=0, editFiles=0, delFiles=0;

		public Version(Int64 ver)
		{
			vec = new List<VersionFile>();
			this.version = ver;
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

		public VersionFile get(int i)
		{
			return vec[i];
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
	}

	class VersionFile
	{
		private String fileNameClient;
		private String fileOperation; /*EDIT, NEW, DEL*/

		public VersionFile(String fileClient, String operation)
		{
			this.fileOperation = operation;
			this.fileNameClient = fileClient;
		}

		public String FileOperation
		{
			get { return this.fileOperation; }
		}

		public String FileNameClient
		{
			get { return this.fileNameClient; }
		}
	}
}