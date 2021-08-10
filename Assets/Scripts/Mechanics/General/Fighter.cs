/// <remarks>
/// 
/// Fighter is used for creating fighting creatures
/// It provides mechanics specific for all fighting objects (character, enemies, etc) 
/// which are represented by separate classes inherited from this one.
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;

using Platformer.Utils;

namespace Platformer.Mechanics.General
{
    public class Fighter : MonoBehaviour
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
     * Aim punch slow mechanics
     * 
     */
    {
        public static event EventHandler<OnAttackEventArgs> OnAttack;
        public class OnAttackEventArgs : EventArgs
        {
            public bool isPower;
        }
        public static event EventHandler<OnDamageTakenEventArgs> OnDamageTaken;
        public class OnDamageTakenEventArgs : EventArgs
        {
            public Damage damage;
        }



        #region Serialized Fields
        [SerializeField] protected Animator animator;
        [Space]
        [Header("Health Stats")]
        [SerializeField] [Range(1, 100)] [Tooltip("Maximum amount of health")] private int maxHitPoints = 1;
        [SerializeField] [Range(1f, 10f)] [Tooltip("Resistance to the aim punch force")] private float aimPunchResistance = 1f;
        [Space]
        [Header("Attack Stats")]
        [SerializeField] [Range(0f, 1f)] [Tooltip("Range of melee attack")] private float attackRange = 1f;
        [SerializeField] [Range(0f, 1f)] [Tooltip("Distance on which object gets pushed when attacking")] private float attackDashRange = 0.5f;
        [Space]
        [SerializeField] [Tooltip("Damage object")] protected Damage damage;
        #endregion

        #region Private Fields
        protected string targetTag = Constants.FRIENDLY_TAG;                            // objects with wich tag to target
        
        private int currentHitPoints;                
        #endregion

        private bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        #region Contex Menu
        [ContextMenu("Default Values")]
        private void DefaultValues()
        {
            maxHitPoints = 1;
            aimPunchResistance = 1f;

            attackRange = 1f;
            attackDashRange = 0.5f;
        }
        #endregion

        #region MonoBehaviour CallBacks
        protected virtual void Start()
        {
            if (!animator)
                animator = gameObject.GetComponent<Animator>();
            
            currentHitPoints = maxHitPoints;
        }

        protected virtual void Update()
        {
            if (currentHitPoints <= 0)
                Despawn();
        }
        #endregion

        #region Protected Functions
        protected virtual void HandleAttack(Vector3 direction)
        {
            // Trigger attack animation
            animator.SetTrigger(Constants.ATTACK);
            OnAttack?.Invoke(this, new OnAttackEventArgs { isPower = false });
            
            int angle = 20;
            /// <remarks>
            /// Best working angles: from 45 to 20
            /// </remarks>
            
            damage.orrigin = transform.position;
            Vector3 directionVector = transform.position + direction.normalized * attackRange;

            // Detect collisions in attack range
            Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, attackRange);
            foreach (Collider2D collision in collisions)
            {
                if (collision.tag == targetTag && VallidateTargetLocation(direction, directionVector, collision.transform, angle))
                    collision.SendMessage("TakeDamage", damage);
            }
        }

        protected virtual void TakeDamage(Damage damage)
        {
            currentHitPoints -= damage.amountOfDamage;
            OnDamageTaken?.Invoke(this, new OnDamageTakenEventArgs { damage = damage });
            
            // DEBUG
            if (showDebug) Debug.Log("<size=13><i><b> Damagable --> </b></i><color=green> OnDamageTaken: </color></size> "
                + gameObject.name + " took " + damage.amountOfDamage + " damage and currently has " + currentHitPoints + " hit points");

            GetComponent<Rigidbody2D>().AddForce((transform.position - damage.orrigin).normalized * damage.aimPunch / aimPunchResistance, ForceMode2D.Impulse);
        }

        protected virtual void Despawn()
        {
            GameObject.Destroy(gameObject);
        }
        #endregion

        private Vector3 RotateVector(Vector3 normalizedVector, int angle)
        {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float x = normalizedVector.x * cos - normalizedVector.y * sin;
            float y = normalizedVector.x * sin + normalizedVector.y * cos;

            return new Vector3(x, y).normalized;
        }

        // Checks if the target is within attack sector
        private bool VallidateTargetLocation(Vector3 direction, Vector3 directionVector, Transform target, int sectorAngle)
        {
            // Calculate target projections on boarders of the attack sector by rotating direction vector
            Vector3 targetLeftProjection = transform.position + RotateVector(direction.normalized, sectorAngle) *
                Vector3.Distance(transform.position, target.position);
            Vector3 targetRightProjection = transform.position + RotateVector(direction.normalized, -sectorAngle) *
                Vector3.Distance(transform.position, target.position);

            float projectionDistance = Vector3.Distance(targetLeftProjection, targetRightProjection);
            if (Vector3.Distance(targetRightProjection, target.position) <= projectionDistance &&
                Vector3.Distance(targetLeftProjection, target.position) <= projectionDistance &&
                Vector3.Distance(target.position, directionVector) <= Vector3.Distance(transform.position, directionVector))
            {
                // DEBUG    
                if (showDebug)
                {
                    Debug.DrawLine(transform.position, directionVector, Color.blue, 2f);                                                                        // direction vector
                    Debug.DrawLine(transform.position, transform.position + RotateVector(direction.normalized, sectorAngle) * attackRange, Color.blue, 2f);     // left boarder vector
                    Debug.DrawLine(transform.position, transform.position + RotateVector(direction.normalized, -sectorAngle) * attackRange, Color.blue, 2f);    // right boarder vector
                    Debug.DrawLine(directionVector, transform.position + RotateVector(direction.normalized, -sectorAngle) * attackRange, Color.blue, 2f);       // direction vector + right boarder vector
                    Debug.DrawLine(directionVector, transform.position + RotateVector(direction.normalized, sectorAngle) * attackRange, Color.blue, 2f);        // direction vector + left boarder vector
                    Debug.DrawLine(transform.position, target.position, Color.red, 2f);                                                                         // target vector
                    Debug.DrawLine(transform.position, targetLeftProjection, Color.red, 2f);                                                                    // target left projection vector                                                              
                    Debug.DrawLine(transform.position, targetRightProjection, Color.red, 2f);                                                                   // target right projection vector
                    Debug.DrawLine(targetLeftProjection, targetRightProjection, Color.red, 2f);                                                                 // target left projection vector + target right projection vector
                    Debug.DrawLine(targetLeftProjection, target.position, Color.red, 2f);                                                                       // target left projection vector + target vector
                    Debug.DrawLine(target.position, targetRightProjection, Color.red, 2f);                                                                      // target right projection vector + target vector
                }

                return true;
            }
            else
                return false;
            /// <summary>
            /// We check if distances from target to its projection are less than the distance between those projections.
            /// And then we validate that the target is within the required sector by checking if the distance 
            /// from the target to the end of direction vector is less than the distance from the attacker to the end of direction vector
            /// </summary>
        }



        protected IEnumerator PowerAttack(Vector3 direction)
        {
            yield return new WaitForFixedUpdate();
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + direction.normalized * attackDashRange);
            
            // DEBUG
            if (showDebug) Debug.DrawLine(transform.position, transform.position + direction.normalized * attackDashRange, Color.yellow, 2f);

            yield return new WaitForFixedUpdate();
            HandleAttack(direction);
        }
    }
}