using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFX2SaveEditor
{
    public static class Utils
    {
        public static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static byte[] BoolsToBytes(bool[] bools)
        {
            if (bools == null) return Array.Empty<byte>();
            int byteLen = (bools.Length + 7) / 8;
            var bytes = new byte[byteLen];
            for (int i = 0; i < bools.Length; i++)
            {
                if (bools[i])
                {
                    int b = i / 8;
                    int bit = i % 8;
                    bytes[b] |= (byte)(1 << bit);
                }
            }
            return bytes;
        }
    }

    public static class Extensions
    {
        public static void Write(this BinaryWriter bw, bool[] bools)
        {
            bw.Write(Utils.BoolsToBytes(bools));
        }

        public static void Write(this BinaryWriter bw, int offset, short value)
        {
            bw.BaseStream.Seek(offset, SeekOrigin.Begin);
            bw.Write(value);
        }
    }
}
