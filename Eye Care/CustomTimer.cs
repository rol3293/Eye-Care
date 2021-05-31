using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Eye_Care
{
    class CustomTimer : System.Timers.Timer
    {
        private System.Timers.Timer timer;
        private NotifyIcon notifyIcon;
        private int sec;
        private Boolean isCountingDown = false;
        private int count = 0;
        private int remainingMin = 0;
        public CustomTimer(NotifyIcon notifyIcon, int sec)
        {
            this.notifyIcon = notifyIcon;
            this.sec = sec;
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            isCountingDown = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (!isCountingDown)
                return;

            // after every minutes, update tooltip text
            if (count % 60 == 0)
            {
                remainingMin = (sec - count) / 60;
                updateToolTipText(remainingMin);
            }

            // when the countdown reached its time
            if (count == sec)
            {
                // display notification
                notifyIcon.ShowBalloonTip(0, "Eye Care", "Take a break form your screen", ToolTipIcon.Info);

                // reset variables
                count = 0;
                remainingMin = sec / 60;
                updateToolTipText(remainingMin);
            }
            Console.WriteLine(count);
            count++;
        }

        private void updateToolTipText(int remainingMin)
        {
            notifyIcon.Text = "Eye Care. " + remainingMin + " minutes remaining";
        }

        public void Pause()
        {
            isCountingDown = false;
        }

        public void Resume()
        {
            isCountingDown = true;
        }

        public Boolean timerIsCountingDown()
        {
            return isCountingDown;
        }

        public int getRemainingMin()
        {
            return remainingMin;
        }
    }
}
