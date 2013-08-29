using System;
using System.IO;

namespace PdfRename
{
	public class SingleFile : Base
	{
		private string HTMLFile;
		private FileInfo Source;
		private string Title;
		private string Target;

		//Input PDF file
		public SingleFile (FileInfo s)
		{
			Source = s;
		}

		~SingleFile()
		{
			if(HTMLFile != null)
				File.Delete(HTMLFile);
		}

		public bool ProcessTitle()
		{
			HTMLFile = GetHTMLFile (Source);
			if (HTMLFile == null)
				return false;
			Title = GetTitle (new FileInfo (HTMLFile));
			return Title != null && Title.Length != 0;
		}

		public bool ProcessRename()
		{
			Target = Source.DirectoryName + "/" + Title + ".pdf";
			if (File.Exists (Target)) {
				Console.WriteLine (Source.FullName + "\n\tto " + Title + " Existed");
				return false;
			}
			if (!Verify (Source.FullName, Title))
				return false;
			File.Move (Source.FullName, Target);
			File.SetLastWriteTime(Target, DateTime.Now);
			return true;
		}

		public bool Process()
		{
			if (!ProcessTitle ())
				return false;
			return ProcessRename ();
		}
	}
}

