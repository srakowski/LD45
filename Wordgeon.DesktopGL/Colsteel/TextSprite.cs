// MIT License - Copyright (C) Shawn Rakowski
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Coldsteel
{
    public class TextSprite : Component, ISprite
    {
        private Asset<SpriteFont> _spriteFont;

        public string AssetName;

        public Rectangle? SourceRectangle;

        public Color Color = Color.White;

        public Vector2 Origin;

        public SpriteEffects SpriteEffects;

        public float LayerDepth;

        public bool Enabled = true;

        public string SpriteLayerName;

        public string Text = "";

        bool ISprite.Enabled => Enabled;

        string ISprite.SpriteLayerName => SpriteLayerName;

        public TextSprite() { }

        public TextSprite(string assetName, string text, string spriteLayerName)
        {
            AssetName = assetName;
            Text = text;
            SpriteLayerName = spriteLayerName;
        }

        private protected override void Activated()
        {
            Engine.SpriteSystem.AddSprite(Scene, this);
            _spriteFont = Scene.Assets.FirstOrDefault(a => a.Name == AssetName) as Asset<SpriteFont>;
        }

        private protected override void Deactivated()
        {
            _spriteFont = null;
            Engine.SpriteSystem.RemoveSprite(Scene, this);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!(_spriteFont?.IsLoaded ?? false) || !Enabled) return;
            spriteBatch.DrawString(
                _spriteFont.GetValue(),
                Text,
                Entity.GlobalPosition,
                Color,
                Entity.GlobalRotation,
                Origin,
                Entity.GlobalScale,
                SpriteEffects,
                LayerDepth
            );
        }

        void ISprite.Draw(SpriteBatch spriteBatch) => Draw(spriteBatch);
    }
}