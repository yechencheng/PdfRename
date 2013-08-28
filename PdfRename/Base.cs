using System;
using System.IO;
using System.Diagnostics;

namespace PdfRename
{
	public class Base
	{  
		protected bool GetVerify()
		{
			while (true) {
				string s = Console.ReadLine ().ToLower ();
				if (s == "n" || s == "no")
					return false;
				if (s.Length == 0 || s == "y" || s == "yes")
					return true;
				Console.WriteLine ("Please Input Y or N ");
			}
		}

		protected bool Verify(string source, string title)
		{
			Console.WriteLine ();
			Console.WriteLine ("File : " + source);
			Console.WriteLine ("Title : " + title);
			Console.Write ("Rename file ? [yes] : ");
			return GetVerify ();
		}

		//Input : PDF file		Output : path of html file
		protected string GetHTMLFile(FileInfo file)
		{
			string Path = file.DirectoryName;
			string Name = file.Name;
			Name = Path + "/" + Name + ".html";

			ProcessStartInfo startInfo = new ProcessStartInfo ();
			startInfo.FileName = "java";
			startInfo.Arguments = "-jar /Users/apple/Project/pdftest/pdfbox-app-1.8.2.jar ExtractText -html \"" + file.FullName + "\" \"" + Name + "\"";
			//startInfo.Arguments = "-version";
			startInfo.UseShellExecute = false;
			startInfo.RedirectStandardError = true;
			startInfo.RedirectStandardOutput = true;
			Process.Start (startInfo).WaitForExit();
			if (!File.Exists (Name))
				return null;
			return Name;
		}

		//Input : HTML File		Output : Title
		protected string GetTitle(FileInfo file)
		{
			const string HeadStr = "\"page-break-before:always; page-break-after:always\"><div><p>";
			const string TailStr = "</p>";
			StreamReader sr = new StreamReader (file.FullName);

			string x = sr.ReadLine ();
			while(x != null && !x.Contains(HeadStr))
			{
				x = sr.ReadLine();
			}

			if (x == null)
				return null;

			string rt = x.Substring(x.LastIndexOf(HeadStr) + HeadStr.Length);
			while (!rt.Contains(TailStr))
				rt += sr.ReadLine ();
			rt = rt.Remove (rt.LastIndexOf (TailStr));
			sr.Close ();
			return rt;
		}
	}
}

