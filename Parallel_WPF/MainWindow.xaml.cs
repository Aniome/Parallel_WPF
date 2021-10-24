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
        DateTime start_time;
        ulong sum = 0;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void setProgressBar(ProgressBar pb)
        {
            Dispatcher.Invoke(() => {
                pb.Value += 25;
            });
        }
        private void PrintResults(TextBlock textBlock, TextBlock result)
        {
            double time = (DateTime.Now - start_time).Ticks * 1e-7;
            Dispatcher.Invoke(() => {
                textBlock.Text = "Время: " + time.ToString() + " с";
                result.Text = "Результат: " + sum.ToString();
                Off_On_Buttons(true, true);
            });
        }
        private void Off_On_Buttons(bool one, bool multi)
        {
            button.IsEnabled = one;
            button_multi.IsEnabled = multi;
        }
        private void oneThread()
        {
            ulong max = 1000000001;
            for (ulong i = 1; i < max; i++)
            {
                sum += i;
                if (i == 250000000 || i == 500000000 || i == 750000000)
                    setProgressBar(ProgressBarOneThread);
            }
            setProgressBar(ProgressBarOneThread);
            PrintResults(txtblock,Result);
            sum = 0;
        }
        private void multiThread()
        {
            Start_End[] start_Ends = new Start_End[4];
            ulong start = 0;
            ulong end = 250000001;
            for (byte i = 0; i < start_Ends.Length; i++)
            {
                start_Ends[i] = new Start_End(start, end);
                start = end;
                end += 250000000;
            }
            Thread[] threads = new Thread[4];
            for (byte i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(SumInThreads));
                threads[i].Start(start_Ends[i]);
            }
            for (byte i = 0; i < threads.Length; i++)
                threads[i].Join();
            PrintResults(txtblock_multi,Result_multi);
            sum = 0;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartThread(ProgressBarOneThread, oneThread);
        }
        private void button_multi_Click(object sender, RoutedEventArgs e)
        {
            StartThread(ProgressBarMultiThread, multiThread);
        }
        private void SumInThreads(object o)
        {
            Start_End se = (Start_End)o;
            ulong res = 0;
            for (ulong i = se.start; i < se.end; i++)
                res += i;
            sum += res;
            setProgressBar(ProgressBarMultiThread);
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
        private void StartThread(ProgressBar pb, ThreadStart threadStart)
        {
            pb.Value = 0;
            Off_On_Buttons(false, false);
            start_time = DateTime.Now;
            Thread thread = new Thread(threadStart);
            thread.Start();
        }
    }
}