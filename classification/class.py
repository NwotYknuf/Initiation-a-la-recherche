import numpy as np
import matplotlib.pyplot as plt
import matplotlib.colors as col
import io
import math
import colorsys

from sklearn.manifold import TSNE
from sklearn.cluster import AgglomerativeClustering,KMeans, AffinityPropagation, Birch, DBSCAN, FeatureAgglomeration, MiniBatchKMeans, MeanShift, SpectralClustering
from sklearn.preprocessing import Normalizer
def getData(path):
    file = open(path, encoding="utf8")
    data = []
    #get values
    for line in file :
        txt = line.split("|")
        first = 1
        vector = []
        correct = 1
        for word in txt :
            correct = 1
            if(first):
                first = 0
                vector.append(word)
            else:
                word = word.replace(",",".")
                value = float(word)
                if(math.isnan(value)):
                    correct = 0
                vector.append(value)
        if(correct):
            data.append(vector)
    data = np.array(data)
    return data


PATH = "D:\\Code\musique\\analyse\\output\\stats"
CLUSTERS = 8

data = getData(PATH)

estimators = [ 
    [ "k-means" ,KMeans(n_clusters = CLUSTERS) ],
    [ "Agglo", AgglomerativeClustering(n_clusters = CLUSTERS, linkage = 'ward')],
    [ "Affinity", AffinityPropagation()],
    #[ "Birch", Birch(n_clusters = CLUSTERS)],
    #[ "DBSCAN", DBSCAN(eps=0.3, min_samples=10)],
    ["MiniBatch", MiniBatchKMeans(n_clusters = CLUSTERS)],
    #["MeanShift", MeanShift()],
    ["Spectral", SpectralClustering(n_clusters = CLUSTERS)]
]

embeding = TSNE(n_components = 2, perplexity = 50, n_iter = 2000)
train = data[:,1:].astype(float)
train = Normalizer().fit_transform(train)
rep = embeding.fit_transform(train)

plot = 1
plt.figure(1)

for name,estimator in estimators:  

    #Fit data on everything except the first column
    estimator.fit(train)

    #get labels
    labels = estimator.labels_

    #Représentation des données en 2D
    plt.subplot(3,2,plot)

    plt.title(name)
    plt.scatter(rep[:,0],rep[:,1], c = labels/max(labels))
    plot += 1

    #sortie text
    output = open(name, 'w')
    
    out = np.vstack((data[:,0], labels))
    out = np.transpose(out)
    out = out[out[:,1].argsort()]
    
    for name,categ in out:
        output.write(name + "   " + categ + "\n")

    output.close()

plt.show()
