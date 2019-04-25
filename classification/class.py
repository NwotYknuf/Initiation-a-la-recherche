import numpy as np
import matplotlib.pyplot as plt
import io
import math

from sklearn.manifold import TSNE
from sklearn.cluster import AgglomerativeClustering,KMeans

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
    [ "Agglo", AgglomerativeClustering(n_clusters = CLUSTERS, linkage = 'ward')]
]

colors = ['b','g','r','c','m','y','k', '0.5']

embeding = TSNE(n_components = 2, perplexity = 50, n_iter = 5000)
X = data[:,1:].astype(float)
X = embeding.fit_transform(X)

plot = 1
plt.figure(1)

for name,estimator in estimators:

    #Fit data on everything except the first column
    estimator.fit(data[:,1:])

    labels = estimator.labels_

    #Représentation des données en 2D

    plt.subplot(len(estimators),1,plot)
    plt.title(name)

    for i in range(0,len(X)-1):
        plt.scatter(X[i,0],X[i,1], c = colors[int(labels[i])])

    plot += 1

plt.show()
