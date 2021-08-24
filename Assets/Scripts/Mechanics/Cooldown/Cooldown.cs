/// <remarks>
/// 
/// Cooldown is used for triggering simple cooldown
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Custom.Mechanics
{
    [RequireComponent(typeof(ICooldown<EventArgs>))]
    [RequireComponent(typeof(Mechanics<EventArgs>))]
    public class Cooldown : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Cooldown --> Start: ");
     * Debug.Log("<size=13><i><b> Cooldown --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Cooldown --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Cooldown --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Cooldown --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        [SerializeField] [Tooltip("Mechanics to put on cooldoown")] protected Mechanics<EventArgs> mechanics;
        [SerializeField] [Range(1, 1000)] [Tooltip("Number of charges it takes to trigger cooldown")] protected int numberOfCharges = 1;
        [SerializeField] [Tooltip("Length of cooldown in seconds")] protected float time;

        protected ICooldown<EventArgs> iMechanics;
        private int currentNumberOfCharger;
        private float currentTime;



        #region MonoBehaviour Callbacks
        protected virtual void Awake()
        {
            iMechanics = mechanics as ICooldown<EventArgs>;
            currentNumberOfCharger = numberOfCharges;
            currentTime = time;
        }

        protected virtual void OnEnable()  =>
            iMechanics.OnPerforemed += Handler;

        protected virtual void OnDisable() =>
            iMechanics.OnPerforemed -= Handler;

        protected virtual void Update()
        {
            if (iMechanics.IsOnCooldown())
            {
                if (currentTime - Time.deltaTime <= 0)
                {
                    iMechanics.StopCooldown();
                    currentNumberOfCharger = numberOfCharges;
                    currentTime = time;
                }
                else
                    currentTime -= Time.deltaTime;
            }
        }
        #endregion

        private void Handler(object sender, EventArgs args)
        {
            if (iMechanics.IsOnCooldown())
                return;

            currentNumberOfCharger--;
            if (currentNumberOfCharger == 0)
                iMechanics.StartCooldown();
        }
    }
}
