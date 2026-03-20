using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Clase hija de Actividad
public class ClickActividad : Actividad
{
    // Para mostrar en el inspector de unity las siguientes variables
    [Header("Elementos de la UI")]

    // texto de palabras a clasificar
    public TextMeshProUGUI textoPalabra;

    // Botones de respuesta.
    public Button[] botonesOpciones;

    // texto de cada boton (verbo, sustantivo, ...)
    public TextMeshProUGUI[] textosOpciones;

    // texto que muestra Correcto o Incorrecto
    public TextMeshProUGUI textoFeedback;

    [Header("Colores de respuesta")]
    // Colores si la respuesta es correcta o incorrecta
    public Color colorCorrecto = Color.green;
    public Color colorIncorrecto = Color.red;

    [Header("Datos pregunta")]
    // palabra que se muestra en pantalla
    public string palabraMostrada;

    public string respuestaCorrecta;

    // palabras que se muestran en los botones
    public string[] opciones;

    // variable para comprobar si el usuario ha contestado
    private bool yaRespondio = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mostrar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // se modifica el metodo heredado Mostrar
    public override void Mostrar()
    {
        // establecer palabra
        textoPalabra.text = palabraMostrada;
        // dejar vario el feedback para que no se vea
        textoFeedback.text = "";

        // 
        for (int i = 0; i < botonesOpciones.Length; i++)
        {
            textosOpciones[i].text = opciones[i];

            int n = i;

            // Limpiar listeners anteriores para evitar que se acumulen
            // si el script se usa varias veces
            botonesOpciones[i].onClick.RemoveAllListeners();

            // AddListener() conecta una funcion al evento OnClick del 
            // Cuando el jugador haga clic, llama a AlHacerClic(indice)
            botonesOpciones[i].onClick.AddListener(() => AlHacerClick(n));
        }
    }

    // metodo que se llama cuando el jugador da clic en un boton, recibiendo el indice del boton presionado
    public void AlHacerClick(int indice)
    {
        if (yaRespondio) // si ya se respondio, no hacer nada
        {
            return;
        }

        // guardar respuesta del jugador
        string respuestaJugador = opciones[indice];

        // se evalua si la respuesta del jugador es correcta
        bool esCorrecta = Evaluar(respuestaJugador);

        // cambiar el color del boton si acierta o no
        botonesOpciones[indice].GetComponent<Image>().color = esCorrecta ? colorCorrecto : colorIncorrecto;
        
        // si no acierta, tambien se marca en verde la correcta
        if (!esCorrecta)
        {
            for (int i = 0; i < opciones.Length; i++)
            {
                if (opciones[i] == respuestaCorrecta)
                {
                    botonesOpciones[i].GetComponent<Image>().color = colorCorrecto;

                }
            }
        }

        // mostrar texto de feedback si acierta o no
        textoFeedback.text = esCorrecta ? "Correcto!! :^)" : "Incorrecto :(, era: " + respuestaCorrecta;
        textoFeedback.color = esCorrecta ? colorCorrecto : colorIncorrecto;

        yaRespondio = true;
        completada = true;

        // Pasar a la siguiente pregunta
        StartCoroutine(SiguientePregunta());
    }

    public override bool Evaluar(object respuesta)
    {
        // retorna True o False si la respuesta del jugador es igual a la respuestaCorrecta
        return respuesta.Equals(respuestaCorrecta);
    }

    IEnumerator SiguientePregunta()
    {
        // retrasar esta funcion 2 segundos
        yield return new WaitForSeconds(2f);

        Debug.Log("cargando siguiente pregunta....");
    }
}
