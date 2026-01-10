using UnityEngine;

public class EventCollision : MonoBehaviour
{
    [SerializeField] public events _event;
    [SerializeField] public trackType _track;
    [SerializeField] public trackDirection _trackDirection;
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
        Boundary,
        PatchOfGrass
    }

    public enum trackType
    {
        Camel,
        Falcon,
        Oasis,
        Well
    }

    public enum trackDirection
    {
        Up,
        Left,
        Down,
        Right
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
}
