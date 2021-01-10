using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace OpenCvDisparityMapGenerator
{
    static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string path);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr module, string name);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr module);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
#if false
                // Just a dummy test if we can load the native library
                {
                    var current_directory = Directory.GetCurrentDirectory();
                    var native_library_name = "OpenCvDisparityMapGeneratorNative.dll";
                    var native_library = NativeMethods.LoadLibrary(native_library_name);
                    if (native_library == IntPtr.Zero)
                    {
                        var error = NativeMethods.GetLastError();
                        throw new ApplicationException(string.Format("Failed to load native library {0}, current working directory is {1}", native_library_name, current_directory));
                    }
                    NativeMethods.FreeLibrary(native_library);
                }
#endif
                base.OnStartup(e);
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
