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

            string dossier = "../data/Musiques";    // Emplacement de la musiques à analyser
            string sortie = "../data/data.json";    // Emplacement du fichier résultat

            string json = "{";
            string[] fichiers = null;
            fichiers = Directory.GetFiles(dossier);
            StreamWriter sw = new StreamWriter(sortie);
            
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

                    DataSong ds = new DataSong();
                    double[] signal;
                    double Fe;

                    // Chargement et analyse de la chanson
                    SongLoadder.chargerChanson(fichier, out Fe, out signal);
                    Annaliseur annaliseur = new Annaliseur(1.0, Fe, signal);
                    annaliseur.calculerFFT();

                    // Récupération des données
                    double[] rms = annaliseur.RMS();
                    double[] zrc = annaliseur.ZCR();
                    double[] rolloff = annaliseur.Rollof(0.85); //!\\ Le rolloff n'est pas utilisé dans le JSON, il est calculé pour rien j'ai l'impression
                    double[] centroid = annaliseur.Centroid();
                    double[] spread = annaliseur.Spread(centroid);

                    // Initialisation de l'objet JSON
                    ds.id = id;
                    ds.median_rms = Stats.mediane(rms);
                    ds.ecarType_rms = Stats.ecart_type(rms);
                    ds.median_zrc = Stats.mediane(zrc);
                    ds.ecarType_zrc = Stats.ecart_type(zrc);
                    ds.median_centroid = Stats.mediane(centroid);
                    ds.ecarType_centroid = Stats.ecart_type(centroid);
                    ds.median_spread = Stats.mediane(spread);
                    ds.ecarType_spread = Stats.ecart_type(spread);
                    ds.songLenght = annaliseur.songLenght();

                    // Serialisation
                    MemoryStream stream = new MemoryStream();  
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(DataSong));
                    ser.WriteObject(stream, ds);

                    // Afficher
                    stream.Position = 0;  
                    StreamReader sr = new StreamReader(stream);
                    json += "\"" + ds.id + "\" : " + sr.ReadToEnd() + ",\n";
                }
            });
            sw.WriteLine(json + "\"\":\"\"}");
            sw.Close();
        }
    }
}
