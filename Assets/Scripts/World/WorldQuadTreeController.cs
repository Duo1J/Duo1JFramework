using Duo1JFramework;
using Duo1JFramework.Actor;
using Duo1JFramework.DataStructure;
using Duo1JFramework.World;
using UnityEngine;

[RequireComponent(typeof(GizmosBounds))]
public class WorldQuadTreeController : BaseMono
{
    private GizmosBounds bounds;

    public Vector3 size;
    public int depth = 4;
    public Transform itemPar;

    private void Start()
    {
        bounds = GetComponent<GizmosBounds>();

        WorldQuadManager.Instance.CreateTree(bounds.Bounds, depth);
        itemPar.ChildForeach((go) =>
        {
            go.GetOrAddComponent<WorldQuadItem>();
        });
    }

    [ContextMenu("重建")]
    public void Rebuild()
    {
        WorldQuadManager.Instance.RebuildTree(bounds.Bounds, depth);
    }
}
