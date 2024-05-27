using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);

        float y = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.localPosition = startPosition + new Vector3(0, y, 0);
    }
}