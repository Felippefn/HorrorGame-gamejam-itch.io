using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentState;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeState(GameState.WalkEntranceHotel);
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Estado atual: " + newState);
    }
}

public enum GameState
{
    WalkEntranceHotel,
    Reception,
    DialogBalcony,
    HeadingToRoom,
}
