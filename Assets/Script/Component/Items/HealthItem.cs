public class HealthItem : BaseItem
{
    public override void SetAmount(BaseCharacter baseCharacter)
    {
        base.SetAmount(baseCharacter);
        var maxHealth = baseCharacter.MaxHealth;
        var min = maxHealth * 0.2f;
        var max = maxHealth * 0.4f;
        amount = GameUtils.RandomWeighted((int)min, (int)max, 3);
    }
}