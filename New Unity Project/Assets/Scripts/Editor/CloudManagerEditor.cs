using UnityEditor;

[CustomEditor (typeof (CloudManager))]
public class CloudManagerEditor : Editor
{
    private void OnSceneGUI()
    {
        var manager = target as CloudManager;
        var transform = manager.transform;
        manager.bottomLeft = transform.InverseTransformPoint(
            Handles.PositionHandle(
                transform.TransformPoint(manager.bottomLeft),
                transform.rotation));
        manager.topRight = transform.InverseTransformPoint(
            Handles.PositionHandle(
                transform.TransformPoint(manager.topRight),
                transform.rotation));
        Handles.BeginGUI();
    }
}
