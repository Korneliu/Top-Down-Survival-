using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private const string AnimAttack = "Attack", AnimWalk = "Walk";
    [SerializeField] private float _health;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _dist;
    [SerializeField] private Animator _animator;
    [SerializeField] private HelthBar _helthBar;
    [SerializeField] private float _radius;
    [SerializeField] private Image _hpBar;
    private NavMeshAgent _navMeshAgent;
    private PlayerController _playerController;

    private void Start()
    {
        _hpBar.fillAmount = 1;
    }

    private void Awake()
    {
        _helthBar.Init(_health);
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        _dist = Vector3.Distance(_player.transform.position, transform.position);
        if (_dist > _radius && _playerController == null)
        {
            _navMeshAgent.enabled = false;
        }
        else if (_dist < _radius && _playerController == null)
        {
            _animator.SetBool(AnimWalk, true);
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_player.transform.position);
        }

        if (_playerController != null) _animator.SetTrigger(AnimAttack);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController))
        {
            _animator.SetBool(AnimWalk, false);
            _playerController = playerController;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.TryGetComponent(out PlayerController playerController))
        {
            _animator.SetBool(AnimWalk, true);
            _playerController = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Bullet bullet))
        {
            _health--;
            _helthBar.SetValue(_health);
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void TakePlayer()
    {
        _hpBar.fillAmount -= 0.1f;
    }
}
