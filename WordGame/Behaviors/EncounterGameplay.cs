using Coldsteel;
using Coldsteel.Controls;
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

        public EncounterGameplay(Gameplay gameplay, TextSprite statusSprite)
        {
            this.gameplay = gameplay;
            this.statusSprite = statusSprite;
        }

        protected override void Initialize()
        {
            textInputControl = GetControl<TextInputControl>(Controls.LetterInput);
            StartCoroutine(RunEncounter());
        }

        public IEnumerator RunEncounter()
        {
            while (gameplay.EncounterIsActive && gameplay.PlayerIsAlive)
            {
                while (gameplay.PlayerIsAlive && gameplay.EnemyIsAlive)
                {
                    statusSprite.Text = "Attack";
                    while (!RunAttack())
                    {
                        yield return Wait.None();
                    }

                    if (!gameplay.PlayerIsAlive || !gameplay.EnemyIsAlive)
                        break;

                    statusSprite.Text = "Defend";
                    while (!RunDefense())
                    {
                        yield return Wait.None();
                    }
                }

                if (gameplay.PlayerIsAlive)
                {
                    statusSprite.Text = "Continue";
                    while (!RunContinue())
                    {
                        yield return Wait.None();
                    }
                }
            }
        }

        private bool RunAttack()
        {
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
            return done;
        }

        private bool RunDefense()
        {
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
                    done = gameplay.Defend();
                    break;
                }
                else
                {
                    gameplay.MakeAutoLetterSelection(c);
                }
            }
            return done;
        }

        private bool RunContinue()
        {
            var done = false;
            var data = textInputControl.InputBuffer();
            if (data.Any(c => c == '\r'))
            {
                gameplay.LoadNextEnemy();
                done = true;
            }
            return done;
        }
    }
}
