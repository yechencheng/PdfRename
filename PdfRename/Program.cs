using System;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Collections.Generic;
using System.Collections;

namespace PdfRename
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length != 1) {
				Console.Error.WriteLine ("No source file or directory\nUsage : command file");
				return;
			}
			FileInfo file = new FileInfo (args [0]);
			if ((file.Attributes & FileAttributes.Directory) != 0) {
				MultiFiles MF = new MultiFiles (new DirectoryInfo (file.FullName));
				MF.Process ();
			}
			else {
				SingleFile sf = new SingleFile (file);
				sf.Process ();
				//ProcessFile (file);
			}
		}
	}
}
