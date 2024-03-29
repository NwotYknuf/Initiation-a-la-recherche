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
CLUSTERS = 15                    # Nombre de clusters

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

# Initialisation des modèles de classification 
estimator = KMeans(n_clusters = CLUSTERS)
training_data = np.vstack([data[:,1].astype(float), data[:,3].astype(float)]).transpose()

estimator.fit(training_data)
labels = estimator.labels_

# text output of clusters
out = np.vstack((data[:,0], labels.astype(int)))
out = np.transpose(out)
out = out[np.argsort(out[:,1])]

output = open("..\\data\\kmeans", "w", encoding='utf8')
for name,categ in out:
    output.write(name + "   " + categ + "\n")
output.close()

plt.figure("Classification")
plt.scatter(training_data[:,0], training_data[:,1], c = labels/max(labels))
plt.plot()
plt.show()