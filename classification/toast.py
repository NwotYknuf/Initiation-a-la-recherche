import numpy as np
import matplotlib.pyplot as plt
import io
import math
import os

from sklearn.cluster import DBSCAN
from scipy import stats

# Lecture du fichier data.json
def getData(path):
    data = []
    with open(path, encoding="utf8") as file:
        for line in file :
            split = line.split("\t")
            data.append([float(split[0]), float(split[1]), float(split[2]), float(split[3])])
    return data


def getDiff(x, y):
    intersection_cardinality = len(set.intersection(*[set(x), set(y)]))
    union_cardinality = len(set.union(*[set(x), set(y)]))
    return intersection_cardinality/float(union_cardinality)       

path = "D:\\Code\\musique\\data\\test\\"
file1 = "08 Anitta & Prince Royce - Rosa"
file2 = "06 Still Take You Home"

files = []

for root, directory, name in os.walk(path):
    files = name

res = []
for song1 in files :
    v = []
    for song2 in files :
        data1 = np.array(getData(path + song1))
        data2 = np.array(getData(path + song2))
        test = getDiff(data1[:,1], data2[:,1])
        v.append(test[1])
    res.append(v)

plt.figure(1)
plt.plot(res[0], files)
plt.show()
