using Coldsteel.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Wordgeon
{
    class Controls
    {
        public const string TextInput = "LetterInput";
        public const string PlayerUp = "PlayerUp";
        public const string PlayerDown = "PlayerDown";
        public const string PlayerLeft = "PlayerLeft";
        public const string PlayerRight = "PlayerRight";
        public const string PlayerCancel = "PlayerCancel";
        public const string CastTileSpell = "CastTileSpell";
        public const string ChangePlacementDirection = "Change Placement Direction";
        public const string StartWordEntry = "StartWordEntry";
        public const string AnyLetter = "AnyLetter";
        public const string Restart = "Restart";

        public static IEnumerable<Control> Create()
        {
            return new Control[]
            {
                new TextInputControl(TextInput)
                    .AddBinding(new WindowTextInputControlBinding(PlayerIndex.One)),

                new ButtonControl(PlayerUp)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Up)
                        ),

                new ButtonControl(PlayerDown)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Down)
                        ),

                new ButtonControl(PlayerLeft)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Left)
                        ),

                new ButtonControl(PlayerRight)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Right)
                        ),

                new ButtonControl(PlayerCancel)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Escape)
                        ),

                new ButtonControl(CastTileSpell)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Enter)
                        ),


                new ButtonControl(ChangePlacementDirection)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Space)
                        ),

                new ButtonControl(StartWordEntry)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.Enter)
                        ),

                new ButtonControl(Restart)
                    .AddBinding(new KeyboardButtonControlBinding(Keys.F5)),

                new ButtonControl(AnyLetter)
                    .AddBinding(
                        new KeyboardButtonControlBinding(Keys.A),
                        new KeyboardButtonControlBinding(Keys.B),
                        new KeyboardButtonControlBinding(Keys.C),
                        new KeyboardButtonControlBinding(Keys.D),
                        new KeyboardButtonControlBinding(Keys.E),
                        new KeyboardButtonControlBinding(Keys.F),
                        new KeyboardButtonControlBinding(Keys.G),
                        new KeyboardButtonControlBinding(Keys.H),
                        new KeyboardButtonControlBinding(Keys.I),
                        new KeyboardButtonControlBinding(Keys.J),
                        new KeyboardButtonControlBinding(Keys.K),
                        new KeyboardButtonControlBinding(Keys.L),
                        new KeyboardButtonControlBinding(Keys.M),
                        new KeyboardButtonControlBinding(Keys.N),
                        new KeyboardButtonControlBinding(Keys.O),
                        new KeyboardButtonControlBinding(Keys.P),
                        new KeyboardButtonControlBinding(Keys.Q),
                        new KeyboardButtonControlBinding(Keys.R),
                        new KeyboardButtonControlBinding(Keys.S),
                        new KeyboardButtonControlBinding(Keys.T),
                        new KeyboardButtonControlBinding(Keys.U),
                        new KeyboardButtonControlBinding(Keys.V),
                        new KeyboardButtonControlBinding(Keys.W),
                        new KeyboardButtonControlBinding(Keys.X),
                        new KeyboardButtonControlBinding(Keys.Y),
                        new KeyboardButtonControlBinding(Keys.Z)
                        ),
        };
    }
}
}
