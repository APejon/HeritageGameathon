using System;
using UnityEngine;
using UnityEngine.Playables;

public class VictorySequenceEnabler : MonoBehaviour
{
    public static Action OnVictorySequenceComplete;
    [SerializeField] private PlayableDirector victorySequence;
    private Collider col;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        MessagesConcept.instance.SetText("Village reached! congratulations");
        if (other.gameObject.CompareTag("Player"))
        {
            var findObjectsByType =
                FindObjectsByType<PropVisibility>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var propVisibility in findObjectsByType)
            {
                propVisibility.MakeVisible();
            }

            var tileVisibilities =
                FindObjectsByType<TileVisibility>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            foreach (var tileVisibility in tileVisibilities)
            {
                tileVisibility.MakeVisible();
            }

            victorySequence.gameObject.SetActive(true);
            victorySequence.stopped += VictorySequenceOnstopped;


            DayNightCycle.Instance.MakeDay();
        }
    }

    private void VictorySequenceOnstopped(PlayableDirector director)
    {
        director.stopped -= VictorySequenceOnstopped;
        OnVictorySequenceComplete?.Invoke();
    }
}
