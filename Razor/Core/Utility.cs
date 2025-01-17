#region license
// Razor: An Ultima Online Assistant
// Copyright (c) 2022 Razor Development Community on GitHub <https://github.com/markdwags/Razor>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
#endregion

using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;

namespace Assistant
{
    public class Utility
    {
        private static Random m_Random = new Random();

        public static int Random(int min, int max)
        {
            return m_Random.Next(max - min + 1) + min;
        }

        public static int Random(int num)
        {
            return m_Random.Next(num);
        }

        public static bool InRange(IPoint2D from, IPoint2D to, int range)
        {
            return (to.X >= (from.X - range))
                   && (to.X <= (from.X + range))
                   && (to.Y >= (from.Y - range))
                   && (to.Y <= (from.Y + range));
        }

        public static int Distance(int fx, int fy, int tx, int ty)
        {
            int xDelta = Math.Abs(fx - tx);
            int yDelta = Math.Abs(fy - ty);

            return (xDelta > yDelta ? xDelta : yDelta);
        }

        public static int Distance(IPoint2D from, IPoint2D to)
        {
            int xDelta = Math.Abs(from.X - to.X);
            int yDelta = Math.Abs(from.Y - to.Y);

            return (xDelta > yDelta ? xDelta : yDelta);
        }

        public static double DistanceSqrt(IPoint2D from, IPoint2D to)
        {
            float xDelta = Math.Abs(from.X - to.X);
            float yDelta = Math.Abs(from.Y - to.Y);

            return Math.Sqrt(xDelta * xDelta + yDelta * yDelta);
        }

        public static void Offset(Direction d, ref int x, ref int y)
        {
            switch (d & Direction.Mask)
            {
                case Direction.North:
                    --y;
                    break;
                case Direction.South:
                    ++y;
                    break;
                case Direction.West:
                    --x;
                    break;
                case Direction.East:
                    ++x;
                    break;
                case Direction.Right:
                    ++x;
                    --y;
                    break;
                case Direction.Left:
                    --x;
                    ++y;
                    break;
                case Direction.Down:
                    ++x;
                    ++y;
                    break;
                case Direction.Up:
                    --x;
                    --y;
                    break;
            }
        }

