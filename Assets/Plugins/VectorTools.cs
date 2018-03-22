using UnityEngine;

public static class VectorTools
{
    /// Returns a world-space point a set distance from A in the direction of B.
    public static Vector3 PointInDirection(Vector3 pointA, Vector3 pointB, float distance)
    {
        Vector3 direction = pointB - pointA;
        Vector3 distanceInDirection = direction.normalized * distance;
        return pointA + distanceInDirection;
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }
}
