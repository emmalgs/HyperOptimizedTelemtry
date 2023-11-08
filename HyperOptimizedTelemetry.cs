using System;
using System.ComponentModel.DataAnnotations;

public static class TelemetryBuffer
{
    public static byte[] ToBuffer(long reading)
    {
        byte[] byteArray = BitConverter.GetBytes(reading);
        int prefix = 256 - byteArray.Length;
        
        return new byte[] { (byte)prefix, byteArray[0], byteArray[1], byteArray[2], byteArray[3], byteArray[4], byteArray[5], byteArray[6], byteArray[7] };
    }

    public static long FromBuffer(byte[] buffer)
    {
        throw new NotImplementedException("Please implement the static TelemetryBuffer.FromBuffer() method");
    }

    public static byte GetPrefix<T>()
    {
        Type dataType = typeof(T);
        int numberOfBytes = 0;

        if (dataType == typeof(byte))
        {
            numberOfBytes = 1;
        }
        else if (dataType == typeof(short) || dataType == typeof(ushort))
        {
            numberOfBytes = 2;
        }
        else if (dataType == typeof(int) || dataType == typeof(uint))
        {
            numberOfBytes = 4;
        }
        else if (dataType == typeof(long) || dataType == typeof(ulong))
        {
            numberOfBytes = 8;
        }
        return (byte)(dataType.IsSigned() ? 256 - numberOfBytes : numberOfBytes);
    }

    public static bool IsSigned(this Type dataType)
    {
        return dataType == typeof(short) || dataType == typeof(int) || dataType == typeof(long);
    }
}
