using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offset = Vector3.zero;

    private void Update()
    {
        Player player = Player.singleton;
        if (player)
        {
            // follow the player around
            transform.position = player.transform.position + offset;
        }
    }
}
