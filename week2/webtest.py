import web
import shutil
import PIL
from PIL import Image

import numpy as np
import matplotlib.pyplot as plt
import matplotlib.image as mpimg

def split_image(img,size = (50,50)):
    maxX, maxY = img.width, img.height

    cols = maxX//size[0]
    rows = maxY//size[1]

    patches = [[0 for x in range(rows)] for y in range(cols)]

    print(len(patches),len(patches[0]))

    for x in range(1,maxX,50):
        for y in range(1,maxY,50):
            patch = img.crop((x,y,x+50,y+50))
            patches[x//50][y//50] = patch

    return patches

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
    '/result','result'
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
        w = open("templates/images/resources/tmp.png",'wb')
        w.write(x['myfile'].file.read())

        img = Image.open("templates/images/resources/tmp.png")

        patches = split_image(img)

        print(len(patches),len(patches[0]))

        raise web.seeother('/result')

class result:
    def GET(self):
        raise web.seeother('/')

    def POST(self):
        raise web.seeother('/')

if __name__ == "__main__":
    app = web.application(urls, globals())
    app.run()