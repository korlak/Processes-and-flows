using System;
using System.IO;
using System.Threading;
using System.Xml;

class Program
{
    static void Main()
    {
        Console.WriteLine("Програма №1. \"Сортування даних\"");



        // Завантаження даних з XML файлу в пам'ять

        // Сортування масиву з 1-секундною затримкою на кожну ітерацію

        // Запис відсортованих даних назад у файл

        // Вивід повідомлення про завершення сортування


        try
        {
            // Отримання доступу до синхронізаційного об'єкта (наприклад, Mutex)
            using (Mutex mutex = new Mutex(false, "MyMutex"))
            {
                // Очікування доступу до спільних даних
                mutex.WaitOne();
                // Обробка натискання клавіші пробілу
                Console.WriteLine("Натисніть пробіл, щоб почати сортування...");
                while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;
                // Критична секція коду для сортування масиву
                Console.WriteLine("Початок сортування...");
                int[] data = LoadDataFromXml("C:\\DATA\\data.xml");

                SortArray(data);

                // Запис відсортованих даних назад у файл
                WriteDataToXml(data, "C:\\DATA\\data.xml");


                // Звільнення ресурсів синхронізації
                mutex.ReleaseMutex();

                Console.WriteLine("Робота завершена.");

                Console.WriteLine("Натисніть пробіл, щоб продовжити...");
                while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) ;

            }


        }
        catch (Exception ex)
        {
            Console.WriteLine("Помилка при сортуванні масиву: " + ex.Message);
        }
    }

    static int[] LoadDataFromXml(string filename)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(filename);

        XmlNodeList nodeList = doc.SelectNodes("/Numbers/Number");
        int[] data = new int[nodeList.Count];
        for (int i = 0; i < nodeList.Count; i++)
        {
            data[i] = int.Parse(nodeList[i].InnerText);
        }

        return data;
    }

    static void SortArray(int[] arr)
    {
        // Використання будь-якого методу сортування (наприклад, QuickSort)
        QuickSort(arr, 0, arr.Length - 1);
    }

    static void QuickSort(int[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivotIndex = Partition(arr, left, right);
            QuickSort(arr, left, pivotIndex - 1);
            QuickSort(arr, pivotIndex + 1, right);
        }
    }

    static int Partition(int[] arr, int left, int right)
    {
        int pivot = arr[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                Swap(arr, i, j);
            }
            // Штучна затримка для наочності
        }

        Swap(arr, i + 1, right);
        return i + 1;
    }

    static void Swap(int[] arr, int i, int j)
    {
        int temp = arr[i];
        arr[i] = arr[j];
        arr[j] = temp;
    }

    static void WriteDataToXml(int[] data, string filename)
    {
        using (XmlWriter writer = XmlWriter.Create(filename))
        {
            writer.WriteStartElement("Numbers");

            foreach (int number in data)
            {
                writer.WriteElementString("Number", number.ToString());
            }

            writer.WriteEndElement();
        }
    }
}