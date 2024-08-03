using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float jumpHeight;
    Rigidbody2D rb;
    public bool isDoubleJump=false;

    public bool isWallSliding=false;
    public bool isDashing=false;
    public bool dashAble=true;
    bool isWallJumping=false;
    float horizontal;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallLayer;

    [SerializeField] float wallJumpDuration;

    [SerializeField] float jumpForce;
    [SerializeField] float dashDuration;
    [SerializeField] float dashForce;

    public bool isWall,isGround;
    TrailRenderer trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        trailRenderer=GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Move();
        WallSlide();
        WallJump();
        Dash();
        isWall=IsWall();
        isGround=IsGround();
    }

    bool IsWall()
    {
    
        return Physics2D.OverlapCircle(wallCheck.position,0.4f,wallLayer);
    }

    bool IsGround()
    {
       if(Physics2D.OverlapCircle(groundCheck.position,0.2f,groundLayer) || Physics2D.OverlapCircle(groundCheck.position,0.2f,wallLayer))
       {
            return true;
       }
       else
       {
        return false;
       }
         
    }

    void WallJump()
    {
        if(isWallSliding)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isWallJumping=true;
            }

            if(isWallJumping)
            {
                
                    rb.velocity=new Vector2(-transform.localScale.x *jumpForce,jumpHeight);
                    isWallSliding=false;
                    Invoke("StopWallJump",wallJumpDuration);
                
            }
        }
    }

    void StopWallJump()
    {
        isWallJumping=false;
    }
    void WallSlide()
    {
        if(isWallJumping)
        {
            isDoubleJump=false;
        }

        if(!IsGround() && IsWall() && horizontal!=0 && rb.velocity.y<=0)
        {
            isWallSliding=true;
            rb.velocity=new Vector2(rb.velocity.x,-3f);
            dashAble=true;
            
        }
        else
        {
            isWallSliding=false;
        }
    }

    void Dash()
    {
        //Nếu dashAble thì có thể nhấn Z để lướt
        if(dashAble)
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                isDashing=true;
                dashAble=false;
            }   
        }

        //Khi đang lướt sẽ invoke method stopdash sau 1 khoảng thời gian
        if(isDashing)
        { 
            rb.velocity= new Vector2(transform.localScale.x*dashForce,0);   
            trailRenderer.enabled=true;
            Invoke("StopDash",dashDuration);
            
        }

        //Khi chạm đất mới có thể lướt lại lần nữa
        if(IsGround())
        {
            dashAble=true;
        }
        
        
    }

    void StopDash()
    {
        isDashing=false;
        trailRenderer.enabled=false;
    }
    void Move()
    {
        if(IsWall() && rb.velocity.y>0 )
        {
            rb.velocity=new Vector2(Mathf.Clamp(rb.velocity.x,-0.001f,0.001f),rb.velocity.y);
        }
        else if(!isWallJumping)
        {
                horizontal = Input.GetAxisRaw("Horizontal");
                rb.velocity=new Vector2(horizontal*speed,rb.velocity.y);
        }
        
        

        if(!isWallSliding)
        {
            if(Input.GetKeyDown(KeyCode.Space) && IsGround() )
                {
                    
                        rb.velocity=new Vector2(rb.velocity.x,jumpHeight);
                        isDoubleJump=false;
                    
                    
                }
            else if(Input.GetKeyDown(KeyCode.Space) && rb.velocity.y !=0 && !isDoubleJump)
                {
                    rb.velocity=new Vector2(rb.velocity.x,jumpHeight/1.2f);
                    isDoubleJump=true;
                }

                
        }
       
           
        

        
        
    }
}
