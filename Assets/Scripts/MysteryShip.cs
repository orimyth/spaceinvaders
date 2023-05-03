using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    public float speed = 5f;
    public float cycleTime = 30f;
    public int score = 300;
    public System.Action<MysteryShip> killed;

    public Vector3 leftDestination { get; private set; }
    public Vector3 rightDestination { get; private set; }
    public int direction { get; private set; } = -1;
    public bool spawned { get; private set; }

    private void Start()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        Vector3 left = transform.position;
        left.x = leftEdge.x - 1f;
        leftDestination = left;
        Vector3 right = transform.position;
        right.x = rightEdge.x + 1f;
        rightDestination = right;

        transform.position = leftDestination;
        Despawn();
    }

    private void Update()
    {
        if (!spawned) {
            return;
        }
        if (direction == 1) {
            MoveRight();
        } else {
            MoveLeft();
        }
    }

    private void MoveRight()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        if (transform.position.x >= rightDestination.x) {
            Despawn();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
        if (transform.position.x <= leftDestination.x) {
            Despawn();
        }
    }

    private void Spawn()
    {
        direction *= -1;
        if (direction == 1) {
            transform.position = leftDestination;
        } else {
            transform.position = rightDestination;
        }

        spawned = true;
    }

    private void Despawn()
    {
        spawned = false;
        if (direction == 1) {
            transform.position = rightDestination;
        } else {
            transform.position = leftDestination;
        }
        Invoke(nameof(Spawn), cycleTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            Despawn();
            if (killed != null) {
                killed.Invoke(this);
            }
        }
    }

}