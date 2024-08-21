using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{   
    private Rigidbody playerRigidbody;

    Vector3 curPosition = Vector3.zero;
    Vector3 prePosition = Vector3.zero;

    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        collision.transform.SetParent(this.transform);
    }
}

private void OnCollisionExit(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        collision.transform.SetParent(null);
    }
}
}
