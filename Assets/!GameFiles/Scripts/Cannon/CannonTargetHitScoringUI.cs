using TMPro;
using UnityEngine;

public class CannonTargetHitScoringUI : MonoBehaviour
{
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        InitTextMeshPro();
    }

    private void OnEnable()
    {
        CannonTargetHitScoring.ScoreInitialized += UpdateScore;
        CannonTargetHitScoring.ScoreIncreased += UpdateScore;
    }

    private void OnDisable()
    {
        CannonTargetHitScoring.ScoreInitialized -= UpdateScore;
        CannonTargetHitScoring.ScoreIncreased -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        _textMeshPro.text = "Score:" + " " + score;
    }

    private void InitTextMeshPro()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        //остальная инициализация
    }
}
