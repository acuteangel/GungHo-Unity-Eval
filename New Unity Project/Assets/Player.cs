using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Transform model;
    private Animator animator;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        model = transform.GetChild(0);
        animator = model.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.MovePosition(rb2d.position + movement * Time.deltaTime);

        if (moveHorizontal != 0)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        if (moveHorizontal < 0 && facingRight)
        {
            facingRight = false;
            StopAllCoroutines();
            StartCoroutine(faceLeft(model.eulerAngles.y));
        }
        else if (moveHorizontal > 0 && !facingRight)
        {
            facingRight = true;
            StopAllCoroutines();
            StartCoroutine(faceRight(model.eulerAngles.y));
        }
    }

    IEnumerator faceLeft(float yAngle)
    {
        var steps = Mathf.Floor(Mathf.Abs(-90 - yAngle) / 5);
        for (var i = 0; i < steps; i++)
        {
            model.eulerAngles += new Vector3(0, 5, 0);
            yield return null;
        }
        model.eulerAngles = new Vector3(0, -90, 0);
        yield return null;
    }

    IEnumerator faceRight(float yAngle)
    {
        var steps = Mathf.Floor(Mathf.Abs(90 - yAngle) / 5);
        for (var i = 0; i < steps; i++)
        {
            model.eulerAngles -= new Vector3(0, 5, 0);
            yield return null;
        }
        model.eulerAngles = new Vector3(0, 90, 0);
        yield return null;
    }
}
