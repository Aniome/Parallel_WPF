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
        private void setProgressBarOneThread(ulong value)
        {
            Dispatcher.Invoke(() => {
                ProgressBarOneThread.Value = value;
            });
        }
        private void setProgressBarMultiThread(ulong value, ulong res)
        {
            sum += res;
            Dispatcher.Invoke(() => {
                ProgressBarMultiThread.Value = value;
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
            ulong max = 999999999;
            for (ulong i = 1; i <= max; i++)
            {
                res += i;
                if (i == 333333333)
                    setProgressBarOneThread(33);
                if (i == 666666666)
                    setProgressBarOneThread(66);
            }
            setProgressBarOneThread(100);
            double time = (DateTime.Now - start).Ticks * 1e-7;
            Dispatcher.Invoke(() => {
                txtblock.Text = "Время: " + time.ToString() + " с";
                Result.Text = "Результат: " + res.ToString();
                Off_On_Buttons(true, true);
            });
        }
        private void multiThread()
        {
            Thread thread1 = new Thread(first_part);
            Thread thread2 = new Thread(second_part);
            Thread thread3 = new Thread(third_part);
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread1.Join();
            thread2.Join();
            thread3.Join();
            double time = (DateTime.Now - start).Ticks * 1e-7;
            Dispatcher.Invoke(() => {
                txtblock_multi.Text = "Время: " + time.ToString() + " с";
                Result_multi.Text = "Результат: " + sum.ToString();
                Off_On_Buttons(true, true);
            });
            sum = 0;
        }
        private void first_part()
        {
            ulong res = 0;
            for (ulong i = 0; i < 333333333; i++)
                res += i;
            setProgressBarMultiThread(33, res);
        }
        private void second_part()
        {
            ulong res = 0;
            for (ulong i = 333333333; i < 666666666; i++)
                res += i;
            setProgressBarMultiThread(66, res);
        }
        private void third_part()
        {
            ulong res = 0;
            for (ulong i = 666666666; i <= 999999999; i++)
                res += i;
            setProgressBarMultiThread(100, res);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Off_On_Buttons(false, false);
            start = DateTime.Now;
            Thread thread = new Thread(oneThread);
            thread.Start();
        }

        private void button_multi_Click(object sender, RoutedEventArgs e)
        {
            Off_On_Buttons(false, false);
            start = DateTime.Now;
            Thread thread = new Thread(multiThread);
            thread.Start();
        }

    }
}
/*
              ulong size = 100000000;
            ulong[] arr = new ulong[size];
            FillArray(arr);
            qsortConsistent(arr, 0, size - 1);
            double time = (DateTime.Now - start).Ticks * 1e-7;
            Dispatcher.Invoke(() => {
                txtblock.Text = "Время: "+time.ToString()+"с";
                button.IsEnabled = true;
            });
            size = 0;
            arr = null;
            System.GC.Collect();
          void FillArray(ulong[] arr)
        {
            Random rnd = new Random();
            for (int i = 0; i < arr.Length; i++)
                arr[i] = (ulong)rnd.Next(100);
        }
        void qsortConsistent(ulong[] arr, ulong first, ulong last)
        {
            if (first < last)
            {
                ulong left = first;
                ulong right = last;
                ulong middle = arr[(left + right) / 2];
                do
                {
                    while (arr[left] < middle)
                        left++;
                    while (arr[right] > middle)
                        right--;
                    if (left <= right)
                    {
                        swap(ref arr[left], ref arr[right]);
                        left++;
                        right--;
                    }
                } while (left <= right);
                qsortConsistent(arr, first, right);
                qsortConsistent(arr, left, last);
            }
        }
        void swap(ref ulong a, ref ulong b)
        {
            ulong t = a;
            a = b;
            b = t;
        }
 */