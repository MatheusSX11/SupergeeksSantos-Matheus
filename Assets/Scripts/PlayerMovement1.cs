using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public float speed = 5f;
    public float speedFast;
    public float jump = 5f;

    // RigidBody: componente de física
    public Rigidbody physics;

    // Transform: componente de transformações (posição, rotação e escala)
    public Transform cameraTransform;

    private bool isOnGround;

    public Animator anim;
    bool isFalling = false;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        physics = GetComponent<Rigidbody>();
        // colliderAnim = GetComponent<Animator>();
        speedFast = speed * 2f;
    }
    void Start()
    {
        transform.position = GameManager.Instance.GetLastPlayerTransform().position;
    }


    void Update()
    {

        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        Vector3 front = cameraTransform.forward;
        Vector3 side = cameraTransform.right;
        front.y = 0; // Garantir que o jogador rotacione com a câmera para baixo ou para cima
        side.y = 0; // Evita movimento indesejado no eixo Y
        front.Normalize(); // Molda o valor para o mesmo tamanho em todos os eixos
        side.Normalize();   // Molda o valor para o mesmo tamanho em todos os eixos

        // Valor de movimentação: soma frente e lado multiplicados pelos inputs e velocidade
        Vector3 movement;

        if (!Input.GetKey(KeyCode.LeftShift))
            movement = (front * inputVertical + side * inputHorizontal) * speed;
        else
            movement = (front * inputVertical + side * inputHorizontal) * speedFast;

        // Aplicar a física
        physics.velocity = new Vector3(movement.x, physics.velocity.y, movement.z);

        // Verifica se estou iniciando o movimento
        if (movement.magnitude > 0.1F)
        {
            // Transform.forward é o valor da frente
            transform.forward =
                Vector3.Slerp(              // Slerp: Método que modifica um valor para outro em x tempo
                    transform.forward,      // Valor modificado: frente do modelo
                    movement.normalized,   // Valor alvo: direção do movimento
                    Time.deltaTime * 10f);  // Time.deltaTime: tempo em segundos
        }

        // PULO
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            physics.velocity = new Vector3(physics.velocity.x, 0, physics.velocity.z);
            anim.SetBool("isJumping", true);
            physics.AddForce(Vector3.up * jump, ForceMode.Impulse);
            isOnGround = false;
        }


        if (isOnGround)
        {

            if (anim.GetBool("isJumping"))
            {
                physics.velocity = new Vector3(physics.velocity.x, 0, physics.velocity.z);
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", false);
                anim.SetBool("isFaceplant", false);
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("velX", inputHorizontal * 2);
                anim.SetFloat("velZ", inputVertical * 2);
            }
            else
            {
                anim.SetFloat("velX", inputHorizontal);
                anim.SetFloat("velZ", inputVertical);
            }
        }
        else
        {
            CheckIfFalling();
        }
    }
    public void Impulse(float impulse)
    {
        physics.velocity = new Vector3(physics.velocity.x, 0, physics.velocity.z);
        anim.SetBool("isJumping", true);
        physics.AddForce(Vector3.up * impulse, ForceMode.Impulse);
        isOnGround = false;
    }

    void CheckIfFalling()
    {
        RaycastHit hit;
        float longRay = 8f;
        float shortRay = 5.5f;
        float verticalVel = physics.velocity.y;

        // Verifica se NÃO há chão abaixo num raio de 6 unidades (longRay)
        if (!Physics.Raycast(transform.position, Vector3.down, longRay))
        {
            // ele já está caindo, porém não queremos mudar a animação agora,
            // e sim só quando ele estiver mais perto do chão, ou se a velocidade
            // for muito alta ao ponto de ele perder o controle da queda

            isFalling = true;
            if (physics.velocity.y <= 0.5f)
                anim.SetBool("isFalling", true);

            Debug.Log("Está caindo true!!");
            //se nao houver colisao 6 abaixo,
            Debug.DrawRay(transform.position, Vector3.down * longRay, Color.magenta);
        }

        // Se está caindo...
        if (isFalling)
        {
            // Verifica se está perto do chão 
            if (Physics.Raycast(transform.position, Vector3.down, out hit, shortRay))
            {
                //verifica se a velocidade de queda está alta
                if (hit.collider.CompareTag("Ground") && verticalVel < -10f)
                {
                    Debug.DrawRay(transform.position, Vector3.down * longRay, Color.red);
                    anim.SetBool("isFaceplant", true);
                    isFalling = false; // resetar o estado
                    isOnGround = true;
                }
            }
        }
        // Desenha os dois raios para debug
        Debug.DrawRay(transform.position, Vector3.down * shortRay, Color.cyan);   // pouso
    }

    private void OnCollisionEnter(Collision collision)
    {
        //CollideAndSlide(physics.velocity, transform.position, 0);

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            anim.SetBool("isFalling", false);
            anim.SetBool("isFaceplant", false);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

}