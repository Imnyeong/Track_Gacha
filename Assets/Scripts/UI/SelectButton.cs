using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image sprite;
    [SerializeField] private Text content;
    [SerializeField] private Text price;

    UnitData curUnitData = null;
    public void SetUpgrade()
    {
        button.onClick.RemoveAllListeners();
        int randomValue = UnityEngine.Random.Range(0, 2);
        int priceValue;

        sprite.color = new Color32(0, 0, 0, 0);

        if (randomValue == 0)
        {
            content.text  = $"아군의 공격력을 {10 * GameManager.instance.atkLevel} 만큼 증가시킵니다.";
            price.text  = $"${100 * GameManager.instance.atkLevel}";
            priceValue = 100 * GameManager.instance.atkLevel;
        }
        else if (randomValue == 1)
        {
            content.text = $"아군의 체력을 {10 * GameManager.instance.hpLevel} 만큼 증가시킵니다.";
            price.text = $"${100 * GameManager.instance.hpLevel}";
            priceValue = 100 * GameManager.instance.hpLevel;

        }
        else
        {
            content.text = $"아군의 이동속도를 {GameManager.instance.spdLevel} 만큼 증가시킵니다.";
            price.text = $"{100 * GameManager.instance.spdLevel}";
            priceValue = 100 * GameManager.instance.spdLevel;
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
        int randomValue = UnityEngine.Random.Range(0, 100);
        int priceValue;

        sprite.color = new Color32(255, 255, 255, 255);

        if (randomValue <= 70)
        {
            UnitData[] class1Chars = Resources.LoadAll<UnitData>("Characters/Class_1");
            curUnitData = class1Chars[UnityEngine.Random.Range(0, class1Chars.Length)];
            price.text = "1000";
            priceValue = 1000;
        }
        else if (randomValue > 70 && randomValue < 90)
        {
            UnitData[] class2Chars = Resources.LoadAll<UnitData>("Characters/Class_2");
            curUnitData = class2Chars[UnityEngine.Random.Range(0, class2Chars.Length)];
            price.text = "5000";
            priceValue = 5000;
        }
        else
        {
            UnitData[] class3Chars = Resources.LoadAll<UnitData>("Characters/Class_3");
            curUnitData = class3Chars[UnityEngine.Random.Range(0, class3Chars.Length)];
            price.text = "10000";
            priceValue = 10000;
        }
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
        GameManager.instance.gold -= _price;
        UIManager.Instance.RefreshGold();
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

        UIManager.Instance.HideSelectPopup();
        GameManager.instance.SetStage(GameManager.instance.stage);
    }

    public void OnClickCharacter(int _price)
    {
        if(GameManager.instance.charList.Count > 3 || GameManager.instance.gold < _price)
        {
            return;
        }
        GameManager.instance.charList.Add(curUnitData);
        SoundManager.instance.PlayUpgradeSound();
        GameManager.instance.gold -= _price;
        UIManager.Instance.RefreshGold();

        UIManager.Instance.HideSelectPopup();
        GameManager.instance.SetStage(GameManager.instance.stage);
    }
}
