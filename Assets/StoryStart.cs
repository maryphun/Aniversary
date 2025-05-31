using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StoryStart : MonoBehaviour
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
    [SerializeField] Sprite sad_panda;
    [SerializeField] Sprite thinking_panda;

    int currentSequence;
    bool isNextSequenceAllowed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panda.sprite = sad_panda;
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
                SetNextDialogue("たいへんだ！", 1.0f, new Vector2(650, 220));
                SetNextSequenceTime(1.1f);
                break;
            case 2:
                SetNextDialogue("サラがしょぼんしている！！", 2.0f, new Vector2(650, 320));
                SetNextSequenceTime(2.1f);
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
                SetNextDialogue("こまった...", 1.0f, new Vector2(650, 220));
                SetNextSequenceTime(1.1f);
                break;
            case 5:
                SetNextDialogue("なんとかしないと...！", 1.5f, new Vector2(650, 320));
                SetNextSequenceTime(1.6f);
                break;
            case 6:
                SetNextDialogue("どうしよう...", 1.0f, new Vector2(650, 220));
                SetNextSequenceTime(1.1f);
                break;
            case 7:
                SetNextDialogue(string.Empty);
                SetNextNarrative("そしてメリーは二人の思い出から\nサラがしょぼんになっている原いん\nのヒントをさがすのだった。", 4.0f);
                SetNextSequenceTime(4.1f);
                break;
            case 8:
                SetSprite(panda, thinking_panda);
                SetNextDialogue("思い出せ...!\nなにがあったのか...", 1.5f, new Vector2(650, 320));
                SetNextNarrative(string.Empty);
                SetNextSequenceTime(3.1f);
                break;
            case 9:
                SetNextDialogue(string.Empty);
                SetNextNarrative("はやくしないと、さらがずっとしょぼんになる！！！\nそんなことはダメに決まってる！", 4.0f);
                SetNextSequenceTime(4.1f);
                break;
            case 10:
                SetNextDialogue(string.Empty);
                SetNextNarrative("がんばれ！\nメリー！！！！！", 2.0f);
                SetNextSequenceTime(2.1f);
                break;
            case 11:
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
                SceneManager.LoadScene("Stage1"); // Replace with your scene name
            });
        }
    }
}
