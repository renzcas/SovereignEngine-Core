using System;
using System.Collections.Generic;

namespace SovereignEngine.Core
{
    // Basic entity identifier
    public readonly struct EntityId
    {
        public int Value { get; }

        public EntityId(int value) => Value = value;

        public override string ToString() => $"Entity({Value})";
    }

    // Base component interface
    public interface IComponent { }

    // Base system interface
    public interface ISystem
    {
        void Update(WorldContext world, float deltaTime);
    }

    // Region in the world
    public class Region
    {
        public string Id { get; }
        public string Name { get; set; }

        public Region(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    // Physics primitive
    public struct PhysicsState
    {
        public float X;
        public float Y;
        public float VX;
        public float VY;
    }

    public class PhysicsComponent : IComponent
    {
        public PhysicsState State;
    }

    // Organism marker
    public class OrganismComponent : IComponent
    {
        public float Energy;
        public bool IsAlive;
    }

    // World event
    public class WorldEvent
    {
        public string Type { get; }
        public EntityId? Source { get; }
        public EntityId? Target { get; }
        public Dictionary<string, object> Data { get; }

        public WorldEvent(string type, EntityId? source = null, EntityId? target = null)
        {
            Type = type;
            Source = source;
            Target = target;
            Data = new Dictionary<string, object>();
        }
    }

    // Event bus
    public class EventBus
    {
        private readonly Queue<WorldEvent> _queue = new();

        public void Emit(WorldEvent evt) => _queue.Enqueue(evt);

        public IEnumerable<WorldEvent> Drain()
        {
            while (_queue.Count > 0)
                yield return _queue.Dequeue();
        }
    }

    // ECS-style world context
    public class WorldContext
    {
        private int _nextEntityId = 1;

        private readonly Dictionary<EntityId, Dictionary<Type, IComponent>> _entities = new();

        public List<Region> Regions { get; } = new();
        public EventBus Events { get; } = new();

        public EntityId CreateEntity()
        {
            var id = new EntityId(_nextEntityId++);
            _entities[id] = new Dictionary<Type, IComponent>();
            return id;
        }

        public void AddComponent<T>(EntityId entity, T component) where T : IComponent
        {
            if (!_entities.TryGetValue(entity, out var comps))
                throw new InvalidOperationException($"Entity {entity} does not exist.");

            comps[typeof(T)] = component;
        }

        public bool TryGetComponent<T>(EntityId entity, out T component) where T : class, IComponent
        {
            component = null;
            if (!_entities.TryGetValue(entity, out var comps))
                return false;

            if (comps.TryGetValue(typeof(T), out var raw))
            {
                component = raw as T;
                return component != null;
            }

            return false;
        }

        public IEnumerable<(EntityId id, T component)> Query<T>() where T : class, IComponent
        {
            foreach (var (id, comps) in _entities)
            {
                if (comps.TryGetValue(typeof(T), out var raw) && raw is T typed)
                    yield return (id, typed);
            }
        }
    }
}
