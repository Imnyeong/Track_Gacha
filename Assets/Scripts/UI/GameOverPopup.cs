using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text text;

    public void ShowGameOverPopup()
    {
        text.text = $"STAGE {GameManager.instance.stage} ���� ���� �Ϸ�!";
        button.onClick.AddListener(delegate
        {
            HideGameOverPopup();
            GameManager.instance.SetFirst();
            UIManager.instance.ShowSelectPopup();
        });
        this.gameObject.SetActive(true);
    }

    public void HideGameOverPopup()
    {
        button.onClick.RemoveAllListeners();
        this.gameObject.SetActive(false);
    }
}