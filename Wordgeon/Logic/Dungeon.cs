using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordgeon.Logic
{
    public class Dungeon
    {
        private readonly IEnumerable<DungeonLevel> levels;

        private Dungeon(DungeonLevel activeLevel, IEnumerable<DungeonLevel> levels)
        {
            ActiveLevel = activeLevel;
            this.levels = levels;
        }

        public DungeonLevel ActiveLevel { get; }

        public static Dungeon New(Random random)
        {
            var levels = Enumerable.Range(1, Constants.NumberOfLevels)
                .Select(l => DungeonLevel.New(l, random))
                .ToArray();

            return new Dungeon(levels.First(), levels);
        }

        public Maybe<DungeonCell> GetCell(Point dungeonPos) => ActiveLevel.GetCell(dungeonPos);

        internal Dungeon SaveActiveLevel()
        {
            return new Dungeon(
                ActiveLevel,
                levels.Select(l => l.Level == ActiveLevel.Level ? ActiveLevel : l)
                );
        }

        internal Dungeon RestoreActiveLevel()
        {
            return new Dungeon(
                levels.First(l => l.Level == ActiveLevel.Level),
                levels
                );
        }

        public Maybe<Dungeon> PlaceLetterTile(Point levelPosition, LetterTile letterTile)
        {
            var level = ActiveLevel.LayTile(levelPosition, letterTile);
            if (!level.HasValue)
                return Maybe.None<Dungeon>();
            return new Dungeon(
                level.Value,
                levels
                );
        }

        internal Maybe<Dungeon> RemoveLetterTile(Point levelPosition)
        {
            var level = ActiveLevel.RemoveTile(levelPosition);
            if (!level.HasValue)
                return Maybe.None<Dungeon>();
            return new Dungeon(
                level.Value,
                levels
                );
        }

        internal bool AllLetterTilesAreAdjacent() => ActiveLevel.AllLetterTilesAreAdjacent();
    }

    public class DungeonLevel
    {
        private readonly Dictionary<Point, DungeonCell> cells;

        private DungeonLevel(
            int level,
            Dictionary<Point, DungeonCell> cells)
        {
            this.cells = cells;
        }

        public int Level { get; }

        public static DungeonLevel New(int level, Random random)
        {
            var cells = Enumerable.Range(0, Constants.LevelDim)
                .SelectMany(row => Enumerable
                    .Range(0, Constants.LevelDim)
                    .Select(col => new Point(col, row))
                    .Select(pt => new
                    {
                        Point = pt,
                        Cell = DungeonCell.Create(pt, random)
                    })
                )
                .ToDictionary(
                    k => k.Point,
                    v => v.Cell
                );

            if (level == 1)
            {
                var mid = Constants.LevelDim / 2;
                var col = mid - 3;
                foreach (var c in "nothing")
                {
                    var pos = new Point(col, mid);
                    var cell = cells[pos];
                    // this will work, its okay
                    cell = cell.LayTile(new LetterTile(c)).Value;
                    cells[pos] = cell;
                    col++;
                }
            }

            return new DungeonLevel(level, cells);
        }

        internal Maybe<DungeonCell> GetCell(Point dungeonPos)
        {
            if (!cells.ContainsKey(dungeonPos)) return Maybe.None<DungeonCell>();
            return cells[dungeonPos];
        }

        internal Maybe<DungeonLevel> LayTile(Point levelPosition, LetterTile letterTile)
        {
            var cell = GetCell(levelPosition);
            if (!cell.HasValue) return Maybe.None<DungeonLevel>();
            var newCell = cell.Value.LayTile(letterTile);
            if (!newCell.HasValue) return Maybe.None<DungeonLevel>();
            return new DungeonLevel(
                Level,
                cells.ToDictionary(
                    k => k.Key,
                    v => v.Value == cell.Value ? newCell.Value : v.Value
                )
            );
        }

        internal Maybe<DungeonLevel> RemoveTile(Point levelPosition)
        {
            var cell = GetCell(levelPosition);
            if (!cell.HasValue) return Maybe.None<DungeonLevel>();
            var newCell = cell.Value.RemoveTile();
            if (!newCell.HasValue) return Maybe.None<DungeonLevel>();
            return new DungeonLevel(
                Level,
                cells.ToDictionary(
                    k => k.Key,
                    v => v.Value == cell.Value ? newCell.Value : v.Value
                )
            );
        }

        internal bool AllLetterTilesAreAdjacent()
        {
            var allLetterCells = cells.Values.Where(c => c.LetterTile.HasValue).ToList();
            if (!allLetterCells.Any()) return true;

            var sc = allLetterCells.First();
            allLetterCells.Remove(sc);

            CheckAdjacency(sc, new Point(0, -1), allLetterCells);
            CheckAdjacency(sc, new Point(0, 1), allLetterCells);
            CheckAdjacency(sc, new Point(-1, 0), allLetterCells);
            CheckAdjacency(sc, new Point(1, 0), allLetterCells);

            return !allLetterCells.Any();
        }

        private void CheckAdjacency(DungeonCell sc, Point dir, List<DungeonCell> allLetterCells)
        {
            var newPos = sc.Position + dir;
            var cell = GetCell(newPos);
            if (!cell.HasValue || !cell.Value.LetterTile.HasValue) return;
            if (!allLetterCells.Contains(cell.Value))
            {
                return;
            }
            allLetterCells.Remove(cell.Value);
            CheckAdjacency(cell.Value, new Point(0, -1), allLetterCells);
            CheckAdjacency(cell.Value, new Point(0, 1), allLetterCells);
            CheckAdjacency(cell.Value, new Point(-1, 0), allLetterCells);
            CheckAdjacency(cell.Value, new Point(1, 0), allLetterCells);
        }
    }

    public class DungeonCell
    {
        private DungeonCell(
            Point position,
            Maybe<IOccupant> occupant,
            Maybe<LetterTile> letterTile
            )
        {
            Position = position;
            Occupant = occupant;
            LetterTile = letterTile;
        }

        public Point Position { get; }

        public Maybe<IOccupant> Occupant { get; }

        public Maybe<LetterTile> LetterTile { get; }

        public Maybe<DungeonCell> LayTile(LetterTile letterTile)
        {
            if (LetterTile.HasValue) return Maybe.None<DungeonCell>();
            return new DungeonCell(Position, Occupant, letterTile);
        }

        public static DungeonCell Create(Point position, Random random)
        {
            return new DungeonCell(position, Maybe.None<IOccupant>(), Maybe.None<LetterTile>());
        }

        internal Maybe<DungeonCell> RemoveTile()
        {
            if (!LetterTile.HasValue) return this;
            return new DungeonCell(Position, Occupant, Maybe.None<LetterTile>());
        }
    }
}
