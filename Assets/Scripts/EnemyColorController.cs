using UnityEngine;

public class EnemyColorController : ColorController {
    // components
    [SerializeField] private EnemyHealth enemyHealth;
    [Space]
    
    // params 
    [SerializeField] private Color damagedColor;
    [SerializeField] private float damagedTime;
    [Space] 
    [SerializeField] private Color lowHealthColor;
    [SerializeField] private float lowHealthFlashPeriod;
    
    protected override void Start() {
        base.Start();
        enemyHealth.OnTakeDamageAction += () => {
            base.SetTemporaryColor(damagedColor, damagedTime);
            
            if (enemyHealth.HealthLeft <= 1) {
                base.SetFlashing(lowHealthColor, lowHealthFlashPeriod, -1);
            }
        };
    }
}