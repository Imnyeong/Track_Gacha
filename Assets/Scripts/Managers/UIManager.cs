using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    [SerializeField] private Text stageText;
    [SerializeField] private Text goldText;

    [SerializeField] private Image speedIcon;
    [SerializeField] private Sprite[] speedSprites;

    [SerializeField] private SelectPopup selectPopup;

    #region Unity Life Cycle
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
    #region UI
    public void RefreshStage()
    {
        stageText.text = GameManager.instance.stage.ToString();
    }
    public void RefreshGold()
    {
        goldText.text = GameManager.instance.gold.ToString();
    }
    public void ChangeIcon(int _value)
    {
        speedIcon.sprite = speedSprites[Convert.ToInt32(_value != 2)];
    }
    public void ShowSelectPopup()
    {
        selectPopup.ShowSelectPopup();
    }
    public void HideSelectPopup()
    {
        selectPopup.HideSelectPopup();
    }
    #endregion
}