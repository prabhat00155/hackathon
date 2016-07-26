import json
import urllib.parse
import urllib.request
import codecs

def executeQuery(query):
	api_key = open('.api_key').read()  #file named .api_key should be there in the same directory with api key
	#query = input('Enter your query:')
	service_url = 'https://kgsearch.googleapis.com/v1/entities:search'
	params = {
	    'query': query,
	    'limit': 10,
	    'indent': True,
	    'key': api_key,
	}
	url = service_url + '?' + urllib.parse.urlencode(params)
	reader = codecs.getreader("utf-8")
	response = json.load(reader(urllib.request.urlopen(url)))
	#print(response)
	#for element in response['itemListElement']:
	#	print(element['result']['name'] + ' (' + str(element['resultScore']) + ')' + ' ' + str(element['result']['@type'])) 
	return str(response['itemListElement'][0]['result']['@type'])
