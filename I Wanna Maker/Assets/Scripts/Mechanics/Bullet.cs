using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// The class of bullets.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        private int bulletDamage = 1;

        /// <summary>
        /// If the bullet hits an object, send "BeShot" and the bullet damage to that object, and destroy itself.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D collision) 
        {
            collision.SendMessage("BeShot", bulletDamage, SendMessageOptions.DontRequireReceiver);
            if (collision.tag != "Checkpoint") Destroy(gameObject);
        }
    }
}