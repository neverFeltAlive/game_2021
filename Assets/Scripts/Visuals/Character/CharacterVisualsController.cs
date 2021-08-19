/// <remarks>
/// 
/// CharacterVisualsController is used for controlling all visual parts of the character.
/// Animation, lights and etc.
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

using Platformer.Mechanics.Character;
using Platformer.Utils;

namespace Platformer.Visuals.Character
{
    public class CharacterVisualsController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("CharacterVisualsController --> Start: ");
     * Debug.Log("<size=13><i><b> CharacterVisualsController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterVisualsController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterVisualsController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> CharacterVisualsController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        #region Serialized Fields
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject dashEffect;
        [SerializeField] private Light2D characterLight;
        [Space]
        [SerializeField] private RuntimeAnimatorController normalAnimator;
        [SerializeField] private RuntimeAnimatorController shootingAnimator;
        #endregion

        private Vector2 direction;



        #region MonoBehaviour Callbacks
        private void Start()
        {
            if (!characterLight)
                characterLight = transform.GetChild(0).GetComponent<Light2D>();
            characterLight.enabled = false;

            CharacterFightController.OnAttack += AttackHandler;
            CharacterFightController.OnFightStateChanged += FightStateChangeHaandler;
            CharacterFightController.OnShoot += ShootHandler;
            DashController.OnDashStateChanged += DashHandler;
        }

        private void Update()
        {
            Vector2 input = CharacterMovementController.playerControls.MainControls.Walk.ReadValue<Vector2>();

            if (GetComponent<CharacterMovementController>().IsMoving())
                animator.SetFloat(Constants.MAGNITUDE, input.magnitude);

            if (input == Vector2.zero)
                input = CharacterMovementController.playerControls.MainControls.Aim.ReadValue<Vector2>();

            if (input != Vector2.zero)                                                      
            {
                direction = input;
                animator.SetFloat(Constants.HORIZONTAL, direction.x);
                animator.SetFloat(Constants.VERTICAL, direction.y);
            }
        }
        #endregion

        #region Functions
        #region Event Handlers
        private void FightStateChangeHaandler(object sender, CharacterFightController.OnFightStateChangedEventArgs args)
        {
            if (args.state == CharacterFightController.FightState.Normal)
                animator.runtimeAnimatorController = normalAnimator;
            else
                animator.runtimeAnimatorController = shootingAnimator;
        }

        private void DashHandler(object sender, DashController.OnDashStateChangedEventArgs args)
        {
            if (args.state == DashController.DashState.Active)
            {
                float angle = Mathf.Atan2(-direction.normalized.y, -direction.normalized.x) * Mathf.Rad2Deg;
                dashEffect.transform.eulerAngles = new Vector3(0f, 0f, angle);
                dashEffect.GetComponent<Animator>().SetTrigger(Constants.DASH);
            }
        }

        private void AttackHandler(object sender, CharacterFightController.OnAttackEventArgs args)
        {
            if (!args.isPower)
                animator.SetTrigger(Constants.ATTACK);
        }

        private void ShootHandler(object sender, EventArgs args)
        {
            animator.SetTrigger(Constants.ATTACK);
        }
        #endregion

        #region Input Actions Handlers
        public void RollHandler(InputAction.CallbackContext context)
        {
            if (context.canceled)
                animator.SetTrigger(Constants.ROLL);
        }

        public void ToggleLights(InputAction.CallbackContext context)
        {
            if (context.performed)
                characterLight.enabled = !characterLight.enabled;
        }
        #endregion
        #endregion
    }
}
