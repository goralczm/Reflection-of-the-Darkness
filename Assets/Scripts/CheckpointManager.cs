using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private List<Checkpoint> checkpoints;
    [SerializeField] private FPSController _player;

    private void Start()
    {
        if (checkpoints.Count == 0)
            return;

        LoadCheckpoint();
    }

    public void ResetCheckpoint()
    {
        PlayerPrefs.SetInt("LastCheckpoint", 0);
    }

    public void LoadCheckpoint()
    {
        int lastCheckpointIndex = PlayerPrefs.GetInt("LastCheckpoint");
        Checkpoint lastCheckpoint = checkpoints[lastCheckpointIndex];
        if (lastCheckpoint.onLoadEvents != null)
            lastCheckpoint.onLoadEvents.Invoke();
        for (int i = 0; i < lastCheckpoint.savedItems.Length; i++)
        {
            Inventory.instance.AddItem(lastCheckpoint.savedItems[i]);
        }
        _player.SetupCharacter(lastCheckpoint.transform.position, lastCheckpoint.targetRot);
    }

    public void SaveCheckpoint(Checkpoint newCheckpoint)
    {
        PlayerPrefs.SetInt("LastCheckpoint", checkpoints.IndexOf(newCheckpoint));
    }
}
