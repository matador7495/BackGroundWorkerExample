using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackGroundWorkerExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        struct DataParameter
        {
            public int process;
            public int delay;
        }
        private DataParameter _inputparameter;

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
            lblPercent.Text = string.Format("Processing...{0}%", e.ProgressPercentage);
            progress.Update();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int process = ((DataParameter)e.Argument).process;
            int delay = ((DataParameter)e.Argument).delay;
            int index = 1;
            try
            {
                for (int i = 0; i < process; i++)
                {
                    if (!backgroundWorker.CancellationPending)
                    {
                        backgroundWorker.ReportProgress(index++ * 100 / process, string.Format("Process Data {0}", i));
                        //used to simulate length of operation
                        Thread.Sleep(delay);
                        //add your code here......


                        //We use the following code to run our code in a separate program from the original application
                        //برای اینکه کد ما در ترید جدا از برنامه اصلی اجرا شود از کد زیر استفاده می کنیم

                        // lblPercent.Invoke(new Action(() =>
                        // {
                        //    lblPercent.Text = i.ToString();
                        // }));
                    }
                }
            }
            catch (Exception ex)
            {
                backgroundWorker.CancelAsync();
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error.....", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                MessageBox.Show("Process has been Canceled.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                progress.Value = 0;
            }
            else
            {
                MessageBox.Show("Process has been completed.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                progress.Value = 0;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                _inputparameter.delay = 10;
                _inputparameter.process = 1200;
                backgroundWorker.RunWorkerAsync(_inputparameter);//دریافت نتیجه ترید توسط آرگومان
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }
    }
}