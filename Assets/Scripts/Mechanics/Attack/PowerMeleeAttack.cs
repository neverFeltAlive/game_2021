using System.Collections;
using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// PowerMeleeAttack is used for extending melee attack with power attack mechancs
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PowerMeleeAttack : VectorMeleeAttack, IPowerMeeleAttack, IOverLoadable
    {
        #region Fields
        [SerializeField] PowerMeeleAttackStats powerMeeleAttackStats;

        private bool _isOverload;
        public bool IsOverload { set { _isOverload = value; } }
        #endregion




        private void Awake() =>
            _isOverload = false;

        public virtual void TriggerAttack(Vector3 direction, bool isPower = false)
        {
            if (isPower || _isOverload)
                StartCoroutine(PowerAttack(direction)); 
            else
                base.TriggerAttack(direction);
        }



        protected IEnumerator PowerAttack(Vector3 direction)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + direction.normalized * powerMeeleAttackStats.powerAttackDashRange);

            #region DEBUG
            if (showDebug) Debug.DrawLine(transform.position, transform.position + direction.normalized * powerMeeleAttackStats.powerAttackDashRange, Color.yellow, 2f);
            #endregion

            yield return new WaitForFixedUpdate();

            base.PerformAttack(direction, powerMeeleAttackStats.powerAttackDamage, powerMeeleAttackStats.powerAttackRange);
        }
    }
}
