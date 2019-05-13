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
    return np.array(data)

def getDiff(x, y):
    sum = 0
    for i in range(0, min(len(x), len(y))) :
        sum+= (y[i]-x[i])**2
    sum /= min(len(x), len(y))
    return sum

path = "D:\\Code\\musique\\data\\test\\"

files = []

for root, directory, name in os.walk(path):
    files = name

res = []

song1 = "1-11 Blackbird"

for song2 in files :
    data1 = getData(path + song1)
    data2 = getData(path + song2)
    res.append(getDiff(data1[:,0], data2[:,0]))

res = np.array(res)
files = np.array(files)
files = files[res.argsort()]
res = res[res.argsort()]

plt.figure(1)
plt.plot(res, files)
plt.show()
