using UnityEngine;
using UnityEngine.SceneManagement;

// Clase de apoyo que se encarga de controlar las escenas
public class ControladorEscena : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
