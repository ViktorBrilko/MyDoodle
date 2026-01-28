using Gameplay.Boosts;

namespace Gameplay.Signals
{
    public class PlayerGetJetpackSignal
    {
        public Jetpack Jetpack { get; }

        public PlayerGetJetpackSignal(Jetpack jetpack)
        {
            Jetpack = jetpack;
        }
    }
}