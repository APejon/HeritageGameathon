using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ResourceBars : MonoBehaviour
{
    private float _health;
    private float _hunger;
    private float _hydration;
    [SerializeField] Image[] _bars;

    public enum stat
    {
        Health,
        Hunger,
        Hydration
    }

    void Start()
    {
        _health = 100;
        _hunger = 100;
        _hydration = 100;
    }


    void Update()
    {
        
    }

    public void increaseResource(stat resource, int amount)
    {
        switch(resource)
        {
            case stat.Health:
                _health += amount;
                setBar(_bars[(int)stat.Health], _health);
                break;
            case stat.Hunger:
                _hunger += amount;
                setBar(_bars[(int)stat.Hunger], _hunger);
                break;
            case stat.Hydration:
                _hydration += amount;
                setBar(_bars[(int)stat.Hydration], _hydration);
                break;
            default:
                Debug.Log("Have you added a resource I don't know about?? for increase");
                break;
        }
    }

    public void decreaseResource(stat resource, int amount)
    {
        switch(resource)
        {
            case stat.Health:
                _health -= amount;
                setBar(_bars[(int)stat.Health], _health);
                break;
            case stat.Hunger:
                _hunger -= amount;
                setBar(_bars[(int)stat.Hunger], _hunger);
                break;
            case stat.Hydration:
                _hydration -= amount;
                setBar(_bars[(int)stat.Hydration], _hydration);
                break;
            default:
                Debug.Log("Have you added a resource I don't know about?? for decrease");
                break;
        }
    }

    private void setBar(Image barToChange, float newAmount)
    {
        barToChange.fillAmount = newAmount / 100;
    }

}
