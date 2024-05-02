using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform RestartPoint;
    private Vector3 RestartPos;

    private bool Activated;
    void Start()
    {
        Activated = false;
        RestartPos = RestartPoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !Activated)
        {
            FullScreenVFXController.instance.SetCheckpointEffect();
            AudioManager.PlayMiscClip("Checkpoint", transform.position);

            Activated = true;

            other.GetComponent<PlayerInfo>().SetCheckpointPos(RestartPos);
            //Update the currentCheckpoint location to this instances
        }
    }
}
