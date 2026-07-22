using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    public Rigidbody2D myRigidBody;
    CalculatePath calculatePath;

    public bool isMovingLeft=false;
    public bool isMovingRight = false;
    public bool isMovingUp = false;
    public bool isMovingDown = false;

    public Vector3 target;
    Vector3 playerOldPosition;
    Vector3 obstacleCheckerPosition;

    [SerializeField] Sprite PlayerLeft;
    [SerializeField] Sprite PlayerFront;
    [SerializeField] Sprite PlayerBack;
    [SerializeField] Sprite PlayerRight;

    SpriteRenderer MySprite;

    public bool playerIsObstructed = false;
    public bool playerCanMove = true;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        target =myRigidBody.transform.position;
        MySprite = GetComponent<SpriteRenderer>();
        calculatePath=FindAnyObjectByType<CalculatePath>();
    }

    void Update()
    {
        PlayerMove();
    }
    void PlayerMove()
    {

        transform.position = Vector3.MoveTowards(myRigidBody.transform.position, target, moveSpeed * Time.deltaTime);
        CheckIfPlayerCanMove();
        if (playerCanMove)
        {
            //playerCanMove = false;
            if (myRigidBody.velocity == new Vector2(0,0))
            {
                playerOldPosition = target;
            }
            if (isMovingUp)
            {
                MoveUp(); 
            }
            else if (isMovingDown)
            {
                MoveDown();
            }
            else if (isMovingLeft)
            {
                MoveLeft();
            }
            else if (isMovingRight)
            {            
                MoveRight();
            }          
        }       
    }
   public void MoveUp()
    {
        MySprite.sprite = PlayerBack;
        target += new Vector3(0, 1f, 0);
        obstacleCheckerPosition = target + new Vector3(0, 1f, 0);
    }
    public void MoveDown()
    {
        MySprite.sprite = PlayerFront;
        target += new Vector3(0, -1f, 0);
        obstacleCheckerPosition = target + new Vector3(0, -1f, 0);
    }
    public void MoveLeft()
    {
        MySprite.sprite = PlayerLeft;
        target += new Vector3(-1f, 0, 0);
        obstacleCheckerPosition = target + new Vector3(-1f, 0, 0);
    }
    public void MoveRight()
    {
        MySprite.sprite = PlayerRight;
        target += new Vector3(1f, 0, 0);
        obstacleCheckerPosition = target + new Vector3(1f, 0, 0);
    }
    void CheckIfPlayerCanMove()
    {
        if (Vector2.Distance(myRigidBody.transform.position, target) < 0.01f)
        {
            playerCanMove = true;
        }
        else
        {
            playerCanMove = false;
        }
    }
    public void ReversePlayer()
    {
        target = playerOldPosition;
        calculatePath.Obtructed();
    }

    public Vector3 GetPlayerOldPosition()
    {
        return playerOldPosition;
    }
}
