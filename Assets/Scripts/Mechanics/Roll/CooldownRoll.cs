/// <remarks>
/// 
/// CooldownRoll is used for implementing cooldown possibility in roll mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;


namespace Custom.Mechanics
{
    public class CooldownRoll : Roll , ICooldown<EventArgs>
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CooldownRoll --> Start: ");
     * Debug.Log("<size=13><i><b> CooldownRoll --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CooldownRoll --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CooldownRoll --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CooldownRoll --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        private bool isOnCooldown;



        protected override void Awake()
        {
            base.Awake();
            isOnCooldown = false;
        }

        protected override void FixedUpdate()
        {
            Debug.Log("<size=13><i><b> CooldownRoll --> </b></i><color=yellow> FixedUpdate: </color></size>" + isOnCooldown);
            if (isOnCooldown)
                return;

            base.FixedUpdate();
        }

        public override void Trigger()
        {
            if (isOnCooldown)
                return;

            base.Trigger();
        }

        public void StartCooldown() =>
            isOnCooldown = true;

        public void StopCooldown() =>
            isOnCooldown = false;

        public bool IsOnCooldown()
        {
            return isOnCooldown;
        }
    }
}
