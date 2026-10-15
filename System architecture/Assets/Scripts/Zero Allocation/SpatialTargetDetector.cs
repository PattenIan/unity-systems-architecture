using System;
using UnityEngine;

public class SpatialTargetDetector : MonoBehaviour
{
    const int BUFFER_CAPACITY = 32;
    Collider[] rawColliderBuffer = new Collider[BUFFER_CAPACITY];
    SpatialHit[] hitCandidateBuffer = new SpatialHit[BUFFER_CAPACITY];

    public int QueryNearest(Vector3 origin, float radius, int kNearest, LayerMask mask)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(origin, radius, rawColliderBuffer, mask);
        if (hitCount == 0) return 0;

        for (int i = 0; i < hitCount; i++)
        {
            Vector3 targetPos = rawColliderBuffer[i].transform.position;
            Vector3 diff = targetPos - origin;
            float sqrDist = diff.sqrMagnitude;
            hitCandidateBuffer[i] = new SpatialHit(rawColliderBuffer[i], sqrDist);
        }

        InsertionSortOfHitCadidateBuffer(hitCount);

        return Math.Min(kNearest, hitCount);
    }

    private void InsertionSortOfHitCadidateBuffer(int hitCount)
    {
        for (int i = 1; i < hitCount; i++)
        {
            SpatialHit key = hitCandidateBuffer[i];
            int j = i - 1;

            while (j >= 0 && hitCandidateBuffer[j].sqrDistance > key.sqrDistance)
            {
                hitCandidateBuffer[j + 1] = hitCandidateBuffer[j];
                j--;
            }

            hitCandidateBuffer[j + 1] = key;
        }
    }

    public SpatialHit GetResult(int index)
    {
        return hitCandidateBuffer[index];
    }

}
