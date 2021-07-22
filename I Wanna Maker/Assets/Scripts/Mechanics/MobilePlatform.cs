using System;
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
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;

        private bool onMobilePlatform = false;

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
        private Vector3 borderStart;
        /// <summary>
        /// 能移动到的右侧最远实际位置。
        /// </summary>
        private Vector3 borderEnd;

        /// <summary>
        /// 玩家父Transform。
        /// </summary>
        private Transform playerTransformParent;
        /// <summary>
        /// 玩家Y坐标。
        /// </summary>
        private float playerPositionY;

        /// <summary>
        /// 初始方向，右:1，左:-1。
        /// </summary>
        [Tooltip("初始方向，1或-1。")]
        public int startDirection = 1;

        private void Start() 
        {
            player = model.player;
            playerTransformParent = player.transform.parent;

            //计算移动起点与终点坐标
            borderStart = new Vector3(transform.position.x + startX, transform.position.y, 0f);
            borderEnd = new Vector3(transform.position.x + endX, transform.position.y, 0f);
        }

        void Update()
        {
            //平台移动
            float range = 0.01f;
            if (startDirection >= 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, borderEnd, Time.deltaTime * maxSpeed * 0.5f);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, borderStart, Time.deltaTime * maxSpeed * 0.5f);
            }

            //速度翻转
            if (transform.position.x >= borderEnd.x - range || transform.position.x <= borderStart.x + range)
            {
                startDirection *= -1 ;
            }

            //移动或者脱离平台时取消同步移动
            if (onMobilePlatform)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    player.transform.SetParent(playerTransformParent);
                    onMobilePlatform = false;
                }
                else if (Input.GetButtonDown("Horizontal"))
                {
                    player.transform.SetParent(playerTransformParent);
                }
                else if (Math.Abs(player.transform.position.y - playerPositionY) >= 0.2f)
                {
                    player.transform.SetParent(playerTransformParent);
                    onMobilePlatform = false;
                }
                else if (player.transform.position.x >= transform.position.x + 0.5f || player.transform.position.x <= transform.position.x - 0.5f)
                {
                    player.transform.SetParent(playerTransformParent);
                    onMobilePlatform = false;
                }
                else if (Input.GetButtonUp("Horizontal"))
                {
                    player.transform.SetParent(transform);
                }
            }
        }

        //使玩家同步移动
        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.tag == "Player" && player.transform.position.y >= transform.position.y +0.25f && player.velocity.y <= 0)
            {
                other.transform.SetParent(transform);
                playerPositionY = player.transform.position.y;
                onMobilePlatform = true;
                Debug.Log(gameObject.transform);
            }
        }
    }
}