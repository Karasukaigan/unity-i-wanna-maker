using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the class of Gameover UI.
    /// </summary> 
    public class Gameover : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetButtonDown("Spawn")) 
            {
                Destroy(this.gameObject);
            }
        }
    } 
}