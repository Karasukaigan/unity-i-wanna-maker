using Platformer.Core;
using Platformer.Mechanics;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 当玩家跳跃时触发。
    /// </summary>
    /// <typeparam name="PlayerJumped"></typeparam>
    public class PlayerJumped : Simulation.Event<PlayerJumped>
    {
        public PlayerController player;

        public override void Execute()
        {
            //判断是一段跳还是二段跳，然后播放相对应的音频
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