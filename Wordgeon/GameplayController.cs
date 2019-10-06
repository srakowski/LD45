using System;
using System.Collections;
using System.Linq;
using Coldsteel;
using Coldsteel.Controls;

namespace Wordgeon
{
    internal class GameplayController : Behavior
    {
        private readonly Gameplay gameplay;
        private ButtonControl up, down, left, right, changeTileDir, cancel, startWord, castTileSpell, anyLetter;
        private TextInputControl textInputControl;


        public GameplayController(Gameplay gameplay)
        {
            this.gameplay = gameplay;
        }

        protected override void Initialize()
        {
            up = GetControl<ButtonControl>(Controls.PlayerUp);
            down = GetControl<ButtonControl>(Controls.PlayerDown);
            left = GetControl<ButtonControl>(Controls.PlayerLeft);
            right = GetControl<ButtonControl>(Controls.PlayerRight);
            cancel = GetControl<ButtonControl>(Controls.PlayerCancel);
            changeTileDir = GetControl<ButtonControl>(Controls.ChangePlacementDirection);
            startWord = GetControl<ButtonControl>(Controls.StartWordEntry);
            castTileSpell = GetControl<ButtonControl>(Controls.CastTileSpell);
            anyLetter = GetControl<ButtonControl>(Controls.AnyLetter);
            textInputControl = GetControl<TextInputControl>(Controls.TextInput);
            StartCoroutine(RunPlayerInteraction());
        }

        private IEnumerator RunPlayerInteraction()
        {
            while (true)
            {
                if (up.WasPushed())
                {
                    gameplay.ActionPlayer(0, -1);
                }
                else if (down.WasPushed())
                {
                    gameplay.ActionPlayer(0, 1);
                }
                else if (left.WasPushed())
                {
                    gameplay.ActionPlayer(-1, 0);
                }
                else if (right.WasPushed())
                {
                    gameplay.ActionPlayer(1, 0);
                }
                else if (castTileSpell.WasPushed())
                {
                    gameplay.StartTilePlacer();
                }
                else if (changeTileDir.WasPushed())
                {
                    gameplay.AscendIfOnStair();
                }

                if (gameplay.TilePlacer.HasValue)
                {
                    StartCoroutine(RunTilePlacer());
                    break;
                }

                if (gameplay.Player.BlankTileOfYendor.HasValue)
                {
                    StartCoroutine(RunGameOver());
                    break;
                }

                yield return Wait.None();
            }
        }

        private IEnumerator RunGameOver()
        {
            Scene.Load(nameof(SceneFactory.GameOverScene), this.gameplay);
            yield return Wait.None();
        }

        private IEnumerator RunTilePlacer()
        {
            while (gameplay.TilePlacer.HasValue)
            {
                if (up.WasPushed())
                {
                    gameplay.MoveTilePlacer(0, -1);
                }
                else if (down.WasPushed())
                {
                    gameplay.MoveTilePlacer(0, 1);
                }
                else if (left.WasPushed())
                {
                    gameplay.MoveTilePlacer(-1, 0);
                }
                else if (right.WasPushed())
                {
                    gameplay.MoveTilePlacer(1, 0);
                }
                else if (cancel.WasPushed())
                {
                    gameplay.CancelTilePlacer();
                    StartCoroutine(RunPlayerInteraction());
                    break;
                }
                else if (changeTileDir.WasPushed())
                {
                    gameplay.ChangeTilePlacementDirection();
                }
                else if (startWord.WasPushed())
                {
                    gameplay.StartWordEntry();
                    StartCoroutine(RunWordEntry());
                    break;
                }
                else if (anyLetter.WasPushed())
                {
                    var key = char.ToLower((anyLetter.GetBindingPushed() as KeyboardButtonControlBinding).Key.ToString().First());
                    gameplay.StartWordEntry();
                    gameplay.WordEntryLetter(key);
                    StartCoroutine(RunWordEntry());
                    break;
                }
                yield return Wait.None();
            }
        }

        private IEnumerator RunWordEntry()
        {
            textInputControl.BeginInput();
            while (true)
            {
                var done = false;
                var data = textInputControl.InputBuffer();
                var enteredLetters = data.Where(c => char.IsLetter(c) || c == '\b' || c == '\r').ToArray();
                foreach (var c in enteredLetters)
                {
                    if (c == '\b')
                    {
                        gameplay.WordEntryBackspace();
                    }
                    else if (c == '\r')
                    {
                        done = gameplay.CommitWordEntry();
                        break;
                    }
                    else
                    {
                        gameplay.WordEntryLetter(c);
                    }
                }

                if (done) break;

                if (cancel.WasPushed())
                {
                    gameplay.CancelWordEntry();
                    gameplay.CancelTilePlacer();
                    break;
                }

                yield return Wait.None();
            }
            textInputControl.EndInput();
            StartCoroutine(RunPlayerInteraction());
        }
    }
}