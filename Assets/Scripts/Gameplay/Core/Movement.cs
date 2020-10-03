using System;
using UnityEngine;

[Serializable]
public class Movement
{
    public delegate void CheckForDebuffsHandler(ref float speed);
    public event CheckForDebuffsHandler CheckForDebuffsEvent;

    private readonly MovementInput input = new MovementInput();
    //[SerializeField] private Game
    [SerializeField] private float maxSpeed = 12f;
    private readonly float acceleration = 2f;
    private readonly float deceleration = 1f;
    private float currentSpeed = 0f;

    [SerializeField] private BooleanVariable isMovementAllowed;
    [SerializeField] private PolygonCollider2D playerCollider;


    //moves object in certain direction (denoted as -1 0 1)
    //called in fixedupdate

    public void Move(Transform transform)
    {
        if (!isMovementAllowed.value)
        {
            return;
        }

        transform.position += transform.rotation * Vector3.up * maxSpeed * Time.deltaTime;
    }

    public void InputControlledMove(Transform transform)
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
            if (currentSpeed > 0)
            {
                currentSpeed = Mathf.Clamp(currentSpeed - Mathf.Sign(currentSpeed) * deceleration, 0, maxSpeed);
            }
            else if (currentSpeed < 0)
            {
                currentSpeed = Mathf.Clamp(currentSpeed - Mathf.Sign(currentSpeed) * deceleration, -maxSpeed, 0);
            }
            else
            {
                currentSpeed = 0;
            }
        }

        if (CheckForDebuffsEvent != null)
        {
            CheckForDebuffsEvent(ref currentSpeed);
        }

        Vector3 transformPosition = transform.position + Vector3.right * currentSpeed * Time.deltaTime;
        CheckBorders(ref transformPosition);
        transform.position = transformPosition;
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
