import json


class City:
    def __init__(self, index, name, lon, lat, municipality, county) -> None:
        self.index = index
        self.name = name
        self.lon = lon
        self.lat = lat
        self.municipality = municipality
        self.county = county
        