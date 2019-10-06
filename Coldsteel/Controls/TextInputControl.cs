using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Coldsteel.Controls
{
    public class TextInputControl : Control<TextInputControlBinding>
    {
        public TextInputControl(string name) : base(name)
        {
        }

        public string InputBuffer(PlayerIndex playerIndex = PlayerIndex.One) =>
            _bindingsByPlayer[(int)playerIndex].FirstOrDefault()?.InputBuffer ?? "";

        public void BeginInput() => InputManager.BeginTextInput();

        public void EndInput() => InputManager.EndTextInput();
    }
}
