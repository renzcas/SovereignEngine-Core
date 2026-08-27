# Create Cave# engine structure
mkdir cave\creatures
mkdir cave\ai
mkdir cave\ai\nodes
mkdir cave\core
mkdir cave\prefabs
mkdir cave\vfx
mkdir cave\audio

# Create creature files
ni cave\creatures\Trog.cs -Force
ni cave\creatures\ShadowWolf.cs -Force
ni cave\creatures\Minotaur.cs -Force
ni cave\creatures\Titan.cs -Force

# Create core engine files
ni cave\core\SoulVectorCore.cs -Force
ni cave\core\InstinctCore.cs -Force
ni cave\core\CombatController.cs -Force
ni cave\core\PrefabLoader.cs -Force
ni cave\core\GameLoop.cs -Force
ni cave\core\EventBus.cs -Force

# Create AI files
ni cave\ai\BehaviorTree.cs -Force
ni cave\ai\AIController.cs -Force

# Create AI node files
ni cave\ai\nodes\TrackNode.cs -Force
ni cave\ai\nodes\StalkNode.cs -Force
ni cave\ai\nodes\LungeNode.cs -Force
ni cave\ai\nodes\RipNode.cs -Force
ni cave\ai\nodes\WarpNode.cs -Force
ni cave\ai\nodes\GuardNode.cs -Force
ni cave\ai\nodes\BondNode.cs -Force
ni cave\ai\nodes\MergeNode.cs -Force
ni cave\ai\nodes\HowlNode.cs -Force

# Create prefab files
ni cave\prefabs\Trog.prefab.json -Force
ni cave\prefabs\ShadowWolf.prefab.json -Force

# Create shader files
ni cave\vfx\ShadowFur.shader -Force
ni cave\vfx\WarpBurst.shader -Force
ni cave\vfx\VoidHowl.shader -Force

# Create audio core file
ni cave\audio\AudioCore.cs -Force
