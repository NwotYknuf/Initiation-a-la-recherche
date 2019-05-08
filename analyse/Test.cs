using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace musique{

    class Test{        
/*
        static void Main(string[] args){

            string dossier = @"D:\Musique";

            Parallel.ForEach(
                Directory.EnumerateFiles(dossier, "*.*", SearchOption.AllDirectories), 
                new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                (fichier) => {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            
                     if(
                    Path.GetExtension(fichier) == ".mp3" || 
                    Path.GetExtension(fichier) == ".flac" ||
                    Path.GetExtension(fichier) == ".wav"){

                    //Recuperrer le signal

                    double[] signal;
                    double Fe;

                    SongLoadder.chargerChanson(fichier, out Fe, out signal);
                    string id = SongLoadder.getArtist(fichier) + " - " + SongLoadder.getTitle(fichier);


                    double fenetre = 0.005;

                    Annaliseur low = new Annaliseur(fenetre, Fe, signal);
                    Annaliseur mid = new Annaliseur(fenetre, Fe, signal);
                    Annaliseur high = new Annaliseur(fenetre, Fe, signal);
                    low.calculerFFT();
                    mid.calculerFFT();
                    high.calculerFFT();

                    //Couper les fréquences
                    low.filtreCoupeBande(300.0,25000.0);

                    mid.filtreCoupeBande(0.0,300.0);
                    mid.filtreCoupeBande(2000.0,25000.0);

                    high.filtreCoupeBande(0.0, 2000.0 );

                    //Calculer l'énergie du signal sur des fenêtres de 10 ms
                    double[] low_energy = low.FFTrms();
                    double[] mid_energy = mid.FFTrms();
                    double[] high_energy = high.FFTrms();
                
                    //Detecter la présence de pics d'énergie
                    int N = (int)(1.0/fenetre);

                    int duree =(int)(10/fenetre);
                    int debut = (int) (60/fenetre);

                    try{
                        StreamWriter sw = new StreamWriter("../data/beat/" + Path.GetFileNameWithoutExtension(fichier));                

                        for(int i = debut + N/2; i < debut + duree + N/2 ;i++){
                            //moyenne d'énergie sur une seconde
                            double low_mean = Stats.moyenne(low_energy.Skip(i-N/2).Take(N).ToArray());
                            double mid_mean = Stats.moyenne(mid_energy.Skip(i-N/2).Take(N).ToArray());
                            double high_mean = Stats.moyenne(high_energy.Skip(i-N/2).Take(N).ToArray());
                            double low_var = Stats.ecart_type(low_energy.Skip(i-N/2).Take(N).ToArray());
                            double mid_var = Stats.ecart_type(mid_energy.Skip(i-N/2).Take(N).ToArray());
                            double high_var = Stats.ecart_type(high_energy.Skip(i-N/2).Take(N).ToArray());

                            sw.WriteLine(
                                "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", 
                                low_energy[i], low_mean, low_var,
                                mid_energy[i], mid_mean, mid_var,
                                high_energy[i], high_mean,high_var);
                        }

                        sw.Close();

                    }
                    catch{
                        Console.Write(fichier + " trop court !");
                    }

                    }

            });

        }
        
     */   
    }
}