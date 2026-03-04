using UnityEngine;
using UnityEngine.Events;

public class CannonTargetHitScoring : MonoBehaviour
{
    public static UnityAction<int> ScoreInitialized; //везде нужно поправить названия всех полей и методов и избаиться от частных Init
    public static UnityAction<int> ScoreIncreased;

    private int score; //инкапсуляция?

    private void OnEnable()
    {
        CannonTarget.TargetHit += IncreaseScore;
    }

    private void OnDisable()
    {
        CannonTarget.TargetHit -= IncreaseScore;
    }

    private void Start() //нужен Bootstrap
    {
        InitScore();
    }

    private void InitScore()
    {
        //score = 0;

        ScoreInitialized.Invoke(score);
    }

    private void IncreaseScore()
    {
        score += 1;

        ScoreIncreased.Invoke(score);
    }
}
