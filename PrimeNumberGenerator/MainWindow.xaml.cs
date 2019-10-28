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

            if ((lastDigit == "2" && number != 2) || lastDigit == "4" || lastDigit == "6" || lastDigit == "8" || (lastDigit == "5" && number != 5) || lastDigit == "0")
            {
                isComposite = true;
            }

            for (ulong i = 2; i <= (Math.Sqrt(number)); i++)
            {
                if (number % i == 0 || isComposite == true)
                {
                    isComposite = true;
                    break;
                }
            }
            return !isComposite;
        }

        private void WriteToFile()
        {
            //directory path in Documents folder named PrimeNumbers
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PrimeNumbers";

            //Create directory
            Directory.CreateDirectory(path);

            //path for data file that keeps track of number of files created
            string pathData = path + @"\data";

            //initial file counter set to 0
            int fileCounter = 0;

            //checks for existence of data file
            if (!File.Exists(pathData))
            {
                File.Create(pathData).Close();
            }
            else
            {
                //reads file counter from data file
                fileCounter = int.Parse(File.ReadAllText(pathData));
            }

            //set file name based on file counter
            string pathFile = path + @"\primenumbers" + fileCounter + ".csv";

            //Checks existence of the file
            if (!File.Exists(pathFile))
            {
                File.Create(pathFile).Close();
            }
            
            //reads the data from latest file so that the process can continue from the latest result
            string[] readLine = File.ReadAllLines(pathFile);
            ulong number = readLine.Length != 0 ? ulong.Parse(readLine[readLine.Length - 1].Split(',')[1]) + 1 : 2;
            ulong counter = readLine.Length != 0 ? ulong.Parse(readLine[readLine.Length - 1].Split(',')[0]) + 1 : 1;
            double ratio = 0;

            while (true)
            {
                if (isPrime(number))
                {
                    ratio = (double)number / (double)counter;
                    File.AppendAllText(path + @"\primenumbers" + fileCounter + ".csv", counter + "," + number + "," + DateTime.Now + "," + ratio.ToString("n20") + "\r\n");

                    //writes the file counter in data file
                    File.WriteAllText(pathData, fileCounter.ToString());

                    //creates new file every million entries
                    fileCounter = (int)(counter / 1000000);

                    counter++;                    
                }
                number++;
            }
        }

    }
}
