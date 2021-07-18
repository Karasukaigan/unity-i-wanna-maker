using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;
using Platformer.Gameplay;

namespace Platformer.Event
{
    /// <summary>
    /// 这个类用来移动摄像机，实现了场景切换，通常配合EventZone使用。
    /// </summary>
    public class MoveCamera : MonoBehaviour
    {
        /// <summary>
        /// 需要摄像机移动到的指定锚点。
        /// </summary>
        [Tooltip("摄像机锚点。")]
        public Transform pointPosition;

        [Tooltip("是否可用。")]
        public bool isAvailable = true;

        /// <summary>
        /// 与玩家发生碰撞时触发，移动摄像机。
        /// </summary>
        /// <param name="collider">玩家的碰撞器。</param>
        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.tag == "Player" && isAvailable)
            {
                Camera.main.transform.position = pointPosition.position;
            }
        }
    }
}