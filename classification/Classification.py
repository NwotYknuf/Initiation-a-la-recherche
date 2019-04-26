import numpy as np
import matplotlib.pyplot as plt
import io
import math
import json

from sklearn.manifold import TSNE
from sklearn.cluster import AgglomerativeClustering,KMeans

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
    [ "k-means" ,KMeans(n_clusters = CLUSTERS) ],
    [ "Agglo", AgglomerativeClustering(n_clusters = CLUSTERS, linkage = 'ward')]
]

# Initialisation des paramètres d'affichage
colors = ['b','g','r','c','m','y','k', '0.5']
embeding = TSNE(n_components = 2, perplexity = 50, n_iter = 5000)
X = data[:,1:].astype(float)
X = embeding.fit_transform(X)

plot = 1
plt.figure(1)

# Application des algorithmes de classification
for name, estimator in estimators:
    
    # Fit data on everything except the first column
    estimator.fit(data[:,1:])
    labels = estimator.labels_

    # Représentation des données en 2D
    plt.subplot(len(estimators), 1, plot)
    plt.title(name)
    for i in range(0, len(X)-1):
        plt.scatter(X[i,0], X[i,1], c = colors[int(labels[i])])
    plot += 1

plt.show()