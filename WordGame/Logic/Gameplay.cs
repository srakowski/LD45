namespace WordGame.Logic
{
    public class Gameplay
    {
        public Gameplay()
        {
            Words = new Words();
            Words.Initialize();
            CurrentState = GameState.New(Words, "Noob");
        }

        public Words Words { get; }

        public GameState CurrentState { get; private set; }

    }
}
