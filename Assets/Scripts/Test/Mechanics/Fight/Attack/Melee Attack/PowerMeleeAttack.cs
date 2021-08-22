/// <remarks>
/// 
/// PowerMeleeAttack is used for extending vector melee attack with power attack mechancs
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    [RequireComponent(typeof(IAttacking))]
    public class PowerMeleeAttack : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PowerMeleeAttack --> Start: ");
     * Debug.Log("<size=13><i><b> PowerMeleeAttack --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PowerMeleeAttack --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PowerMeleeAttack --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PowerMeleeAttack --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public static event EventHandler OnPowerAttack;



        #region Serialized Fields
        [Space] [Header("Power Attack Stats")]
        [SerializeField] protected float powerAttackRange;
        [SerializeField] protected float powerAttackDashRange;
        [SerializeField] protected Damage powerAttackDamage;
        #endregion

        private IAttacking attack;

        protected bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        private void Awake() =>
            attack = GetComponent<IAttacking>();

        public virtual void TriggerAttack()
        {
            OnPowerAttack?.Invoke(this, EventArgs.Empty);

            StartCoroutine(PowerAttack(attack.Direction));
        }



        protected IEnumerator PowerAttack(Vector3 direction)
        {
            gameObject.GetComponent<Rigidbody2D>().MovePosition(transform.position + direction.normalized * powerAttackDashRange);

            // DEBUG
            if (showDebug) Debug.DrawLine(transform.position, transform.position + direction.normalized * powerAttackDashRange, Color.yellow, 2f);

            yield return new WaitForFixedUpdate();
            attack.HandleAttack(direction, powerAttackDamage, powerAttackRange);
        }
    }
}
