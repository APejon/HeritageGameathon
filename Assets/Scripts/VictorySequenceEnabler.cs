using UnityEngine;
using UnityEngine.Playables;

public class VictorySequenceEnabler : MonoBehaviour
{
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
        }
    }
}
