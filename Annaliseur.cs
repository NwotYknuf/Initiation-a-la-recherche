
namespace musique{

    /*
     * Classe qui va annaliser un signal audio en le découpant en seguements
     */
    public class Annaliseur{

        private double _Fe; /* Fréquence d'échantillonage */
        private double _fenetre; /* Fenetre de temps en secondes */

        private double[] _signal;
        private double[][] _fft;

        public int Fe{
            get{ return (int)_Fe; }
        }
        /*
         * fenetre : longeure de la fenetre d'analyse en secondes
         * Fe : fréquence d'echantillonage en Hz
         * signal : tableau contenant les échantillons du signal audio
         */
        public Annaliseur(double fenetre, double Fe, double[] signal){
            _fenetre = fenetre;
            _Fe = Fe;
            _signal = signal;
        }

        /*
         * Calcule la transformée de fourrier du signal audio
         * C'est un calcul nécessaire pour pouvoir calculer les critères fréquenciels
         */
        public void calculeFFT(){
            
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            _fft = new double[N][];

            for(int i = 0; i < N ; i++){
                _fft[i] = Traitements.FFT(_signal, i * tailleFenetre, tailleFenetre);
            }
        }

        /*
         * Calcule le Zero Crossing Rate pour chaque fenetre
         */
        public double[] calculeZCR(){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.ZRC(_signal, i * tailleFenetre, tailleFenetre);
            }

            return res;

        }


        /*
         * Calcule le Root Mean Square du signal audio
         * Les données sont normalisée entre 0 et 1
         */
        public double[] calculeRMS(){
             int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.RMS(_signal, i * tailleFenetre, tailleFenetre);
            }

            Stats.normalise(ref res);

            return res;

        }

        public double[] calculeCentroid(){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.Centroid(_fft[i], (int)_Fe);
            }

            return res;
        }

        public double[] calculeRollOff(double gama){
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