using UnityEngine;

public class Character : Unit
{
    public override UnitType unitType => UnitType.Character;
    private Vector3 defaultPosition;

    #region Unity Life Cycle
    public void Awake()
    {
        defaultPosition = this.transform.localPosition;
    }
    #endregion
    #region Override
    public override void Init()
    {
        StopUnitCoroutines();
        base.Init();

        atkPower += GameManager.instance.atkLevel * 10.0f;
        maxHp += GameManager.instance.hpLevel * 10.0f;
        moveSpeed += GameManager.instance.spdLevel * 0.1f;

        this.transform.localPosition = defaultPosition;
    }
    public override void Die()
    {
        base.Die();
        GameManager.instance.CheckGameEnd();
    }
    #endregion
}
