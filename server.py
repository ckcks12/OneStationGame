# -*- coding: utf-8 -*-

from flask import Flask, make_response
from flask_httpauth import HTTPBasicAuth
from flask_restful import Resource, Api, abort, reqparse
import pymysql
import json
from time import mktime
import datetime

app = Flask(__name__)
api = Api(app)
auth = HTTPBasicAuth()

parser = reqparse.RequestParser()
parser.add_argument("id")
parser.add_argument("score")
parser.add_argument("pw")
parser.add_argument("game_id")
parser.add_argument("title")
parser.add_argument("content")

conn = pymysql.connect(host="localhost", user="onestationgame", password="", db="onestationgame", charset="utf8")
cursor = conn.cursor(pymysql.cursors.DictCursor)

class DateTimeEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, datetime.datetime):
            return "%s.%02d.%02d %02d:%02d:%02d" % (obj.year, obj.month, obj.day, obj.hour, obj.minute, obj.second)
        return json.JSONEncoder.default(Self, obj)
def custom_date_time_json_output(data, code, headers=None):
    dumped = json.dumps(data, cls=DateTimeEncoder)
    resp = make_response(dumped, code)
    resp.headers.extend(headers or {})
    return resp
api.representations.update({
    "application/json": custom_date_time_json_output
})

class Score(Resource):
    def get(self, id):
        q = "select * from score where `id` = %s order by `pk` desc limit 1"
        cursor.execute(q, (id))
        q = cursor.fetchall()
        if len(q) > 0:
            q = q[0]
        else:
            q = {}
        return q
    def post(self):
        param = parser.parse_args()
        q = "insert into score(`id`, `score`, `game_id`) values(%s, %s, %s)"
        try:
            cursor.execute(q, (param["id"], int(param["score"]), int(param["game_id"])))
            conn.commit()
        except:
            abort(500, message="db error")
        return {"status": 1}
    def delete(self, id):
        q = "delete from score where `id` = %s"
        try:
            cursor.execute(q, (id))
            conn.commit()
        except:
            abort(500, message="db error")
        return {"status": 1}

api.add_resource(Score, "/score", "/score/<id>")

class Article(Resource):
    def get(self, pk=None):
        if pk is None:
            q = "select * from article order by `pk` desc limit 1"
        else:
            q = "select * from article where `pk` = %s"
        cursor.execute(q, (pk))
        q = cursor.fetchall()
#        if not pk is None:
#            q = q[0]
        q = q[0]
        return q
    def post(self):
        param = parser.parse_args()
        q = "insert into article(`title`, `content`) values(%s, %s)"
        last_pk = -1
        try:
            cursor.execute(q, (param["title"], param["content"]))
            conn.commit()
            last_pk = cursor.lastrowid
        except:
            abort(500, message="db error")
        return {"status": 1, "pk": last_pk}

api.add_resource(Article, "/article", "/article/<pk>")

@auth.verify_password
def login(id, pw):
    if pw == "123":
        return True
    else:
        return False

if __name__ == "__main__":
    app.run(debug=True, host="0.0.0.0")
