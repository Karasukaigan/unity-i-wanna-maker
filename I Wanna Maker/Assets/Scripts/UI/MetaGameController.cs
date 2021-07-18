using Platformer.Mechanics;
using Platformer.UI;
using UnityEngine;

namespace Platformer.UI
{
    /// <summary>
    /// 用于主菜单和游戏系统间的切换控制。
    /// 这个类来自平台游戏Microgame模板。
    /// </summary>
    public class MetaGameController : MonoBehaviour
    {
        /// <summary>
        /// 用于菜单的主要UI对象。
        /// </summary>
        public MainUIController mainMenu;

        /// <summary>
        /// 在游戏过程中使用的画布对象列表（当主用户界面关闭时）
        /// </summary>
        public Canvas[] gamePlayCanvasii;

        /// <summary>
        /// 游戏控制器。
        /// </summary>
        public GameController gameController;

        bool showMainCanvas = false;

        void OnEnable()
        {
            _ToggleMainMenu(showMainCanvas);
        }

        /// <summary>
        /// 打开或关闭主菜单。
        /// </summary>
        /// <param name="show"></param>
        public void ToggleMainMenu(bool show)
        {
            if (this.showMainCanvas != show)
            {
                _ToggleMainMenu(show);
            }
        }

        void _ToggleMainMenu(bool show)
        {
            if (show)
            {
                Time.timeScale = 0;
                mainMenu.gameObject.SetActive(true);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                mainMenu.gameObject.SetActive(false);
                foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
            }
            this.showMainCanvas = show;
        }

        void Update()
        {
            if (Input.GetButtonDown("Menu"))
            {
                ToggleMainMenu(show: !showMainCanvas);
            }
        }
    }
}
