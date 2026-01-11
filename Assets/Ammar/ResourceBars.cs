using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class ResourceBars : MonoBehaviour
{
    private float _health;
    private float _hunger;
    private float _hydration;
    private float _maxStat;
    private float _lerpSpeed;
    private Boolean _changeBar;
    [SerializeField] GameObject[] _bars;
    private Image[][] _mainBar;
    private Image[][] _subBar;
    private Coroutine _resourceMainBarRoutine;
    private Coroutine _resourceSubBarRoutine;
    private SnakeCombat _snakeCombat;
    private QuickSandInteraction _quicksandCombat;
    public Action death;

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
        _lerpSpeed = 3f;
        _mainBar = new Image[3][];
        _subBar = new Image[3][];
        for (int i = 0; i <= 2; i++)
        {
            int j = 0;
            _mainBar[i] = new Image[10];
            foreach (Transform child in _bars[i].GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("MainBar"))
                {
                    _mainBar[i][j] = child.GetComponent<Image>();
                    j++;
                }
            }
            j = 0;
            _subBar[i] = new Image[10];
            foreach (Transform child in _bars[i].GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("SubBar"))
                {
                    _subBar[i][j] = child.GetComponent<Image>();
                    j++;
                }
            }
        }
        _snakeCombat = GetComponent<SnakeCombat>();
        _quicksandCombat = GetComponent<QuickSandInteraction>();
        _snakeCombat.OnSnakeBite += Damaged;
        _quicksandCombat.OnTakenDamage += Damaged;
    }


    void Update()
    {
        // if (Keyboard.current.digit1Key.wasPressedThisFrame)
        //     decreaseResource(stat.Health, 20);
        // if (Keyboard.current.digit2Key.wasPressedThisFrame)
        //     increaseResource(stat.Health, 40);
        // if (Keyboard.current.digit3Key.wasPressedThisFrame)
        //     decreaseResource(stat.Hunger, 30);
        // if (Keyboard.current.digit4Key.wasPressedThisFrame)
        //     increaseResource(stat.Hunger, 50);
        // if (Keyboard.current.digit5Key.wasPressedThisFrame)
        //     decreaseResource(stat.Hydration, 10);
        // if (Keyboard.current.digit6Key.wasPressedThisFrame)
        //     increaseResource(stat.Hydration, 10);
    }

    public void Hurt()
    {
        decreaseResource(stat.Health, 20);
    }

    public void Damaged()
    {
        decreaseResource(stat.Health, 10);
    }

    public void increaseResource(stat resource, int amount)
    {
        while (amount > 0)
        {
            switch(resource)
            {
                case stat.Health:
                    if (_health < 100)
                        setBar(_mainBar[0][(int)(_health / _maxStat * 10)], _subBar[0][(int)(_health / _maxStat * 10)], 1);
                    _health = Mathf.Clamp(_health + 10, 0, 100);
                    // setBar(_bars[(int)stat.Health], _health);
                    break;
                case stat.Hunger:
                    if (_hunger < 100)
                        setBar(_mainBar[1][(int)(_hunger / _maxStat * 10)], _subBar[1][(int)(_hunger / _maxStat * 10)], 1);
                    _hunger = Mathf.Clamp(_hunger + 10, 0, 100);
                    // setBar(_bars[(int)stat.Hunger], _hunger);
                    break;
                case stat.Hydration:
                    if (_hydration < 100)
                        setBar(_mainBar[2][(int)(_hydration / _maxStat * 10)], _subBar[2][(int)(_hydration / _maxStat * 10)], 1);
                    _hydration = Mathf.Clamp(_hydration + 10, 0, 100);
                    // setBar(_bars[(int)stat.Hydration], _hydration);
                    break;
                default:
                    Debug.Log("Have you added a resource I don't know about?? for increase");
                    break;
            }
            amount -= 10;
        }
    }

    public void decreaseResource(stat resource, int amount)
    {
        while (amount > 0)
        {
            switch(resource)
            {
                case stat.Health:
                    _health = Mathf.Clamp(_health - 10, 0, 100);
                    setBar(_mainBar[0][(int)(_health / _maxStat * 10)], _subBar[0][(int)(_health / _maxStat * 10)], 0);
                    // setBar(_bars[(int)stat.Health], _health);
                    break;
                case stat.Hunger:
                    if (amount > _hunger)
                    {
                        MessagesConcept.instance.SetText("Hunger depleted, health is lost instead");
                        _health = Mathf.Clamp(_health - 10, 0, 100);
                        setBar(_mainBar[0][(int)(_health / _maxStat * 10)], _subBar[0][(int)(_health / _maxStat * 10)], 0);
                    }
                    else
                    {
                        _hunger = Mathf.Clamp(_hunger - 10, 0, 100);
                        setBar(_mainBar[1][(int)(_hunger / _maxStat * 10)], _subBar[1][(int)(_hunger / _maxStat * 10)], 0);
                    }
                    // setBar(_bars[(int)stat.Hunger], _hunger);
                    break;
                case stat.Hydration:
                    if (amount > _hydration)
                    {
                        MessagesConcept.instance.SetText("Hydration depleted, health is lost instead");
                        _health = Mathf.Clamp(_health - 10, 0, 100);
                        setBar(_mainBar[0][(int)(_health / _maxStat * 10)], _subBar[0][(int)(_health / _maxStat * 10)], 0);
                    }
                    else
                    {
                        _hydration = Mathf.Clamp(_hydration - 10, 0, 100);
                        setBar(_mainBar[2][(int)(_hydration / _maxStat * 10)], _subBar[2][(int)(_hydration / _maxStat * 10)], 0);
                    }
                    // setBar(_bars[(int)stat.Hydration], _hydration);
                    break;
                default:
                    Debug.Log("Have you added a resource I don't know about?? for decrease");
                    break;
            }
            amount -= 10;
            if (_health <= 0)
            {
                death?.Invoke();
                MessagesConcept.instance.SetText("You can't go on...");
            }
        }
    }

    private void setBar(Image mainBarToChange, Image subBarToChange, float newAmount)
    {
        if (_resourceMainBarRoutine != null)
            StopCoroutine(_resourceMainBarRoutine);
        // barToChange.fillAmount = newAmount / 100;
        if (newAmount == 0)
        {
            _resourceMainBarRoutine = StartCoroutine(AnimateMainBar(mainBarToChange, subBarToChange, newAmount));
            _resourceSubBarRoutine = StartCoroutine(AnimateSubBar(mainBarToChange, subBarToChange, newAmount));
        }
        if (newAmount == 1)
        {
            _resourceSubBarRoutine = StartCoroutine(AnimateSubBar(mainBarToChange, subBarToChange, newAmount));
            _resourceMainBarRoutine = StartCoroutine(AnimateMainBar(mainBarToChange, subBarToChange, newAmount));
        }
    }
 
    private IEnumerator AnimateMainBar(Image mainBar, Image subBar, float targetFill)
    {
        while (!Mathf.Approximately(mainBar.fillAmount, targetFill))
        {
            mainBar.fillAmount = Mathf.MoveTowards(
                mainBar.fillAmount,
                targetFill,
                _lerpSpeed * Time.deltaTime
            );

            yield return null; // wait one frame
        }

        mainBar.fillAmount = targetFill; // snap cleanly at the end
    }

    private IEnumerator AnimateSubBar(Image mainBar, Image subBar, float targetFill)
    {
        if (_resourceMainBarRoutine != null)
            yield return AnimateMainBar(mainBar, subBar, targetFill);
        yield return new WaitForSeconds(0.5f);
        while (!Mathf.Approximately(subBar.fillAmount, targetFill))
        {
            subBar.fillAmount = Mathf.MoveTowards(
                subBar.fillAmount,
                targetFill,
                _lerpSpeed * Time.deltaTime
            );

            yield return null; // wait one frame
        }

        subBar.fillAmount = targetFill; // snap cleanly at the end
    }
}
