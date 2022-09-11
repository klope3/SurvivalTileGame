using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool showDebugMessages;
    private Vector2Int position;
    private Transform trans;
    private bool isMoving;
    private static readonly List<Vector2Int> directions = new List<Vector2Int>()
    {
        Vector2Int.left,
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down
    };

    private void Awake()
    {
        trans = transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(new Vector2(position.x, position.y), Vector2.one);
    }

    [Button]
    public void Move(Vector2Int direction)
    {
        if (!directions.Contains(direction))
        {
            ShowDebugMessage("Invalid direction.");
            return;
        }
        if (!CanMove(direction))
        {
            ShowDebugMessage("Movement failed.");
            return;
        }
        position += direction;
        StartCoroutine(CO_MovementAnimation());
    }

    private IEnumerator CO_MovementAnimation()
    {
        Vector3 startPos = trans.position;
        Vector3 targetPos = new Vector2(position.x, position.y);
        float t = 0;
        isMoving = true;
        while (trans.position != targetPos)
        {
            t += Time.deltaTime * moveSpeed;
            trans.position = Vector2.Lerp(startPos, targetPos, t);
            yield return 0;
        }
        isMoving = false;
    }

    private bool CanMove(Vector2Int direction)
    {
        if (isMoving)
            return false;
        return true;
    }

    private void ShowDebugMessage(string message)
    {
        if (showDebugMessages)
            Debug.Log(message);
    }
}
