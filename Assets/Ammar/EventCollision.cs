using UnityEngine;

public class EventCollision : MonoBehaviour
{
    [SerializeField] public events _event;
    [SerializeField] public trackType _track;
    [SerializeField] public hazardType _hazard;
    [SerializeField] public plantType _plant;

    public enum events
    {
        None,
        Track,
        TrackEnd,
        Hazards,
        Plant,
        Event,
        Village,
        Boundary
    }

    public enum trackType
    {
        Camel,
        Falcon,
        Oasis
    }

    public enum hazardType
    {
        Scorpion,
        Snake,
        Sunspot,
        QuickSand
    }

    public enum plantType
    {
        Poisonous,
        Medicinal
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
    }
}
