using UnityEngine;

public class SegmentTrigger : MonoBehaviour
{
    public MapManager mapManager;

    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Segment")) 
        {
            mapManager.TriggerEntered();
        }
    }
}
