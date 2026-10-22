using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _31_2_pavlov_arsenchik_Levchenko_1.NeuroNet.Neuron;

namespace _31_2_pavlov_arsenchik_Levchenko_1.NeuroNet
{
    abstract class Layer
    {
    

        protected string name_Layer;                 // Имя слоя (например, "Hidden" или "Output"), нужно для имени файла весов
        string pathDirWeights;                       // Путь к папке "memory", где лежат файлы весов всех слоёв
        string pathFileWeights;                      // Полный путь к файлу весов именно этого слоя
        protected int numofneurons;                  // Сколько нейронов в текущем слое
        protected int numofprevneurons;              // Сколько нейронов в предыдущем слое (сколько входов у каждого нейрона)
        protected const double learningrate = 0.060; // Скорость обучения — насколько сильно менять веса при каждой ошибке
        protected const double momentum = 0.050d;    // Момент инерции — добавляет долю предыдущего изменения веса, чтобы обучение не "дёргалось"
        protected double[,] lastdeltaweights;        // Изменения весов на предыдущем шаге (нужны для momentum)
        protected Neuron[] neurons;                  // Массив объектов Neuron — сам слой по факту

        

        // Публичный доступ к массиву нейронов слоя (геттер/сеттер поверх поля neurons)
        public Neuron[] Neurons { get => neurons; set => neurons = value; }

        // Свойство "Data" — при присвоении значения (set) раздаёт входные числа по нейронам
        // и сразу считает активацию. Геттера нет — только запись.
        public double[] Data
        {
            set
            {
                // Проходим по всем нейронам слоя
                for (int i = 0; i < numofneurons; i++)
                {
                    // Берём i-е входное число, пропускаем через Leaky ReLU
                    // и записываем результат в поле output i-го нейрона
                    Neurons[i].output = ActivationFunctions.LeakyRelu(value[i]);
                }
            }
        }

        

        // Конструктор слоя.
        // non — число нейронов в ЭТОМ слое,
        // nopn — число нейронов в ПРЕДЫДУЩЕМ слое (=> число входов),
        // nt — тип нейрона (входной/скрытый/выходной),
        // nm_Layer — имя слоя для файла весов.
        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            numofneurons = non;                     // Запоминаем число нейронов текущего слоя
            numofprevneurons = nopn;                // Запоминаем число нейронов предыдущего слоя
            Neurons = new Neuron[non];              // Выделяем массив под нейроны (пока пустые)
            name_Layer = nm_Layer;                  // Запоминаем имя слоя
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\"; // Папка с весами рядом с exe
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";       // Файл весов именно этого слоя

            lastdeltaweights = new double[non, nopn + 1]; // Матрица прошлых изменений: [нейрон, вес] (+1 — это bias)

            double[,] Weights;                      // Локальная матрица весов, заполним ниже

            // Если файл весов уже есть — читаем из него
            if (File.Exists(pathFileWeights))
                Weights = WeightInitialize(MemoryMode.GET, pathFileWeights);
            else
            {
                // Иначе создаём папку (если её нет) и генерируем веса случайно
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightInitialize(MemoryMode.INIT, pathFileWeights);
            }

