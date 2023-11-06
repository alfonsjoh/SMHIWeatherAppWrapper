run:
	cd ./App/WeatherApp && dotnet run --configuration Release
search:
	cd ./Meilisearch && start meilisearch --master-key="${meilisearch_master_key}"
init-search:
	python -m pip install -r ./InitMeilisearch/requirements.txt
	python ./InitMeilisearch/main.py
test-search:
	python -m pip install -r ./InitMeilisearch/requirements.txt
	python .\InitMeilisearch\test_meilisearch.py
run-redis:
	wsl.exe sudo service redis-server start
