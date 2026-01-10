using UnityEngine;

public class TileVisibility : MonoBehaviour, IVisibility
{
    public Material visibleMaterial;
    public Material invisibleMaterial;
    private MeshRenderer meshRenderer;


    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
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
        meshRenderer.material = visibleMaterial;
    }


    public void MakeInvisible()
    {
        meshRenderer.material = invisibleMaterial;
    }
}
