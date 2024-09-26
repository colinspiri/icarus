public class PlayerHealth : Health {
    protected override void DeathEffect() {
        GameManager.Instance.Reload();
    }
}