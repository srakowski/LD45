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
        };
    }
}
}
