using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// 使陷阱振动，提示玩家此处是一个陷阱，也可以用来虚晃一枪。
    /// </summary>
    public class TrapVibration : MonoBehaviour
    {
        /// <summary>
        /// 振动幅度，默认值为0.1，但是0.05效果可能会更好。
        /// </summary>
        [Tooltip("振动幅度。")]
        public float coordinateOffset = 0.1f;

        /// <summary>
        /// 振动周期，默认值为0.1，但是0.05效果可能会更好。
        /// </summary>
        [Tooltip("振动周期。")]
        public float cycle = 0.1f;

        /// <summary>
        /// 计时器。
        /// </summary>
        private float timer = 0f;

        /// <summary>
        /// 用来判断坐标移动方向，-1为左，1为右。
        /// </summary>
        private int flag = -1;

        /// <summary>
        /// 陷阱初始坐标。
        /// </summary>
        private Transform initialPosition;

        private void Start() {
            initialPosition = transform;
        }

        void Update()
        {
            if (timer > 0)
            {  
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                flag *= -1;
                transform.position = new Vector3(initialPosition.position.x + (coordinateOffset * flag), transform.position.y, 0);
                timer = cycle;
            }
        }
    }
}