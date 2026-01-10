using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PropVisibility : MonoBehaviour, IVisibility
{
    private List<MeshRenderer> meshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>().ToList();
    }

    private void Start()
    {
        if (IsVisibleOnStart)
        {
            MakeVisible();
        }
        else
        {
            MakeInvisible();
        }
    }

    [field: SerializeField] public bool IsVisibleOnStart { get; private set; }

    public void MakeVisible()
    {
        meshRenderers.ForEach(meshRenderer => meshRenderer.enabled = true);
    }

    public void MakeInvisible()
    {
        meshRenderers.ForEach(meshRenderer => meshRenderer.enabled = false);
    }
}
