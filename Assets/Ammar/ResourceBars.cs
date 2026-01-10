using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ResourceBars : MonoBehaviour
{
    private float _health;
    private float _hunger;
    private float _hydration;
    private float _maxStat;
    [SerializeField] GameObject[] _bars;
    private Image[][] _mainBar;
    private Image[][] _subBar;

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
        _maxStat = 100;
        _mainBar = new Image[3][];
        _subBar = new Image[3][];
        for (int i = 0; i <= 2; i++)
        {
            int j = 0;
            foreach (Transform child in _bars[i].GetComponentsInChildren<Transform>())
            {
                j = 0;
                if (child.CompareTag("MainBar"))
                {
                    _mainBar[i][j] = child.GetComponent<Image>();
                    j++;
                }
            }
            foreach (Transform child in _bars[i].GetComponentsInChildren<Transform>())
            {
                j = 0;
                if (child.CompareTag("SubBar"))
                {
                    _subBar[i][j] = child.GetComponent<Image>();
                    j++;
                }
            }
        }
        for (int j = 0; j < _mainBar.Length; j++)
        {
            for (int i = 0; i < _mainBar[j].Length; i++)
                Debug.Log(_mainBar[j][i]);
        }
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
                // setBar(_bars[(int)stat.Health], _health);
                break;
            case stat.Hunger:
                _hunger += amount;
                // setBar(_bars[(int)stat.Hunger], _hunger);
                break;
            case stat.Hydration:
                _hydration += amount;
                // setBar(_bars[(int)stat.Hydration], _hydration);
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
                // setBar(_bars[(int)stat.Health], _health);
                break;
            case stat.Hunger:
                _hunger -= amount;
                // setBar(_bars[(int)stat.Hunger], _hunger);
                break;
            case stat.Hydration:
                _hydration -= amount;
                // setBar(_bars[(int)stat.Hydration], _hydration);
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
