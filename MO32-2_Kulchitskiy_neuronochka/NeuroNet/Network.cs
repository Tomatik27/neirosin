using System;
using System.IO;

namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    class Network
    {
        // слои
        private InputLayer input_layer = null;
        private HiddenLayer hidden_layer1 = new HiddenLayer(70, 15, NeuronType.Hidden, nameof(hidden_layer1));
        private HiddenLayer hidden_layer2 = new HiddenLayer(34, 70, NeuronType.Hidden, nameof(hidden_layer2));
        private OutputLayer output_layer = new OutputLayer(10, 34, NeuronType.Output, nameof(output_layer));


        private double[] fact = new double[10];  // фактический вывод
        private double[] e_error_avr;            // среднее значение энергии ошибки

        // свойства
        public double[] Fact { get => fact; }   // геттер вывода
        public double[] E_error_avr { get => e_error_avr; set => e_error_avr = value; }

        // конструктор
        public Network() { }

        // прямой проход НС
        public void ForwardPass(double[] netInput)
        {
            hidden_layer1.Data = netInput;
            hidden_layer1.Recognize(null, hidden_layer2);
            hidden_layer2.Recognize(null, output_layer);
            output_layer.Recognize(this, null);
        }

        // само обучение; возвращает имя папки, куда сохранены новые веса
        public string Train(Network net)
        {
            input_layer = new InputLayer(NetworkMode.Train);

            int epoches = 20;        // к-во эпох
            double tmpSumError;     // сумма ошибок
            double[] errors;        // вектор сингала ошибки вх слоя
            double[] temp_gsums1;   // вектор градиента 1 скр слоя
            double[] temp_gsums2;   // вектор градиента 2 скр слоя

            e_error_avr = new double[epoches];

            for (int k = 0; k < epoches; k++) // перебор эпох
            {
                e_error_avr[k] = 0;
                input_layer.Shuffling_Array_Rows(input_layer.Trainset);
                for (int i = 0; i < input_layer.Trainset.GetLength(0); i++)
                {
                    double[] tmpTrain = new double[15];  // обучабщий образ
                    for (int j = 0; j < tmpTrain.Length; j++)
                        tmpTrain[j] = input_layer.Trainset[i, j + 1]; // пиксели без метки цифры

                    ForwardPass(tmpTrain);

                    // вычисление ошибок по итерации
                    tmpSumError = 0;
                    errors = new double[fact.Length];
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == input_layer.Trainset[i, 0])
                            errors[x] = 1.0 - fact[x];
                        else
                            errors[x] = 0.0 - net.fact[x];

                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length; // суммарное значение ошибки k-ой эпохи

                    // обратное распространение ошибки и коррекция весов
                    temp_gsums1 = output_layer.BackwardPass(errors);    // градиенты выходного слоя
                    temp_gsums2 = hidden_layer2.BackwardPass(temp_gsums1);
                    hidden_layer1.BackwardPass(temp_gsums2);
                }
            }

            input_layer = null;

            // запись скорректированных весов в ОТДЕЛЬНУЮ папку memory_<дата-время>,
            // чтобы не затирать текущую модель в memory\
            string memoryDir = "memory_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string pathDir = AppDomain.CurrentDomain.BaseDirectory + memoryDir + "\\";
            Directory.CreateDirectory(pathDir);

            hidden_layer1.WeightsInitialize(MemoryMode.SET, pathDir + nameof(hidden_layer1) + "_memory.csv");
            hidden_layer2.WeightsInitialize(MemoryMode.SET, pathDir + nameof(hidden_layer2) + "_memory.csv");
            output_layer.WeightsInitialize(MemoryMode.SET, pathDir + nameof(output_layer) + "_memory.csv");

            return memoryDir;
        }


        public void Test()
        {
            input_layer = new InputLayer(NetworkMode.Test);

            int epoches = 3;        // к-во эпох
            double tmpSumError;     // сумма ошибок
            double[] errors;        // вектор сингала ошибки вх слоя

            e_error_avr = new double[epoches];

            for (int k = 0; k < epoches; k++) // перебор эпох
            {
                e_error_avr[k] = 0;
                input_layer.Shuffling_Array_Rows(input_layer.Testset);
                for (int i = 0; i < input_layer.Testset.GetLength(0); i++)
                {
                    double[] tmpTest = new double[15];  // обучабщий образ
                    for (int j = 0; j < tmpTest.Length; j++)
                        tmpTest[j] = input_layer.Testset[i, j + 1]; // пиксели без метки цифры

                    ForwardPass(tmpTest);

                    // вычисление ошибок по итерации
                    tmpSumError = 0;
                    errors = new double[fact.Length];
                    for (int x = 0; x < errors.Length; x++)
                    {
                        if (x == input_layer.Testset[i, 0])
                            errors[x] = 1.0 - fact[x];
                        else
                            errors[x] = 0.0 - fact[x];

                        tmpSumError += errors[x] * errors[x] / 2;
                    }
                    e_error_avr[k] += tmpSumError / errors.Length; // суммарное значение ошибки k-ой эпохи
                }
                e_error_avr[k] /= input_layer.Testset.GetLength(0);
            }

            input_layer = null;
        }


    }
}
