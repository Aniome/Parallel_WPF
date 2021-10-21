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
        bool busy = false;
        public MainWindow()
        {
            InitializeComponent();
        }
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
        private void oneThread()
        {
            busy = true;
            ulong size = 10000000;
            ulong[] arr = new ulong[size];
            FillArray(arr);
            qsortConsistent(arr, 0, size - 1);
            this.Dispatcher.Invoke(()=> {
                txtbox.Text = "Hey";
            });
            busy = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!busy)
            {
                Thread thread = new Thread(oneThread);
                thread.Start();
            }
        }
    }
}
