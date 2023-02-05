using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;
    public Transform target;
    public int points = 200;
    // Added below 4 lines
    private float teleportInterval = 7f; // interval between each teleport in seconds
    private Vector3 randomGridPos;
    // private float timeToTeleport = 10f;
    // private float timeElapsed = 0f;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();
        StartCoroutine(TeleportGhostsRoutine());
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior) {
            home.Disable();
        }

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }
    }

    private IEnumerator TeleportGhostsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(teleportInterval);
            Vector3 randomPosition = GetRandomPosition();
            SetPosition(randomPosition);
        }
    }

    // private Vector3 GetRandomPosition()
    // {
    //     float x = Random.Range(0f, 20f);
    //     float y = Random.Range(0f, 20f);
    //     return new Vector3(x, y, 0f);
    // }

    private Vector3 GetRandomPosition()
    {
        float gridSize = 20f; // Change this value to the size of your grid
        // float x = Random.Range(0f, 20f);
        // float y = Random.Range(0f, 20f);
        // return new Vector3(x, y, 0f);
        float randomX = UnityEngine.Random.Range(-gridSize / 2f, gridSize / 2f);
        float randomY = UnityEngine.Random.Range(-gridSize / 2f, gridSize / 2f);

        return new Vector3(randomX, randomY, 0f);
    }

    // private void Update()
    // {
    //     timeElapsed += Time.deltaTime;
    //     if (timeElapsed >= timeToTeleport)
    //     {
    //         randomGridPos = GetRandomPosition();
    //         SetPosition(randomGridPos);
    //         timeElapsed = 0f;
    //     }
    // }

    // private Vector3 GetRandomPosition()
    // {
    //     // Code to get a random position on the grid can be added here
    //     return new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
    // }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                FindObjectOfType<GameManager>().GhostEaten(this);
            } else {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }

}
