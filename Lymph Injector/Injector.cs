using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace LymphInjector
{
    public static class Injector
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out int lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

        private const uint PROCESS_ALL_ACCESS = 0x001F0FFF;
        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RESERVE = 0x2000;
        private const uint PAGE_READWRITE = 0x04;
        private const uint INFINITE = 0xFFFFFFFF;

        public static string Inject(string dllPath, string processName = "Gorilla Tag")
        {
            if (!File.Exists(dllPath)) return $"DLL not found: {dllPath}";
            var procs = Process.GetProcessesByName(processName);
            if (procs.Length == 0) return $"Process '{processName}' not running.";
            var proc = procs[0];
            IntPtr hProcess = OpenProcess(PROCESS_ALL_ACCESS, false, proc.Id);
            if (hProcess == IntPtr.Zero) return "OpenProcess failed: " + new Win32Exception(Marshal.GetLastWin32Error()).Message;

            byte[] dllBytes = Encoding.ASCII.GetBytes(dllPath + "\0");
            IntPtr alloc = VirtualAllocEx(hProcess, IntPtr.Zero, (uint)dllBytes.Length, MEM_COMMIT | MEM_RESERVE, PAGE_READWRITE);
            if (alloc == IntPtr.Zero) { CloseHandle(hProcess); return "VirtualAllocEx failed."; }

            if (!WriteProcessMemory(hProcess, alloc, dllBytes, (uint)dllBytes.Length, out _)) { CloseHandle(hProcess); return "WriteProcessMemory failed."; }

            IntPtr loadLib = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (loadLib == IntPtr.Zero) { CloseHandle(hProcess); return "GetProcAddress(LoadLibraryA) failed."; }

            IntPtr thread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, loadLib, alloc, 0, out _);
            if (thread == IntPtr.Zero) { CloseHandle(hProcess); return "CreateRemoteThread failed."; }

            WaitForSingleObject(thread, INFINITE);
            CloseHandle(thread);
            CloseHandle(hProcess);
            return $"Injected '{Path.GetFileName(dllPath)}' into '{processName}' (PID {proc.Id}).";
        }

        public static string Eject(string dllName, string processName = "Gorilla Tag")
        {
            // Eject via FreeLibrary in remote process (requires handle to loaded module; best-effort)
            var procs = Process.GetProcessesByName(processName);
            if (procs.Length == 0) return $"Process '{processName}' not running.";
            return $"Eject requested for '{dllName}'. (Remote FreeLibrary requires module handle; use restart or manual unload.)";
        }
    }
}
