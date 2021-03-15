using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed;
    public Rigidbody2D PlayerRigidbody => playerRigidbody;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    PlayerAttributes playerMovement;
    Rigidbody2D playerRigidbody;
    Vector2 movement;
    Animator playerAnimator;
    Vector3 cursorPos;

    void OnEnable()
    {
        Managers.MapGenerator.OnMapGenerated += MovePlayerToSpawnPoint;
    }

    void OnDisable()
    {
        Managers.MapGenerator.OnMapGenerated -= MovePlayerToSpawnPoint;
    }

    void Start()
    {
        playerMovement = GetComponent<PlayerAttributes>();
        moveSpeed = playerMovement.MovementSpeed;
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        //Get input from axis
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Get direction from player to cursor position
        cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 cursorDir = cursorPos - playerRigidbody.transform.position;

        //Calculate dot products from player up/right/movement and cursor position directions to let animator know which animations to play
        float playerUpToCursorDot = Vector2.Dot(playerRigidbody.transform.up, cursorDir);
        float playerRightToCursorDot = Vector2.Dot(playerRigidbody.transform.right, cursorDir);
        float movementToCursorDot = Vector2.Dot(movement, cursorDir);

        //Set variable values for animator
        playerAnimator.SetFloat("UpDot", playerUpToCursorDot);
        playerAnimator.SetFloat("RightDot", playerRightToCursorDot);
        playerAnimator.SetFloat("MovementDot", movementToCursorDot);
        playerAnimator.SetFloat("Speed", movement.sqrMagnitude);

        //Normalize movement vector
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        //Move players rigidbody by desired amount
        playerRigidbody.MovePosition(playerRigidbody.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }

    //Move player to new spawn position generater by map generator
    void MovePlayerToSpawnPoint()
    {
        transform.position = Managers.MapGenerator.PlayerSpawnPosition;
    }
}
