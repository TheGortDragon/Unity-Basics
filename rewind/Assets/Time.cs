using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time : MonoBehaviour
{
    private bool isRewinding = false;
    private bool isFastRewinding = false;

    public int speed = 1;

    List<PointInTime> pointsInTime;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        pointsInTime = new List<PointInTime>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            StartFastRewind();
        }

        if (Input.GetKeyUp(KeyCode.Backspace)) {
            StopRewind();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            speed++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && speed > 1) {
            speed--;
        }
    }

    private void FixedUpdate()
    {
        /*if (isRewinding)
        {
            Rewind();
        } else
        {
            Record();
        }*/

        if (isFastRewinding) {
            FastRewind(speed);
        } else {
            Record();
        }
    }

    private void Record()
    {
        if (pointsInTime.Count > 500)
        {
            pointsInTime.RemoveAt(pointsInTime.Count - 1);
        }

        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, rb.velocity, rb.angularVelocity));
    }

    private void Rewind()
    {
        if (pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            rb.velocity = pointInTime.velocity;
            rb.angularVelocity = pointInTime.angularVelocity;

            pointsInTime.RemoveAt(0);
        } else
        {
            StopRewind();
        }
    }

    private void FastRewind(int speed) {
        if (pointsInTime.Count > speed - 1) {
            for (int i = 0; i < speed; i++) {
                PointInTime pointInTime = pointsInTime[0];
                transform.position = pointInTime.position;
                transform.rotation = pointInTime.rotation;
                rb.velocity = pointInTime.velocity;
                rb.angularVelocity = pointInTime.angularVelocity;

                pointsInTime.RemoveAt(0);
            }
        
        } else {
            StopRewind();
        }
    }

    private void StartFastRewind() {
        isFastRewinding = true;
        rb.isKinematic = true;
    }

    private void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
    }

    private void StopRewind()
    {
        isRewinding = false;
        isFastRewinding = false;
        rb.isKinematic = false;
    }

}
