using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordgeon.Logic
{
    public class Player
    {
        private Player(
            Point levelPos,
            IEnumerable<LetterTile> letterInventory,
            Maybe<BlankTileOfYendor> blankTileOfYendor)
        {
            LevelPosition = levelPos;
            LetterInventory = letterInventory;
            BlankTileOfYendor = blankTileOfYendor;
        }

        public Point LevelPosition { get; }

        public IEnumerable<LetterTile> LetterInventory { get; }

        public Maybe<BlankTileOfYendor> BlankTileOfYendor { get; }

        public static Player New()
        {
            var lvlCenter = Constants.LevelDim / 2;
            return new Player(
                new Point(lvlCenter, lvlCenter),
                Enumerable.Empty<LetterTile>(),
                Maybe.None<BlankTileOfYendor>()
                );
        }

        public Maybe<(Player, DungeonCell)> Action(DungeonCell cell)
        {
            if (!cell.Occupant.HasValue && cell.LetterTile.HasValue)
            {
                return (new Player(cell.Position, LetterInventory, BlankTileOfYendor), cell);
            }

            if  (cell.Occupant.HasValue)
            {
                var occupant = cell.Occupant.Value;
                var player = occupant.InteractWithPlayer(this);

                cell = cell.SetOccupant(Maybe.None<IOccupant>());

                return (player, cell);
            }

            return Maybe.None<(Player, DungeonCell)>();
        }

        internal Player SetBlankTileOfYendor(Player player, BlankTileOfYendor btoy)
        {
            return new Player(LevelPosition, LetterInventory, btoy);
        }

        public Maybe<Player> RemoveLetter(char letter)
        {
            if (!LetterInventory.Any(l => l.Value == letter)) return Maybe.None<Player>();
            var letterTile = LetterInventory.First(l => l.Value == letter);
            return new Player(
                LevelPosition,
                LetterInventory.Where(l => l != letterTile).ToArray(),
                BlankTileOfYendor
                );
        }

        internal Player AddLetterTiles(IEnumerable<LetterTile> enumerable)
        {
            return new Player(
                LevelPosition,
                LetterInventory.Concat(enumerable).ToArray(),
                BlankTileOfYendor
                );
        }

        internal Player SetPosition(Point position)
        {
            return new Player(position,
                LetterInventory,
                BlankTileOfYendor);
        }
    }
}
