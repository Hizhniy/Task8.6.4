using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    [Serializable]
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Write("Введите путь к базе студентов: ");
            string pathToFile = string.Empty;
            pathToFile = Console.ReadLine();

            if (File.Exists(pathToFile))
            {
                using (FileStream fs = new FileStream(pathToFile, FileMode.Open))
                {
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        Student[] students = (Student[])formatter.Deserialize(fs); // считали базу из файла в массив

                        // задали путь к результирующей папке на десктопе и создаем ее, если ее не было
                        string desktopPath = string.Empty;
                        desktopPath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), @"\students");
                        if (!Directory.Exists(desktopPath)) Directory.CreateDirectory(desktopPath);                        

                        string fileGroupPath = string.Empty;
                        foreach (Student student in students) // проходимся по всем студентам
                        {
                            fileGroupPath = desktopPath + @"\" + student.Group + @".txt"; // формируем путь к файлу группы студентов
                            using (FileStream sw = new FileStream(fileGroupPath, FileMode.Append))
                            {
                                using (StreamWriter streamWriter = new StreamWriter(sw))
                                {
                                    streamWriter.WriteLine($"{student.Name}, {student.DateOfBirth:dd.MM.yyyy}"); // вносим информацию о студенте
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("EXCEPTION: " + e.Message);
                    }
                }
            }
            else { Console.WriteLine("Файл не найден..."); }
        }
    }
}