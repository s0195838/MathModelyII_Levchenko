using System.Data.Common;

namespace _31_2_pavlov_arsenchik_Levchenko_1.NeuroNet
{
    class OutputLayer : Layer
    {
        public OutputLayer(int non, int nopn, NeuronType nt, string type) : base(non, nopn, nt, type) { }


        public override void Recognize(Network net, Layer nextlayer)
        {
            double e_sum = 0;
            for (int i = 1; i < neurons.Length; i++)
                e_sum += neurons[i].output;
            for (int i = 1; i < neurons.Length; i++)
                e_sum += neurons[i].output;
        }

        public override double[] BackwardPass(double[] gr_sum)
        {
            return gr_sum;
        }

    }
}
