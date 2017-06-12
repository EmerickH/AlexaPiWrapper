#! /usr/bin/env python

from __future__ import print_function
import os
import json
import socket
import uuid
import hashlib

import yaml
import cherrypy
import requests

try:
	from urllib.parse import quote
except ImportError:
	from urllib import quote

import alexapi.config

with open(alexapi.config.filename, 'r') as stream:
	config = yaml.load(stream)


class Start(object):

	def index(self):
		sd = json.dumps({
			"alexa:all": {
				"productID": config['alexa']['Device_Type_ID'],
				"productInstanceAttributes": {
					"deviceSerialNumber": hashlib.sha256(str(uuid.getnode()).encode()).hexdigest()
				}
			}
		})

		url = "https://www.amazon.com/ap/oa"
		callback = cherrypy.url() + "code"
		payload = {
			"client_id": config['alexa']['Client_ID'],
			"scope": "alexa:all",
			"scope_data": sd,
			"response_type": "code",
			"redirect_uri": callback
		}
		req = requests.Request('GET', url, params=payload)
		prepared_req = req.prepare()
		raise cherrypy.HTTPRedirect(prepared_req.url)

	def code(self, var=None, **params):		# pylint: disable=unused-argument
		code = quote(cherrypy.request.params['code'])
		callback = cherrypy.url()
		payload = {
			"client_id": config['alexa']['Client_ID'],
			"client_secret": config['alexa']['Client_Secret'],
			"code": code,
			"grant_type": "authorization_code",
			"redirect_uri": callback
		}
		url = "https://api.amazon.com/auth/o2/token"
		response = requests.post(url, data=payload)
		resp = response.json()

		alexapi.config.set_variable(['alexa', 'refresh_token'], resp['refresh_token'])

		return "<h2>Success!</h2>" \
				"<p>The refresh token has been added to your config file.</p>" \
				"<p>Now:</p>" \
				"<ul>" \
				"<li>close your this browser window,</li>" \
				"<li>exit the setup script as indicated,</li>" \
				"<li>and follow the Post-installation steps.</li>" \
				"</ul>"


	index.exposed = True
	code.exposed = True

cherrypy.config.update({'server.socket_host': '0.0.0.0'})
cherrypy.config.update({'server.socket_port': int(os.environ.get('PORT', '5050'))})
cherrypy.config.update({"environment": "embedded"})


ip = [(s.connect(('8.8.8.8', 53)), s.getsockname()[0], s.close()) for s in [socket.socket(socket.AF_INET, socket.SOCK_DGRAM)]][0][1]
print("Ready goto http://{}:5050 or http://localhost:5050  to begin the auth process".format(ip))
print("(Close this window once authorization is complete)")
cherrypy.quickstart(Start())
