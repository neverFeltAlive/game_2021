/// <remarks>
/// 
/// CharacterVisualsController is used for controlling all visual parts of the character.
/// Animation, lights and etc.
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
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
        [Header("Objects")]
        [SerializeField] private Light2D characterLight;
        [Space]
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private RuntimeAnimatorController normalAnimatorController;
        [SerializeField] private RuntimeAnimatorController shootingAnimatorController;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Space]
        [Header("Materials")]
        [SerializeField] private Material blurMaterial;
        [SerializeField] private Material dashMaterial;
        [SerializeField] private Material glowMaterial;
        [SerializeField] private Material defaultMaterial;
        #endregion

        private Vector2 direction;



        #region MonoBehaviour Callbacks
        private void Start()
        {
            if (!characterLight)
                characterLight = transform.GetChild(0).GetComponent<Light2D>();
            if (!spriteRenderer)
                spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

            characterLight.enabled = false;
            spriteRenderer.material.mainTextureScale = new Vector2(200, 200);
            //spriteRenderer.material.mainTexture = new Texture2D(200, 200, TextureFormat.DXT5, false);

            CharacterFightController.OnAttack += AttackHandler;
            CharacterFightController.OnFightStateChanged += FightStateChangeHaandler;
            CharacterFightController.OnShoot += ShootHandler;
            DashController.OnDashStateChanged += DashHandler;
            TrackController.OnTrack += TrackHandler;
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
                animator.runtimeAnimatorController = normalAnimatorController;
            else
                animator.runtimeAnimatorController = shootingAnimatorController;

            StartCoroutine(AnimateGlowMaterial(1));
        }

        private void DashHandler(object sender, DashController.OnDashStateChangedEventArgs args)
        {
            if (args.state == DashController.DashState.Active)
            {
                StartCoroutine(AnimateDashMaterial());
            }
        }

        private void TrackHandler(object sender, TrackController.OnTrackEventArgs args)
        {
            StartCoroutine(AnimateBlurMaterial(args.castingTime));
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



        #region Coroutines
        IEnumerator AnimateGlowMaterial(float time = 2f)
        {
            spriteRenderer.material = glowMaterial;
            yield return new WaitForSeconds(time);
            spriteRenderer.material = defaultMaterial;
        }

        IEnumerator AnimateDashMaterial()
        {
            float frameDuration = .1f;

            spriteRenderer.material = dashMaterial;

            dashMaterial.SetFloat("_Step", 1f);
            dashMaterial.SetFloat("_BlurAmount", 30f);
            dashMaterial.SetVector("_Direction", direction.normalized);

            yield return new WaitForSeconds(frameDuration);
            dashMaterial.SetFloat("_BlurAmount", dashMaterial.GetFloat("_BlurAmount") - 12.8f);
            yield return new WaitForSeconds(frameDuration);
            dashMaterial.SetFloat("_BlurAmount", dashMaterial.GetFloat("_BlurAmount") - 7f);
            yield return new WaitForSeconds(frameDuration);

            spriteRenderer.material = defaultMaterial;
        }

        IEnumerator AnimateBlurMaterial(float duration)
        {
            spriteRenderer.material = blurMaterial;

            blurMaterial.SetFloat("_BlurAmount", 6f);
            yield return new WaitForSeconds(duration / 3);
            blurMaterial.SetFloat("_BlurAmount", 2f);
            yield return new WaitForSeconds(duration / 3);
            blurMaterial.SetFloat("_BlurAmount", 9f);
            yield return new WaitForSeconds(duration / 3);

            spriteRenderer.material = defaultMaterial;
        }
        #endregion
    }
}
