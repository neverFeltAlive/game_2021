using System;

namespace Custom.Mechanics
{
    /// <summary>
    /// States for simple state machine implementation
    /// </summary>
    public enum State
    {
        Active,
        Ready,
        OnCooldown
    }

    /// <summary>
    /// Event arguments for events in simple state machine systems
    /// </summary>
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
}
