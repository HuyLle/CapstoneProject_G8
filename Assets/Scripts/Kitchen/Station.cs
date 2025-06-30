using UnityEngine;

public class Station : MonoBehaviour
{
    private GameObject currentItem = null; // Vật thể hiện tại trên Station

    public bool IsEmpty => currentItem == null; // Kiểm tra Station trống

    public bool TryPlaceItem(GameObject item)
    {
        if (IsEmpty)
        {
            currentItem = item;
            return true;
        }
        return false;
    }

    public void ClearItem()
    {
        currentItem = null;
    }

    public GameObject GetCurrentItem()
    {
        return currentItem;
    }
}