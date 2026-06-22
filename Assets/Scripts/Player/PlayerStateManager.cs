using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        Normal,
        CutScene,
        Recording,
        Droning,
        Interacting,
        WaitingForInput
    }

    public PlayerMovementController movementController;
    private PlayerState currentState;

    void Start()
    {
        SetState(PlayerState.Normal); 
    }

    public void SetState(PlayerState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case PlayerState.Normal:
                movementController.EnableMovement();
                break;

            default:
                movementController.DisableMovement();
                break;
        }
    }

    public PlayerState GetState()
    {
        return currentState;
    }
}
