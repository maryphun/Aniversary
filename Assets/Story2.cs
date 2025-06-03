using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Story2 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image panda;
    [SerializeField] Image rabbit;
    [SerializeField] RectTransform chatbubble;
    [SerializeField] TMP_Text chattext;
    [SerializeField] TMP_Text narrativeText;
    [SerializeField] GameObject clickToContinue;
    [SerializeField] GameObject pandaName;
    [SerializeField] GameObject rabbitName;

    [Header("sprite")]
    [SerializeField] Sprite serious_panda;
    [SerializeField] Sprite thinking_panda;
    [SerializeField] Sprite sad_rabbit;

    int currentSequence;
    bool isNextSequenceAllowed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panda.sprite = serious_panda;
        rabbit.sprite = sad_rabbit;
        currentSequence = 0;
        panda.gameObject.SetActive(false);
        rabbit.gameObject.SetActive(false);
        chatbubble.gameObject.SetActive(false);
        chattext.gameObject.SetActive(false);

        AlphaFadeManager.Instance.FadeIn(1.0f);
        AudioManager.Instance.PlayMusicWithFade("Bubble Bounce", 1.0f);
        DOTween.Sequence()
            .AppendInterval(1.0f)
            .AppendCallback(() => panda.gameObject.SetActive(true))
            .Append(panda.DOFade(1.0f, 1.0f))
            .AppendInterval(1.0f)
            .AppendCallback(() => pandaName.gameObject.SetActive(true))
            .AppendCallback(() => pandaName.GetComponent<TMP_Text>().alpha = 0.0f)
            .AppendCallback(() => pandaName.GetComponent<TMP_Text>().DOFade(1.0f, 1.0f))
            .AppendCallback(() => isNextSequenceAllowed = true);
    }

    private void Update()
    {
        if (isNextSequenceAllowed && clickToContinue.activeSelf == false)
        {
            clickToContinue.SetActive(true);
        }
        else if (!isNextSequenceAllowed && clickToContinue.activeSelf == true)
        {
            clickToContinue.SetActive(false);
        }

        if (Input.anyKeyDown && isNextSequenceAllowed)
        {
            NextSequence();
        }
    }

    void SetNextDialogue(string text, float time = 0, Vector2 bubbleSize = new Vector2())
    {
        if (text == string.Empty)
        {
            chatbubble.GetComponent<Image>().DOFade(0.0f, 0.15f);
            chattext.DOFade(0.0f, 0.15f);
            return;
        }

        chatbubble.gameObject.SetActive(true);
        chattext.gameObject.SetActive(true);
        chatbubble.DOSizeDelta(bubbleSize, 0.15f);
        chatbubble.GetComponent<Image>().DOFade(1, 0.15f);
        chattext.alpha = 1.0f;
        chattext.text = "";
        AudioManager.Instance.PlaySFX("message");

        chattext.GetComponent<TextMeshProUGUI>().DOText(text, time).SetEase(Ease.Linear);
    }

    void SetNextNarrative(string text, float time = 0)
    {
        if (text == string.Empty)
        {
            narrativeText.DOFade(0.0f, 0.15f);
            return;
        }

        narrativeText.gameObject.SetActive(true);
        narrativeText.alpha = 1.0f;
        narrativeText.text = "";
        AudioManager.Instance.PlaySFX("message");

        narrativeText.GetComponent<TextMeshProUGUI>().DOText(text, time).SetEase(Ease.Linear);
    }

    void SetNextSequenceTime(float time)
    {
        DOVirtual.DelayedCall(time, () => { isNextSequenceAllowed = true; });
    }

    void SetSprite(Image target, Sprite sprite)
    {
        DOTween.Sequence()
            .Append(target.DOFade(0, 0.3f))
            .AppendInterval(0.3f)
            .AppendCallback(() => target.sprite = sprite)
            .Append(target.DOFade(1, 0.3f));
    }

    void NextSequence()
    {
        AudioManager.Instance.PlaySFX("cutePush");
        isNextSequenceAllowed = false;
        currentSequence++;
        switch (currentSequence)
        {
            case 1:
                SetNextDialogue("たいへん...", 1.0f, new Vector2(550, 220));
                SetNextSequenceTime(1.1f);
                break;
            case 2:
                SetNextDialogue("もう考えつかれた...", 1.5f, new Vector2(650, 220));
                SetNextSequenceTime(1.6f);
                break;
            case 3:
                SetNextDialogue(string.Empty);
                rabbit.gameObject.SetActive(true);
                rabbit.DOFade(1.0f, 1.0f);
                AudioManager.Instance.PlaySFX("syobon");
                rabbitName.SetActive(true);
                rabbitName.GetComponent<TMP_Text>().alpha = 0.0f;
                rabbitName.GetComponent<TMP_Text>().DOFade(1.0f, 1.0f);
                SetNextSequenceTime(1.2f);
                break;
            case 4:
                SetNextDialogue("しかもさらはさっきよりにしょぼんしている...！！", 3.0f, new Vector2(650, 320));
                SetNextSequenceTime(3.1f);
                break;
            case 5:
                SetNextDialogue("なんとかしてやりたい！", 2.0f, new Vector2(650, 220));
                SetNextSequenceTime(2.1f);
                break;
            case 6:
                SetNextDialogue("...", 1.0f, new Vector2(250, 220));
                SetNextSequenceTime(1.1f);
                break;
            case 7:
                SetNextDialogue(string.Empty);
                SetNextNarrative("めりーはもういっかいこの一年のおもいでをふりかえてみる", 4.0f);
                SetNextSequenceTime(4.1f);
                break;
            case 8:
                SetSprite(panda, thinking_panda);
                SetNextDialogue("ぜったいなにかヒントがあるはずだ！", 2.0f, new Vector2(650, 320));
                SetNextNarrative(string.Empty);
                SetNextSequenceTime(2.1f);
                break;
            case 9:
                NextScene();
                break;
            default:
                break;
        }

        void NextScene()
        {
            AudioManager.Instance.PlaySFX("confirm", 2.0f);
            AlphaFadeManager.Instance.FadeOut(1.0f);

            AudioManager.Instance.StopMusicWithFade();

            DOVirtual.DelayedCall(1.5f, () => {
                SceneManager.LoadScene("Stage2"); // Replace with your scene name
            });
        }
    }
}
