using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Clase que representa una sola pregunta con su oración y opciones
// [System.Serializable] permite editarla como bloque en el Inspector
[System.Serializable]
public class PreguntaClick
{
    // La oración completa con _____ donde va la palabra
    // Ej: "El perro _____ en el parque"
    [TextArea(1, 3)]
    public string oracion;

    public string respuestaCorrecta;

    // Exactamente 4 opciones para los botones
    public string[] opciones = new string[4];
}

public class ClickActividad : Actividad
{
    [Header("Elementos de la UI")]
    public TextMeshProUGUI textoPalabra;       // Muestra la oración
    public Button[] botonesOpciones;           // Los 4 botones
    public TextMeshProUGUI[] textosOpciones;   // Textos de cada botón
    public TextMeshProUGUI textoFeedback;
    public TextMeshProUGUI textoProgreso;      // Ej: "Pregunta 1 / 4"

    [Header("Colores de respuesta")]
    public Color colorCorrecto = Color.green;
    public Color colorIncorrecto = Color.red;
    public Color colorNormal = Color.white;    // Color original de los botones

    [Header("Preguntas")]
    // Array de 4 preguntas — se configura en el Inspector
    public PreguntaClick[] preguntas = new PreguntaClick[4];

    // Estado interno
    private int indicePreguntaActual = 0;
    private bool yaRespondio = false;
    private int preguntasCorrectas = 0;

    void Start()
    {
        MostrarPreguntaActual();
    }

    // Carga la pregunta del índice actual en la UI
    void MostrarPreguntaActual()
    {
        // Resetear estado para la nueva pregunta
        yaRespondio = false;
        textoFeedback.text = "";

        // Restaurar color original de todos los botones
        foreach (Button b in botonesOpciones)
            b.GetComponent<Image>().color = colorNormal;

        // Obtener la pregunta actual del array
        PreguntaClick pregunta = preguntas[indicePreguntaActual];

        // Mostrar la oración con el espacio vacío
        textoPalabra.text = pregunta.oracion;

        // Actualizar progreso
        textoProgreso.text = $"Pregunta {indicePreguntaActual + 1} / {preguntas.Length}";

        // Asignar opciones a los botones
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            textosOpciones[i].text = pregunta.opciones[i];

            int n = i; // capturar índice para el listener
            botonesOpciones[i].onClick.RemoveAllListeners();
            botonesOpciones[i].onClick.AddListener(() => AlHacerClick(n));

            // Reactivar botones por si estaban desactivados
            botonesOpciones[i].interactable = true;
        }
    }

    public void AlHacerClick(int indice)
    {
        if (yaRespondio) return;

        PreguntaClick pregunta = preguntas[indicePreguntaActual];
        string respuestaJugador = pregunta.opciones[indice];
        bool esCorrecta = respuestaJugador.Equals(pregunta.respuestaCorrecta);

        // Colorear botón presionado
        botonesOpciones[indice].GetComponent<Image>().color =
            esCorrecta ? colorCorrecto : colorIncorrecto;

        // Si falló, mostrar cuál era la correcta en verde
        if (!esCorrecta)
        {
            for (int i = 0; i < pregunta.opciones.Length; i++)
            {
                if (pregunta.opciones[i] == pregunta.respuestaCorrecta)
                    botonesOpciones[i].GetComponent<Image>().color = colorCorrecto;
            }
        }

        // Feedback
        textoFeedback.text = esCorrecta ? "¡Correcto! :^)" : "Incorrecto, era: " + pregunta.respuestaCorrecta;
        textoFeedback.color = esCorrecta ? colorCorrecto : colorIncorrecto;

        // Desactivar botones para que no puedan volver a clickear
        foreach (Button b in botonesOpciones)
            b.interactable = false;

        if (esCorrecta) preguntasCorrectas++;

        yaRespondio = true;
        completada = true;

        StartCoroutine(SiguientePregunta());
    }

    IEnumerator SiguientePregunta()
    {
        // Esperar 2 segundos mostrando el feedback
        yield return new WaitForSeconds(2f);

        indicePreguntaActual++;

        // Si quedan preguntas, cargar la siguiente
        if (indicePreguntaActual < preguntas.Length)
        {
            MostrarPreguntaActual();
        }
        else
        {
            // Se acabaron las 4 preguntas — calcular estrellas
            int estrellas = 0;
            if (preguntasCorrectas == preguntas.Length) estrellas = 3;
            else if (preguntasCorrectas >= preguntas.Length / 2) estrellas = 2;
            else if (preguntasCorrectas > 0) estrellas = 1;

            if (Juego.Instance != null)
                Juego.Instance.NivelCompletado(estrellas);
            else
                Debug.LogError("No se encontró Juego.Instance");
        }
    }

    // No se usa directamente pero se mantiene por herencia de Actividad
    public override void Mostrar() { MostrarPreguntaActual(); }

    public override bool Evaluar(object respuesta)
    {
        return respuesta.Equals(preguntas[indicePreguntaActual].respuestaCorrecta);
    }
}
