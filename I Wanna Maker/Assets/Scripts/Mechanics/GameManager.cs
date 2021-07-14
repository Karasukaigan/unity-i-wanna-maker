using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// It has the same function as GameController, but it is no longer used.
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