using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public string playerName = "Player";
    public int hp = 100;

    // 이동 속도
    public float moveSpeed = 5f;

    // 점프 힘
    public float jumpPower = 8f;

    // 낙사 기준 높이
    public float fallLimit = -10f;

    // 시작 위치 (Y = 20)
    public Vector3 startPosition = new Vector3(0, 20, 0);

    // 목숨
    public int lives = 3;

    // 캐릭터 그래픽
    public Transform visual;

    // 공중에 있을 때 캐릭터 크기
    public Vector2 airScale = new Vector2(0.8f, 1.2f);

    private Vector2 moveInput;
    private Rigidbody2D rb;

    // 땅에 있는지 확인
    private bool isGrounded = false;

    // 게임 오버 상태
    private bool isGameOver = false;

    // 캐릭터가 바라보는 방향
    private float facing = 1f;


    void Start()
    {
        // 체력 상태 확인
        if (hp >= 70)
        {
            Debug.Log("건강");
        }
        else if (hp >= 30)
        {
            Debug.Log("주의");
        }
        else
        {
            Debug.Log("위험");
        }

        // Rigidbody2D 가져오기
        rb = GetComponent<Rigidbody2D>();

        // Y = 20 위치에서 시작
        transform.position = startPosition;

        Debug.Log(playerName + " 시작. 체력 = " + hp);
        Debug.Log("목숨 = " + lives);
    }


    // 좌우 이동 입력
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        // 오른쪽을 바라봄
        if (moveInput.x > 0)
        {
            facing = 1f;
        }

        // 왼쪽을 바라봄
        else if (moveInput.x < 0)
        {
            facing = -1f;
        }
    }


    // 점프
    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded && !isGameOver)
        {
            Debug.Log("점프!");

            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x, jumpPower);
        }
    }


    void Update()
    {
        // 게임 오버가 아닐 때만 이동
        if (!isGameOver)
        {
            transform.Translate(
                Vector3.right * moveInput.x
                * moveSpeed * Time.deltaTime);
        }


        // 낙사 판정
        if (transform.position.y < fallLimit)
        {
            // 목숨 1 감소
            lives -= 1;

            // 시작 위치로 이동
            transform.position = startPosition;

            // 속도 초기화
            rb.linearVelocity = Vector2.zero;

            // 공중 상태로 변경
            isGrounded = false;

            Debug.Log("낙사. 남은 목숨 " + lives);


            // 목숨이 0이 되면 게임 오버
            if (lives <= 0)
            {
                isGameOver = true;

                Debug.Log("게임 오버");
            }
        }


        // 캐릭터 크기 및 방향 변경
        if (visual != null)
        {
            // 땅에 있을 때
            if (isGrounded)
            {
                visual.localScale =
                    new Vector3(facing, 1f, 1f);
            }

            // 공중에 있을 때
            else
            {
                visual.localScale =
                    new Vector3(
                        facing * airScale.x,
                        airScale.y,
                        1f);
            }
        }
    }


    // 바닥에 닿았을 때
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            Debug.Log("착지");
        }
    }


    // 바닥에서 떨어졌을 때
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;

            Debug.Log("공중");
        }
    }
}