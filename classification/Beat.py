import numpy as np
import matplotlib.pyplot as plt
import io


file = open("D:\\Code\\musique\\analyse\\test.txt")

rms = []
beat = []

for line in file:
    split = line.split("\t")

    rms.append(float(split[0]))
    beat.append(int(split[1]))

plt.subplot(2,1,1)
plt.plot(rms)
plt.subplot(2,1,2)
plt.plot(beat)
plt.show()
