using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


namespace Platformer.Mechanics
{
    /// <summary>
    /// 用于加载、控制StartScreen，通常被添加在StartScreenController空物体上。
    /// </summary>
    public class StartScreenController : MonoBehaviour
    {
        [Tooltip("主摄像机。")]
        public Transform mainCamera; 
        private bool isArchiveScreen = false;
        [Tooltip("摄像机锚点1。")]
        public Transform cameraPoint1;
        [Tooltip("摄像机锚点2。")]
        public Transform cameraPoint2;
        [Tooltip("存档选择标识物，通常为SelectArchive预制体。")]
        public Transform selectArchive;
        [Tooltip("存档选择标识物移动锚点，通常大小为3，从Anchor01~03。")]
        public Transform[] uiAnchor = new Transform[3];
        [Tooltip("各存档的游戏难度，通常大小为3，从Difficulty1~3。")]
        public Text[] gameDifficulty = new Text[3];
        [Tooltip("各存档的死亡次数，通常大小为3，从Death1~3。")]
        public Text[] death = new Text[3];
        [Tooltip("各存档的游戏时间，通常大小为3，从Time1~3。")]
        public Text[] gameTime = new Text[3];

        /// <summary>
        /// 用于接受键盘输入，判断该移动到哪个存档。
        /// </summary>
        private float switchArchive;
        /// <summary>
        /// 当前所选择的存档编号。
        /// </summary>
        private int currentArchive = 0; //一共三个存档，Data1:0，Data2:1，Data3:2


        void Start()
        {
            cameraPoint1.position = new Vector3(0.53f, -0.4f, -10f);
            cameraPoint2.position = new Vector3(40.05f, -1f, -10f);
            selectArchive.position = uiAnchor[0].position;

            int[] gameDifficultyInt = new int[3];
            int[] deathInt = new int[3];
            int[] timeInt = new int[3];
            for (int i=0; i<3; i++)
            {
                //读取难度
                gameDifficultyInt[i] = PlayerPrefs.GetInt("GameDifficulty"+i, 0);
                switch(gameDifficultyInt[i])
                {
                    case 0 :
                        gameDifficulty[i].text = "Medium";
                        break; 
                    case 1 :
                        gameDifficulty[i].text = "Hard";
                        break; 
                    case 2 :
                        gameDifficulty[i].text = "Very Hard";
                        break; 
                    case 3 :
                        gameDifficulty[i].text = "Impossible";
                        break; 
                    default :
                        gameDifficulty[i].text = "???";
                        break;
                }

                //读取死亡次数
                deathInt[i] = PlayerPrefs.GetInt("Death"+i, 0);
                death[i].text = "Death:" + deathInt[i];

                //读取游戏时间
                timeInt[i] = PlayerPrefs.GetInt("Time"+i, 0);
                gameTime[i].text = "Time:\r\n" + CalculateTime(timeInt[i]);
            }
        }

        void Update()
        {
            //选择存档
            if (Input.GetButtonDown("Horizontal"))
            {
                switchArchive = Input.GetAxis("Horizontal");
                if (isArchiveScreen)
                {
                    if (switchArchive > 0 && currentArchive < 2)
                    {
                        currentArchive++;
                        selectArchive.position = uiAnchor[currentArchive].position;
                    }
                    else if (switchArchive < 0 && currentArchive > 0)
                    {
                        currentArchive--;
                        selectArchive.position = uiAnchor[currentArchive].position;
                    }
                }
                Debug.Log("currentArchive:"+currentArchive);
            }
            //进入存档选择界面
            else if (Input.GetButtonDown("Jump") && !isArchiveScreen)
            {
                isArchiveScreen = true;
                mainCamera.position = cameraPoint2.position;
            }
            //加载存档
            else if (Input.GetButtonDown("Jump") && isArchiveScreen)
            {
                PlayerPrefs.SetInt("CurrentArchive", currentArchive);
                PlayerPrefs.SetInt("StartGame", 0);
                SceneManager.LoadScene ("GameScene");
            }
            //退出游戏
            else if (Input.GetButtonDown("Exit")) QuitGame();
        }

        void QuitGame()
        {
            {
                PlayerPrefs.SetInt("StartGame", 0);

                //退出游戏，测试用
                UnityEditor.EditorApplication.isPlaying = false;
                //退出游戏，打包用
                Application.Quit();
            }
        }

        /// <summary>
        /// 将时间格式化。
        /// </summary>
        /// <param name="second">游戏时间，单位为秒。</param>
        String CalculateTime(int second)
        {
            int minute = 0;
            int hour = 0;

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }

            return hour + ":" + minute + ":" + second;
        }
    }
}