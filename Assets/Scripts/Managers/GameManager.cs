using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Game Status")]
    [SerializeField] private Camera mainCamera;
    private int cameraIndex = 0;

    [Header("Game Status")]
    public int stage = 1;
    public int gameSpeed = 1;
    public int gold = 0;
    public List<UnitData> charList;

    [Header("Transform")]
    public Transform characters;
    public Transform monsters;

    //[HideInInspector] public UnitData[] unitDatas;

    [HideInInspector] public int hpLevel = 1;
    [HideInInspector] public int atkLevel = 1;
    [HideInInspector] public int spdLevel = 1;

    #region Unity Life Cycle
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void Start()
    {
        UIManager.Instance.ShowSelectPopup();
    }
    #endregion

    #region Stage
    public void StartStage()
    {
        SetStage(stage);
    }
    public void CheckGameEnd()
    {
        bool gameEnd = true;
        for (int i = 0; i < characters.childCount; i++)
        {
            if (characters.GetChild(i).GetComponent<Unit>().unitState == UnitState.Live)
            {
                gameEnd = false;
            }
        }
        if(gameEnd)
        {
            Debug.Log("GameOver");
            //SetStage();
        }
    }
    public void SetStage(int _stage)
    {
        if (_stage == 1)
        {
            stage = 1;
            gold = 0;
            atkLevel = 1;
            hpLevel = 1;
            spdLevel = 1;
        }

        MonsterSpawner.instance.StopSpawnerCoroutine();
        MonsterSpawner.instance.ClearSpawner();
        MonsterSpawner.instance.SetSpawner();

        UIManager.Instance.RefreshStage();
        UIManager.Instance.RefreshGold();

        for (int i = 0; i < charList.Count; i++)
        {
            Character curChar = characters.GetChild(i).GetComponent<Character>();
            curChar.SetData(charList[i]);
            curChar.Init();
        }
        SetCamera(cameraIndex);
    }
    public void ClearStage()
    {
        for (int i = 0; i < charList.Count; i++)
        {
            characters.GetChild(i).GetComponent<Character>().StopUnitCoroutines();
        }
        for (int i = 0; i < monsters.childCount; i++)
        {
            Destroy(monsters.GetChild(i).gameObject);
        }
        MonsterSpawner.instance.StopSpawnerCoroutine();
        stage++;
        UIManager.Instance.ShowSelectPopup();
    }
    #endregion
    #region Camera
    public void ChangeCamera()
    {
        if (charList.Count == 1)
            return;

        SoundManager.instance.PlayButtonSound();
        cameraIndex = cameraIndex + 1 == charList.Count ? 0 : cameraIndex + 1;

        SetCamera(cameraIndex);
    }
    public void SetCamera(int _index)
    {
        mainCamera.transform.SetParent(characters.GetChild(_index));
        mainCamera.transform.localPosition = new Vector3(0.0f, 0.0f, -20.0f);
        mainCamera.transform.localScale = Vector3.one;
    }
    #endregion
    #region Setting
    public void ChangeSpeed()
    {
        SoundManager.instance.PlayButtonSound();
        gameSpeed = gameSpeed == 1 ? 2 : 1;
        UIManager.Instance.ChangeIcon(gameSpeed);
        SoundManager.instance.sourceBGM.pitch = gameSpeed;
    }
    #endregion
}
