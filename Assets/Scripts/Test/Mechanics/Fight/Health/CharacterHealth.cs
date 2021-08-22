/// <remarks>
/// 
/// CharacterHealth is used for extending health mechanics with taking aim punch and healing
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterHealth : Health
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CharacterHealth --> Start: ");
     * Debug.Log("<size=13><i><b> CharacterHealth --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterHealth --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterHealth --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterHealth --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public static event EventHandler<OnHealEventArgs> OnHeal;
        public class OnHealEventArgs : EventArgs
        {
            public int amount;
            public int currentHitPoints;
        }



        [SerializeField] private float aimPunchResistance = 1f;

        private Rigidbody2D body;



        protected override void Awake()
        {
            base.Awake();

            body = GetComponent<Rigidbody2D>();
        }

        public override void TakeDamage(Damage damage)
        {
            base.TakeDamage(damage);

            body.AddForce((transform.position - damage.orrigin).normalized * damage.aimPunch / aimPunchResistance, ForceMode2D.Impulse);
        }

        public virtual void Heal(int amount)
        {
            OnHeal?.Invoke(this, new OnHealEventArgs { amount = amount, currentHitPoints = currentHitPoints });

            currentHitPoints += amount;
            if (currentHitPoints > maxHitPoints)
                currentHitPoints = maxHitPoints;
        }
    }
}
