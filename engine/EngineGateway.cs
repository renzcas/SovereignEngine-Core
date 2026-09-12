using System;
using SovereignEngine.Core;
using SovereignEngine.Runtime;

namespace SovereignEngine.Gateway
{
    /// <summary>
    /// Central gateway that wires:
    /// - SovereignEngine-Core (C#)
    /// - BIG_ANIMAL (Python, via external bridge)
    /// - Renzoverse (world scripts, CaveSharp)
    /// - Cockpit / RB-App (commands + telemetry)
    /// </summary>
    public class EngineGateway
    {
        private readonly WorldContext _world;
        private readonly EngineRuntime _runtime;

        // External bridges (to be implemented by you or separate services)
        public Action<WorldContext>? BigAnimalBridge { get; set; }
        public Action<WorldContext>? RenzoverseBridge { get; set; }
        public Action<string, object?>? CockpitTelemetry { get; set; }
        public Func<string, object?, bool>? CockpitCommandHandler { get; set; }

        public EngineGateway()
        {
            _world = new WorldContext();
            _runtime = new EngineRuntime(_world);

            // Wire runtime hooks
            _runtime.CaveSharpScriptRunner = RunCaveSharpScripts;
            _runtime.TelemetryEmitter = EmitTelemetry;

            // Register core systems
            _runtime.RegisterSystem(new PhysicsSystem());
            _runtime.RegisterSystem(new OrganismSystem());

            // Seed basic world
            InitializeWorld();
        }

        private void InitializeWorld()
        {
            // Example: create a region and a simple organism
            var region = new Region("labyrinth_01", "First Labyrinth");
            _world.Regions.Add(region);

            var entity = _world.CreateEntity();
            _world.AddComponent(entity, new PhysicsComponent
            {
                State = new PhysicsState { X = 0, Y = 0, VX = 1, VY = 0 }
            });
            _world.AddComponent(entity, new OrganismComponent
            {
                Energy = 100f,
                IsAlive = true
            });
        }

        private void RunCaveSharpScripts(WorldContext world)
        {
            // Placeholder: this is where CaveSharp world/organism scripts run.
            // You can route into your CaveSharp runtime here.
            // For now, we just call external bridges if present.

            RenzoverseBridge?.Invoke(world);
            BigAnimalBridge?.Invoke(world);
        }

        private void EmitTelemetry(WorldContext world)
        {
            // Minimal telemetry example.
            CockpitTelemetry?.Invoke("world.region.count", world.Regions.Count);

            int alive = 0;
            int total = 0;
            foreach (var (_, org) in world.Query<OrganismComponent>())
            {
                total++;
                if (org.IsAlive) alive++;
            }

            CockpitTelemetry?.Invoke("organisms.total", total);
            CockpitTelemetry?.Invoke("organisms.alive", alive);
        }

        /// <summary>
        /// External caller (cockpit, CLI, etc.) can tick the engine.
        /// </summary>
        public void Tick(float deltaTime)
        {
            _runtime.Tick(deltaTime);
        }

        /// <summary>
        /// Handle a command from the cockpit (RB-App, CLI, etc.).
        /// </summary>
        public bool HandleCommand(string command, object? payload = null)
        {
            // First let a custom handler try
            if (CockpitCommandHandler != null &&
                CockpitCommandHandler.Invoke(command, payload))
                return true;

            // Basic built-in commands
            switch (command)
            {
                case "world.quake":
                    _world.Events.Emit(new WorldEvent("world.quake"));
                    return true;

                case "spawn.organism":
                    var entity = _world.CreateEntity();
                    _world.AddComponent(entity, new PhysicsComponent
                    {
                        State = new PhysicsState { X = 0, Y = 0, VX = 0, VY = 0 }
                    });
                    _world.AddComponent(entity, new OrganismComponent
                    {
                        Energy = 50f,
                        IsAlive = true
                    });
                    CockpitTelemetry?.Invoke("command.spawn.organism", entity.ToString());
                    return true;

                default:
                    return false;
            }
        }
    }
}
