using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// This class is used to move the camera, realize scene switching.
    /// </summary>
    public class MoveCamera : MonoBehaviour
    {
        public Transform pointPosition; //The position where the camera needs to be moved
        public bool isAvailable = true;

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.tag == "Player" && isAvailable)
            {
                Camera.main.transform.position = pointPosition.position;
            }
        }
    }
}