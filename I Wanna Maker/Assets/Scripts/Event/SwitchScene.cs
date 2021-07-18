using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Platformer.Event
{
    /// <summary>
    /// 切换场景文件，场景需要预先添加到BuildSetting（文件>生成设置）的场景列表里。
    /// </summary>
    public class SwitchScene : MonoBehaviour
    {
        [Tooltip("场景文件名。")]
        public String nextScene;

        void Start()
        {
            SceneManager.LoadScene (nextScene);
        }
    }
}