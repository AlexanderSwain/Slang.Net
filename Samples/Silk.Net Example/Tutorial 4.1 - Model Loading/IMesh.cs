using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tutorial
{
    public interface IMesh : IDisposable
    {
        void Draw(IRenderer renderer, TransformBuffer transform);
    }
}
