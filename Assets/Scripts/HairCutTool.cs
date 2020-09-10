using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class HairCutTool : HairTool
{
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        _animator.SetBool(_is_active_hash, true);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        var hit = Physics2D.Raycast(_raycast_point.position, Vector2.right * -1f, _ray_distance, _hair_layer);

        if (hit)
        {
            var point = hit.point;
            var hair_curl = hit.transform;
            var local_point = hair_curl.InverseTransformPoint(point);
            var distance = Mathf.Abs(point.y - hair_curl.position.y);
            var sr = hair_curl.GetComponent<SpriteRenderer>();

            var curl_height = sr.bounds.size.y;
            var relative_pos = distance / curl_height;

            
            Debug.DrawLine(hair_curl.position, hair_curl.TransformPoint(local_point), Color.red);
            //Debug.Break();
            Debug.Log($"<b>HairCutTool.OnTriggerEnter2D</b> distance: {distance}, curl_height: {curl_height}, relative: {relative_pos}", hair_curl);

            var new_relative_height = hair_curl.transform.localScale.y * relative_pos / _hair_manager.MaxHairHeight;

            _hair_manager.ChangeHairHeight(hair_curl, new_relative_height, HairManager.HairToolMode.Cut);
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        _animator.SetBool(_is_active_hash, false);
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && _animator && _animator.GetBool(_is_active_hash))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_raycast_point.position, Vector2.right * -1f * _ray_distance);
        }
    }
}
