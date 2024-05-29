using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp1
{
    class Repository
    {
        private readonly string _fileName;

        public Repository(string fileName)
        {
            _fileName = fileName;

            if (!File.Exists(_fileName))
            {
                File.Create(_fileName).Close();
            }
        }

        /// <summary>
        /// Просмотр записей
        /// </summary>
        /// <returns></returns>
        public Worker[] GetAllWorkers()
        {
            var workers = File.ReadAllLines(_fileName)
                .Select(Worker.Parse)
                .ToArray();
            return workers;
        }

        /// <summary>
        /// Просмотр записей по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Worker GetWorkerById(int id)
        {
            var workers = GetAllWorkers();
            return workers.FirstOrDefault(w => w.Id == id);
        }

        /// <summary>
        /// Удаление записей
        /// </summary>
        /// <param name="id"></param>
        public void DeleteWorker(int id)
        {
            var workers = GetAllWorkers();

            Console.WriteLine("Debug: идентификаторы в файле:");

            foreach (var worker in workers)
            {
                Console.WriteLine($"ID: {worker.Id}");
            }

            workers = GetAllWorkers();

            var workerToDelete = workers.FirstOrDefault(w => w.Id == id);

            if (!workerToDelete.Equals(default(Worker)))
            {
                var updatedWorkers = workers.Where(w => w.Id != id).ToArray();

                File.WriteAllLines(_fileName, updatedWorkers.Select(w => w.ToString()));
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запись удалена.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Работник с таким ID не найден.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="worker"></param>
        public void AddWorker(Worker worker)
        {
            var workers = GetAllWorkers();

            worker.Id = workers.Length > 0 ? workers.Max(w => w.Id) + 1 : 1;

            using (var sw = new StreamWriter(_fileName, true))
            {
                sw.WriteLine(worker.ToString());
            }
        }

        /// <summary>
        /// Просмотр сотрудников по дате добавления
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        public Worker[] GetWorkersBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
        {
            var workersInRange = new List<Worker>();

            using (var reader = new StreamReader(_fileName))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var worker = Worker.Parse(line);

                    if (worker.RecordAdded.Date >= dateFrom.Date && worker.RecordAdded.Date <= dateTo.Date)
                    {
                        workersInRange.Add(worker);
                    }
                }
            }

            return workersInRange.ToArray();
        }

        /// <summary>
        /// редактирование записей
        /// </summary>
        /// <param name="updatedWorker"></param>
        public void UpdateWorker(Worker updatedWorker)
        {
            var workers = GetAllWorkers();

            var workerIndex = Array.FindIndex(workers, w => w.Id == updatedWorker.Id);
            
            if (workerIndex != -1)
            {
                workers[workerIndex] = updatedWorker;
                
                File.WriteAllLines(_fileName, workers.Select(w => w.ToString()));
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запись обновлена.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Работник с таким ID не найден.");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Генерация Записей
        /// </summary>
        /// <param name="numberOfRecords"></param>
        public void GenerateRecords(int numberOfRecords)
        {
            var workers = GetAllWorkers().ToList();

            int currentMaxId = workers.Any() ? workers.Max(w => w.Id) : 0;
            
            for (int i = 1; i <= numberOfRecords; i++)
            {
                var worker = new Worker(
                    id: ++currentMaxId,
                    recordAdded: DateTime.Now,
                    fio: $"Имя{i}",
                    age: new Random().Next(20, 65),
                    height: new Random().Next(150, 200),
                    dateOfBirth: DateTime.Now.AddYears(-new Random().Next(20, 65)),
                    placeOfBirth: $"Город{i}"
                );

                workers.Add(worker);
            }
            File.WriteAllLines(_fileName, workers.Select(w => w.ToString()));
        }
    }
}
