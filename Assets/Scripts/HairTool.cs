using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public abstract class HairTool : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected static int _is_active_hash = Animator.StringToHash("isSelected");

    [Header("Initialization:")]
    [SerializeField] protected LayerMask _hair_layer;
    [SerializeField] protected Vector3 _original_pos;
    [SerializeField] protected float _ray_distance = 1f;

    [Header("Components:")]
    [SerializeField] protected Camera _camera;
    [Space]
    [SerializeField] protected HairManager _hair_manager;
    [Space]
    [SerializeField] protected Transform _raycast_point;
    [Space]
    [SerializeField] protected Animator _animator;

    protected virtual void Awake()
    {
        _original_pos = transform.position;
        _camera = _camera ?? Camera.main;
        _hair_manager = _hair_manager ?? FindObjectOfType<HairManager>();
        _animator = GetComponent<Animator>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        _animator.SetBool(_is_active_hash, true);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        var world_pos = _camera.ScreenToWorldPoint(eventData.position);
        world_pos.z = 0;

        transform.position = world_pos;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.position = _original_pos;
        _animator.SetBool(_is_active_hash, false);
    }

}
