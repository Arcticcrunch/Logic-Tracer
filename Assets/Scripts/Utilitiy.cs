using System;
using System.Numerics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO.Ports;
//using CSCore;
//using CSCore.DSP;
//using CSCore.SoundIn;
//using CSCore.Streams;
//using ut = CSCore.Utils;

using UnityEngine;

namespace Utility
{
    public static class BinaryOperations
    {
        public enum Byte2Index
        {
            First, Second
        }
        public enum Byte4Index
        {
            First, Second, Third, Fourth
        }
        public enum Byte8Index
        {
            First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth
        }


        public static byte GetByteFromsShort(short number, Byte2Index byteIndex)
        {
            byte result = 0;
            switch (byteIndex)
            {
                case Byte2Index.First:
                    result = (byte)number;
                    break;
                case Byte2Index.Second:
                    result = (byte)(number >> 8);
                    break;
                default:
                    break;
            }
            return result;
        }
        public static byte GetByteFromUShort(ushort number, Byte2Index byteIndex)
        {
            byte result = 0;
            switch (byteIndex)
            {
                case Byte2Index.First:
                    result = (byte)number;
                    break;
                case Byte2Index.Second:
                    result = (byte)(number >> 8);
                    break;
                default:
                    break;
            }
            return result;
        }
        public static byte GetByteFromInt(int number, Byte4Index byteIndex)
        {
            byte result = 0;
            switch (byteIndex)
            {
                case Byte4Index.First:
                    result = (byte)number;
                    break;
                case Byte4Index.Second:
                    result = (byte)(number >> 8);
                    break;
                case Byte4Index.Third:
                    result = (byte)(number >> 16);
                    break;
                case Byte4Index.Fourth:
                    result = (byte)(number >> 24);
                    break;
                default:
                    break;
            }
            return result;
        }
        public static byte GetByteFromUInt(uint number, Byte4Index byteIndex)
        {
            byte result = 0;
            switch (byteIndex)
            {
                case Byte4Index.First:
                    result = (byte)number;
                    break;
                case Byte4Index.Second:
                    result = (byte)(number >> 8);
                    break;
                case Byte4Index.Third:
                    result = (byte)(number >> 16);
                    break;
                case Byte4Index.Fourth:
                    result = (byte)(number >> 24);
                    break;
                default:
                    break;
            }
            return result;
        }
        public static byte GetByteFromLong(long number, Byte8Index byteIndex)
        {
            byte result = 0;
            switch (byteIndex)
            {
                case Byte8Index.First:
                    result = (byte)number;
                    break;
                case Byte8Index.Second:
                    result = (byte)(number >> 8);
                    break;
                case Byte8Index.Third:
                    result = (byte)(number >> 16);
                    break;
                case Byte8Index.Fourth:
                    result = (byte)(number >> 24);
                    break;
                case Byte8Index.Fifth:
                    result = (byte)(number >> 32);
                    break;
                case Byte8Index.Sixth:
                    result = (byte)(number >> 40);
                    break;
                case Byte8Index.Seventh:
                    result = (byte)(number >> 48);
                    break;
                case Byte8Index.Eighth:
                    result = (byte)(number >> 56);
                    break;
                default:
                    break;
            }
            return result;
        }
        public static byte GetByteFromULong(ulong number, Byte8Index byteIndex)
        {
            byte result = 0;
            switch (byteIndex)
            {
                case Byte8Index.First:
                    result = (byte)number;
                    break;
                case Byte8Index.Second:
                    result = (byte)(number >> 8);
                    break;
                case Byte8Index.Third:
                    result = (byte)(number >> 16);
                    break;
                case Byte8Index.Fourth:
                    result = (byte)(number >> 24);
                    break;
                case Byte8Index.Fifth:
                    result = (byte)(number >> 32);
                    break;
                case Byte8Index.Sixth:
                    result = (byte)(number >> 40);
                    break;
                case Byte8Index.Seventh:
                    result = (byte)(number >> 48);
                    break;
                case Byte8Index.Eighth:
                    result = (byte)(number >> 56);
                    break;
                default:
                    break;
            }
            return result;
        }

    }
    public static class FourierTransform
    {
        /// <summary>
        /// Fourier transformation direction.
        /// </summary>
        public enum Direction
        {
            /// <summary>
            ///   Forward direction of Fourier transformation.
            /// </summary>
            /// 
            Forward = 1,

            /// <summary>
            ///   Backward direction of Fourier transformation.
            /// </summary>
            /// 
            Backward = -1
        };
        /// <summary>
        /// One dimensional Discrete Fourier Transform.
        /// </summary>
        /// 
        /// <param name="data">Data to transform.</param>
        /// <param name="direction">Transformation direction.</param>
        /// 
        public static void DFT(Complex[] data, Direction direction)
        {
            int n = data.Length;
            double arg, cos, sin;
            var dst = new Complex[n];

            // for each destination element
            for (int i = 0; i < dst.Length; i++)
            {
                dst[i] = Complex.Zero;

                arg = -(int)direction * 2.0 * System.Math.PI * (double)i / (double)n;

                // sum source elements
                for (int j = 0; j < data.Length; j++)
                {
                    cos = System.Math.Cos(j * arg);
                    sin = System.Math.Sin(j * arg);

                    double re = data[j].Real * cos - data[j].Imaginary * sin;
                    double im = data[j].Real * sin + data[j].Imaginary * cos;

                    dst[i] += new Complex(re, im);
                }
            }

            // copy elements
            if (direction == Direction.Forward)
            {
                // devide also for forward transform
                for (int i = 0; i < data.Length; i++)
                    data[i] /= n;
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                    data[i] = dst[i];
            }
        }
        /// <summary>
        /// Two dimensional Discrete Fourier Transform.
        /// </summary>
        /// 
        /// <param name="data">Data to transform.</param>
        /// <param name="direction">Transformation direction.</param>
        /// 
        public static void DFT2(Complex[,] data, Direction direction)
        {
            int n = data.GetLength(0);	// rows
            int m = data.GetLength(1);	// columns
            double arg, cos, sin;
            var dst = new Complex[System.Math.Max(n, m)];

            // process rows
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < dst.Length; j++)
                {
                    dst[j] = Complex.Zero;

                    arg = -(int)direction * 2.0 * System.Math.PI * (double)j / (double)m;

                    // sum source elements
                    for (int k = 0; k < m; k++)
                    {
                        cos = System.Math.Cos(k * arg);
                        sin = System.Math.Sin(k * arg);

                        double re = data[i, k].Real * cos - data[i, k].Imaginary * sin;
                        double im = data[i, k].Real * sin + data[i, k].Imaginary * cos;

                        dst[j] += new Complex(re, im);
                    }
                }

