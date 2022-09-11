using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    [SerializeField] private ObjectMover mover;
    [SerializeField] private float delay;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > delay)
        {
            timer = 0;
            mover.MoveRandom();
        }
    }
}
