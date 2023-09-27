using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
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
        loseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void TriggerWinState()
    {
        // Trigger lose condition
        winUI.SetActive(true);
        Time.timeScale = 0;
    }
}
