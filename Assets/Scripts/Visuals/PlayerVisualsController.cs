/// <remarks>
/// 
/// PlayerVisualsController is used for controlling all visual parts of the character.
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

using Custom.Mechanics;
using Custom.Utils;
using Platformer.Controls;

namespace Platformer.Visuals
{
    public class PlayerVisualsController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("PlayerVisualsController --> Start: ");
     * Debug.Log("<size=13><i><b> PlayerVisualsController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerVisualsController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerVisualsController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> PlayerVisualsController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        #region Serialized Fields
        [Header("Animation")]
        [SerializeField] private RuntimeAnimatorController normalAnimatorController;
        [SerializeField] private RuntimeAnimatorController shootingAnimatorController;
        [Space] [Header("Materials")]
        [SerializeField] private Material blurMaterial;
        [SerializeField] private Material dashMaterial;
        [SerializeField] private Material glowMaterial;
        [SerializeField] private Material defaultMaterial;
        #endregion

        private Vector2 direction;

        private Light2D characterLight;
        private SpriteRenderer spriteRenderer;
        private Animator animator;

        private IEnumerator dashRoutine;
        private IEnumerator blurRoutine;
        private IEnumerator glowCoroutine;



        #region MonoBehaviour Callbacks
        private void Awake()
        {
            characterLight = transform.GetChild(0).GetChild(0).GetComponent<Light2D>();
            spriteRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

            characterLight.enabled = false;
        }

        private void Start()
        {
            //Roll.OnRoll += RollHandler;
            VectorMeleeAttack.OnAttack += AttackHandler;
            //PowerMeleeAttack.OnPowerAttack += AttackHandler;
            TriggerableTrack.OnTrack += TrackHandler;
            //DashAndReturn.OnDashStateChanged += DashHandler;
        }

        private void Update() =>
            PlayWalkAnimation();
        #endregion

        #region Functions
        private void PlayWalkAnimation()
        {
            Vector2 input = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();

            animator.SetFloat(Constants.MAGNITUDE, input.magnitude);

            if (input == Vector2.zero)
                input = PlayerController.Instance.playerControls.MainControls.Aim.ReadValue<Vector2>();

            if (input != Vector2.zero)
            {
                direction = input;
                animator.SetFloat(Constants.HORIZONTAL, direction.x);
                animator.SetFloat(Constants.VERTICAL, direction.y);
            }
        }

        #region Event Handlers
/*        private void DashHandler(object sender, DashAndReturn.OnDashStateChangedEventArgs args)
        {
            switch (args.state)
            {
                case DashAndReturn.DashState.Active:
                    PlayDashAnimation();
                    break;

                case DashAndReturn.DashState.Ready:
                    break;

                case DashAndReturn.DashState.OnCooldown:
                    break;
            }
        }*/

        private void TrackHandler(object sender, EventArgs args) =>
            PlayTrackAnimation();

        private void AttackHandler(object sender, System.EventArgs args) =>
            PlayAttackAnimation();

        private void RollHandler(object sender, System.EventArgs args) =>
            PlayRollAnimmation();
        #endregion

        #region Public Functions
        public void PlayDashAnimation()
        {
            if (dashRoutine == null)
            {
                dashRoutine = AnimateDashMaterial();
                StartCoroutine(dashRoutine);
            }
        }

        public void PlayTrackAnimation()
        {
            if (blurRoutine == null)
            {
                blurRoutine = AnimateBlurMaterial();
                StartCoroutine(blurRoutine);
            }
        }

        public void PlayAttackAnimation() =>
            animator.SetTrigger(Constants.ATTACK);

        public void PlayRollAnimmation() =>
            animator.SetTrigger(Constants.ROLL);
        #endregion

        public void ToggleLights(InputAction.CallbackContext context)
        {
            if (context.performed)
                characterLight.enabled = !characterLight.enabled;
        }
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
            int numberOfFrames = 6;
            float frameDuration = .05f;
            float blurAmount = 30f;
            float blurAmountDrop = 5f;

            spriteRenderer.material = dashMaterial;
            dashMaterial.SetVector("_Direction", direction.normalized);

            for (int i = 0; i < numberOfFrames; i++)
            {
                yield return new WaitForSeconds(frameDuration);
                dashMaterial.SetFloat("_BlurAmount", blurAmount - blurAmountDrop * i);
            }

            spriteRenderer.material = defaultMaterial;
            dashRoutine = null;
        }

        IEnumerator AnimateBlurMaterial()
        {
            int numberOfFrames = 3;
            float castingTime = .3f;
            float maxBlurAmount = 9f;
            float minBlurAmount = .5f;

            spriteRenderer.material = blurMaterial;

            for (int i = 0; i < numberOfFrames; i++)
            {
                yield return new WaitForSeconds(castingTime / numberOfFrames);
                blurMaterial.SetFloat("_BlurAmount", UnityEngine.Random.Range(minBlurAmount, maxBlurAmount));
            }

            spriteRenderer.material = defaultMaterial;
            blurRoutine = null;
        }
        #endregion
    }
}
