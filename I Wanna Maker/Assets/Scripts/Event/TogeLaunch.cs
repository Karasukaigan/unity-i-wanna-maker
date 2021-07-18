using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// 用来发射陷阱，通常添加在EventZone上，用来发射刺、钉板和苹果。
    /// </summary>
    public class TogeLaunch : MonoBehaviour
    {
        [Tooltip("陷阱的刚体。")]
        public Rigidbody2D toge;

        /// <summary>
        /// 水平方向速度。值为负，则向左发射；值为正，则向右发射。
        /// </summary>
        [Tooltip("水平方向速度。")]
        public float speedX = 0f;
        /// <summary>
        /// 垂直方向速度。值为负，则向下发射；值为正，则向上发射。
        /// </summary>
        [Tooltip("垂直方向速度。")]
        public float speedY = 10f;

        [Tooltip("是否可用。")]
        public bool isAvailable = true;
        
        /// <summary>
        /// 与玩家发生碰撞时赋予陷阱速度。
        /// </summary>
        /// <param name="collider">玩家的碰撞器。</param>
        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.tag == "Player" && isAvailable)
            {
                Vector2 launchVector = new Vector2(speedX, speedY);
                toge.velocity = launchVector;
            }
        }
    }
}