/// <remarks>
/// 
/// CharacterVisualsController is used for [short description]
/// NeverFeltAlive
/// 
/// </remarks>


using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

// [add custom usings if needed]

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
        [SerializeField] private Light2D light;



        private void Start()
        {
            if (!light)
                light = transform.GetChild(0).GetComponent<Light2D>();
            light.enabled = false;
        }

        #region Funstions
        // Turn on and off lights
        public void ToggleLights(InputAction.CallbackContext context)
        {
            if (context.performed)
                light.enabled = !light.enabled;
        }
        #endregion
    }
}
