using System;

namespace musique{

    /*
     * Classe qui va annaliser un signal audio en le découpant en segments
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
         * retourne la longueure de la chanson
         */
        public int songLenght(){
            return (int)(_signal.Length/_Fe);
        }

        /*
         * Calcule la transformée de fourrier du signal audio
         * C'est un calcul nécessaire pour pouvoir calculer les critères fréquenciels
         */
        public void calculerFFT(){
            
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
        public double[] ZCR(){
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
        public double[] RMS(){
             int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.RMS(_signal, i * tailleFenetre, tailleFenetre);
            }

            Stats.normalise(ref res);

            return res;

        }

        public double[] Centroid(){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.Centroid(_fft[i], (int)_Fe);
            }

            return res;
        }

        public double[] Spread(double[] centroid = null){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){

                if(centroid != null){
                    res[i] = Traitements.Spread(_fft[i], (int)_Fe, centroid[i]);
                }
                else{
                    res[i] = Traitements.Spread(_fft[i], (int)_Fe);
                }
            }

            return res;
        }

        public double[] Rollof(double gama){
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.RollOff(_fft[i], gama, (int)_Fe);
            }

            return res;
        }

        /*
         * Filtre la transformée de fourrier pour ne garder que les 
         * fréquences entre freq_debut et freq_fin
         */
        public void filtrePasseBande(double freq_debut, double freq_fin){

            double pas = (double)Fe/(2*_fft[0].Length);

            for(int i = 0; i < _fft.Length; i++){
                for(int j = 0; j < _fft[i].Length;j++){
                    if(pas * j < freq_debut || pas * j > freq_fin){
                        _fft[i][j] = 0;
                    }
                }
            }
        }

        /*
         * Filtre la transformée de fourrier pour ne garder que les 
         * fréquences avant freq_debut et après freq_fin
         */
        public void filtreCoupeBande(double freq_debut, double freq_fin){

            double pas = (double)Fe/(2*_fft[0].Length);

            for(int i = 0; i < _fft.Length; i++){
                for(int j = 0; j < _fft[i].Length;j++){
                    if(pas * j > freq_debut && pas * j < freq_fin){
                        _fft[i][j] = 0;
                    }
                }
            }
        }

        /*
         * Récupère la transformé de fourrier de la n-ième fenetre
         */
        public double[] getFFT(int n){
            return (double[])_fft[n].Clone();
        }

        /*
         * Calcule le root mean square du signal à partir de la fft
         */
         public double[] FFTrms(){
            
            int tailleFenetre = (int)(_Fe * _fenetre);
            int N = _signal.Length / tailleFenetre;

            double[] res = new double[N];

            for(int i = 0; i < N ; i++){
                res[i] = Traitements.FFTrms(_fft[i]);
            }

            Stats.normalise(ref res);

            return res;
         }

    }
}