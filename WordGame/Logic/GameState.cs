﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WordGame.Logic
{
    public class GameState
    {
        private GameState(
            Words words,
            StartsWith startsWith,
            CharBoard charBoard,
            IEnumerable<AttemptResult> attemptResults,
            Player player,
            Encounter encounter)
        {
            Words = words;
            StartsWith = startsWith;
            CharBoard = charBoard;
            AttemptResults = attemptResults;
            Player = player;
            Encounter = encounter;

            // Debug.WriteLine(string.Join("\n", CharBoard.PossibleWords.Select(c => c.Value).OrderBy(c => c.Length)));
        }

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

        public static GameState New(Words words, string playerName)
        {
            var startsWith = new StartsWith('n', 'o');
            var charBoard = CharBoard.New(words, startsWith, "nothing");
            return new GameState(
                words,
                startsWith,
                charBoard,
                Enumerable.Empty<AttemptResult>(),
                Player.New(playerName),
                Encounters.EnterDungeon()
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
                Words,
                StartsWith,
                maybeCharBoard.Value,
                AttemptResults,
                Player,
                Encounter).ToMaybe();
        }

        public Maybe<GameState> UndoLastSelection()
        {
            var maybeCharBoard = CharBoard.UndoLastSelection();

            if (!maybeCharBoard.HasValue)
            {
                return Maybe.None<GameState>();
            }

            return new GameState(
                Words,
                StartsWith,
                maybeCharBoard.Value,
                AttemptResults,
                Player,
                Encounter).ToMaybe();
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

            var combatValue = CharBoard.GetCombatValue();

            var encounter = Encounter;
            var player = Player;

            if (attemptResult is AttemptResult.SuccessResult)
            {
                if (mode == CombatMode.Attack)
                {
                    encounter = Encounter.TakeDamage(combatValue);
                }
            }
            else
            {
                if (mode == CombatMode.Defense)
                {
                    player = Player.TakeDamage(combatValue);
                }
            }

            var nextStartsWith = Words.GetNextStartsWith(seed, attemptResult is AttemptResult.SuccessResult ? word.ToMaybe() : Maybe.None<Word>());

            return new GameState(
                Words,
                nextStartsWith,
                CharBoard.New(Words, nextStartsWith),
                AttemptResults.Prepend(attemptResult),
                player,
                encounter
            ).ToMaybe();
        }

        public Maybe<GameState> LoadNextEnemy()
        {
            var encounter = Encounter.LoadNextEnemy();
            if (!encounter.HasValue)
            {
                return Maybe.None<GameState>();
            }
            return new GameState(
                Words,
                StartsWith,
                CharBoard,
                AttemptResults,
                Player,
                encounter.Value
                ).ToMaybe();
        }
    }
}
