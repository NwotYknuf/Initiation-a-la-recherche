import numpy as np
import matplotlib.pyplot as plt
import io
import math

from sklearn.cluster import AgglomerativeClustering

def getData(path):
    file = open(path)
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
CLUSTERS = 11

data = getData(PATH)
estimator = AgglomerativeClustering(n_clusters = CLUSTERS, linkage = 'ward')

#Fit data on everything except the first column
estimator.fit(data[:,1:])

labels = estimator.labels_
res = np.c_[data[:,0], labels]

#Sort data by clusters
res = res[res[:,1].argsort()]

print(res)