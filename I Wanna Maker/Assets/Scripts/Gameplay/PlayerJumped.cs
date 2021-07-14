using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player performs a Jump.
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped>
    {
        public PlayerController player;

        public override void Execute()
        {
            if (player.audioSource && player.jumpAudio)
                if(player.jumpTimes == 1){
                    player.audioSource.PlayOneShot(player.jumpAudio);
                }
                if(player.jumpTimes == 2){
                    player.audioSource.PlayOneShot(player.doubleJumpAudio);
                }    
        }
    }
}