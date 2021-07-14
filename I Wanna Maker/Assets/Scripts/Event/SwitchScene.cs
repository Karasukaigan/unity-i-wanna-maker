using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace Platformer.Event
{
    /// <summary>
    /// Load another scene.
    /// </summary>
    public class SwitchScene : MonoBehaviour
    {
        public String nextScene;

        void Start()
        {
            SceneManager.LoadScene (nextScene);
        }
    }
}