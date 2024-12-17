using System.Collections;
using UnityEngine;

public class Hitstop : MonoBehaviour
{
    public static Hitstop Instance;

    [SerializeField] private float hitstopTimeScale = 0f;
    [SerializeField] private float defaultHitstopTime = 1f;
    [SerializeField] private float hitstopShakeStrength;
    [Space]
    [SerializeField] private MinMaxFloat enemyHealthForHitstopOnDeath;
    [SerializeField] private MinMaxFloat hitstopTimeOnEnemyDeathByHealth;

    private void Awake()
    {
        Instance = this;
    }

    public void DoHitstop(float hitstopTime = 0f)
    {
        CameraShake.Instance.Shake(hitstopShakeStrength);
        StartCoroutine(HitstopCoroutine(hitstopTime));
    }

    private IEnumerator HitstopCoroutine(float hitstopTime = 0f)
    {
        if (hitstopTime == 0) hitstopTime = defaultHitstopTime;
        
        TimeManager.Instance.SetTimeScale(hitstopTimeScale);

        var timer = 0f;
        while (timer < hitstopTime) {
            if(!GameManager.Instance.GamePaused) timer += Time.unscaledDeltaTime;
            yield return null;
        }
        
        TimeManager.Instance.SetTimeScale(1);
    }

    public void NotifyEnemyDeath(float enemyMaxHealth) {
        if (enemyMaxHealth < enemyHealthForHitstopOnDeath.min) {
            CameraShake.Instance.Shake(hitstopShakeStrength);
            return;
        }

        float f = Mathf.InverseLerp(enemyHealthForHitstopOnDeath.min, enemyHealthForHitstopOnDeath.max, enemyMaxHealth);
        float time = hitstopTimeOnEnemyDeathByHealth.LerpValue(f);
        DoHitstop(time);
    }
}