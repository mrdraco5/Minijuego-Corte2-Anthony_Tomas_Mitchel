using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject prefabFruta;
    public Transform[] puntosSpawn;
    [SerializeField] private bool usarMision = false;
    [SerializeField] private Transform puntoGoldenApple;
    [SerializeField] private Transform puntoGoldenAppleFinal;

    [Header("Spawn Bananas Mision 2")]
    [SerializeField] private Transform[] puntosBanana;

    [Header("Sonidos de las frutas")]
    public AudioClip sonidoBanana;
    public AudioClip sonidoStrawberry;
    public AudioClip sonidoOrange;
    public AudioClip sonidoMelon;
    public AudioClip sonidoGoldenApple;

    private void Start()
    {
        SpawnFrutas();
    }

    void SpawnFrutas()
    {
        var lista = GameDataLoader.Instance.datos.coleccionables;

        if (!usarMision)
        {
            List<Coleccionable> frutasValidas = new List<Coleccionable>();
            foreach (var f in lista)
            {
                if (f.nombre != "GoldenApple") frutasValidas.Add(f);
            }

            for (int i = 0; i < puntosSpawn.Length; i++)
            {
                int indexAleatorio = Random.Range(0, frutasValidas.Count);
                CrearFruta(frutasValidas[indexAleatorio], puntosSpawn[i]);
            }
        }
        else
        {
            List<string> frutasBase = new List<string>()
        {
            "Strawberry", "Strawberry", "Orange", "Melon", "Melon"
        };

            List<string> resultado = new List<string>();

            while (frutasBase.Count > 0)
            {
                int index = Random.Range(0, frutasBase.Count);
                string fruta = frutasBase[index];

                if (resultado.Count > 0)
                {
                    string ultima = resultado[resultado.Count - 1];
                    if (ultima == "Strawberry" && fruta == "Strawberry")
                        continue;
                }

                resultado.Add(fruta);
                frutasBase.RemoveAt(index);
            }

            for (int i = 0; i < puntosSpawn.Length; i++)
            {
                var frutaData = System.Array.Find(lista, f => f.nombre == resultado[i]);
                CrearFruta(frutaData, puntosSpawn[i]);
            }
        }
    }

    public void SpawnBananasMision2()
    {
        var lista = GameDataLoader.Instance.datos.coleccionables;

        var bananaData = System.Array.Find(lista, f => f.nombre == "Banana");

        for (int i = 0; i < puntosBanana.Length; i++)
        {
            CrearFruta(bananaData, puntosBanana[i]);
        }
    }

    void CrearFruta(Coleccionable fruta, Transform punto)
    {
        GameObject obj = Instantiate(prefabFruta, punto.position, Quaternion.identity);
        Sprite sprite = Resources.Load<Sprite>("Frutas/" + fruta.iconoId);
        ItemRecolectable item = obj.GetComponent<ItemRecolectable>();

        item.Inicializar(fruta.nombre, fruta.valor, sprite);

        if (fruta.nombre == "Banana")
            item.sonido = sonidoBanana;
        else if (fruta.nombre == "Strawberry")
            item.sonido = sonidoStrawberry;
        else if (fruta.nombre == "Orange")
            item.sonido = sonidoOrange;
        else if (fruta.nombre == "Melon")
            item.sonido = sonidoMelon;
        else if (fruta.nombre == "GoldenApple")
            item.sonido = sonidoGoldenApple;
    }

    public void SpawnGoldenApple()
    {
        var lista = GameDataLoader.Instance.datos.coleccionables;
        var appleData = System.Array.Find(lista, f => f.nombre == "GoldenApple");

        CrearFruta(appleData, puntoGoldenApple);
    }

    public void SpawnGoldenAppleFinal()
    {
        var lista = GameDataLoader.Instance.datos.coleccionables;
        var appleData = System.Array.Find(lista, f => f.nombre == "GoldenApple");

        GameObject obj = Instantiate(prefabFruta, puntoGoldenAppleFinal.position, Quaternion.identity);

        Sprite sprite = Resources.Load<Sprite>("Frutas/" + appleData.iconoId);
        ItemRecolectable item = obj.GetComponent<ItemRecolectable>();

        item.Inicializar(appleData.nombre, appleData.valor, sprite);
        item.sonido = sonidoGoldenApple;
        item.MarcarComoFinal();
    }
}