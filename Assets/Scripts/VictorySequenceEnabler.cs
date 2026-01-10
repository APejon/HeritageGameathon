using System;
using UnityEngine;
using UnityEngine.Playables;

public class VictorySequenceEnabler : MonoBehaviour
{
    [SerializeField] private PlayableDirector victorySequence;
    private Rigidbody rb;
    private Collider col;


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
        if (other.gameObject.CompareTag("Player"))
        {
            victorySequence.gameObject.SetActive(true);
        }
    }
}
