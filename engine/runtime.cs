using System;
using System.Collections.Generic;
using SovereignEngine.Core;

namespace SovereignEngine.Runtime
{
    public class EngineRuntime
    {
        private readonly WorldContext _world;
        private readonly List<ISystem> _systems = new();

        // Hooks for external integrations
        public Action<WorldContext>? CaveSharpScriptRunner { get; set; }
        public Action<WorldContext>? TelemetryEmitter { get; set; }

        public EngineRuntime(WorldContext world)
        {
            _world = world;
        }

        public void RegisterSystem(ISystem system) => _systems.Add(system);

        public void Tick(float deltaTime)
        {
            // 1. Run CaveSharp scripts (world + organisms)
            CaveSharpScriptRunner?.Invoke(_world);

            // 2. Update systems (physics, organisms, hazards, etc.)
            foreach (var system in _systems)
                system.Update(_world, deltaTime);

            // 3. Process events (world events, organism events)
            foreach (var evt in _world.Events.Drain())
                HandleEvent(evt);

            // 4. Emit telemetry to cockpit / AgentDash
            TelemetryEmitter?.Invoke(_world);
        }

        private void HandleEvent(WorldEvent evt)
        {
            // Minimal event routing; you can expand this later.
            switch (evt.Type)
            {
                case "hazard.hit":
                    // Example: reduce energy of target organism
                    if (evt.Target is EntityId target &&
                        _world.TryGetComponent<OrganismComponent>(target, out var org))
                    {
                        org.Energy -= 10f;
                        if (org.Energy <= 0)
                            org.IsAlive = false;
                    }
                    break;

                case "world.quake":
                    // Example: global physics disturbance
                    foreach (var (_, phys) in _world.Query<PhysicsComponent>())
                    {
                        phys.State.VX += 0.5f;
                        phys.State.VY += 0.5f;
                    }
                    break;

                default:
                    // Unknown events can be logged or ignored
                    break;
            }
        }
    }

    // Example physics system
    public class PhysicsSystem : ISystem
    {
        public void Update(WorldContext world, float deltaTime)
        {
            foreach (var (_, phys) in world.Query<PhysicsComponent>())
            {
                phys.State.X += phys.State.VX * deltaTime;
                phys.State.Y += phys.State.VY * deltaTime;
            }
        }
    }

    // Example organism system
    public class OrganismSystem : ISystem
    {
        public void Update(WorldContext world, float deltaTime)
        {
            foreach (var (id, org) in world.Query<OrganismComponent>())
            {
                if (!org.IsAlive) continue;

                // Simple energy drain over time
                org.Energy -= 0.1f * deltaTime;

                if (org.Energy <= 0)
                {
                    org.IsAlive = false;
                    world.Events.Emit(new WorldEvent("organism.dead", source: id));
                }
            }
        }
    }
}
