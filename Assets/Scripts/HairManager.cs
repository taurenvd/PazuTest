using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class HairManager : MonoBehaviour
{
    public enum HairToolMode
    {
        Unknown,
        Cut,
        Grow,
        Dry
    }

    [Header("Prefs:")]
    [SerializeField] GameObject _hair_curl_pref;

    [Header("Initialization")]
    [SerializeField, Range(.1f, 2)] float _max_hair_height = 1.25f;

    [Header("Components:")]
    [SerializeField] List<Transform> _hair_spawn_points = new List<Transform>();
    [SerializeField] Transform _hair_parent;

    [Header("Runtime:")]
    [SerializeField] List<Transform> _hair_curls;


    #region Properties
    public float MaxHairHeight { get => _max_hair_height; set => _max_hair_height = value; }

    #endregion


    void Start()
    {
        Init();
    }
    void Init()
    {
        foreach (var hair_spawn_point in _hair_spawn_points)
        {
            var curl = Instantiate(_hair_curl_pref, hair_spawn_point.position, hair_spawn_point.rotation, _hair_parent);
            var curl_transform = curl.transform;
            var new_scale = new Vector3(curl_transform.localScale.x, _max_hair_height, curl_transform.localScale.z);

            curl.GetComponent<HingeJoint2D>().connectedBody = _hair_parent.GetComponent<Rigidbody2D>();

            curl_transform.localScale = new_scale;
            _hair_curls.Add(curl_transform);
        }
    }

    public void ChangeHairHeight(Transform hair_curl, float new_relative_height, HairToolMode tool_mode)
    {
        var current = hair_curl.transform.localScale;
        var current_height = current.y / _max_hair_height;

        //Debug.Log($"<b>HairManager.ChangeHairHeight</b> {hair_curl.name}, new_height: {current_height}->{new_relative_height}, mode: {tool_mode}");

        if (tool_mode == HairToolMode.Cut && new_relative_height > current_height) // do not increase hair in "Cut" mode
        {
            return;
        }

        if (tool_mode == HairToolMode.Grow && new_relative_height < current_height) // do not decrease hair in "Grow" mode
        {
            return;
        }
        var clamped = Mathf.Clamp01(new_relative_height);

        //Debug.Log($"<b>HairManager.ChangeHairHeight</b> new_height: {new_relative_height}");

        hair_curl.transform.localScale = new Vector3(current.x, clamped * _max_hair_height, current.z);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (_hair_spawn_points != null && _hair_spawn_points.Count > 0)
        {
            for (int i = 0; i < _hair_spawn_points.Count - 1; i++)
            {
                var current = _hair_spawn_points[i];
                var next = _hair_spawn_points[i + 1];

                Gizmos.DrawWireSphere(current.position, .1f);
                Gizmos.DrawLine(current.position, next.position);
            }
        }
    }
}
