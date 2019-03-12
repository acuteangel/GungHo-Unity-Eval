using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : PhysicsObject
{
    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    public float dashSpeed = 140;

    private Transform model;
    private Animator animator;
    private bool facingRight = true;
    private bool jumping = false;
    private bool canJump = false;
    private bool punching = false;
    private bool dashing = false;
    private bool canDash = false;

    // Start is called before the first frame update
    void Start()
    {
        model = transform.GetChild(0);
        animator = model.GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        if (grounded)
        {            
            canJump = true;
            if (!punching && !canDash && !dashing)
                StartCoroutine(DashCooldown(1f));
        }

        Vector2 move = Vector2.zero;

        if (!jumping && !punching)
            move.x = Input.GetAxis ("Horizontal");

        if (move.x != 0 && grounded)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        if (move.x < 0 && facingRight && !jumping && !punching)
        {
            facingRight = false;
            StopAllCoroutines();
            StartCoroutine(FaceLeft(model.eulerAngles.y));
        }
        else if (move.x > 0 && !facingRight && !jumping && !punching)
        {
            facingRight = true;
            StopAllCoroutines();
            StartCoroutine(FaceRight(model.eulerAngles.y));
        }

        if (Input.GetButtonDown("Jump") && canJump && !punching)
        {
            
            StartCoroutine(Jump());
            animator.SetBool("jumping", true);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            //if (velocity.y > 0)
                //velocity.y = velocity.y * .5f;
        } else if (grounded && animator.GetBool("jumping")){
            animator.SetBool("jumping", false);
        }
        if (Input.GetButtonDown("Fire1") && canDash)
        {
            canDash = false;
            dashing = true;
            StartCoroutine("Dash");
        }
        targetVelocity = move * maxSpeed;
    }


    IEnumerator FaceLeft(float yAngle)
    {
        var steps = Mathf.Floor(Mathf.Abs(270-yAngle) / 5);
        for (var i = 0; i < steps; i++)
        {
            model.eulerAngles += new Vector3(0, 5, 0);
            yield return null;
        }
        model.eulerAngles = new Vector3(0, 270, 0);
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
        canJump = false;
        yield return null;
    }

    IEnumerator Dash()
    {
        if (punching)
            yield break;
        velocity.y = 0;
        gravityModifier = 0.1f;
        punching = true;      
        animator.SetTrigger("punch");
        yield return new WaitForSeconds(1f);        
        for (var i = 0; i < 3; i++)
        {
            velocity.y = 0;
            int xMod = 1;
            if (!facingRight)
                xMod = -1;
            targetVelocity = new Vector2(dashSpeed * xMod, 0);
            yield return null;
        }
        canJump = true;
        punching = false;
        gravityModifier = 1f;
        for (var i = 0; i < 37; i++)
        {            
            int xMod = 1;
            if (!facingRight)
                xMod = -1;
            targetVelocity = new Vector2(dashSpeed * xMod, 0);
            yield return null;
        }
        dashing = false;
    }

    IEnumerator DashCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canDash = true;
    }
}
