using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annealing
{
    class Program
    {
        
        static void Main(string[] args)
        {
            #region Объявление
            Random rand = new Random();
            Console.Write("Введите колличество городов = ");
            int n = 0;
            try
            {
                n = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("---Неверный формат ввода!---");
                int timer = DateTime.Now.Second + 5;
                while (timer != DateTime.Now.Second)
                { n = 0; }
                Environment.Exit(0);
            }
            ArrayList arr = new ArrayList();
            int[] queue = new int[n + 1];
            int[] queue1 = new int[n + 1];
            int[,] costArray = new int[n, n];
            int lineCost = int.MaxValue;
            int condition = 0;
            int possibility = 0;
            double pstar = 0;
            const double e = 2.71828182846;
            #endregion

            #region Методы

            //Составление первого пути
            void FirstLineFill()
            {
                arr.Add(1);
                bool cond = true;
                while (cond)
                {
                    int next = rand.Next(1, n + 1);

                    if (!arr.Contains(next))
                    {
                        arr.Add(next);
                    }

                    if (arr.Count == n)
                    { break; }
                }
                arr.Add(1);
                queue = (int[])arr.ToArray(typeof(int));
                for (int i = 0; i < queue.Length; i++)
                {
                    Console.Write("-" + queue[i]);
                }
                Console.WriteLine();
            }

            //Изменение пути
            void ReplaceLineQueue()
            {
                OverwritingQueue();
                int temporary = 0, index1 = 0, index2 = 0;
                index1 = rand.Next(1, n);
                do
                { index2 = rand.Next(1, n); }
                while (index1 == index2);
                temporary = queue[index1];
                queue[index1] = queue[index2];
                queue[index2] = temporary;
            }

            //Назначение стоимости пути от одного города к другому
            void CostFill()
            {
                for (int i = 0; i < n - 1; i++)
                {
                    Console.WriteLine("----------\n");
                    for (int j = i + 1; j < n; j++)
                    {
                        Console.Write("Цена города от {0} до города {1} = ", i + 1, j + 1);
                        try
                        {
                            costArray[i, j] = int.Parse(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("---Неверный формат ввода!---");
                            int timer = DateTime.Now.Second + 5;
                            while (timer != DateTime.Now.Second)
                            { n = 0; }
                            Environment.Exit(0);
                        }
                    }
                }
            }

            //Нахождение стоимости пути
            void LineCost()
            {
                int localLineCost = 0;
                for (int i = 0; i < queue.Length + 1; i++)
                {
                    if (queue[i] - 1 < queue[i + 1] - 1)
                    { localLineCost = localLineCost + costArray[queue[i] - 1, queue[i + 1] - 1]; }
                    else
                    { localLineCost = localLineCost + costArray[queue[i + 1] - 1, queue[i] - 1]; }
                    if (i + 1 == n)
                        break;
                }
                if (lineCost > localLineCost && lineCost != localLineCost)
                {
                    lineCost = localLineCost;
                    condition = 0;
                }
                else
                {
                    condition = localLineCost - lineCost;
                }
            }

            //Отжиг
            void Cycle()
            {
                double temperature = 100;
                while (temperature >= 1)
                {
                    ReplaceLineQueue();
                    LineCost();
                    if (condition != 0)
                    {
                        pstar = 100 * (Math.Pow(e, (double)(-(condition / temperature))));
                        FullRandom();
                        OverwritingQueue();
                    }
                    temperature = 0.8 * temperature;
                }
            }

            //Запоминание кратчайшего пути
            void OverwritingQueue()
            {
                if (condition == 0)
                {
                    for (int i = 0; i < queue.Length; i++)
                    {
                        queue1[i] = queue[i];
                    }
                }
                else
                {
                    if (pstar > possibility)
                    {
                        for (int i = 0; i < queue.Length; i++)
                        {
                            queue1[i] = queue[i];
                        }
                    }
                    else
                    {
                        for (int i = 0; i < queue.Length; i++)
                        {
                            queue[i] = queue1[i];
                        }
                    }
                }
            }

            //Рандом для вероятности
            void FullRandom()
            {
                possibility = rand.Next(0, 100);
            }
            #endregion

            #region Исполнительная часть
            FirstLineFill();
            CostFill();
            LineCost();
            Cycle();
            Console.WriteLine();
            Console.WriteLine("Цена кратчайшего пути = " + lineCost);
            Console.Write("Кротчайший путь = 1");
            for (int i = 1; i < queue.Length; i++)
            {
                Console.Write("-" + queue1[i]);
            }
            Console.WriteLine();
            Console.ReadLine();
            #endregion
        }
    }
}
