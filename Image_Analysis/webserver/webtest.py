import web
import shutil
import PIL
from PIL import Image

import numpy as np
import matplotlib.pyplot as plt
import matplotlib.image as mpimg

from PIL import Image
from tensorflow import keras
from keras import layers
from keras.utils.np_utils import to_categorical
import cv2
import numpy as np
import sklearn
import os

from image_functions import *

render = web.template.render('templates/')

db = web.database(
    dbn='mysql',
    host='127.0.0.1',
    port=3306,
    user='stefy',
    pw='Stefan2000',
    db='pytest'
)

urls = (
    '/', 'index',
    '/add', 'add',
    '/upload_image', 'upload_image',
    '/upload_book','upload_book',
    '/upload','Upload',
    '/result','result',
    '/images/(.*)','get_image'
)

class index:
    def GET(self):
        print("something")
        todos = db.select('carti')
        return render.index(todos)

class add:
    def POST(self):
        i = web.input()
        n = db.insert('carti', isbn = i.isbn,titlu=i.titlu,autor=i.autor)
        raise web.seeother('/')

class upload_book:
    def GET(self):
        print("another thing")
        return render.upload_book()

class upload_image:
    def GET(self):
        print("yet another thing")
        return render.upload_image()

class Upload:
    def POST(self):
        x = web.input(myfile={})
        print(type(x))
        print(type(x['myfile']))
        web.debug(x['myfile'].filename) # This is the filename
        #web.debug(x['myfile'].value) # This is the file contents
        #web.debug(x['myfile'].file.read()) # Or use a file(-like) object
        w = open("static/images/resources/tmp.png",'wb')
        w.write(x['myfile'].file.read())
        w.close()

        #"""
        total, blanks, pozitives, percent = analyse_by_name(
            "static/images/resources/tmp.png",
            "static/images/resources/tmp_rez.png",
            "the_one")

        #"""

        #total, blanks, pozitives, percent = 1,2,3,4
        

        w = open("templates/images/resources/tmp_rez.png",'rb')
        w.close()

        print(render.result(total,blanks,pozitives, percent,"images/resources/tmp_rez.png"))

        return render.result(total,blanks,pozitives, percent,"images/resources/tmp_rez.png")

class result:
    def GET(self):
        raise web.seeother('/')

    def POST(self):
        raise web.seeother('/')

class get_image:
    def GET(self,fileName):
      imageBinary = open("/templates/images/"+fileName,'rb').read()
      return imageBinary

if __name__ == "__main__":
    app = web.application(urls, globals())
    app.run()