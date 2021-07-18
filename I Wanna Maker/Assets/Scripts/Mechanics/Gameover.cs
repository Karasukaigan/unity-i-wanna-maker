using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 用来标记GameOver的UI，该脚本应该被添加在GameOver预制体上。
    /// </summary> 
    public class Gameover : MonoBehaviour
    {
        void Update()
        {
            //如果按下R键则销毁GameOver的UI
            if (Input.GetButtonDown("Spawn")) Destroy(this.gameObject);
        }
    } 
}