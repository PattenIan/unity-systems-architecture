using UnityEngine;

public class SpatialBenchmarkHarness : MonoBehaviour
{
    [SerializeField] SpatialTargetDetector detector;
    [SerializeField] float searchRadius = 15f;
    [SerializeField] int kNearest = 3;
    [SerializeField] LayerMask targetMask;
    [SerializeField] int dummyCount = 25;

    int validHitCount = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < dummyCount; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * (searchRadius * 1.2f);
            GameObject dummyObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dummyObject.transform.position = spawnPos;
            dummyObject.layer = (int)Mathf.Log(targetMask.value, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        validHitCount = detector.QueryNearest(transform.position, searchRadius, kNearest, targetMask);
    }



    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);

        Gizmos.color = Color.green;
        for (int i = 0; i < validHitCount; i++)
        {
            SpatialHit hit = detector.GetResult(i);
            if (hit.targetCollider != null)
            {
                Gizmos.DrawLine(transform.position, hit.targetCollider.transform.position);
            }
        }
    }
}
