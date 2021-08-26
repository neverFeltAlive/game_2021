namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// IDisablableMovement is used for implementing a possibility to disable default input in the movement mechinics
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public interface IDisablableMovement : IMovement
    {
        public void DisableMovement();
        public void DisableMovement(float time);
        public void EnableMovement();

        public bool IsMoving();
    }
}
