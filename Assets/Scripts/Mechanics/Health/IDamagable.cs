namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// IDamagable is used for identifying objects that can be damaged
    /// NeverFeltAlive
    /// 
    /// </summary>
    /// <typeparam name="TDamage">Damage type</typeparam>
    public interface IDamagable<TDamage>
    {
        public void TakeDamage(TDamage damage);
    }
}
