using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public string playerName = "Player";
    public int hp = 100;
    public float moveSpeed = 5f;
    public float jumpPower = 8f;
    public Vector3 startPosition;
    public Transform visual;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = startPosition;
        Debug.Log(playerName + " 시작. 체력 " + hp);
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        if (moveInput.x > 0)
        { visual.localScale = new Vector3(1, 1, 1); }
        else if (moveInput.x < 0)
        { visual.localScale = new Vector3(-1, 1, 1); }
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("점프!");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }
    }
    void Update()
    {
        transform.Translate(Vector3.right * moveInput.x
        * moveSpeed * Time.deltaTime);
    }
}
