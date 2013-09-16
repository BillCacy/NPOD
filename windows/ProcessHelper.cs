using System.ComponentModel;

namespace NasaPicOfDay
{
    public class ProcessHelper
    {
        public BackgroundWorker _processHelper;
        public delegate void RunFunction(); //Allows a function to be passed into the class
        public RunFunction thisFunction;
        LoadingScreen loadingScr;

        public void BackgroundLoading(RunFunction newFunction)
        {
            thisFunction = newFunction;
            _processHelper = new BackgroundWorker();
            _processHelper.DoWork += new DoWorkEventHandler(_processHelper_DoWork);
            _processHelper.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_processHelper_RunWorkerCompleted);
        }

        void _processHelper_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            loadingScr.Dispose();
        }

        void _processHelper_DoWork(object sender, DoWorkEventArgs e)
        {
            if (thisFunction != null)
                thisFunction();
        }

        public void Start()
        {
            _processHelper.RunWorkerAsync();
            loadingScr = new LoadingScreen();
            loadingScr.ShowDialog();
        }
    }
}
