using UnityEngine;
using UnityEngine.InputSystem;

using Custom.Mechanics;

namespace Custom.Controlls
{
    /// <summary>
    /// 
    /// PlayerTriggerableTrackController is used for controlling triggerable track with user input 
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(TriggerableTrack))]
    public class PlayerTriggerableTrackController : MonoBehaviour
    {
        private TriggerableTrack track;



        private void Awake() =>
            track = GetComponent<TriggerableTrack>();

        public void TriggerTrack(InputAction.CallbackContext context)
        {
            if (context.performed)
                track.TriggerSaving();

            if (context.canceled)
                track.TriggerTrack();
        }
    }
}
