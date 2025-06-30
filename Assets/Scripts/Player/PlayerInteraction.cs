using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject heldItem = null;
    private GameObject highlightedObject = null; // Vật thể đang được highlight
    [SerializeField] private Transform holdPoint; // Điểm giữ vật thể
    [SerializeField] private float interactDistance = 1.5f; // Khoảng cách tương tác

    void Update()
    {
        UpdateHighlight();
    }

    public void OnInteract()
    {
        if (heldItem == null)
        {
            TryPickUp();
        }
        else
        {
            TryPlace();
        }
    }

    private void UpdateHighlight()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance);
        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            // Chỉ highlight Ingredient khi không cầm gì
            if (heldItem == null && hit.CompareTag("Ingredient"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = hit.gameObject;
                }
            }
            // Chỉ highlight Station trống khi đang cầm vật thể
            else if (heldItem != null && hit.CompareTag("Station"))
            {
                Station station = hit.GetComponent<Station>();
                if (station != null && station.IsEmpty)
                {
                    float distance = Vector3.Distance(transform.position, hit.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestObject = hit.gameObject;
                    }
                }
            }
        }

        // Cập nhật highlight
        if (closestObject != highlightedObject)
        {
            if (highlightedObject != null)
            {
                var highlight = highlightedObject.GetComponent<Highlightable>();
                if (highlight != null) highlight.Unhighlight();
            }

            highlightedObject = closestObject;
            if (highlightedObject != null)
            {
                var highlight = highlightedObject.GetComponent<Highlightable>();
                if (highlight != null) highlight.Highlight();
            }
        }
    }

    private void TryPickUp()
    {
        if (highlightedObject != null && highlightedObject.CompareTag("Ingredient"))
        {
            heldItem = highlightedObject;
            Transform anchorPoint = heldItem.transform.Find("AnchorPoint");

            if (anchorPoint != null)
            {
                heldItem.transform.SetParent(holdPoint);
                heldItem.transform.localPosition = -anchorPoint.localPosition;
                heldItem.transform.localRotation = Quaternion.identity;
            }
            else
            {
                heldItem.transform.SetParent(holdPoint);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;
            }

            heldItem.GetComponent<Collider>().enabled = false;
            var highlight = heldItem.GetComponent<Highlightable>();
            if (highlight != null) highlight.Unhighlight();
            highlightedObject = null;

            // Xóa vật thể khỏi Station (nếu nó đang ở trên Station)
            Station station = FindStationForItem(heldItem);
            if (station != null) station.ClearItem();

            Debug.Log(gameObject.name + " Picked up: " + heldItem.name);
        }
    }

    private void TryPlace()
    {
        if (highlightedObject != null && highlightedObject.CompareTag("Station"))
        {
            Station station = highlightedObject.GetComponent<Station>();
            if (station != null && station.TryPlaceItem(heldItem))
            {
                Transform placePoint = highlightedObject.transform.Find("PlacePoint");
                heldItem.transform.SetParent(null);

                if (placePoint != null)
                {
                    heldItem.transform.position = placePoint.position;
                    heldItem.transform.rotation = placePoint.rotation;
                }
                else
                {
                    heldItem.transform.position = highlightedObject.transform.position + Vector3.up * 0.5f;
                    heldItem.transform.rotation = Quaternion.identity;
                }

                heldItem.GetComponent<Collider>().enabled = true;
                var highlight = highlightedObject.GetComponent<Highlightable>();
                if (highlight != null) highlight.Unhighlight();
                heldItem = null;
                highlightedObject = null;
                Debug.Log(gameObject.name + " Placed item on: " + station.name);
            }
        }
    }

    private Station FindStationForItem(GameObject item)
    {
        Station[] stations = FindObjectsOfType<Station>();
        foreach (var station in stations)
        {
            if (station.GetCurrentItem() == item)
            {
                return station;
            }
        }
        return null;
    }

    private void OnDestroy()
    {
        if (highlightedObject != null)
        {
            var highlight = highlightedObject.GetComponent<Highlightable>();
            if (highlight != null) highlight.Unhighlight();
        }
    }
}