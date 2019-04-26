import numpy as np
import matplotlib.pyplot as plt
import io
import math
import json

from sklearn.manifold import TSNE
from sklearn.cluster import AgglomerativeClustering, KMeans, SpectralClustering

# Lecture du fichier data.json
def getData(path):
    with open(path) as json_data:
        data_dict = json.load(json_data)
    data_str = json.dumps(data_dict)
    return json.loads(data_str)

PATH = "..\\data\\data.json"    # Chemin vers le fichier data.json
CLUSTERS = 8                    # Nombre de clusters

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
estimators = [
    [ "k-means", KMeans(n_clusters = CLUSTERS)],
    [ "Agglo", AgglomerativeClustering(n_clusters = CLUSTERS, linkage = 'ward')],
    [ "SpectralClustering", SpectralClustering(n_clusters = CLUSTERS, eigen_solver = 'arpack', affinity = "nearest_neighbors")]
]

# Initialisation des paramètres d'affichage
colors = ['b','g','r','c','m','y','k', '0.5']
embeding = TSNE(n_components = 2, perplexity = 50, n_iter = 5000)
dataClusturing = data[:,1:].astype(float)
dataClusturing = embeding.fit_transform(dataClusturing)

plot = 1
plt.figure(1)
resultat = []

# Application des algorithmes de classification
for name, estimator in estimators:

    # Fit data on everything except the first column
    estimator.fit(dataClusturing)
    labels = estimator.labels_

    # Représentation des données en 2D
    plt.subplot(len(estimators), 1, plot)
    plt.title(name)
    for i in range(0, len(dataClusturing)-1):
        plt.scatter(dataClusturing[i,0], dataClusturing[i,1], c = colors[int(labels[i])])
    plot += 1

plt.show()
