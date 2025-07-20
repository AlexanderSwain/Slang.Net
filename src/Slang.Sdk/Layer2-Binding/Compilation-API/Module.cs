using Slang.Sdk.Interop;
using static Slang.Sdk.Interop.SlangNativeInterop;
using static Slang.Sdk.Interop.Utilities;

namespace Slang.Sdk.Binding;

internal unsafe sealed class Module
{
    internal Session Parent { get; }
    internal Interop.ModuleHandle Handle { get; }

    public Module(Session parent, string moduleName, string modulePath, string shaderSource)
    {
        Parent = parent;
        fixed (char* pName = moduleName)
        fixed (char* pPath = moduleName)
        fixed (char* pSource = moduleName)
        {
            Handle = new SlangModuleHandle(
                Module_Create(Parent.Handle, pName, pPath, pSource)
            );

            if (Handle.IsInvalid)
                throw new SlangException(SlangResult.Fail, $"Failed to create Slang module: {GetLastError() ?? "<No error was returned from Slang>"}");
        }
    }

    ~Module()
    {
        Handle?.Dispose();
    }
}



///// <summary>
///// Represents a Slang module containing shader programs and entry points.
///// </summary>
//public sealed class Module : IDisposable
//{
//    private readonly SlangModuleHandle _moduleHandle;
//    private readonly Session _session;
//    private readonly string _name;
//    private readonly string _path;
//    private ShaderProgram? _program;
//    private bool _disposed;

//    /// <summary>
//    /// Gets the name of this module.
//    /// </summary>
//    public string Name => _name;

//    /// <summary>
//    /// Gets the file path of this module.
//    /// </summary>
//    public string Path => _path;

//    /// <summary>
//    /// Gets the session that owns this module.
//    /// </summary>
//    public Session Session => _session;

//    /// <summary>
//    /// Gets the shader program for this module.
//    /// </summary>
//    public ShaderProgram Program => _program ??= CreateProgram();

//    internal Module(Session session, SlangModuleHandle moduleHandle, string name, string path)
//    {
//        _session = session ?? throw new ArgumentNullException(nameof(session));
//        _moduleHandle = moduleHandle ?? throw new ArgumentNullException(nameof(moduleHandle));
//        _name = name ?? throw new ArgumentNullException(nameof(name));
//        _path = path ?? throw new ArgumentNullException(nameof(path));
//    }

//    /// <summary>
//    /// Finds an entry point by name in this module.
//    /// </summary>
//    /// <param name="entryPointName">The name of the entry point to find.</param>
//    /// <returns>The entry point handle, or IntPtr.Zero if not found.</returns>
//    /// <exception cref="SlangException">Thrown if the entry point is not found.</exception>
//    public unsafe EntryPoint FindEntryPoint(string entryPointName)
//    {
//        ObjectDisposedException.ThrowIfDisposed(_disposed, this);
//        ArgumentNullException.ThrowIfNull(entryPointName);

//        var entryPointNamePtr = StringMarshaling.ToUtf8(entryPointName);
//        try
//        {
//            var entryPointPtr = SlangNativeInterop.FindEntryPoint(_moduleHandle, entryPointNamePtr);
            
//            if (entryPointPtr == IntPtr.Zero)
//            {
//                var errorMessage = _session.GetLastError();
//                throw new SlangException(SlangResult.Fail, $"Entry point '{entryPointName}' not found in module '{_name}': {errorMessage ?? "Unknown error"}");
//            }

//            return new EntryPoint(this, entryPointPtr, entryPointName);
//        }
//        finally
//        {
//            StringMarshaling.FreeUtf8(entryPointNamePtr);
//        }
//    }

//    /// <summary>
//    /// Gets parameter information for a specific entry point.
//    /// </summary>
//    /// <param name="entryPoint">The entry point to get parameters for.</param>
//    /// <returns>An array of parameter information.</returns>
//    public unsafe ParameterInfo[] GetParameterInfo(EntryPoint entryPoint)
//    {
//        ObjectDisposedException.ThrowIfDisposed(_disposed, this);
//        ArgumentNullException.ThrowIfNull(entryPoint);

//        void* parameterInfo = null;
//        int parameterCount = 0;

//        SlangNativeInterop.GetParameterInfo(entryPoint.Handle, &parameterInfo, &parameterCount);

//        if (parameterCount == 0 || parameterInfo == null)
//            return Array.Empty<ParameterInfo>();

//        // For now, return empty array. In a full implementation, we would:
//        // 1. Marshal the native parameter info structures
//        // 2. Convert them to managed ParameterInfo objects
//        return Array.Empty<ParameterInfo>();
//    }

//    private ShaderProgram CreateProgram()
//    {
//        ObjectDisposedException.ThrowIfDisposed(_disposed, this);

//        var programPtr = SlangNativeInterop.CreateProgram(_moduleHandle);
        
//        if (programPtr == IntPtr.Zero)
//        {
//            var errorMessage = _session.GetLastError();
//            throw new SlangException(SlangResult.Fail, $"Failed to create program for module '{_name}': {errorMessage ?? "Unknown error"}");
//        }

//        var programHandle = new SlangProgramHandle(programPtr);
//        return new ShaderProgram(this, programHandle);
//    }

//    internal SlangModuleHandle Handle => _moduleHandle;

//    public void Dispose()
//    {
//        if (!_disposed)
//        {
//            _program?.Dispose();
//            _moduleHandle?.Dispose();
//            _disposed = true;
//        }
//    }
//}

///// <summary>
///// Represents parameter information for a shader entry point.
///// </summary>
//public class ParameterInfo
//{
//    /// <summary>
//    /// The name of the parameter.
//    /// </summary>
//    public string Name { get; }

//    /// <summary>
//    /// The type of the parameter.
//    /// </summary>
//    public string Type { get; }

//    /// <summary>
//    /// The binding location of the parameter.
//    /// </summary>
//    public int BindingLocation { get; }

//    internal ParameterInfo(string name, string type, int bindingLocation)
//    {
//        Name = name;
//        Type = type;
//        BindingLocation = bindingLocation;
//    }
//}