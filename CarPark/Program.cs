using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CarPark
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                var param = input.Split(' ');
                int type = int.Parse(param[0]);
                int duration = int.Parse(param[1]);

                Task task = Task.Run(() => VehicleEnter(type, duration));
            }

        }

        public static BlockingCollection<int> capability = new BlockingCollection<int>(10);
        public static int limit = 10;

        public static void VehicleEnter(int type, int duration)
        {

            while (capability.IsCompleted || limit - capability.Count < type)
            { }

            for (int i = 0; i < type; i++)
                capability.Add(1);

            if (capability.Count == limit)
                Console.WriteLine("Car park full!");


            Console.WriteLine("{0} vehicles / total capacity", limit - capability.Sum());
            int c = 0;
            while (c < duration)
            {
                Thread.Sleep(1000);
                c++;
            }


            for (int i = 0; i < type; i++)
                capability.Take();
            var left = limit - capability.Count;
            Console.WriteLine("{0} has just left the car park staying for {1} seconds time and there are {2} spots left", type == 2 ? "Car" : "Motorcycle", duration, left);

        }
    }
}
