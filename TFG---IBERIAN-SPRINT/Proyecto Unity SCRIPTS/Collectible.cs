using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int coinValue = 1;  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.IncrementarContador();
            GameManager.Instance.AddCoins(coinValue);  
            Destroy(gameObject);  
        }
    }
}