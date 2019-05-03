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

# Initialisation des modèles de classification 
estimators = [ 
    [ "k-means" ,KMeans(n_clusters = CLUSTERS) ],
    [ "Agglo", AgglomerativeClustering(n_clusters = CLUSTERS, linkage = 'ward')],
    [ "Affinity", AffinityPropagation()],
    #[ "Birch", Birch(n_clusters = CLUSTERS)],
    #[ "DBSCAN", DBSCAN(eps=0.3, min_samples=10)],
    #["MiniBatch", MiniBatchKMeans(n_clusters = CLUSTERS)],
    #["MeanShift", MeanShift()],
    ["Spectral", SpectralClustering(n_clusters = CLUSTERS)]
]

training_data = data[:,1:].astype(float)

# Normalisation des données
norm = Normalizer()
training_data = norm.fit_transform(training_data)

# Initialisation de t-SNE pour visualiser les donnés en 2D
#embeding = TSNE(n_components = 2, perplexity = 50, n_iter = 5000)
#training_data = data[:,1:].astype(float)
#training_data = embeding.fit_transform(training_data)

# Réduction des composantes principales avec PCA
pca = PCA(n_components = 2 )

training_data = pca.fit_transform(training_data)

print(pca)

plot = 1
plt.figure(1)
resultat = []

# Application des algorithmes de classification
for name, estimator in estimators:

    # Entrainement
    estimator.fit(training_data)
    labels = estimator.labels_

    # Représentation des données
    plt.subplot(len(estimators)/2,2,plot)
    plt.title(name)
    plt.scatter(training_data[:,0],training_data[:,1], c = labels/max(labels))
    plot += 1

    # text output of clusters
    out = np.vstack((data[:,0], labels.astype(int)))
    out = np.transpose(out)
    out = out[np.argsort(out[:,1])]

    output = open("..\\data\\" + name, "w", encoding='utf8')
    
    for name,categ in out:
        output.write(name + "   " + categ + "\n")

    output.close()

plt.show()
