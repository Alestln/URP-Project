using UnityEngine;

public class MouseAI : MonoBehaviour
{
    public enum MouseState
    {
        Patrol, // Патрулирование
        Chase // Преследование
    }

    [Header("AI Logic")]
    [SerializeField] private float _chaseRadius = 3f; // Радиус преследования
    [SerializeField] private Transform _target; // Цель преследования (игрок)

    [Header("Component References")]
    [SerializeField] private MouseMover _mover; // Компонент для движения мыши
    [SerializeField] private MouseAnimator _animator; // Компонент для анимации мыши

    private MouseState _currentState;

    private void Start()
    {
        _currentState = MouseState.Patrol;
    }

    private void SwitchState(MouseState newState)
    {
        if (_currentState == newState)
        {
            return;
        }

        _currentState = newState;
    }

    private void Update()
    {
        RunFSM(); // Запуск конечного автомата состояний (FSM) для мыши
        // Здесь можно добавить дополнительные проверки или логику, если нужно
        // Например, проверка на здоровье, взаимодействие с окружением и т.д.
    }

    private void RunFSM()
    {
        float distanceToTarget = Vector2.Distance(transform.position, _target.position);

        switch(_currentState)
        {
            case MouseState.Patrol:
                if (distanceToTarget < _chaseRadius)
                {
                    SwitchState(MouseState.Chase);
                }
                // Здесь можно добавить логику патрулирования, например, движение по точкам
                break;
            case MouseState.Chase:
                if (distanceToTarget > _chaseRadius)
                {
                    SwitchState(MouseState.Patrol);
                }
                ExecuteChaseState();
                break;
        }
    }

    private void ExecuteChaseState()
    {
        Vector2 direction = (_target.position - transform.position).normalized;
        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction);
    }

    private void OnDrawGizmosSelected()
    {
        // Отображение радиуса преследования в редакторе
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
    }
}