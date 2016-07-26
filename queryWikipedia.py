import json
import urllib.parse
import urllib.request
import codecs

def parse_category(category):
	prefix = "Category:"
	if category.startswith(prefix):
		return category[len(prefix):]
	return category

query = input('Enter your query:')
service_url = 'https://en.wikipedia.org/w/api.php'
params = {
    'titles': query,
    'format': 'json',
    'action' : 'query',
    'prop' : 'categories',

}
url = service_url + '?' + urllib.parse.urlencode(params)
reader = codecs.getreader("utf-8")
response = json.load(reader(urllib.request.urlopen(url)))

title = 'title'
pageid = list(response['query']['pages'].keys())[0]

page = response['query']['pages'][pageid]
allTypes = []
if('categories' in page):
	categories = page['categories']
	allTypes = [ parse_category( category[title]) for category in categories if not category[title].startswith('Category:All')
															  and not category[title].startswith('Category:Article')
															  and not category[title].startswith('Category:Commons')
															  and not category[title].startswith('Category:Pages')
															  and not category[title].startswith('Category:Wikipedia')]

print(allTypes)

