using UnityEngine;

/// <summary>
/// This player controller class will update the events from the vehicle player.
/// Standar coding documentation can be found in 
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class MovimientoRepartidor : MonoBehaviour
{
    public float speed = 15.0f;
    public float turnSpeed = 10.0f;
    public float horizontalInput;
    public float forwardInput;

    private Rigidbody rb;
    Vector3 movement;

    /// <summary>
    /// This method is called before the first frame update
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// This method is called once per frame
    /// </summary>
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
        movement = transform.forward * forwardInput;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
