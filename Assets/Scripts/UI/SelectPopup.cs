using UnityEngine;

public class SelectPopup : MonoBehaviour
{
    [SerializeField] private SelectButton[] selects;

    public void ShowSelectPopup()
    {
        foreach(SelectButton select in selects)
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