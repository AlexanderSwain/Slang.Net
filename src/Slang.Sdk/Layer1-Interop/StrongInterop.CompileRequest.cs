using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Slang.Sdk.Interop;

/// <summary>
/// Strongly-typed wrapper methods for CompileRequest APIs.
/// </summary>
internal unsafe partial class StrongInterop
{
    internal class CompileRequest
    {
        /// <summary>
        /// Creates a compile request with strongly-typed handle.
        /// </summary>
        internal static CompileRequestHandle Create(SessionHandle parentSession, out string? error)
        {
            char* pError = null;
            var handle = SlangNativeInterop.CompileRequest_Create(parentSession, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
            return new CompileRequestHandle(handle);
        }

        /// <summary>
        /// Releases a compile request with strongly-typed handle.
        /// </summary>
        internal static void Release(CompileRequestHandle compileRequest, out string? error)
        {
            char* pError = null;
            SlangNativeInterop.CompileRequest_Release(compileRequest, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
        }

        /// <summary>
        /// Gets the native compile request handle.
        /// </summary>
        internal static nint GetNative(CompileRequestHandle compileRequest, out string? error)
        {
            char* pError = null;
            nint result = SlangNativeInterop.CompileRequest_GetNative(compileRequest, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
            return result;
        }

        /// <summary>
        /// Adds a code generation target to the compile request.
        /// </summary>
        internal static void AddCodeGenTarget(CompileRequestHandle compileRequest, int target, out string? error)
        {
            char* pError = null;
            SlangNativeInterop.CompileRequest_AddCodeGenTarget(compileRequest, target, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
        }

        /// <summary>
        /// Adds an entry point to the compile request.
        /// </summary>
        internal static int AddEntryPoint(
            CompileRequestHandle compileRequest,
            int translationUnitIndex,
            string name,
            int stage,
            out string? error)
        {
            char* pError = null;
            char* pName = (char*)Utf8StringMarshaller.ConvertToUnmanaged(name);

            try
            {
                int result = SlangNativeInterop.CompileRequest_AddEntryPoint(
                    compileRequest, translationUnitIndex, pName, stage, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                return result;
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pName);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds an entry point with generic arguments to the compile request.
        /// </summary>
        internal static int AddEntryPointEx(
            CompileRequestHandle compileRequest,
            int translationUnitIndex,
            string name,
            int stage,
            string[] genericArgs,
            out string? error)
        {
            char* pError = null;
            char* pName = (char*)Utf8StringMarshaller.ConvertToUnmanaged(name);
            char*[] managedGenericArgs = new char*[genericArgs.Length];

            try
            {
                // Convert string array to char** for native interop
                for (int i = 0; i < genericArgs.Length; i++)
                {
                    managedGenericArgs[i] = (char*)Utf8StringMarshaller.ConvertToUnmanaged(genericArgs[i]);
                }

                int result = SlangNativeInterop.CompileRequest_AddEntryPointEx(
                    compileRequest, translationUnitIndex, pName, stage,
                    genericArgs.Length, managedGenericArgs, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                return result;
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pName);
                for (int i = 0; i < genericArgs.Length; i++)
                {
                    if (managedGenericArgs[i] != null)
                        Utf8StringMarshaller.Free((byte*)managedGenericArgs[i]);
                }
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds a library reference to the compile request.
        /// </summary>
        internal static void AddLibraryReference(
            CompileRequestHandle compileRequest,
            CompileRequestHandle baseRequest,
            string libName,
            out string? error)
        {
            char* pError = null;
            char* pLibName = (char*)Utf8StringMarshaller.ConvertToUnmanaged(libName);

            try
            {
                SlangNativeInterop.CompileRequest_AddLibraryReference(
                    compileRequest, baseRequest, pLibName, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pLibName);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds a preprocessor define to the compile request.
        /// </summary>
        internal static void AddPreprocessorDefine(
            CompileRequestHandle compileRequest,
            string key,
            string value,
            out string? error)
        {
            char* pError = null;
            char* pKey = (char*)Utf8StringMarshaller.ConvertToUnmanaged(key);
            char* pValue = (char*)Utf8StringMarshaller.ConvertToUnmanaged(value);

            try
            {
                SlangNativeInterop.CompileRequest_AddPreprocessorDefine(
                    compileRequest, pKey, pValue, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pKey);
                Utf8StringMarshaller.Free((byte*)pValue);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds a reference to the compile request.
        /// </summary>
        internal static void AddRef(CompileRequestHandle compileRequest, out string? error)
        {
            char* pError = null;
            SlangNativeInterop.CompileRequest_AddRef(compileRequest, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
        }

        /// <summary>
        /// Adds a search path to the compile request.
        /// </summary>
        internal static void AddSearchPath(
            CompileRequestHandle compileRequest,
            string searchDir,
            out string? error)
        {
            char* pError = null;
            char* pSearchDir = (char*)Utf8StringMarshaller.ConvertToUnmanaged(searchDir);

            try
            {
                SlangNativeInterop.CompileRequest_AddSearchPath(compileRequest, pSearchDir, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pSearchDir);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds a target capability to the compile request.
        /// </summary>
        internal static void AddTargetCapability(
            CompileRequestHandle compileRequest,
            int targetIndex,
            int capability,
            out string? error)
        {
            char* pError = null;
            SlangNativeInterop.CompileRequest_AddTargetCapability(
                compileRequest, targetIndex, capability, &pError);
            error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            SlangNativeInterop.FreeChar(&pError);
        }

        /// <summary>
        /// Adds a translation unit to the compile request.
        /// </summary>
        internal static int AddTranslationUnit(
            CompileRequestHandle compileRequest,
            int language,
            string name,
            out string? error)
        {
            char* pError = null;
            char* pName = (char*)Utf8StringMarshaller.ConvertToUnmanaged(name);

            try
            {
                int result = SlangNativeInterop.CompileRequest_AddTranslationUnit(
                    compileRequest, language, pName, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
                return result;
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pName);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds a preprocessor define for a specific translation unit.
        /// </summary>
        internal static void AddTranslationUnitPreprocessorDefine(
            CompileRequestHandle compileRequest,
            int translationUnitIndex,
            string key,
            string value,
            out string? error)
        {
            char* pError = null;
            char* pKey = (char*)Utf8StringMarshaller.ConvertToUnmanaged(key);
            char* pValue = (char*)Utf8StringMarshaller.ConvertToUnmanaged(value);

            try
            {
                SlangNativeInterop.CompileRequest_AddTranslationUnitPreprocessorDefine(
                    compileRequest, translationUnitIndex, pKey, pValue, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pKey);
                Utf8StringMarshaller.Free((byte*)pValue);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds source from a blob to a translation unit.
        /// </summary>
        internal static void AddTranslationUnitSourceBlob(
            CompileRequestHandle compileRequest,
            int translationUnitIndex,
            string path,
            nint sourceBlob,
            out string? error)
        {
            char* pError = null;
            char* pPath = (char*)Utf8StringMarshaller.ConvertToUnmanaged(path);

            try
            {
                SlangNativeInterop.CompileRequest_AddTranslationUnitSourceBlob(
                    compileRequest, translationUnitIndex, pPath, sourceBlob, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pPath);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds source from a file to a translation unit.
        /// </summary>
        internal static void AddTranslationUnitSourceFile(
            CompileRequestHandle compileRequest,
            int translationUnitIndex,
            string path,
            out string? error)
        {
            char* pError = null;
            char* pPath = (char*)Utf8StringMarshaller.ConvertToUnmanaged(path);

            try
            {
                SlangNativeInterop.CompileRequest_AddTranslationUnitSourceFile(
                    compileRequest, translationUnitIndex, pPath, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pPath);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds source from a string to a translation unit.
        /// </summary>
        internal static void AddTranslationUnitSourceString(
            CompileRequestHandle compileRequest,
            int translationUnitIndex,
            string path,
            string source,
            out string? error)
        {
            char* pError = null;
            char* pPath = (char*)Utf8StringMarshaller.ConvertToUnmanaged(path);
            char* pSource = (char*)Utf8StringMarshaller.ConvertToUnmanaged(source);

            try
            {
                SlangNativeInterop.CompileRequest_AddTranslationUnitSourceString(
                    compileRequest, translationUnitIndex, pPath, pSource, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pPath);
                Utf8StringMarshaller.Free((byte*)pSource);
                SlangNativeInterop.FreeChar(&pError);
            }
        }

        /// <summary>
        /// Adds source from a string span to a translation unit.
        /// </summary>
        internal static void AddTranslationUnitSourceStringSpan(
            CompileRequestHandle compileRequest,
            int translationUnitIndex,
            string path,
            string sourceBegin,
            string sourceEnd,
            out string? error)
        {
            char* pError = null;
            char* pPath = (char*)Utf8StringMarshaller.ConvertToUnmanaged(path);
            char* pSourceBegin = (char*)Utf8StringMarshaller.ConvertToUnmanaged(sourceBegin);
            char* pSourceEnd = (char*)Utf8StringMarshaller.ConvertToUnmanaged(sourceEnd);

            try
            {
                SlangNativeInterop.CompileRequest_AddTranslationUnitSourceStringSpan(
                    compileRequest, translationUnitIndex, pPath, pSourceBegin, pSourceEnd, &pError);
                error = Utf8StringMarshaller.ConvertToManaged((byte*)pError);
            }
            finally
            {
                Utf8StringMarshaller.Free((byte*)pPath);
                Utf8StringMarshaller.Free((byte*)pSourceBegin);
                Utf8StringMarshaller.Free((byte*)pSourceEnd);
                SlangNativeInterop.FreeChar(&pError);
            }
        }
    }
}