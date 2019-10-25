using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;

namespace PrimeNumberGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WriteToFile();
        }

        private bool isPrime(ulong number)
        {
            bool isComposite = false;

            string lastDigit = number.ToString().Substring(number.ToString().Length - 1, 1);

            if(lastDigit=="2" ||lastDigit=="4" || lastDigit=="6" || lastDigit=="8" || (lastDigit=="5" && number!=5) || lastDigit=="0")
            {
                isComposite = true;
            }

            for (ulong i = 2; i <= (Math.Sqrt(number)); i++)
            {
                if (number % i == 0  || isComposite==true)
                {
                    isComposite = true;
                    break;
                }
            }
            return !isComposite;
        }

        private void WriteToFile()
        {
            //txtDisplay.Text = "Process started\r\n";
            string[] readLine = File.ReadAllLines("primeNumbers.txt");
            ulong number = readLine.Length!=0?ulong.Parse(readLine[readLine.Length-1].Split()[0])+1:2;
            ulong counter = readLine.Length != 0 ? ulong.Parse(readLine[readLine.Length - 1].Split()[1])+1 : 1;
            double ratio = 0;

            while (true)
            {
                if (isPrime(number))
                {
                    ratio = (double)number / (double)counter;
                    File.AppendAllText("primeNumbers.txt",number+" "+counter+" "+DateTime.Now+" "+ratio.ToString("n20")+"\r\n");
                    //txtDisplay.Text+= number + " " + counter + " " + DateTime.Now + "\r\n";
                    counter++;
                }
                number++;
            }
        }
    }
}
