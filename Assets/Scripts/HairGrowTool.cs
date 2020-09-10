using UnityEngine;
using UnityEngine.EventSystems;

public class HairGrowTool : HairTool
{

    [SerializeField, Range(0.0005f, 0.05f)] float _grow_factor = 0.005f;

    public float GrowFactor { get => _grow_factor; set => _grow_factor = value; }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        var hit = Physics2D.Raycast(_raycast_point.position, Vector2.right * -1, _ray_distance, _hair_layer);

        if (hit)
        {
            var point = hit.point;
            var hair_curl = hit.transform;

            Debug.Log($"<b>HairGrowTool.OnTriggerEnter2D</b> {point}", hair_curl);

            var new_relative_height = (hair_curl.localScale.y + _grow_factor * Random.value * 2) / _hair_manager.MaxHairHeight;

            _hair_manager.ChangeHairHeight(hair_curl, new_relative_height: new_relative_height, tool_mode: HairManager.HairToolMode.Grow);
        }
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_raycast_point.position, Vector2.right * -1f * _ray_distance);
        }
    }
}
