using UnityEngine;
using System;

[Serializable]
public class Movement
{

    private MovementInput input = new MovementInput();
    //[SerializeField] private Game
    private float maxSpeed      = 14f;
    private float acceleration  = 1.4f;
    private float deceleration  = 0.7f;
    private float currentSpeed  = 0f;

    [SerializeField] private BooleanVariable isMovementAllowed;

    [SerializeField] private PolygonCollider2D playerCollider;


    //moves object in certain direction (denoted as -1 0 1)
    //called in fixedupdate
    public void Move(ref Vector3 transformPosition)
    {
        if (!isMovementAllowed.value)
        {
            return;
        }
        int direction = input.UpdateDirection();
        //direction < 0, move left
        if (direction < 0)
        {
            if (!Mathf.Approximately(currentSpeed, -maxSpeed))
            {
                currentSpeed = Mathf.Clamp(currentSpeed - acceleration, -maxSpeed, maxSpeed);
            }
        }
        //direction > 0, move right
        else if (direction > 0)
        {
            if (!Mathf.Approximately(currentSpeed, maxSpeed))
            {
                currentSpeed = Mathf.Clamp(currentSpeed + acceleration, -maxSpeed, maxSpeed);
            }
        }
        else
        {
            //if it's moving without input, stop slowly
            if (!Mathf.Approximately(currentSpeed, 0f))
            {
                currentSpeed = Mathf.Clamp(currentSpeed - Mathf.Sign(currentSpeed)*deceleration, -maxSpeed, maxSpeed);
            }
            //if tis almost still, stop.
            else
            {
                currentSpeed = 0;
            }
        }

        transformPosition += Vector3.right * currentSpeed * Time.deltaTime;
        CheckBorders(ref transformPosition);
    }


    void CheckBorders(ref Vector3 pos)
    {
        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect;

        Bounds playerBounds = playerCollider.bounds;
        float borderHorizontal = (screenWidth - (playerBounds.extents.x));
        if (pos.x > borderHorizontal)
        {
            pos = new Vector2(borderHorizontal, pos.y);
        }
        else if (pos.x < -borderHorizontal)
        {
            pos = new Vector2(-borderHorizontal, pos.y);
        }
    }

}
