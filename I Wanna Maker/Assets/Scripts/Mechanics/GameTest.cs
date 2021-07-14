using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// The class used to test the game.
    /// It is generally mounted on the GameController object and called when needed.
    /// </summary> 
    public class GameTest : MonoBehaviour
    {
        //public float playerSpawnX;
        //public float playerSpawnY;
        //public float playerSpawnZ;
        void Start()
        {
            //PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("StartGame", 0); //Whether to start the game (enter the level), no: 0, yes: 1

            PlayerPrefs.SetInt("GameDifficulty", 0); //Game difficulty, medium:0, hard:1, very hard:2, impossible:3

            PlayerPrefs.SetFloat("PlayerSpawnX", 0.5f);
            PlayerPrefs.SetFloat("PlayerSpawnY", -7f);
            PlayerPrefs.SetFloat("PlayerSpawnZ", 0f);

            PlayerPrefs.SetFloat("cameraPointPositionX", 0.53f);
            PlayerPrefs.SetFloat("cameraPointPositionY", -0.4f);
            PlayerPrefs.SetFloat("cameraPointPositionZ", -10f);
        }
    }
}