
using CSCore;
using CSCore.Codecs;

namespace musique{

    public class Annaliseur{

        private double _Fe; /* Fréquence d'échantillonage */
        private double _fenetre; /* Fenetre de temps en secondes */

        private double[] _signal;
        private double[][] _fft;

        public int Fe{
            get{ return (int)_Fe; }
        }

        public Annaliseur(double fenetre){
            _fenetre = fenetre;
        }

        public void chargeChanson(string chemin){
            IWaveSource source = CodecFactory.Instance.GetCodec(chemin);
            ISampleSource  signal = source.ToSampleSource();
            _Fe = source.WaveFormat.SampleRate;
            signal = signal.ToMono();
            int size = (int)signal.Length;
            float[] valeurs = new float[size];
            signal.Read(valeurs, 0, size);
            _signal = new double[size];
            for(int i = 0; i < size;i++){
                _signal[i] = (double)valeurs[i];
            }


        }

        public void calculFFT(){
            
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            _fft = new double[N][];

            for(int i = 0; i < N ; i++){
                _fft[i] = Traitements.FFT(_signal, i * tailleFenetre, tailleFenetre);
            }
        }

        public double[] calculZCR(){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.ZRC(_signal, i * tailleFenetre, tailleFenetre);
            }

            return res;

        }

         public double[] computeRMS(){
             int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.RMS(_signal, i * tailleFenetre, tailleFenetre);
            }

            return res;

        }

        public double[] calculCentroid(){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.Centroid(_fft[i], (int)_Fe);
            }

            return res;
        }

        public double[] computeRollOff(double gama){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.RollOff(_fft[i], gama, (int)_Fe);
            }

            return res;
        }

    }
}