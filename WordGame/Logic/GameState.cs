using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WordGame.Logic
{
    public class GameState
    {
        private GameState(
            Random random,
            Words words,
            StartsWith startsWith,
            CharBoard charBoard,
            IEnumerable<AttemptResult> attemptResults,
            Player player,
            Encounter encounter,
            IEnumerable<Item> itemsEscrow,
            int xpEscrow)
        {
            Random = random;
            Words = words;
            StartsWith = startsWith;
            CharBoard = charBoard;
            AttemptResults = attemptResults;
            Player = player;
            Encounter = encounter;
            LootEscrow = itemsEscrow;
            XPEscrow = xpEscrow;
            // Debug.WriteLine(string.Join("\n", CharBoard.PossibleWords.Select(c => c.Value).OrderBy(c => c.Length)));
        }

        public Random Random { get; }

        public Words Words { get; }

        public StartsWith StartsWith { get; }

        public CharBoard CharBoard { get; }

        public IEnumerable<AttemptResult> AttemptResults { get; }

        public Player Player { get; }

        public bool PlayerIsAlive => Player.IsAlive;

        public Encounter Encounter { get; }

        public bool EnemyIsAlive => Encounter.ActiveEnemy.HasValue;

        public bool EncounterIsActive => Encounter.HasLivingEnemies;

        public Maybe<Enemy> ActiveEnemy => Encounter.ActiveEnemy;

        public IEnumerable<Item> LootEscrow { get; }

        public int XPEscrow { get; }

        public static GameState New(Words words, string playerName)
        {
            var random = new Random();
            var startsWith = new StartsWith('n', 'o');
            var charBoard = CharBoard.New(random, words, startsWith, "nothing");
            return new GameState(
                random,
                words,
                startsWith,
                charBoard,
                Enumerable.Empty<AttemptResult>(),
                Player.New(playerName, random),
                Encounters.EnterDungeon(random),
                Enumerable.Empty<Item>(),
                0
            );
        }

        public Maybe<GameState> MakeAutoLetterSelection(char letter)
        {
            var maybeCharBoard = CharBoard.MakeAutoLetterSelection(letter);

            if (!maybeCharBoard.HasValue)
            {
                return Maybe.None<GameState>();
            }

            return new GameState(
                Random,
                Words,
                StartsWith,
                maybeCharBoard.Value,
                AttemptResults,
                Player,
                Encounter,
                LootEscrow,
                XPEscrow).ToMaybe();
        }

        public Maybe<GameState> UndoLastSelection()
        {
            var maybeCharBoard = CharBoard.UndoLastSelection();

            if (!maybeCharBoard.HasValue)
            {
                return Maybe.None<GameState>();
            }

            return new GameState(
                Random,
                Words,
                StartsWith,
                maybeCharBoard.Value,
                AttemptResults,
                Player,
                Encounter,
                LootEscrow,
                XPEscrow).ToMaybe();
        }

        public Maybe<GameState> CompleteWord(CombatMode mode)
        {
            if (!CharBoard.HasSelectedCharCells)
            {
                return Maybe.None<GameState>();
            }

            var word = new Word(StartsWith.Value + CharBoard.EndsWith);

            var seed = word.Value.Aggregate(0, (c, a) => (int)c + a);

            var attemptResult = CharBoard.HasWord(word)
                ? AttemptResult.Success(word.Value)
                : AttemptResult.Failure(word.Value);

            var encounter = Encounter;
            var player = Player;

            var items = LootEscrow;
            var xp = XPEscrow;

            if (attemptResult is AttemptResult.SuccessResult)
            {
                if (mode == CombatMode.Attack)
                {
                    encounter = Encounter.TakeMitigatedDamage(Player.AttackDamage(Random, word.Value.Length));
                }
                else if (mode == CombatMode.Defense)
                {
                    var enemyAttack = encounter.ActiveEnemy.Select(e => e.AttackDamage(Random, -word.Value.Length)).ValueOr(() => 0);
                    player = Player.TakeMitigatedDamage(enemyAttack);
                }

                items = items.Concat(CharBoard.CollectItems());
                xp += word.Value.Length;
            }
            else
            {
                if (mode == CombatMode.Defense)
                {
                    var enemyAttack = encounter.ActiveEnemy.Select(e => e.AttackDamage(Random, word.Value.Length)).ValueOr(() => 0);
                    player = Player.TakeMitigatedDamage(enemyAttack);
                }
            }

            var nextStartsWith = Words.GetNextStartsWith(Random, attemptResult is AttemptResult.SuccessResult ? word.ToMaybe() : Maybe.None<Word>());

            return new GameState(
                Random,
                Words,
                nextStartsWith,
                CharBoard.New(Random, Words, nextStartsWith),
                AttemptResults.Prepend(attemptResult),
                player,
                encounter,
                items,
                xp
            ).ToMaybe();
        }

        public Maybe<GameState> CollectWinnings()
        {
            return new GameState(
                Random,
                Words,
                StartsWith,
                CharBoard,
                AttemptResults,
                Player.Progress(XPEscrow, LootEscrow),
                Encounter,
                Enumerable.Empty<Item>(),
                0
                ).ToMaybe();
        }

        public Maybe<GameState> LoadNextEnemy()
        {
            var encounter = Encounter.LoadNextEnemy();
            if (!encounter.HasValue)
            {
                return Maybe.None<GameState>();
            }

            var newCharBoard = CharBoard.CreateLoot(Random, encounter.Value);

            return new GameState(
                Random,
                Words,
                StartsWith,
                newCharBoard,
                AttemptResults,
                Player,
                encounter.Value,
                LootEscrow,
                XPEscrow).ToMaybe();
        }

        public Maybe<GameState> ExecuteEnemyTurn()
        {
            var player = Player;
            var enemyAttack = Encounter.ActiveEnemy.Select(e => e.AttackDamage(Random, 0)).ValueOr(() => 0);
            player = Player.TakeMitigatedDamage(enemyAttack);

            return new GameState(
                Random,
                Words,
                StartsWith,
                CharBoard,
                AttemptResults,
                player,
                Encounter,
                LootEscrow,
                XPEscrow).ToMaybe();
        }
    }
}
