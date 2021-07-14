using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// This class is used to launch designated traps.
    /// </summary>
    public class TogeLaunch : MonoBehaviour
    {
        public Rigidbody2D toge; //Rigidbody2D of the trap
        
        //Launch direction
        public float speedX = 0f;
        public float speedY = 10f;
        
        public bool isAvailable = true;
        
        private void OnTriggerEnter2D(Collider2D other) {

            if (other.tag == "Player" && isAvailable)
            {
                Vector2 launchVector = new Vector2(speedX, speedY);
                toge.velocity = launchVector;
            }
        }
    }
}