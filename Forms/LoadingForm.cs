using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Wikisplorer
{
    public partial class LoadingForm : Form
    {
        private Timer timer;
        private int dotCount = 0;

        public LoadingForm()
        {
            InitializeComponent();
            label1.Text = "Loading";

            // Initialize and start the timer
            timer = new Timer();
            timer.Interval = 500; // .5 second
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            dotCount = (dotCount + 1) % 4; // 0 to 3 dots
            label1.Text = "Loading" + new string('.', dotCount);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer.Stop();
            base.OnFormClosing(e);
        }
    }
}
