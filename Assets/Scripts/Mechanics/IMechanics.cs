/// <remarks>
/// 
/// IMechanics is used for implementing event system in mechanics script
/// NeverFeltAlive
/// 
/// </remarks>


using System;

namespace Custom.Mechanics
{
    public interface IMechanics<TEventArgs>
        where TEventArgs : EventArgs
    {
        public event EventHandler<TEventArgs> OnTriggered;
        public event EventHandler<TEventArgs> OnPerforemed;

        public void Trigger();
    }
}
