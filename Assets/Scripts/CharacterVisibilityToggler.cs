using UnityEngine;

public class CharacterVisibilityToggler : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
    }

    public void ToggleVisibility()
    {
        var colliders = Physics.OverlapBox(transform.position, Vector3.one * 1.85f / 2f);
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
