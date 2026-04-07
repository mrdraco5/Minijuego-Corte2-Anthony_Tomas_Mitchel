using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    private string nombre;
    private int valor;
    private SpriteRenderer sr;
    private AudioSource audioSource;
    [HideInInspector] public AudioClip sonido;

    private Transform jugador;

    [SerializeField] private float distanciaActivacion = 2.5f;

    private bool yaActivada = false;
    private bool esFinal = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Inicializar(string n, int v, Sprite s)
    {
        nombre = n;
        valor = v;
        sr.sprite = s;
    }

    public void MarcarComoFinal()
    {
        esFinal = true;
    }

    void Update()
    {
        if (nombre != "GoldenApple" || yaActivada || esFinal)
            return;

        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia <= distanciaActivacion)
        {
            yaActivada = true;

            if (MissionManager.Instance != null)
            {
                MissionManager.Instance.ActivarMisionFinal();
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (nombre == "GoldenApple" && !esFinal)
            return;

        sr.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        audioSource.PlayOneShot(sonido);

        GameManager.Instance.RegistrarFruta(nombre, valor);

        if (MissionManager.Instance != null && !(nombre == "GoldenApple" && esFinal))
        {
            MissionManager.Instance.RegistrarProgreso(nombre);
        }

        Destroy(gameObject, sonido.length);
    }
}