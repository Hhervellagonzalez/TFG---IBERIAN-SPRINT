using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public TMP_Text coinsText;
    public TMP_Text distanciaText;

    void Start()
    {
        UpdateCoinsDisplay();
        UpdateDistanciaDisplay(0);
    }

    void Update()
    {
        if (GameManager.Instance != null)
        {
            UpdateDistanciaDisplay(GameManager.Instance.distanciaRecorrida);
        }
    }

    void UpdateCoinsDisplay()
    {
        if (SessionManager.Instance && SessionManager.Instance.CurrentUser != null)
        {
            coinsText.text = SessionManager.Instance.CurrentUser.monedas.ToString();
        }
    }

    void UpdateDistanciaDisplay(float distancia)
    {
        if (distanciaText != null)
        {
            distanciaText.text = Mathf.FloorToInt(distancia).ToString() + " m";
        }
    }
}