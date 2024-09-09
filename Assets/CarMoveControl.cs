using UnityEngine;

public class CarControl : MonoBehaviour
{
    // Hýz, ivme ve dönüþ parametreleri
    public float maxSpeed = 20f; // Maksimum hýz
    public float acceleration = 5f; // Ývme (hýzlanma)
    public float deceleration = 10f; // Frenleme kuvveti
    public float turnSpeed = 100f; // Dönüþ hýzý

    private float currentSpeed = 0f; // Anlýk hýz
    private Rigidbody2D rb;

    void Start()
    {
        // Rigidbody2D bileþenini al
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Ýleri-geri hareketi için input al
        float moveInput = Input.GetAxis("Vertical");

        // Ývme ile hýzlanma
        if (moveInput > 0) // Ýleriye gitme
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;
        }
        else if (moveInput < 0) // Geriye gitme
        {
            currentSpeed -= acceleration * Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.Space)) // Fren sistemi
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.fixedDeltaTime); // Hýz kademeli olarak sýfýra yaklaþacak
        }
        else
        {
            // Araç gaz kesildiðinde yavaþ yavaþ durmalý (doðal sürtünme)
            currentSpeed = Mathf.Lerp(currentSpeed, 0, 0.5f * Time.fixedDeltaTime);
        }

        // Maksimum hýzý aþmamasý için hýzý sýnýrla
        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        // Arabanýn hareketi: Sprite aþaðýya bakýyor, bu yüzden eksiyi kullanýyoruz
        rb.velocity = -transform.up * currentSpeed;

        // Eðer hareket varsa, dönüþ yapmaya izin ver
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turnInput = Input.GetAxis("Horizontal");

            if (currentSpeed < 0) // Geri gidiyorsa
            {
                turnInput = -turnInput; // Geri giderken dönüþ yönünü ters çevir
            }

            // Dönüþ hareketi
            transform.Rotate(Vector3.forward, -turnInput * turnSpeed * Time.fixedDeltaTime);
        }
    }
}
