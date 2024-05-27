using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text text;

    public void ShowGameOverPopup()
    {
        text.text = $"STAGE {GameManager.instance.stage} 까지 돌파 완료!";
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