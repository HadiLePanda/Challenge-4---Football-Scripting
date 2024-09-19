using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player singleton;

    private void Awake()
    {
        singleton = this;
    }
}
