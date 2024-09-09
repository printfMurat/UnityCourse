using UnityEngine;

public class CarControl : MonoBehaviour
{
    // H�z, ivme ve d�n�� parametreleri
    public float maxSpeed = 20f; // Maksimum h�z
    public float acceleration = 5f; // �vme (h�zlanma)
    public float deceleration = 10f; // Frenleme kuvveti
    public float turnSpeed = 100f; // D�n�� h�z�

    private float currentSpeed = 0f; // Anl�k h�z
    private Rigidbody2D rb;

    void Start()
    {
        // Rigidbody2D bile�enini al
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // �leri-geri hareketi i�in input al
        float moveInput = Input.GetAxis("Vertical");

        // �vme ile h�zlanma
        if (moveInput > 0) // �leriye gitme
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
        }
        else if (moveInput < 0) // Geriye gitme
        {
            currentSpeed -= acceleration * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.Space)) // Fren sistemi
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.fixedDeltaTime); // H�z kademeli olarak s�f�ra yakla�acak
        }
        else
        {
            // Ara� gaz kesildi�inde yava� yava� durmal� (do�al s�rt�nme)
            currentSpeed = Mathf.Lerp(currentSpeed, 0, 0.5f * Time.fixedDeltaTime);
        }

        // Maksimum h�z� a�mamas� i�in h�z� s�n�rla
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Araban�n hareketi: Sprite a�a��ya bak�yor, bu y�zden eksiyi kullan�yoruz
        rb.velocity = -transform.up * currentSpeed;

        // E�er hareket varsa, d�n�� yapmaya izin ver
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turnInput = Input.GetAxis("Horizontal");

            if (currentSpeed < 0) // Geri gidiyorsa
            {
                turnInput = -turnInput; // Geri giderken d�n�� y�n�n� ters �evir
            }

            // D�n�� hareketi
            transform.Rotate(Vector3.forward, -turnInput * turnSpeed * Time.fixedDeltaTime);
        }
    }
}
