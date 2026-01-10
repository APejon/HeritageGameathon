using UnityEngine;

public class EventCollision : MonoBehaviour
{
    public enum events
    {
        None,
        Track,
        TrackEnd,
        Hazards,
        Plant,
        Event,
        Village,
        Boundary,
        PatchOfGrass,
        QuickSand
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

    public enum trackType
    {
        Camel,
        Falcon,
        Oasis
    }

    [SerializeField] public events _event;
    [SerializeField] public trackType _track;
    [SerializeField] public hazardType _hazard;
    [SerializeField] public plantType _plant;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.gameObject.name);
    }
}
