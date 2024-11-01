public class PlayerHealth : Health {
    
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        CameraShake.Instance.Shake();
    }

    protected override void DeathEffect() {
        GameManager.Instance.Reload();
    }
}