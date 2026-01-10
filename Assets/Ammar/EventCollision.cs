using UnityEngine;

public class EventCollision : MonoBehaviour
{
    [SerializeField] events _event;
    [SerializeField] trackType _track;
    [SerializeField] hazardType _hazard;
    [SerializeField] plantType _plant;
    
    public enum events
    {
        Track,
        TrackEnd,
        Hazards,
        Plant,
        Event,
        Village
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
