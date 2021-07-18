using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 子弹的类，脚本应当被添加在Bullet的预制体上。
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        /// <summary>
        /// 子弹伤害，通常值为1。
        /// </summary>
        private int bulletDamage = 1;

        /// <summary>
        /// 与某物体发生碰撞时的操作。
        /// </summary>
        /// <param name="collision">被射击物体的碰撞器。</param>
        private void OnTriggerEnter2D(Collider2D collision) 
        {
            //向物体发送"BeShot"消息，以调用该物体的BeShot()方法
            collision.SendMessage("BeShot", bulletDamage, SendMessageOptions.DontRequireReceiver); 

            //如果碰上存档点以外的物体，则销毁自身
            if (collision.tag != "Checkpoint") Destroy(gameObject); 
        }
    }
}