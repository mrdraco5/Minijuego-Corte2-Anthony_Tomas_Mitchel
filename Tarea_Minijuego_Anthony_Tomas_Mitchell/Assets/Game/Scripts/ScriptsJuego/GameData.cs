using System;

[Serializable]
public class Coleccionable
{
    public string nombre;
    public string rareza;
    public int valor;
    public string iconoId;
}

[Serializable]
public class Mision
{
    public int id;
    public string titulo;
    public string descripcion;
    public Objetivo[] objetivos;
}

[Serializable]
public class Objetivo
{
    public string itemName;
    public int cantidad;
}

[Serializable]
public class ListaDatos
{
    public Coleccionable[] coleccionables;
    public Mision[] misiones;
}