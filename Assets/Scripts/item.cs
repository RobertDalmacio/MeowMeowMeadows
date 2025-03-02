using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class item : MonoBehaviour
{
    public itemData data;

    [HideInInspector] public Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
}
