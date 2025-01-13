using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class score : MonoBehaviour
{
    [SerializeField] public static int scoreValue;
    public Text text;

    public static void Add()
    {
        scoreValue++;
        Debug.Log(scoreValue);
    }
}