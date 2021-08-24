/// <remarks>
/// 
/// StateMechanics is used for representing basic structure for state mechanics class
/// NeverFeltAlive
/// 
/// </remarks>


using System;

namespace Custom.Mechanics
{
    public abstract class StateMechanics<TEventArgs> : Mechanics<TEventArgs> , IStateMechanics<TEventArgs>
        where TEventArgs : EventArgs 
    {
        public virtual event EventHandler<OnStateChangedEventArgs> OnStateChanged;


            
        public MechanicsState State { get; protected set; }



        protected void ChangeState(MechanicsState state)
        {
            this.State = state;
            OnStateChanged.Invoke(this, new OnStateChangedEventArgs { state = this.State });
        }
    }
}
