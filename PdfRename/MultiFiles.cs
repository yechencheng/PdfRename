using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;

namespace PdfRename
{
	public class MultiFiles
	{
		private DirectoryInfo SourceDir;
		private ConcurrentQueue<SingleFile> TaskQueue;

		private AutoResetEvent event1;

		private Thread O1;
		private	Thread O2;
		private bool TaskContinue;

		public MultiFiles (DirectoryInfo dir)
		{
			SourceDir = dir;
			TaskQueue = new ConcurrentQueue<SingleFile>();
			TaskContinue = true;
			event1 = new AutoResetEvent (false);
			O1 = new Thread (new ThreadStart (OperatorGetTitles));
			O2 = new Thread (new ThreadStart (OperatorProcessRename));
		}


		//Pipeline : OperatorGetTitles -> OperatorProcessRename
		private void OperatorGetTitles()
		{
			foreach (FileInfo f in SourceDir.GetFiles()) {
				if (Path.GetExtension (f.Name) != ".pdf")
					continue;
				SingleFile SF = new SingleFile (f);
				SF.ProcessTitle ();
				TaskQueue.Enqueue (SF);
				event1.Set ();
			}
			TaskContinue = false;
			event1.Set ();
		}

		private void OperatorProcessRename()
		{
			while (TaskContinue) {
				SingleFile SF;
				while (TaskQueue.TryDequeue(out SF)) {
					SF.ProcessRename ();
				}
				event1.WaitOne ();
			}
		}

		public void Process()
		{
			O1.Start ();
			O2.Start ();
			O1.Join ();
			O2.Join ();
		}
	}
}

