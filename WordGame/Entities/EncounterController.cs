using Coldsteel;
using WordGame.Behaviors;
using WordGame.Logic;

namespace WordGame.Entities
{
    public class EncounterController : Entity
    {
        private Gameplay gameplay;

        public EncounterController(Gameplay gameplay)
        {
            this.gameplay = gameplay;
            AddComponent(new WordInput(gameplay));
        }
    }
}
