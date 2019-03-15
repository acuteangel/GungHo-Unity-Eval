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
    private bool canJump = true;
    private bool punching = false;
    private bool dashing = false;
    private bool canDash = true;
    private Coroutine turn;

    // Start is called before the first frame update
    void Start()
    {
        model = transform.GetChild(0);
        animator = model.GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        if (transform.position.y < -20)
            transform.position = Vector3.zero;
        if (grounded)
        {            
            canJump = true;            
        }

        Vector2 move = Vector2.zero;

        if (!jumping && !punching)
            move.x = Input.GetAxis ("Horizontal");
        if (!grounded)
            move.x = move.x * 1.5f;

        if (move.x != 0 && grounded)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);

        if (move.x < 0 && facingRight && !jumping && !punching)
        {
            facingRight = false;
            if (turn != null)
                StopCoroutine(turn);
            turn = StartCoroutine(FaceLeft(model.eulerAngles.y));
        }
        else if (move.x > 0 && !facingRight && !jumping && !punching)
        {
            facingRight = true;
            if (turn != null)
                StopCoroutine(turn);
            turn = StartCoroutine(FaceRight(model.eulerAngles.y));
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
            dashing = true;
            canDash = false;
            StartCoroutine(Dash(facingRight));
        }
        targetVelocity += move * maxSpeed;
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
        turn = null;
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
        turn = null;
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

    IEnumerator Dash(bool facing)
    {
        if (punching)
            yield break;
        int xMod = 1;
        if (!facing)
            xMod = -1;
        velocity.y = Mathf.Max(0, velocity.y);        
        punching = true;      
        animator.SetTrigger("punch");        
        for (double i=0; i<=1; i += Time.deltaTime)
        {
            velocity.y = Mathf.Max(0, velocity.y);
            yield return null;
        }
        for (var i = 0; i < 3; i++)
        {
            velocity.y = Mathf.Max(0, velocity.y);
            targetVelocity = new Vector2(dashSpeed * xMod, 0);
            yield return null;
        }
        canJump = true;
        punching = false;
        int duration = 37;
        float currentSpeed = dashSpeed;
        for (var i = 0; i < duration; i++)
        {
            if (grounded)
                currentSpeed = currentSpeed * (duration-1)/duration;
            targetVelocity = new Vector2(currentSpeed * xMod, 0);
            yield return null;
        }
        while (currentSpeed > 0.1f)
        {
            currentSpeed = currentSpeed * 4 / 5;
            if (grounded)
                currentSpeed = currentSpeed * (duration - 1) / duration;
            targetVelocity = new Vector2(currentSpeed * xMod, 0);
            yield return null;
        }
        dashing = false;
        StartCoroutine(DashCooldown(1f));
    }

    IEnumerator DashCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        while (!grounded)
            yield return null;        
        canDash = true;
    }
}
