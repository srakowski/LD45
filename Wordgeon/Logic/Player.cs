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
            IEnumerable<LetterTile> letterInventory
            )
        {
            LevelPosition = levelPos;
            LetterInventory = letterInventory;
        }

        public Point LevelPosition { get; }

        public IEnumerable<LetterTile> LetterInventory { get; }

        public static Player New()
        {
            var lvlCenter = Constants.LevelDim / 2;
            return new Player(
                new Point(lvlCenter, lvlCenter),
                Enumerable.Empty<LetterTile>()
                );
        }

        public Maybe<(Player, DungeonCell)> Action(DungeonCell cell)
        {
            if (!cell.Occupant.HasValue && cell.LetterTile.HasValue)
            {
                return (new Player(cell.Position, LetterInventory), cell);
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

        public Maybe<Player> RemoveLetter(char letter)
        {
            if (!LetterInventory.Any(l => l.Value == letter)) return Maybe.None<Player>();
            var letterTile = LetterInventory.First(l => l.Value == letter);
            return new Player(
                LevelPosition,
                LetterInventory.Where(l => l != letterTile).ToArray()
                );
        }

        internal Player AddLetterTiles(IEnumerable<LetterTile> enumerable)
        {
            return new Player(
                LevelPosition,
                LetterInventory.Concat(enumerable).ToArray()
                );
        }

        internal Player SetPosition(Point position)
        {
            return new Player(position, LetterInventory);
        }
    }
}
