using Coldsteel;
using Coldsteel.Controls;
using System;
using System.Collections;
using System.Linq;
using WordGame.Logic;

namespace WordGame.Behaviors
{
    public class EncounterGameplay : Behavior
    {
        private Gameplay gameplay;
        private TextInputControl textInputControl;
        private TextSprite statusSprite;
        private double timeLeft;

        public EncounterGameplay(Gameplay gameplay, TextSprite statusSprite)
        {
            this.gameplay = gameplay;
            this.statusSprite = statusSprite;
        }

        protected override void Initialize()
        {
            textInputControl = GetControl<TextInputControl>(Controls.LetterInput);
            StartCoroutine(RunContinue());
        }

        private IEnumerator RunAttack()
        {
            var timer = StartCoroutine(RunTimer());
            while (true)
            {
                statusSprite.Text = $"Attack {Math.Clamp((int)(timeLeft / 1000), 0, 9)}";
                bool done = false;
                var data = textInputControl.InputBuffer();
                var enteredLetters = data.Where(c => char.IsLetter(c) || c == '\b' || c == '\r').ToArray();
                foreach (var c in enteredLetters)
                {
                    if (c == '\b')
                    {
                        gameplay.UndoLastSelection();
                    }
                    else if (c == '\r')
                    {
                        done = gameplay.Attack();
                        break;
                    }
                    else
                    {
                        gameplay.MakeAutoLetterSelection(c);
                    }
                }
                if (done) break;

                if (timer.IsFinished)
                {
                    gameplay.Fail();
                    break;
                }

                yield return Wait.None();
            }

            timer.Abort();

            if (gameplay.PlayerIsAlive && gameplay.EnemyIsAlive)
            {
                StartCoroutine(RunDefense());
            }
            else
            {
                StartCoroutine(RunContinue());
            }
        }

        private IEnumerator RunDefense()
        {
            var timer = StartCoroutine(RunTimer());
            statusSprite.Text = "Defend";
            while (true)
            {
                statusSprite.Text = $"Defend {Math.Clamp((int)(timeLeft / 1000), 0, 9)}";
                var done = false;
                var data = textInputControl.InputBuffer();
                var enteredLetters = data.Where(c => char.IsLetter(c) || c == '\b' || c == '\r').ToArray();
                foreach (var c in enteredLetters)
                {
                    if (c == '\b')
                    {
                        gameplay.UndoLastSelection();
                    }
                    else if (c == '\r')
                    {
                        done = gameplay.Defend();
                        break;
                    }
                    else
                    {
                        gameplay.MakeAutoLetterSelection(c);
                    }
                }
                if (done) break;

                if (timer.IsFinished)
                {
                    gameplay.Fail();
                    break;
                }

                yield return Wait.None();
            }

            timer.Abort();

            if (gameplay.PlayerIsAlive && gameplay.EnemyIsAlive)
            {
                StartCoroutine(RunAttack());
            }
            else
            {
                StartCoroutine(RunContinue());
            }
        }

        private IEnumerator RunContinue()
        {
            statusSprite.Text = "Continue";
            while (true)
            {
                var data = textInputControl.InputBuffer();
                if (data.Any(c => c == '\r'))
                {
                    gameplay.LoadNextEnemy();
                    break;
                }
                yield return Wait.None();
            }

            if (gameplay.EncounterIsActive && gameplay.PlayerIsAlive)
            {
                StartCoroutine(RunAttack());
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private IEnumerator RunTimer()
        {
            timeLeft = (Constants.TimeToGuessInSecs * 1000);
            while (timeLeft > 0)
            {
                timeLeft -= Delta;
                yield return Wait.None();
            }
        }
    }
}
