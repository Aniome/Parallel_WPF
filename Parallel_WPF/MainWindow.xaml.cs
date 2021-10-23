using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Parallel_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DateTime start;
        ulong sum = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void setProgressBarOneThread()
        {
            Dispatcher.Invoke(() => {
                ProgressBarOneThread.Value += 25;
            });
        }
        private void setProgressBarMultiThread(ulong res)
        {
            sum += res;
            Dispatcher.Invoke(() => {
                ProgressBarMultiThread.Value += 25;
            });
        }
        private void Off_On_Buttons(bool one, bool multi)
        {
            button.IsEnabled = one;
            button_multi.IsEnabled = multi;
        }
        private void oneThread()
        {
            ulong res = 0;
            ulong max = 1000000001;
            ulong[] values = { 250000000, 500000000, 750000000 };
            for (ulong i = 1; i < max; i++)
            {
                res += i;
                if (i.Equals(values[0]) || i.Equals(values[1]) || i.Equals(values[2]))
                    setProgressBarOneThread();
            }
            setProgressBarOneThread();
            double time = (DateTime.Now - start).Ticks * 1e-7;
            Dispatcher.Invoke(() => {
                txtblock.Text = "Время: " + time.ToString() + " с";
                Result.Text = "Результат: " + res.ToString();
                Off_On_Buttons(true, true);
            });
        }
        private void multiThread()
        {
            Start_End se1 = new Start_End(0, 250000000);
            Thread thread1 = new Thread(new ParameterizedThreadStart(SumInThreads));
            Start_End se2 = new Start_End(250000000, 500000000);
            Thread thread2 = new Thread(new ParameterizedThreadStart(SumInThreads));
            Start_End se3 = new Start_End(500000000, 750000000);
            Thread thread3 = new Thread(new ParameterizedThreadStart(SumInThreads));
            Start_End se4 = new Start_End(750000000, 1000000001);
            Thread thread4 = new Thread(new ParameterizedThreadStart(SumInThreads));
            thread1.Start(se1);
            thread2.Start(se2);
            thread3.Start(se3);
            thread4.Start(se4);
            thread1.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();
            double time = (DateTime.Now - start).Ticks * 1e-7;
            Dispatcher.Invoke(() => {
                txtblock_multi.Text = "Время: " + time.ToString() + " с";
                Result_multi.Text = "Результат: " + sum.ToString();
                Off_On_Buttons(true, true);
            });
            sum = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartThread(ProgressBarOneThread);
            Thread thread = new Thread(oneThread);
            thread.Start();
        }
        private void button_multi_Click(object sender, RoutedEventArgs e)
        {
            StartThread(ProgressBarMultiThread);
            Thread thread = new Thread(multiThread);
            thread.Start();
        }
        private void SumInThreads(object o)
        {
            Start_End se = (Start_End)o;
            ulong res = 0;
            for (ulong i = se.start; i < se.end; i++)
                res += i;


            setProgressBarMultiThread(res);
        }
        private class Start_End
        {
            public Start_End(ulong s, ulong e) 
            {
                start = s;
                end = e;
            }
            public ulong start { get; }
            public ulong end { get; }
        }
        private void StartThread(ProgressBar pb)
        {
            pb.Value = 0;
            Off_On_Buttons(false, false);
            start = DateTime.Now;
        }
    }
}