import meilisearch
from anyascii import anyascii
from city import City
import csv
import os
import time

def connect_meilisearch():
    client = meilisearch.Client('http://localhost:7700', os.environ['meilisearch_master_key'])

    return client

def format_name(name):
    return anyascii(name.lower().replace(' ', '_').replace('/', '-'))

if __name__ == '__main__':
    client = connect_meilisearch()
    index = client.index('cities')

    tasks = []

    # Add all cities to Meilisearch
    for cities_file in os.listdir('cities'):
        with open(f'cities/{cities_file}', 'r', encoding='utf-8') as f:
            cities_reader = csv.reader(f)

            # Skip header
            next(cities_reader)

            cities = list(map(lambda city:
                City(
                    format_name(city[0]),
                    city[0],
                    city[3],
                    city[4],
                    city[1],
                    city[2]
                ).__dict__,
                cities_reader))

            tasks.append(index.add_documents(cities, 'index'))

    # Wait for all tasks to finish
    for task in tasks:
        while True:
            taskRes = client.get_task(task.task_uid)
            status = taskRes.status
            if status == 'succeeded':
                break
            elif not (status == 'enqueued' or status == 'processing'):
                print(f'Error! Status: {status}, {taskRes}')
                exit(1)

            time.sleep(0.5)
