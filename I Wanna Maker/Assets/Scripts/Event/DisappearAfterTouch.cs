using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// Destroy if touched by the player.
    /// </summary>
    public class DisappearAfterTouch : MonoBehaviour
    {
        public bool disappearNow = true;
        private void IsPlayer(bool isPlayer) 
        {
            if(disappearNow)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this);
            }
            
        }
    }
}

