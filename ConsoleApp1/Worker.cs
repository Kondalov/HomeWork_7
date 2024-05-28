using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    struct Worker
    {
        public int Id { get; set; }
        public DateTime RecordAdded { get; set; }
        public string FIO { get; set; }
        public int Age { get; set; }
        public int Height { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }

        public Worker(int id, DateTime recordAdded, string fio, int age, int height, DateTime dateOfBirth, string placeOfBirth)
        {
            Id = id;
            RecordAdded = recordAdded;
            FIO = fio;
            Age = age;
            Height = height;
            DateOfBirth = dateOfBirth;
            PlaceOfBirth = placeOfBirth;
        }

        public override string ToString()
        {
            return $"{Id}#{RecordAdded}#{FIO}#{Age}#{Height}#{DateOfBirth}#{PlaceOfBirth}";
        }

        public static Worker Parse(string record)
        {
            var fields = record.Split('#');
            return new Worker(
            int.Parse(fields[0]),
            DateTime.Parse(fields[1]),
            fields[2],
            int.Parse(fields[3]),
            int.Parse(fields[4]),
            DateTime.Parse(fields[5]),
            fields[6]
            );
        }
    }
}
