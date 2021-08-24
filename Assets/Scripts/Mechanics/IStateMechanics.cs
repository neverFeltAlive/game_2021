/// <remarks>
/// 
/// IStateMechanics is used for implementing state macihine into mechanics script
/// NeverFeltAlive
/// 
/// </remarks>


using System;

namespace Custom.Mechanics
{
    public enum MechanicsState
    {
        Ready,
        Active,
        OnCooldown
    }

    public class OnStateChangedEventArgs : EventArgs
    {
        public MechanicsState state;
    }

    public interface IStateMechanics<TEventArgs> : IMechanics<TEventArgs>
        where TEventArgs : EventArgs
    {
        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

        public MechanicsState State { get; }
    }
}
