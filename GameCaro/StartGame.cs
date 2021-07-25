using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public partial class StartGame : Form
    {
        public StartGame()
        {
            InitializeComponent();
            txtLoading.Hide();
            prbLoading.Hide();
        }

        //Data process
        private Task ProcessData(List<string> list, IProgress<ProgessReport> progress)
        {
            int index = -1;
            int totalProcess = list.Count;
            var progressReport = new ProgessReport();
            return Task.Run(() =>
            {
                for (int i = 0; i < totalProcess; i++)
                {
                    progressReport.PercentComplete = index++ * 100 / totalProcess;
                    progress.Report(progressReport);
                    Thread.Sleep(2);

                }
            });
        }

        //Loading and convert to KingofSky Form
        private async void pcbStart_Click(object sender, EventArgs e)
        {
            btnStart.Hide();
            btnClose.Hide();
            prbLoading.Show();
            txtLoading.Show();
            List<string> list = new List<string>();
            for (int i = 0; i < 1000; i++)
                list.Add(i.ToString());
            txtLoading.Text = "Loading...";
            var progress = new Progress<ProgessReport>();
            progress.ProgressChanged += (o, report) =>
            {
                txtLoading.Text = "Loading..." + report.PercentComplete + "%"; //string.Format("Loading...(0)%", report.PercentComplete);
                prbLoading.Value = report.PercentComplete;
                prbLoading.Update();

            };
            await ProcessData(list, progress);
            KingofSkyGame kingofSkyGame = new KingofSkyGame();
            Visible = false;
            kingofSkyGame.Show();
            
        }

        private void pcbClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
