using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.SetParent(transform.parent);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetButton("Jump") || Input.GetButton("Fire1"))
            collision.transform.SetParent(null);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.transform.SetParent(null);
    }
}
