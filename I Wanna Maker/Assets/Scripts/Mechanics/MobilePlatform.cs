using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    public class MobilePlatform : MonoBehaviour
    {
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;

        public Vector3 move;
        public float maxSpeed = 5f;
        public float startX = -1;
        public float endX = 1;
        private float borderStart;
        private float borderEnd;
        public int startDirection = 1; //Initial direction, right: 1, left: -1

        private void Start() {
            borderStart = transform.position.x + startX;
            borderEnd = transform.position.x + endX;
            if (startDirection >= 0)
            {
                move = new Vector3(maxSpeed, 0, 0);
            }
            else
            {
                move = new Vector3(-1 * maxSpeed, 0, 0);
            }
        }

        void Update()
        {
            transform.position += (move * Time.deltaTime * 0.5f);
            if(transform.position.x >= borderEnd || transform.position.x <= borderStart)
            {
                move *= -1 ;
            }
        }
    }
}