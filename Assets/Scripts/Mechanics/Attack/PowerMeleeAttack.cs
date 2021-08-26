using System.Collections;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// PowerMeleeAttack is used for extending melee attack with power attack mechancs
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PowerMeleeAttack : VectorMeleeAttack
    {
        #region Serialized Fields
        [Space] [Header("Power Attack Stats")]
        [SerializeField] protected float powerAttackDashRange;
        [SerializeField] protected float powerAttackRange;
        [SerializeField] protected Damage powerAttackDamage;
        #endregion



        public virtual void TriggerAttack(Vector3 direction, bool isPower = false)
        {
            if (isPower)
                StartCoroutine(PowerAttack(direction, isPower));
            else
                base.TriggerAttack(direction);
        }



        protected IEnumerator PowerAttack(Vector3 direction, bool isPower)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + direction.normalized * powerAttackDashRange);

            #region DEBUG
            if (showDebug) Debug.DrawLine(transform.position, transform.position + direction.normalized * powerAttackDashRange, Color.yellow, 2f);
            #endregion

            yield return new WaitForFixedUpdate();

            if (isPower)
                base.PerformAttack(direction, powerAttackDamage, powerAttackRange);
        }
    }
}
