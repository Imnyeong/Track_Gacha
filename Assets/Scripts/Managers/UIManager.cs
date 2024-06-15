using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;

    [SerializeField] private Text stageText;
    [SerializeField] private Text goldText;

    [SerializeField] private Image speedIcon;
    [SerializeField] private Sprite[] speedSprites;

    [SerializeField] private SelectPopup selectPopup;
    [SerializeField] private GameOverPopup gameoverPopup;

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
    #region UI
    public void RefreshUI()
    {
        stageText.text = GameManager.instance.stage.ToString();
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
    public void ShowGameOverPopup()
    {
        gameoverPopup.ShowGameOverPopup();
    }
    public void HideGameOverPopup()
    {
        gameoverPopup.HideGameOverPopup();
    }
    #endregion
}