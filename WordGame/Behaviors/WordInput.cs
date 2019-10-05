using Coldsteel;
using Coldsteel.Controls;
using System.Diagnostics;
using System.Linq;
using WordGame.Logic;

namespace WordGame.Behaviors
{
    public class WordInput : Behavior
    {
        private Gameplay gameplay;
        private TextInputControl textInputControl;

        public WordInput(Gameplay gameplay)
        {
            this.gameplay = gameplay;
        }

        protected override void Initialize()
        {
            textInputControl = GetControl<TextInputControl>(Controls.LetterInput);
        }

        protected override void Update()
        {
            var data = textInputControl.InputBuffer();

            //if (data.Any(c => !char.IsLetter(c)))
            //{
            //    Debugger.Break();
            //}

            var enteredLetters = data.Where(c => char.IsLetter(c) || c == '\b' || c == '\r').ToArray();
            foreach (var c  in enteredLetters)
            {
                if (c == '\b')
                {
                    gameplay.UndoLastSelection();
                }
                else if (c == '\r')
                {
                    gameplay.CompleteWord();
                }
                else
                {
                    gameplay.MakeAutoLetterSelection(c);
                }
            }
        }
    }
}
