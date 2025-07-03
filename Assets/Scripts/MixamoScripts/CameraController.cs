using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;  // Sensibilidade ajust�vel no editor
    public float limitVerticalRotationDown = 90f;  // Limite para rota��o vertical
    public float limitVerticalRotationUp = 90f;  // Limite para rota��o vertical
    public Transform playerTransform;  // Transform do jogador

    private float _xRotation = 0f;  // Para rastrear a rota��o no eixo X

    void Start()
    {
        // Trava o cursor no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obt�m as entradas do mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Controla a rota��o vertical (eixo X)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, limitVerticalRotationDown, limitVerticalRotationUp);

        // Aplica a rota��o na c�mera no eixo X (olhar para cima/baixo)
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        // Aplica a rota��o horizontal (eixo Y) no jogador
        playerTransform.Rotate(Vector3.up * mouseX);
    }
}
