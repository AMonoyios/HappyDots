using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DotType
{
    BabyBlue,
    CyanGreen,
    DesertTan,
    Green,
    Magenta,
    NavyBlue,
    Red,
    Yellow
}

public class Dot : MonoBehaviour
{
    private Color initialColor;
    new private SpriteRenderer renderer;

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float swipeAngle;
    private const float swipeResist = 0.5f;

    private int column;
    private int prevColumn;
    private int row;
    private int prevRow;
    private int targetX;
    private int targetY;

    private const float moveSpeed = 0.2f;
    private Vector2 movePosition;

    public Board Board { private get; set; }
    private Dot otherDot;

    [SerializeField]
    private DotType dotType;
    public DotType GetDotType { get { return dotType; } }

    public bool IsMatched { get; set; }

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        initialColor = renderer.color;

        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        column = targetX;
        row = targetY;
        prevColumn = column;
        prevRow = row;

        IsMatched = false;
    }

    private void Update()
    {
        FindMatches();

        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > 0.1f)
        {
            movePosition = new(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, movePosition, moveSpeed);
        }
        else
        {
            movePosition = new(targetX, transform.position.y);
            transform.position = movePosition;

            if (Board.Dots[column, row] != this)
            {
                Board.Dots[column, row] = this;
            }
        }

        if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
        {
            movePosition = new(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, movePosition, moveSpeed);
        }
        else
        {
            movePosition = new(transform.position.x, targetY);
            transform.position = movePosition;

            if (Board.Dots[column, row] != this)
            {
                Board.Dots[column, row] = this;
            }
        }

        if (IsMatched)
        {
            renderer.color = new(initialColor.r, initialColor.g, initialColor.b, 0.25f);
        }
    }

    private void OnMouseDown()
    {
        startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        endTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        MoveAttempt();
    }

    public IEnumerator CheckMoveCoroutine()
    {
        yield return new WaitForSeconds(moveSpeed);

        if (otherDot != null)
        {
            if (!IsMatched && !otherDot.IsMatched)
            {
                otherDot.row = row;
                otherDot.column = column;
                row = prevRow;
                column = prevColumn;
            }

            otherDot = null;
        }
    }

    private void MoveAttempt()
    {
        if (Vector2.Distance(endTouchPosition, startTouchPosition) <= swipeResist)
            return;

        swipeAngle = Mathf.Atan2(endTouchPosition.y - startTouchPosition.y, endTouchPosition.x - startTouchPosition.x) * 180.0f / Mathf.PI;

        if (swipeAngle > -45.0f && swipeAngle <= 45.0f && column < Board.GetGridSize.x - 1)
        {
            otherDot = Board.Dots[column + 1, row];
            otherDot.column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45.0f && swipeAngle <= 135.0f && row < Board.GetGridSize.y - 1)
        {
            otherDot = Board.Dots[column, row + 1];
            otherDot.row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135.0f || swipeAngle <= -135.0f) && column > 0)
        {
            otherDot = Board.Dots[column - 1, row];
            otherDot.column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45.0f && swipeAngle >= -135.0f && row > 0)
        {
            otherDot = Board.Dots[column, row - 1];
            otherDot.row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveCoroutine());
    }

    private void FindMatches()
    {
        if (column > 0 && column < Board.GetGridSize.x - 1)
        {
            Dot leftDot = Board.Dots[column - 1, row];
            Dot rightDot = Board.Dots[column + 1, row];

            if (leftDot != null && rightDot != null && dotType == leftDot.GetDotType && dotType == rightDot.GetDotType)
            {
                leftDot.IsMatched = true;
                rightDot.IsMatched = true;
                IsMatched = true;
            }
        }
        if (row > 0 && row < Board.GetGridSize.y - 1)
        {
            Dot upDot = Board.Dots[column, row + 1];
            Dot downDot = Board.Dots[column, row - 1];

            if (upDot != null && downDot != null && dotType == upDot.GetDotType && dotType == downDot.GetDotType)
            {
                upDot.IsMatched = true;
                downDot.IsMatched = true;
                IsMatched = true;
            }
        }
    }
}
