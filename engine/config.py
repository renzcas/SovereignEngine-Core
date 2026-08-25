# engine/config.py

import json

class Config:
    def __init__(self, path="config.json"):
        self.path = path
        self.data = {}

    def load(self):
        try:
            with open(self.path, "r") as f:
                self.data = json.load(f)
            print("[Config] Loaded config.json")
        except FileNotFoundError:
            print("[Config] No config.json found, using defaults")
            self.data = {}
