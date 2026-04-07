using UnityEngine;

public class LavaSube : MonoBehaviour
{
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float aceleracion = 0.5f;
    [SerializeField] private float alturaMaxima;

    private bool activa = false;
    private float velocidadActual;
    private float velocidadInicial;
    private Vector2 posicionInicial;

    void Awake()
    {
        posicionInicial = transform.position;
        velocidadInicial = velocidad;
        velocidadActual = velocidad;
    }

    public void ActivarLava()
    {
        activa = true;
    }

    void Update()
    {
        if (!activa)
            return;

        velocidad += aceleracion * Time.deltaTime;

        float nuevaY = transform.position.y + velocidadActual * Time.deltaTime;

        if (nuevaY >= alturaMaxima)
        {
            nuevaY = alturaMaxima;
        }

        transform.position = new Vector2(transform.position.x, nuevaY);

        if (transform.position.y >= alturaMaxima)
        {
            velocidadActual = 0f;
        }
    }

    public void ReiniciarLava()
    {
        transform.position = posicionInicial;
        velocidadActual = velocidadInicial;
    }
}