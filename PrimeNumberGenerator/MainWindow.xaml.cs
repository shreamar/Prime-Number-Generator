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
            //test();
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
            ulong number = readLine.Length != 0 ? ulong.Parse(readLine[readLine.Length - 1].Split(',')[1]) + 2 : 2;
            ulong counter = readLine.Length != 0 ? ulong.Parse(readLine[readLine.Length - 1].Split(',')[0]) + 1 : 1;
            double ratio = 0;

            while (true)
            {
                if (isPrimeNew(number))
                {
                    ratio = (double)number / (double)counter;
                    File.AppendAllText(path + @"\primenumbers" + fileCounter + ".csv", counter + "," + number + "," + DateTime.Now + "," + ratio.ToString("n20") + "\r\n");

                    //writes the file counter in data file
                    File.WriteAllText(pathData, fileCounter.ToString());

                    //creates new file every million entries
                    fileCounter = (int)(counter / 1000000);

                    counter++;
                }

                //increases to 3 if the number is 2
                if(number == 2)
                {
                    number++;
                }
                else
                {
                    //increases by 2 so the number is always odd
                    number += 2;
                }

                //increment the number in such a way that its always either prime or semi-prime
                bool repeat;
                do
                {
                    repeat = false;

                    if (number % 3 == 0 && number != 3)
                    {
                        number += 2;
                        repeat = true;
                    }
                    if (number % 5 == 0 && number != 5)
                    {
                        number += 2;
                        repeat = true;
                    }
                    if (number % 7 == 0 && number != 7)
                    {
                        number += 2;
                        repeat = true;
                    }
                } while (repeat);
            }
        }

        /// <summary>
        /// Upgraded alogorithm to determine prime number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool isPrimeNew(ulong number)
        {
            //assume the number to be composite
            bool isComposite = true;

            //last digit of the given number
            string lastDigit = number.ToString().Substring(number.ToString().Length - 1, 1);

            //prime numbers always end with either of 1, 3, 7 or 9
            if (lastDigit == "1" || lastDigit == "3" || lastDigit == "7" || (lastDigit == "9" && number.ToString().Length != 1))
            {
                //now assume the number to be prime
                isComposite = false;

                //every prime number can be represented as either 6n+1 or 6n-1, where n = any natural number
                if ((number + 1) % 6 == 0 || (number - 1) % 6 == 0)
                {
                    //check if the given number has a perfect square root
                    if (Math.Sqrt((double)number) == (ulong)(Math.Sqrt((double)number)))
                    {
                        isComposite = true;
                    }
                    else
                    {
                        //check if the number is divisible by primes and semi-primes from 3 to the square root of the given number
                        for (ulong i = 3; i <= (Math.Sqrt(number)); i = i)
                        {
                            if (number % i == 0)
                            {
                                isComposite = true;
                                break;
                            }

                            //increment by 2 so that the number is always odd
                            i += 2;

                            bool repeat;

                            //increments number in a way that it is always either prime or semi-prime
                            do
                            {
                                repeat = false;

                                if (i % 3 == 0)
                                {
                                    i += 2;
                                    repeat = true;
                                }
                                if (i % 5 == 0)
                                {
                                    i += 2;
                                    repeat = true;
                                }
                                if (i % 7 == 0 && i != 7)
                                {
                                    i += 2;
                                    repeat = true;
                                }
                            } while (repeat);
                        }
                    }
                }
                else
                {
                    isComposite = true;
                }
            }
            else if (number == 2 || number == 5)
            {
                isComposite = false;
            }

            return !isComposite;
        }

        private void test()
        {
            /*
            string a = "";
            for (ulong i = 4; i < 1000; i++)
            {
                if (isPrime(i) != isPrimeNew(i))
                {
                    a = i.ToString();
                    break;
                }
                else
                {
                    a = "All is well";
                }

            }

            txtDisplay.Text = a;
            */

            int counter = 0;
            ulong top = 0;
            for (ulong i = 3; i <= 10000; i = i)
            {
                top++;
                txtDisplay.Text += !isPrime(i)?i.ToString()+" ":"";
                counter += !isPrime(i) ? 1 : 0;

                i += 2;

                bool repeat = false;

                do
                {
                    repeat = false;

                    if (i % 3 == 0)
                    {
                        i += 2;
                        repeat = true;
                    }
                    if (i % 5 == 0)
                    {
                        i += 2;
                        repeat = true;
                    }
                    if (i % 7 == 0 && i != 7)
                    {
                        i += 2;
                        repeat = true;
                    }
                } while (repeat);
            }
            //txtDisplay.Text += "\r\n" +  ((decimal)counter/ (decimal)top)*100;
        }
    }
}
