public class EnemyHealth : Health {
    
    protected override void DeathEffect() {
        Destroy(gameObject);
    }
}