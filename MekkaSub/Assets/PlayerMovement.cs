using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    //public float jumpForce;
    private float moveInput;

    public Rigidbody2D rb;

    private bool facingRight = true;
    bool isMoving;

    bool expBuffer;

    public bool canWalk;
    public float deccelaration;
    public float acceleration;
    public float velPower;

    public Transform groundCheck;
    public Vector2 groundCheckSize;
    public LayerMask whatIsGround;
    public float LastOnGroundTime { get; private set; }

    public float frictionAmount;

    

    // Start is called before the first frame update
    void Start()
    {
        expBuffer = false;
        rb = GetComponent<Rigidbody2D>();


        
    }

    private void Update()
    {
        if (Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, whatIsGround)) //checks if set box overlaps with ground
            LastOnGroundTime = 0.1f;

        LastOnGroundTime -= Time.deltaTime;


        if(moveInput < 0.1f && moveInput > -0.1f)
        {
            
            isMoving = false;
        }
        else isMoving = true;

        //Debug.Log(isMoving);
    }


    void FixedUpdate()
    {
        #region Run
        if (canWalk)
        {
            moveInput = Input.GetAxis("Horizontal");

            float targetSpeed = moveInput * speed;
            float speedDiff = targetSpeed - moveInput;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deccelaration;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

            rb.AddForce(movement * Vector2.right);
        }

        //if (!expBuffer) rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        #endregion

        #region Friction

        if (LastOnGroundTime > 0 && !isMoving)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            //Debug.Log("Yes");
        }
        else return;
        
        #endregion

        #region Flip
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
        #endregion
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= 1;
        transform.localScale = scaler;
    }

    public void ExplosionBufferEnabler()
    {
        /*
        expBuffer = true;
        StartCoroutine(ExpBufferDisable());
        */
    }
    IEnumerator ExpBufferDisable()
    {
        yield return new WaitForSeconds(0f);
        expBuffer = false;
        yield return null;
    }

    

}
