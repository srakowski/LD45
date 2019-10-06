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
            var m = (Constants.TileDim * Constants.LevelDim) / 2;
            Position = new Vector2(m, m);
            AddComponent(new Camera());
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

            Rocks = new Sprite("rocks", SpriteLayers.OccupantTiles) { Enabled = false };
            AddComponent(Rocks);

            LetterCrystals = new Sprite("crystals", SpriteLayers.OccupantTiles) { Enabled = false };
            AddComponent(LetterCrystals);

            StairsSprite = new Sprite("stairs", SpriteLayers.OccupantTiles) { Enabled = false };
            AddComponent(StairsSprite);

            BTOYendorSprite = new Sprite("btoy", SpriteLayers.OccupantTiles)
            {
                Enabled = false,
                Origin = new Vector2(32, 40),
            };
            AddComponent(BTOYendorSprite);

            LetterSprite = new Sprite("lettertiles", SpriteLayers.OccupantTiles) { Enabled = false };
            AddComponent(LetterSprite);

            AddComponent(new RelayBehavior(UpdateTile));
        }

        public Sprite Sprite { get; }
        public Sprite LetterSprite { get; }

        public Sprite Rocks { get; }
        public Sprite LetterCrystals { get; }
        public Sprite StairsSprite { get; }
        public Sprite BTOYendorSprite { get; }

        private void UpdateTile()
        {
            var tile = Gameplay.GetDungeonCell(dungeonPos);
            if (!tile.HasValue)
            {
                return;
            }

            LetterSprite.Enabled = tile.Bind(v => v.LetterTile).HasValue;
            if (tile.HasValue && tile.Value.LetterTile.HasValue && LetterSprite.Enabled)
            {
                var c = char.ToUpper(tile.Value.LetterTile.Value.Value);
                var i = c - 'A';
                var r = i / 8;
                var cl = i % 8;
                var pt = (new Point(cl, r).ToVector2() * Constants.TileDim);
                LetterSprite.SourceRectangle = new Rectangle(pt.ToPoint(), new Point(Constants.TileDim, Constants.TileDim));
            }

            var occupant = tile.Bind(v => v.Occupant).ValueOr(() => null);
            Rocks.Enabled = occupant is Rocks;
            LetterCrystals.Enabled = occupant is LetterChest;
            StairsSprite.Enabled = occupant is UpStairs || occupant is DownStairs;
            BTOYendorSprite.Enabled = occupant is BlankTileOfYendor;

            Sprite.Color = Color.White;
            if (Gameplay.TilePlacer.HasValue)
            {
                var t = tile.Value;
                var tp = Gameplay.TilePlacer.Value;
                if ((tp.TilePlacementDir == Logic.TilePlacementDir.Row && t.Position.Y == tp.LevelPosition.Y && t.Position.X >= tp.LevelPosition.X) ||
                    (tp.TilePlacementDir == Logic.TilePlacementDir.Col && t.Position.X == tp.LevelPosition.X && t.Position.Y >= tp.LevelPosition.Y))
                {
                    Sprite.Color = Color.LightGray;
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
            Position = new Vector2(1000, 100);
            lettersSprite = new TextSprite("Font", "", SpriteLayers.Player);
            AddComponent(lettersSprite);
            AddComponent(new RelayBehavior(Update));
            Update();
        }

        private void Update()
        {
            var chars = Gameplay.Player.LetterInventory.Select(l => l.Value).ToArray();
            var values = chars.OrderBy(c => c)
                .GroupBy(c => c)
                .Select(c => new { Char = c.Key, Count = c.Count() })
                .Select(c => $"{char.ToUpper(c.Char)} x {c.Count}");

            lettersSprite.Text = string.Join("\n", values);

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
