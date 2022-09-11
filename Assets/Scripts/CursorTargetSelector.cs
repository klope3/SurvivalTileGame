using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class CursorTargetSelector : MonoBehaviour
{
    private Transform curMenu;
    private CursorTarget curTarget;

    private void OnEnable()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Move(Vector2Int.up);
        if (Input.GetKeyDown(KeyCode.A))
            Move(Vector2Int.left);
        if (Input.GetKeyDown(KeyCode.S))
            Move(Vector2Int.down);
        if (Input.GetKeyDown(KeyCode.D))
            Move(Vector2Int.right);
    }

    /// <summary>
    /// The default target is used, for example, to guarantee that a target is selected when a menu is opened.
    /// </summary>
    /// <returns></returns>
    private CursorTarget FindDefaultTarget()
    {
        var targets = curMenu.GetComponentsInChildren<CursorTarget>();
        //Try to select the top left target
        var sortedX = targets.OrderBy(target => target.GetComponent<RectTransform>().anchoredPosition.x);
        var sortedY = sortedX.OrderByDescending(target => target.GetComponent<RectTransform>().anchoredPosition.y);
        return sortedY.ToList()[0];
    }

    private void SelectTarget(CursorTarget target)
    {
        curTarget = target;
        transform.position = curTarget.transform.position;
    }

    [Button]
    private void Move(Vector2Int direction)
    {
        var targets = curMenu.GetComponentsInChildren<CursorTarget>();
        Vector2 curTargetPos = curTarget.GetComponent<RectTransform>().anchoredPosition;
        //desperate need of refactoring!!
        if (direction == Vector2Int.right)
        {
            var sortedByXDist = targets.OrderBy(target => target.GetComponent<RectTransform>().anchoredPosition.x - curTargetPos.x);
            var sortedByYDist = sortedByXDist.OrderBy(target => Mathf.Abs(target.GetComponent<RectTransform>().anchoredPosition.y - curTargetPos.y));
            var allOnRight = sortedByYDist.Where(target => target.GetComponent<RectTransform>().anchoredPosition.x > curTargetPos.x).ToList();
            if (allOnRight.Count == 0)
            {
                Debug.Log("No more targets on right");
                return;
            }
            SelectTarget(allOnRight[0]);
        }
        if (direction == Vector2Int.left)
        {
            var sortedByXDist = targets.OrderByDescending(target => target.GetComponent<RectTransform>().anchoredPosition.x - curTargetPos.x);
            var sortedByYDist = sortedByXDist.OrderBy(target => Mathf.Abs(target.GetComponent<RectTransform>().anchoredPosition.y - curTargetPos.y));
            var allOnLeft = sortedByYDist.Where(target => target.GetComponent<RectTransform>().anchoredPosition.x < curTargetPos.x).ToList();
            if (allOnLeft.Count == 0)
            {
                Debug.Log("No more targets on left");
                return;
            }
            SelectTarget(allOnLeft[0]);
        }
        if (direction == Vector2Int.up)
        {
            var sortedByYDist = targets.OrderBy(target => target.GetComponent<RectTransform>().anchoredPosition.y - curTargetPos.y);
            var sortedByXDist = sortedByYDist.OrderBy(target => Mathf.Abs(target.GetComponent<RectTransform>().anchoredPosition.x - curTargetPos.x));
            var allAbove = sortedByXDist.Where(target => target.GetComponent<RectTransform>().anchoredPosition.y > curTargetPos.y).ToList();
            if (allAbove.Count == 0)
            {
                Debug.Log("No more targets above");
                return;
            }
            SelectTarget(allAbove[0]);
        }
        if (direction == Vector2Int.down)
        {
            var sortedByYDist = targets.OrderByDescending(target => target.GetComponent<RectTransform>().anchoredPosition.y - curTargetPos.y);
            var sortedByXDist = sortedByYDist.OrderBy(target => Mathf.Abs(target.GetComponent<RectTransform>().anchoredPosition.x - curTargetPos.x));
            var allBelow = sortedByXDist.Where(target => target.GetComponent<RectTransform>().anchoredPosition.y < curTargetPos.y).ToList();
            if (allBelow.Count == 0)
            {
                Debug.Log("No more targets below");
                return;
            }
            SelectTarget(allBelow[0]);
        }
    }

    [Button]
    public void SetMenu(Transform menu)
    {
        curMenu = menu;
        SelectTarget(FindDefaultTarget());
    }
}
