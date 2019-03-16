using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.enabled = false;
            GetComponent<AudioSource>().Play();
            GameManager.instance.Invoke("NextScene", 1.5f);
        }
    }
}
