import numpy as np
import matplotlib.pyplot as plt
import math
import io


file = open("D:\\Code\\musique\\data\\beat\\06 Still Take You Home")

low_energy = []
low_mean = []
low_var = []

mid_energy = []
mid_mean = []
mid_var = []

high_energy = []
high_mean = []
high_var = []

for line in file:
    split = line.split("\t")

    low_energy.append(float(split[0]))
    low_mean.append(float(split[1]))
    low_var.append(float(split[2]))
    mid_energy.append(float(split[3]))
    mid_mean.append(float(split[4]))
    mid_var.append(float(split[5]))
    high_energy.append(float(split[6]))
    high_mean.append(float(split[7]))
    high_var.append(float(split[8]))

def isBeat(rms, mean, var):
    if mean > 0.1 and rms > mean :
        return rms / mean
    else :
        return 0

def detect_beat (rms, mean, var) :
    
    beat = []
    
    for i in range(0,len(rms)):
        beat.append(isBeat(rms[i],mean[i],var[i]))

    beat_mean = 0
    N = 0
    for i in range(0,len(rms)):
        if beat[i] > 1.0 :
            beat_mean += beat[i]
            N += 1

    beat_mean = beat_mean/N

    beat_var = 0.0

    for i in range(0,len(rms)):
        if beat[i] > 1.0 :
            beat_var += (beat[i] - beat_mean)**2
    
    beat_var = math.sqrt(beat_var/N)

    for i in range(0, len(rms)):
        if beat[i] > (1.5 + beat_var) * beat_mean :
            beat[i] = 1.0
        else:
            beat[i] = 0.0

    return [ beat , beat_mean, beat_var]


low_beat, low_beat_mean, low_beat_var = detect_beat(low_energy,low_mean,low_var)
mid_beat, mid_beat_mean, low_beat_var = detect_beat(mid_energy,mid_mean,mid_var)
high_beat, high_beat_mean, low_beat_var = detect_beat(high_energy,high_mean,high_var)

plt.subplot(3,1,1)
plt.plot(low_energy)
plt.plot(low_beat, color = 'r')

plt.subplot(3,1,2)
plt.plot(mid_energy)
plt.plot(mid_beat, color = 'r')

plt.subplot(3,1,3)
plt.plot(high_energy)
plt.plot(high_beat, color = 'r')

plt.show()