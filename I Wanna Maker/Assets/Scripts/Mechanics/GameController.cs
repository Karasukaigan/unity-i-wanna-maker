using System;
using Platformer.Core;
using Platformer.Model;
using Platformer.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This class exposes the the game model in the inspector, and ticks the simulation.
    /// </summary> 
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }

        //This model field is public and can be therefore be modified in the inspector.
        //The reference actually comes from the InstanceRegister, and is shared through the simulation and events. 
        //Unity will deserialize over this shared reference when the scene loads, allowing the model to be conveniently configured inside the inspector.
        public PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;
        public Transform spawnPoint;

        public String currentSceneName = "StartScene"; 

        //The coordinates of the spawn point
        private float playerSpawnX = 0f; 
        private float playerSpawnY = 0f; 
        private float playerSpawnZ = 0f; 

        private void Start() {
            //Get the coordinates of the spawn point
            playerSpawnX = PlayerPrefs.GetFloat("PlayerSpawnX", 0.5f); 
            playerSpawnY = PlayerPrefs.GetFloat("PlayerSpawnY", -7f); 
            playerSpawnZ = PlayerPrefs.GetFloat("PlayerSpawnZ", 0f);

            if (PlayerPrefs.GetInt("StartGame", 0) == 1)
            {
                //Move the player
                player.transform.position = new Vector3(playerSpawnX, playerSpawnY, 0f); 
                //Move the camera
                Camera.main.transform.position = new Vector3(PlayerPrefs.GetFloat("cameraPointPositionX", 0.53f), 
                    PlayerPrefs.GetFloat("cameraPointPositionY", -0.4f), 
                    PlayerPrefs.GetFloat("cameraPointPositionZ", -10f)); 
            }
            else
            {
                player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0f); //Move the player
            }
            
        }

        private void Awake() {
            player = model.player;
        }
        
        void OnEnable()
        {
            Instance = this;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
            if (Input.GetButtonDown("Exit")) QuitGame();
            if (Input.GetButtonDown("Suicide")) Simulation.Schedule<PlayerDeath>(0);

            //if (Input.GetButtonDown("Spawn") && player.dead == true) 
            if (Input.GetButtonDown("Spawn")) 
            {
                //Simulation.Schedule<PlayerSpawn>(2);
                //PlayerSpawn();
                SceneManager.LoadScene (currentSceneName);
            }
        }

        void QuitGame()
        {
            {
                PlayerPrefs.SetInt("StartGame", 0);

                //Exit the game
                UnityEditor.EditorApplication.isPlaying = false; //For testing
                //Application.Quit(); //For compilation
            }
        }

        //No longer used
        void PlayerSpawn()
        {
            player.collider2d.enabled = true;
            player.controlEnabled = false; //Prohibited operation

            if (player.audioSource && player.respawnAudio) player.audioSource.PlayOneShot(player.respawnAudio);

            player.health.Increment(); //Reset HP
            player.dead = false;
            player.Teleport(model.spawnPoint.transform.position);
            player.jumpState = PlayerController.JumpState.InFlight;

            player.animator.SetBool("dead", false);
            
            //model.virtualCamera.m_Follow = player.transform;
            //model.virtualCamera.m_LookAt = player.transform;

            //Simulation.Schedule<EnablePlayerInput>(2f);
            player.controlEnabled = true;
            player.Show();
        }

    }
}