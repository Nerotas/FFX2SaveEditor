using System;
using System.Collections.Generic;
using System.Linq;

namespace FFX2SaveEditor
{
    public class SaveValidationResult
    {
        public bool Passed => Errors.Count == 0;
        public List<string> Errors { get; } = new List<string>();
        public override string ToString() => Passed ? "Validation PASS" : $"Validation FAIL: {string.Join(" | ", Errors)}";
    }

    public static class SaveValidator
    {
        // Validates key invariants of the saved bytes.
        // - CRC at 0x1A and crcOffset matches recomputed CRC16-CCITT of bytes [0x40 .. crcOffset+3]
        // - Story completion progress at 0x0C matches (525 - setBits(storyFlags))/5
        // - Al Bhed primer count and master flag are consistent
        public static SaveValidationResult Validate(byte[] bytes, int crcOffset, int storyOffset)
        {
            var result = new SaveValidationResult();
            if (bytes == null || bytes.Length == 0)
            {
                result.Errors.Add("Empty save bytes");
                return result;
            }

            try
            {
                // 1) CRC check
                if (crcOffset + 1 >= bytes.Length)
                {
                    result.Errors.Add($"crcOffset out of range: 0x{crcOffset:X}");
                }
                else
                {
                    var hasher = new Crc16_CCITT();
                    int start = 0x40;
                    int len = (crcOffset + 4) - start; // inclusive to crcOffset+3
                    if (start < 0 || len < 0 || start + len > bytes.Length)
                    {
                        result.Errors.Add("CRC range out of bounds");
                    }
                    else
                    {
                        var segment = new byte[len];
                        Array.Copy(bytes, start, segment, 0, len);
                        var hash = hasher.ComputeHash(segment);
                        // hasher.Value is ushort
                        ushort expected = (ushort)hasher.Value;
                        ushort at1A = bytes.Length > 0x1B ? BitConverter.ToUInt16(bytes, 0x1A) : (ushort)0xFFFF;
                        ushort atCrc = BitConverter.ToUInt16(bytes, crcOffset);

                        if (at1A != expected)
                            result.Errors.Add($"CRC mismatch at 0x1A: got 0x{at1A:X4}, expected 0x{expected:X4}");
                        if (atCrc != expected)
                            result.Errors.Add($"CRC mismatch at 0x{crcOffset:X}: got 0x{atCrc:X4}, expected 0x{expected:X4}");
                    }
                }

                // 2) Story completion progress
                const int storyLength = 0x4000;
                if (storyOffset + storyLength <= bytes.Length)
                {
                    int ones = 0;
                    for (int i = 0; i < storyLength; i++)
                    {
                        ones += PopCount(bytes[storyOffset + i]);
                    }
                    byte expectedProgress = (byte)((525 - ones) / 5);
                    byte storedProgress = bytes[0x0C];
                    if (storedProgress != expectedProgress)
                    {
                        result.Errors.Add($"Story progress mismatch at 0x0C: got {storedProgress}, expected {expectedProgress}");
                    }
                }
                else
                {
                    result.Errors.Add($"Story flags range out of bounds: offset 0x{storyOffset:X}");
                }

                // 3) Al Bhed primer consistency
                if (bytes.Length > 0x8164 && bytes.Length > 0x11A7)
                {
                    byte p1 = bytes[0x8161];
                    byte p2 = bytes[0x8162];
                    byte p3 = bytes[0x8163];
                    byte p4 = bytes[0x8164];
                    int count = PopCount(p1) + PopCount(p2) + PopCount(p3) + PopCount(p4);
                    byte storedCount = bytes[0x11A6];
                    byte storedMaster = bytes[0x11A7];
                    if (storedCount != (byte)count)
                        result.Errors.Add($"Al Bhed primer count mismatch at 0x11A6: got {storedCount}, expected {count}");
                    byte expectedMaster = (byte)(count == 26 ? 1 : 0);
                    if (storedMaster != expectedMaster)
                        result.Errors.Add($"Al Bhed master flag mismatch at 0x11A7: got {storedMaster}, expected {expectedMaster}");
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Validator exception: {ex.Message}");
            }

            return result;
        }

        private static int PopCount(byte b)
        {
            int c = 0;
            while (b != 0)
            {
                b &= (byte)(b - 1);
                c++;
            }
            return c;
        }
    }
}
