using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial
{

    [StructLayout(LayoutKind.Sequential)]
    public struct TransformBuffer
    {
        public Matrix4x4 uModel_0;
        public Matrix4x4 uView_0;
        public Matrix4x4 uProjection_0;
    }
}
