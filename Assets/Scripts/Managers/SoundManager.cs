using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource sourceBGM;
    public AudioSource sourceEffect;
    public AudioClip clipOutgameBGM;
    public AudioClip clipIngameBGM;
    public AudioClip[] clipEffects;
    public AudioClip clipButton;
    public AudioClip clipUpgrade;

    public static SoundManager instance = null;

    #region Unity Life Cycle
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        StartBGM("OutGame");
    }
    #endregion
    #region BGM
    public void StartBGM(string _sceneName)
    {
        if (_sceneName == "OutGame")
        {
            sourceBGM.clip = clipOutgameBGM;
        }
        else
        {
            sourceBGM.clip = clipIngameBGM;
        }
        sourceBGM.Play();
    }
    #endregion
    #region Effect
    public void PlayButtonSound()
    {
        sourceEffect.clip = clipButton;
        if (GameManager.instance == null)
        {
            sourceEffect.pitch = (1.0f);
        }
        else
        {
            sourceEffect.pitch = (GameManager.instance.gameSpeed);
        }
        sourceEffect.Play();
    }
    public void PlayUpgradeSound()
    {
        sourceEffect.clip = clipUpgrade;
        sourceEffect.pitch = (GameManager.instance.gameSpeed);
        sourceEffect.Play();
    }
    public void PlayEffect(CharacterClass _class)
    {
        switch (_class)
        {
            case CharacterClass.Tanker:
                {
                    sourceEffect.clip = clipEffects[0];
                    break;
                }
            case CharacterClass.ShortDistanceAttack:
                {
                    sourceEffect.clip = clipEffects[1];
                    break;
                }
            case CharacterClass.LongDistanceAttack:
                {
                    sourceEffect.clip = clipEffects[2];
                    break;
                }
            case CharacterClass.Healer:
                {
                    sourceEffect.clip = clipEffects[3];
                    break;
                }
        }
        sourceEffect.pitch = (GameManager.instance.gameSpeed);
        sourceEffect.Play();
    }
    #endregion
}
