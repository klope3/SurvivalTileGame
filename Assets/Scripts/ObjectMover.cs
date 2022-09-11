using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ObjectMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform visualsParent;
    [SerializeField] private LayerMask obstacleLayers;
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
        position = new Vector2Int((int)trans.position.x, (int)trans.position.y);
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
        trans.position = new Vector2(position.x, position.y);
        visualsParent.localPosition = -1 * new Vector3(direction.x, direction.y);
        StartCoroutine(CO_MovementAnimation());
    }

    private IEnumerator CO_MovementAnimation()
    {
        Vector3 startPos = visualsParent.localPosition;
        Vector3 targetPos = Vector2.zero;
        float t = 0;
        isMoving = true;
        while (visualsParent.localPosition != targetPos)
        {
            t += Time.deltaTime * moveSpeed;
            visualsParent.localPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return 0;
        }
        isMoving = false;
    }

    private bool CanMove(Vector2Int direction)
    {
        if (isMoving)
            return false;
        if (!IsNextCellFree(position + direction))
            return false;
        return true;
    }

    private bool IsNextCellFree(Vector2Int nextCoords)
    {
        Vector2 pos = new Vector2(nextCoords.x, nextCoords.y);
        RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector2.zero, float.MaxValue, obstacleLayers);
        return rayHit.collider == null;
    }

    public void MoveRandom()
    {
        int randIndex = Random.Range(0, directions.Count);
        Vector2Int randDirection = directions[randIndex];
        Move(randDirection);
    }

    private void ShowDebugMessage(string message)
    {
        if (showDebugMessages)
            Debug.Log(message);
    }
}
