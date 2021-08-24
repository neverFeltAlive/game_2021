/// <remarks>
/// 
/// PlayerMechanicsController is used for controlling simple mechanics with user input
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Custom.Mechanics;

namespace Custom.Controlls
{
    public abstract class PlayerMechanicsController<TEventArgs, TMech> : MonoBehaviour
        where TEventArgs : EventArgs
        where TMech : Mechanics<TEventArgs>
    {
        protected TMech mechanics;



        protected virtual void Awake() =>
            mechanics = gameObject.GetComponent<TMech>();

        public virtual void Trigger(InputAction.CallbackContext context)
        {
            if (context.canceled)
                mechanics.Trigger();
        }
    }
}
