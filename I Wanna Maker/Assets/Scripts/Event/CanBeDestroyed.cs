using UnityEngine;

namespace Platformer.Event
{
    /// <summary>
    /// If the object is shot, destroy it.
    /// </summary>
    public class CanBeDestroyed : MonoBehaviour
    {
        private void BeShot(int bulletDamage) 
        {
            Destroy(this.gameObject);
        }
    }
}