using System;
using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public static MonsterSpawner instance = null;

    [Header("Spawner Info")]
    public int maxCount = 5;
    public int spawnDelay = 5;
    public Monster monsterPrefab;

    public UnitData monsterData;
    public UnitData bossData;

    private float rangeX = 8.0f;
    private float rangeY = 4.5f;
    [HideInInspector] public int killCount = 0;

    #region Unity Life Cycle
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
    #region Spawn
    public IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSecondsRealtime(spawnDelay / GameManager.instance.gameSpeed);

        if (this.transform.childCount > 0)
        {
            SpawnMonster(this.transform.GetChild(0).gameObject);//, isBoss);
        }
        StartCoroutine(SpawnCoroutine());
    }
    public void EndStage()
    {
        StopAllCoroutines();
        ClearSpawner();
    }
    #endregion
    #region Object Pool
    public void SetSpawner()
    {
        killCount = 0;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameManager.instance.stage; i++)
        {
            GameObject go = GameObject.Instantiate(monsterPrefab.gameObject);
            go.GetComponent<Monster>().SetData(monsterData);
            go.transform.SetParent(this.transform);
        }

        GameObject boss = GameObject.Instantiate(monsterPrefab.gameObject);
        boss.GetComponent<Monster>().SetData(bossData);
        boss.GetComponent<Monster>().isBoss = true;
        boss.transform.SetParent(this.transform);

        StartCoroutine(SpawnCoroutine());
    }
    public void ClearSpawner()
    {
        for (int i = 0; i < GameManager.instance.monsters.childCount; i++)
        {
            Destroy(GameManager.instance.monsters.GetChild(i).gameObject);
        }
    }
    public void SpawnMonster(GameObject _go)//, bool _isBoss)
    {
        int randomIndex = UnityEngine.Random.Range(0, GameManager.instance.characters.childCount);
        Vector2 charPos= GameManager.instance.characters.GetChild(randomIndex).transform.localPosition;
        float randomX = UnityEngine.Random.Range(charPos.x - rangeX, charPos.x + rangeX);
        float randomY = UnityEngine.Random.Range(charPos.y - rangeY, charPos.y + rangeY);

        _go.transform.SetParent(GameManager.instance.monsters);
        _go.transform.localScale = Vector3.one;
        _go.transform.localPosition = new Vector3(randomX, randomY, 0.0f);
        _go.SetActive(true);
        _go.GetComponent<Monster>().Init();
    }
    public void DespawnMonster(GameObject _go, bool _isBoss)
    {
        _go.transform.SetParent(this.transform);
        _go.SetActive(false);
        if (_isBoss)
        {
            GameManager.instance.ClearStage();
        }
    }
    #endregion
}