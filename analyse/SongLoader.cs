
using CSCore;
using CSCore.Codecs;
using TagLib;

namespace musique{

    public static class SongLoadder{

        public static void chargeChanson(string chemin, out double Fe, out double[] res){
            IWaveSource source = CodecFactory.Instance.GetCodec(chemin);
            ISampleSource  signal = source.ToSampleSource();
            Fe = source.WaveFormat.SampleRate;
            signal = signal.ToMono();
            int size = (int)signal.Length;
            float[] valeurs = new float[size];
            signal.Read(valeurs, 0, size);
            res = new double[size];
            for(int i = 0; i < size;i++){
                res[i] = (double)valeurs[i];
            }
        }

        public static string getArtist(string path){
            TagLib.File tagFile = TagLib.File.Create(path);
            return tagFile.Tag.FirstAlbumArtist;
        }

        public static string getTitle(string path){
            TagLib.File tagFile = TagLib.File.Create(path);
            return tagFile.Tag.Title;
        }

    }
}