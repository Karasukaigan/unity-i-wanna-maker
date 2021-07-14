using UnityEngine;
using Platformer.Core;
using Platformer.Model;

namespace Platformer.Mechanics
{
    /// <summary>
    /// The class of checkpoints.
    /// </summary>
    public class Checkpoint : MonoBehaviour
    {
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;
        internal Animator animator;
        private Transform cameraPointPosition;

        private int gameDifficulty; 

        public bool mediumIsAvailable = true;
        public bool hardIsAvailable = true;
        public bool veryHardIsAvailable = true;
        public bool impossibleIsAvailable = true;
        private bool isAvailable = false;

        private bool beShot = false;
        private float lightTime = 1f;

        private float playerSpawnX = 0f; 
        private float playerSpawnY = 0f; 
        public float offsetX = 0f;
        public float offsetY = 0f;

        private void Start() {
            gameDifficulty = PlayerPrefs.GetInt("GameDifficulty", 0); 
            if ((gameDifficulty == 0 && mediumIsAvailable) || (gameDifficulty == 1 && hardIsAvailable) || (gameDifficulty == 2 && veryHardIsAvailable) || (gameDifficulty == 3 && impossibleIsAvailable))
            {
                isAvailable = true;
            }
        }

        private void BeShot(int bulletDamage) 
        {
            if (isAvailable)
            {
                animator = GetComponent<Animator>();
                beShot = true;
                lightTime = 1f;
                playerSpawnX = this.transform.position.x + offsetX; 
                playerSpawnY = this.transform.position.y + offsetY; 
                cameraPointPosition = Camera.main.transform;

                animator.SetBool("beShot", true);

                //Update spawn point
                PlayerPrefs.SetFloat("PlayerSpawnX", playerSpawnX);
                PlayerPrefs.SetFloat("PlayerSpawnY", playerSpawnY);
                PlayerPrefs.SetFloat("PlayerSpawnZ", 0f);

                //Update camera position
                PlayerPrefs.SetFloat("cameraPointPositionX", cameraPointPosition.position.x);
                PlayerPrefs.SetFloat("cameraPointPositionY", cameraPointPosition.position.y);
                PlayerPrefs.SetFloat("cameraPointPositionZ", -10f);
            }
        }

        protected void Update()
        {
            animator = GetComponent<Animator>();

            if (beShot)
            {
                lightTime -= Time.deltaTime;
            } 
            if (lightTime <= 0)
            {
                beShot = false;
                lightTime = 1f;
                animator.SetBool("beShot", false);
            }
        }
    
}
}

