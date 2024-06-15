using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    private int cameraIndex = 0;

    [Header("Game Status")]
    public int gameSpeed = 1;
    public List<UnitData> charList { get; private set; } = new List<UnitData>();
    public int stage { get; private set; } = 1;
    public int gold { get; private set; } = 0;

    [Header("Transform")]
    public Transform characters;
    public Transform monsters;

    [HideInInspector] public int hpLevel = 1;
    [HideInInspector] public int atkLevel = 1;
    [HideInInspector] public int spdLevel = 1;

    [HideInInspector] private const int maxUnitCount = 4;
    private Vector3 defaultCameraPos = new Vector3(0.0f, 0.0f, -20.0f);

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
        SetFirst();
        ResetData();
        UIManager.instance.ShowSelectPopup();
    }
    #endregion
    public bool CanAddUnit()
    {
        return charList.Count < maxUnitCount;
    }
    #region Stage
    public void SetFirst()
    {
        stage = 1;
        gold = 100;
        charList.Clear();
        UIManager.instance.RefreshUI();
    }
    public void ResetData()
    {
        atkLevel = 1;
        hpLevel = 1;
        spdLevel = 1;
    }
    public void CheckGameEnd()
    {
        bool gameEnd = true;
        for (int i = 0; i < characters.childCount; i++)
        {
            if (characters.GetChild(i).GetComponent<Unit>().unitState == UnitState.Live)
            {
                gameEnd = false;
                break;
            }
        }
        if(gameEnd)
        {            
            MonsterSpawner.instance.EndStage();
            UIManager.instance.ShowGameOverPopup();
        }
    }
    public void SetStage(int _stage)
    {
        for (int i = 0; i < characters.childCount; i++)
        {
            characters.GetChild(i).gameObject.SetActive(false);
        }
        if (_stage == 1)
        {
            ResetData();
        }
        MonsterSpawner.instance.SetSpawner();

        for (int i = 0; i < charList.Count; i++)
        {
            Character curChar = characters.GetChild(i).GetComponent<Character>();
            curChar.SetData(charList[i]);
            curChar.Init();
        }
        SetCamera(cameraIndex);
        UIManager.instance.RefreshUI();
    }
    public void ClearStage()
    {
        stage++;
        MonsterSpawner.instance.EndStage();
        UIManager.instance.ShowSelectPopup();
    }
    public void SetGold(int _value)
    {
        gold += _value;
        UIManager.instance.RefreshUI();
    }
    public void BuyCharacter(UnitData _unitData, int _price)
    {
        charList.Add(_unitData);
        gold -= _price;
        UIManager.instance.RefreshUI();
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
        mainCamera.transform.localPosition = defaultCameraPos;
        mainCamera.transform.localScale = Vector3.one;
    }
    #endregion
    #region Setting
    public void ChangeSpeed()
    {
        SoundManager.instance.PlayButtonSound();
        gameSpeed = gameSpeed == 1 ? 2 : 1;
        UIManager.instance.ChangeIcon(gameSpeed);
        SoundManager.instance.sourceBGM.pitch = gameSpeed;
    }
    #endregion
}
