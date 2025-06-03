using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameoverManager : MonoBehaviour
{
    public GameObject retrybutton;
    public TMPro.TMP_Text text;
    public string BGM = "Mischief in Motion";
    [TextArea] public string showtext = "‚Ó‚½‚è‚Ç‚à‚µ‚å‚Ú‚ñ‚µ‚Ä‚¢‚é...\n‚à‚¤ƒ_ƒ‚¾...";
    public float showtextTime = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlphaFadeManager.Instance.FadeIn(1.0f);
        AudioManager.Instance.PlayMusicWithFade(BGM, 1.0f);

        DOTween.Sequence()
            .AppendInterval(2.0f)
            .Append(text.DOText(showtext, showtextTime)).SetEase(Ease.Linear).SetUpdate(true)
            .AppendInterval(1f)
            .AppendCallback(() => AudioManager.Instance.PlaySFX("kawaii"))
            .AppendCallback(() => retrybutton.gameObject.SetActive(true));
    }

    public void OnClickRetryButton()
    {
        AudioManager.Instance.PlaySFX("cutePush");

        retrybutton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        AlphaFadeManager.Instance.FadeOut(1.0f);
        AudioManager.Instance.StopMusicWithFade();

        DOVirtual.DelayedCall(1.5f, () => {
            SceneManager.LoadScene("Title"); // Replace with your scene name
        });
    }
}
