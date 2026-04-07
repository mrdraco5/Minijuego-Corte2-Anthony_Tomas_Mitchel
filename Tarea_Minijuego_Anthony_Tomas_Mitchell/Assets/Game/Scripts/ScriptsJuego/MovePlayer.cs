using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MovePlayer : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float longitudRaycast = 0.15f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Collider2D col;
    private SpriteRenderer sr;
    private Vector2 ultimoPuntoSeguro;
    private int saltosRestantes = 2;
    private bool estabaEnPiso;
    private bool saltoPresionado;
    private bool invulnerable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        ultimoPuntoSeguro = transform.position;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        saltoPresionado = true;

        if (EnPiso())
            saltosRestantes = 2;

        if (saltosRestantes <= 0) return;

        if (saltosRestantes == 1)
            animator.SetTrigger("dobleSalto");

        float fuerza = saltosRestantes == 2 ? jumpForce : jumpForce * 0.8f;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerza);

        saltosRestantes--;
    }

    private bool EnPiso()
    {
        Vector2 origin = new Vector2(col.bounds.center.x, col.bounds.min.y + 0.02f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, longitudRaycast, capaSuelo);
        return hit.collider != null;
    }

    private bool EsPuntoSeguro()
    {
        float ancho = col.bounds.extents.x * 0.9f;

        Vector2 centro = new Vector2(col.bounds.center.x, col.bounds.min.y);
        Vector2 izquierda = centro + Vector2.left * ancho;
        Vector2 derecha = centro + Vector2.right * ancho;

        RaycastHit2D hitCentro = Physics2D.Raycast(centro, Vector2.down, longitudRaycast, capaSuelo);
        RaycastHit2D hitIzq = Physics2D.Raycast(izquierda, Vector2.down, longitudRaycast, capaSuelo);
        RaycastHit2D hitDer = Physics2D.Raycast(derecha, Vector2.down, longitudRaycast, capaSuelo);

        return hitCentro.collider != null && hitIzq.collider != null && hitDer.collider != null;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        bool enPisoAhora = EnPiso();

        if (!enPisoAhora && estabaEnPiso)
        {
            if (!saltoPresionado)
                saltosRestantes = 0;
        }

        if (enPisoAhora && !estabaEnPiso)
        {
            saltosRestantes = 2;
            saltoPresionado = false;
        }

        if (enPisoAhora && EsPuntoSeguro())
            ultimoPuntoSeguro = transform.position;

        estabaEnPiso = enPisoAhora;

        animator.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.1f);
        animator.SetBool("EnPiso", enPisoAhora);
        animator.SetFloat("velocidadY", rb.linearVelocity.y);

        if (moveInput.x < 0)
            sr.flipX = true;
        else if (moveInput.x > 0)
            sr.flipX = false;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
            return;

        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        if (rb.linearVelocity.y < 0)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.fixedDeltaTime;

        if (rb.linearVelocity.y > 0 && !Keyboard.current.spaceKey.isPressed)
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * 2f * Time.fixedDeltaTime;

        if (EnPiso() && rb.linearVelocity.y < 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("KillZone") || invulnerable)
            return;

        if (MissionManager.Instance != null && MissionManager.Instance.EnMisionFinal())
        {
            StartCoroutine(MissionManager.Instance.MostrarFallasteFinal());
            return;
        }

        Respawn();
    }

    void Respawn()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = ultimoPuntoSeguro;
        StartCoroutine(Parpadeo());
    }

    IEnumerator Parpadeo()
    {
        invulnerable = true;

        for (int i = 0; i < 4; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        invulnerable = false;
    }
}