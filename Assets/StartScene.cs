using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] RectTransform panda;
    [SerializeField] RectTransform rabbit;
    [SerializeField] RectTransform btn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AlphaFadeManager.Instance.FadeIn(1.0f);
        AudioManager.Instance.SetMusicVolume(0.6f);
        AudioManager.Instance.PlayMusic("Fun & Frolic");
        panda.DOMoveY(panda.position.y + 20.0f, 3.0f)
              .SetLoops(-1, LoopType.Yoyo)
              .SetEase(Ease.InOutSine);

        rabbit.DOMoveY(rabbit.position.y + 20.0f, 3.0f)
              .SetDelay(0.25f) // Delay by 0.5 seconds
              .SetLoops(-1, LoopType.Yoyo)
              .SetEase(Ease.InOutSine);
    }

    public void OnClickButton()
    {
        AudioManager.Instance.PlaySFX("confirm", 2.0f);
        btn.GetComponent<UnityEngine.UI.Button>().interactable = false;
        AlphaFadeManager.Instance.FadeOut(1.0f);

        AudioManager.Instance.StopMusicWithFade();

        DOVirtual.DelayedCall(1.5f, () => {
            SceneManager.LoadScene("StoryStart"); // Replace with your scene name
        });
    }
}
