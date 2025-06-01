using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EndStory : MonoBehaviour
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
    [SerializeField] Sprite happy_panda;
    [SerializeField] Sprite sad_rabbit;
    [SerializeField] Sprite happy_rabbit;

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
            .AppendCallback(() => rabbit.gameObject.SetActive(true))
            .Append(panda.DOFade(1.0f, 1.0f))
            .Append(rabbit.DOFade(1.0f, 1.0f))
            .AppendInterval(1.0f)
            .AppendCallback(() => pandaName.gameObject.SetActive(true))
            .AppendCallback(() => pandaName.GetComponent<TMP_Text>().alpha = 0.0f)
            .AppendCallback(() => pandaName.GetComponent<TMP_Text>().DOFade(1.0f, 1.0f))
            .AppendCallback(() => rabbitName.gameObject.SetActive(true))
            .AppendCallback(() => rabbitName.GetComponent<TMP_Text>().alpha = 0.0f)
            .AppendCallback(() => rabbitName.GetComponent<TMP_Text>().DOFade(1.0f, 1.0f))
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

        chattext.GetComponent<TextMeshProUGUI>().DOText(text, time).SetEase(Ease.Linear).SetUpdate(true);
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

        narrativeText.GetComponent<TextMeshProUGUI>().DOText(text, time).SetEase(Ease.Linear).SetUpdate(true);
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
                SetNextDialogue("やっと思い出した！", 1.0f, new Vector2(650, 220));
                SetNextSequenceTime(1.1f);
                break;
            case 2:
                SetNextDialogue("今日はなんと....", 1.5f, new Vector2(550, 220));
                SetNextSequenceTime(1.6f);
                break;
            case 3:
                SetNextDialogue("ぼくとさらの一しゅう年記ねん日だったんだ！！", 1.5f, new Vector2(650, 320));
                SetNextSequenceTime(1.6f);
                break;
            case 4:
                SetNextDialogue(string.Empty);
                SetSprite(rabbit, happy_rabbit);
                AudioManager.Instance.PlaySFX("happy");
                SetNextSequenceTime(1.2f);
                break;
            case 5:
                SetSprite(panda, happy_panda);
                SetNextDialogue("やった！\nあったみたいだ！\nしょぼんしなくなっている！", 3.0f, new Vector2(550, 430));
                AudioManager.Instance.PlaySFX("success");
                SetNextSequenceTime(3.1f);
                break;
            case 6:
                SetNextDialogue("さら、いつも本当にありがたう。", 2.0f, new Vector2(650, 320));
                SetNextSequenceTime(2.1f);
                break;
            case 7:
                SetNextDialogue("おかげさまですごくたのしい一年をすごせた。", 1.5f, new Vector2(650, 320));
                SetNextSequenceTime(1.6f);
                break;
            case 8:
                SetNextDialogue("これからも、\nずっとずっとよろしくね。", 1.5f, new Vector2(650, 350));
                SetNextSequenceTime(1.6f);
                break;
            case 9:
                SetNextDialogue("もう記ねん日をわすれることはないよ！", 2.0f, new Vector2(650, 320));
                SetNextNarrative(string.Empty);
                SetNextSequenceTime(2.1f);
                break;
            case 10:
                SetNextDialogue("さらのこと大すき！", 1.5f, new Vector2(650, 320));
                SetNextNarrative(string.Empty);
                SetNextSequenceTime(1.6f);
                break;
            case 11:
                SetNextDialogue(string.Empty);
                SetNextNarrative("さらのこと大すき！", 2.0f);
                SetNextSequenceTime(2.1f);
                break;
            case 12:
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
                SceneManager.LoadScene("Ending"); // Replace with your scene name
            });
        }
    }
}
