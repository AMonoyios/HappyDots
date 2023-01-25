using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private float swipeAngle;

    public int column;
    public int row;
    public int targetX;
    public int targetY;

    public Board Board { private get; set; }

    private GameObject otherDot;

    private Vector2 movePosition;

    private void Start()
    {
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        column = targetX;
        row = targetY;
    }

    private void Update()
    {
        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            movePosition = new(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, movePosition, 0.4f);
        }
        else
        {
            movePosition = new(targetX, transform.position.y);
            transform.position = movePosition;

            if (Board.Dots[column, row] != gameObject)
            {
                Board.Dots[column, row] = gameObject;
            }
        }

        if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            movePosition = new(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, movePosition, 0.4f);
        }
        else
        {
            movePosition = new(transform.position.x, targetY);
            transform.position = movePosition;

            if (Board.Dots[column, row] != gameObject)
            {
                Board.Dots[column, row] = gameObject;
            }
        }
    }

    private void OnMouseDown()
    {
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ExecuteMove
        (
            () => Debug.Log("Moved Dot Successfuly"),
            () => Debug.Log("Failed to move Dot")
        );
    }

    private void ExecuteMove(Action onSuccess, Action onError)
    {
        swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180.0f / Mathf.PI;

        if (Move())
            onSuccess?.Invoke();
        else
            onError?.Invoke();
    }

    private bool Move()
    {
        if (swipeAngle > -45.0f && swipeAngle <= 45.0f && column < Board.GetGridSize.x)
        {
            Debug.Log($"Moving {transform.name} to the right...");

            otherDot = Board.Dots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;
            column += 1;

            
        }
        else if (swipeAngle > 45.0f && swipeAngle <= 135.0f && row < Board.GetGridSize.y)
        {
            Debug.Log($"Moving {transform.name} up...");

            otherDot = Board.Dots[column, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;
            row += 1;

            
        }
        else if ((swipeAngle > 135.0f || swipeAngle <= -135.0f) && column > 0)
        {
            Debug.Log($"Moving {transform.name} to the left...");

            otherDot = Board.Dots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;

            targetX = column;
        }
        else if (swipeAngle < -45.0f && swipeAngle >= -135.0f && row > 0)
        {
            Debug.Log($"Moving {transform.name} down...");

            otherDot = Board.Dots[column, row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;

            targetY = row;
        }
        else
        {
            return false;
        }

        return true;
    }
}
