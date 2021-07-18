using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 用于移动平台，通常被添加在MobilePlatform物体上。
    /// 暂时无法还原I Wanna原版效果，尚待改进。
    /// </summary>
    public class MobilePlatform : MonoBehaviour
    {
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;

        /// <summary>
        /// 移动向量。
        /// </summary>
        [Tooltip("移动向量。")]
        public Vector3 move;
        /// <summary>
        /// 最大速度。
        /// </summary>
        [Tooltip("最大速度。")]
        public float maxSpeed = 5f;
        /// <summary>
        /// 能移动到的左侧最远位置（相对于起始位置）。
        /// </summary>
        [Tooltip("能移动到的左侧最远位置（相对于起始位置）。")]
        public float startX = -1f;
        /// <summary>
        /// 能移动到的右侧最远位置（相对于起始位置）。
        /// </summary>
        [Tooltip("能移动到的右侧最远位置（相对于起始位置）。")]
        public float endX = 1f;
        /// <summary>
        /// 能移动到的左侧最远实际位置。
        /// </summary>
        private float borderStart;
        /// <summary>
        /// 能移动到的右侧最远实际位置。
        /// </summary>
        private float borderEnd;

        /// <summary>
        /// 初始方向，右:1，左:-1。
        /// </summary>
        [Tooltip("初始方向，1或-1。")]
        public int startDirection = 1;

        private void Start() {
            borderStart = transform.position.x + startX;
            borderEnd = transform.position.x + endX;
            if (startDirection >= 0)
            {
                move = new Vector3(maxSpeed, 0, 0);
            }
            else
            {
                move = new Vector3(-1 * maxSpeed, 0, 0);
            }
        }

        void Update()
        {
            transform.position += (move * Time.deltaTime * 0.5f);
            if(transform.position.x >= borderEnd || transform.position.x <= borderStart)
            {
                move *= -1 ;
            }
        }
    }
}