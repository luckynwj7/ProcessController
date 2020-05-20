using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProcessController
{
    public static class RealTimeEventHandler
    {
        public static void RealTimeMethod(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += TimerTick;
            timer.Start();
        }
        private static void TimerTick(object sender, EventArgs e)
        {
            ProcessManager.ProcessRead();
            ProcessManager.ProcessesKill();
        }
    }
}
