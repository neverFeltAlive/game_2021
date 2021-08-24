/// <remarks>
/// 
/// ICooldown is used for implementing cooldown into a mechanics class
/// NeverFeltAlive
/// 
/// </remarks>


using System;

namespace Custom.Mechanics
{
    public interface ICooldown<TEventArgs> : IMechanics<TEventArgs>
        where TEventArgs : EventArgs
    {
        public void StartCooldown();

        public void StopCooldown();

        public bool IsOnCooldown();
    }
}
