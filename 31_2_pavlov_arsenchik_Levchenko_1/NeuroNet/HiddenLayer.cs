using System.Data.Common;

namespace _31_2_pavlov_arsenchik_Levchenko_1.NeuroNet
{
    class HiddenLayer : Layer
    {
        public HiddenLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }


        public override void Recognize(Network net, Layer nextlayer)
        {
            double[] hidden_out = new double[numofneurons];
            for (int i = 0; i < numofneurons; i++)
                hidden_out[i] = neurons[i].output;
            nextlayer.Data = hidden_out;
        }

        public override double[] BackwardPass(double[] gr_sum)
        {
            return gr_sum;
        }

    }
}

