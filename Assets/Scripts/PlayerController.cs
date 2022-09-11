using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ObjectMover playerMover;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
            playerMover.Move(Vector2Int.up);
        if (Input.GetKey(KeyCode.A))
            playerMover.Move(Vector2Int.left);
        if (Input.GetKey(KeyCode.S))
            playerMover.Move(Vector2Int.down);
        if (Input.GetKey(KeyCode.D))
            playerMover.Move(Vector2Int.right);
    }
}
