using UnityEngine;

public class EnemyX : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;

    [Header("Settings")]
    public float baseSpeed = 5f;

    private float speed;
    private PlayerGoal playerGoal;

    private void Start()
    {
        // get player goal reference
        playerGoal = FindObjectOfType<PlayerGoal>();
    }

    private void Update()
    {
        // update speed based on current wave
        speed = baseSpeed * SpawnManagerX.singleton.GetCurrentWaveSpeedMultiplier();

        // set movement direction towards player goal
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;

        // move towards goal
        MoveTowards(lookDirection);
    }

    private void MoveTowards(Vector3 lookDirection)
    {
        rb.AddForce(speed * Time.deltaTime * lookDirection);
    }

    private void OnCollisionEnter(Collision other)
    {
        // if enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject);
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}
