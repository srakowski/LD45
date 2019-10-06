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
            LetterBag letterBag
            )
        {
            LevelPosition = levelPos;
            LetterInventory = letterInventory;
            LetterBag = letterBag;
        }

        public Point LevelPosition { get; }

        public IEnumerable<LetterTile> LetterInventory { get; }

        public LetterBag LetterBag { get; }

        public static Player New(Random random)
        {
            var letterbag = LetterBag.New(random);
            var (lb, tiles) = letterbag.PullTiles(7);
            var lvlCenter = Constants.LevelDim / 2;
            return new Player(
                new Point(lvlCenter, lvlCenter),
                tiles,
                lb
                );
        }

        public Maybe<Player> Action(DungeonCell cell)
        {
            if (!cell.Occupant.HasValue && cell.LetterTile.HasValue)
            {
                return new Player(cell.Position, LetterInventory, LetterBag);
            }

            return Maybe.None<Player>();
        }

        public Maybe<Player> RemoveLetter(char letter)
        {
            if (!LetterInventory.Any(l => l.Value == letter)) return Maybe.None<Player>();
            var letterTile = LetterInventory.First(l => l.Value == letter);
            return new Player(
                LevelPosition,
                LetterInventory.Where(l => l != letterTile).ToArray(),
                LetterBag
                );
        }

        internal Player AddLetterTiles(IEnumerable<LetterTile> enumerable)
        {
            return new Player(
                LevelPosition,
                LetterInventory.Concat(enumerable).ToArray(),
                LetterBag
                );
        }
    }
}
