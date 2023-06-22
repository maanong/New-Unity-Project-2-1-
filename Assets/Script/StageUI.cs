using TMPro;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textStage;
    [SerializeField]
    private TextMeshProUGUI textCoin;

    public void UpdateTextStage(string stageName)
    {
        textStage.text = stageName;
    }
    public void UpdateCoin(int current)
    {
        textCoin.text = $"Coin : {current}";
    }

}
