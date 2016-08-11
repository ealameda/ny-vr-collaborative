using UnityEngine;
using Leap.Unity;
using System.Collections.Generic;

public class DrawStateRPC
{
  private List<Vector3> _vertices = new List<Vector3>();
  private List<int> _tris = new List<int>();
  private List<Vector2> _uvs = new List<Vector2>();
  private List<Color> _colors = new List<Color>();

  private Material _material;
  private float _minSegmentLength;
  private int _drawResolution;
  private Color _drawColor;
  private float _drawRadius;
  private float _smoothingDelay;

  private int _rings = 0;

  private Vector3 _prevRing0 = Vector3.zero;
  private Vector3 _prevRing1 = Vector3.zero;

  private Vector3 _prevNormal0 = Vector3.zero;

  private Mesh _mesh;
  private SmoothedVector3 _smoothedPosition;

  public DrawStateRPC(Material material, float minSegmentLength, int drawResolution, Color drawColor, float drawRadius, float smoothingDelay) {
  	_material = material;
  	_minSegmentLength = minSegmentLength;
  	_drawResolution = drawResolution;
  	_drawColor = drawColor;
  	_drawRadius = drawRadius;
  	_smoothingDelay = smoothingDelay;

    _smoothedPosition = new SmoothedVector3();
    _smoothedPosition.delay = _smoothingDelay;
    _smoothedPosition.reset = true;
  }

  public GameObject BeginNewLine() {
    _rings = 0;
    _vertices.Clear();
    _tris.Clear();
    _uvs.Clear();
    _colors.Clear();

    _smoothedPosition.reset = true;

    _mesh = new Mesh();
    _mesh.name = "Line Mesh";
    _mesh.MarkDynamic();

    GameObject lineObj = new GameObject("Line Object");
    lineObj.transform.position = Vector3.zero;
    lineObj.transform.rotation = Quaternion.identity;
    lineObj.transform.localScale = Vector3.one;
    lineObj.AddComponent<MeshFilter>().mesh = _mesh;
    lineObj.AddComponent<MeshRenderer>().sharedMaterial = _material;
    lineObj.tag = "RemoteLine";

    return lineObj;
  }

  public void UpdateLine(Vector3 position) {
    _smoothedPosition.Update(position, Time.deltaTime);

    bool shouldAdd = false;

    shouldAdd |= _vertices.Count == 0;
    shouldAdd |= Vector3.Distance(_prevRing0, _smoothedPosition.value) >= _minSegmentLength;

    if (shouldAdd) {
      addRing(_smoothedPosition.value);
      updateMesh();
    }
  }

  public void FinishLine() {
    _mesh.Optimize();
    _mesh.UploadMeshData(true);
  }

  private void updateMesh() {
    _mesh.SetVertices(_vertices);
    _mesh.SetColors(_colors);
    _mesh.SetUVs(0, _uvs);
    _mesh.SetIndices(_tris.ToArray(), MeshTopology.Triangles, 0);
    _mesh.RecalculateBounds();
    _mesh.RecalculateNormals();
  }

  private void addRing(Vector3 ringPosition) {
    _rings++;

    if (_rings == 1) {
      addVertexRing();
      addVertexRing();
      addTriSegment();
    }

    addVertexRing();
    addTriSegment();

    Vector3 ringNormal = Vector3.zero;
    if (_rings == 2) {
      Vector3 direction = ringPosition - _prevRing0;
      float angleToUp = Vector3.Angle(direction, Vector3.up);

      if (angleToUp < 10 || angleToUp > 170) {
        ringNormal = Vector3.Cross(direction, Vector3.right);
      } else {
        ringNormal = Vector3.Cross(direction, Vector3.up);
      }

      ringNormal = ringNormal.normalized;

      _prevNormal0 = ringNormal;
    } else if (_rings > 2) {
      Vector3 prevPerp = Vector3.Cross(_prevRing0 - _prevRing1, _prevNormal0);
      ringNormal = Vector3.Cross(prevPerp, ringPosition - _prevRing0).normalized;
    }

    if (_rings == 2) {
      updateRingVerts(0,
                      _prevRing0,
                      ringPosition - _prevRing1,
                      _prevNormal0,
                      0);
    }

    if (_rings >= 2) {
      updateRingVerts(_vertices.Count - _drawResolution,
                      ringPosition,
                      ringPosition - _prevRing0,
                      ringNormal,
                      0);
      updateRingVerts(_vertices.Count - _drawResolution * 2,
                      ringPosition,
                      ringPosition - _prevRing0,
                      ringNormal,
                      1);
      updateRingVerts(_vertices.Count - _drawResolution * 3,
                      _prevRing0,
                      ringPosition - _prevRing1,
                      _prevNormal0,
                      1);
    }

    _prevRing1 = _prevRing0;
    _prevRing0 = ringPosition;

    _prevNormal0 = ringNormal;
  }

  private void addVertexRing() {
    for (int i = 0; i < _drawResolution; i++) {
      _vertices.Add(Vector3.zero);  //Dummy vertex, is updated later
      _uvs.Add(new Vector2(i / (_drawResolution - 1.0f), 0));
      _colors.Add(_drawColor);
    }
  }

  //Connects the most recently added vertex ring to the one before it
  private void addTriSegment() {
    for (int i = 0; i < _drawResolution; i++) {
      int i0 = _vertices.Count - 1 - i;
      int i1 = _vertices.Count - 1 - ((i + 1) % _drawResolution);

      _tris.Add(i0);
      _tris.Add(i1 - _drawResolution);
      _tris.Add(i0 - _drawResolution);

      _tris.Add(i0);
      _tris.Add(i1);
      _tris.Add(i1 - _drawResolution);
    }
  }

  private void updateRingVerts(int offset, Vector3 ringPosition, Vector3 direction, Vector3 normal, float radiusScale) {
    direction = direction.normalized;
    normal = normal.normalized;

    for (int i = 0; i < _drawResolution; i++) {
      float angle = 360.0f * (i / (float)(_drawResolution));
      Quaternion rotator = Quaternion.AngleAxis(angle, direction);
      Vector3 ringSpoke = rotator * normal * _drawRadius * radiusScale;
      _vertices[offset + i] = ringPosition + ringSpoke;
    }
  }
}
