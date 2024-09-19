using System.Collections;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private GameObject focalPoint;
    [SerializeField] private GameObject powerupIndicator;
    [SerializeField] private ParticleSystem smokeParticle;

    [Header("Settings")]
    [SerializeField] private float speed = 500f;
    [SerializeField] private float boostForce = 20f;
    [SerializeField] private float boostCooldown = 1f;
    [Tooltip("How hard to hit enemy without powerup")]
    [SerializeField] private float normalStrength = 10;
    [Tooltip("How hard to hit enemy with powerup")]
    [SerializeField] private float powerupStrength = 25;
    [SerializeField] private float powerUpDuration = 5f;

    [Header("Settings")]
    [SerializeField] private AudioClip boostSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip powerupPickupSound;

    private bool hasPowerup;
    private Coroutine powerupRoutine;
    private float lastBoostTime;

    private void Start()
    {
        lastBoostTime = float.MinValue;
    }

    private void Update()
    {
        // add force to player in direction of the focal point (and camera)
        ProcessMovement();

        // do a boost movement when pressing space
        if (Input.GetKeyDown(KeyCode.Space) &&
            CanBoost())
        {
            Boost();
        }
    }

    private bool CanBoost()
    {
        // check if boost is off-cooldown
        return Time.time > lastBoostTime + boostCooldown;
    }

    private void ProcessMovement()
    {
        // get movement input
        float verticalInput = Input.GetAxis("Vertical");

        // move based on input
        var focalPointForward = focalPoint.transform.forward;
        rb.AddForce(speed * Time.deltaTime * verticalInput * focalPointForward);
    }

    private void Boost()
    {
        lastBoostTime = Time.time;

        // propel the player
        rb.AddForce(focalPoint.transform.forward * boostForce, ForceMode.Impulse);

        // play boost particle
        smokeParticle.Play();

        // play boost sound
        AudioManager.singleton.PlaySourceOneShot(playerAudio, boostSound);
    }

    // coroutine to count down powerup duration
    private IEnumerator RemovePowerupAfterTime()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // player collides with powerup
        if (other.gameObject.CompareTag("Powerup"))
        {
            // destroy powerup
            Destroy(other.gameObject);

            // activate powerup state
            hasPowerup = true;
            powerupIndicator.SetActive(true);

            // play powerup pickup sound
            AudioManager.singleton.PlaySourceOneShot(playerAudio, powerupPickupSound);

            // remove powerup state after its duration
            if (powerupRoutine != null)
                StopCoroutine(powerupRoutine);
            powerupRoutine = StartCoroutine(RemovePowerupAfterTime());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // player collides with an enemy
        if (other.gameObject.TryGetComponent(out EnemyX enemy))
        {
            Rigidbody enemyRb = enemy.rb;
            Vector3 awayFromPlayer = enemy.transform.position - transform.position;

            // we hit the enemy with a powerup
            if (hasPowerup)
            {
                // shoot it with powerup strength
                enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            // we hit the enemy normally
            else
            {
                // shoot it with normal strength
                enemyRb.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }

            // play shoot sound
            playerAudio.PlayOneShot(shootSound);
        }
    }
}
