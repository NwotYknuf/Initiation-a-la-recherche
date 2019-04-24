using System;
using CSCore;
using CSCore.Codecs;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace musique{

    class Program{        

        
        static void Main(string[] args){

            string dossier = "D:/MusiqueTriée/";
            string sortie = "./output/";

            string[] fichiers = null;
            fichiers = Directory.GetFiles(dossier);
            
            StreamWriter s = new StreamWriter(sortie + "stats");
            
            Parallel.ForEach(
                Directory.EnumerateFiles(dossier, "*.*", SearchOption.AllDirectories), 
                new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                (fichier) => {
                
                if(
                    Path.GetExtension(fichier) == ".mp3" || 
                    Path.GetExtension(fichier) == ".flac" ||
                    Path.GetExtension(fichier) == ".wav"){

                    string id = SongLoadder.getArtist(fichier) + " - " + SongLoadder.getTitle(fichier);

                    Console.WriteLine("Analyse de : {0}", id);

                    double[] signal;
                    double Fe;

                    SongLoadder.chargeChanson(fichier, out Fe, out signal);

                    Annaliseur ana = new Annaliseur(1.0, Fe, signal);
                    ana.calculeFFT();

                    double[] rms = ana.RMS();
                    double[] zrc = ana.ZCR();
                    double[] rolloff = ana.Rollof(0.85);
                    double[] centroid = ana.Centroid();
                    double[] spread = ana.Spread(centroid);

                    s.WriteLine("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                        id,
                        Stats.mediane(rms), Stats.ecart_type(rms),
                        Stats.mediane(zrc), Stats.ecart_type(zrc),
                        Stats.mediane(centroid), Stats.ecart_type(centroid),
                        Stats.mediane(spread), Stats.ecart_type(spread),
                        ana.songLenght()
                        );
                }
            });
            
            s.Close();
        }
    }
}
