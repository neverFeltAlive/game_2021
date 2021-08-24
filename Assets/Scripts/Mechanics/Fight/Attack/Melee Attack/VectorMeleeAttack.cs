/// <remarks>
/// 
/// VectorMeleeAttack is used for simple attack mechanics.
/// It validates if target can be hit using linear algebra
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    public class VectorMeleeAttack : MonoBehaviour, IMeeleAttack<Damage, float>
    /* DEBUG statements for this document 
     * 
     * Debug.Log("VectorMeleeAttack --> Start: ");
     * Debug.Log("<size=13><i><b> VectorMeleeAttack --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> VectorMeleeAttack --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> VectorMeleeAttack --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> VectorMeleeAttack --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        public static event EventHandler OnAttack;



        #region Fields
        [SerializeField] protected float attackRange = .2f;
        [SerializeField] protected string targetTag = Constants.FRIENDLY_TAG;                          
        [SerializeField] protected Damage damage;

        protected Vector3 _direction;

        public Vector3 Direction
        {
            get { return _direction; }
            set {
                if (value != Vector3.zero)
                    _direction = value;
            } 
        }
        #endregion

        protected bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        public virtual void TriggerAttack() =>
            HandleAttack(_direction, damage, attackRange);

        public virtual void HandleAttack(Vector3 direction, Damage damage, float range)
        {
            OnAttack?.Invoke(this, EventArgs.Empty);

            int angle = 20;
            /// <remarks>
            /// Best working angles: from 45 to 20
            /// </remarks>
            
            damage.orrigin = transform.position;
            Vector3 directionVector = transform.position + direction.normalized * attackRange;

            // Detect collisions in attack range
            Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, range);
            foreach (Collider2D collision in collisions)
            {
                if (collision.tag == targetTag)
                {
                    if (collision.GetComponent<IDamagable<Damage>>() != null)
                    {
                        if (VallidateTargetLocation(direction, directionVector, collision.transform, angle))
                            collision.GetComponent<IDamagable<Damage>>().TakeDamage(damage);
                    }
                }
            }
        }

        #region Methods
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
        #endregion
    }
}
