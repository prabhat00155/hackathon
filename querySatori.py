import json
import urllib.parse
import urllib.request
import codecs
import re

def parse_type(value):
	if(value.startswith("mso:")):
		value = re.split(':|\.',value)[-1]
	return " ".join(value.split('_'))

query = input('Enter your query:')
service_url = 'https://www.bing.com/api/v5/entities/custom/SatoriTextAnalytics/Search'
params = {
    'q': query,
    'appid': 'D41D8CD98F00B204E9800998ECF8427E2CDA1C5E',
}
url = service_url + '?' + urllib.parse.urlencode(params)
reader = codecs.getreader("utf-8")
response = json.load(reader(urllib.request.urlopen(url)))

valueString = 'value'
bestEntity = response['answers'][0]
for prop in bestEntity['value'][0]['properties']:
	if prop['propertyName'] == 'Type':
		types = prop['values']
		break

allTypes = [ parse_type(element[valueString]) for element in types if not element[valueString] == 'PrecisionGraphEntity'
															  and not element[valueString].startswith('http')]
print(allTypes)

