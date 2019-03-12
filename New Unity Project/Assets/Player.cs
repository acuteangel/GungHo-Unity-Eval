using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private Transform model;
    private Animator animator;
    private bool facingRight = true;
    private bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        model = transform.GetChild(0);
        animator = model.GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        if (!jumping)
            move.x = Input.GetAxis ("Horizontal");

        if (move.x != 0 && grounded)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        if (move.x < 0 && facingRight)
        {
            facingRight = false;
            StopAllCoroutines();
            StartCoroutine(FaceLeft(model.eulerAngles.y));
        }
        else if (move.x > 0 && !facingRight)
        {
            facingRight = true;
            StopAllCoroutines();
            StartCoroutine(FaceRight(model.eulerAngles.y));
        }

        if (Input.GetButtonDown("Jump") && grounded)
        {
            StartCoroutine(Jump());
            animator.SetBool("jumping", true);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
                velocity.y = velocity.y * .5f;
        } else if (grounded && animator.GetBool("jumping")){
            animator.SetBool("jumping", false);
        }

        targetVelocity = move * maxSpeed;
    }


    IEnumerator FaceLeft(float yAngle)
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

    IEnumerator FaceRight(float yAngle)
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

    IEnumerator Jump()
    {
        jumping = true;
        yield return new WaitForSeconds(.2f);
        jumping = false;
        velocity.y = jumpTakeOffSpeed;
        yield return null;
    }
}
