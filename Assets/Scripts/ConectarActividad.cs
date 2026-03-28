using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ConectarActividad : MonoBehaviour
{
    [Header("Prefab de linea")]
    // Arrastra aqui un GameObject con Image + LineaUI.cs
    public GameObject lineaPrefab;

    [Header("Contenedor de lineas")]
    // GameObject vacio hijo del Canvas donde se crearan las lineas
    public Transform lineasContainer;

    [Header("UI")]
    public TextMeshProUGUI textoProgreso;
    public TextMeshProUGUI textoFeedback;

    [Header("Colores de lineas")]
    public Color colorCorrecto = Color.green;
    public Color colorIncorrecto = Color.red;
    public Color colorPendiente = Color.yellow;

    // El elemento de columna A que fue clickeado primero
    private ElementoConectable elementoSeleccionado = null;

    // Contadores
    private int conexionesCorrectas = 0;
    private int totalConexiones;

    // Lista de lineas instanciadas (para poder borrarlas si es necesario)
    private List<LineaUI> lineasActivas = new List<LineaUI>();

    void Start()
    {
        // Cuenta cuantos elementos hay en la columna A
        // Eso define el total de conexiones posibles
        ElementoConectable[] todos = FindObjectsByType<ElementoConectable>
            (FindObjectsSortMode.None);

        // Contamos solo los de columna A para saber el total
        foreach (var e in todos)
            if (e.esColumnaA) totalConexiones++;

        ActualizarProgreso();
    }

    // Llamado por ElementoConectable.AlHacerClic()
    public void RegistrarClick(ElementoConectable elemento)
    {
        // CASO 1: No hay nada seleccionado todavia
        if (elementoSeleccionado == null)
        {
            // Solo aceptamos el primer clic desde columna A
            if (!elemento.esColumnaA) return;

            elementoSeleccionado = elemento;
            elemento.SetEstado("seleccionado");
            textoFeedback.text = "Ahora selecciona su par en la columna derecha";
            return;
        }

        // CASO 2: Ya hay un elemento A seleccionado y el jugador clickea otro A
        // Deseleccionamos el anterior y seleccionamos el nuevo
        if (elemento.esColumnaA)
        {
            elementoSeleccionado.SetEstado("normal");
            elementoSeleccionado = elemento;
            elemento.SetEstado("seleccionado");
            return;
        }

        // CASO 3: Ya hay un elemento A y el jugador clickea un elemento B
        // Aqui evaluamos si la conexion es correcta
        EvaluarConexion(elementoSeleccionado, elemento);
    }

    void EvaluarConexion(ElementoConectable elementoA, ElementoConectable elementoB)
    {
        // Los IDs deben coincidir para que sea correcto
        bool esCorrecta = elementoA.idConexion == elementoB.idConexion;

        // Dibujamos la linea entre los dos elementos
        LineaUI linea = InstanciarLinea(
            elementoA.GetPosicion(),
            elementoB.GetPosicion(),
            esCorrecta ? colorCorrecto : colorIncorrecto
        );

        if (esCorrecta)
        {
            elementoA.SetEstado("correcto");
            elementoB.SetEstado("correcto");
            conexionesCorrectas++;
            textoFeedback.text = "Correcto!";
            ActualizarProgreso();

            // Verificamos si se completaron todas las conexiones
            if (conexionesCorrectas >= totalConexiones)
                StartCoroutine(NivelCompleto());
        }
        else
        {
            elementoA.SetEstado("incorrecto");
            elementoB.SetEstado("incorrecto");
            textoFeedback.text = "Ese par no coincide X";

            // Borramos la linea incorrecta y reseteamos despues de 1 segundo
            StartCoroutine(ResetearIncorrecto(elementoA, elementoB, linea));
        }

        // Limpiamos la seleccion actual
        elementoSeleccionado = null;
    }

    // Instancia un GameObject de linea y lo configura
    LineaUI InstanciarLinea(Vector3 desde, Vector3 hasta, Color color)
    {
        // Instantiate() crea una copia del prefab en la escena
        // lineasContainer como padre lo coloca dentro de ese objeto
        GameObject lineaObj = Instantiate(lineaPrefab, lineasContainer);
        LineaUI linea = lineaObj.GetComponent<LineaUI>();
        linea.Inicializar(desde, hasta, color);
        lineasActivas.Add(linea);
        return linea;
    }

    IEnumerator ResetearIncorrecto(ElementoConectable a, ElementoConectable b, LineaUI linea)
    {
        yield return new WaitForSeconds(1f);

        a.SetEstado("normal");
        b.SetEstado("normal");
        textoFeedback.text = "";

        // Destroy elimina la linea incorrecta de la escena
        if (linea != null)
            Destroy(linea.gameObject);
    }

    IEnumerator NivelCompleto()
    {
        textoFeedback.text = "¡Completaste todas las conexiones!";
        yield return new WaitForSeconds(2f);

        int estrellas = 0;
        if (conexionesCorrectas == totalConexiones) estrellas = 3;
        else if (conexionesCorrectas >= totalConexiones / 2) estrellas = 2;
        else if (conexionesCorrectas > 0) estrellas = 1;

        Juego.Instance.NivelCompletado(estrellas);
    }

    void ActualizarProgreso()
    {
        textoProgreso.text = $"{conexionesCorrectas} / {totalConexiones} conexiones";
    }
}