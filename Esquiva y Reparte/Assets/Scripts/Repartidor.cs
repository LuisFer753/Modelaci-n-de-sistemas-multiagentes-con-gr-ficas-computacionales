using UnityEngine;

public class Repartidor : MonoBehaviour
{
    public GestorPartida gestor;

    // Detecta cuando entra en una zona Is Trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Casa"))
        {
            gestor.Entregar(other.gameObject);
        }
        else if (other.CompareTag("Paquetes"))
        {
            gestor.VisitarCorreos();
        }
    }

    // Detecta cuando choca con un coche
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coche"))
        {
            gestor.PenalizarChoque();
        }
    }
}
