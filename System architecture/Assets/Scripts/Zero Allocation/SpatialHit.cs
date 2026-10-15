using System;
using UnityEngine;

public struct SpatialHit : IComparable<SpatialHit>
{
    public Collider targetCollider;
    public float sqrDistance;

    public SpatialHit(Collider collider, float sqrDistance)
    {
        targetCollider = collider;
        this.sqrDistance = sqrDistance;
    }

    public int CompareTo(SpatialHit other)
    {
        if (sqrDistance < other.sqrDistance) return -1;
        if (sqrDistance > other.sqrDistance) return 1;
        return 0;
    }

}
