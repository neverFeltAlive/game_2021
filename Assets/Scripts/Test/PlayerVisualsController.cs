/// <remarks>
/// 
/// PlayerVisualsController is used for controlling all visual parts of the character.
/// NeverFeltAlive
/// 
/// </remarks>


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
        [Space]
        [Header("Materials")]
        [SerializeField] private Material blurMaterial;
        [SerializeField] private Material dashMaterial;
        [SerializeField] private Material glowMaterial;
        [SerializeField] private Material defaultMaterial;
        #endregion

        private bool isWalking;

        private Vector2 direction;

        private Light2D characterLight;
        private SpriteRenderer spriteRenderer;
        private Animator animator;



        #region MonoBehaviour Callbacks
        private void Awake()
        {
            characterLight = transform.GetChild(0).GetChild(0).GetComponent<Light2D>();
            spriteRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            animator = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

            characterLight.enabled = false;
        }

        private void Update()
        {
            Vector2 input = PlayerController.Instance.playerControls.MainControls.Walk.ReadValue<Vector2>();

            if (isWalking)
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
        #endregion

        #region Functions
        public void StartWalkAnimation()
        {
            isWalking = true;
        }

        public void PlayDashAnimation()
        {
            StartCoroutine(AnimateDashMaterial());
        }

        public void PlayTrackAnimation(float time)
        {
            StartCoroutine(AnimateBlurMaterial(time));
        }

        public void PlayAttackAnimation()
        {
            animator.SetTrigger(Constants.ATTACK);
        }

        public void PlayRollAnimmation()
        {
            animator.SetTrigger(Constants.ROLL);
        }

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
