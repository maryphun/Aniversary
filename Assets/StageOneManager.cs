using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;
using System.Linq;                 // for .ToList()

using UnityEngine.SceneManagement;
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);  // UnityEngine.Random
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}

public class StageOneManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Image panda;
    [SerializeField] TMP_Text tutorial;
    [SerializeField] TMP_Text gametitle;
    [SerializeField] Button startButton;
    [SerializeField] Image image_first;
    [SerializeField] Image image_second;
    [SerializeField] Image MaruBatsu;
    [SerializeField] GameObject blackboard;
    [SerializeField] TMP_Text scoreCnt;
    [SerializeField] TMP_Text questionCnt;
    [SerializeField] GameObject resultPanel;
    [SerializeField] TMP_Text resultScore;

    [Header("panda")]
    [SerializeField] public Sprite panda_normal;
    [SerializeField] public Sprite panda_correct;
    [SerializeField] public Sprite panda_incorrect;
    [SerializeField] public Sprite maru;
    [SerializeField] public Sprite batsu;

    [Header("memories")]
    public Memories[] memory_set;

    public List<Memories> memoriesRandomized;
    Memories currentMemory;
    int questionAsked = 0;
    int score = 0;

    private void Start()
    {
        questionAsked = 0;
        score = 0;
        memoriesRandomized = memory_set.ToList();
        memoriesRandomized.Shuffle();

        AlphaFadeManager.Instance.FadeIn(1.0f);
        AudioManager.Instance.PlayMusicWithFade("Goofy Escape", 1.0f);

        DOTween.Sequence()
            .AppendInterval(1.0f)
            .AppendCallback(() => panda.gameObject.SetActive(true))
            .Append(panda.DOFade(1.0f, 1.0f))
            .AppendInterval(1.0f)
            .AppendCallback(() => tutorial.gameObject.SetActive(true))
            .AppendCallback(() => tutorial.GetComponent<TMP_Text>().alpha = 1.0f)
            .AppendCallback(() => AudioManager.Instance.PlaySFX("message"))
            .Append(tutorial.DOText("今から写真を２まいつづ見せられる。\n新しい方の写真をえらびなさい。\nわかった？？", 2.5f)).SetEase(Ease.Linear).SetUpdate(true)
            .AppendInterval(3.0f)
            .AppendCallback(() => AudioManager.Instance.PlaySFX("cutePush"))
            .AppendCallback(() => startButton.gameObject.SetActive(true));
    }

    public void OnStartGame()
    {
        AudioManager.Instance.PlaySFX("kawaii");
        startButton.gameObject.SetActive(false);
        tutorial.gameObject.SetActive(false);
        gametitle.gameObject.SetActive(true);

        scoreCnt.text = "スコア 0";
        questionCnt.text = "問題 1";

        DOTween.Sequence()
           .AppendInterval(0.75f)
           .AppendCallback(() => panda.sprite = panda_normal)
           .AppendCallback(() => blackboard.gameObject.SetActive(true))
           .AppendCallback(() => MaruBatsu.gameObject.SetActive(false))
           .AppendCallback(() => NewQuestion())
           .Append(blackboard.GetComponent<CanvasGroup>().DOFade(1.0f, 0.75f));
    }

    public void OnSelectMemory_First()
    {
        if (currentMemory.isAnswerFirst)
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

    public void OnSelectMemory_Second()
    {
        if (!currentMemory.isAnswerFirst)
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

        panda.sprite = panda_correct;

        MaruBatsu.gameObject.SetActive(true);
        MaruBatsu.sprite = maru;

        DOTween.Sequence()
           .AppendInterval(0.75f)
           .AppendCallback(() => panda.sprite = panda_normal)
           .AppendCallback(() => MaruBatsu.gameObject.SetActive(false))
           .AppendCallback(() => NewQuestion());
    }

    void Incorrect()
    {
        AudioManager.Instance.PlaySFX("incorrect");

        panda.sprite = panda_incorrect;

        MaruBatsu.gameObject.SetActive(true);
        MaruBatsu.sprite = batsu;

        DOTween.Sequence()
           .AppendInterval(0.75f)
           .AppendCallback(() => panda.sprite = panda_normal)
           .AppendCallback(() => MaruBatsu.gameObject.SetActive(false))
           .AppendCallback(() => NewQuestion());
    }

    public void NewQuestion()
    {
        if (questionAsked == memory_set.Length)
        {
            AudioManager.Instance.PlaySFX("result");
            blackboard.GetComponent<CanvasGroup>().interactable = false;
            blackboard.GetComponent<CanvasGroup>().DOFade(0.0f, 1.0f);

            resultPanel.SetActive(true);
            resultScore.SetText("スコア <size=100>" + score.ToString() + "/" + memory_set.Length.ToString());

            DOTween.Sequence()
           .AppendInterval(2.0f)
           .Append(resultPanel.GetComponent<CanvasGroup>().DOFade(1.0f, 1.0f))
           .AppendInterval(1.0f)
           .AppendCallback(() =>
           {
               if (score > 7)
               {
                   // win
                   AlphaFadeManager.Instance.FadeOut(1.0f);

                   AudioManager.Instance.StopMusicWithFade();

                   DOVirtual.DelayedCall(1.5f, () => {
                       SceneManager.LoadScene("Stage1Result"); // Replace with your scene name
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
        Memories original = memoriesRandomized[questionAsked];
        currentMemory = new Memories();

        // Manually copy fields (Unity doesn't deep-copy by default)
        currentMemory.memory_first = original.memory_first;
        currentMemory.memory_second = original.memory_second;
        currentMemory.isAnswerFirst = original.isAnswerFirst;

        image_first.sprite = currentMemory.memory_first;
        image_second.sprite = currentMemory.memory_second;
        questionAsked++;
        questionCnt.text = "問題 " + questionAsked.ToString();
    }
}
