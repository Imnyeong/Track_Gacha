using UnityEngine;
using UnityEngine.UI;

public class SelectPopup : MonoBehaviour
{
    [SerializeField] private SelectButton[] selects;
    [SerializeField] private Button skipButton;

    private void Start()
    {
        skipButton.onClick.RemoveAllListeners();
        skipButton.onClick.AddListener(delegate
        {
            UIManager.instance.HideSelectPopup();
            GameManager.instance.SetStage(GameManager.instance.stage);
        });
    }
    public void ShowSelectPopup()
    {
        skipButton.interactable = !(GameManager.instance.stage == 1);

        foreach (SelectButton select in selects)
        {
            if(GameManager.instance.stage == 1)
            {
                select.SetCharacter();
            }
            else
            {
                int randomValue = Random.Range(0, 100);
                if (randomValue <= 80)
                {
                    select.SetUpgrade();
                }
                else
                {
                    select.SetCharacter();
                }
            }
        }
        this.gameObject.SetActive(true);
    }

    public void HideSelectPopup()
    {
        this.gameObject.SetActive(false);
    }
}