using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.Core;
using Platformer.Model;
using Platformer.Mechanics;

namespace Platformer.Event
{
    /// <summary>
    /// 用于载入关卡，通常添加在各游戏难度或者LoadGame的传送门上。
    /// </summary>
    public class EnterLevel : MonoBehaviour
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;

        /// <summary>
        /// 当前场景文件的文件名，通常为“GameScene”。
        /// </summary>
        [Tooltip("当前场景文件的文件名，通常为“GameScene”。")]
        public String currentSceneName = "GameScene"; 

        /// <summary>
        /// 关卡出生点的位置信息。
        /// </summary>
        [Tooltip("关卡出生点。")]
        public Transform levelStartPointPosition;

        /// <summary>
        /// 摄像机的位置信息。
        /// </summary>
        [Tooltip("摄像机锚点。")]
        public Transform cameraPointPosition;

        [Tooltip("是否开放。")]
        public bool isAvailable = true; //关卡是否开放

        /// <summary>
        /// 是否载入存档以继续游戏。
        /// </summary>
        [Tooltip("是否载入存档。仅LoadGame传送门可以选为True。")]
        public bool isLoadGame = false;

        /// <summary>
        /// 关卡难度，0:Medium，1:Hard，2:VeryHard，3:Impossible。
        /// </summary>
        [Tooltip("关卡难度，0~3。")]
        public int gameDifficulty = 0;

        /// <summary>
        /// 出生点坐标。
        /// </summary>
        private float playerSpawnX; 
        private float playerSpawnY; 
        private float playerSpawnZ; 

        /// <summary>
        /// 存档编号，0~2。
        /// </summary>
        private int archiveNumber;

        /// <summary>
        /// 与玩家发生碰撞时触发，执行载入关卡的操作。
        /// </summary>
        /// <param name="collider">与其发生碰撞的碰撞器。</param>
        private void OnTriggerEnter2D(Collider2D collider) 
        {
            player = model.player;

            //将StartGame设置为已进入关卡
            PlayerPrefs.SetInt("StartGame", 1);

            //获取存档编号
            archiveNumber = PlayerPrefs.GetInt("CurrentArchive", 0);

            //读取存档里的玩家和摄像机坐标
            playerSpawnX = PlayerPrefs.GetFloat("PlayerSpawnX" + archiveNumber, 0.5f); 
            playerSpawnY = PlayerPrefs.GetFloat("PlayerSpawnY" + archiveNumber, -7f); 
            playerSpawnZ = PlayerPrefs.GetFloat("PlayerSpawnZ" + archiveNumber, 0f); 
            playerSpawnX = PlayerPrefs.GetFloat("cameraPointPositionX" + archiveNumber, 0.53f); 
            playerSpawnY = PlayerPrefs.GetFloat("cameraPointPositionY" + archiveNumber, -0.4f); 
            playerSpawnZ = PlayerPrefs.GetFloat("cameraPointPositionZ" + archiveNumber, -10f); 

            //如果关卡开放且碰到了玩家
            if (isAvailable && collider.tag == "Player")
            {
                //以载入形式进入关卡
                if (isLoadGame)
                {
                    SceneManager.LoadScene (currentSceneName); //重新载入场景文件
                    //player.transform.position = new Vector3(playerSpawnX, playerSpawnY, playerSpawnZ); //将玩家移动到之前的储存点
                    //Camera.main.transform.position = new Vector3(PlayerPrefs.GetFloat("cameraPointPositionX", cameraPointPosition.position.x), 
                    //    PlayerPrefs.GetFloat("cameraPointPositionY", cameraPointPosition.position.y), 
                    //    -10f); //将摄像机移到之前的关卡，已弃用
                }
                //开始新游戏
                else
                {
                    PlayerPrefs.SetInt("GameDifficulty" + archiveNumber, gameDifficulty); //设置关卡难度

                    //保存出生点坐标，以及摄像机坐标
                    PlayerPrefs.SetFloat("PlayerSpawnX" + archiveNumber, levelStartPointPosition.position.x);
                    PlayerPrefs.SetFloat("PlayerSpawnY" + archiveNumber, levelStartPointPosition.position.y);
                    PlayerPrefs.SetFloat("PlayerSpawnZ" + archiveNumber, 0f);
                    PlayerPrefs.SetFloat("cameraPointPositionX" + archiveNumber, cameraPointPosition.position.x);
                    PlayerPrefs.SetFloat("cameraPointPositionY" + archiveNumber, cameraPointPosition.position.y);
                    PlayerPrefs.SetFloat("cameraPointPositionZ" + archiveNumber, -10f);

                    SceneManager.LoadScene (currentSceneName); //重新载入场景文件

                    //player.transform.position = levelStartPointPosition.position; //将玩家移动到关卡起始点，已弃用
                    //Camera.main.transform.position = cameraPointPosition.position; //将摄像机移到对应关卡，已弃用
                }
            }
        }
    }
}