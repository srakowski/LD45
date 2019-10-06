using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordgeon.Logic
{
    public class TilePlacer
    {
        public TilePlacer(
            Point dungeonPosition,
            Player player,
            TilePlacementDir tileDir,
            IEnumerable<Maybe<LetterTile>> wordEntry)
        {
            LevelPosition = dungeonPosition;
            Player = player;
            TilePlacementDir = tileDir;
            WordEntry = wordEntry;
        }

        public Point LevelPosition { get; }

        public Player Player { get; }

        public TilePlacementDir TilePlacementDir { get; }

        public IEnumerable<Maybe<LetterTile>> WordEntry { get; }

        internal Maybe<TilePlacer> Move(DungeonCell cell)
        {
            return new TilePlacer(cell.Position, Player, TilePlacementDir, WordEntry);
        }

        internal Maybe<TilePlacer> ChangePlacementDirection()
        {
            return new TilePlacer(LevelPosition, Player, TilePlacementDir == TilePlacementDir.Col ? TilePlacementDir.Row : TilePlacementDir.Col, WordEntry);
        }

        internal TilePlacer AdvanceWord(Maybe<LetterTile> maybeTile)
        {
            return new TilePlacer(
                LevelPosition + (TilePlacementDir == TilePlacementDir.Row ? new Point(1, 0) : new Point(0, 1)),
                Player,
                TilePlacementDir,
                WordEntry.Append(maybeTile)
                );
        }

        internal TilePlacer FlushLetterTiles()
        {
            return new TilePlacer(LevelPosition, Player, TilePlacementDir, Enumerable.Empty<Maybe<LetterTile>>());
        }

        internal TilePlacer RegressWord()
        {
            if (!WordEntry.Any()) return this;

            return new TilePlacer(
                LevelPosition - (TilePlacementDir == TilePlacementDir.Row ? new Point(1, 0) : new Point(0, 1)),
                Player,
                TilePlacementDir,
                WordEntry.SkipLast(1).ToArray()
                );
        }
    }

    public enum TilePlacementDir
    {
        Row,
        Col
    }
}