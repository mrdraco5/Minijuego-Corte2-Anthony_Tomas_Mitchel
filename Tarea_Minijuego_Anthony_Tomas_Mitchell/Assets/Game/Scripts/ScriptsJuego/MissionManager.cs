using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("UI Misión")]
    [SerializeField] private TextMeshProUGUI textoMision;
    [SerializeField] private GameObject panelFelicidades;
    [SerializeField] private GameObject panelFallaste;
    [SerializeField] private TextMeshProUGUI textoFelicidades1;
    [SerializeField] private TextMeshProUGUI textoFelicidades2;
    [SerializeField] private float tiempoMostrarPanel = 2f;
    [SerializeField] private GameObject panelPeligro;

    [Header("Spawner")]
    [SerializeField] private ItemSpawner itemSpawner;

    [Header("Misión 2 - Tiempo")]
    [SerializeField] private float tiempoLimite = 15f;
    [SerializeField] private GameObject panelTiempo;
    [SerializeField] private TextMeshProUGUI textoTiempo;

    [Header("Respawn")]
    [SerializeField] private Transform spawnMision2;
    [SerializeField] private Transform spawnFinal;
    [SerializeField] private Transform spawnFinalGoldenApple;
    [SerializeField] private GameObject jugador;

    [Header("Misión Final")]
    [SerializeField] private LavaSube lava;

    private List<Mision> misiones;
    private Dictionary<string, int> progreso = new Dictionary<string, int>();

    private int indiceMisionActual = 0;

    private float tiempoActual;
    private bool contandoTiempo = false;

    private bool esperarMisionFinal = false;
    private bool eventoFinalActivado = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        misiones = new List<Mision>(GameDataLoader.Instance.datos.misiones);
        CargarMision();
    }

    void Update()
    {
        if (contandoTiempo)
        {
            tiempoActual -= Time.deltaTime;

            if (textoTiempo != null)
                textoTiempo.text = Mathf.Ceil(tiempoActual).ToString();

            if (tiempoActual <= 0f)
            {
                contandoTiempo = false;
                StartCoroutine(MostrarFallaste());
            }
        }
    }

    void CargarMision()
    {
        progreso.Clear();

        var mision = misiones[indiceMisionActual];

        foreach (var obj in mision.objetivos)
            progreso[obj.itemName] = 0;

        if (indiceMisionActual == 1)
        {
            tiempoActual = tiempoLimite;
            contandoTiempo = true;

            if (panelTiempo != null)
                panelTiempo.SetActive(true);

            itemSpawner.SpawnBananasMision2();
        }
        else
        {
            contandoTiempo = false;

            if (panelTiempo != null)
                panelTiempo.SetActive(false);
        }

        ActualizarTexto();
    }

    public void RegistrarProgreso(string nombre)
    {
        if (!progreso.ContainsKey(nombre))
            return;

        progreso[nombre]++;
        ActualizarTexto();

        if (MisionCompleta())
        {
            contandoTiempo = false;

            itemSpawner.SpawnGoldenApple();

            if (panelTiempo != null)
                panelTiempo.SetActive(false);

            if (indiceMisionActual == 1)
                esperarMisionFinal = true;

            StartCoroutine(MostrarPanelYSiguiente());
        }
    }

    bool MisionCompleta()
    {
        var mision = misiones[indiceMisionActual];

        foreach (var obj in mision.objetivos)
        {
            if (progreso[obj.itemName] < obj.cantidad)
                return false;
        }

        return true;
    }

    IEnumerator MostrarPanelYSiguiente()
    {
        textoFelicidades1.text = "¡MISIÓN COMPLETADA!";
        textoFelicidades2.text = "Avanzarás a la siguiente misión...";

        panelFelicidades.SetActive(true);
        yield return new WaitForSeconds(tiempoMostrarPanel);
        panelFelicidades.SetActive(false);

        if (!esperarMisionFinal)
            SiguienteMision();
    }

    IEnumerator MostrarFallaste()
    {
        if (panelTiempo != null)
            panelTiempo.SetActive(false);

        Time.timeScale = 0f;
        panelFallaste.SetActive(true);

        yield return null;
    }

    public void ReintentarMision()
    {
        Time.timeScale = 1f;
        panelFallaste.SetActive(false);

        if (EnMisionFinal())
        {
            if (lava != null)
            {
                lava.gameObject.SetActive(true);
                lava.ReiniciarLava();
            }

            jugador.transform.position = spawnFinal.position;
            StartCoroutine(ParpadeoJugador());

            return;
        }

        progreso.Clear();

        var mision = misiones[indiceMisionActual];
        foreach (var obj in mision.objetivos)
            progreso[obj.itemName] = 0;

        ActualizarTexto();

        StartCoroutine(RespawnJugador());
    }

    IEnumerator ParpadeoJugador()
    {
        SpriteRenderer sr = jugador.GetComponent<SpriteRenderer>();

        for (int i = 0; i < 5; i++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator RespawnJugador()
    {
        yield return StartCoroutine(ParpadeoJugador());

        jugador.transform.position = spawnMision2.position;

        ItemRecolectable[] items = Object.FindObjectsByType<ItemRecolectable>(FindObjectsSortMode.None);
        foreach (var item in items)
            Destroy(item.gameObject);

        itemSpawner.SpawnBananasMision2();

        tiempoActual = tiempoLimite;
        contandoTiempo = true;

        if (panelTiempo != null)
            panelTiempo.SetActive(true);
    }

    void SiguienteMision()
    {
        indiceMisionActual++;

        if (indiceMisionActual >= misiones.Count)
        {
            textoMision.text = "¡Todas las misiones completadas!";
            return;
        }

        CargarMision();
    }

    void ActualizarTexto()
    {
        var mision = misiones[indiceMisionActual];

        string texto = "<b>Misión " + (indiceMisionActual + 1) + "</b>\n\n";
        texto += "<b>" + mision.titulo + "</b>\n\n";

        foreach (var obj in mision.objetivos)
        {
            texto += obj.itemName + "\n";

            int valorActual = progreso[obj.itemName];
            if (valorActual > obj.cantidad)
                valorActual = obj.cantidad;

            texto += valorActual + " / " + obj.cantidad + "\n\n";
        }

        textoMision.text = texto;
    }

    public void ActivarMisionFinal()
    {
        if (eventoFinalActivado)
            return;

        eventoFinalActivado = true;
        StartCoroutine(SecuenciaFinal());
    }

    IEnumerator SecuenciaFinal()
    {
        StartCoroutine(ParpadeoPeligro());

        yield return new WaitForSeconds(2f);

        panelPeligro.SetActive(false);

        if (lava != null)
        {
            lava.gameObject.SetActive(true);
            lava.ActivarLava();
        }

        if (itemSpawner != null)
        {
            itemSpawner.SpawnGoldenAppleFinal();
        }

        esperarMisionFinal = false;
        SiguienteMision();
    }

    IEnumerator ParpadeoPeligro()
    {
        float tiempo = 2f;
        float intervalo = 0.2f;

        while (tiempo > 0)
        {
            panelPeligro.SetActive(!panelPeligro.activeSelf);
            yield return new WaitForSeconds(intervalo);
            tiempo -= intervalo;
        }

        panelPeligro.SetActive(false);
    }

    public bool EnMisionFinal()
    {
        return indiceMisionActual == 2;
    }

    public IEnumerator MostrarFallasteFinal()
    {
        Time.timeScale = 0f;
        panelFallaste.SetActive(true);
        yield return null;
    }

    public void MoverGoldenAppleFinal(Transform apple)
    {
        apple.position = spawnFinalGoldenApple.position;
    }
}