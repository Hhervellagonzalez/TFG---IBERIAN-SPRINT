using UnityEngine;

public class movimientoMapa : MonoBehaviour
{
    public float velocidad = 5.0f;  
    public float incrementoVelocidad = 0.2f;  
    public float maxVelocidad = 15.0f;  
    private bool debeMoverse = true;

    void Update()
    {
        if (debeMoverse && velocidad < maxVelocidad)
        {
            velocidad += incrementoVelocidad * Time.deltaTime;
        }

        if (debeMoverse)
        {
            
            transform.Translate(Vector3.right * velocidad * Time.deltaTime);
        }
    }

    public void DetenerMovimiento()
    {
        debeMoverse = false;  
    }
}
