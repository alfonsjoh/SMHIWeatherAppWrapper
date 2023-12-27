import json


class City:
    def __init__(self, index, name, lat, lon, municipality, county) -> None:
        self.index = index
        self.name = name
        self.lat = lat
        self.lon = lon
        self.municipality = municipality
        self.county = county
        