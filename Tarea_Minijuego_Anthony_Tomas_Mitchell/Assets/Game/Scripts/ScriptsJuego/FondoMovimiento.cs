using UnityEngine;
public class FondoMovimiento : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;

    private Vector2 offset;
    private Material material;
    private Rigidbody2D jugadorRB;
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        jugadorRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        float movimientoX = jugadorRB.linearVelocity.x;

        offset += new Vector2(movimientoX * 0.1f, 0) * velocidadMovimiento * Time.deltaTime;

        material.mainTextureOffset = offset;
    }
}