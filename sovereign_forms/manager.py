# sovereign_forms/manager.py

class SovereignFormManager:
    def __init__(self):
        self.forms = ["titan", "echo_monarch", "shard_emperor", "infinity"]

    def initialize(self):
        print("[SovereignFormManager] Available forms:", ", ".join(self.forms))
