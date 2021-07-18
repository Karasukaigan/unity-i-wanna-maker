using System;
using Platformer.Core;
using Platformer.Model;
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 此类用来控制、管理游戏中的一些操作和数据。LoadGame、玩家死亡后复活等情况下会重新加载该类。
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        /// <summary>
        /// 传入PlatformerModel。通常用model.player来获取、修改玩家数据。
        /// </summary>
        [Tooltip("PlatformerModel")]
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        /// <summary>
        /// 传入玩家的PlayerController。
        /// </summary>
        [Tooltip("传入玩家的PlayerController。")]
        PlayerController player;

        /// <summary>
        /// 出生点坐标。
        /// </summary>
        [Tooltip("出生点坐标。")]
        public Transform spawnPoint;

        /// <summary>
        /// 当前场景文件名，通常为GameScene。
        /// </summary>
        [Tooltip("场景文件名，通常为GameScene。")]
        public String currentSceneName = "GameScene"; 

        //出生点坐标
        private float playerSpawnX = 0f; 
        private float playerSpawnY = 0f; 
        private float playerSpawnZ = 0f; 

        /// <summary>
        /// 存档编号。
        /// </summary>
        private int archiveNumber;

        private void Start() {
            //获取存档编号
            archiveNumber = PlayerPrefs.GetInt("CurrentArchive", 0);
            //获取出生点坐标
            playerSpawnX = PlayerPrefs.GetFloat("PlayerSpawnX" + archiveNumber, 0.5f); 
            playerSpawnY = PlayerPrefs.GetFloat("PlayerSpawnY" + archiveNumber, -7f); 
            playerSpawnZ = PlayerPrefs.GetFloat("PlayerSpawnZ" + archiveNumber, 0f);

            //Debug.Log("archiveNumber:" + archiveNumber);
            //Debug.Log("PlayerSpawnX" + archiveNumber + ":" + playerSpawnX + 
            //    ", playerSpawnY" + archiveNumber + ":" + playerSpawnY + 
            //    ", playerSpawnZ" + archiveNumber + ":" + playerSpawnZ);

            //如果已经进入关卡（StartGame为1）时，载入当前存档
            if (PlayerPrefs.GetInt("StartGame", 0) == 1)
            {
                player.transform.position = new Vector3(playerSpawnX, playerSpawnY, 0f); //将玩家移动到当前存档里保存的出生点位置
                Camera.main.transform.position = new Vector3(PlayerPrefs.GetFloat("cameraPointPositionX" + archiveNumber, 0.53f), 
                    PlayerPrefs.GetFloat("cameraPointPositionY" + archiveNumber, -0.4f), 
                    PlayerPrefs.GetFloat("cameraPointPositionZ" + archiveNumber, -10f)); //将摄像机移动到对应的的关卡
            }
            //如果尚未进入关卡（StartGame为0）时，载入初始的场景，可以加载存档，或者选择难度开启新游戏
            else
            {
                player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f); //将玩家移动到出生点位置
            }
            
        }

        private void Awake() {
            player = model.player;
        }
        
        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
            if (Input.GetButtonDown("Exit")) QuitGame(); //按下Esc键退出游戏
            if (Input.GetButtonDown("Suicide")) Simulation.Schedule<PlayerDeath>(0); //按下Q键自杀

            //if (Input.GetButtonDown("Spawn") && player.dead == true) 
            if (Input.GetButtonDown("Spawn"))  //按下R键复活或者重置游戏
            {
                //Simulation.Schedule<PlayerSpawn>(2);
                //PlayerSpawn();

                SceneManager.LoadScene (currentSceneName);
            }
        }

        /// <summary>
        /// 退出游戏。
        /// </summary>
        void QuitGame()
        {
            {
                PlayerPrefs.SetInt("StartGame", 0); //将存档状态设置为未开始游戏
                PlayerPrefs.SetInt("Time" + archiveNumber, PlayerPrefs.GetInt("Time" + archiveNumber, 0) + (int)Time.time); //增加存档的游戏时长

                //退出游戏，测试用
                UnityEditor.EditorApplication.isPlaying = false;
                //退出游戏，打包用
                Application.Quit();
            }
        }

        /// <summary>
        /// 复活玩家。已弃用。
        /// </summary>
        void PlayerSpawn()
        {
            player.collider2d.enabled = true;
            player.controlEnabled = false;

            if (player.audioSource && player.respawnAudio) player.audioSource.PlayOneShot(player.respawnAudio);

            player.health.Increment();
            player.dead = false;
            player.Teleport(model.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.InFlight;

            player.animator.SetBool("dead", false);

            //model.virtualCamera.m_Follow = player.transform;
            //model.virtualCamera.m_LookAt = player.transform;

            //Simulation.Schedule<EnablePlayerInput>(2f);
            player.controlEnabled = true;
            player.Show();
        }
    }
}