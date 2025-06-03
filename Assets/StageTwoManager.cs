using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System.Linq;                 // for .ToList()
using UnityEngine.SceneManagement;

public class StageTwoManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image rabbit;
    [SerializeField] TMP_Text tutorial;
    [SerializeField] TMP_Text questionText;
    [SerializeField] TMP_Text scoreCnt;
    [SerializeField] TMP_Text questionCnt;
    [SerializeField] Button startButton;
    [SerializeField] Image MaruBatsu;
    [SerializeField] GameObject blackboard;
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text resultScore;

    [Header("questions")]
    public Questions[] question_set;
    public List<Questions> questionRandomized;
    Questions currentQuestion;
    [SerializeField] public Sprite maru;
    [SerializeField] public Sprite batsu;

    [Header("rabbit")]
    [SerializeField] public Sprite rabbit_normal;
    [SerializeField] public Sprite rabbit_correct;
    [SerializeField] public Sprite rabbit_incorrect;

    int questionAsked = 0;
    int score = 0;

    private void Start()
    {
        questionAsked = 0;
        score = 0;
        questionRandomized = question_set.ToList();
        questionRandomized.Shuffle();

        AlphaFadeManager.Instance.FadeIn(1.0f);
        AudioManager.Instance.PlayMusicWithFade("Bubble Bounce", 1.0f);

        DOTween.Sequence()
            .AppendInterval(1.0f)
            .AppendCallback(() => rabbit.gameObject.SetActive(true))
            .Append(rabbit.DOFade(1.0f, 1.0f))
            .AppendInterval(1.0f)
            .AppendCallback(() => tutorial.gameObject.SetActive(true))
            .AppendCallback(() => tutorial.GetComponent<TMP_Text>().alpha = 1.0f)
            .AppendCallback(() => AudioManager.Instance.PlaySFX("message"))
            .Append(tutorial.DOText("この一年間発生したことでしつもんします。\nそれぞれの出来ことはどっちの方が当てはまるか\n当ててください。\nわかった？？", 3.0f)).SetEase(Ease.Linear)
            .AppendInterval(1.0f)
            .AppendCallback(() => AudioManager.Instance.PlaySFX("cutePush"))
            .AppendCallback(() => startButton.gameObject.SetActive(true));
    }

    public void OnStartGame()
    {
        AudioManager.Instance.PlaySFX("kawaii");
        startButton.gameObject.SetActive(false);
        tutorial.gameObject.SetActive(false);

        scoreCnt.text = "スコア 0";
        questionCnt.text = "問題 1";

        DOTween.Sequence()
           .AppendInterval(0.75f)
           .AppendCallback(() => blackboard.gameObject.SetActive(true))
           .AppendCallback(() => MaruBatsu.gameObject.SetActive(false))
           .AppendCallback(() => NewQuestion())
           .Append(blackboard.GetComponent<CanvasGroup>().DOFade(1.0f, 0.75f));
    }

    public void OnClickMary()
    {
        if (!currentQuestion.isSarah)
        {
            // correct
            Correct();
        }
        else
        {
            // incorrect
            Incorrect();
        }
    }

    public void OnClickSarah()
    {
        if (currentQuestion.isSarah)
        {
            // correct
            Correct();
        }
        else
        {
            // incorrect
            Incorrect();
        }
    }

    void Correct()
    {
        score++;
        scoreCnt.text = "スコア " + score.ToString();
        AudioManager.Instance.PlaySFX("correct");

        rabbit.sprite = rabbit_correct;

        MaruBatsu.gameObject.SetActive(true);
        MaruBatsu.sprite = maru;

        DOTween.Sequence()
           .AppendInterval(0.75f)
           .AppendCallback(() => rabbit.sprite = rabbit_normal)
           .AppendCallback(() => MaruBatsu.gameObject.SetActive(false))
           .AppendCallback(() => NewQuestion());
    }

    void Incorrect()
    {
        AudioManager.Instance.PlaySFX("incorrect");

        rabbit.sprite = rabbit_incorrect;

        MaruBatsu.gameObject.SetActive(true);
        MaruBatsu.sprite = batsu;

        DOTween.Sequence()
           .AppendInterval(0.75f)
           .AppendCallback(() => rabbit.sprite = rabbit_normal)
           .AppendCallback(() => MaruBatsu.gameObject.SetActive(false))
           .AppendCallback(() => NewQuestion());
    }

    public void NewQuestion()
    {
        if (questionAsked == question_set.Length)
        {
            AudioManager.Instance.PlaySFX("result");
            blackboard.GetComponent<CanvasGroup>().interactable = false;
            blackboard.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f);

            resultPanel.SetActive(true);
            resultScore.SetText("スコア <size=100>" + score.ToString() + "/" + question_set.Length.ToString());

            DOTween.Sequence()
           .AppendInterval(2.0f)
           .Append(resultPanel.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f))
           .AppendInterval(1.0f)
           .AppendCallback(() =>
           {
               if (score > 5)
               {
                   // win
                   AlphaFadeManager.Instance.FadeOut(1.0f);

                   AudioManager.Instance.StopMusicWithFade();

                   DOVirtual.DelayedCall(1.5f, () => {
                       SceneManager.LoadScene("EndStory"); // Replace with your scene name
                   });
               }
               else
               {
                   // game over
                   AlphaFadeManager.Instance.FadeOut(1.0f);

                   AudioManager.Instance.StopMusicWithFade();

                   DOVirtual.DelayedCall(1.5f, () => {
                       SceneManager.LoadScene("GameOver"); // Replace with your scene name
                   });
               }
           });

            // 結果
            return;
        }

        AudioManager.Instance.PlaySFX("newquestion");

        // Copy the second item
        Questions original = questionRandomized[questionAsked];
        currentQuestion = new Questions();

        // Manually copy fields (Unity doesn't deep-copy by default)
        currentQuestion.Question = original.Question;
        currentQuestion.isSarah = original.isSarah;

        questionText.DOText(currentQuestion.Question, 1.5f).SetEase(Ease.Linear);
        AudioManager.Instance.PlaySFX("message");

        questionAsked++;
        questionCnt.text = "問題 " + questionAsked.ToString();
    }
}
