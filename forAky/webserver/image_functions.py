import numpy as np
from PIL import Image
from tensorflow import keras
from keras import layers
from keras.models import model_from_json, Sequential
from keras.optimizers import SGD, RMSprop, Adam, Adagrad, Adadelta
from keras.utils.np_utils import to_categorical
import matplotlib.pylab as plt
import cv2
import numpy as np
import sklearn
import os

from sklearn import model_selection
from sklearn.model_selection import train_test_split, KFold, cross_val_score, StratifiedKFold, learning_curve, GridSearchCV
from sklearn.metrics import confusion_matrix, make_scorer, accuracy_score
from sklearn.linear_model import LogisticRegression
from sklearn.tree import DecisionTreeClassifier
from sklearn.neighbors import KNeighborsClassifier
from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
from sklearn.naive_bayes import GaussianNB
from sklearn.svm import SVC, LinearSVC
from sklearn.ensemble import RandomForestClassifier, GradientBoostingClassifier
import keras
from keras import backend as K
from keras.callbacks import Callback, EarlyStopping, ReduceLROnPlateau, ModelCheckpoint
from keras.preprocessing.image import ImageDataGenerator
from keras.utils.np_utils import to_categorical
from keras.models import Sequential, model_from_json, load_model
from keras.optimizers import SGD, RMSprop, Adam, Adagrad, Adadelta
from keras.layers import Dense, Dropout, Activation, Flatten, BatchNormalization, Conv2D, MaxPool2D, MaxPooling2D

path = "/home/stefy/cpp/ProiectIP/"

thresholds = [0.5,0.8]

MAX_SUM = 1800000

def split_image_aky(img, size = (50,50)):
    maxX, maxY = img.width, img.height

    cols = (maxX+size[0]-1)//size[0]
    rows = (maxY+size[1]-1)//size[1]

    patches = [[0 for x in range(rows)] for y in range(cols)]

    new_im = Image.new('RGB',(cols*size[0],rows*size[1]),color=(255,255,255))

    new_im.paste(img)

    print(len(patches),len(patches[0]))

    patches_list = []

    for x in range(1,maxX,50):
        for y in range(1,maxY,50):
            patch = new_im.crop((x,y,x+50,y+50))
            patches_list.append(patch)

    return patches_list


def split_image(img, size = (50,50)):
    maxX, maxY = img.width, img.height

    cols = (maxX+size[0]-1)//size[0]
    rows = (maxY+size[1]-1)//size[1]

    patches = [[0 for x in range(rows)] for y in range(cols)]

    new_im = Image.new('RGB',(cols*size[0],rows*size[1]),color=(255,255,255))

    new_im.paste(img)

    #print(len(patches),len(patches[0]))

    for x in range(1,maxX,50):
        for y in range(1,maxY,50):
            patch = new_im.crop((x,y,x+50,y+50))
            patches[x//50][y//50] = patch

    return patches

def split_file(filename,size = (50,50)):
    img = Image.open(filename)
    return split_image(img)



def getPatches(image_patches):
    x = len(image_patches)
    y = len(image_patches[0])

    print(x,y)
    print()

    patches = []
    index = 0
    path = "/home/stefy/cpp/ProiectIP/webserver/temporary/"
    for patch_list in image_patches:
        for patch in patch_list:
            new_image = Image.new('RGB', (50, 50), color=(255,255,255))
            new_image.paste(patch)
            temporary_file_path = path + "temporary" + str(index) + ".jpg"
            new_image.save(temporary_file_path)
            
            patches.append(cv2.imread(temporary_file_path))
            os.remove(temporary_file_path)
            index += 1

    return patches, x, y

def analize_patches(patches,x,y, model):
    
    y_pred = model.predict(patches/255.0)
    y = np.reshape(y_pred,(x,y,2))

    return y[:,:,1]

def get_model(name):
    with open(path + "models/" + name + "/" + name + ".json", "r") as json_file:
        _model = model_from_json(json_file.read())
        _model.load_weights(path + "models/" + name + "/" + name + ".h5")

    _model.compile(loss=keras.losses.categorical_crossentropy,
                  optimizer=keras.optimizers.Adadelta(),
                  metrics=['accuracy'])

    return _model

def merge_images(img1,img2,x=0.5):

    arr1 = np.array(img1)
    arr2 = np.array(img2)
    arr = arr1//4*3 + arr2//4
    img = Image.fromarray(arr,'RGB')

    return img

def generate_image(image_patches,grid,path = 'result.png'):
    x,y = grid.shape
    img = Image.new('RGB',(x*50,y*50),color=(255,255,255))

    for i in range(x):
        for j in range(y):
            if grid[i][j] > thresholds[1]:
                img.paste(merge_images(
                    image_patches[i][j],
                    Image.new('RGB',(50,50),color=(200,0,0))
                )
                    ,(i*50+1,j*50+1))
            elif grid[i][j] < thresholds[0]:
                img.paste(merge_images(image_patches[i][j],Image.new('RGB',(50,50),color=(0,200,0))),(i*50+1,j*50+1))
            else:
                img.paste(merge_images(image_patches[i][j],Image.new('RGB',(50,50),color=(200,200,0))),(i*50+1,j*50+1))
    img.save(path)

def count_blanks(patches):
    count = 0

    sums = []

    for patch in patches:
        sum = patch.sum()
        if sum > MAX_SUM: count += 1
    return count

def count_poz(grid):
    x,y = grid.shape
    count = 0

    for i in range(x):
        for j in range(y):
            if grid[i][j] > thresholds[1]:
                count += 1

    return count    



def analyse(source_path, destination_path, rn_model):
    image_patches = split_file(source_path)

    patches, xx, yy = getPatches(image_patches)

    #RESHAPE THE PHOTOS FOR TESTING
    width, height, channels = 50, 50, 3
    patches_reshaped = np.reshape(patches, (len(patches),height,width,channels))

    rez = analize_patches(patches_reshaped,xx,yy, rn_model)

    generate_image(image_patches,rez,destination_path)

    blanks = count_blanks(patches)
    total = len(patches)
    poz = count_poz(rez)

    percent = str(round(poz/(total-blanks)*100,2))

    return total, blanks, poz, percent


def analyse_by_name(source_path, destination_path, rn_name):
    return analyse(source_path,destination_path,get_model(rn_name))

"""
rn_model = get_model("the_one")

analyse("../8863.png","../8863_r.png",rn_model)
analyse("../8863_free.png","../8863_0_r.png",rn_model)
"""