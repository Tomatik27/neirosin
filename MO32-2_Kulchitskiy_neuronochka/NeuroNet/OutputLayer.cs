using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    class OutputLayer : Layer
    {
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double e_sum = 0;
            for (int i = 0; i < neurons.Length; i++)
                e_sum += neurons[i].Output;

            for (int i = 0; i < neurons.Length; i++)
                net.Fact[i] = neurons[i].Output / e_sum;
        }

        public override double[] BackwardPass(double[] errors)
        {
            double[] gr_sum = new double[numofprevneurons];
            return gr_sum;
        }

    }
}
