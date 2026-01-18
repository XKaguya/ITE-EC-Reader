using System.Runtime.InteropServices;

namespace ITE_EC_Reader.Generic;

public static class WinRing0
{
    public static bool WinRingInitOk = false;

    [DllImport("WinRing0x64.dll")]
    public static extern DllErrorCode GetDllStatus();

    [DllImport("WinRing0x64.dll")]
    public static extern ulong GetDllVersion(ref byte major, ref byte minor, ref byte revision, ref byte release);

    [DllImport("WinRing0x64.dll")]
    public static extern ulong GetDriverVersion(ref byte major, ref byte minor, ref byte revision, ref byte release);

    [DllImport("WinRing0x64.dll")]
    public static extern DriverType GetDriverType();

    [DllImport("WinRing0x64.dll")]
    public static extern bool InitializeOls();

    [DllImport("WinRing0x64.dll")]
    public static extern void DeinitializeOls();

    [DllImport("WinRing0x64.dll")]
    public static extern byte ReadIoPortByte(uint address);

    [DllImport("WinRing0x64.dll")]
    public static extern void WriteIoPortByte(uint port, byte value);

    public static void DirectEcWrite(byte ecAddrPort, byte ecDataPort, ushort addr, byte data)
    {
        WriteIoPortByte(ecAddrPort, 0x2E);
        WriteIoPortByte(ecDataPort, 0x11);
        WriteIoPortByte(ecAddrPort, 0x2F);
        WriteIoPortByte(ecDataPort, (byte)((addr >> 8) & 0xFF));

        WriteIoPortByte(ecAddrPort, 0x2E);
        WriteIoPortByte(ecDataPort, 0x10);
        WriteIoPortByte(ecAddrPort, 0x2F);
        WriteIoPortByte(ecDataPort, (byte)(addr & 0xFF));

        WriteIoPortByte(ecAddrPort, 0x2E);
        WriteIoPortByte(ecDataPort, 0x12);
        WriteIoPortByte(ecAddrPort, 0x2F);
        WriteIoPortByte(ecDataPort, data);
    }

    public static void DirectEcWriteArray(byte ecAddrPort, byte ecDataPort, ushort baseAddr, byte[] data)
    {
        for (var i = 0; i < data.Length; i++)
        {
            var addr = (ushort)(baseAddr + i);
            WriteIoPortByte(ecAddrPort, 0x2E);
            WriteIoPortByte(ecDataPort, 0x11);
            WriteIoPortByte(ecAddrPort, 0x2F);
            WriteIoPortByte(ecDataPort, (byte)((addr >> 8) & 0xFF));

            WriteIoPortByte(ecAddrPort, 0x2E);
            WriteIoPortByte(ecDataPort, 0x10);
            WriteIoPortByte(ecAddrPort, 0x2F);
            WriteIoPortByte(ecDataPort, (byte)(addr & 0xFF));

            WriteIoPortByte(ecAddrPort, 0x2E);
            WriteIoPortByte(ecDataPort, 0x12);
            WriteIoPortByte(ecAddrPort, 0x2F);
            WriteIoPortByte(ecDataPort, data[i]);
        }
    }

    public static byte DirectEcRead(byte ecAddrPort, byte ecDataPort, ushort addr)
    {
        WriteIoPortByte(ecAddrPort, 0x2E);
        WriteIoPortByte(ecDataPort, 0x11);
        WriteIoPortByte(ecAddrPort, 0x2F);
        WriteIoPortByte(ecDataPort, (byte)((addr >> 8) & 0xFF));

        WriteIoPortByte(ecAddrPort, 0x2E);
        WriteIoPortByte(ecDataPort, 0x10);
        WriteIoPortByte(ecAddrPort, 0x2F);
        WriteIoPortByte(ecDataPort, (byte)(addr & 0xFF));

        WriteIoPortByte(ecAddrPort, 0x2E);
        WriteIoPortByte(ecDataPort, 0x12);
        WriteIoPortByte(ecAddrPort, 0x2F);
        return ReadIoPortByte(ecDataPort);
    }

    public static byte[] DirectEcReadArray(byte ecAddrPort, byte ecDataPort, ushort baseAddr, int size)
    {
        var buffer = new byte[size];
        for (var i = 0; i < size; i++)
        {
            var addr = (ushort)(baseAddr + i);
            WriteIoPortByte(ecAddrPort, 0x2E);
            WriteIoPortByte(ecDataPort, 0x11);
            WriteIoPortByte(ecAddrPort, 0x2F);
            WriteIoPortByte(ecDataPort, (byte)((addr >> 8) & 0xFF));

            WriteIoPortByte(ecAddrPort, 0x2E);
            WriteIoPortByte(ecDataPort, 0x10);
            WriteIoPortByte(ecAddrPort, 0x2F);
            WriteIoPortByte(ecDataPort, (byte)(addr & 0xFF));

            WriteIoPortByte(ecAddrPort, 0x2E);
            WriteIoPortByte(ecDataPort, 0x12);
            WriteIoPortByte(ecAddrPort, 0x2F);
            buffer[i] = ReadIoPortByte(ecDataPort);
        }
        return buffer;
    }
}

public enum DllErrorCode
{
    OlsDllNoError = 0,
    OlsDllUnsupportedPlatform = 1,
    OlsDllDriverNotLoaded = 2,
    OlsDllDriverNotFound = 3,
    OlsDllDriverUnloaded = 4,
    OlsDllDriverNotLoadedOnNetwork = 5,
    OlsDllUnknownError = 9
}

public enum DriverType
{
    OlsDriverTypeUnknown = 0,
    OlsDriverTypeWin9X = 1,
    OlsDriverTypeWinNt = 2,
    OlsDriverTypeWinNt4 = 3,
    OlsDriverTypeWinNtX64 = 4,
    OlsDriverTypeWinNtIa64 = 5
}