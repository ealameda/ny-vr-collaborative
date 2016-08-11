using UnityEngine;
using System.Collections.Generic;

namespace Leap.Unity.DetectionExamples
{

    public class PinchDraw : MonoBehaviour
    {

        [Tooltip("Each pinch detector can draw one line at a time.")]
        [SerializeField]
        public PinchDetector[] _pinchDetectors;

        [SerializeField]
        public Material _material;

        [SerializeField]
        public Color _drawColor = Color.white;

        [SerializeField]
        public float _smoothingDelay = 0.01f;

        [SerializeField]
        public float _drawRadius = 0.002f;

        [SerializeField]
        public int _drawResolution = 8;

        [SerializeField]
        public float _minSegmentLength = 0.005f;

        public float beginDrawDuration = 0.0f;

        private float pinchStartTime;

        private bool beganPinching;
        private bool lineStarted;

        public MeshRenderer[] renderers;

        public DrawState[] _drawStates;

        private PhotonView myPhotonView;

        private Color baseColor;

        public Color DrawColor
        {
            get
            {
                return _drawColor;
            }
            set
            {
                _drawColor = value;
            }
        }

        public float DrawRadius
        {
            get
            {
                return _drawRadius;
            }
            set
            {
                _drawRadius = value;
            }
        }

        void OnValidate()
        {
            _drawRadius = Mathf.Max(0, _drawRadius);
            _drawResolution = Mathf.Clamp(_drawResolution, 3, 24);
            _minSegmentLength = Mathf.Max(0, _minSegmentLength);
        }

        void Awake()
        {
            if (_pinchDetectors.Length == 0)
            {
                Debug.LogWarning("No pinch detectors were specified!  PinchDraw can not draw any lines without PinchDetectors.");
            }
        }

        void Start()
        {
            _drawStates = new DrawState[_pinchDetectors.Length];
            renderers = new MeshRenderer[_pinchDetectors.Length];
            for (int i = 0; i < _pinchDetectors.Length; i++)
            {
                _drawStates[i] = new DrawState(this);
                renderers[i] = _pinchDetectors[i].drawingTool.GetComponent<MeshRenderer>();
            }

        }

        void Update()
        {
            for (int i = 0; i < _pinchDetectors.Length; i++)
            {
                var renderer = renderers[i];
                var detector = _pinchDetectors[i];
                var drawState = _drawStates[i];
                Color color = new Color();
                if (renderer != null)
                {
                    color = renderer.material.color;
                }

                if (detector != null)
                {
                    if (detector.DidStartPinch)
                    {
                        pinchStartTime = Time.time;
                        beganPinching = true;
                        renderer.enabled = true;
                    }

                    if (detector.DidEndPinch)
                    {
                        if (lineStarted)
                        {
                            drawState.FinishLine();
                            myPhotonView.RPC("FinishLine", PhotonTargets.OthersBuffered, i);
                            lineStarted = false;
                        }
                        renderer.material.color = baseColor;
                        renderer.enabled = false;

                    }

                    if (detector.IsPinching)
                    {
                        if (beganPinching && ((Time.time - pinchStartTime) >= beginDrawDuration))
                        {
                            if (lineStarted)
                            {
                                drawState.UpdateLine(detector.Position);
                                myPhotonView.RPC("UpdateLine", PhotonTargets.OthersBuffered, detector.Position, i);
                            }
                            else
                            {
                                color.g = 50f;
                                renderer.material.color = color;
                                drawState.BeginNewLine();
                                myPhotonView.RPC("BeginDraw", PhotonTargets.OthersBuffered, i);
                                lineStarted = true;
                            }
                        }
                        else
                        {
                            color.a += 0.03f;
                            color.g += .01f;
                            renderer.material.color = color;
                        }
                    }
                }
            }
        }

        public void AddPinchDetector(PinchDetector pinchDetector)
        {
            if (_pinchDetectors == null || _pinchDetectors.Length <= 0)
            {
                _pinchDetectors = new PinchDetector[2];
                _drawStates = new DrawState[2];
                renderers = new MeshRenderer[2];
                _pinchDetectors[0] = pinchDetector;
                _drawStates[0] = new DrawState(this);
                renderers[0] = pinchDetector.drawingTool.GetComponent<MeshRenderer>();
                baseColor = renderers[0].material.color;
            }
            else
            {
                _pinchDetectors[1] = pinchDetector;
                _drawStates[1] = new DrawState(this);
                renderers[1] = pinchDetector.drawingTool.GetComponent<MeshRenderer>();
            }
            myPhotonView = gameObject.GetComponent<PhotonView>();
            myPhotonView.RPC("MakeDrawStateRPCs", PhotonTargets.AllBuffered, _pinchDetectors.Length);
        }

        public class DrawState
        {
            private List<Vector3> _vertices = new List<Vector3>();
            private List<int> _tris = new List<int>();
            private List<Vector2> _uvs = new List<Vector2>();
            private List<Color> _colors = new List<Color>();

            private PinchDraw _parent;

            private int _rings = 0;

            private Vector3 _prevRing0 = Vector3.zero;
            private Vector3 _prevRing1 = Vector3.zero;

