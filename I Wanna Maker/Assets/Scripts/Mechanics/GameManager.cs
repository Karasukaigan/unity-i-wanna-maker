using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// 相当于GameController。但该项目中不使用。
    /// </summary> 
    public class GameManager : MonoBehaviour
    {
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private void Awake() {
            
        }

        void Update()
        {
            
        }
    }
}