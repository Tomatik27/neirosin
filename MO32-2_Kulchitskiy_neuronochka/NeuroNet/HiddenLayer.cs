using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO32_2_Kulchitskiy_neuronochka.NeuroNet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }

        public override void Recognize(Network net, Layer nextLayer)
        {
            double[] hidden_out = new double[numofneurons];
            for (int i = 0; i < numofneurons; i++)
                hidden_out[i] = neurons[i].Output;

            nextLayer.Data = hidden_out;
        }

        public override double[] BackwardPass(double[] gr_sums)
        {
            double[] gr_sum = new double[numofprevneurons];
            return gr_sum;
        }

    }
}
