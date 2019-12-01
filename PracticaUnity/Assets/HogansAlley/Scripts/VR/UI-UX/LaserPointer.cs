/************************************************************************************

Copyright (c) Facebook Technologies, LLC and its affiliates. All rights reserved.  

See SampleFramework license.txt for license terms.  Unless required by applicable law 
or agreed to in writing, the sample code is provided “AS IS” WITHOUT WARRANTIES OR 
CONDITIONS OF ANY KIND, either express or implied.  See the license for specific 
language governing permissions and limitations under the license.

************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace FS.VR
{

    public class LaserPointer : OVRCursor
    {
        public enum LaserBeamBehavior
        {
            On,        // laser beam always on
            Off,        // laser beam always off
            OnWhenHitTarget,  // laser beam only activates when hit valid target
        }

        public GameObject cursorVisual;
        public float maxLength = 10.0f;
        public float originForwardOffset = 0.05f;

        private LaserBeamBehavior _laserBeamBehavior;

        public LaserBeamBehavior laserBeamBehavior
        {
            set
            {
                _laserBeamBehavior = value;
                if (laserBeamBehavior == LaserBeamBehavior.Off || laserBeamBehavior == LaserBeamBehavior.OnWhenHitTarget)
                {
                    lineRenderer.enabled = false;
                }
                else
                {
                    lineRenderer.enabled = true;
                }
            }
            get
            {
                return _laserBeamBehavior;
            }
        }
        private Vector3 _startPoint;
        private Vector3 _forward;
        private Vector3 _endPoint;
        private bool _hitTarget;
        private LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            lineRenderer.useWorldSpace = true;

            if (cursorVisual) cursorVisual.SetActive(false);
        }

        public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
        {
            _startPoint = start;
            _endPoint = dest;
            _hitTarget = true;
        }

        public override void SetCursorRay(Transform t)
        {
            _startPoint = t.position;
            _forward = t.forward;
            _hitTarget = false;
        }

        private void LateUpdate()
        {
            lineRenderer.SetPosition(0, _startPoint + _forward * originForwardOffset);
            if (_hitTarget)
            {
                float distance = Vector3.Distance(_startPoint, _endPoint);
                float rayLength = distance > maxLength ? maxLength : distance;

                lineRenderer.SetPosition(1, _startPoint + _forward * rayLength);
                UpdateLaserBeam(_startPoint + _forward * originForwardOffset, _startPoint + _forward * rayLength);
                if (cursorVisual)
                {
                    cursorVisual.transform.position = _endPoint;
                    cursorVisual.SetActive(true);
                }
            }
            else
            {
                UpdateLaserBeam(_startPoint + _forward * originForwardOffset, _startPoint + maxLength * _forward);
                lineRenderer.SetPosition(1, _startPoint + maxLength * _forward);
                if (cursorVisual) cursorVisual.SetActive(false);
            }
        }

        // make laser beam a behavior with a prop that enables or disables
        private void UpdateLaserBeam(Vector3 start, Vector3 end)
        {
            if (laserBeamBehavior == LaserBeamBehavior.Off)
            {
                return;
            }
            else if (laserBeamBehavior == LaserBeamBehavior.On)
            {
                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);
            }
            else if (laserBeamBehavior == LaserBeamBehavior.OnWhenHitTarget)
            {
                if (_hitTarget)
                {
                    if (!lineRenderer.enabled)
                    {
                        lineRenderer.enabled = true;
                        lineRenderer.SetPosition(0, start);
                        lineRenderer.SetPosition(1, end);
                    }
                }
                else
                {
                    if (lineRenderer.enabled)
                    {
                        lineRenderer.enabled = false;
                    }
                }
            }
        }

        void OnDisable()
        {
            if (cursorVisual) cursorVisual.SetActive(false);
        }
    }
}