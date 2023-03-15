using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BeeColony
{
    class BeeColonyAI : INotifyPropertyChanged
    {
        readonly private Random _random = new();
        private double _x1;     //переменные для расчета
        private  double _x2;    //
        private int _x1Left;    // Диапозон фргументов
        private int _x1Right;   //
        private int _x2Left;    //
        private int _x2Right;   //
        private readonly Function _function; //пользовательская функция
        private double _functionMin = double.MaxValue; // ответ минимума функции
        private double _x1Min = double.MaxValue; // ответ минимума первого аргумента
        private double _x2Min = double.MaxValue; // ответ минимума второго аргумента
        readonly List<Bee> firstIntelligenceAgents = new(); // количество всех разведчиков
        readonly List<Bee> bestAgents = new(); // количество разведчиков в лучших местах
        readonly List<Bee> ordinaryAgents = new(); // количество разведчиков обычных мест

        public event PropertyChangedEventHandler? PropertyChanged;

        public double FunctionMin
        {
            get { return _functionMin; }
            set { _functionMin = value; }
        }

        public double X1Min
        {
            get { return _x1Min; }
            set { _x1Min = value; }
        }

        public double X2Min
        {
            get { return _x2Min; }
            set { _x2Min = value; }
        }

        public BeeColonyAI(int x1Left, int x1Right, int x2Left, int x2Right, string function)
        {
            _x1Left = x1Left;
            _x1Right = x1Right;
            _x2Left = x2Left;
            _x2Right = x2Right;
            _function = new Function(function);
        }
        /// <summary>
        /// Создание первых разведчиков
        /// </summary>
        /// <param name="sizeColony"> Количество разведчиков </param>
        public void CreateFirstAgents(int sizeColony)
        {
            for (var i = 0; i < sizeColony; i++)
            {
                _x1 = _random.NextDouble() * (_x1Right - _x1Left) + _x1Left;
                _x2 = _random.NextDouble() * (_x2Right - _x2Left) + _x2Left;
                firstIntelligenceAgents.Add(new Bee(_x1, _x2, _function.Calculate(_x1, _x2)));
            }
        }
        /// <summary>
        /// Сортировка результатов разведки по возрастанию
        /// </summary>
        public void SortBee()
        {
            Bee replace;
            for (var i = 0; i < firstIntelligenceAgents.Count; i++)
            {
                for (var j = 0; j < firstIntelligenceAgents.Count - 1; j++)
                {
                    if (firstIntelligenceAgents[j].Rezult > firstIntelligenceAgents[j + 1].Rezult)
                    {
                        replace = firstIntelligenceAgents[j + 1];
                        firstIntelligenceAgents[j + 1] = firstIntelligenceAgents[j];
                        firstIntelligenceAgents[j] = replace;
                    }
                }
            }
        }
        /// <summary>
        /// Создание разведчиков в потенциально лучших местах
        /// </summary>
        /// <param name="numberAgents"> Количество разведчиков потенциально лучших мест </param>
        public void CreateAgents(int numberAgents)
        {
            for (var i = 0; i < numberAgents; i++)
            {
                bestAgents.Add(firstIntelligenceAgents[i]);
            }
        }
        /// <summary>
        /// Создание разведчиков новых потенциально лучших мест
        /// </summary>
        /// <param name="numberAgents"> Количество разведчиков </param>
        /// <param name="choiseParametr"> Параметр, определяющий каких пчел следует добавить </param>
        public void CreateAgents(int numberAgents, int choiseParametr)
        {
            for (var i = 0; i < numberAgents; i++)
            {
                ordinaryAgents.Add(firstIntelligenceAgents[choiseParametr]);
                choiseParametr++;
            }
        }
        /// <summary>
        /// Возвращение серединной точки диапозона
        /// </summary>
        /// <param name="left"> Левая граница </param>
        /// <param name="right"> Правая граница </param>
        /// <returns> Серединная точка диапозона </returns>
        public double RangeReduction(double left, double right)
        {
            return (left - right) / 2;
        }
        /// <summary>
        /// Сокращение диапозона поиска
        /// </summary>
        public void RangeReduction()
        {
            _x1Left /= 2;
            _x1Right /= 2;
            _x2Left /= 2;
            _x2Right /= 2;
        }
        /// <summary>
        /// Создание новых разведчиков
        /// </summary>
        /// <param name="workBee"> Список для создания </param>
        /// <param name="numberAgents"> Количество разведчиков </param>
        public void MakeNewAgents(List<Bee> workBee,  int numberAgents)
        {
            for (var j = 0; j < workBee.Count; j++)
            {
                for (var k = 0; k < numberAgents; k++)
                {
                    _x1 = _random.NextDouble() * 2 * RangeReduction(_x1Left, _x1Right) + workBee[j].X1Value - RangeReduction(_x1Left, _x1Right);
                    _x2 = _random.NextDouble() * 2 * RangeReduction(_x2Left, _x2Right) + workBee[j].X2Value - RangeReduction(_x2Left, _x2Right);
                    if (workBee[j].Rezult > _function.Calculate(_x1, _x2))
                    {
                        workBee[j].Rezult = _function.Calculate(_x1, _x2);
                        workBee[j].X1Value = _x1;
                        workBee[j].X2Value = _x2;
                    }
                }
            }
        }
        /// <summary>
        /// Расчет минимального значения
        /// </summary>
        /// <param name="numberIteration"> Количество итераций </param>
        public void Calculate(int numberIteration)
        {
            for(var i = 0; i < numberIteration; i++)
            {
                MakeNewAgents(bestAgents, 5);
                MakeNewAgents(ordinaryAgents, 2);                               
                for(var j = 0; j < ordinaryAgents.Count + bestAgents.Count; j++)
                {
                    if(j < bestAgents.Count)
                    {
                        if (bestAgents[j].Rezult < _functionMin)
                        {
                            _functionMin = bestAgents[j].Rezult;
                            _x1Min = bestAgents[j].X1Value;
                            _x2Min = bestAgents[j].X2Value;
                        }
                    }
                    else
                    {
                        if (ordinaryAgents[j - bestAgents.Count].Rezult < _functionMin)
                        {
                            _functionMin = ordinaryAgents[j].Rezult;
                            _x1Min = ordinaryAgents[j].X1Value;
                            _x2Min = ordinaryAgents[j].X2Value;
                        }
                    }
                }
                RangeReduction();
            }
        }
        /// <summary>
        /// Функция реализующая алгоритм пчелиной колонии
        /// </summary>
        /// <param name="sizeOfColony"> Размер колонии </param>
        /// <param name="sizeBestAgents"> Количество разведчиков в потенциально лучших местах </param>
        /// <param name="sizeOrdinaryAgents"> Количество разведчиков потенциально лучших мест </param>
        /// <param name="numberNumeration"> Количество итераций </param>
        public void GetAnswer(int sizeOfColony, int sizeBestAgents, int sizeOrdinaryAgents, int numberNumeration)
        {
            CreateFirstAgents(sizeOfColony);
            SortBee();
            CreateAgents(sizeBestAgents);
            CreateAgents(sizeBestAgents ,sizeOrdinaryAgents);
            RangeReduction();
            Calculate(numberNumeration);
        }

    }
}
