using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SlangCube
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TransformBuffer
    {
        public Matrix4x4 uModel;
        public Matrix4x4 uView;
        public Matrix4x4 uProjection;
    }
}
