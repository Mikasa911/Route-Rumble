using UnityEngine;

public class Key : MonoBehaviour
{
    TreasureChest treasureChest;
    private void Start()
    {
        treasureChest = FindAnyObjectByType<TreasureChest>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            treasureChest.ObtainedKey();
            Destroy(gameObject);
        }
    }
}
