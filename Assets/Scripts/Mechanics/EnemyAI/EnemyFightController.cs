/// <remarks>
/// 
/// EnemyFightController is used for controlling fighting logics of enemies AI
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Mechanics.General;
using Custom.Utils;

namespace Platformer.Mechanics.EnemyAI
{
    public class EnemyFightController : Fighter
    /* DEBUG statements for this document 
     * 
     * Debug.Log("EnemyFightController --> Start: ");
     * Debug.Log("<size=13><i><b> EnemyFightController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> EnemyFightController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> EnemyFightController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> EnemyFightController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        #region Serialized Fields
        [SerializeField] [Range(0f, 1f)] [Tooltip("Distance from where to perform the attack")] private float _attackToggleRange = 0.02f;
        [SerializeField] [Range(0f, 1f)] [Tooltip("Time it needs to reset attack cooldown in seconds")] private float attackCooldown = 1f;
        #endregion

        #region Private Fields
        private float lastAttackTime = 0;

        private bool _toggleAttack = false;
        #endregion

        #region Properties
        public bool ToggleAttack { set { _toggleAttack = value; } }
        public float AttackToggleRange { get { return _attackToggleRange; } }
        #endregion



        #region MonoBehaviour Callbacks
        // Every fixed time period
        protected override void Update()
        {
            base.Update();

            // Check for collisions with player
            Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, _attackToggleRange);
            foreach (Collider2D collision in collisions)
            {
                if (collision.tag == Constants.FRIENDLY_TAG)
                {
                    // Check for cooldown
                    if (lastAttackTime - Time.deltaTime > 0)
                        lastAttackTime -= Time.deltaTime;
                    else
                    {
                        //animator.SetTrigger(Constants.ATTACK);

                        // Toggle attack
                        collision.SendMessage("OnDamageTaken", damage);

                        // Reset cooldown
                        lastAttackTime = attackCooldown;
                    }
                }
            }

            // Check if attack is toggled
            if (_toggleAttack)
            {

            }

        }
        #endregion

        protected override void Despawn()
        {
            _toggleAttack = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

            base.Despawn();
        }
    }
}
