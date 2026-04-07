using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Texto frutas (Panel Superior)")]
    public TextMeshProUGUI textoBanana;
    public TextMeshProUGUI textoStrawberry;
    public TextMeshProUGUI textoOrange;
    public TextMeshProUGUI textoMelon;
    public TextMeshProUGUI textoGoldenApple;

    [Header("Texto frutas (Panel Puntaje)")]
    public TextMeshProUGUI textoBananaPuntaje;
    public TextMeshProUGUI textoStrawberryPuntaje;
    public TextMeshProUGUI textoOrangePuntaje;
    public TextMeshProUGUI textoMelonPuntaje;
    public TextMeshProUGUI textoGoldenApplePuntaje;

    [Header("Texto total")]
    public TextMeshProUGUI textoPuntajeTotal;

    [Header("Configuracion nivel")]
    [SerializeField] private GameObject panelPuntaje;
    [SerializeField] private int totalFrutasNivel;
    [SerializeField] private bool usarPanelFinal = true;
    [SerializeField] private bool esNivel2;

    private Dictionary<string, int> cantidadFrutas = new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);
    private Dictionary<string, int> puntajeFrutas = new Dictionary<string, int>(System.StringComparer.OrdinalIgnoreCase);

    private int puntajeTotal = 0;
    private int frutasRecogidas = 0;

    public void IrANivel2()
    {
        SceneManager.LoadScene("Nivel2");
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ReintentarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel1");
    }

    public void ReintentarNivel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel2");
    }

    void Awake()
    {
        Instance = this;

        cantidadFrutas["Banana"] = 0;
        cantidadFrutas["Strawberry"] = 0;
        cantidadFrutas["Orange"] = 0;
        cantidadFrutas["Melon"] = 0;
        cantidadFrutas["GoldenApple"] = 0;

        puntajeFrutas["Banana"] = 0;
        puntajeFrutas["Strawberry"] = 0;
        puntajeFrutas["Orange"] = 0;
        puntajeFrutas["Melon"] = 0;
        puntajeFrutas["GoldenApple"] = 0;
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    public void RegistrarFruta(string nombre, int valor)
    {
        if (!cantidadFrutas.ContainsKey(nombre))
            return;

        Debug.Log("Fruta Recolectada: <color=yellow>" + nombre + "</color> | Valor: <color=green>" + valor + "</color>");

        cantidadFrutas[nombre] += 1;
        puntajeFrutas[nombre] += valor;
        puntajeTotal += valor;
        frutasRecogidas++;

        ActualizarUI();

        if (usarPanelFinal)
        {
            bool condicionNivel1 = !esNivel2 && frutasRecogidas >= totalFrutasNivel;
            bool condicionNivel2 = esNivel2 && nombre == "GoldenApple";

            if (condicionNivel1 || condicionNivel2)
            {
                StartCoroutine(MostrarPanelDelay());
            }
        }
    }

    IEnumerator MostrarPanelDelay()
    {
        yield return null;
        MostrarPanelFinal();
    }

    void MostrarPanelFinal()
    {
        if (textoBananaPuntaje != null)
            textoBananaPuntaje.text = cantidadFrutas["Banana"].ToString();

        if (textoStrawberryPuntaje != null)
            textoStrawberryPuntaje.text = cantidadFrutas["Strawberry"].ToString();

        if (textoOrangePuntaje != null)
            textoOrangePuntaje.text = cantidadFrutas["Orange"].ToString();

        if (textoMelonPuntaje != null)
            textoMelonPuntaje.text = cantidadFrutas["Melon"].ToString();

        if (textoGoldenApplePuntaje != null)
            textoGoldenApplePuntaje.text = cantidadFrutas["GoldenApple"].ToString();

        if (textoPuntajeTotal != null)
            textoPuntajeTotal.text = puntajeTotal.ToString();

        if (panelPuntaje != null)
        {
            panelPuntaje.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void ActualizarUI()
    {
        if (textoBanana != null)
            textoBanana.text = puntajeFrutas["Banana"].ToString();

        if (textoStrawberry != null)
            textoStrawberry.text = puntajeFrutas["Strawberry"].ToString();

        if (textoOrange != null)
            textoOrange.text = puntajeFrutas["Orange"].ToString();

        if (textoMelon != null)
            textoMelon.text = puntajeFrutas["Melon"].ToString();

        if (textoGoldenApple != null)
            textoGoldenApple.text = puntajeFrutas["GoldenApple"].ToString();
    }
}