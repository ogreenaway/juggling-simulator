using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class Snapshot
{
    public Vector3 position;
    public Vector3 rotation;
    public string type;
    public DateTime timestamp;

    public Snapshot(Vector3 _position, Vector3 _rotation) {
        position = _position;
        rotation = _rotation;
        type = "test";
        timestamp = DateTime.Now;
    }
}

[System.Serializable]
public class ReplayConnection
{
    public GameObject real;
    public GameObject mock;
    public bool recordRotation;
    public List<Snapshot> snapshots = new List<Snapshot>();
}

public class Replay : MonoBehaviour
{
    [SerializeField]
    private bool isRecording = false;

    private Sequence mySequence;

    public ReplayConnection[] ReplayConnections;

    // Start is called before the first frame update
    void Start()
    {
        mySequence = DOTween.Sequence();
        GameEvents.current.OnLaunch += OnLaunch;
        GameEvents.current.OnDrop += StopRecording;

    }

    private void OnDestroy()
    {
        GameEvents.current.OnLaunch -= OnLaunch;
        GameEvents.current.OnDrop -= StopRecording;
    }

    private void OnLaunch()
    {
        isRecording = !isRecording;
        if (isRecording)
        {
            foreach (ReplayConnection replayConnection in ReplayConnections)
            {
                replayConnection.snapshots = new List<Snapshot>();
                StartCoroutine(RecordPositionAndRotation(replayConnection.real, replayConnection.snapshots));
                // TODO: clean up coroutine?
            }
        }
        else
        {
            mySequence.Kill();
            mySequence = DOTween.Sequence().SetLoops(-1, LoopType.Restart);
            foreach (ReplayConnection replayConnection in ReplayConnections)
            {
                Play(replayConnection.mock, replayConnection.snapshots);
            }
        }
    }

    public IEnumerator RecordPositionAndRotation(GameObject gameObject, List<Snapshot> snapshots)
    {
        while (isRecording)
        {
            Snapshot snapshot = new Snapshot(gameObject.transform.position, gameObject.transform.rotation.eulerAngles);
            snapshots.Add(snapshot);
            //Debug.Log("Owen2 " + rightControllerSnapshots.ToArray()[rightControllerSnapshots.Count - 1].position + "  " + rightControllerSnapshots.ToArray()[rightControllerSnapshots.Count - 1].rotation);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public void Play(GameObject gameObject, List<Snapshot> snapshots)
    {
        DateTime initialTime = snapshots.ToArray()[0].timestamp;
        //DateTime previousTime = snapshots.ToArray()[0].timestamp;
        gameObject.transform.position = snapshots.ToArray()[0].position;

        foreach (Snapshot snapshot in snapshots)
        {

            //float duration = Convert.ToSingle((snapshot.timestamp - previousTime).TotalSeconds);
            float insertTime = Convert.ToSingle((snapshot.timestamp - initialTime).TotalSeconds);
            //Debug.Log("insertTime: " + insertTime + " duration: " + duration);
            Tween moveTween = gameObject.transform.DOLocalMove(snapshot.position, 0.5f);

            Tween rotateTween = gameObject.transform.DOLocalRotate(snapshot.rotation, 0.5f);

            mySequence.Insert(insertTime, moveTween);
            mySequence.Insert(insertTime, rotateTween);
            //previousTime = snapshot.timestamp;
        }
    }

    private void StopRecording()
    {
        if(isRecording)
        {
            isRecording = false;
            mySequence.Kill();
            mySequence = DOTween.Sequence().SetLoops(-1, LoopType.Restart);
            foreach (ReplayConnection replayConnection in ReplayConnections)
            {
                Play(replayConnection.mock, replayConnection.snapshots);
            }
        }
    }
}