        public static void FormatBuffer(TextWriter output, Stream input, int length)
        {
            output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

            int byteIndex = 0;

            int whole = length >> 4;
            int rem = length & 0xF;

            for (int i = 0; i < whole; ++i, byteIndex += 16)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(16);

                for (int j = 0; j < 16; ++j)
                {
                    int c = input.ReadByte();

                    bytes.Append(c.ToString("X2"));

                    if (j != 7)
                    {
                        bytes.Append(' ');
                    }
                    else
                    {
                        bytes.Append("  ");
                    }

                    if (c >= 0x20 && c < 0x80)
                    {
                        chars.Append((char) c);
                    }
                    else
                    {
                        chars.Append('.');
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                output.Write("  ");
                output.WriteLine(chars.ToString());
            }

            if (rem != 0)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(rem);

                for (int j = 0; j < 16; ++j)
                {
                    if (j < rem)
                    {
                        int c = input.ReadByte();

                        bytes.Append(c.ToString("X2"));

                        if (j != 7)
                        {
                            bytes.Append(' ');
                        }
                        else
                        {
                            bytes.Append("  ");
                        }

                        if (c >= 0x20 && c < 0x80)
                        {
                            chars.Append((char) c);
                        }
                        else
                        {
                            chars.Append('.');
                        }
                    }
                    else
                    {
                        bytes.Append("   ");
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                if (rem <= 8)
                    output.Write("   ");
                else
                    output.Write("  ");
                output.WriteLine(chars.ToString());
            }
        }

        public static unsafe void FormatBuffer(TextWriter output, byte* buff, int length)
        {
            output.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            output.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");

            int byteIndex = 0;

            int whole = length >> 4;
            int rem = length & 0xF;

            for (int i = 0; i < whole; ++i, byteIndex += 16)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(16);

                for (int j = 0; j < 16; ++j)
                {
                    int c = *buff++;

                    bytes.Append(c.ToString("X2"));

                    if (j != 7)
                    {
                        bytes.Append(' ');
                    }
                    else
                    {
                        bytes.Append("  ");
                    }

                    if (c >= 0x20 && c < 0x80)
                    {
                        chars.Append((char) c);
                    }
                    else
                    {
                        chars.Append('.');
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                output.Write("  ");
                output.WriteLine(chars.ToString());
            }

            if (rem != 0)
            {
                StringBuilder bytes = new StringBuilder(49);
                StringBuilder chars = new StringBuilder(rem);

                for (int j = 0; j < 16; ++j)
                {
                    if (j < rem)
                    {
                        int c = *buff++;

                        bytes.Append(c.ToString("X2"));

                        if (j != 7)
                        {
                            bytes.Append(' ');
                        }
                        else
                        {
                            bytes.Append("  ");
                        }

                        if (c >= 0x20 && c < 0x80)
                        {
                            chars.Append((char) c);
                        }
                        else
                        {
                            chars.Append('.');
                        }
                    }
                    else
                    {
                        bytes.Append("   ");
                    }
                }

                output.Write(byteIndex.ToString("X4"));
                output.Write("   ");
                output.Write(bytes.ToString());
                if (rem <= 8)
                    output.Write("   ");
                else
                    output.Write("  ");
                output.WriteLine(chars.ToString());
            }
        }

        private static char[] pathChars = new char[] {'\\', '/'};

        public static string PathDisplayStr(string path, int maxLen)
        {
            if (path == null || path.Length <= maxLen || path.Length < 5)
                return path;

            int first = (maxLen - 3) / 2;
            int last = path.LastIndexOfAny(pathChars);
            if (last == -1 || last < maxLen / 4)
                last = path.Length - first;
            first = maxLen - last - 3;
            if (first < 0)
                first = 1;
            if (last < first)
                last = first;

            return $"{path.Substring(0, first)}...{path.Substring(last)}";
        }

        public static string FormatSize(long size)
        {
            if (size < 1024) // 1 K
                return $"{size:#,##0} B";
            else if (size < 1048576) // 1 M
                return $"{size / 1024.0:#,###.0} KB";
            else
                return $"{size / 1048576.0:#,###.0} MB";
        }

        public static string FormatTime(int sec)
        {
            int m = sec / 60;
            int h = m / 60;
            m = m % 60;
            return $"{h:#0}:{m:00}:{sec % 60:00}";
        }

        public static string FormatTimeMS(int ms)
        {
            int s = ms / 1000;
            int m = s / 60;
            int h = m / 60;

            ms = ms % 1000;
            s = s % 60;
            m = m % 60;

            if (h > 0 || m > 55)
                return $"{h:#0}:{m:00}:{s:00}.{ms:000}";
            else
                return $"{m:00}:{s:00}.{ms:000}";
        }

        public static int ToInt32(string str, int def)
        {
            if (str == null)
                return def;

            if (str == null)
                return def;

            int val;
            if (str.StartsWith("0x"))
            {
                if (int.TryParse(str.Substring(2), NumberStyles.HexNumber, Engine.Culture, out val))
                    return val;
            }
            else if (int.TryParse(str, out val))
                return val;

            return def;
        }

        public static uint ToUInt32(string str, uint def)
        {
            if (str == null)
                return def;

            uint val;
            if (str.StartsWith("0x"))
            {
                if (uint.TryParse(str.Substring(2), NumberStyles.HexNumber, Engine.Culture, out val))
                    return val;
            }
            else if (uint.TryParse(str, out val))
                return val;

            return def;
        }

        public static double ToDouble(string str, double def)
        {
            if (str == null)
                return def;

            if (double.TryParse(str, out double d))
                return d;
            return def;
        }

        public static ushort ToUInt16(string str, ushort def)
        {
            if (str == null)
                return def;

            ushort val;
            if (str.StartsWith("0x"))
            {
                if (ushort.TryParse(str.Substring(2), NumberStyles.HexNumber, Engine.Culture, out val))
                    return val;
            }
            else if (ushort.TryParse(str, out val))
                return val;

            return def;
        }

        public static void LaunchBrowser(string url)
        {
            try
            {
                Process.Start(url);

                /*if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    Process.Start("xdg-open", url);
                }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(null, ex.Message, "Unable to open directory", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
            }
        }
    }
}