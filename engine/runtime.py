# engine/runtime.py

import time

class EngineRuntime:
    def __init__(self):
        self.modules = []
        self.running = False

    def register(self, module):
        self.modules.append(module)

    def start(self):
        print("[EngineRuntime] Starting engine...")
        self.running = True

        for module in self.modules:
            module.initialize()

        self.loop()

    def loop(self):
        print("[EngineRuntime] Entering main loop...")
        while self.running:
            for module in self.modules:
                module.update()

            time.sleep(0.016)  # ~60 FPS

    def stop(self):
        print("[EngineRuntime] Stopping engine...")
        self.running = False
