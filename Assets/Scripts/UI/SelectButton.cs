using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image sprite;
    [SerializeField] private Text content;
    [SerializeField] private Text price;

    private const int defaultValue = 10;
    private const int defaultCost = 100;

    private const int weightOne = 70;
    private const int weightTwo = 20;
    private const int weightThree = 10;

    private const int costOne = 100;
    private const int costTwo = 500;
    private const int costThree = 1000;

    UnitData curUnitData = null;
    public void SetUpgrade()
    {
        button.onClick.RemoveAllListeners();
        int randomValue = Random.Range(0, 2);
        int priceValue;

        sprite.color = new Color32(0, 0, 0, 0);

        if (randomValue == 0)
        {
            content.text  = $"아군의 공격력을 {defaultValue * GameManager.instance.atkLevel} 만큼 증가시킵니다.";
            price.text  = $"${defaultCost * GameManager.instance.atkLevel}";
            priceValue = defaultCost * GameManager.instance.atkLevel;
        }
        else if (randomValue == 1)
        {
            content.text = $"아군의 체력을 {defaultValue * GameManager.instance.hpLevel} 만큼 증가시킵니다.";
            price.text = $"${defaultCost * GameManager.instance.hpLevel}";
            priceValue = defaultCost * GameManager.instance.hpLevel;
        }
        else
        {
            content.text = $"아군의 이동속도를 {GameManager.instance.spdLevel} 만큼 증가시킵니다.";
            price.text = $"{defaultCost * GameManager.instance.spdLevel}";
            priceValue = defaultCost * GameManager.instance.spdLevel;
        }
        button.onClick.AddListener(delegate
        {
            OnClickUpgrade(randomValue, priceValue);
        });
    }
    public void SetCharacter()
    {
        button.onClick.RemoveAllListeners();
        curUnitData = null;
        int randomValue = Random.Range(0, 100);
        int priceValue;
        sprite.color = new Color32(255, 255, 255, 255);

        if (GameManager.instance.stage == 1)
            randomValue = 0;

        if (randomValue <= weightOne)
        {
            UnitData[] class1Chars = Resources.LoadAll<UnitData>("Characters/Class_1");
            curUnitData = class1Chars[Random.Range(0, class1Chars.Length)];
            priceValue = costOne;
        }
        else if (randomValue > weightOne && randomValue < weightOne + weightTwo)
        {
            UnitData[] class2Chars = Resources.LoadAll<UnitData>("Characters/Class_2");
            curUnitData = class2Chars[Random.Range(0, class2Chars.Length)];
            priceValue = costTwo;
        }
        else
        {
            UnitData[] class3Chars = Resources.LoadAll<UnitData>("Characters/Class_3");
            curUnitData = class3Chars[Random.Range(0, class3Chars.Length)];
            priceValue = costThree;
        }
        price.text = priceValue.ToString();
        content.text = $"공격력: {curUnitData.atkPower}\n체력: {curUnitData.maxHp}\n이동속도: {curUnitData.moveSpeed}";
        sprite.sprite = curUnitData.sprite;
        button.onClick.AddListener(delegate 
        {
            OnClickCharacter(priceValue);
        });
    }
    public void OnClickUpgrade(int _type,  int _price)
    {
        if (GameManager.instance.gold < _price)
        {
            return;
        }

        SoundManager.instance.PlayUpgradeSound();
        GameManager.instance.SetGold(_price * -1);
        if (_type == 0)
        {
            GameManager.instance.atkLevel++;
        }
        else if (_type == 1)
        {
            GameManager.instance.hpLevel++;
        }
        else
        {
            GameManager.instance.spdLevel++;
        }

        UIManager.instance.HideSelectPopup();
        GameManager.instance.SetStage(GameManager.instance.stage);
    }
    public void OnClickCharacter(int _price)
    {
        if(!GameManager.instance.CanAddUnit() || GameManager.instance.gold < _price)
        {
            return;
        }
        SoundManager.instance.PlayUpgradeSound();
        GameManager.instance.BuyCharacter(curUnitData, _price);
        UIManager.instance.HideSelectPopup();
        GameManager.instance.SetStage(GameManager.instance.stage);
    }
}
