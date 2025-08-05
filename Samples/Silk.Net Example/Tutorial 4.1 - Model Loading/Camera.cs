using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial
{
    public struct Camera()
    {
        public Vector3 Position = new Vector3(0.0f, 0.0f, 3.0f);
        public Vector3 Front = new Vector3(0.0f, 0.0f, -1.0f);
        public Vector3 Up = Vector3.UnitY;
        public Vector3 Direction = Vector3.Zero;
        public float Yaw = -90f;
        public float Pitch = 0f;
        public float Zoom = 45f;
    }
}
