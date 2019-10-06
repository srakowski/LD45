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

            levels = PlaceStairs(levels, random);

            return new Dungeon(levels.First(), levels);
        }

        private static DungeonLevel[] PlaceStairs(DungeonLevel[] levels, Random random)
        {
            List<DungeonLevel> newLevels = new List<DungeonLevel>();
            var upStairsForNext = Maybe.None<Point>();
            foreach (var level in levels)
            {
                var lwus = level;
                if (upStairsForNext.HasValue)
                {
                    var setStairsAt = upStairsForNext.Value;
                    var mcell = lwus.GetCell(setStairsAt);
                    if (!mcell.HasValue) break;
                    //var cws = mcell.Value.SetOccupant(new UpStairs());

                    var cws = mcell.Value.LayTile(new LetterTile((char)random.Next((int)'a', (int)'z' + 1))).Value;
                    lwus = lwus.SetCell(cws);
                }

                var nextLevel = levels.FirstOrDefault(l => l.Level == level.Level + 1).ToMaybe();
                if (!nextLevel.HasValue)
                {
                    newLevels.Add(lwus);
                    break;
                }

                var nl = nextLevel.Value;

                var nonOccupiedGoodCells = lwus.Cells
                .Where(c => !c.Occupant.HasValue && !c.LetterTile.HasValue)
                .Where(c => c.Position.X != 0 && c.Position.X != Constants.LevelDim - 1)
                .Where(c => c.Position.Y != 0 && c.Position.Y != Constants.LevelDim - 1);


                var stairCell = nonOccupiedGoodCells
                    .OrderBy(_ => random.Next())
                    .First(l =>
                    {
                        var belowCell = nl.Cells.First(c => c.Position == l.Position);
                        return !belowCell.Occupant.HasValue && !belowCell.LetterTile.HasValue;
                    });

                upStairsForNext = stairCell.Position;

                var sc = stairCell.SetOccupant(new DownStairs());
                var lws = lwus.SetCell(sc);
                newLevels.Add(lws);
            }
            return newLevels.ToArray();
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

        internal Maybe<Dungeon> Ascend()
        {
            var d = SaveActiveLevel();
            var up = d.levels.FirstOrDefault(l => l.Level == ActiveLevel.Level - 1).ToMaybe();
            if (!up.HasValue) return Maybe.None<Dungeon>();
            return new Dungeon(up.Value, d.levels);
        }

        internal Maybe<Dungeon> Descend()
        {
            var d = SaveActiveLevel();
            var down = d.levels.FirstOrDefault(l => l.Level == ActiveLevel.Level + 1).ToMaybe();
            if (!down.HasValue) return Maybe.None<Dungeon>();
            return new Dungeon(down.Value, d.levels);
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

        public Dungeon ReplaceCell(DungeonCell cell)
        {
            return new Dungeon(
                ActiveLevel.SetCell(cell),
                levels);
        }

        internal bool AllLetterTilesAreAdjacent() => ActiveLevel.AllLetterTilesAreAdjacent();

        internal IEnumerable<string> CollectAllWords() => ActiveLevel.CollectAllWords();
    }

    public class DungeonLevel
    {
        private readonly Dictionary<Point, DungeonCell> cells;

        private DungeonLevel(
            int level,
            Dictionary<Point, DungeonCell> cells)
        {
            this.Level = level;
            this.cells = cells;
        }

        public int Level { get; }
        public IEnumerable<DungeonCell> Cells => cells.Values;

        public static DungeonLevel New(int level, Random random)
        {
            var lb = LetterBag.New(random);

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

            var mid = Constants.LevelDim / 2;

            if (level == 1)
            {
                var col = mid - 3;
                foreach (var c in "nothing")
                {
                    var pos = new Point(col, mid);
                    var cell = cells[pos];
                    // this will work, its okay
                    cell = cell.LayTile(new LetterTile(c)).Value;
                    cells[pos] = cell;

                    if (c == 'h')
                    {
                        var pos2 = new Point(col, mid - 1);
                        var cell2 = cells[pos2];
                        var (r, t) = lb.PullTiles(20);
                        lb = r;
                        cell2 = cell2.SetOccupant(new LetterChest(t));
                        cells[pos2] = cell2;
                    }

                    col++;
                }
            }

            if (level == Constants.NumberOfLevels)
            {
                var pos = new Point(mid, mid);
                var cell = cells[pos];
                // this will work, its okay
                cell = cell.SetOccupant(new BlankTileOfYendor());
                cells[pos] = cell;
            }

            var d = new DungeonLevel(level, cells);


            for (int i = 0; i < 24; i++)
            {
                var rcell = cells.Values
                    .Where(n => !n.Occupant.HasValue)
                    .Where(n => n.Position.X < 4 || n.Position.X > (Constants.LevelDim - 4) || n.Position.Y < 4 || n.Position.Y > (Constants.LevelDim - 4))
                    .OrderBy(n => random.Next()).First();
                rcell = rcell.SetOccupant(new Rocks());
                d = d.SetCell(rcell);
            }

            for (int i = 0; i < 4; i++)
            {
                var rcell = cells.Values.Where(n => !n.Occupant.HasValue)
                    .Where(n => n.Position.X < 4 || n.Position.X > (Constants.LevelDim - 4) || n.Position.Y < 4 || n.Position.Y > (Constants.LevelDim - 4))
                    .OrderBy(n => random.Next()).First();

                var (r, t) = lb.PullTiles(random.Next(10, 20));
                lb = r;
                rcell = rcell.SetOccupant(new LetterChest(t));
                d = d.SetCell(rcell);
            }


            return d;
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

        public IEnumerable<string> CollectAllWords()
        {
            var colWords = Enumerable
                .Range(0, Constants.LevelDim)
                .SelectMany(c => new string(
                    Enumerable.Range(0, Constants.LevelDim)
                        .Select(r => cells[new Point(c, r)])
                        .Select(cell => cell.LetterTile.Select(lt => lt.Value).ValueOr(() => ' '))
                        .ToArray()
                    ).Split()
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Where(s => s.Length > 1)
                );

            var rowWords = Enumerable
                .Range(0, Constants.LevelDim)
                .SelectMany(r => new string(
                    Enumerable.Range(0, Constants.LevelDim)
                        .Select(c => cells[new Point(c, r)])
                        .Select(cell => cell.LetterTile.Select(lt => lt.Value).ValueOr(() => ' '))
                        .ToArray()
                    ).Split()
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Where(s => s.Length > 1)
                );

            return colWords.Concat(rowWords);
        }

        internal DungeonLevel SetCell(DungeonCell cell)
        {
            return new DungeonLevel(
                Level,
                cells.ToDictionary(
                    k => k.Key,
                    v => v.Value.Position == cell.Position ? cell : v.Value
                )
            );
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

        internal DungeonCell SetOccupant(Maybe<IOccupant> occupant)
        {
            return new DungeonCell(Position, occupant, LetterTile);
        }

    }
}
