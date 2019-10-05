using Coldsteel.Controls;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace WordGame
{
    class Controls
    {
        public const string LetterInput = "LetterInput";

        public static IEnumerable<Control> Create()
        {
            return new Control[]
            {
                new TextInputControl(LetterInput)
                    .AddBinding(new WindowTextInputControlBinding(PlayerIndex.One)),
            };
        }
    }
}
