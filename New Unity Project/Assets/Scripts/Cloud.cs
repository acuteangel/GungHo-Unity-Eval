using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed = 0;

    private Vector2 bottomLeft;
    private Vector2 topRight;
    // Start is called before the first frame update
    void Start()
    {
        CloudManager manager = FindObjectOfType<CloudManager>();
        bottomLeft = manager.bottomLeft;
        topRight = manager.topRight;
        if (speed == 0)
            speed = Random.Range(1, 10);
        transform.position = new Vector3(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y), Random.Range(4, 10));
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > topRight.x)
            transform.position = new Vector3(bottomLeft.x, Random.Range(bottomLeft.y, topRight.y), transform.position.z);
        transform.position += new Vector3(Time.deltaTime * speed, 0);

    }
}
