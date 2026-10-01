using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _31_2_pavlov_arsenchik_Levchenko_1.NeuroNet
{
    internal class Neuron
    {
        // поля
        private NeuronType type; //тип нейрона 
        private double[] weights;//вес
        private double[] inputs;// входы
        private double output;//выход     (софт макс функция )
        private double derivative;//производная
    }
    


// Это просто сборник инструментов. Слово static означает, 
// что нам не нужно создавать "копию" этого класса через new.
public static class ActivationFunctions
    {
        // Это коэффициент затухания. 
        // Если число отрицательное, мы уменьшим его в 100 раз (умножим на 0.01).
        private const double Alpha = 0.01;

        
        /// Главная функция: обрабатывает ОДНО число
        
        public static double LeakyRelu(double x)
        {
            // ЕСЛИ число больше нуля
            if (x > 0)
            {
                // Возвращаем его как есть, без изменений
                return x;
            }
            // ИНАЧЕ (если число ноль или отрицательное)
            else
            {
                // Ослабляем его: умножаем на 0.01
                return Alpha * x;
            }
        }

       
        /// Производная: нужна ТОЛЬКО во время обучения нейросети
        
        public static double LeakyReluDerivative(double x)
        {
            // ЕСЛИ число было положительным
            if (x > 0)
            {
                // Возвращаем единицу (ошибка передается на 100%)
                return 1.0;
            }
            // ИНАЧЕ (если число было отрицательным)
            else
            {
                // Возвращаем наш коэффициент (ошибка передается на 1%)
                return Alpha;
            }
        }

      
        /// Автомат: обрабатывает МАССИВ чисел (целый слой нейронов)
        
        public static double[] ApplyToLayer(double[] layerInput)
        {
            // 1. Узнаем, сколько чисел нам дали на вход (например, 71 или 35)
            int numberOfNeurons = layerInput.Length;

            // 2. Создаем новый пустой массив такого же размера для результатов
            double[] output = new double[numberOfNeurons];

            // 3. По очереди берем каждое число из входного массива
            for (int i = 0; i < numberOfNeurons; i++)
            {
                double rawValue = layerInput[i]; // Взяли сырое число

                // Пропускаем его через нашу функцию LeakyRelu (которая написана выше)
                double activatedValue = LeakyRelu(rawValue);

                output[i] = activatedValue; // Записали готовый результат в новый массив
            }

            // 4. Отдаем готовый массив измененных чисел обратно
            return output;
        }
    }


}
