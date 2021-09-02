public class HealthSystem
{  
    private int health;
    private int maxHealth;
    public healthBar healthBar;
    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }
    public int getHealth()
    {
        return health;
    }
    public void takeDamage(int damageAmount)
    {
        health -= damageAmount;
        if(health <0)
        {
            health = 0;
        }
        healthBar.setHealth(health);

    }
    public void heal(int healAmount)
    {
        health += healAmount;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.setHealth(health);
    }
}
