/// <remarks>
/// 
/// CharacterVisualsController is used for controlling all visual parts of the character.
/// Animation, lights and etc.
/// NeverFeltAlive
/// 
/// </remarks>


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
        [SerializeField] private Animator animator;
        [SerializeField] private Light2D characterLight;
        [Space]
        [SerializeField] private Color32 defaultColor;
        [SerializeField] private Color32 shootingColor;

        private Vector2 direction;
        private PlayerInput playerInput;



        private void Start()
        {
            if (!characterLight)
                characterLight = transform.GetChild(0).GetComponent<Light2D>();
            characterLight.enabled = false;

            playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (playerInput.currentActionMap.name == Constants.DEFAULT_MAP)
            {
                direction = CharacterMovementController.playerControls.MainControls.Walk.ReadValue<Vector2>();
                animator.SetFloat(Constants.MAGNITUDE, direction.magnitude);
            }
            else
            {
                direction = CharacterMovementController.playerControls.ShootingControls.Aim.ReadValue<Vector2>();
                animator.SetFloat(Constants.MAGNITUDE, 0f);
            }

            if (direction != Vector2.zero)                                                      
            {
                animator.SetFloat(Constants.HORIZONTAL, direction.x);
                animator.SetFloat(Constants.VERTICAL, direction.y);
            }
        }

        public void ToggleLights(InputAction.CallbackContext context)
        {
            if (context.performed)
                characterLight.enabled = !characterLight.enabled;
        }

        public void ChangeLightColor(InputAction.CallbackContext context)
        {
/*            if (!context.canceled)
                return;

            if (characterLight.color == shootingColor)
                characterLight.color = defaultColor;
            else
                characterLight.color = shootingColor;*/
        }
    }
}
