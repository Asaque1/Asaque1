using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class All_Move : MonoBehaviour
{
    public float speed = 1f;
    private Transform tf;
    private Vector2 CurVec;
    private Vector2 NextVec;
    public bool Move = true;
    void Start()
    {
    }

    void Update()
    {
        CurVec = transform.position;
        NextVec = new Vector2(1 * speed, 0) * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if (Move)
        {
            transform.position = CurVec + NextVec;
        }
    }
}
