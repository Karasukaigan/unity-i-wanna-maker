using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 当玩家重生时触发，已弃用。
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            player.collider2d.enabled = true;
            player.controlEnabled = false; //禁止操作

            //播放复活音效
            if (player.audioSource && player.respawnAudio) player.audioSource.PlayOneShot(player.respawnAudio);

            player.health.Increment(); //重置HP
            player.dead = false;
            player.Teleport(model.spawnPoint.transform.position); //传送玩家到出生点
            player.jumpState = PlayerController.JumpState.InFlight; //设置跳跃状态为下落


            player.animator.SetBool("dead", false); //改变动画器dead值为false

            //model.virtualCamera.m_Follow = player.transform;
            //model.virtualCamera.m_LookAt = player.transform;

            //Simulation.Schedule<EnablePlayerInput>(2f);
            player.controlEnabled = true; //允许操作
            player.Show(); //显示玩家        
        }
    }
}