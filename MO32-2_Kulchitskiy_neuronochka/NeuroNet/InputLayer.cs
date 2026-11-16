using System;
using System.Collections.Generic;
using System.IO;

namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    class InputLayer
    {

        private double[,] trainset; // изображения в обучающей выборке
        private double[,] testset;  // изображения в тестовой выборке

        public double[,] Trainset { get => trainset; }
        public double[,] Testnset { get => testset; }

        // конструктор
        public InputLayer(NetworkMode nm)
        {
            switch (nm)
            {
                case NetworkMode.Train:
                    trainset = LoadDataset("train.txt");
                    break;
                case NetworkMode.Test:
                    testset = LoadDataset("test.txt");
                    break;
                case NetworkMode.Demo:
                    break;
                default:
                    break;
            }

        }

        // чтение выборки из файла: строка вида «цифра п1 ... п15»;
        // пустые и битые строки пропускаются, чтобы обучение не падало
        private double[,] LoadDataset(string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + fileName;

            if (!File.Exists(path))
            {
                System.Windows.Forms.MessageBox.Show("Файл " + fileName + " не найден\nСначала сохраните образцы кнопками «Сохранить образец»",
                    "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                return new double[0, 16];
            }

            List<double[]> rows = new List<double[]>();
            string[] tmpArrStr = File.ReadAllLines(path);

            for (int i = 0; i < tmpArrStr.Length; i++)
            {
                string line = tmpArrStr[i].Trim();
                if (line.Length == 0)
                    continue;

                string[] tmpStr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tmpStr.Length != 16)
                    continue; // строка не по формату «цифра + 15 пикселей»

                double[] row = new double[16];
                bool valid = true;

                for (int j = 0; j < 16; j++)
                {
                    if (!double.TryParse(tmpStr[j].Replace(',', '.'),
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out row[j]))
                    {
                        valid = false;
                        break;
                    }
                }

                if (valid)
                    rows.Add(row);
            }

            double[,] dataset = new double[rows.Count, 16];
            for (int i = 0; i < rows.Count; i++)
                for (int j = 0; j < 16; j++)
                    dataset[i, j] = rows[i][j];

            return dataset;
        }

        // перетасовка строк массива методом Фиишера-Йетса
        public void Shuffling_Array_Rows(double[,] arr)
        {
            int numofrows = arr.GetLength(0); // к-во строк
            int numofcols = arr.GetLength(1); // к-во столбцов
            if (numofrows < 2)
                return;

            Random random = new Random();
            for (int i = numofrows - 1; i > 0; i--)
            {
                int j = random.Next(i + 1); // номер случайно выбранной строки
                for (int c = 0; c < numofcols; c++) // меняем строки i и j местами
                {
                    double tmp = arr[i, c];
                    arr[i, c] = arr[j, c];
                    arr[j, c] = tmp;
                }
            }
        }

    }
}
