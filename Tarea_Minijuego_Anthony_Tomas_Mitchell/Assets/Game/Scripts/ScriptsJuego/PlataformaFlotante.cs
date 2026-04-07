using UnityEngine;

public class PlataformaFlotante : MonoBehaviour
{
    [SerializeField] private float amplitud = 0.4f;
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float desfase;

    private float posicionInicialY;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicialY = rb.position.y;
        desfase = Random.Range(0f, 10f);
    }

    void FixedUpdate()
    {
        float nuevaY = posicionInicialY + Mathf.Sin(Time.time * velocidad + desfase) * amplitud;
        rb.MovePosition(new Vector2(rb.position.x, nuevaY));
    }
}