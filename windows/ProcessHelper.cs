using System.ComponentModel;

namespace NasaPicOfDay
{
	public class ProcessHelper
	{
		private BackgroundWorker _processHelper;
		public delegate void RunFunction(); //Allows a function to be passed into the class
		public RunFunction ThisFunction;
		public LoadingScreen LoadingScr;

		public void BackgroundLoading(RunFunction newFunction)
		{
			ThisFunction = newFunction;
			_processHelper = new BackgroundWorker();
			_processHelper.DoWork += new DoWorkEventHandler(_processHelper_DoWork);
			_processHelper.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_processHelper_RunWorkerCompleted);
		}

		void _processHelper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			LoadingScr.Dispose();
		}

		void _processHelper_DoWork(object sender, DoWorkEventArgs e)
		{
			if (ThisFunction != null)
				ThisFunction();
		}

		public void Start()
		{
			_processHelper.RunWorkerAsync();
			LoadingScr = new LoadingScreen();
			LoadingScr.ShowDialog();
		}
	}
}