                // copy elements
                if (direction == Direction.Forward)
                {
                    // devide also for forward transform
                    for (int j = 0; j < dst.Length; j++)
                        data[i, j] = dst[j] / m;
                }
                else
                {
                    for (int j = 0; j < dst.Length; j++)
                        data[i, j] = dst[j];
                }
            }

            // process columns
            for (int j = 0; j < m; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    dst[i] = Complex.Zero;

                    arg = -(int)direction * 2.0 * System.Math.PI * (double)i / (double)n;

                    // sum source elements
                    for (int k = 0; k < n; k++)
                    {
                        cos = System.Math.Cos(k * arg);
                        sin = System.Math.Sin(k * arg);

                        double re = data[k, j].Real * cos - data[k, j].Imaginary * sin;
                        double im = data[k, j].Real * sin + data[k, j].Imaginary * cos;

                        dst[i] += new Complex(re, im);
                    }
                }

                // copy elements
                if (direction == Direction.Forward)
                {
                    // devide also for forward transform
                    for (int i = 0; i < dst.Length; i++)
                        data[i, j] = dst[i] / n;
                }
                else
                {
                    for (int i = 0; i < dst.Length; i++)
                        data[i, j] = dst[i];
                }
            }
        }
        /// <summary>
        /// One dimensional Fast Fourier Transform.
        /// </summary>
        /// 
        /// <param name="data">Data to transform.</param>
        /// <param name="direction">Transformation direction.</param>
        /// 
        /// <remarks><para><note>The method accepts <paramref name="data"/> array of 2<sup>n</sup> size
        /// only, where <b>n</b> may vary in the [1, 14] range.</note></para></remarks>
        /// 
        /// <exception cref="ArgumentException">Incorrect data length.</exception>
        /// 
        public static void FFT(Complex[] data, Direction direction)
        {
            int n = data.Length;
            int m = Utility.Math.Log2(n);

            // reorder data first
            ReorderData(data);

            // compute FFT
            int tn = 1, tm;

            for (int k = 1; k <= m; k++)
            {
                Complex[] rotation = FourierTransform.GetComplexRotation(k, direction);

                tm = tn;
                tn <<= 1;

                for (int i = 0; i < tm; i++)
                {
                    Complex t = rotation[i];

                    for (int even = i; even < n; even += tn)
                    {
                        int odd = even + tm;
                        Complex ce = data[even];
                        Complex co = data[odd];

                        double tr = co.Real * t.Real - co.Imaginary * t.Imaginary;
                        double ti = co.Real * t.Imaginary + co.Imaginary * t.Real;

                        data[even] += new Complex(tr, ti);
                        data[odd] = new Complex(ce.Real - tr, ce.Imaginary - ti);
                    }
                }
            }

            if (direction == Direction.Forward)
            {
                for (int i = 0; i < data.Length; i++)
                    data[i] /= (double)n;
            }
        }
        /// <summary>
        /// Two dimensional Fast Fourier Transform.
        /// </summary>
        /// 
        /// <param name="data">Data to transform.</param>
        /// <param name="direction">Transformation direction.</param>
        /// 
        /// <remarks><para><note>The method accepts <paramref name="data"/> array of 2<sup>n</sup> size
        /// only in each dimension, where <b>n</b> may vary in the [1, 14] range. For example, 16x16 array
        /// is valid, but 15x15 is not.</note></para></remarks>
        /// 
        /// <exception cref="ArgumentException">Incorrect data length.</exception>
        /// 
        public static void FFT2(Complex[,] data, Direction direction)
        {
            int k = data.GetLength(0);
            int n = data.GetLength(1);

            // check data size
            if (!Utility.Math.IsPowerOf2(k) || !Utility.Math.IsPowerOf2(n))
                throw new ArgumentException("The matrix rows and columns must be a power of 2.");

            if (k < minLength || k > maxLength || n < minLength || n > maxLength)
                throw new ArgumentException("Incorrect data length.");

            // process rows
            var row = new Complex[n];

            for (int i = 0; i < k; i++)
            {
                // copy row
                for (int j = 0; j < row.Length; j++)
                    row[j] = data[i, j];

                // transform it
                FourierTransform.FFT(row, direction);

                // copy back
                for (int j = 0; j < row.Length; j++)
                    data[i, j] = row[j];
            }

            // process columns
            var col = new Complex[k];

            for (int j = 0; j < n; j++)
            {
                // copy column
                for (int i = 0; i < k; i++)
                    col[i] = data[i, j];

                // transform it
                FourierTransform.FFT(col, direction);

                // copy back
                for (int i = 0; i < k; i++)
                    data[i, j] = col[i];
            }
        }

        #region Private Region

        private const int minLength = 2;
        private const int maxLength = 16384;
        private const int minBits = 1;
        private const int maxBits = 14;
        private static int[][] reversedBits = new int[maxBits][];
        private static Complex[,][] complexRotation = new Complex[maxBits, 2][];
        // Get array, indicating which data members should be swapped before FFT
        private static int[] GetReversedBits(int numberOfBits)
        {
            if ((numberOfBits < minBits) || (numberOfBits > maxBits))
                throw new ArgumentOutOfRangeException();

            // check if the array is already calculated
            if (reversedBits[numberOfBits - 1] == null)
            {
                int n = Utility.Math.Pow2(numberOfBits);
                int[] rBits = new int[n];

                // calculate the array
                for (int i = 0; i < n; i++)
                {
                    int oldBits = i;
                    int newBits = 0;

                    for (int j = 0; j < numberOfBits; j++)
                    {
                        newBits = (newBits << 1) | (oldBits & 1);
                        oldBits = (oldBits >> 1);
                    }
                    rBits[i] = newBits;
                }
                reversedBits[numberOfBits - 1] = rBits;
            }
            return reversedBits[numberOfBits - 1];
        }
        // Get rotation of complex number
        private static Complex[] GetComplexRotation(int numberOfBits, Direction direction)
        {
            int directionIndex = (direction == Direction.Forward) ? 0 : 1;

            // check if the array is already calculated
            if (complexRotation[numberOfBits - 1, directionIndex] == null)
            {
                int n = 1 << (numberOfBits - 1);
                double uR = 1.0;
                double uI = 0.0;
                double angle = System.Math.PI / n * (int)direction;
                double wR = System.Math.Cos(angle);
                double wI = System.Math.Sin(angle);
                double t;
                Complex[] rotation = new Complex[n];

                for (int i = 0; i < n; i++)
                {
                    rotation[i] = new Complex(uR, uI);
                    t = uR * wI + uI * wR;
                    uR = uR * wR - uI * wI;
                    uI = t;
                }

                complexRotation[numberOfBits - 1, directionIndex] = rotation;
            }
            return complexRotation[numberOfBits - 1, directionIndex];
        }
        // Reorder data for FFT using
        private static void ReorderData(Complex[] data)
        {
            int len = data.Length;

            // check data length
            if ((len < minLength) || (len > maxLength) || (!Utility.Math.IsPowerOf2(len)))
                throw new ArgumentException("Incorrect data length.");

            int[] rBits = GetReversedBits(Utility.Math.Log2(len));

            for (int i = 0; i < len; i++)
            {
                int s = rBits[i];

                if (s > i)
                {
                    Complex t = data[i];
                    data[i] = data[s];
                    data[s] = t;
                }
            }
        }

        #endregion
    }
    public static class Math
    {
        public const float PI = 3.14159265359f;
        public const float E = 2.71828182846f;
        public static int Sign(float value)
        {
            if (value >= 0)
                return 1;
            else return -1;
        }
        public static float Abs(float value)
        {
            if (value >= 0)
                return value;
            return value * (-1);
        }
        public static int Abs(int value)
        {
            if (value >= 0)
                return value;
            return value * (-1);
        }
        public struct Vector2I
        {
            public int x, y;

            public Vector2I(int xPos, int yPos)
            {
                this.x = xPos;
                this.y = yPos;
            }

            public static Vector2I Zero
            {
                get { return new Vector2I(0, 0); }
            }
            public static Vector2I Up
            {
                get { return new Vector2I(0, 1); }
            }
            public static Vector2I Right
            {
                get { return new Vector2I(1, 0); }
            }
            public static Vector2I Down
            {
                get { return new Vector2I(0, -1); }
            }
            public static Vector2I Left
            {
                get { return new Vector2I(-1, 0); }
            }

            public override string ToString()
            {
                return "Vector2I(" + x + ", " + y + ")";
            }

            public static int GetNumberIn2dArray(Vector2I pos, int arrayWidth)
            {
                return (pos.y * arrayWidth) + pos.x;
            }

            public static Vector2I operator +(Vector2I a, Vector2I b)
            {
                return new Vector2I(a.x + b.x, a.y + b.y);
            }
            public static Vector2I operator /(Vector2I a, int b)
            {
                return new Vector2I(a.x / b, a.y / b);
            }
        }
        public static byte ClampToByteRange(float value)
        {
            if (value < 0)
                return 0;
            else if (value > 255)
                return 255;
            return (byte)value;
        }
        public static byte ClampToByteRange(int value)
        {
            if (value < 0)
                return 0;
            else if (value > 255)
                return 255;
            return (byte)value;
        }
        public static byte ClampIndexInArray(byte index, byte arrayLength)
        {
            if (index >= arrayLength) return (byte)(index - arrayLength);
            else if (index < 0) return (byte)(index + arrayLength);
            return index;
        }
        public static int ClampIndexInArray(int index, int arrayLength)
        {
            if (index >= arrayLength) return index - arrayLength;
            else if (index < 0) return index + arrayLength;
            return index;
        }
        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
        public static float Clamp01(float value)
        {
            if (value < 0)
                return 0;
            if (value > 1)
                return 1;
            return value;
        }
        public static float ClampMax(float max, float value)
        {
            if (value > max)
                return max;
            return value;
        }
        public static int ClampMax(int max, int value)
        {
            if (value > max)
                return max;
            return value;
        }
        public static float ClampMin(float min, float value)
        {
            if (value < min)
                return min;
            return value;
        }
        public static int ClampMin(int min, int value)
        {
            if (value < min)
                return min;
            return value;
        }
        public static int ClampLoop(int min, int max, int value)
        {
            if (value >= min && value <= max)
                return value;
            int range = max - min;
            if (value < min)
            {
                int t = max - value;
                int rest = (t + 1) % (range + 1);
                if (rest == 0) return min;
                return max - (rest - 1);
            }
            else
            {
                int t = value - min;
                int rest = (t + 1) % (range + 1);
                if (rest == 0) return max;
                return min + (rest - 1);
            }
        }
        public static int Floor(float value)
        {
            return (int)value;
        }
        public static float GetDecimalPart(float value)
        {
            return value - (int)value;
        }
        public static float InverseLerp(float min, float max, float value)
        {
            return (value - min) / (max - min);
        }
        public static float Lerp(float min, float max, float value)
        {
            return (max - min) * Clamp01(value) + min;
        }
        public static int Log2(int x)
        {
            if (x <= 65536)
            {
                if (x <= 256)
                {
                    if (x <= 16)
                    {
                        if (x <= 4)
                        {
                            if (x <= 2)
                            {
                                if (x <= 1)
                                    return 0;
                                return 1;
                            }
                            return 2;
                        }
                        if (x <= 8)
                            return 3;
                        return 4;
                    }
                    if (x <= 64)
                    {
                        if (x <= 32)
                            return 5;
                        return 6;
                    }
                    if (x <= 128)
                        return 7;
                    return 8;
                }
                if (x <= 4096)
                {
                    if (x <= 1024)
                    {
                        if (x <= 512)
                            return 9;
                        return 10;
                    }
                    if (x <= 2048)
                        return 11;
                    return 12;
                }
                if (x <= 16384)
                {
                    if (x <= 8192)
                        return 13;
                    return 14;
                }
                if (x <= 32768)
                    return 15;
                return 16;
            }

            if (x <= 16777216)
            {
                if (x <= 1048576)
                {
                    if (x <= 262144)
                    {
                        if (x <= 131072)
                            return 17;
                        return 18;
                    }
                    if (x <= 524288)
                        return 19;
                    return 20;
                }
                if (x <= 4194304)
                {
                    if (x <= 2097152)
                        return 21;
                    return 22;
                }
                if (x <= 8388608)
                    return 23;
                return 24;
            }
            if (x <= 268435456)
            {
                if (x <= 67108864)
                {
                    if (x <= 33554432)
                        return 25;
                    return 26;
                }
                if (x <= 134217728)
                    return 27;
                return 28;
            }
            if (x <= 1073741824)
            {
                if (x <= 536870912)
                    return 29;
                return 30;
            }
            return 31;
        }
        public static bool IsPowerOf2(int x)
        {
            return (x > 0) ? ((x & (x - 1)) == 0) : false;
        }
        public static int Pow2(int power)
        {
            return ((power >= 0) && (power <= 30)) ? (1 << power) : 0;
        }
        public static bool IsEven(int number)
        {
            if (number % 2 == 0)
                return true;
            else return false;
        }
        public static bool IsInArrayRange(float value, float lowerRange, float higherRange)
        {
            if (value >= lowerRange && value < higherRange)
                return true;
            else return false;
        }
        public static bool IsInArrayRange(int value, int lowerRange, int higherRange)
        {
            if (value >= lowerRange && value < higherRange)
                return true;
            else return false;
        }
        public static int ClosesHigherPowerOfTwo(int value)
        {
            if (value < 1 || value > 2147483647)
                return 0;
            for (int i = 1; i < 30; i++)
            {
                if (GetPowerOfTwo(i) >= value)
                    return i;
            }
            return 0;
        }
        public static int ClosesHigherPowerOfTwo(int value, out int difference)
        {
            difference = 0;
            if (value < 1 || value > 2147483647)
                return 0;
            for (int i = 1; i < 30; i++)
            {
                int powerOfTwo = GetPowerOfTwo(i);
                if (powerOfTwo >= value)
                {
                    difference = powerOfTwo - value;
                    return i;
                }
            }
            return 0;
        }
        public static int GetPowerOfTwo(int power)
        {
            if (power < 0 || power > 30)
            {
                return 0;
            }
            if (power == 0)
                return 1;
            else if (power == 1)
                return 2;
            else if (power == 2)
                return 4;
            else if (power == 3)
                return 8;
            else if (power == 4)
                return 16;
            else if (power == 5)
                return 32;
            else if (power == 6)
                return 64;
            else if (power == 7)
                return 128;
            else if (power == 8)
                return 256;
            else if (power == 9)
                return 512;
            else if (power == 10)
                return 1024;
            else if (power == 11)
                return 2048;
            else if (power == 12)
                return 4096;
            else if (power == 13)
                return 8192;
            else if (power == 14)
                return 16384;
            else if (power == 15)
                return 32768;
            else if (power == 16)
                return 65536;
            else if (power == 17)
                return 131072;
            else if (power == 18)
                return 262144;
            else if (power == 19)
                return 524288;
            else if (power == 20)
                return 1048576;
            else if (power == 21)
                return 2097152;
            else if (power == 22)
                return 4194304;
            else if (power == 23)
                return 8388608;
            else if (power == 24)
                return 16777216;
            else if (power == 25)
                return 33554432;
            else if (power == 26)
                return 67108864;
            else if (power == 27)
                return 134217728;
            else if (power == 28)
                return 268435456;
            else if (power == 29)
                return 536870912;
            else return 1073741824;
        }
    }
    public class Utilitiy
    {

    }
    public class SpawnHandler
    {
        private List<GameObject> livePool = new List<GameObject>();
        private Stack deadPool = new Stack();
        public GameObject spawnObject;

        public List<GameObject> LivePool
        {
            get
            {
                return livePool;
            }
        }
        public Stack DeadPool
        {
            get
            {
                return deadPool;
            }
        }

        public SpawnHandler(GameObject go)
        {
            this.spawnObject = go;
        }

        public GameObject SpawnObject()
        {
            GameObject result;
            if (deadPool.Count == 0)
            {
                result = (GameObject)GameObject.Instantiate(spawnObject);
            }
            else
            {
                result = (GameObject)deadPool.Pop();
                result.SetActive(true);
            }
            livePool.Add(result);
            return result;
        }
        public bool DespawnObject(GameObject go)
        {
            if (livePool.Contains(go))
            {
                go.SetActive(false);
                livePool.Remove(go);
                deadPool.Push(go);
                return true;
            }
            else return false;
        }
        public void DesrtoyAllObjects()
        {
            foreach (GameObject go in livePool)
            {
                GameObject.Destroy(go);
            }
            foreach (GameObject go in deadPool)
            {
                GameObject.Destroy(go);
            }
            livePool.Clear();
            deadPool.Clear();
        }
        public void DestroyDeadObjects()
        {
            for (int i = 0; i < deadPool.Count; i++)
            {
                GameObject go = (GameObject)deadPool.Pop();
                GameObject.Destroy(go);
            }
        }
        public bool AddToAliveList(GameObject go)
        {
            if (livePool.Contains(go))
                return false;
            else livePool.Add(go);
            return true;
        }
        public bool RemoveFromAliveList(GameObject go)
        {
            if (livePool.Contains(go) == false)
                return false;
            else livePool.Remove(go);
            return true;
        }
    }

    namespace Imaging
    {
        //public class Image
        //{
        //    public static unsafe Byte3Color[] GetColorArrayFromBitmap(Bitmap bitmap)
        //    {
        //        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
        //        int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
        //        int heightInPixels = bitmapData.Height;
        //        int widthInBytes = bitmapData.Width * bytesPerPixel;
        //        byte* ptrFirstPixel = (byte*)bitmapData.Scan0;
        //    
        //        Byte3Color[] result = new Byte3Color[heightInPixels * bitmapData.Width];
        //    
        //        int currentPixelIndex = 0;
        //        for (int y = 0; y < heightInPixels; y++)
        //        {
        //            byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
        //            for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
        //            {
        //                byte blue = currentLine[x];
        //                byte green = currentLine[x + 1];
        //                byte red = currentLine[x + 2];
        //    
        //                result[currentPixelIndex] = new Byte3Color(red, green, blue);
        //                currentPixelIndex++;
        //            }
        //        }
        //        bitmap.UnlockBits(bitmapData);
        //        return result;
        //    }
        //}
        public static class PixelLetters
        {
            public const int WIDTH = 3;
            public const int HEIGHT = 5;
            private static Letter[] letters = new Letter[]
            {
                // Цифра 0
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true}, {true, false, false, false, true}, {true, true, true, true, true}}),
                // Цифра 1
                new Letter(new bool[WIDTH, HEIGHT] {{false, false, false, false, false}, {false, false, false, false, false }, {true, true, true, true, true}}),
                // Цифра 2
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, false, true}, {true, false, true, false, true}, {true, false, true, true, true}}),
                // Цифра 3
                new Letter(new bool[WIDTH, HEIGHT] {{true, false, true, false, true}, {true, false, true, false, true}, {true, true, true, true, true}}),
                // Цифра 4
                new Letter(new bool[WIDTH, HEIGHT] {{false, false, true, true, true}, {false, false, true, false, false}, {true, true, true, true, true}}),
                // Цифра 5
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, true, true, true}, {true, false, true, false, true}, {true, true, true, false, true}}),
                // Цифра 6
                new Letter(new bool[WIDTH, HEIGHT] { { true, true, true, true, true }, {true, false, true, false, true}, {true, true, true, false, true}}),
                // Цифра 7
                new Letter(new bool[WIDTH, HEIGHT] { { false, false, false, false, true }, { false, false, false, false, true }, { true, true, true, true, true } }),
                // Цифра 8
                new Letter(new bool[WIDTH, HEIGHT] { { true, true, true, true, true }, { true, false, true, false, true }, { true, true, true, true, true } }),
                // Цифра 9
                new Letter(new bool[WIDTH, HEIGHT] { { true, false, true, true, true }, { true, false, true, false, true }, { true, true, true, true, true } }),
                // Пробел
                new Letter(new bool[WIDTH, HEIGHT] { { false, false, false, false, false }, { false, false, false, false, false }, { false, false, false, false, false } }),
                // .
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, false, false, false}, { false, false, false, false, false }, { false, false, false, false, false } }),
                // -
                new Letter(new bool[WIDTH, HEIGHT] {{false, false, true, false, false}, {false, false, true, false, false }, { false, false, true, false, false } }),
                // A
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true}, {false, false, true, false, true}, {true, true, true, true, true}}),
                // M
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, false, true}, {false, false, false, true, false}, {true, true, true, false, true}}),
                // S
                new Letter(new bool[WIDTH, HEIGHT] {{true, false, true, true, true}, { true, false, true, false, true }, {true, true, true, false, true}}),
                // R
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true}, { false, false, false, false, true }, { false, false, false, true, true}}),
                // T
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, true}, { true, true, true, true, true }, { false, false, false, false, true}}),
                // U
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, true, true}, { true, false, false, false, false }, { true, true, true, true, true}}),
                // B
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, true, true}, { true, false, true, false, false }, { true, true, true, false, false } }),
                // C
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, true, true}, { true, false, false, false, true }, { true, false, false, false, true } }),
                // D
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, false, false }, { true, false, true, false, false }, { true, true, true, true, true } }),
                // E
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, true, true }, { true, false, true, false, true }, { true, false, true, false, true } }),
                // F
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, true, true }, { false, false, true, false, true }, { false, false, true, false, true } }),
                // G
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, true, true}, { true, false, false, false, true }, { true, true, false, false, true } }),
                // I
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, false}, { true, true, true, false, true }, { false, false, false, false, false } }),
                // H
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true}, { false, false, true, false, false }, {true, true, true, true, true}}),
                // J
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, false, false }, { true, false, false, false, false }, {true, true, true, true, true}}),
                // K
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true }, { false, false, true, false, false }, {true, true, false, true, true}}),
                // L
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true }, { true, false, false, false, false }, {true, false, false, false, false } }),
                // N
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true }, { false, false, false, false, true }, {true, true, true, true, true } }),
                // O
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true}, {true, false, false, false, true}, {true, true, true, true, true}}),
                // P
                new Letter(new bool[WIDTH, HEIGHT] {{true, true, true, true, true}, { false, false, true, false, true}, { false, false, true, true, true}}),
                // Q
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, true, true, true}, { false, false, true, false, true}, { true, true, true, true, true}}),
                // V
                new Letter(new bool[WIDTH, HEIGHT] {{ false, true, true, true, true}, { true, true, false, false, false }, { false, true, true, true, true}}),
                // W
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, false, true, true}, { false, true, false, false, false }, { true, false, false, true, true } }),
                // X
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, false, true, true}, { false, false, true, false, false }, { true, true, false, true, true } }),
                // Y
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, true, true, true}, { true, false, true, false, false }, { true, true, true, true, true } }),
                // Z
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, false, false, true}, { true, false, true, false, true }, { true, false, false, true, true } }),


                // %
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, false, false, true}, { false, false, true, false, false }, { true, false, false, true, true } }),
                // !
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, false }, { true, false, true, true, true }, { false, false, false, false, false } }),
                // ^
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, true, false }, { false, false, false, false, true }, { false, false, false, true, false } }),
                // /
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, false, false, false }, { false, false, true, false, false }, { false, false, false, true, true } }),
                // \
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, true, true }, { false, false, true, false, false }, { true, true, false, false, false } }),
                // |
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, false }, { true, true, true, true, true }, { false, false, false, false, false } }),
                // ,
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, false, false, false }, { false, false, false, false, false }, { false, false, false, false, false } }),
                // '
                new Letter(new bool[WIDTH, HEIGHT] { { false, false, false, true, true }, { false, false, false, false, false }, { false, false, false, false, false } }),
                // "
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, true, true }, { false, false, false, false, false }, { false, false, false, true, true } }),
                // [
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, false }, { true, true, true, true, true }, { true, false, false, false, true } }),
                // ]
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, false, false, true}, { true, true, true, true, true }, { false, false, false, false, false } }),
                // {
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, true, false, false }, { true, true, true, true, true }, { true, false, false, false, true } }),
                // }
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, false, false, true}, { true, true, true, true, true }, { false, false, true, false, false } }),
                // (
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, false }, { false, true, true, true, false }, { true, false, false, false, true } }),
                // )
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, false, false, true}, { false, true, true, true, false }, { false, false, false, false, false } }),
                // ?
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, true}, { true, false, true, false, true }, { false, false, false, true, true } }),
                // +
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, true, false, false }, { false, true, true, true, false }, { false, false, true, false, false } }),
                // =
                new Letter(new bool[WIDTH, HEIGHT] {{ false, true, false, true, false }, { false, true, false, true, false }, { false, true, false, true, false } }),
                // ;
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, false }, { true, true, false, true, false }, { false, false, false, false, false } }),
                // :
                new Letter(new bool[WIDTH, HEIGHT] {{ false, false, false, false, false }, { false, true, false, true, false }, { false, false, false, false, false } }),
                // _
                new Letter(new bool[WIDTH, HEIGHT] {{ true, false, false, false, false }, { true, false, false, false, false }, { true, false, false, false, false } }),
                // *
                new Letter(new bool[WIDTH, HEIGHT] {{ false, true, false, true, false }, { false, false, true, false, false }, { false, true, false, true, false } }),
                // @
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, false, true }, { true, false, true, false, true }, { true, true, true, true, true } }),
                // #
                new Letter(new bool[WIDTH, HEIGHT] {{ false, true, true, true, false }, { false, false, false, false, false }, { false, true, true, true, false } }),
                // $
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, false, true }, { true, false, true, false, true }, { true, false, true, true, true } }),
                // &
                new Letter(new bool[WIDTH, HEIGHT] {{ true, true, true, true, true }, { true, false, true, true, true }, { true, false, true, true, true } }),
            };

            public static Letter GetLetter(char ch)
            {
                switch (ch)
                {
                    case '0':
                        return letters[0];
                    case '1':
                        return letters[1];
                    case '2':
                        return letters[2];
                    case '3':
                        return letters[3];
                    case '4':
                        return letters[4];
                    case '5':
                        return letters[5];
                    case '6':
                        return letters[6];
                    case '7':
                        return letters[7];
                    case '8':
                        return letters[8];
                    case '9':
                        return letters[9];
                    case ' ':
                        return letters[10];
                    case '.':
                        return letters[11];
                    case '-':
                        return letters[12];
                    case 'a':
                        return letters[13];
                    case 'A':
                        return letters[13];
                    case 'm':
                        return letters[14];
                    case 'M':
                        return letters[14];
                    case 's':
                        return letters[15];
                    case 'S':
                        return letters[15];
                    case 'r':
                        return letters[16];
                    case 'R':
                        return letters[16];
                    case 't':
                        return letters[17];
                    case 'T':
                        return letters[17];
                    case 'u':
                        return letters[18];
                    case 'U':
                        return letters[18];
                    case 'b':
                        return letters[19];
                    case 'B':
                        return letters[19];
                    case 'c':
                        return letters[20];
                    case 'C':
                        return letters[20];
                    case 'd':
                        return letters[21];
                    case 'D':
                        return letters[21];
                    case 'e':
                        return letters[22];
                    case 'E':
                        return letters[22];
                    case 'f':
                        return letters[23];
                    case 'F':
                        return letters[23];
                    case 'g':
                        return letters[24];
                    case 'G':
                        return letters[24];
                    case 'i':
                        return letters[25];
                    case 'I':
                        return letters[25];
                    case 'h':
                        return letters[26];
                    case 'H':
                        return letters[26];
                    case 'j':
                        return letters[27];
                    case 'J':
                        return letters[27];
                    case 'k':
                        return letters[28];
                    case 'K':
                        return letters[28];
                    case 'l':
                        return letters[29];
                    case 'L':
                        return letters[29];
                    case 'n':
                        return letters[30];
                    case 'N':
                        return letters[30];
                    case 'o':
                        return letters[31];
                    case 'O':
                        return letters[31];
                    case 'p':
                        return letters[32];
                    case 'P':
                        return letters[32];
                    case 'q':
                        return letters[33];
                    case 'Q':
                        return letters[33];
                    case 'v':
                        return letters[34];
                    case 'V':
                        return letters[34];
                    case 'w':
                        return letters[35];
                    case 'W':
                        return letters[35];
                    case 'x':
                        return letters[36];
                    case 'X':
                        return letters[36];
                    case 'y':
                        return letters[37];
                    case 'Y':
                        return letters[37];
                    case 'z':
                        return letters[38];
                    case 'Z':
                        return letters[38];
                    case '%':
                        return letters[39];
                    case '!':
                        return letters[40];
                    case '^':
                        return letters[41];
                    case '/':
                        return letters[42];
                    case '\u005c':   // \ (backslash)
                        return letters[43];
                    case '|':
                        return letters[44];
                    case ',':
                        return letters[45];
                    case '\'':      // '
                        return letters[46];
                    case '\"':      // "
                        return letters[47];
                    case '[':
                        return letters[48];
                    case ']':
                        return letters[49];
                    case '{':
                        return letters[50];
                    case '}':
                        return letters[51];
                    case '(':
                        return letters[52];
                    case ')':
                        return letters[53];
                    case '?':
                        return letters[54];
                    case '+':
                        return letters[55];
                    case '=':
                        return letters[56];
                    case ';':
                        return letters[57];
                    case ':':
                        return letters[58];
                    case '_':
                        return letters[59];
                    case '*':
                        return letters[60];
                    case '@':
                        return letters[61];
                    case '#':
                        return letters[62];
                    case '$':
                        return letters[63];
                    case '&':
                        return letters[64];
                    default:
                        return letters[0];
                }
            }
            public static Letter ScaleLetter(Letter letter, int scale)
            {
                scale = Math.ClampMin(1, scale);
                bool[,] pixels = new bool[WIDTH * scale, HEIGHT * scale];
                Math.Vector2I pos = Math.Vector2I.Zero;
                for (int y = 0; y < letter.Pixels.GetLength(1); y++)
                {
                    for (int x = 0; x < letter.Pixels.GetLength(0); x++)
                    {
                        if (letter.Pixels[x, y] == true)
                        {
                            pos.x = x * scale;
                            pos.y = y * scale;
                            for (int yOffset = 0; yOffset < scale; yOffset++)
                            {
                                for (int xOffset = 0; xOffset < scale; xOffset++)
                                {
                                    pixels[pos.x + xOffset, pos.y + yOffset] = true;
                                }
                            }
                        }
                    }
                }
                return new Letter(pixels);
            }
            public static bool[,] GetPixelStringInGrid(int width, int heigth, string str, int scale, int charSpacing, Math.Vector2I startPos)
            {
                bool[,] grid = new bool[width, heigth];
                Math.Vector2I pointer = startPos;
                int xOffset = WIDTH * scale;
                int yOffset = HEIGHT * scale;
                int spacingOffset = charSpacing * scale;
                for (int i = 0; i < str.Length; i++)
                {
                    Letter currentLetter = ScaleLetter(GetLetter(str[i]), scale);
                    for (int y = 0; y < currentLetter.Pixels.GetLength(1); y++)
                    {
                        for (int x = 0; x < currentLetter.Pixels.GetLength(0); x++)
                        {
                            if (pointer.x < width && pointer.y < heigth)
                            {
                                int xNewPos = x + pointer.x;
                                int yNewPos = y + pointer.y;
                                if (xNewPos >= 0 && xNewPos < width && yNewPos >= 0 && yNewPos < heigth)
                                {
                                    if (currentLetter.Pixels[x, y] == true)
                                        grid[xNewPos, yNewPos] = true;
                                    else grid[xNewPos, yNewPos] = false;
                                }
                            }
                        }
                    }
                    pointer.x += xOffset + spacingOffset;
                }
                return grid;
            }

            public struct Letter
            {
                private bool[,] pixels;
                public bool[,] Pixels
                {
                    get { return pixels; }
                    set { pixels = value; }
                }
                public Letter(bool[,] pixels)
                {
                    this.pixels = pixels;
                }
            }
        }
        public struct Byte3Color
        {
            public const byte MIN_VALUE = byte.MinValue;
            public const byte MAX_VALUE = byte.MaxValue;
            public byte r, g, b;

            public Byte3Color(byte r, byte g, byte b)
            {
                this.r = r;
                this.g = g;
                this.b = b;
            }

            public static Byte3Color White
            {
                get { return new Byte3Color(MAX_VALUE, MAX_VALUE, MAX_VALUE); }
            }
            public static Byte3Color Black
            {
                get { return new Byte3Color(MIN_VALUE, MIN_VALUE, MIN_VALUE); }
            }
            public static Byte3Color Red
            {
                get { return new Byte3Color(MAX_VALUE, MIN_VALUE, MIN_VALUE); }
            }
            public static Byte3Color Green
            {
                get { return new Byte3Color(MIN_VALUE, MAX_VALUE, MIN_VALUE); }
            }
            public static Byte3Color Blue
            {
                get { return new Byte3Color(MIN_VALUE, MIN_VALUE, MAX_VALUE); }
            }
            public static Byte3Color Yellow
            {
                get { return new Byte3Color(MAX_VALUE, 235, 4); }
            }

            public override string ToString()
            {
                return "Color(" + r + ", " + g + ", " + b + ")";
            }
            public byte[] ToByteArray()
            {
                return new byte[] { r, g, b };
            }

            public static Byte3Color operator +(Byte3Color a, Byte3Color b)
            {
                return new Byte3Color(Math.ClampToByteRange(a.r + b.r), Math.ClampToByteRange(a.g + b.g), Math.ClampToByteRange(a.b + b.b));
            }
            public static Byte3Color operator *(Byte3Color a, float b)
            {
                return new Byte3Color(Math.ClampToByteRange(a.r * b), Math.ClampToByteRange(a.g * b), Math.ClampToByteRange(a.b * b));
            }
            public static Byte3Color operator *(float b, Byte3Color a)
            {
                return new Byte3Color(Math.ClampToByteRange(a.r * b), Math.ClampToByteRange(a.g * b), Math.ClampToByteRange(a.b * b));
            }

            public static Byte3Color Lerp(Byte3Color colorA, Byte3Color colorB, float value)
            {
                return new Byte3Color((byte)Math.Lerp(colorA.r, colorB.r, value), (byte)Math.Lerp(colorA.g, colorB.g, value), (byte)Math.Lerp(colorA.b, colorB.b, value));
            }
            public static UnityEngine.Color ToUnityColor(Byte3Color color)
            {
                return new UnityEngine.Color(color.r / (float)255, color.g / (float)255, color.b / (float)255);
            }
            public static Byte3Color AverageColor(Byte3Color[] colors)
            {
                Byte3Color result = new Byte3Color(0, 0, 0);
                int count = colors.Length;
                if (count > 0)
                {
                    int red = 0, green = 0, blue = 0;
                    for (int i = 0; i < count; i++)
                    {
                        red += colors[i].r;
                        green += colors[i].g;
                        blue += colors[i].b;
                    }
                    result.r = (byte)(red / count);
                    result.g = (byte)(green / count);
                    result.b = (byte)(blue / count);
                }
                return result;
            }


            //public static unsafe Byte3Color[] GetColorArrayFromBitmap(Bitmap bitmap)
            //{
            //    BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            //    int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            //    int heightInPixels = bitmapData.Height;
            //    int widthInBytes = bitmapData.Width * bytesPerPixel;
            //    byte* ptrFirstPixel = (byte*)bitmapData.Scan0;
            //
            //    Byte3Color[] result = new Byte3Color[heightInPixels * bitmapData.Width];
            //
            //    int currentPixelIndex = 0;
            //    for (int y = 0; y < heightInPixels; y++)
            //    {
            //        byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
            //        for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
            //        {
            //            byte blue = currentLine[x];
            //            byte green = currentLine[x + 1];
            //            byte red = currentLine[x + 2];
            //
            //            result[currentPixelIndex] = new Byte3Color(red, green, blue);
            //            currentPixelIndex++;
            //        }
            //    }
            //    bitmap.UnlockBits(bitmapData);
            //    return result;
            //}
        }
        public static class ColorConversion
        {

        }
    }

    namespace Audio
    {
        /*
        public class AudioEQVisualizer
        {
            #region private
            private bool isInit = false;
            private bool isAnalyzing = false;
            private float scale = 1;
            private int timeBetweenUpdInMs = 17; // 17 миллисекунд ~ 60 обновлений в секунду
            private int RATE = 44100;
            private int BUFFERSIZE = 2048;
            private int BUFFER_SIZE_POWER;
            //FIXIT: Сделать интерполяцию между результатами FFT и количеством полос эквалайзера
            private float[] barValues;

            private Timer timer;
            TimerCallback timerCallback;
            private WasapiLoopbackCapture waveIn;
            private WriteableBufferingSource buffer;
            #endregion

            #region public
            public bool IsInit
            {
                get
                {
                    return isInit;
                }
            }
            public bool IsAnalyzing
            {
                get
                {
                    return isAnalyzing;
                }
            }
            public bool UseHammingWindow { get; set; } = true;
            public float Scale
            {
                get
                {
                    return scale;
                }
                set
                {
                    scale = Math.ClampMin(0, value);
                }
            }
            public int SampleRate
            {
                get
                {
                    return RATE;
                }
                set
                {
                    if (value == 11025 || value == 22050 || value == 44100 || value == 48000 || value == 96000 || value == 192000)
                    {
                        RATE = value;
                    }
                    else throw new Exception("Wrong sampling rate!");
                }
            }
            public int BufferSize
            {
                get
                {
                    return BUFFERSIZE;
                }
                set
                {
                    if (Math.IsPowerOf2(value))
                    {
                        BUFFERSIZE = value;
                        BUFFER_SIZE_POWER = Math.Log2(value);
                    }
                    else throw new Exception("Wrong buffer size. Must be power of 2.");
                }
            }
            public int TimeBetweenUpdInMs
            {
                get
                {
                    return timeBetweenUpdInMs;
                }
                set
                {
                    if (value > 0)
                    {
                        timeBetweenUpdInMs = value;
                    }
                    else throw new Exception("Time between updates can not be 0.");
                }
            }
            public int BarCount
            {
                get
                {
                    return barValues.Length;
                }
                set
                {
                    if (value > 0)
                        barValues = new float[value];
                    else throw new Exception("Wrong bar count. Must be greater than 0.");
                }
            }
            #endregion

            public AudioEQVisualizer(int barCount)
            {
                BarCount = barCount;
            }

            public bool Init()
            {
                if (barValues == null)
                    throw new Exception("Bar count is not set.");
                if (isInit == false)
                {
                    BUFFER_SIZE_POWER = Math.Log2(BUFFERSIZE);
                    waveIn = new WasapiLoopbackCapture();
                    waveIn.DataAvailable += BufferAvalableCallback;
                    waveIn.Initialize();
                    
                    timerCallback = new TimerCallback(UpdateTick);
                    
                    buffer = new WriteableBufferingSource(new WaveFormat(RATE, 16, 1), BUFFERSIZE);
                    isInit = true;
                    return true;
                }
                else return false;
            }
            
            public void StartAnalysis()
            {
                if (isInit)
                {
                    if (isAnalyzing == false)
                    {
                        waveIn.Start();
                        isAnalyzing = true;
                        timer = new Timer(timerCallback, null, 0, timeBetweenUpdInMs);
                        //thread.Start();
                    }
                }
                else throw new Exception("Not initialized!. Use Init() func first.");
            }
            public void StopAnalysis()
            {
                if (isInit)
                {
                    isAnalyzing = false;
                    timer.Dispose();
                    waveIn.Stop();
                    buffer.FillWithZeros = true;
                }
                else throw new Exception("Not initialized!. Use Init() func first.");
            }
            public float[] GetBarData()
            {
                return barValues;
            }
            public CSCore.CoreAudioAPI.MMDeviceCollection GetDeviceList(CSCore.CoreAudioAPI.DeviceState deviceState)
            {
                CSCore.CoreAudioAPI.MMDeviceEnumerator enu = new CSCore.CoreAudioAPI.MMDeviceEnumerator();
                return enu.EnumAudioEndpoints(CSCore.CoreAudioAPI.DataFlow.All, deviceState);
            }
            public CSCore.CoreAudioAPI.MMDeviceCollection GetActiveDeviceList()
            {
                CSCore.CoreAudioAPI.MMDeviceEnumerator enu = new CSCore.CoreAudioAPI.MMDeviceEnumerator();
                return enu.EnumAudioEndpoints(CSCore.CoreAudioAPI.DataFlow.All, CSCore.CoreAudioAPI.DeviceState.Active);
            }
            public void SetDevice(CSCore.CoreAudioAPI.MMDevice device)
            {
                waveIn.Device = device;
            }
            public void Dispose()
            {
                waveIn.Stop();
                timer.Dispose();
                waveIn.Dispose();
                buffer.Dispose();
                isInit = false;
                isAnalyzing = false;
            }

            private void UpdateTick(object obj)
            {
                byte[] values = new byte[BUFFERSIZE];
                buffer.Read(values, 0, (int)buffer.Length);
                float[] fftData = new float[BUFFERSIZE];
                int BYTES_PER_POINT = 4;
                int graphPointCount = values.Length / BYTES_PER_POINT;
                for (int i = 0; i < graphPointCount; i++)
                {
                    float val = BitConverter.ToSingle(values, i * 4);
                    //HACK: провожу масштабирование после преобразования Фурье, а не до.
                    fftData[i] = (val * 32);// * scale;
                }
                float[] result = GetFFT(fftData);
                for (int i = 0; i < barValues.Length; i++)
                {
                    if (i < result.Length)
                        barValues[i] = result[i] * scale;
                }
            }
            private void BufferAvalableCallback(object obj, DataAvailableEventArgs e)
            {
                buffer.Write(e.Data, 0, e.ByteCount);
            }
            private float[] GetFFT(float[] data)
            {
                float[] result = new float[data.Length];
                ut.Complex[] complex = new ut.Complex[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    complex[i].Real = UseHammingWindow ? data[i] * FastFourierTransformation.HammingWindowF(i, data.Length) : data[i];
                }
                FastFourierTransformation.Fft(complex, BUFFER_SIZE_POWER);
                for (int i = 0; i < complex.Length; i++)
                {
                    result[i] = (float)complex[i].Value;
                }
                return result;
            }
        }
    }
    */

        namespace IO
        {
            //public class ComPortUSB
            //{
            //    private SerialPort port;
            //    private int timeBetweenUpdInMs = 17; //17 миллисекунд ~ 60 обновлений в секунду
            //    public SerialPort PortInstance { get { return port; } }
            //    public ComPortUSB(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            //    {
            //        port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            //    }
            //
            //
            //    public bool OpenPort()
            //  {
            //      try
            //      {
            //          if (port.IsOpen == false)
            //          {
            //              port.Open();
            //              return true;
            //          }
            //          else return false;
            //      }
            //      catch { return false; }
            //  }
            //    public bool ClosePort()
            //  {
            //      try
            //      {
            //          if (port.IsOpen)
            //          {
            //              port.Close();
            //              return true;
            //          }
            //          else return false;
            //      }
            //      catch { return false; }
            //  }
            //}
        }
    }
}
