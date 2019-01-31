
using CSCore;
using CSCore.Codecs;

namespace musique{

    public class Analyzer{

        private double _sampleRate;
        private double _timeWindow; /* in seconds */

        private double[] _samples;
        private double[][] _fft;

        public int sampleRate{
            get{ return (int)_sampleRate; }
        }

        public Analyzer(double timeWindow){
            _timeWindow = timeWindow;
        }

        public void loadSong(string path){
            IWaveSource source = CodecFactory.Instance.GetCodec(path);
            ISampleSource  sample = source.ToSampleSource();
            _sampleRate = source.WaveFormat.SampleRate;
            sample = sample.ToMono();
            int size = (int)sample.Length;
            float[] values = new float[size];
            sample.Read(values, 0, size);
            _samples = new double[size];
            for(int i = 0; i < size;i++){
                _samples[i] = (double)values[i];
            }


        }

        public void computeFFT(){
            
            int stepSize = (int)(_sampleRate * _timeWindow);
            int N = _samples.Length / stepSize;

            _fft = new double[N][];

            for(int i = 0; i < N ; i++){
                _fft[i] = Processing.computeFFT(_samples, i * stepSize, stepSize);
            }
        }

        public double[] computeZCR(){
            int stepSize = (int)(_sampleRate * _timeWindow);
            int N = _samples.Length / stepSize;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Processing.ZeroCrossingRate(_samples, i * stepSize, stepSize);
            }

            return res;

        }

         public double[] computeRMS(){
            int stepSize = (int)(_sampleRate * _timeWindow);
            int N = _samples.Length / stepSize;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Processing.RootMeanSquare(_samples, i * stepSize, stepSize);
            }

            return res;

        }

        public double[] computeSpectralCentroid(){
            int stepSize = (int)(_sampleRate * _timeWindow);
            int N = _samples.Length / stepSize;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Processing.SpectralCentroid(_fft[i], (int)_sampleRate);
            }

            return res;
        }

    }
}