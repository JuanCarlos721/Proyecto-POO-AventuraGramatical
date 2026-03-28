using UnityEngine;

// Clase abstracta como base para las clases hijas:
// ClickActividad, ConectarActividad y ArrastrarActividad
public abstract class Actividad : MonoBehaviour
{

    // Variables protected son solo accesibles por esta clase y clases hijas
    protected string enunciado;
    protected bool completada = false;

    // metodos abstractos que se las clases hijas especializaran
    public abstract void Mostrar();
    public abstract bool Evaluar(object respuesta);

    public void ReproducirSonido(AudioClip audio)
    {
        // AudioSource.PlayClipAtPoint() reproduce un clip de audio en una zona de la pantalla
        // Se usa Vector3.zero ya que no se necesita una posicion de origen especifica

        AudioSource.PlayClipAtPoint(audio, Vector3.zero);
    }

}
