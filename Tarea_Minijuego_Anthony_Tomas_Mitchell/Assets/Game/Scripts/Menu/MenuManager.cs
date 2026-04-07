using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject panelInstrucciones;

    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Salir del juego");
    }

    public void MostrarInstrucciones()
    {
        panelInstrucciones.SetActive(true);
    }

    public void CerrarInstrucciones()
    {
        panelInstrucciones.SetActive(false);
    }
}