using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject heldItem = null;
    [SerializeField] private Transform holdPoint; // Điểm giữ vật thể
    [SerializeField] private float interactDistance = 1.5f; // Khoảng cách tương tác

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

    private void TryPickUp()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Ingredient"))
            {
                heldItem = hit.gameObject;
                Transform anchorPoint = heldItem.transform.Find("AnchorPoint");

                if (anchorPoint != null)
                {
                    // Căn chỉnh theo AnchorPoint
                    heldItem.transform.SetParent(holdPoint);
                    heldItem.transform.localPosition = -anchorPoint.localPosition;
                    heldItem.transform.localRotation = Quaternion.identity; // Hướng cố định
                }
                else
                {
                    // Fallback nếu không có AnchorPoint
                    heldItem.transform.SetParent(holdPoint);
                    heldItem.transform.localPosition = Vector3.zero;
                    heldItem.transform.localRotation = Quaternion.identity;
                }

                heldItem.GetComponent<Collider>().enabled = false; // Tắt collider
                Debug.Log(gameObject.name + " Picked up: " + heldItem.name);
                break;
            }
        }
    }

    private void TryPlace()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactDistance);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Station"))
            {
                Transform placePoint = hit.transform.Find("PlacePoint");
                heldItem.transform.SetParent(null);

                if (placePoint != null)
                {
                    // Đặt vào PlacePoint trên Station
                    heldItem.transform.position = placePoint.position;
                    heldItem.transform.rotation = placePoint.rotation;
                }
                else
                {
                    // Fallback: Đặt trên mặt bàn
                    heldItem.transform.position = hit.transform.position + Vector3.up * 0.5f;
                    heldItem.transform.rotation = Quaternion.identity;
                }

                heldItem.GetComponent<Collider>().enabled = true; // Bật lại collider
                heldItem = null;
                Debug.Log(gameObject.name + " Placed item");
                break;
            }
        }
    }
}