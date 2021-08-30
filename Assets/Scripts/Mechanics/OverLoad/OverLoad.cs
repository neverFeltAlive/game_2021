using System.Collections.Generic;
using UnityEngine;


namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// OverLoad is used for making all mechecnics powerful (bigger range / damage etc) for certain amount of time
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(IOverLoadable))]
    public class OverLoad : MonoBehaviour
    {
        [SerializeField] private float overLoadTime = 3f;
        [SerializeField] private float cooldownTime = 100f;
        [Space]
        [SerializeField] private List<MonoBehaviour> mechanics;

        private float currentOverLoadTime;
        private float currentCooldownTime;

        private State state;



        private void Awake() =>
            state = State.Ready;

        private void Update()
        {
            if (state == State.Active)
            {
                if (currentOverLoadTime - Time.deltaTime > 0)
                    currentOverLoadTime -= Time.deltaTime;
                else
                {
                    state = State.OnCooldown;
                    currentCooldownTime = cooldownTime;

                    SetOverload(false);
                }
            }

            if (state == State.OnCooldown)
            {
                if (currentCooldownTime - Time.deltaTime > 0)
                    currentCooldownTime -= Time.deltaTime;
                else
                    state = State.Ready;
            }
        }

        public void TriggerOverLoad()
        {
            if (state == State.Ready)
            {
                currentOverLoadTime = overLoadTime;
                state = State.Active;

                SetOverload(true);
            }
        }

        private void SetOverload(bool state)
        {
            foreach (IOverLoadable mechanic in mechanics)
                mechanic.IsOverload = state;
        }
    }
}
