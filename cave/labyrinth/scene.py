class CaveScene:
    def __init__(self):
        self.loaded = False
        self.tick = 0

    def initialize(self):
        print("[CaveScene] Loading cave labyrinth...")
        self.loaded = True

    def update(self):
        self.tick += 1

        if self.tick % 60 == 0:
            print(f"[CaveScene] Tick {self.tick}")