using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> segmentPrefabs; 
    [SerializeField] private Transform player; 
    [SerializeField] private float segmentLength = 100f; 
    [SerializeField] private int initialSegments = 5; 
    [SerializeField] private float speed = 10f;

    private Queue<GameObject> segments = new Queue<GameObject>(); 
    private float lastSegmentZPosition; 

    private void Start()
    {
        
        lastSegmentZPosition = player.position.z - segmentLength * (initialSegments - 1);
        for (int i = 0; i < initialSegments; i++)
        {
            AddSegment();
        }
    }

    private void Update()
    {
        
        foreach (GameObject segment in segments)
        {
            segment.transform.position += Vector3.back * speed * Time.deltaTime;
        }

        if (segments.Count > 0 && player.position.z - segments.Peek().transform.position.z > segmentLength)
        {
            RemoveSegment();
            AddSegment();
        }
    }

    private void AddSegment()
    {
        int randomIndex = Random.Range(0, segmentPrefabs.Count);
        GameObject segmentPrefab = segmentPrefabs[randomIndex];


        Vector3 spawnPosition = new Vector3(0, 0, lastSegmentZPosition);

        GameObject newSegment = Instantiate(segmentPrefab, spawnPosition, Quaternion.identity);
        segments.Enqueue(newSegment); 

      
        lastSegmentZPosition += segmentLength;
    }

    private void RemoveSegment()
    {
        
        if (segments.Count > 0)
        {
            GameObject oldSegment = segments.Dequeue();
            Destroy(oldSegment); 
        }
    }

    public void TriggerEntered()
    {
        RemoveSegment();
        AddSegment();
    }
}
