using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Unit
{
    public override UnitType unitType => UnitType.Monster;

    [Header("Monster Reward")]
    [HideInInspector] public bool isBoss = false;
    public int rewardGold { get; private set; }
    private float despawnDelay = 1.2f;

    #region Override
    public override void Init()
    {
        atkPower = MonsterSpawner.instance.monsterData.atkPower * GameManager.instance.stage;
        maxHp = MonsterSpawner.instance.monsterData.maxHp * GameManager.instance.stage;
        base.Init();
    }
    public override void SetData(UnitData _data)
    {
        base.SetData(_data);
        rewardGold = _data.rewardGold;
    }
    public override void Die()
    {
        base.Die();

        GameManager.instance.SetGold(rewardGold);
        UIManager.instance.RefreshUI();
        if (!isBoss)
        {
            MonsterSpawner.instance.killCount++;
        }
        StartCoroutine(DespawnCoroutine());
    }
    #endregion
    public IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSecondsRealtime(despawnDelay / GameManager.instance.gameSpeed);
        MonsterSpawner.instance.DespawnMonster(this.gameObject, this.isBoss);
    }
}
