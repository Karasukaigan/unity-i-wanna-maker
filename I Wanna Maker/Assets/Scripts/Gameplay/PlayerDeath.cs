using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// 当玩家死亡时触发。
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        System.Random rd = new System.Random();

        /// <summary>
        /// 添加Blood01到03，三个预制体，用来实现血液喷溅的粒子效果。
        /// </summary>
        [Tooltip("Blood01到03，三个预制体。")]
        GameObject thisBlood;

        /// <summary>
        /// 玩家死亡后，头的发射速度。
        /// </summary>
        [Tooltip("头的发射速度。")]
        Vector2 launchSpeed;

        /// <summary>
        /// 存档编号。
        /// </summary>
        private int archiveNumber;
        
        public override void Execute()
        {
            var player = model.player;

            //获取存档编号
            archiveNumber = PlayerPrefs.GetInt("CurrentArchive", 0);
            
            if (player.health.IsAlive)
            {
                player.Hide(); //隐藏玩家
                
                player.health.Die(); //执行Health类里的Die()

                //model.virtualCamera.m_Follow = null;
                //model.virtualCamera.m_LookAt = null;

                player.controlEnabled = false; //禁用操作
                player.playerDeadTimer = 0.8f; //重置计时器，该计时器用来实现“GameOver”的延迟显示
                player.dead = true;

                //增加当前存档的死亡次数
                PlayerPrefs.SetInt("Death" + archiveNumber, PlayerPrefs.GetInt("Death" + archiveNumber, 0) + 1);
                
                //播放音效
                if (player.audioSource && player.ouchAudio) player.audioSource.PlayOneShot(player.ouchAudio);

                //改变动画器参数
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);

                //在玩家位置生成一个头
                GameObject.Instantiate(player.head, new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, 0f), Quaternion.identity);
                //喷血
                SpurtingBlood();
            }
        }

        /// <summary>
        /// 实现了血液喷溅的粒子效果。
        /// </summary>
        public void SpurtingBlood()
        {
            var player = model.player;

            //定义不同的发射角度
            float [,] launchAngle = new float[11,2] {
                {0.97f, 0.26f} ,
                {0.87f, 0.5f} ,
                {0.71f, 0.71f} ,
                {0.5f, 0.87f} ,
                {0.26f, 0.97f} ,
                {0f, 1f} ,
                {-0.26f, 0.97f} ,
                {-0.5f, 0.87f} ,
                {-0.71f, 0.71f} ,
                {-0.87f, 0.5f} ,
                {-0.97f, 0.26f} 
            };

            //生成500个粒子，角度和速度皆为随机
            for (int i = 0; i < 500; i++)
            {
                int x = rd.Next(0, 11); //随机发射角度
                int y = rd.Next(0, 20); //随机发射速度
                float[] thisLaunchAngle = {launchAngle[x,0], launchAngle[x,1]}; //设置发射角度
                launchSpeed = new Vector2(y * thisLaunchAngle[0] + (rd.Next(0, 30) * 0.1f), 
                    y * thisLaunchAngle[1] + (rd.Next(0, 10) * 0.1f)); //设置发射速度

                thisBlood = GameObject.Instantiate(player.blood[rd.Next(0,3)],
                    new Vector3(player.transform.position.x, player.transform.position.y - 0.2f, 0f), Quaternion.identity); //生成粒子
                thisBlood.GetComponent<Rigidbody2D>().velocity = launchSpeed; //赋予粒子速度
            }
        }
    }
}