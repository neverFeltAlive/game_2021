using System;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// VectorMeleeAttack is used for simple attack mechanics.
    /// It validates if target can be hit using linear algebra
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class VectorMeleeAttack : MonoBehaviour
    {
        public event EventHandler OnAttack;


        
        #region Fields
        [SerializeField] protected float attackRange = .2f;
        [SerializeField] protected Damage damage;

        protected string targetTag = Constants.FRIENDLY_TAG;
        #endregion

        #region DEBUG
        protected bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>
        #endregion


        public virtual void TriggerAttack(Vector3 direction) =>
            PerformAttack(direction, damage, attackRange);

        protected virtual void PerformAttack(Vector3 direction, Damage damage, float range)
        {
            OnAttack?.Invoke(this, EventArgs.Empty);

            int angle = 20;
            /// Best working angles: from 45 to 20
            
            damage.orrigin = transform.position;

            // Detect collisions in attack range
            Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, range);
            foreach (Collider2D collision in collisions)
            {
                if (collision.tag == targetTag)
                {
                    if (collision.GetComponent<IDamagable<Damage>>() != null)
                    {
                        if (VallidateTargetLocation(direction, collision.transform, angle))
                            collision.GetComponent<IDamagable<Damage>>().TakeDamage(damage);
                    }
                }
            }
        }

        #region Methods
        /// <summary>
        /// Checks if the target is within attack sector
        /// </summary>
        /// <param name="direction">Direction of attack</param>
        /// <param name="target">Target location</param>
        /// <param name="sectorAngle">How wide is the attack sector</param>
        /// <returns></returns>
        /// <remarks>
        /// Checks if distances from target to its projections on attack sector are less than the distance between those projections.
        /// And then validates that the target is within the required sector by checking if the distance 
        /// from the target to the furthest point of the sector is less than the distance from the attacker to that point
        /// </remarks>
        private bool VallidateTargetLocation(Vector3 direction, Transform target, int sectorAngle)
        {
            Vector3 directionVector = transform.position + direction.normalized * attackRange;

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
                #region DEBUG  
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
                #endregion

                return true;
            }
            else
                return false;
            /// <remarks>
            /// 
            /// </remarks>
        }

        private Vector3 RotateVector(Vector3 normalizedVector, int angle)
        {
            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float x = normalizedVector.x * cos - normalizedVector.y * sin;
            float y = normalizedVector.x * sin + normalizedVector.y * cos;

            return new Vector3(x, y).normalized;
        }
        #endregion
    }
}
