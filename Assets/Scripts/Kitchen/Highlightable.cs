using UnityEngine;

public class Highlightable : MonoBehaviour
{
    private Renderer renderer;
    private Material originalMaterial;
    [SerializeField] private Material highlightMaterial; // Material có viền sáng

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>(); // Tìm Renderer ở con nếu cần
        }
        originalMaterial = renderer.material; // Lưu material gốc
    }

    public void Highlight()
    {
        if (renderer != null && highlightMaterial != null)
        {
            renderer.material = highlightMaterial; // Đổi sang material highlight
        }
    }

    public void Unhighlight()
    {
        if (renderer != null)
        {
            renderer.material = originalMaterial; // Khôi phục material gốc
        }
    }
}