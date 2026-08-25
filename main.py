# main.py

from engine.runtime import EngineRuntime
from engine.log import Log
from engine.config import Config

from cave.labyrinth.scene import CaveScene
from big_animal.fauna.manager import FaunaManager
from oracle.corruption.engine import CorruptionEngine
from astral.identity.core import AstralIdentityCore
from sovereign_forms.manager import SovereignFormManager

def main():
    Log.info("Booting SovereignEngine-Core")

    config = Config()
    config.load()

    runtime = EngineRuntime()

    runtime.register(CaveScene())
    runtime.register(FaunaManager())
    runtime.register(CorruptionEngine())
    runtime.register(AstralIdentityCore())
    runtime.register(SovereignFormManager())

    runtime.start()

if __name__ == "__main__":
    main()
