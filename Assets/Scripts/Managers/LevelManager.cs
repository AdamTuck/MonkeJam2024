using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class LevelManager : MonoBehaviour
{
    [SerializeField] private bool isFinalLevel;
    [SerializeField] public Transform checkpointPos;

    public UnityEvent onLevelStart, onLevelEnd;

    public void StartLevel()
    {
        onLevelStart?.Invoke();
    }

    public void EndLevel ()
    {
        onLevelEnd?.Invoke();

        if (isFinalLevel)
        {
            GameManager.instance.ChangeState(GameManager.GameState.GameEnd, this);
        }
        else
        {
            GameManager.instance.ChangeState(GameManager.GameState.LevelEnd, this);
        }
    }

    public Transform CheckpointPos ()
    {
        return checkpointPos;
    }
}
