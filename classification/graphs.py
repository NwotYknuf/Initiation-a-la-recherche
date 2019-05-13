import numpy as np
import matplotlib.pyplot as plt
import io
import math
import json

from sklearn.preprocessing import Normalizer
from sklearn.manifold import TSNE
from sklearn.decomposition import PCA
from sklearn.cluster import AgglomerativeClustering, KMeans, SpectralClustering, AffinityPropagation, MiniBatchKMeans

# Lecture du fichier data.json
def getData(path):
    with open(path, encoding="utf8") as json_data:
        data_dict = json.load(json_data)
    data_str = json.dumps(data_dict)
    return json.loads(data_str)

PATH = "..\\data\\data.json"    # Chemin vers le fichier data.json
CLUSTERS = 20                    # Nombre de clusters

# Désérialisation du fichier JSON
dataJSON = getData(PATH)
data = []
for dic in dataJSON:
    if dic == "" :
        break
    tabTemp = []
    tabTemp.append(dic)
    tabTemp.append((float)(dataJSON[dic]['median_rms']))
    tabTemp.append((float)(dataJSON[dic]['ecarType_rms']))
    tabTemp.append((float)(dataJSON[dic]['median_zrc']))
    tabTemp.append((float)(dataJSON[dic]['ecarType_zrc']))
    tabTemp.append((float)(dataJSON[dic]['median_centroid']))
    tabTemp.append((float)(dataJSON[dic]['ecarType_centroid']))
    tabTemp.append((float)(dataJSON[dic]['median_spread']))
    tabTemp.append((float)(dataJSON[dic]['ecarType_spread']))
    tabTemp.append((float)(dataJSON[dic]['songLenght']))
    data.append(tabTemp)

data = np.array(data)

training_data = data[:,1:].astype(float)

titles = ["mediane rms", "ecart rms", "mediane zcr", "ecar zcr", "mediane centoid", "ecart centroid" "mediane spead", "ecart spead", "durée"]

for i in range(1,2):
    v = training_data[:,i]
    n = data[:,0]
    n = n[v.argsort()]
    v = v[v.argsort()]

    plt.figure(titles[i])
    plt.subplot(1,2,1)
    plt.plot(v, n)
    plt.subplot(1,2,2)
    plt.boxplot(training_data[:,0])

plt.show()