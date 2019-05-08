using System;
using CSCore;
using CSCore.Codecs;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;

namespace musique{

    class Program{

        static void Main(string[] args){

            string dossier = "D:/Musique";    // Emplacement de la musiques à analyser
            string sortie = "../data/test/";    // Emplacement du fichier résultat

            Parallel.ForEach(
                Directory.EnumerateFiles(dossier, "*.*", SearchOption.AllDirectories), 
                new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                (fichier) => {

                StreamWriter sw = new StreamWriter(sortie + Path.GetFileNameWithoutExtension(fichier));            
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

                if(
                    Path.GetExtension(fichier) == ".mp3" || 
                    Path.GetExtension(fichier) == ".flac" ||
                    Path.GetExtension(fichier) == ".wav"){

                    string id = SongLoadder.getArtist(fichier) + " - " + SongLoadder.getTitle(fichier);
                    Console.WriteLine("Analyse de : {0}", id);

                    DataSong ds = new DataSong();
                    double[] signal;
                    double Fe;

                    // Chargement et analyse de la chanson
                    SongLoadder.chargerChanson(fichier, out Fe, out signal);
                    Annaliseur annaliseur = new Annaliseur(0.5, Fe, signal);
                    annaliseur.calculerFFT();

                    // Récupération des données
                    double[] rms = annaliseur.RMS();
                    double[] zrc = annaliseur.ZCR();
                    double[] centroid = annaliseur.Centroid();
                    double[] spread = annaliseur.Spread(centroid);

                    for(int i = 0; i < rms.Length; i++){
                        string s = id.ToString() + " " + i.ToString() + "/" + rms.Length.ToString();
                        sw.WriteLine("{0}\t{1}\t{2}\t{3}\t", rms[i], centroid[i], spread[i], zrc[i]);
                    }

                    sw.Close();

                }
            });


            
        }
    }
    
}
