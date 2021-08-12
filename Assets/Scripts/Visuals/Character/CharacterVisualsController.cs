/// <remarks>
/// 
/// CharacterVisualsController is used for [short description]
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

        private Vector2 direction;



        private void Start()
        {
            if (!characterLight)
                characterLight = transform.GetChild(0).GetComponent<Light2D>();
            characterLight.enabled = false;
        }

        private void Update()
        {
            direction = CharacterMovementController.playerControls.MainControls.Walk.ReadValue<Vector2>();

            if (direction != Vector2.zero)                                                      
            {
                animator.SetFloat(Constants.HORIZONTAL, direction.x);
                animator.SetFloat(Constants.VERTICAL, direction.y);
            }
            animator.SetFloat(Constants.MAGNITUDE, direction.magnitude);
        }

        public void ToggleLights(InputAction.CallbackContext context)
        {
            if (context.performed)
                characterLight.enabled = !characterLight.enabled;
        }
    }
}
