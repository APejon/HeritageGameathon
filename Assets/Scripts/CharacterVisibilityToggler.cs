using UnityEngine;

public class CharacterVisibilityToggler : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * 1.75f);
    }

    public void ToggleVisibility()
    {
        var colliders = Physics.OverlapBox(transform.position, Vector3.one * 1.75f / 2f);
        Debug.Log(colliders.Length);
        foreach (var collider in colliders)
        {
            var visibility = collider.gameObject.GetComponent<IVisibility>();
            if (visibility != null)
            {
                visibility.MakeVisible();
            }
        }
    }
}
