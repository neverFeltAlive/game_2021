using System;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// CharacterHealth is used for extending health mechanics with taking aim punch and healing
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterHealth : Health
    {
        public static event EventHandler<OnHealEventArgs> OnHeal;
        public class OnHealEventArgs : EventArgs
        {
            public int amount;
            public int currentHitPoints;
        }



        [SerializeField] private float aimPunchResistance = 1f;



        public override void TakeDamage(Damage damage)
        {
            base.TakeDamage(damage);

            GetComponent<Rigidbody2D>().AddForce((transform.position - damage.orrigin).normalized * damage.aimPunch / aimPunchResistance, ForceMode2D.Impulse);
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
