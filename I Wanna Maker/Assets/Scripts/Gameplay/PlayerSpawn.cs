using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// It comes from the Platformer Microgame template. No longer used.
    /// </summary>
    public class PlayerSpawn : Simulation.Event<PlayerSpawn>
    {
        
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute()
        {
            var player = model.player;
            player.collider2d.enabled = true;
            player.controlEnabled = false; //禁止操作

            if (player.audioSource && player.respawnAudio) player.audioSource.PlayOneShot(player.respawnAudio);

            player.health.Increment(); //重置HP
            player.dead = false;
            player.Teleport(model.spawnPoint.transform.position); //传送
            //player.transform.position = model.spawnPoint.transform.position;
            player.jumpState = PlayerController.JumpState.InFlight;


            player.animator.SetBool("dead", false); //改变动画器dead值为false

            //model.virtualCamera.m_Follow = player.transform;
            //model.virtualCamera.m_LookAt = player.transform;

            //Simulation.Schedule<EnablePlayerInput>(2f);
            player.controlEnabled = true;
            player.Show();            
        }
    }
}