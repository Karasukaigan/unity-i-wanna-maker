using Platformer.Core;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath>
    {
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();
        System.Random rd = new System.Random();
        GameObject thisBlood;
        Vector2 launchSpeed;
        
        public override void Execute()
        {
            var player = model.player;
            
            if (player.health.IsAlive)
            {
                player.Hide();
                
                player.health.Die();

                //model.virtualCamera.m_Follow = null;
                //model.virtualCamera.m_LookAt = null;

                player.controlEnabled = false;
                player.playerDeadTimer = 0.8f;
                player.dead = true;
                
                if (player.audioSource && player.ouchAudio) player.audioSource.PlayOneShot(player.ouchAudio);

                //Change the animator parameters
                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);

                //Generate head at player position
                GameObject.Instantiate(player.head, new Vector3(player.transform.position.x, player.transform.position.y + 0.2f, 0f), Quaternion.identity);
                
                SpurtingBlood(); //Spatter effect
            }
        }

        //Achieve the effect of splashing blood.
        public void SpurtingBlood()
        {
            var player = model.player;
            //Launch direction
            float [,] launchAngle = new float[11,2] {
                {0.97f, 0.26f} ,
                {0.87f, 0.5f} ,
                {0.71f, 0.71f} ,
                {0.5f, 0.87f} ,
                {0.26f, 0.97f} ,
                {0f, 1f} ,
                {-0.26f, 0.97f} ,
                {-0.5f, 0.87f} ,
                {-0.71f, 0.71f} ,
                {-0.87f, 0.5f} ,
                {-0.97f, 0.26f} 
            };

            //Generate 500 particles
            for (int i = 0; i < 500; i++)
            {
                int x = rd.Next(0, 11); //Choose the launch direction
                int y = rd.Next(0, 20); //Random launch speed
                float[] thisLaunchAngle = {launchAngle[x,0], launchAngle[x,1]}; //Set the launch direction
                launchSpeed = new Vector2(y * thisLaunchAngle[0] + (rd.Next(0, 30) * 0.1f), 
                    y * thisLaunchAngle[1] + (rd.Next(0, 10) * 0.1f)); //Set the launch speed
                thisBlood = GameObject.Instantiate(player.blood[rd.Next(0,3)],
                    new Vector3(player.transform.position.x, player.transform.position.y - 0.2f, 0f), Quaternion.identity); //Generate particles
                thisBlood.GetComponent<Rigidbody2D>().velocity = launchSpeed; //Give speed to objects
            }
        }
    }
}