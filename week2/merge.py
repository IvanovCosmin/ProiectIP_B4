import sys
import os
import numpy as np
from glob import glob
from PIL import Image

def global_image_path(b,x,y,pic):
    return '/home/stefy/cpp/ProiectIP/IDC_regular_ps50_idx5/' + pic + '/' + str(b) + '/' + pic + '_idx5_x' + str(x) + '_y' + str(y) + '_class' + str(b) + '.png'

def save_image(pic):

    if(not os.path.isdir('/home/stefy/cpp/ProiectIP/IDC_regular_ps50_idx5/' + pic)): 
        print(pic + ' - unsuccessfull')
        return

    imagePatches = glob('/home/stefy/cpp/ProiectIP/IDC_regular_ps50_idx5/' + pic + '/**/*.png', recursive=True)

    #print(type(imagePatches))
    #print(type(imagePatches[0]))
    #print(imagePatches[0])

    path = '/home/stefy/cpp/ProiectIP/IDC_regular_ps50_idx5/' + pic + '/0/' + pic + '_idx5_x951_y851_class0.png'

    def image_path(b,x,y):
        return global_image_path(b,x,y,pic)

    #print(path)

    #print(imagePatches.__contains__(image_path(0,951,851)))

    maxX = 0
    maxY = 0



    for x in range(1,10002,50): 
        for y in range(1,10002,50):
            if imagePatches.__contains__(image_path(0,x,y)):
                #contains[x//50][y//50] = 0
                if(x > maxX): maxX = x
                if(y > maxY): maxY = y
            elif imagePatches.__contains__(image_path(1,x,y)):
                #contains[x//50][y//50] = 1
                if(x > maxX): maxX = x
                if(y > maxY): maxY = y
            #else: contains[x//50][y//50] = -1

    #contains = np.zeros([maxX//50+1,maxY//50+1])

    old_img = Image.open('/home/stefy/cpp/ProiectIP/IDC_regular_ps50_idx5/' + pic + '/' + pic+".png")

    #print(maxX+49,maxY+49)
    #print(old_img.width,old_img.height)

    if( (int)(old_img.width) == (int)(maxX+49) and (int)(old_img.height) == (int)(maxY+49) ):
        print(pic + ' - file already exists')
        return

    new_im = Image.new('RGB',(maxX+49,maxY+49),color=(255,255,255))
    #new_im.save("blank.jpg")

    for x in range(1,maxX+1,50): 
        for y in range(1,maxY+1,50):
            if imagePatches.__contains__(image_path(0,x,y)):
                #contains[x//50][y//50] = 0
                im_to_paste = Image.open(image_path(0,x,y))
                new_im.paste(im_to_paste,(x,y))
            elif imagePatches.__contains__(image_path(1,x,y)):
                #contains[x//50][y//50] = 1
                im_to_paste = Image.open(image_path(1,x,y))
                new_im.paste(im_to_paste,(x,y))
            #else: contains[x//50][y//50] = -1

    #print(contains)
    #print(contains.shape)

    new_im.save('/home/stefy/cpp/ProiectIP/IDC_regular_ps50_idx5/' + pic + '/' + pic+".png")

    #print(maxX,maxY)

    #print(len(imagePatches))

    print(pic + ' - successfull')

#save_image('8955')

def add_blank(pic):
    if(not os.path.isdir('/home/stefy/cpp/ProiectIP/IDC_regular_ps50_idx5/' + pic)): 
        print(pic + ' - unsuccessfull')
        return

    if( os.path.isfile(global_image_path(0,1,1,pic)) or os.path.isfile(global_image_path(0,1,1,pic)) ): 
        print(pic + ' - file already exists') 
        return
    
    blank = Image.new('RGB',(50,50),color=(255,255,255))
    #print(blank.width,blank.height)

    blank.save(global_image_path(0,1,1,pic))

    print(pic + ' - successfull')

    
    
#"""
for i in range(8863,16900):
    #add_blank(str(i))
    save_image(str(i))
#"""

#save_image('8863')