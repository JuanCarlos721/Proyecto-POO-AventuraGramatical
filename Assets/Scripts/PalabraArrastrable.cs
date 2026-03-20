using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// clase que contiene una palabra y a que categoria pertenece
public class PalabraArrastrable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Datos de la palabra")]
    public string palabra; // ejemplo: 
    public string categoria;

    [Header("UI")]

    // variable de transparencia, se puede modificar en el inspector.
    public float transparencia = 0.7f;

    // texto visible en la caja
    public TextMeshProUGUI textoPalabra;

    // variable para tener guardada la posicion de las palabras
    private Vector3 posicionOriginal;

    // Necesario para que unity calcule la posicion al arrastrar
    private Canvas canvas;

    // RectTransform es un Componente de posicion y tamano de objetos en el UI para Canvas
    private RectTransform rectTransform;

    // para controlar colisiones con otras palabras
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // asignar componentes de unity a variables para controlarlos
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponent<Canvas>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // guardar posicion inicial, para poder regresar las palabras
        posicionOriginal = rectTransform.localPosition;

        // mostrar texto de la palabra en la caja
        textoPalabra.text = palabra;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // OnBeginDrag se jecuta cuando empieza el arrastre de un objeto
    public void OnBeginDrag(PointerEventData eventData)
    {
        // blockRaycasts en false hace que este objeto deje pasar clicks a traves de este.
        // Para que la ZonaDestino detecte las palabras que recibe
        canvasGroup.blocksRaycasts = false;

        // alpha representa la transparencia de los objetos.
        // para ayudar a diferenciar que objeto se esta moviendo
        canvasGroup.alpha = transparencia;
    }

    // OnDrag se ejecuta cada frame que el jugador esta arrastrando un objeto
    public void OnDrag(PointerEventData eventData) 
    {
        // eventData.delta es cuantos pixeles se mueve el mouse este frame
        // se divide por canvas.scaleFactor para que funcione en cualquier resolucion de pantalla
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // OnEndDrag se ejecuta al soltar el click despues de arrastrar
    public void OnEndDrag(PointerEventData eventData)
    {
        // reactivar raycast
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        //Si la palabra se arrastro y no es aceptada por la ZonaDestino, regresa a su posicion original
        rectTransform.localPosition = posicionOriginal;
    }

    // Colocar es llamado por ZonaDestino cuando acepta una palabra.
    // mueve la palabra a la posicion de la zona y la congela ahi
    public void Colocar(Vector3 posicion)
    {
        rectTransform.position = posicion;

        Destroy(GetComponent<PalabraArrastrable>());
    }
}
