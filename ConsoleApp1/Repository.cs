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

        public Worker[] GetAllWorkers()
        {
            var workers = File.ReadAllLines(_fileName)
                .Select(Worker.Parse)
                .ToArray();
            return workers;
        }

        public Worker GetWorkerById(int id)
        {
            var workers = GetAllWorkers();
            return workers.FirstOrDefault(w => w.Id == id);
        }

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

        public void AddWorker(Worker worker)
        {
            var workers = GetAllWorkers();
            worker.Id = workers.Length > 0 ? workers.Max(w => w.Id) + 1 : 1;
            using (var sw = new StreamWriter(_fileName, true))
            {
                sw.WriteLine(worker.ToString());
            }
        }

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
    }
}
