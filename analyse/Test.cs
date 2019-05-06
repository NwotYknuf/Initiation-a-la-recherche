using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace musique{

    class Test{        

        static void Main(string[] args){

            Parallel.ForEach(
                Directory.EnumerateFiles(@"D:\Musique", "*.*", SearchOption.AllDirectories), 
                new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                (fichier) => {
            
                if(
                    Path.GetExtension(fichier) == ".mp3" || 
                    Path.GetExtension(fichier) == ".flac" ||
                    Path.GetExtension(fichier) == ".wav"){

                    //Recuperrer le signal

                    double[] signal;
                    double Fe;

                    SongLoadder.chargerChanson(fichier, out Fe, out signal);
                    string id = SongLoadder.getArtist(fichier) + " - " + SongLoadder.getTitle(fichier);

                    StreamWriter sw = new StreamWriter("../data/beat/" + Path.GetFileNameWithoutExtension(fichier));                

                    Annaliseur annaliseur = new Annaliseur(0.01, Fe, signal);
                    annaliseur.calculerFFT();

                    //isoler les fréquences basses

                    annaliseur.filtreCoupeBande(0.0, 25.0);
                    annaliseur.filtreCoupeBande(75.0,32000.0);

                    //Calculer l'énergie du signal sur des fenêtres de 10 ms
                    double[] rms = annaliseur.FFTrms();
                
                    //Detecter la présence de pics d'énergie

                    int n_echantillons = 50;
                    double seuil_ecart_type = 0.1;
                    double seuil_energie = 0.6;

                    for(int i = n_echantillons/2; i < rms.Length - n_echantillons/2;i++){

                        //moyenne d'énergie sur une seconde
                        double moyenne = Stats.moyenne(rms.Skip(i-n_echantillons/2).Take(n_echantillons).ToArray());
                        //ecart_type d'énergie sur une seconde
                        double ecart_type = Stats.ecart_type(rms.Skip(i-n_echantillons/2).Take(n_echantillons).ToArray());

                        //Si l'écart type est inférieur au seuil on ignore la section (ecart type faible = pas de pic)
                        if(ecart_type > seuil_ecart_type){
                            sw.WriteLine("{0}\t{1}", 
                                rms[i].ToString(System.Globalization.CultureInfo.InvariantCulture), 
                                rms[i] > (1 + seuil_energie) * moyenne ? 1:0);
                        }
                        else{
                            sw.WriteLine("{0}\t0", rms[i].ToString(System.Globalization.CultureInfo.InvariantCulture));
                        }

                    }

                    sw.Close();
                }

            });

        

        }
        
        
    }
}