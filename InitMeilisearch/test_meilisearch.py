import unittest
import main
import json
import city

class TestMeiliSearch(unittest.TestCase):
    def test_search(self):
        client = main.connect_meilisearch()
        index = client.index('cities')
        print([hit['name'] for hit in index.search('eksjo')['hits']])
        print([hit['name'] for hit in index.search('eksjö')['hits']])
        print([hit['name'] for hit in index.search('eks')['hits']])
        print([hit['name'] for hit in index.search('sjo')['hits']])
        print([hit['name'] for hit in index.search('sto')['hits']])
        print([hit['name'] for hit in index.search('väsby')['hits']])



if __name__ == '__main__':
    unittest.main()
