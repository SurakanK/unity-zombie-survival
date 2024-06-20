using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] public Camera MainCamera;

    private static Game _instance;
    public static Game Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Game>();
            }

            return _instance;
        }
    }
}