using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    abstract class Layer
    {
        // поля
        // модификатор protected стоят для внутрииерархии
        protected string name_Layer;                    // имя слоя
        string pathDirWeights;                          // путь к каталогу с весами
        string pathFileWeights;                         // путь к файлу с весами
        protected int numofneurons;                     // число нейронов
        protected int numofprevneurons;                 // число нейронов предыдущ слоя
        protected const double learningrate = 0.9;     // скорость обновления 
        protected const double momentum = 0.1;          // момент
        protected double[,] lastdeltaweights;           // веса
        protected Neuron[] neurons;                     // массив нейронов

        // свойства
        public double[] Data                           // передача входных данных на нейронный слой
        {
            set
            {
                for (int i = 0; i < numofneurons; i++)
                {
                    neurons[i].Activator(value);
                }
            }
        }

        // конструктор
        protected Layer(int non, int nopn, NeuronType nt, string nm_Layer)
        {
            numofneurons = non;
            numofprevneurons= nopn;
            neurons = new Neuron[non];
            name_Layer = nm_Layer;
            pathDirWeights = AppDomain.CurrentDomain.BaseDirectory + "memory\\";
            pathFileWeights = pathDirWeights + name_Layer + "_memory.csv";

            lastdeltaweights = new double[non, nopn + 1];
            double[,] Weights; // временный массив синаптических весов текущего слоя

            if (File.Exists(pathFileWeights))
                Weights = WeightsInitialize(MemoryMode.GET, pathFileWeights);

            else
            {
                Directory.CreateDirectory(pathDirWeights);
                Weights = WeightsInitialize(MemoryMode.INIT, pathFileWeights);
            }

            for (int i = 0; i < non; i++)
            {
                double[] tmp_weights = new double[nopn + 1];
                
                for (int j = 0; j < nopn + 1; j++)
                {
                    tmp_weights[j] = Weights[i, j];
                }
                
                neurons[i] = new Neuron(tmp_weights, nt);
            }
        }

        // метод работы с массивом синаптических весов слоя
        public double[,] WeightsInitialize(MemoryMode mm, string path)
        {
            char[] delim = new char[] { ';', ' ' };
            string[] tmpStrWeights;
            double[,] weights = new double[numofneurons, numofprevneurons + 1];

            switch (mm)
            {
                case MemoryMode.GET:
                    
                    tmpStrWeights = File.ReadAllLines(path);
                    string[] memory_element;
                    int rows_in_file = Math.Min(numofneurons, tmpStrWeights.Length);
                    
                    for (int i = 0; i < rows_in_file; i++)
                    {
                        memory_element = tmpStrWeights[i].Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        int cols = Math.Min(numofprevneurons + 1, memory_element.Length);
                        for (int j = 0; j < cols; j++)
                        {
                            weights[i, j] = double.Parse(memory_element[j].Replace(',', '.'),
                                System.Globalization.CultureInfo.InvariantCulture);
                        }
                    }

                    break;

                case MemoryMode.SET:
                    
                    tmpStrWeights = new string[numofneurons];
                    
                    if (!File.Exists(path))
                    {
                        MessageBox.Show("Файл не найден\nпосле нажатия ОК будет создан отсутствующий файл",
                            "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    for (int i = 0; i < numofneurons; i++)
                    {
                        double[] w = neurons[i].Weights;
                        string[] parts = new string[w.Length];
                        for (int j = 0; j < w.Length; j++)
                            parts[j] = w[j].ToString(System.Globalization.CultureInfo.InvariantCulture);
                        tmpStrWeights[i] = string.Join(";", parts);
                    }
                    
                    File.WriteAllLines(path, tmpStrWeights);
                    
                    break;

                case MemoryMode.INIT:

                    MessageBox.Show("Файл не найден\nПосле нажатия ОК будет создан отсутствующий файл",
                        "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    Random random = new Random();

                    double tmpRatio; // коэффициент
                    double tmpShift; // смещение
                    double[] tmpArr = new double[numofprevneurons + 1];
                    tmpStrWeights = new string[numofneurons];

                    for (int i = 0; i < numofneurons; i++)
                    {
                        double mean = 0;

                        for (int j = 0; j < numofprevneurons + 1; j++)
                            tmpArr[j] = 0.02 * random.NextDouble() - 0.01;

                        double N = numofprevneurons;

                        for (int j = 1; j < numofprevneurons + 1; j++)
                            mean += tmpArr[j];

                        mean /= N;
                        double disp = 0;

                        for (int j = 1; j < numofprevneurons + 1; j++)
                            disp += (tmpArr[j] - mean) * (tmpArr[j] - mean);

                        disp /= N;
                        disp = Math.Sqrt(disp);
                        double disp_target = Math.Sqrt(2.0 / (numofneurons + numofprevneurons));

                        for (int j = 1; j < numofprevneurons + 1; j++)
                            tmpArr[j] = (tmpArr[j] - mean) / disp * disp_target;

                        for (int j = 0; j < numofprevneurons + 1; j++)
                            weights[i, j] = tmpArr[j];

                        tmpStrWeights[i] = string.Join(";", tmpArr.Select(v => v.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                    }
                    File.WriteAllLines(path, tmpStrWeights);
                    break;

                default:
                    break;
            }
            return weights;
        }
        abstract public void Recognize(Network net, Layer nextLayer);   // для прямых проходов
        abstract public double[] BackwardPass(double[] stuff);          // и обратных
    }
}
