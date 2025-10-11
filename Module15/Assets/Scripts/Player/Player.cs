using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Rigidbody2D playerRigidbody2D;

    [Header("Speed Setup")]
    public Vector2 friction = new Vector2(.1f, 0);
    public float speed;
    public float speedRun;
    public float forceJump = 15;

    [Header("Animation Setup")]
    public float jumpScaleY = 2f;
    public float jumpScaleX = .5f;
    public float animationDuration = 0.5f;
    public Ease ease = Ease.OutBack;
    public float landingScaleY = .5f;
    public float landingScaleX = 2f;


    [Header("Animation Setup")]
    public string boolRun = "Run";
    public string boolJumpUp = "JumpUp";
    public string boolJumpDown = "JumpDown";
    public string boolJumpLanding = "JumpLanding";
    public Animator animator;

    public Vector3 _jumpPoint;
    private float _currentSpeed;
    private bool _colided = false;
    private bool _isInTheGround = false;

    void Update()
    {
        HandleJump();
        HandleMoviment();
        if (!_isInTheGround)
        {
            jumpState();
        }
        
    }

    private void HandleMoviment()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _currentSpeed = speedRun;
            animator.speed = 2;
        }
        else
        {
            _currentSpeed = speed;
            animator.speed = 1;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerRigidbody2D.velocity = new Vector2(-_currentSpeed, playerRigidbody2D.velocity.y);

            if (gameObject.transform.rotation != new Quaternion(0, 180, 0, 0))
            {
                gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            }

            animator.SetBool(boolRun, true);

            //playerRigidbody2D.MovePosition(playerRigidbody2D.position + velocity*Time.deltaTime);

            //playerRigidbody2D.velocity = new Vector2(Input.GetKey(KeyCode.LeftControl)  ? -speed : -speedRun, playerRigidbody2D.velocity.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playerRigidbody2D.velocity = new Vector2(_currentSpeed, playerRigidbody2D.velocity.y);
            
            if(gameObject.transform.rotation != new Quaternion(0, 0, 0, 0))
            {
                gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            
            animator.SetBool(boolRun, true);

            //playerRigidbody2D.MovePosition(playerRigidbody2D.position - velocity * Time.deltaTime);

            //playerRigidbody2D.velocity = new Vector2(Input.GetKey(KeyCode.LeftControl)  ? speed : speedRun, playerRigidbody2D.velocity.y);
        }
        else
        {
            animator.SetBool(boolRun, false);
        }

        if(playerRigidbody2D.velocity.x > 0)
        {
            playerRigidbody2D.velocity -= friction;
        }
        else if (playerRigidbody2D.velocity.x < 0)
        {
            playerRigidbody2D.velocity += friction;
        }
    }

    private void jumpState()
    {
        if (_jumpPoint != null)
        {
            if(_jumpPoint.y != gameObject.transform.position.y)
            {
                if (_jumpPoint.y < gameObject.transform.position.y)
                {
                    animator.SetBool(boolJumpUp, true);
                    new WaitForEndOfFrame();
                }
                else if (_jumpPoint.y > gameObject.transform.position.y)
                {
                    animator.SetBool(boolJumpUp, false);
                    animator.SetBool(boolJumpDown, true);
                    new WaitForEndOfFrame();
                }
            }
            //Debug.Log("_jumpPoint.y = " + _jumpPoint.y);
            //Debug.Log("gameObject.transform.position.y = " + gameObject.transform.position.y);
            //Debug.Log("_jumpPoint.y < gameObject.transform.position.y = " + (_jumpPoint.y < gameObject.transform.position.y));
            //Debug.Log("_jumpPoint.y > gameObject.transform.position.y = " + (_jumpPoint.y > gameObject.transform.position.y));
        }

        _jumpPoint.y = gameObject.transform.position.y;
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpPoint = gameObject.transform.position;

            playerRigidbody2D.velocity = Vector2.up * forceJump;
            playerRigidbody2D.transform.localScale = Vector2.one;

            DOTween.Kill(playerRigidbody2D.transform);

            HandleScaleJump();

            _isInTheGround = false;
            _colided = false;
        }
    }

    private void HandleScaleJump()
    {
        playerRigidbody2D.transform.DOScaleY(jumpScaleY, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        playerRigidbody2D.transform.DOScaleX(jumpScaleX, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
    }

    private void HandleLanding()
    {
        if (_colided)
        {
            DOTween.Kill(playerRigidbody2D.transform);

            animator.SetBool(boolJumpDown, false);
            animator.SetBool(boolJumpLanding, true);

            HandleScaleLanding();
        }
    }

    private void HandleScaleLanding()
    {
        playerRigidbody2D.transform.DOScaleY(landingScaleY, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);
        playerRigidbody2D.transform.DOScaleX(landingScaleX, animationDuration).SetLoops(2, LoopType.Yoyo).SetEase(ease);

        playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && _isInTheGround) {
            return;
        }
        else if (collision.gameObject.CompareTag("Ground") && _isInTheGround == false)
        {
            _colided = true;
            _isInTheGround = true;

            playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

            HandleLanding();
        }

        Debug.Log("Entrou em contato");
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Contato ativo");
        animator.SetBool(boolJumpLanding, false);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Sem contato");
    }
}
