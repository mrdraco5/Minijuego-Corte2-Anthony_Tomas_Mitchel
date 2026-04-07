using UnityEngine;

public class FrutaFlotante : MonoBehaviour
{
    public float velocidad = 2f;
    public float altura = 0.25f;
    float yInicial;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        yInicial = rb.position.y;
    }

    void FixedUpdate()
    {
        float nuevaY = yInicial + Mathf.Sin(Time.time * velocidad) * altura;
        rb.MovePosition(new Vector2(rb.position.x, nuevaY));
    }
}