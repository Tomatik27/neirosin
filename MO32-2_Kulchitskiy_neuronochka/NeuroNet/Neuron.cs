using static System.Math;

namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    class Neuron
    {

        private NeuronType type;  //тип нейрона
        private double[] weights; //веса
        private double[] inputs;  //входы
        private double output;    //выходы
        private double derivative;//производная

        // константы для функции активации
        private double a;

        // свойства

        public double[] Weights { get => weights; set => weights = value; }
        public double[] Inputs { get => inputs; set => inputs = value; }
        public double Output { get => output; }
        public double Derivative { get => derivative; }


        public Neuron(double[] memoryWeights, NeuronType typeNeorun)
        {
            type = typeNeorun;
            weights = memoryWeights;
        }

        public void Activator(double[] i)
        {
            inputs = i;

            double sum = weights[0];

            for (int j = 0; j < inputs.Length; j++)
            {
                sum += inputs[j] * weights[j + 1];
            }

            switch (type)
            {
                case NeuronType.Hidden:
                    output = Logistic(sum);
                    derivative = Logistic_Derivativator(sum);
                    break;
                case NeuronType.Output:
                    output = Exp(sum);
                    break;
            }
        }

        private double Logistic(double sum)
        {
            if (sum >= 0)
            {
                return 1 / (1 + (System.Math.Exp(-sum)));
            }
            else
            {
                return System.Math.Exp(sum) / (1 + (System.Math.Exp(sum)));

            }

        }

        private double Logistic_Derivativator(double sum)
        {
            double s = Logistic(sum);
            return s * (1.0 - s);
        }

    }
}