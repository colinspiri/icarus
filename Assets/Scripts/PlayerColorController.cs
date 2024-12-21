
using UnityEngine;

public class PlayerColorController : ColorController {
    // components
    [SerializeField] private PlayerHealth playerHealth;
    [Space]

    // params
    [SerializeField] private Color damagedColor;
    [SerializeField] private float damagedTime;
    [Space] 
    [SerializeField] private Color invulnerableFlashColor;
    [SerializeField] private float invulnerableFlashPeriod;
    [Space]
    [SerializeField] private Color dashColor;

    protected override void Start() { 
        base.Start();
        playerHealth.OnTakeDamageAction += () => {
            base.SetTemporaryColor(damagedColor, damagedTime);
        };
        playerHealth.OnStartInvulnerable += invulnerableTime => {
            base.SetFlashing(invulnerableFlashColor, invulnerableFlashPeriod, invulnerableTime);
        };
    }

    public void Dash(float dashTime) {
        base.SetTemporaryColor(dashColor, dashTime);
    }
}