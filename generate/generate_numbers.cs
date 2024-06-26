using System;
using System.IO;
using System.Xml;
using System.Threading; // Додано директиву using для простору iмен System.Threading

namespace RandomNumberGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Отримання доступу до синхронiзацiйного об'єкта (наприклад, Mutex)
                using (Mutex mutex = new Mutex(false, "MyMutex"))
                {
                    // Очiкування доступу до спiльних даних
                    mutex.WaitOne();

                    int min = 10;
                    int max = 100;
                    int count = new Random().Next(20, 31); // Випадкова кiлькiсть чисел вiд 20 до 30

                    int[] numbers = GenerateRandomNumbers(min, max, count);

                    WriteNumbersToXml(numbers, "data.xml");

                    // Звiльнення ресурсiв синхронiзацiї
                    mutex.ReleaseMutex();

                    Console.WriteLine($"Generated {count} random numbers and saved to data.xml");

                    Console.WriteLine("Натиснiть пробiл, щоб продовжити...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при генеруваннi випадкових чисел: " + ex.Message);
            }
        }

        static int[] GenerateRandomNumbers(int min, int max, int count)
        {
            Random random = new Random();
            int[] numbers = new int[count];
            for (int i = 0; i < count; i++)
            {
                numbers[i] = random.Next(min, max + 1); // +1, щоб включити максимальне значення
            }
            return numbers;
        }

        static void WriteNumbersToXml(int[] numbers, string fileName)
        {
            using (XmlWriter writer = XmlWriter.Create(fileName))
            {
                writer.WriteStartElement("Numbers");

                foreach (int number in numbers)
                {
                    writer.WriteElementString("Number", number.ToString());
                }

                writer.WriteEndElement();
            }
        }
    }
}