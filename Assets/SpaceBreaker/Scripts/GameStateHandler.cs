using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    [Header("Game State UI")]
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject winUI;

    public static GameStateHandler Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TriggerLoseState()
    {
        // Trigger lose condition
        loseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void TriggerWinState()
    {
        // Trigger win condition
        winUI.SetActive(true);
        Time.timeScale = 0;
    }
}
