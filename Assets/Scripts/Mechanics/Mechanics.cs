/// <remarks>
/// 
/// Mechanics is used for representing basic structure for mechanics class
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

namespace Custom.Mechanics
{
    public abstract class Mechanics<TEventArgs> : MonoBehaviour, IMechanics<TEventArgs>
        where TEventArgs : EventArgs
    {
        public event EventHandler<TEventArgs> OnTriggered;
        public event EventHandler<TEventArgs> OnPerforemed;
        /// <remarks>
        /// These events are not virtual because we need to make sure that every mechanic fires them.
        /// We use generics and GenerateEventArgs method to still be able to customize events args
        /// </remarks>



        #region IMehanics implementation
        // Fires trigger event and hands execution over to handle function
        public virtual void Trigger()
        {
            OnTriggered?.Invoke(this, GenerateEventArgs());
            Handle();
        }

        // Performs the action and fires the performed event
        protected virtual void Handle() =>
            OnPerforemed?.Invoke(this, GenerateEventArgs());
        #endregion

        protected abstract TEventArgs GenerateEventArgs();
    }
}
