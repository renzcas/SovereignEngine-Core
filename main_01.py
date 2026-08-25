# main.py

from cave.labyrinth.scene import CaveScene
from big_animal.fauna.manager import FaunaManager
from oracle.corruption.engine import CorruptionEngine
from astral.identity.core import AstralIdentityCore
from sovereign_forms.manager import SovereignFormManager


def main():
    cave = CaveScene()
    fauna = FaunaManager()
    corruption = CorruptionEngine()
    astral = AstralIdentityCore()
    forms = SovereignFormManager()

    print("SovereignEngine-Core booting...")
    cave.load()
    fauna.initialize()
    corruption.initialize()
    astral.initialize()
    forms.initialize()

    print("Engine ready. (Loop not yet implemented.)")


if __name__ == "__main__":
    main()
