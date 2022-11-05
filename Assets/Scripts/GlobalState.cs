
public class GlobalState : Singleton<GlobalState>
{
    public float resourceHeath;
    public float resourceMaxHealth;

    public void SetHealth(float maxHealth, float currentHealth)
    {
        resourceMaxHealth = maxHealth;
        resourceHeath = currentHealth;
    }
}
