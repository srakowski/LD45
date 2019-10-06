using System;
using System.Linq;
using Coldsteel;
using Microsoft.Xna.Framework;
using Wordgeon.Logic;

namespace Wordgeon
{
    public abstract class WordgeonEntity : Entity
    {
        public WordgeonEntity(Gameplay gameplay)
        {
            Gameplay = gameplay;
        }

        public Gameplay Gameplay { get; }
    }

    public class GameplayControllerEntity : WordgeonEntity
    {
        public GameplayControllerEntity(Gameplay gameplay) : base(gameplay)
        {
            AddComponent(new GameplayController(gameplay));
        }
    }

    public class DungeonLevelDisplay : WordgeonEntity
    {
        public DungeonLevelDisplay(Gameplay gameplay) : base(gameplay)
        {
            for (int r = 0; r < gameplay.DugeonLevelDim.Y; r++)
                for (int c = 0; c < gameplay.DugeonLevelDim.X; c++)
                {
                    var pos = new Point(c, r);
                    AddChild(new DungeonCellDisplay(gameplay, pos));
                }
        }
    }

    public class DungeonCellDisplay : WordgeonEntity
    {
        private readonly Point dungeonPos;
        private readonly TextSprite letterSprite;

        public DungeonCellDisplay(Gameplay gameplay, Point dungeonPos) : base(gameplay)
        {
            this.dungeonPos = dungeonPos;
            Position = dungeonPos.ToVector2() * (float)Constants.TileDim;
            Sprite = new Sprite("tile", SpriteLayers.Default);
            AddComponent(Sprite);

            var tile = Gameplay.GetDungeonCell(dungeonPos);
            if (!tile.HasValue)
            {
                return;
            }

            ChestSprite = new Sprite("letterchest", SpriteLayers.OccupantTiles) { Enabled = false };
            AddComponent(ChestSprite);

            StairsSprite = new Sprite("stairs", SpriteLayers.OccupantTiles) { Enabled = false };
            AddComponent(StairsSprite);

            letterSprite = new TextSprite("Font", "", SpriteLayers.LetterTiles)
            {
                Enabled = false,
                Color = Color.Black,
            };
            AddComponent(letterSprite);
            AddComponent(new RelayBehavior(UpdateTile));
        }

        public Sprite Sprite { get; }

        public Sprite ChestSprite { get; }
        public Sprite StairsSprite { get; }

        private void UpdateTile()
        {
            var tile = Gameplay.GetDungeonCell(dungeonPos);
            if (!tile.HasValue)
            {
                return;
            }

            letterSprite.Enabled = tile.Bind(v => v.LetterTile).HasValue;
            if (tile.HasValue && tile.Value.LetterTile.HasValue)
            {
                letterSprite.Text = tile.Value.LetterTile.Value.Value.ToString().ToUpper();
            }

            var occupant = tile.Bind(v => v.Occupant).ValueOr(() => null);
            ChestSprite.Enabled = occupant is LetterChest;
            StairsSprite.Enabled = occupant is UpStairs || occupant is DownStairs;


            Sprite.Color = Color.White;
            if (Gameplay.TilePlacer.HasValue)
            {
                var t = tile.Value;
                var tp = Gameplay.TilePlacer.Value;
                if ((tp.TilePlacementDir == Logic.TilePlacementDir.Row && t.Position.Y == tp.LevelPosition.Y && t.Position.X >= tp.LevelPosition.X) ||
                    (tp.TilePlacementDir == Logic.TilePlacementDir.Col && t.Position.X == tp.LevelPosition.X && t.Position.Y >= tp.LevelPosition.Y))
                {
                    Sprite.Color = Color.Green;
                }
            }
        }
    }

    public class PlayerEntity : WordgeonEntity
    {
        public PlayerEntity(Gameplay gameplay) : base(gameplay)
        {
            Sprite = new Sprite("player", SpriteLayers.Player);
            AddComponent(Sprite);
            AddComponent(new RelayBehavior(Update));
            Update();
        }

        public Sprite Sprite { get; }

        private void Update()
        {
            Sprite.Enabled = !Gameplay.TilePlacer.HasValue;
            Position = Gameplay.Player.LevelPosition.ToVector2() * Constants.TileDim;
        }
    }

    public class Hud : WordgeonEntity
    {
        private readonly TextSprite lettersSprite;

        public Hud(Gameplay gameplay) : base(gameplay)
        {
            Position = new Vector2(1000, 800);
            lettersSprite = new TextSprite("Font", "", SpriteLayers.Player);
            AddComponent(lettersSprite);
            AddComponent(new RelayBehavior(Update));
            Update();
        }

        private void Update()
        {
            lettersSprite.Text = new string(Gameplay.Player.LetterInventory.Select(l => l.Value).ToArray());
        }
    }

    public class TilePlacerEntity : WordgeonEntity
    {
        public TilePlacerEntity(Gameplay gameplay) : base(gameplay)
        {
            Sprite = new Sprite("tile_placer", SpriteLayers.Player);
            AddComponent(Sprite);
            AddComponent(new RelayBehavior(Update));
            Update();
        }

        public Sprite Sprite { get; }

        private void Update()
        {
            Sprite.Enabled = Gameplay.TilePlacer.HasValue;
            if (Gameplay.TilePlacer.HasValue)
            {
                var tp = Gameplay.TilePlacer.Value;
                Position = tp.LevelPosition.ToVector2() * Constants.TileDim;
            }
        }
    }
}
