import web
import shutil
import PIL
from PIL import Image

import numpy as np
import matplotlib.pyplot as plt
import matplotlib.image as mpimg

from tensorflow import keras
from keras import layers
from keras.utils.np_utils import to_categorical
import cv2
import numpy as np
import sklearn
import os

from image_functions3 import *

import json

render = web.template.render('templates2/')

urls = (
    '/', 'index',
    '/add', 'add',
    '/upload_image', 'upload_image',
    '/upload_book','upload_book',
    '/upload','Upload',
    '/result','result',
    '/images/(.*)','get_image',
    '/resources','resources'
)

class index:
    def GET(self):
        print("something")
        return render.index()

class add:
    def POST(self):
        i = web.input()
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
    def OPTIONS(self):
        '''Respond to options requests'''
        web.ctx.status = '204'
        web.header('Access-Control-Allow-Origin', 'http://localhost:4200')
        return ""

    def POST(self):
        x = web.input(image_to_process={})
        print(type(x))
        print(type(x['image_to_process']))
        img_name = x['image_to_process'].filename
        print(img_name) # This is the filename
        #web.debug(x['myfile'].value) # This is the file contents
        #web.debug(x['myfile'].file.read()) # Or use a file(-like) object
        w = open("static/images/resources/" + img_name,'wb')
        w.write(x['image_to_process'].file.read())
        w.close()

        prob, rez_path = analyse_img_name(img_name)
        web.header('Content-Type', 'application/json')
        web.header('Access-Control-Allow-Origin', 'http://localhost:4200')
        #return render.response(rez_path)

        resp = {
            'prob': prob,
            'result': '/' + rez_path
        }
        return json.dumps(resp)

class result:
    def GET(self):
        raise web.seeother('/')

    def POST(self):
        raise web.seeother('/')

class get_image:
    def GET(self,fileName):
      imageBinary = open("/templates/images/"+fileName,'rb').read()
      return imageBinary

class resources:
    def GET(self):
        resp = {'contents': ['Breast Cancer', 'Breast Tumour']}
        web.header('Content-Type', 'application/json')
        web.header('Access-Control-Allow-Origin', 'http://localhost:4200')
        return json.dumps(resp)


if __name__ == "__main__":
    app = web.application(urls, globals())
    app.run()