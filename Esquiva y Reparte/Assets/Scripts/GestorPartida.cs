using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorPartida : MonoBehaviour
{
    public int meta = 20; // Total de paquetes a entregar
    public int capacidadCamioneta = 5;
    public float tiempoRestante = 60f;
    public float penalizacionChoque = 2f; // Segundos perdidos al chocar con coches
    public Renderer estacionCorreosRenderer; // Para pintarla de color como aviso
    public GameObject[] todasLasCasas; // Todas las casas
    public int entregados = 0;
    public int enCamioneta = 0;
    private bool terminada = false;
    public bool MisionCumplida
    {
        get { return entregados >= meta; }
    }

    void Start()
    {
        // Al iniciar se cargan 5 paquetes y activa 5 casas
        RecargarPaquetes();
    }

    void Update()
    {
        if (terminada)
        {
            if (Input.GetKeyDown(KeyCode.R)) Reiniciar();
            return;
        }

        // Cuenta regresiva
        tiempoRestante -= Time.deltaTime;
        if (tiempoRestante <= 0f)
        {
            Perder();
        }
    }

    public void Entregar(GameObject casa)
    {
        if (terminada || enCamioneta <= 0) return;

        enCamioneta--;
        entregados++;
        casa.SetActive(false);
        Debug.Log("Paquete entregado (" + entregados + " de " + meta + ")");

        if (MisionCumplida)
        {
            Debug.Log("Todos entregados. Ve a estación de correos.");
            if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.green;
        }
        else if (enCamioneta == 0)
        {
            Debug.Log("Vuelve a la estación para recargar.");
            if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.yellow;
        }
    }

    public void VisitarCorreos()
    {
        if (terminada) return;

        if (MisionCumplida)
        {
            Ganar();
        }
        else if (enCamioneta == 0)
        {
            RecargarPaquetes();
            if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.white;
        }
    }

    public void PenalizarChoque()
    {
        if (terminada) return;
        tiempoRestante -= penalizacionChoque;
        Debug.Log("¡Choque! -" + penalizacionChoque + "segs");
        if (tiempoRestante <= 0f)
        {
            tiempoRestante = 0f;
            Perder();
        }
    }

    private void RecargarPaquetes()
    {
        enCamioneta = capacidadCamioneta;

        for (int i = 0; i < todasLasCasas.Length; i++)
            todasLasCasas[i].SetActive(false);

        List<int> disponibles = new List<int>();
        for (int i = 0; i < todasLasCasas.Length; i++)
            disponibles.Add(i);

        int cantidad = Mathf.Min(capacidadCamioneta, todasLasCasas.Length);
        for (int i = 0; i < cantidad; i++)
        {
            int azar = Random.Range(0, disponibles.Count);
            todasLasCasas[disponibles[azar]].SetActive(true);
            disponibles.RemoveAt(azar);
        }
    }

    public void Ganar()
    {
        if (terminada) return;
        terminada = true;
        Debug.Log("¡Ganaste! Presiona R para reiniciar.");
    }

    public void Perder()
    {
        if (terminada) return;
        terminada = true;
        Debug.Log("Perdiste");
        if (estacionCorreosRenderer != null) estacionCorreosRenderer.material.color = Color.red;
        Invoke("Reiniciar", 2.0f); // Muestra mensaje y reinicia solo
    }

    void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}