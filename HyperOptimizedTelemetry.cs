using System;

public static class TelemetryBuffer
{
    public static byte[] ToBuffer(long reading)
    {
        byte prefix;
        byte[] bytes;
        byte[] buffer = null;

        if (reading > uint.MaxValue || reading < int.MinValue)
        {
            prefix = 256 - 8;
            bytes = BitConverter.GetBytes(reading);
            buffer = new byte[] { prefix, bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5], bytes[6], bytes[7] };
        }
        else if (reading > int.MaxValue)
        {
            prefix = 4;
            bytes = BitConverter.GetBytes((uint)reading);
            buffer = new byte[] { prefix, bytes[0], bytes[1], bytes[2], bytes[3], 0, 0, 0, 0 };
        }
        else if (reading > ushort.MaxValue || reading < short.MinValue)
        {
            prefix = 256 - 4;
            bytes = BitConverter.GetBytes((int)reading);
            buffer = new byte[] { prefix, bytes[0], bytes[1], bytes[2], bytes[3], 0, 0, 0, 0 };
        }
        else if (reading >= 0)
        {
            prefix = 2;
            bytes = BitConverter.GetBytes((ushort)reading);
            buffer = new byte[] { prefix, bytes[0], bytes[1], 0, 0, 0, 0, 0, 0 };
        }
        else if (reading < 0)
        {
            prefix = 256 - 2;
            bytes = BitConverter.GetBytes((short)reading);
            buffer = new byte[] { prefix, bytes[0], bytes[1], 0, 0, 0, 0, 0, 0 };
        }


        return buffer;
    }

    public static long FromBuffer(byte[] buffer)
    {
        long reading = 0;
        byte prefix = buffer[0];

        if (prefix == 2)
        {
            reading = BitConverter.ToUInt16(buffer, 1);
        }
        else if (prefix == 4)
        {
            reading = BitConverter.ToUInt32(buffer, 1);
        }
        else if (prefix == 8)
        {
            reading = BitConverter.ToInt64(buffer, 1);
        }
        else if (prefix == 254)
        {
            reading = BitConverter.ToInt16(buffer, 1);
        }
        else if (prefix == 252)
        {
            reading = BitConverter.ToInt32(buffer, 1);
        }
        else if (prefix == 248)
        {
            reading = BitConverter.ToInt64(buffer, 1);
        }

        return reading;
    }
}
