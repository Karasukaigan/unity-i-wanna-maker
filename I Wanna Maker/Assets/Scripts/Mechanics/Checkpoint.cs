using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 存档点的类，脚本应当被添加在Checkpoint物体上。
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;
        internal Animator animator;
        private Transform cameraPointPosition;

        /// <summary>
        /// 游戏难度。
        /// </summary>
        private int gameDifficulty; 

        /// <summary>
        /// 存档点编号，从0开始。
        /// </summary>
        [Tooltip("存档点编号。")]
        public int checkpointNumber = 0;
        //设置当前存档点在各难度下是否开放存档功能
        [Tooltip("Medium难度下是否开放。")]
        public bool mediumIsAvailable = true;
        [Tooltip("Hard难度下是否开放。")]
        public bool hardIsAvailable = true;
        [Tooltip("VeryHard难度下是否开放。")]
        public bool veryHardIsAvailable = true;
        [Tooltip("Impossible难度下是否开放。")]
        public bool impossibleIsAvailable = true;

        /// <summary>
        /// 是否可用。
        /// </summary>
        private bool isAvailable = false;

        /// <summary>
        /// 是否被射击。
        /// </summary>
        private bool beShot = false;

        /// <summary>
        /// 存档点点亮时间，默认为1。
        /// </summary>
        private float lightTime = 1f;

        /// <summary>
        /// 玩家出生点X轴坐标。
        /// </summary>
        private float playerSpawnX = 0f; 
        /// <summary>
        /// 玩家出生点Y轴坐标。
        /// </summary>
        private float playerSpawnY = 0f; 
        /// <summary>
        /// 相对于出生点坐标的X轴方向偏移量，用于调整出生点相对于存档点的位置，通常为0。
        /// </summary>
        [Tooltip("相对于出生点坐标的X轴方向偏移量。")]
        public float offsetX = 0f;
        /// <summary>
        /// 相对于出生点坐标的Y轴方向偏移量，用于调整出生点相对于存档点的位置，通常为0。
        /// </summary>
        [Tooltip("相对于出生点坐标的Y轴方向偏移量。")]
        public float offsetY = 0f;

        /// <summary>
        /// 存档编号。
        /// </summary>
        private int archiveNumber;

        private void Start() {
            animator = GetComponent<Animator>();

            archiveNumber = PlayerPrefs.GetInt("CurrentArchive", 0); //获取存档编号
            gameDifficulty = PlayerPrefs.GetInt("GameDifficulty" + archiveNumber, 0); //获取当前存档的游戏难度

            if ((gameDifficulty == 0 && mediumIsAvailable) || (gameDifficulty == 1 && hardIsAvailable) || (gameDifficulty == 2 && veryHardIsAvailable) || (gameDifficulty == 3 && impossibleIsAvailable))
            {
                isAvailable = true; //设置为可用
            }

            //如果是已经存过档的检查点，则顶上的文字从“WUSS”变成“SAVE”
            if (checkpointNumber == PlayerPrefs.GetInt("CurrentCheckpoint" + archiveNumber, 0))
            {
                animator.SetBool("isSaved", true);
            }
        }

        /// <summary>
        /// 被子弹击中后销毁自身。当收到“BeShot”消息时触发。
        /// </summary>
        /// <param name="bulletDamage">子弹的伤害。</param>
        private void BeShot(int bulletDamage) 
        {
            if (isAvailable)
            {
                beShot = true;
                lightTime = 1f; //重置计时器，用来维持点亮状态

                //计算出生点位置
                playerSpawnX = this.transform.position.x + offsetX; 
                playerSpawnY = this.transform.position.y + offsetY; 

                //获取摄像机位置
                cameraPointPosition = Camera.main.transform;

                //切换为点亮状态
                animator.SetBool("beShot", true);

                //更新出生点
                PlayerPrefs.SetFloat("PlayerSpawnX" + archiveNumber, playerSpawnX);
                PlayerPrefs.SetFloat("PlayerSpawnY" + archiveNumber, playerSpawnY);
                PlayerPrefs.SetFloat("PlayerSpawnZ" + archiveNumber, 0f);
                PlayerPrefs.SetInt("CurrentCheckpoint" + archiveNumber, checkpointNumber);

                //更新摄像机位置
                PlayerPrefs.SetFloat("cameraPointPositionX" + archiveNumber, cameraPointPosition.position.x);
                PlayerPrefs.SetFloat("cameraPointPositionY" + archiveNumber, cameraPointPosition.position.y);
                PlayerPrefs.SetFloat("cameraPointPositionZ" + archiveNumber, -10f);
            }
        }

        protected void Update()
        {
            animator = GetComponent<Animator>();

            //当被射击时，启动计时器
            if (beShot)
            {
                lightTime -= Time.deltaTime;
            } 
            //计时器时间小于等于0时，熄灭存档点
            if (lightTime <= 0)
            {
                beShot = false;
                lightTime = 1f;
                animator.SetBool("beShot", false); 
                Debug.Log("与移动平台碰撞");
            }
        }
    }
}

