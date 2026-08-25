# cave/labyrinth/scene.py

class CaveScene:
    def __init__(self):
        self.loaded = False

    def load(self):
        # TODO: hook in sprites, physics, portals
        print("[CaveScene] Loading cave labyrinth...")
        self.loaded = True
