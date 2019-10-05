// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Coldsteel
{
    public class Entity
    {
        private readonly List<Component> _components = new List<Component>();

        private readonly List<Entity> _children = new List<Entity>();

        private Engine _engine;

        private Scene _scene;

        private Entity _parent;

        public Vector2 Position;

        public Vector2 GlobalPosition => Vector2.Transform(Position, _parent?.TransformMatrix ?? Matrix.Identity);

        public float Rotation;

        public float GlobalRotation => Rotation + (_parent?.GlobalRotation ?? 0f);

        public float Scale = 1f;

        public float GlobalScale => Scale + (_parent?.GlobalScale - 1f ?? 0f);

        public Matrix TransformMatrix =>
            Matrix.Identity *
            Matrix.CreateRotationZ(GlobalRotation) *
            Matrix.CreateScale(GlobalScale) *
            Matrix.CreateTranslation(GlobalPosition.X, GlobalPosition.Y, 0f);

        public IEnumerable<Component> Components => _components;

        public Entity AddComponent(Component component)
        {
            _components.Add(component);
            if (_engine != null)
                component.Activate(_engine, _scene, this);
            return this;
        }


        public Entity AddChild(Entity entity)
        {
            _children.Add(entity);
            if (_engine != null)
                entity.Activate(_engine, _scene, this);
            return this;
        }


        internal void Activate(Engine engine, Scene scene, Entity parent)
        {
            _engine = engine;
            _scene = scene;
            _parent = parent;

            foreach (var entity in _children.ToArray())
                entity.Activate(engine, scene, this);

            foreach (var component in _components.ToArray())
                component.Activate(engine, scene, this);
        }

        internal void Deactivate()
        {
            foreach (var component in _components)
                component.Deactivate();

            foreach (var entity in _children.ToArray())
                entity.Deactivate();

            _parent = null;
            _scene = null;
            _engine = null;
        }
    }
}
