using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TileMap2D tilemap2D;
    private Movement2D movement2D;

    private float deathLimitY;

    public void Setup(Vector2Int position, int mapSizeY)
    {
        movement2D = GetComponent<Movement2D>();

        transform.position = new Vector3(position.x, position.y, 0);

        deathLimitY = -mapSizeY / 2;
    }

    private void Update()
    {
        if ( transform.position.y <= deathLimitY)
        {
            SceneLoader.LoadScene();
        }

        UpdateMove();
    }

    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");

        movement2D.Moveto(x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Item"))
        {
            tilemap2D.GetCoin(collision.gameObject);
        }
    }
}
