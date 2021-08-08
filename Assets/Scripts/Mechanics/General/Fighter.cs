/// <remarks>
/// 
/// Fighter is used for creating fighting creatures
/// It provides mechanics specific for all fighting objects (character, enemies, etc) 
/// which are represented by separate classes inherited from this one.
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Utils;

namespace Platformer.Mechanics.General
{
    public class Fighter : Damagable
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Fighter --> Start: ");
     * Debug.Log("<size=13><i><b> Fighter --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Fighter --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Fighter --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Fighter --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     * Aim punch recovery and slow mechanics
     * 
     */
    {
        // Fields

        #region Serialized Fields
        [SerializeField] protected Mover movement;
        [SerializeField] protected Animator animator;
        [Space]
        [SerializeField] [Range(0f, 5f)] [Tooltip("Time after recieving damage untill when object does not recieve damage")] protected float immuneTime = 0f;
        [SerializeField] [Range(1f, 10f)] [Tooltip("Resistance to the aim punch force")] private float aimPunchResistance = 1f;
        //[SerializeField] [Range(1f, 10f)] [Tooltip("Time it takes to recover from aim punch in seconds")] private float aimPunchRecoveryTime = 0.2f;
        //[SerializeField] [Range(1f, 10f)] [Tooltip("Movement speed slow after punch taken")] private float aimPunchSlow = 0.2f;
        [Space]
        [SerializeField] [Range(0f, 1f)] [Tooltip("Range of melee attack")] private float attackRange = 1f;
        [SerializeField] [Tooltip("Damage object")] protected Damage damage;
        #endregion

        #region Protected Fields
        protected Vector2 punchDirection;                   // where to be pushed when damage recieved

        protected string targetTag = Constants.FRIENDLY_TAG;                         // objects with wich tag to target

        protected float lastImmuneTime;                     // last time object was immune to damage
        #endregion

        // Functions 

        #region Context Menu
        [ContextMenu("Boy Values")]
        private void BoyValues()
        {
            attackRange = 0.16f;
            damage = new Damage(instant: true, amount: 1);
        }
        #endregion

        #region MonoBehaviour Callbacks
        // When the script is enabled
        protected override void Start()
        {
            base.Start();

            // Assign variables 
            if (!movement)
                movement = gameObject.GetComponent<Mover>();
            if (!animator)
                animator = gameObject.GetComponent<Animator>();
        }
        #endregion

        #region Protected Functions
        protected virtual void Attack()
        {
            // Trigger attack animation
            animator.SetTrigger(Constants.ATTACK);

            // Identify possible objects to attack
            Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange);

            foreach (Collider2D collision in collisions)
            {
                // Check if target is an enemy
                if (collision.tag == targetTag)
                {
                    // Define attack direction based on current position and direction 
                    if (Mathf.Abs(movement.Direction.x) < Mathf.Abs(movement.Direction.y))
                    {
                        if (movement.Direction.y >= 0)
                        {
                            // Check if enemy is in direction
                            if (collision.transform.position.y >= gameObject.transform.position.y)
                                collision.SendMessage("OnDamageTaken", damage);
                        }
                        else
                        {
                            // Check if enemy is in direction
                            if (collision.transform.position.y <= gameObject.transform.position.y)
                                collision.SendMessage("OnDamageTaken", damage);
                        }
                    }
                    else
                    {
                        if (movement.Direction.x >= 0)
                        {
                            // Check if enemy is in direction
                            if (collision.transform.position.x >= gameObject.transform.position.x)
                                collision.SendMessage("OnDamageTaken", damage);
                        }
                        else
                        {
                            // Check if enemy is in direction
                            if (collision.transform.position.x <= gameObject.transform.position.x)
                                collision.SendMessage("OnDamageTaken", damage);
                        }
                    }
                }
            }
        }
        #endregion

        #region Event Handlers
        // When damage is recieved
        protected override void OnDamageTaken(Damage damage)
        {
            // Check for immunity 
            if (Time.time - lastImmuneTime >= immuneTime)
            {
                // Update last immune time
                lastImmuneTime = Time.time;

                // Take damage through base class function
                base.OnDamageTaken(damage);

                // Determine aim punch direction
                punchDirection = (gameObject.transform.position - damage.Orrigin).normalized * damage.AimPunch / aimPunchResistance;
            }
        }
        #endregion
    }
}
