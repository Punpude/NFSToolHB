using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSLocaleTool
{
    public static class Utils
    {
		public static byte[] ReadNullTerminatedArray(BinaryReader reader)
		{
			List<byte> result = new List<byte>();
			while (true)
			{
				byte b = reader.ReadByte();
				if(b != 0)
				{
                    result.Add(b);
					continue;
                }
				break;
			}
			return result.ToArray();
		}
		

        public static string ReadString(byte[] namebuf, Encoding encoding)
        {
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(namebuf));
            if (encoding == null) throw new ArgumentNullException("encoding");

            List<byte> data = new List<byte>();

            while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
            {
                data.Add(binaryReader.ReadByte());

                string partialString = encoding.GetString(data.ToArray(), 0, data.Count);

                if (partialString.Length > 0 && partialString.Last() == '\0')
                    return encoding.GetString(data.SkipLast(encoding.GetByteCount("\0")).ToArray()).TrimEnd('\0');
            }
            throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
        }
        public static string ReadString(this BinaryReader binaryReader, Encoding encoding)
		{
			if (binaryReader == null) throw new ArgumentNullException("binaryReader");
			if (encoding == null) throw new ArgumentNullException("encoding");

			List<byte> data = new List<byte>();

			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				data.Add(binaryReader.ReadByte());

				string partialString = encoding.GetString(data.ToArray(), 0, data.Count);

				if (partialString.Length > 0 && partialString.Last() == '\0')
					return encoding.GetString(data.SkipLast(encoding.GetByteCount("\0")).ToArray()).TrimEnd('\0');
			}
			throw new InvalidDataException("Hit end of stream while reading null-terminated string.");
		}
		private static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (source == null) throw new ArgumentNullException("source");

			Queue<TSource> queue = new Queue<TSource>();

			foreach (TSource item in source)
			{
				queue.Enqueue(item);

				if (queue.Count > count) yield return queue.Dequeue();
			}
		}
    }
}
