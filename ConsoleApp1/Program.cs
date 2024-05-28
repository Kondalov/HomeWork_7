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
        Console.WriteLine("7. \tРедактирование записи");
        Console.WriteLine("8. \tГенерация записей");
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
            //редактирование записей
            case 7:
                Console.Write("Введите ID записи для редактирования: ");
                input = Console.ReadLine();

                if (!int.TryParse(input, out int editId))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID должен быть числовым значением.");
                    Console.ResetColor();
                }
                else
                {
                    var workerToEdit = repository.GetWorkerById(editId);
                    if (workerToEdit.Equals(default(Worker)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Работник с таким ID не найден.");
                        Console.ResetColor();
                        break;
                    }

                    Console.Write("Введите новое Ф.И.О. (оставьте пустым для сохранения текущего значения): ");
                    string newFio = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newFio) && !Regex.IsMatch(newFio, @"^[А-Яа-яA-Za-z\s]+$"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ф.И.О. должно содержать только буквы и пробелы.");
                        Console.ResetColor();
                        break;
                    }

                    Console.Write("Введите новый возраст (оставьте пустым для сохранения текущего значения): ");
                    
                    string newAgeInput = Console.ReadLine();
                    int newAge = workerToEdit.Age;

                    if (!string.IsNullOrWhiteSpace(newAgeInput) && !int.TryParse(newAgeInput, out newAge))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Возраст должен быть числовым значением.");
                        Console.ResetColor();
                        break;
                    }

                    Console.Write("Введите новый рост (оставьте пустым для сохранения текущего значения): ");
                    
                    string newHeightInput = Console.ReadLine();
                    int newHeight = workerToEdit.Height;
                    
                    if (!string.IsNullOrWhiteSpace(newHeightInput) && !int.TryParse(newHeightInput, out newHeight))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Рост должен быть числовым значением.");
                        Console.ResetColor();
                        break;
                    }

                    Console.Write("Введите новую дату рождения (дд.мм.гггг) (оставьте пустым для сохранения текущего значения): ");
                    
                    string newDobInput = Console.ReadLine();
                    DateTime newDob = workerToEdit.DateOfBirth;
                    
                    if (!string.IsNullOrWhiteSpace(newDobInput) && !DateTime.TryParseExact(newDobInput, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out newDob))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Дата рождения должна быть в формате дд.мм.гггг.");
                        Console.ResetColor();
                        break;
                    }

                    Console.Write("Введите новое место рождения (оставьте пустым для сохранения текущего значения): ");
                    string newPob = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newPob) && !Regex.IsMatch(newPob, @"^[А-Яа-яA-Za-z\s]+$"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Место рождения должно содержать только буквы и пробелы.");
                        Console.ResetColor();
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(newFio)) workerToEdit.FIO = newFio;
                    if (!string.IsNullOrWhiteSpace(newAgeInput)) workerToEdit.Age = newAge;
                    if (!string.IsNullOrWhiteSpace(newHeightInput)) workerToEdit.Height = newHeight;
                    if (!string.IsNullOrWhiteSpace(newDobInput)) workerToEdit.DateOfBirth = newDob;
                    if (!string.IsNullOrWhiteSpace(newPob)) workerToEdit.PlaceOfBirth = newPob;

                    repository.UpdateWorker(workerToEdit);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Запись обновлена.");
                    Console.ResetColor();
                }
                break;
            //Генерация записей
            case 8:
                Console.Write("Введите количество записей для генерации: ");
                input = Console.ReadLine();

                if (!int.TryParse(input, out int numberOfRecords))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Количество записей должно быть числовым значением.");
                    Console.ResetColor();
                }
                else
                {
                    repository.GenerateRecords(numberOfRecords);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Записи сгенерированы.");
                    Console.ResetColor();
                }
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

//Вывод в таблице
static void PrintWorkersTable(Worker[] workers)
{
    const int pageSize = 10;
    int totalPages = (int)Math.Ceiling((double)workers.Length / pageSize);
    int currentPage = 1;

    while (true)
    {
        Console.Clear();

        int tableWidth = 130;
        string[] headers = { "ID", "Дата добавления", "Ф.И.О.", "Возраст", "Рост", "Дата рождения", "Место рождения" };
        int[] columnWidths = { 5, 25, 30, 10, 10, 20, 30 };

        PrintLine(tableWidth);
        PrintRow(headers, columnWidths);
        PrintLine(tableWidth);

        var pageWorkers = workers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToArray();
        foreach (var worker in pageWorkers)
        {
            string[] row = {
                    worker.Id.ToString(),
                    worker.RecordAdded.ToString("dd.MM.yyyy HH:mm:ss"),
                    worker.FIO,
                    worker.Age.ToString(),
                    worker.Height.ToString(),
                    worker.DateOfBirth.ToString("dd.MM.yyyy"),
                    worker.PlaceOfBirth
                };
            PrintRow(row, columnWidths);
        }
        PrintLine(tableWidth);

        Console.WriteLine($"\nСтраница {currentPage} из {totalPages}");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Введите `W` для перехода к следующей странице, `S` для перехода к предыдущей странице, или Q для выхода.");
        Console.ResetColor();

        var input = Console.ReadKey();
        Console.WriteLine();

        if (input.Key == ConsoleKey.W || input.Key == ConsoleKey.UpArrow)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nВы достигли последней страницы\n");
                Console.ResetColor();
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
        else if (input.Key == ConsoleKey.S || input.Key == ConsoleKey.DownArrow)
        {
            if (currentPage > 1)
            {
                currentPage--;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nВы достигли первой страницы\n");
                Console.ResetColor();
                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
        else if (input.Key == ConsoleKey.Q)
        {
            break;
        }
    }
}

//Линии по горизонту
static void PrintLine(int width)
{
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine(new string('-', width));
    Console.ResetColor();
}

static void PrintRow(string[] columns, int[] columnWidths)
{
    string row = "|";

    for (int i = 0; i < columns.Length; i++)
    {
        row += AlignCentre(columns[i], columnWidths[i]) + "|";
        
    }
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(row);
    Console.ResetColor();

}

static string AlignCentre(string text, int width)
{
    if (text.Length > width)
    {
        return text.Substring(0, width - 3) + "...";
    }
    if (string.IsNullOrEmpty(text))
    {
        return new string(' ', width);
    }

    return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
}