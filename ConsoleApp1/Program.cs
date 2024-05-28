using ConsoleApp1;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

Repository repository = new Repository("workers.txt");

    while (true)
    {
        Console.WriteLine("1. \tПросмотр всех записей");
        Console.WriteLine("2. \tПросмотр записи по ID");
        Console.WriteLine("3. \tСоздание записи");
        Console.WriteLine("4. \tУдаление записи");
        Console.WriteLine("5. \tЗагрузка записей в диапазоне дат");
        Console.WriteLine("6. \tСортировка записей по полю");
        Console.WriteLine("0. \tВыход");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\nВведите номер операции: ");
        Console.ResetColor();

        string input = Console.ReadLine();

    if (int.TryParse(input, out int operation))
    {
        switch (operation)
        {
            //Просмотр записей
            case 1:
                var workers = repository.GetAllWorkers();
                PrintWorkersTable(workers);
                break;
            //Просмотр записи по ID
            case 2:
                Console.Write("Введите ID записи: ");
                input = Console.ReadLine();

                if (int.TryParse(input, out int id))
                {
                    var workerById = repository.GetWorkerById(id);

                    if (workerById.Equals(default(Worker)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Запись не найдена.");
                        Console.ResetColor();
                    }
                    else
                    {
                        PrintWorkersTable(new[] { workerById });
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введите корректное числовое значение для ID.");
                    Console.ResetColor();
                }
                break;
            //ФИО; Возраст; Рост; Дата рождения; Место жительство
            case 3:
                Console.Write("Введите Ф.И.О.: ");
                string fio = Console.ReadLine();

                if (!Regex.IsMatch(fio, @"^[А-Яа-яA-Za-z\s]+$"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ф.И.О. должно содержать только буквы и пробелы.");
                    Console.ResetColor();
                    break;
                }

                Console.Write("Введите возраст: ");
                input = Console.ReadLine();
                if (!int.TryParse(input, out int age))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Возраст должен быть числовым значением.");
                    Console.ResetColor();
                    break;
                }

                Console.Write("Введите рост: ");
                input = Console.ReadLine();
                if (!int.TryParse(input, out int height))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Рост должен быть числовым значением.");
                    Console.ResetColor();
                    break;
                }

                Console.Write("Введите дату рождения (дд.мм.гггг): ");
                input = Console.ReadLine();
                if (!DateTime.TryParseExact(input, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dob))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Дата рождения должна быть в формате дд.мм.гггг.");
                    Console.ResetColor();
                    break;
                }

                Console.Write("Введите место рождения: ");
                string pob = Console.ReadLine();

                if (!Regex.IsMatch(pob, @"^[А-Яа-яA-Za-z\s]+$"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Место рождения должно содержать только буквы и пробелы.");
                    Console.ResetColor();
                    break;
                }

                Worker newWorker = new Worker(0, DateTime.Now, fio, age, height, dob, pob);
                repository.AddWorker(newWorker);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запись добавлена.");
                Console.ResetColor();
                break;
            //Удаление по ID
            case 4:
                Console.Write("Введите ID записи для удаления: ");
                input = Console.ReadLine();

                if (!int.TryParse(input, out int delId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID должен быть числовым значением.");
                    Console.ResetColor();
                }

                else
                {
                    repository.DeleteWorker(delId);
                }

                break;
            //Загрузка записей в диапазоне дат
            case 5:
                DateTime dateFrom;
                DateTime dateTo;
                string inputDateFrom;
                string inputDateTo;

                Console.Write("Введите начальную дату (дд.мм.гггг): ");
                inputDateFrom = Console.ReadLine();

                if (!DateTime.TryParseExact(inputDateFrom, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dateFrom))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный формат даты. Используйте формат дд.мм.гггг.");
                    Console.ResetColor();
                    break;
                }

                Console.Write("Введите конечную дату (дд.мм.гггг): ");
                inputDateTo = Console.ReadLine();

                if (!DateTime.TryParseExact(inputDateTo, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dateTo))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Некорректный формат даты. Используйте формат дд.мм.гггг.");
                    Console.ResetColor();
                    break;
                }

                var workersInRange = repository.GetWorkersBetweenTwoDates(dateFrom, dateTo);
                PrintWorkersTable(workersInRange);
                
                
                break;
            //Сортировка
            case 6:
                Console.WriteLine("Сортировка по: \n1. ID \n2. Дата и время добавления записи \n3. Ф.И.О. \n4. Возраст \n5. Рост \n6. Дата рождения \n7. Место рождения");
                Console.Write("Введите номер поля для сортировки: ");

                if (!int.TryParse(Console.ReadLine(), out int field))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Номер поля должен быть числовым значением.");
                    Console.ResetColor();
                    break;
                }

                var sortedWorkers = repository.GetAllWorkers();

                switch (field)
                {
                    case 1:
                        sortedWorkers = sortedWorkers.OrderBy(w => w.Id).ToArray();
                        break;
                    case 2:
                        sortedWorkers = sortedWorkers.OrderBy(w => w.RecordAdded).ToArray();
                        break;
                    case 3:
                        sortedWorkers = sortedWorkers.OrderBy(w => w.FIO).ToArray();
                        break;
                    case 4:
                        sortedWorkers = sortedWorkers.OrderBy(w => w.Age).ToArray();
                        break;
                    case 5:
                        sortedWorkers = sortedWorkers.OrderBy(w => w.Height).ToArray();
                        break;
                    case 6:
                        sortedWorkers = sortedWorkers.OrderBy(w => w.DateOfBirth).ToArray();
                        break;
                    case 7:
                        sortedWorkers = sortedWorkers.OrderBy(w => w.PlaceOfBirth).ToArray();
                        break;
                }
                PrintWorkersTable(sortedWorkers);
                break;
            //Выход
            case 0:
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Выход.");
                Console.ResetColor();
                Environment.Exit(0);
                
                break;

            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неккоректный номер операции. Попробуйте снова");
                Console.ResetColor();
                break;
        }
    }

    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Введите корректное числовое значение для номера операции.");
        Console.ResetColor();
    }
    }

static void PrintWorkersTable(Worker[] workers)
{
    Console.WriteLine("\n{0,-3} {1,-25} {2,-30} {3,-7} {4,-4} {5,-20} {6,-20}\n", "ID", "Дата добавления", "Ф.И.О.", "Возраст", "Рост", "Дата рождения", "Место рождения");
    Console.WriteLine(new string('-', 110));
    foreach (var worker in workers)
    {
        Console.WriteLine("\n{0,-3} {1,-25} {2,-30} {3,-7} {4,-4} {5,-20:dd.MM.yyyy} {6,-20}\n", worker.Id, worker.RecordAdded, worker.FIO, worker.Age, worker.Height, worker.DateOfBirth, worker.PlaceOfBirth);
    }
}