using UnityEngine;

public class RotateCameraX : MonoBehaviour
{
    [Header("Settings")]
    private float speed = 200;

    public GameObject player;

    private void Update()
    {
        // get input
        float horizontalInput = Input.GetAxis("Horizontal");

        // rotate with input
        transform.Rotate(Vector3.up, horizontalInput * speed * Time.deltaTime);

        // move focal point with player
        transform.position = player.transform.position; 
    }
}
