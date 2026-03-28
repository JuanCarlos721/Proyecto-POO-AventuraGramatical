using UnityEngine;
using UnityEngine.SceneManagement;

// Clase de apoyo que se encarga de controlar las escenas
public class ControladorEscena : MonoBehaviour
{
    // Ir a escena a Menu
    public void IniciarMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Ir a escena de nivel con id
    public void IniciarNivel(int id)
    {
        SceneManager.LoadScene("Nivel");
    }

    // Ir a pantalla de resultados
    public void IniciarResultado()
    {
        SceneManager.LoadScene("Resultado");
    }

    public void IrACreditos()
    {
        SceneManager.LoadScene("Creditos");
    }
}
