import web
import json

render = web.template.render('templates3/')

urls = (
    '/resources', 'resources'
)

class resources:
    def GET(self):
        resp = {'contents': ['Breast Cancer', 'Breast Tumour']}
        web.header('Content-Type', 'application/json')
        web.header('Access-Control-Allow-Origin', 'http://localhost:4200')
        return json.dumps(resp)


if __name__ == "__main__":
    app = web.application(urls, globals())
    app.run()
