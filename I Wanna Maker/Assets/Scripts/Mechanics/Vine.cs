using UnityEngine;using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    public class Vine : MonoBehaviour
    {
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            var player = model.player;
            if(collider.tag == "Player" && !player.onVine && !player.IsGrounded)
            {
                player.velocity.x = 0;
                player.velocity.y = 0;
                player.vinePosition = player.transform.position.x;
                player.animator.SetBool("onVine", true);
                player.onVine = true;
                Debug.Log("velocity.x:"+player.velocity.x+", velocity.y:"+player.velocity.y+", onVine:"+player.onVine);
            }
        }
    }
}