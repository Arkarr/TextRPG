using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Serialization
{
    public abstract class DataFile<T>
    {
        public const int INT_BYTES_LENGTH = 4;
        public const int FLOAT_BYTES_LENGTH = 4;
        public const int DOUBLE_BYTES_LENGTH = 8;
        public const int BOOLEAN_BYTES_LENGTH = 1;
        public const int CHAR_BYTES_LENGTH = 2;


        public List<byte> bytes = new List<byte>();
        private int readIndex = 0;

        public DataFile() { }

        public void WriteToFile (FileInfo file)
        {
            /*DateTime start = DateTime.Now;

            File.WriteAllBytes(file.FullName, bytes.ToArray());
            TimeSpan end = DateTime.Now - start;
            Console.WriteLine("Elapsed : " + end);

            start = DateTime.Now;
            using (FileStream writer = file.OpenWrite())
            {
                bytes.ForEach(b => writer.WriteByte(b));
            }
            end = DateTime.Now - start;
            Console.WriteLine("Elapsed : " + end);

            start = DateTime.Now;
            using (FileStream writer = file.OpenWrite())
            {
                foreach (byte b in bytes)
                {
                    writer.WriteByte(b);
                }
            }
            end = DateTime.Now - start;
            Console.WriteLine("Elapsed : " + end);*/

            // Result WriteAllBytes is faster (and shorter in actual code ...) :

            File.WriteAllBytes(file.FullName, bytes.ToArray());
        }

        #region Write Methods
        public void Write(int value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(double value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(float value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(bool value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(char value)
        {
            bytes.AddRange(BitConverter.GetBytes(value));
        }

        public void Write(string value)
        {
            bytes.AddRange(Encoding.UTF8.GetBytes(value));
        }
        #endregion

        #region Read Methods
        private bool ensureNbBytes(int nbBytes)
        {
            return readIndex + nbBytes < bytes.Count;
        }

        private byte[] readNext()
        {
            List<byte> bytes = new List<byte>();

            while (bytes[readIndex] != (byte)0)
            {
                bytes.Add(bytes[readIndex++]);
            }

            readIndex++;
            return bytes.ToArray();
        }

        private byte[] readNext(int nbBytes)
        {
            if (!ensureNbBytes(nbBytes))
            {
                Console.WriteLine("You went past the limit of how much data can hold a message!");
                return null;
            }

            List<byte> bytes = new List<byte>();

            for (int i = 0; i < nbBytes; i++)
            {
                bytes.Add(bytes[readIndex++]);
            }

            return bytes.ToArray();
        }

        public int ReadInt ()
        {
            return BitConverter.ToInt32(readNext(INT_BYTES_LENGTH), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(readNext(DOUBLE_BYTES_LENGTH), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(readNext(FLOAT_BYTES_LENGTH), 0);
        }

        public bool ReadBoolean()
        {
            return BitConverter.ToBoolean(readNext(BOOLEAN_BYTES_LENGTH), 0);
        }

        public char ReadChar()
        {
            return BitConverter.ToChar(readNext(CHAR_BYTES_LENGTH), 0);
        }

        public string ReadString()
        {
            return Encoding.UTF8.GetString(readNext());
        }
        #endregion
    }
}
