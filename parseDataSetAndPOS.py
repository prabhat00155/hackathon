import nltk
import sys
import queryKnowledgeGraph

if len(sys.argv) != 2:
	print('Insufficient number of command line arguments!')
training_file = open(sys.argv[1])
reader = training_file.readlines()
for example in reader:
	row = example.strip().split('\t')
	print(row[0])
	print(row[1])
	text = nltk.word_tokenize(row[1])
	pos_tags = nltk.pos_tag(text)
	for tag in pos_tags:
		if tag[1] == 'NN' or tag[1] == 'NNP':
			print(tag[0])
			entity = queryKnowledgeGraph.executeQuery(tag[0])
			print(entity)
	
training_file.close()
	
