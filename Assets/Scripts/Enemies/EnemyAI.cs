using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol, // Патрулирование
        Chase   // Преследование
    }

    [Header("AI Logic")]
    [Tooltip("Радиус, в котором враг замечает игрока и начинает преследование.")]
    [SerializeField] private float _chaseRadius = 7f;
    [Tooltip("Ссылка на трансформ игрока. Должна быть установлена в инспекторе на сцене.")]
    [SerializeField] private Transform _playerTransform;

    [Header("Patrol Settings")]
    [Tooltip("Массив точек, между которыми враг будет патрулировать.")]
    [SerializeField] private Transform[] _patrolPoints;
    [Tooltip("Насколько близко враг должен подойти к точке патруля, чтобы считать ее достигнутой.")]
    [SerializeField] private float _patrolPointReachedThreshold = 0.5f;

    [Header("Component References")]
    [SerializeField] private MouseMover _mover;
    [SerializeField] private MouseAnimator _animator;

    private EnemyState _currentState;
    private int _currentPatrolPointIndex;

    private void Start()
    {
        // Валидация: проверяем, все ли настроено правильно.
        if (_playerTransform == null || _mover == null || _animator == null)
        {
            Debug.LogError("Ошибка конфигурации EnemyAI! Не все ссылки установлены.", this);
            enabled = false; // Отключаем компонент, чтобы избежать ошибок в рантайме.
            return;
        }

        // Устанавливаем начальное состояние
        _currentState = EnemyState.Patrol;
    }

    private void Update()
    {
        // Основной цикл принятия решений
        RunFSM();
    }

    private void RunFSM()
    {
        // Расстояние до игрока вычисляется один раз за кадр для экономии ресурсов
        float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

        // Логика переключения состояний
        switch (_currentState)
        {
            case EnemyState.Patrol:
                // Если игрок вошел в радиус агро, переключаемся на преследование
                if (distanceToPlayer < _chaseRadius)
                {
                    SwitchState(EnemyState.Chase);
                }
                ExecutePatrolState();
                break;

            case EnemyState.Chase:
                // Если игрок вышел из радиуса, возвращаемся в патруль
                if (distanceToPlayer > _chaseRadius)
                {
                    SwitchState(EnemyState.Patrol);
                }
                ExecuteChaseState();
                break;
        }
    }

    private void SwitchState(EnemyState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;
    }

    // Выполнение логики патрулирования
    private void ExecutePatrolState()
    {
        if (_patrolPoints == null || _patrolPoints.Length == 0)
        {
            _mover.Stop(); // Если патрулировать негде, стоим на месте
            _animator.SetDirection(Vector2.down); // Смотрим вниз для примера
            return;
        }

        Transform targetPoint = _patrolPoints[_currentPatrolPointIndex];
        Vector2 direction = (targetPoint.position - transform.position).normalized;

        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction); // Обновляем направление для аниматора

        // Если достигли точки патруля, выбираем следующую
        if (Vector2.Distance(transform.position, targetPoint.position) < _patrolPointReachedThreshold)
        {
            _currentPatrolPointIndex = (_currentPatrolPointIndex + 1) % _patrolPoints.Length;
        }
    }

    // Выполнение логики преследования
    private void ExecuteChaseState()
    {
        Vector2 direction = (_playerTransform.position - transform.position).normalized;
        _mover.SetMoveDirection(direction);
        _animator.SetDirection(direction);
    }

    // Визуализация радиуса преследования в редакторе для удобной настройки
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
    }
}
