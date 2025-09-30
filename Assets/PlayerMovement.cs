using UnityEngine;
using UnityEngine.Assertions; // Добавлен для проверки Rigidbody, как у преподавателя

// Используем имя из кода преподавателя для ясности
public class ObjectMover : MonoBehaviour
{
    // Используем приватные поля с [SerializeField], как у преподавателя
    [SerializeField] private float _speed = 10f; // Переименовано из moveSpeed
    [SerializeField] private float _jumpForce = 20f; // Переименовано из jumpForce

    // Переменные для проверки земли (Raycast удален)
    private Rigidbody2D _rigidbody;
    private bool _isGrounded;

    private float _horizontalInput;

    private void Awake() // Используем Awake вместо Start, как у преподавателя
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        // Assert.IsNotNull используется для строгой проверки
        Assert.IsNotNull(_rigidbody, "Ошибка: Rigidbody2D не найдено на объекте Player!");
    }

    private void Update()
    {
        // 1. Горизонтальное движение (Используем только velocity, без MovePosition/FixedUpdate)

        // Устанавливаем горизонтальный ввод (стрелки)
        _horizontalInput = 0f;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _horizontalInput = 1f;
        }

        // Применяем горизонтальную скорость через linearVelocity (как в коде преподавателя)
        // Примечание: В новых версиях Unity это rb.velocity.x, но оставляем linearVelocity для соответствия
        _rigidbody.linearVelocity = new Vector2(_horizontalInput * _speed, _rigidbody.linearVelocity.y);

        // 2. Прыжок (Пробел или Стрелка Вверх)
        // Используем Стрелку Вверх, как вы просили, но логика Jump взята у преподавателя
        if (Input.GetKeyDown(KeyCode.UpArrow) && _isGrounded)
        {
            // Сначала запрещаем повторный прыжок
            _isGrounded = false;

            // Применяем силу мгновенным толчком (AddForce с Impulse)
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }

    // FixedUpdate и BoxCast удалены.
    // Вместо них используем OnCollisionEnter2D для проверки земли.

    // MonoBehavior method that raised when object interacted with another Collider2D
    private void OnCollisionEnter2D(Collision2D other)
    {
        // ОБЯЗАТЕЛЬНО убедитесь, что ваши платформы имеют тег "Ground"!
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    // Этот метод поможет, если игрок слетел с платформы (добавлен для надежности)
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }
}