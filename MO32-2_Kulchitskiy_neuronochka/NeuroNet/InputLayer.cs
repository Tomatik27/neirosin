using System;
using System.IO;

namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    class InputLayer
    {

        private double[,] trainset; // 100 изображений в обучающей выборке
        private double[,] testset;  // 10 в тестовой

        public double[,] Trainset { get => trainset; }
        public double[,] Testnset { get => testset; }

        // конструктор
        public InputLayer(NetworkMode nm)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string[] tmpArrStr;
            string[] tmpStr;

            switch (nm)
            {
                case NetworkMode.Train:
                    tmpArrStr = File.ReadAllLines(path + "train.txt");
                    trainset = new double[tmpArrStr.Length, 16];

                    for (int i = 0; i < tmpArrStr.Length; i++)
                    {
                        tmpStr = tmpArrStr[i].Split(' ');

                        for (int j = 0; j < 16; j++)
                        {
                            trainset[i, j] = double.Parse(tmpStr[j]);
                        }
                    }
                    break;

                case NetworkMode.Test:
                    tmpArrStr = File.ReadAllLines(path + "test.txt");
                    testset = new double[tmpArrStr.Length, 16];


                    break;
                case NetworkMode.Demo:
                    break;
                default:
                    break;
            }

        }

    }
}
