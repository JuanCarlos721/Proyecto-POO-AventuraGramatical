using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// clase maneja dialogos de DialogoData
public class ControladorDialogo : MonoBehaviour
{
    [Header("Referencias UI")]
    public Image spritePersonaje; // imagen del personaje
    public TextMeshProUGUI nombrePersonaje;
    public TextMeshProUGUI textoDialogo;
    public Button botonContinuar;
    // panel del globo de texto completo
    public GameObject globoTexto;

    [Header("Animacion sprite")]
    // valores de cuanto se mueve y a que velocidad
    public float amplitudFlotacion = 8f;
    public float velocidadFlotacion = 1.5f;

    [Header("Configuracion")]
    public float velocidadEscritura = 0.4f;

    // instancia de la clase DialogoData, se le asigna la referencia en el inspector
    public DialogoData dialogoActual;

    private int indiceLinea = 0;
    private bool escribiendo = false;
    // referencia para cancelar la escritura
    private Coroutine coroutineEscritura;
    private Vector3 posOriginalSprite; // posicion base de la animacion
    private AudioSource audioSource;

    private void Awake()
    {
        // asegurar el component de audio
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is 
    void Start()
    {

        if (Juego.Instance != null)
            dialogoActual = Juego.Instance.dialogoPendiente;

        posOriginalSprite = spritePersonaje.rectTransform.localPosition;

        // desactivar boton hasta que el personaje termine el dialogo
        botonContinuar.interactable = false;

        // agregar listener al boton con el metodo AlPresionarContinuar
        botonContinuar.onClick.AddListener(AlPresionarContinuar);

        // cargar dialogo
        if (dialogoActual != null)
        {
            MostrarLinea(0);
        }
        else
        {
            Debug.LogError("No hay dialogo asignado en DialogoManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Animacion de flotacion del sprite del personaje
        float offsetY = Mathf.Sin(Time.time * velocidadFlotacion) * amplitudFlotacion;
        spritePersonaje.rectTransform.localPosition = posOriginalSprite + new Vector3(0, offsetY, 0);
    }

    void MostrarLinea(int i)
    {
        LineaDialogo linea = dialogoActual.lineas[indiceLinea];

        // actualizar sprite y nombre 
        spritePersonaje.sprite = linea.spritePersonaje;
        nombrePersonaje.text = linea.nombrePersonaje;

        // cancelar escritura si habia una anterior
        if (coroutineEscritura != null)
        {
            StopCoroutine(coroutineEscritura);
        }

        // efecto de maquina de escribir
        coroutineEscritura = StartCoroutine(EscribirTexto(linea));
    }

    IEnumerator EscribirTexto(LineaDialogo linea)
    {
        escribiendo = true;
        botonContinuar.interactable = false;
        textoDialogo.text = "";

        // reproducir audio cuando hay linea de texto
        if(linea.audioLinea != null)
        {
            audioSource.clip = linea.audioLinea;
            audioSource.Play();
        }

        // agregar cada caracter con pausas entre si
        foreach( char letra in linea.texto)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        // se termino de escribir
        escribiendo = false;
        botonContinuar.interactable = true;
    }

    void AlPresionarContinuar()
    {
        if (escribiendo)
        {
            StopCoroutine(coroutineEscritura);
            escribiendo = false;
            textoDialogo.text = dialogoActual.lineas[indiceLinea].texto;
            audioSource.Stop();
            botonContinuar.interactable = true;
            return;
        }

        // siguiente linea
        indiceLinea++;

        if (indiceLinea >= dialogoActual.lineas.Length)
        {
            DialogoTerminado();
            return;
        }


        MostrarLinea(indiceLinea);
    }

    void DialogoTerminado()
    {
        // indicar que termino el dialogo
        // para cargar el nivel
        Juego.Instance.DialogoCompletado();
    }
}
