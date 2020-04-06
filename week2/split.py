import os
import numpy as np
from PIL import Image

"""
returns patches, a matrix of PIL.Image objects of the given size
"""
def split_image(img, size = (50,50)):
    maxX, maxY = img.width, img.height

    cols = (maxX+size[0]-1)//size[0]
    rows = (maxY+size[1]-1)//size[1]

    patches = [[0 for x in range(rows)] for y in range(cols)]

    new_im = Image.new('RGB',(cols*size[0],rows*size[1]),color=(255,255,255))

    new_im.paste(img)

    print(len(patches),len(patches[0]))

    for x in range(1,maxX,50):
        for y in range(1,maxY,50):
            patch = new_im.crop((x,y,x+50,y+50))
            patches[x//50][y//50] = patch

    return patches

def split_file(filename,size = (50,50)):
    
    if(not os.path.isfile(filename)):
        return 0
    
    img = Image.open(filename)

    return split_image(img)

split_file('8863.png')

