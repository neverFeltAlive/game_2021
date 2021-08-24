/// <remarks>
/// 
/// AnimationTrigger is used for controlling simple mechanics animation using animator triggers
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

using Custom.Mechanics;

namespace Custom.Visuals
{
    public abstract class AnimationTrigger<TMech> : MonoBehaviour
        where TMech : Mechanics<EventArgs>
    /* DEBUG statements for this document 
     * 
     * Debug.Log("AnimationTrigger --> Start: ");
     * Debug.Log("<size=13><i><b> AnimationTrigger --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> AnimationTrigger --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> AnimationTrigger --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> AnimationTrigger --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        [SerializeField] protected string triggerName;
        [SerializeField] protected Animator animator;
        
        protected TMech mechanics;



        protected virtual void Awake() =>
            mechanics = GetComponent<TMech>();
        protected virtual void OnEnable() =>
            mechanics.OnTriggered += Handler;

        protected virtual void OnDisable() =>
            mechanics.OnTriggered -= Handler;

        protected virtual void Handler(object sender, EventArgs args) =>
            animator.SetTrigger(triggerName);
    }
}