            // Раздаём каждому нейрону его строку весов из общей матрицы
            for (int i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1]; // Временный массив весов одного нейрона
                for (int j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j]; // Копируем j-й вес i-го нейрона
                }
                Neurons[i] = new Neuron(tmp_weights, nt); // Создаём нейрон с его весами и типом
            }
        }

        

        // Метод для чтения/записи/инициализации матрицы весов слоя.
        // mm — режим (GET — читать, SET — писать, INIT — сгенерировать случайно),
        // path — путь к файлу весов.
        public double[,] WeightInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };       // Разделители в файле: точка с запятой и пробел
            string tmpStr;                                 // Буфер для сборки строки при INIT
            string[] tmpStrWeights;                        // Массив строк файла (по одному нейрону в каждой)
            double[,] weights = new double[numofneurons, numofprevneurons + 1]; // Итоговая матрица весов

            switch (mm)
            {
                // РЕЖИМ ЧТЕНИЯ 
                case MemoryMode.GET:
                    tmpStrWeights = File.ReadAllLines(path); // Читаем все строки файла

                    string[] memory_element;                 // Сюда попадут числа из одной строки
                    for (int i = 0; i < numofneurons; i++)
                    {
                        memory_element = tmpStrWeights[i].Split(delim); // Разбиваем строку по разделителям
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            // Парсим строку в double, заменяя запятую на точку (локаль),
                            // используем InvariantCulture, чтобы не зависеть от региональных настроек
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }
                    break;

                // РЕЖИМ ЗАПИСИ 
                case MemoryMode.SET:
                    // Открываем файл для перезаписи (false — не дозаписывать, а заменить)
                    using (StreamWriter sw = new StreamWriter(path, false))
                    {
                        for (int i = 0; i < numofneurons; i++)
                        {
                            for (int j = 0; j < numofprevneurons + 1; j++)
                            {
                                // Пишем вес, заменяя запятую на точку (чтобы формат был единым)
                                sw.Write(weights[i, j].ToString().Replace(',', '.'));

                                // Между весами ставим ';', после последнего — не ставим
                                if (j != numofprevneurons) sw.Write(";");
                            }
                            sw.WriteLine(); // Переход на новую строку — следующий нейрон
                        }
                    }
                    break;

                //РЕЖИМ ИНИЦИАЛИЗАЦИИ
                case MemoryMode.INIT:
                    Random random = new Random();                  // Генератор случайных чисел
                    double tmpRatio;                                // Дисперсия строки
                    double tmpShift;                                // Среднее строки
                    double[] tmpArr = new double[numofprevneurons + 1]; // Временный массив случайных чисел
                    tmpStrWeights = new string[numofneurons];       // (объявлен, но не используется дальше)

                    for (int i = 0; i < numofneurons; i++)
                    {
                        // Заполняем строку случайными числами [0,1)
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            tmpArr[j] = random.NextDouble();
                        }
                        tmpShift = tmpArr.Average(); // Среднее по строке

                        // Считаем средний квадрат элементов (для дисперсии)
                        double[] tmpArr2 = new double[numofprevneurons + 1];
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            tmpArr2[j] = tmpArr[j] * tmpArr[j];
                        }

                        tmpRatio = tmpArr2.Average() - Math.Pow(tmpShift, 2); // Дисперсия = E[x^2] - (E[x])^2

                        // Нормализуем: вычитаем среднее и делим на дисперсию — получаем центрированные веса
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            tmpArr[j] = (tmpArr[j] - tmpShift) / tmpRatio;
                            weights[i, j] = tmpArr[j]; // Пишем в итоговую матрицу
                        }
                    }

                    tmpStr = "";                                    // Сбрасываем буфер строки
                    for (int i = 0; i < numofneurons; i++)
                    {
                        for (int j = 0; j < numofprevneurons + 1; j++)
                        {
                            tmpStr += weights[i, j].ToString() + " "; // Собираем строку: веса через пробел
                        }
                        tmpStr += "\n";                              // Конец строки — конец нейрона
                    }
                    Console.WriteLine(tmpStr);                      // Выводим для отладки
                    File.AppendAllText(path, tmpStr);                // Дописываем в файл весов
                    break;

                default:
                    break; 
            }
            return weights; // Возвращаем матрицу весов (для GET — прочитанную, для INIT — сгенерированную, для SET — ту, что передали на запись)
        }

        

        // Прямой проход: получить вход, посчитать выход и передать его следующему слою.
        // net — ссылка на всю сеть (для доступа к данным),
        // nextlayer — следующий слой, куда передаются результаты.
        abstract public void Recognize(Network net, Layer nextlayer);

        // Обратный проход: получить ошибку от следующего слоя и вернуть ошибку для предыдущего.
        // stuff — массив дельт, пришедших с предыдущего (в обратном направлении) слоя.
        abstract public double[] BackwardPass(double[] stuff);
    }
}
