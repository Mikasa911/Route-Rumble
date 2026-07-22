using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] float rockMoveSpeed = 2f;
    float moveAmountHor = 1f;
    float moveAmountVer = 1f;
    public LayerMask BlankSpace;
    Collider2D myCollider;
    bool reachedTarget = false;
    bool moveCheck;
    SpriteRenderer spriteRenderer;
    [SerializeField] Color32 OriginalColor;
    [SerializeField] Color32 usedColor;
    Rigidbody2D myRigidBody;
    PlayerMovement player;
    [SerializeField] Vector2 target;
    Vector3 moveDirection;
    Vector3 myPos;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        player = FindAnyObjectByType<PlayerMovement>();
        myRigidBody = GetComponent<Rigidbody2D>();
        target = myRigidBody.transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!reachedTarget && new Vector2(transform.position.x, transform.position.y) != target)
            myRigidBody.transform.position = Vector2.MoveTowards(myRigidBody.transform.position, target, rockMoveSpeed * Time.deltaTime);
        if (myCollider.isTrigger == true && new Vector2(transform.position.x, transform.position.y) == target)
        {
            reachedTarget = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (myCollider.isTrigger == false)
            {
                myPos = transform.position;
                CalculateMoveDirection();
            }
        }
    }

    void CalculateMoveDirection()
    {
        if (Mathf.Abs(player.transform.position.x - player.GetPlayerOldPosition().x) <= 0.05)
        {
            moveDirection = new Vector2(0, (moveAmountVer) * Mathf.Sign(player.transform.position.y - player.GetPlayerOldPosition().y) * 1);
        }
        else if (Mathf.Abs(player.transform.position.y - player.GetPlayerOldPosition().y) <= 0.05)
        {
            moveDirection = new Vector2((moveAmountHor) * Mathf.Sign(player.transform.position.x - player.GetPlayerOldPosition().x) * 1, 0);
        }
        moveCheck = RockIsObstructed();
        if (!moveCheck)
        {
            target = myPos + moveDirection;
            target.x = Mathf.Round(target.x);
            target.y = Mathf.Round(target.y);
            if (Physics2D.OverlapCircle(target, 0.1f, BlankSpace) && !Physics2D.OverlapCircle(target, 0.1f, LayerMask.GetMask("Rock")))
            {
                myCollider.isTrigger = true;
                spriteRenderer.sortingLayerName = "UsedRock";
                spriteRenderer.color = usedColor;
            }
        }
        else
        {
            player.ReversePlayer();
        }
    }

    public void RetraceRock(Vector2 pos)
    {
        myCollider.isTrigger = false;
        spriteRenderer.sortingLayerName = "Rock";
        spriteRenderer.color = OriginalColor;
        target = pos;
        reachedTarget = false;
    }

    bool RockIsObstructed()
    {
        if (Physics2D.OverlapCircle(myRigidBody.transform.position + moveDirection, 0.2f, LayerMask.GetMask("Rock")))
        {
            Collider2D collider = Physics2D.OverlapPoint(myRigidBody.transform.position + moveDirection, LayerMask.GetMask("Rock"));
            if (collider.isTrigger == false)
            {
                return true;
            }
        }
        if ((Physics2D.OverlapCircle(myRigidBody.transform.position + moveDirection, 0.2f, LayerMask.GetMask("Interactables", "Obstacles", "Key"))))
        {
            return true;
        }
        return false;
    }
}