            private Vector3 _prevNormal0 = Vector3.zero;

            private Mesh _mesh;
            private SmoothedVector3 _smoothedPosition;

            public DrawState(PinchDraw parent)
            {
                _parent = parent;

                _smoothedPosition = new SmoothedVector3();
                _smoothedPosition.delay = parent._smoothingDelay;
                _smoothedPosition.reset = true;
            }

            public GameObject BeginNewLine()
            {
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
                lineObj.AddComponent<MeshRenderer>().sharedMaterial = _parent._material;
                lineObj.tag = "Line";

                return lineObj;
            }

            public void UpdateLine(Vector3 position)
            {
                _smoothedPosition.Update(position, Time.deltaTime);

                bool shouldAdd = false;

                shouldAdd |= _vertices.Count == 0;
                shouldAdd |= Vector3.Distance(_prevRing0, _smoothedPosition.value) >= _parent._minSegmentLength;

                if (shouldAdd)
                {
                    addRing(_smoothedPosition.value);
                    updateMesh();
                }
            }

            public void FinishLine()
            {
                _mesh.Optimize();
                _mesh.UploadMeshData(true);
            }

            private void updateMesh()
            {
                _mesh.SetVertices(_vertices);
                _mesh.SetColors(_colors);
                _mesh.SetUVs(0, _uvs);
                _mesh.SetIndices(_tris.ToArray(), MeshTopology.Triangles, 0);
                _mesh.RecalculateBounds();
                _mesh.RecalculateNormals();
            }

            private void addRing(Vector3 ringPosition)
            {
                _rings++;

                if (_rings == 1)
                {
                    addVertexRing();
                    addVertexRing();
                    addTriSegment();
                }

                addVertexRing();
                addTriSegment();

                Vector3 ringNormal = Vector3.zero;
                if (_rings == 2)
                {
                    Vector3 direction = ringPosition - _prevRing0;
                    float angleToUp = Vector3.Angle(direction, Vector3.up);

                    if (angleToUp < 10 || angleToUp > 170)
                    {
                        ringNormal = Vector3.Cross(direction, Vector3.right);
                    }
                    else
                    {
                        ringNormal = Vector3.Cross(direction, Vector3.up);
                    }

                    ringNormal = ringNormal.normalized;

                    _prevNormal0 = ringNormal;
                }
                else if (_rings > 2)
                {
                    Vector3 prevPerp = Vector3.Cross(_prevRing0 - _prevRing1, _prevNormal0);
                    ringNormal = Vector3.Cross(prevPerp, ringPosition - _prevRing0).normalized;
                }

                if (_rings == 2)
                {
                    updateRingVerts(0,
                                    _prevRing0,
                                    ringPosition - _prevRing1,
                                    _prevNormal0,
                                    0);
                }

                if (_rings >= 2)
                {
                    updateRingVerts(_vertices.Count - _parent._drawResolution,
                                    ringPosition,
                                    ringPosition - _prevRing0,
                                    ringNormal,
                                    0);
                    updateRingVerts(_vertices.Count - _parent._drawResolution * 2,
                                    ringPosition,
                                    ringPosition - _prevRing0,
                                    ringNormal,
                                    1);
                    updateRingVerts(_vertices.Count - _parent._drawResolution * 3,
                                    _prevRing0,
                                    ringPosition - _prevRing1,
                                    _prevNormal0,
                                    1);
                }

                _prevRing1 = _prevRing0;
                _prevRing0 = ringPosition;

                _prevNormal0 = ringNormal;
            }

            private void addVertexRing()
            {
                for (int i = 0; i < _parent._drawResolution; i++)
                {
                    _vertices.Add(Vector3.zero);  //Dummy vertex, is updated later
                    _uvs.Add(new Vector2(i / (_parent._drawResolution - 1.0f), 0));
                    _colors.Add(_parent._drawColor);
                }
            }

            //Connects the most recently added vertex ring to the one before it
            private void addTriSegment()
            {
                for (int i = 0; i < _parent._drawResolution; i++)
                {
                    int i0 = _vertices.Count - 1 - i;
                    int i1 = _vertices.Count - 1 - ((i + 1) % _parent._drawResolution);

                    _tris.Add(i0);
                    _tris.Add(i1 - _parent._drawResolution);
                    _tris.Add(i0 - _parent._drawResolution);

                    _tris.Add(i0);
                    _tris.Add(i1);
                    _tris.Add(i1 - _parent._drawResolution);
                }
            }

            private void updateRingVerts(int offset, Vector3 ringPosition, Vector3 direction, Vector3 normal, float radiusScale)
            {
                direction = direction.normalized;
                normal = normal.normalized;

                for (int i = 0; i < _parent._drawResolution; i++)
                {
                    float angle = 360.0f * (i / (float)(_parent._drawResolution));
                    Quaternion rotator = Quaternion.AngleAxis(angle, direction);
                    Vector3 ringSpoke = rotator * normal * _parent._drawRadius * radiusScale;
                    _vertices[offset + i] = ringPosition + ringSpoke;
                }
            }
        }
    }
}
