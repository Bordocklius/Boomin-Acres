using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LegMovement : MonoBehaviour
{
    [SerializeField] private Transform leftLegTransform;
    [SerializeField] private Transform rightLegTransform;
    [SerializeField] private Transform leftLegVisualTransform;
    [SerializeField] private Transform rightLegVisualTransform;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private int leftPlayerSlotID = 0;

    [SerializeField] private float groundLevel = 0f;
    [SerializeField] private float legMoveSpeed = 5f;
    [SerializeField] private float maxHorizontalDistance = 2f;
    [SerializeField] private float maxLegHeight = 2f;
    [SerializeField] private float landingSnapDistance = 0.02f;
    [SerializeField] private float bodyVerticalFollow = 0.15f;
    [SerializeField] private float bodyVerticalMaxOffset = 0.2f;

    private bool isLeftGrounded = true;
    private bool isRightGrounded = true;
    private float leftLegFloorY;
    private float rightLegFloorY;
    private Vector2 bodyOffsetFromLegCenter;
    private float baseLegCenterY;
    private float bodyBaseY;
    private float bodyFixedZ;
    private PlayerInput leftPlayerInput;
    private PlayerInput rightPlayerInput;

    private void Awake()
    {
        if (leftLegTransform != null)
        {
            leftLegFloorY = leftLegTransform.position.y;
        }

        if (rightLegTransform != null)
        {
            rightLegFloorY = rightLegTransform.position.y;
        }

        if (bodyTransform != null && leftLegTransform != null && rightLegTransform != null)
        {
            Vector2 legCenter = new Vector2(
                (leftLegTransform.position.x + rightLegTransform.position.x) * 0.5f,
                (leftLegTransform.position.y + rightLegTransform.position.y) * 0.5f
            );

            Vector2 bodyXY = new Vector2(bodyTransform.position.x, bodyTransform.position.y);
            bodyOffsetFromLegCenter = bodyXY - legCenter;
            baseLegCenterY = legCenter.y;
            bodyBaseY = bodyTransform.position.y;
            bodyFixedZ = bodyTransform.position.z;
        }
    }

    private void Start()
    {
        leftPlayerInput = GlobdataContainer.GetPlayerIDInput(leftPlayerSlotID);
        rightPlayerInput = GlobdataContainer.GetPlayerIDInput(leftPlayerSlotID + 1);
    }

    private void Update()
    {
        if (leftPlayerInput == null || rightPlayerInput == null)
            return;

        Vector2 leftInput = leftPlayerInput.actions["Move"].ReadValue<Vector2>();
        Vector2 rightInput = rightPlayerInput.actions["Move"].ReadValue<Vector2>();

        UpdateLegState(true, leftInput);
        UpdateLegState(false, rightInput);

        //make sure legs never go below ground level
        EnforceGroundLevel(leftLegTransform);
        EnforceGroundLevel(rightLegTransform);

        UpdateBodyPosition();
        UpdateVisualPositions();
    }

    private void UpdateLegState(bool isLeft, Vector2 input)
    {
        Transform legTransform = isLeft ? leftLegTransform : rightLegTransform;
        bool otherLegGrounded = isLeft ? isRightGrounded : isLeftGrounded;

        bool isGrounded = isLeft ? isLeftGrounded : isRightGrounded;

        if (isGrounded)
        {
            if (input.y > 0.5f && otherLegGrounded)
            {
                isGrounded = false;
            }
        }
        else
        {
            MoveLeg(legTransform, input, ref isGrounded);
        }

        if (isLeft)
            isLeftGrounded = isGrounded;
        else
            isRightGrounded = isGrounded;
    }

    private void MoveLeg(Transform legTransform, Vector2 input, ref bool isGrounded)
    {
        Vector3 legPos = legTransform.position;
        Vector3 otherLegPos = (legTransform == leftLegTransform) ? rightLegTransform.position : leftLegTransform.position;
        float legFloorY = (legTransform == leftLegTransform) ? leftLegFloorY : rightLegFloorY;

        //move horizontally (left/right)
        legPos.x += input.x * legMoveSpeed * Time.deltaTime;

        //keep horizontal distance from other leg within range
        float horizontalDistance = Mathf.Abs(legPos.x - otherLegPos.x);
        if (horizontalDistance > maxHorizontalDistance)
        {
            legPos.x = otherLegPos.x + Mathf.Sign(legPos.x - otherLegPos.x) * maxHorizontalDistance;
        }

        //move vertically (up/down)
        legPos.y += input.y * legMoveSpeed * Time.deltaTime;
        legPos.y = Mathf.Clamp(legPos.y, legFloorY, legFloorY + maxLegHeight);

        legTransform.position = legPos;

        //check if leg should be grounded again
        //only snap back to grounded when leg is near the floor
        //and the player is not trying to move up anymore
        if (input.y <= 0f && legPos.y <= legFloorY + landingSnapDistance)
        {
            isGrounded = true;
            legPos.y = legFloorY;
            legTransform.position = legPos;
        }
    }

    private void UpdateBodyPosition()
    {
        if (bodyTransform != null && leftLegTransform != null && rightLegTransform != null)
        {
            float legCenterX = (leftLegTransform.position.x + rightLegTransform.position.x) * 0.5f;
            float legCenterY = (leftLegTransform.position.y + rightLegTransform.position.y) * 0.5f;
            float legCenterDeltaY = legCenterY - baseLegCenterY;
            float bodyVerticalOffset = Mathf.Clamp(legCenterDeltaY * bodyVerticalFollow, -bodyVerticalMaxOffset, bodyVerticalMaxOffset);

            bodyTransform.position = new Vector3(
                legCenterX + bodyOffsetFromLegCenter.x,
                bodyBaseY + bodyVerticalOffset,
                bodyFixedZ
            );
        }
    }

    private void UpdateVisualPositions()
    {
        if (leftLegVisualTransform != null)
        {
            leftLegVisualTransform.position = leftLegTransform.position;
        }

        if (rightLegVisualTransform != null)
        {
            rightLegVisualTransform.position = rightLegTransform.position;
        }
    }

    private void EnforceGroundLevel(Transform legTransform)
    {
        if (legTransform == null)
        {
            return;
        }

        Vector3 legPos = legTransform.position;
        float legFloorY = (legTransform == leftLegTransform) ? leftLegFloorY : rightLegFloorY;
        if (legPos.y < legFloorY)
        {
            legPos.y = legFloorY;
            legTransform.position = legPos;
        }
    }
}
