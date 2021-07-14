using System;
using UnityEngine;
using Platformer.Core;
using Platformer.Model;
using Platformer.Mechanics;
using UnityEngine.SceneManagement;

namespace Platformer.Event
{
    /// <summary>
    /// Load the level.
    /// </summary>
    public class EnterLevel : MonoBehaviour
    {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        PlayerController player;

        public String currentSceneName = "GameScene"; //Current scene name
        public Transform levelStartPointPosition;
        public Transform cameraPointPosition;

        public bool isAvailable = true; //Is the level open?
        public bool isLoadGame = false; //Load the archive?
        public int gameDifficulty = 0; //The difficulty of the level

        private float playerSpawnX; 
        private float playerSpawnY; 
        private float playerSpawnZ; 

        private void OnTriggerEnter2D(Collider2D collider) 
        {
            player = model.player;

            //Set the StartGame to enter the level
            PlayerPrefs.SetInt("StartGame", 1);

            //Read the player and camera coordinates in the archive
            playerSpawnX = PlayerPrefs.GetFloat("PlayerSpawnX", 0.5f); 
            playerSpawnY = PlayerPrefs.GetFloat("PlayerSpawnY", -7f); 
            playerSpawnZ = PlayerPrefs.GetFloat("PlayerSpawnZ", 0f); 
            playerSpawnX = PlayerPrefs.GetFloat("cameraPointPositionX", 0.53f); 
            playerSpawnY = PlayerPrefs.GetFloat("cameraPointPositionY", -0.4f); 
            playerSpawnZ = PlayerPrefs.GetFloat("cameraPointPositionZ", -10f); 

            //If the checkpoint is open
            if (isAvailable && collider.tag == "Player")
            {
                //Enter the level in the form of loading
                if (isLoadGame)
                {
                    SceneManager.LoadScene (currentSceneName);
                }
                //From scratch
                else
                {
                    PlayerPrefs.SetInt("GameDifficulty", gameDifficulty); //Set level difficulty
                    PlayerPrefs.SetFloat("PlayerSpawnX", levelStartPointPosition.position.x);
                    PlayerPrefs.SetFloat("PlayerSpawnY", levelStartPointPosition.position.y);
                    PlayerPrefs.SetFloat("PlayerSpawnZ", 0f);
                    PlayerPrefs.SetFloat("cameraPointPositionX", cameraPointPosition.position.x);
                    PlayerPrefs.SetFloat("cameraPointPositionY", cameraPointPosition.position.y);
                    PlayerPrefs.SetFloat("cameraPointPositionZ", -10f);
                    SceneManager.LoadScene (currentSceneName);
                }
            }
        }
    }
}