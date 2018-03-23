using UnityEngine;

public class NodeHealth_CollisionDetection : MonoBehaviour
{
    public Node_Health health;

    private void OnCollisionEnter(Collision other)
    {
        health.Collided(other, this.gameObject);
    }
}
