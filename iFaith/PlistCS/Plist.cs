namespace iFaith.PlistCS
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Xml;

    public sealed class Plist
    {
        private static List<byte> objectTable = new List<byte>();
        private static int objRefSize;
        private static int offsetByteSize;
        private static List<int> offsetTable = new List<int>();
        private static long offsetTableOffset;
        private static int refCount;

        private Plist()
        {
        }

        private static void compose(object value, XmlWriter writer)
        {
            if ((value == null) || (value is string))
            {
                writer.WriteElementString("string", value as string);
            }
            else if ((value is int) || (value is long))
            {
                writer.WriteElementString("integer", Conversions.ToInteger(value).ToString(NumberFormatInfo.InvariantInfo));
            }
            else if ((value is Dictionary<string, object>) || value.GetType().ToString().StartsWith("System.Collections.Generic.Dictionary`2[System.String"))
            {
                Dictionary<string, object> dictionary = value as Dictionary<string, object>;
                if (dictionary == null)
                {
                    IEnumerator enumerator = null;
                    dictionary = new Dictionary<string, object>();
                    IDictionary dictionary2 = (IDictionary) value;
                    try
                    {
                        enumerator = dictionary2.Keys.GetEnumerator();
                        while (enumerator.MoveNext())
                        {
                            object objectValue = RuntimeHelpers.GetObjectValue(enumerator.Current);
                            dictionary.Add(objectValue.ToString(), RuntimeHelpers.GetObjectValue(dictionary2[RuntimeHelpers.GetObjectValue(objectValue)]));
                        }
                    }
                    finally
                    {
                        if (enumerator is IDisposable)
                        {
                            (enumerator as IDisposable).Dispose();
                        }
                    }
                }
                writeDictionaryValues(dictionary, writer);
            }
            else if (value is List<object>)
            {
                composeArray((List<object>) value, writer);
            }
            else if (value is byte[])
            {
                writer.WriteElementString("data", Convert.ToBase64String((byte[]) value));
            }
            else if ((value is float) || (value is double))
            {
                writer.WriteElementString("real", Conversions.ToDouble(value).ToString(NumberFormatInfo.InvariantInfo));
            }
            else if (value is DateTime)
            {
                string str = XmlConvert.ToString(Conversions.ToDate(value), XmlDateTimeSerializationMode.Utc);
                writer.WriteElementString("date", str);
            }
            else
            {
                if (!(value is bool))
                {
                    throw new Exception(string.Format("Value type '{0}' is unhandled", value.GetType().ToString()));
                }
                writer.WriteElementString(value.ToString().ToLower(), "");
            }
        }

        private static void composeArray(List<object> value, XmlWriter writer)
        {
            writer.WriteStartElement("array");
            using (List<object>.Enumerator enumerator = value.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    compose(RuntimeHelpers.GetObjectValue(RuntimeHelpers.GetObjectValue(enumerator.Current)), writer);
                }
            }
            writer.WriteEndElement();
        }

        private static byte[] composeBinary(object obj)
        {
            switch (obj.GetType().ToString())
            {
                case "System.Collections.Generic.Dictionary`2[System.String,System.Object]":
                    return writeBinaryDictionary((Dictionary<string, object>) obj);

                case "System.Collections.Generic.List`1[System.Object]":
                    return composeBinaryArray((List<object>) obj);

                case "System.Byte[]":
                    return writeBinaryByteArray((byte[]) obj);

                case "System.Double":
                    return writeBinaryDouble(Conversions.ToDouble(obj));

                case "System.Int32":
                    return writeBinaryInteger(Conversions.ToInteger(obj), true);

                case "System.String":
                    return writeBinaryString((string) obj, true);

                case "System.DateTime":
                    return writeBinaryDate(Conversions.ToDate(obj));

                case "System.Boolean":
                    return writeBinaryBool(Conversions.ToBoolean(obj));
            }
            return new byte[0];
        }

        private static byte[] composeBinaryArray(List<object> objects)
        {
            List<byte> collection = new List<byte>();
            List<byte> list2 = new List<byte>();
            List<int> list3 = new List<int>();
            for (int i = objects.Count - 1; i >= 0; i += -1)
            {
                composeBinary(RuntimeHelpers.GetObjectValue(objects[i]));
                offsetTable.Add(objectTable.Count);
                list3.Add(refCount);
                refCount--;
            }
            if (objects.Count < 15)
            {
                list2.Add(Convert.ToByte((int) (160 | Convert.ToByte(objects.Count))));
            }
            else
            {
                list2.Add(0xaf);
                list2.AddRange(writeBinaryInteger(objects.Count, false));
            }
            foreach (int num2 in list3)
            {
                byte[] array = RegulateNullBytes(BitConverter.GetBytes(num2), objRefSize);
                Array.Reverse(array);
                collection.InsertRange(0, array);
            }
            collection.InsertRange(0, list2);
            objectTable.InsertRange(0, collection);
            return collection.ToArray();
        }

        private static int countObject(object value)
        {
            int num = 0;
            switch (value.GetType().ToString())
            {
                case "System.Collections.Generic.Dictionary`2[System.String,System.Object]":
                {
                    Dictionary<string, object> dictionary = (Dictionary<string, object>) value;
                    foreach (string str2 in dictionary.Keys)
                    {
                        num += countObject(RuntimeHelpers.GetObjectValue(dictionary[str2]));
                    }
                    num += dictionary.Keys.Count;
                    num++;
                    return num;
                }
                case "System.Collections.Generic.List`1[System.Object]":
                {
                    List<object> list = (List<object>) value;
                    using (List<object>.Enumerator enumerator2 = list.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            object objectValue = RuntimeHelpers.GetObjectValue(enumerator2.Current);
                            num += countObject(RuntimeHelpers.GetObjectValue(objectValue));
                        }
                    }
                    num++;
                    return num;
                }
            }
            num++;
            return num;
        }

        private static int getCount(int bytePosition, ref int newBytePosition)
        {
            byte num = objectTable[bytePosition];
            byte num2 = Convert.ToByte((int) (num & 15));
            if (num2 < 15)
            {
                int num3 = num2;
                newBytePosition = bytePosition + 1;
                return num3;
            }
            return Conversions.ToInteger(parseBinaryInt(bytePosition + 1, ref newBytePosition));
        }

        public static plistType getPlistType(Stream stream)
        {
            byte[] buffer = new byte[8];
            stream.Read(buffer, 0, 8);
            if (BitConverter.ToInt64(buffer, 0) == 0x30307473696c7062L)
            {
                return plistType.Binary;
            }
            return plistType.Xml;
        }

        private static object parse(XmlNode node)
        {
            string name = node.Name;
            switch (name)
            {
                case "dict":
                    return parseDictionary(node);

                case "array":
                    return parseArray(node);

                case "string":
                    return node.InnerText;

                case "integer":
                    return Convert.ToInt32(node.InnerText, NumberFormatInfo.InvariantInfo);

                case "real":
                    return Convert.ToDouble(node.InnerText, NumberFormatInfo.InvariantInfo);

                case "false":
                    return false;

                case "true":
                    return true;

                case "null":
                    return null;

                case "date":
                    return XmlConvert.ToDateTime(node.InnerText, XmlDateTimeSerializationMode.Utc);
            }
            if (name != "data")
            {
                throw new ApplicationException(string.Format("Plist Node `{0}' is not supported", node.Name));
            }
            return Convert.FromBase64String(node.InnerText);
        }

        private static List<object> parseArray(XmlNode node)
        {
            IEnumerator enumerator = null;
            List<object> list = new List<object>();
            try
            {
                enumerator = node.ChildNodes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    XmlNode current = (XmlNode) enumerator.Current;
                    object objectValue = RuntimeHelpers.GetObjectValue(parse(current));
                    if (objectValue != null)
                    {
                        list.Add(RuntimeHelpers.GetObjectValue(objectValue));
                    }
                }
            }
            finally
            {
                if (enumerator is IDisposable)
                {
                    (enumerator as IDisposable).Dispose();
                }
            }
            return list;
        }

        private static object parseBinary(int objRef)
        {
            byte num = objectTable[offsetTable[objRef]];
            switch ((num & 240))
            {
                case 0:
                    return ((objectTable[offsetTable[objRef]] != 0) && (objectTable[offsetTable[objRef]] == 9));

                case 0x10:
                    return parseBinaryInt(offsetTable[objRef]);

                case 0x20:
                    return parseBinaryReal(offsetTable[objRef]);

                case 0x30:
                    return parseBinaryDate(offsetTable[objRef]);

                case 0x40:
                    return parseBinaryByteArray(offsetTable[objRef]);

                case 80:
                    return parseBinaryAsciiString(offsetTable[objRef]);

                case 0x60:
                    return parseBinaryUnicodeString(offsetTable[objRef]);

                case 0xd0:
                    return parseBinaryDictionary(objRef);

                case 160:
                    return parseBinaryArray(objRef);
            }
            throw new Exception("This type is not supported");
        }

        private static object parseBinaryArray(int objRef)
        {
            int num3 = 0;
            List<object> list = new List<object>();
            List<int> list2 = new List<int>();
            int num = 0;
            byte num2 = objectTable[offsetTable[objRef]];
            num = getCount(offsetTable[objRef], ref num3);
            if (num < 15)
            {
                num3 = offsetTable[objRef] + 1;
            }
            else
            {
                num3 = (offsetTable[objRef] + 2) + RegulateNullBytes(BitConverter.GetBytes(num), 1).Length;
            }
            int index = num3;
            while (index < (num3 + (num * objRefSize)))
            {
                byte[] array = objectTable.GetRange(index, objRefSize).ToArray();
                Array.Reverse(array);
                list2.Add(BitConverter.ToInt32(RegulateNullBytes(array, 4), 0));
                index += objRefSize;
            }
            int num5 = num - 1;
            for (index = 0; index <= num5; index++)
            {
                list.Add(RuntimeHelpers.GetObjectValue(parseBinary(list2[index])));
            }
            return list;
        }

        private static object parseBinaryAsciiString(int headerPosition)
        {
            int num = 0;
            int count = getCount(headerPosition, ref num);
            List<byte> range = objectTable.GetRange(num, count);
            if (range.Count <= 0)
            {
                return string.Empty;
            }
            return Encoding.ASCII.GetString(range.ToArray());
        }

        private static object parseBinaryByteArray(int headerPosition)
        {
            int num = 0;
            int count = getCount(headerPosition, ref num);
            return objectTable.GetRange(num, count).ToArray();
        }

        public static object parseBinaryDate(int headerPosition)
        {
            byte[] array = objectTable.GetRange(headerPosition + 1, 8).ToArray();
            Array.Reverse(array);
            return PlistDateConverter.ConvertFromAppleTimeStamp(BitConverter.ToDouble(array, 0));
        }

        private static object parseBinaryDictionary(int objRef)
        {
            int num3 = 0;
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            List<int> list = new List<int>();
            int num = 0;
            byte num2 = objectTable[offsetTable[objRef]];
            num = getCount(offsetTable[objRef], ref num3);
            if (num < 15)
            {
                num3 = offsetTable[objRef] + 1;
            }
            else
            {
                num3 = (offsetTable[objRef] + 2) + RegulateNullBytes(BitConverter.GetBytes(num), 1).Length;
            }
            int index = num3;
            while (index < (num3 + ((num * 2) * objRefSize)))
            {
                byte[] array = objectTable.GetRange(index, objRefSize).ToArray();
                Array.Reverse(array);
                list.Add(BitConverter.ToInt32(RegulateNullBytes(array, 4), 0));
                index += objRefSize;
            }
            int num5 = num - 1;
            for (index = 0; index <= num5; index++)
            {
                dictionary.Add((string) parseBinary(list[index]), RuntimeHelpers.GetObjectValue(parseBinary(list[index + num])));
            }
            return dictionary;
        }

        private static object parseBinaryInt(int headerPosition)
        {
            int num = 0;
            return parseBinaryInt(headerPosition, ref num);
        }

        private static object parseBinaryInt(int headerPosition, ref int newHeaderPosition)
        {
            byte num = objectTable[headerPosition];
            int count = (int) Math.Round(Math.Truncate(Math.Pow(2.0, (double) (num & 15))));
            byte[] array = objectTable.GetRange(headerPosition + 1, count).ToArray();
            Array.Reverse(array);
            newHeaderPosition = (headerPosition + count) + 1;
            return BitConverter.ToInt32(RegulateNullBytes(array, 4), 0);
        }

        private static object parseBinaryReal(int headerPosition)
        {
            byte num = objectTable[headerPosition];
            int count = (int) Math.Round(Math.Truncate(Math.Pow(2.0, (double) (num & 15))));
            byte[] array = objectTable.GetRange(headerPosition + 1, count).ToArray();
            Array.Reverse(array);
            return BitConverter.ToDouble(RegulateNullBytes(array, 8), 0);
        }

        private static object parseBinaryUnicodeString(int headerPosition)
        {
            int num = 0;
            int num2 = getCount(headerPosition, ref num) * 2;
            byte[] bytes = new byte[(num2 - 1) + 1];
            int num3 = num2 - 1;
            for (int i = 0; i <= num3; i += 2)
            {
                byte num5 = objectTable.GetRange(num + i, 1)[0];
                byte num6 = objectTable.GetRange((num + i) + 1, 1)[0];
                if (BitConverter.IsLittleEndian)
                {
                    bytes[i] = num6;
                    bytes[i + 1] = num5;
                }
                else
                {
                    bytes[i] = num5;
                    bytes[i + 1] = num6;
                }
            }
            return Encoding.Unicode.GetString(bytes);
        }

        private static Dictionary<string, object> parseDictionary(XmlNode node)
        {
            XmlNodeList childNodes = node.ChildNodes;
            if ((childNodes.Count % 2) != 0)
            {
                throw new DataMisalignedException("Dictionary elements must have an even number of child nodes");
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            int num = childNodes.Count - 1;
            for (int i = 0; i <= num; i += 2)
            {
                XmlNode node2 = childNodes[i];
                XmlNode node3 = childNodes[i + 1];
                if (node2.Name != "key")
                {
                    throw new ApplicationException("expected a key node");
                }
                object objectValue = RuntimeHelpers.GetObjectValue(parse(node3));
                if (objectValue != null)
                {
                    dictionary.Add(node2.InnerText, RuntimeHelpers.GetObjectValue(objectValue));
                }
            }
            return dictionary;
        }

        private static void parseOffsetTable(List<byte> offsetTableBytes)
        {
            for (int i = 0; i < offsetTableBytes.Count; i += offsetByteSize)
            {
                byte[] array = offsetTableBytes.GetRange(i, offsetByteSize).ToArray();
                Array.Reverse(array);
                offsetTable.Add(BitConverter.ToInt32(RegulateNullBytes(array, 4), 0));
            }
        }

        private static void parseTrailer(List<byte> trailer)
        {
            offsetByteSize = BitConverter.ToInt32(RegulateNullBytes(trailer.GetRange(6, 1).ToArray(), 4), 0);
            objRefSize = BitConverter.ToInt32(RegulateNullBytes(trailer.GetRange(7, 1).ToArray(), 4), 0);
            byte[] array = trailer.GetRange(12, 4).ToArray();
            Array.Reverse(array);
            refCount = BitConverter.ToInt32(array, 0);
            byte[] buffer2 = trailer.GetRange(0x18, 8).ToArray();
            Array.Reverse(buffer2);
            offsetTableOffset = BitConverter.ToInt64(buffer2, 0);
        }

        private static object readBinary(byte[] data)
        {
            offsetTable.Clear();
            List<byte> list = new List<byte>();
            objectTable.Clear();
            refCount = 0;
            objRefSize = 0;
            offsetByteSize = 0;
            offsetTableOffset = 0L;
            List<byte> list2 = new List<byte>(data);
            parseTrailer(list2.GetRange(list2.Count - 0x20, 0x20));
            objectTable = list2.GetRange(0, (int) offsetTableOffset);
            parseOffsetTable(list2.GetRange((int) offsetTableOffset, (list2.Count - ((int) offsetTableOffset)) - 0x20));
            return parseBinary(0);
        }

        public static object readPlist(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return readPlist(stream, plistType.Auto);
            }
        }

        public static object readPlist(byte[] data)
        {
            return readPlist(new MemoryStream(data), plistType.Auto);
        }

        public static object readPlist(Stream stream, plistType type)
        {
            if (type == plistType.Auto)
            {
                type = getPlistType(stream);
                stream.Seek(0L, SeekOrigin.Begin);
            }
            if (type == plistType.Binary)
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return readBinary(reader.ReadBytes((int) reader.BaseStream.Length));
                }
            }
            XmlDocument xml = new XmlDocument();
            xml.XmlResolver = null;
            xml.Load(stream);
            return readXml(xml);
        }

        public static object readPlistSource(string source)
        {
            return readPlist(Encoding.UTF8.GetBytes(source));
        }

        private static object readXml(XmlDocument xml)
        {
            XmlNode node = xml.DocumentElement.ChildNodes[0];
            return (Dictionary<string, object>) parse(node);
        }

        private static byte[] RegulateNullBytes(byte[] value)
        {
            return RegulateNullBytes(value, 1);
        }

        private static byte[] RegulateNullBytes(byte[] value, int minBytes)
        {
            Array.Reverse(value);
            List<byte> list = new List<byte>(value);
            int num = list.Count - 1;
            for (int i = 0; i <= num; i++)
            {
                if ((list[i] != 0) || (list.Count <= minBytes))
                {
                    break;
                }
                list.Remove(list[i]);
                i--;
            }
            if (list.Count < minBytes)
            {
                int num3 = minBytes - list.Count;
                int num4 = num3 - 1;
                for (int j = 0; j <= num4; j++)
                {
                    list.Insert(0, 0);
                }
            }
            value = list.ToArray();
            Array.Reverse(value);
            return value;
        }

        public static byte[] writeBinary(object value)
        {
            offsetTable.Clear();
            objectTable.Clear();
            refCount = 0;
            objRefSize = 0;
            offsetByteSize = 0;
            offsetTableOffset = 0L;
            int num = countObject(RuntimeHelpers.GetObjectValue(value)) - 1;
            refCount = num;
            objRefSize = RegulateNullBytes(BitConverter.GetBytes(refCount)).Length;
            composeBinary(RuntimeHelpers.GetObjectValue(value));
            writeBinaryString("bplist00", false);
            offsetTableOffset = objectTable.Count;
            offsetTable.Add(objectTable.Count - 8);
            offsetByteSize = RegulateNullBytes(BitConverter.GetBytes(offsetTable[offsetTable.Count - 1])).Length;
            List<byte> collection = new List<byte>();
            offsetTable.Reverse();
            int num2 = offsetTable.Count - 1;
            for (int i = 0; i <= num2; i++)
            {
                offsetTable[i] = objectTable.Count - offsetTable[i];
                byte[] array = RegulateNullBytes(BitConverter.GetBytes(offsetTable[i]), offsetByteSize);
                Array.Reverse(array);
                collection.AddRange(array);
            }
            objectTable.AddRange(collection);
            objectTable.AddRange(new byte[6]);
            objectTable.Add(Convert.ToByte(offsetByteSize));
            objectTable.Add(Convert.ToByte(objRefSize));
            byte[] bytes = BitConverter.GetBytes((long) (num + 1L));
            Array.Reverse(bytes);
            objectTable.AddRange(bytes);
            objectTable.AddRange(BitConverter.GetBytes((long) 0L));
            bytes = BitConverter.GetBytes(offsetTableOffset);
            Array.Reverse(bytes);
            objectTable.AddRange(bytes);
            return objectTable.ToArray();
        }

        public static void writeBinary(object value, Stream stream)
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(writeBinary(RuntimeHelpers.GetObjectValue(value)));
            }
        }

        public static void writeBinary(object value, string path)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(path, FileMode.Create)))
            {
                writer.Write(writeBinary(RuntimeHelpers.GetObjectValue(value)));
            }
        }

        public static byte[] writeBinaryBool(bool obj)
        {
            List<byte> collection = new List<byte>(new byte[] { obj ? ((byte) 9) : ((byte) 8) });
            objectTable.InsertRange(0, collection);
            return collection.ToArray();
        }

        private static byte[] writeBinaryByteArray(byte[] value)
        {
            List<byte> collection = new List<byte>(value);
            List<byte> list2 = new List<byte>();
            if (value.Length < 15)
            {
                list2.Add(Convert.ToByte((int) (0x40 | Convert.ToByte(value.Length))));
            }
            else
            {
                list2.Add(0x4f);
                list2.AddRange(writeBinaryInteger(collection.Count, false));
            }
            collection.InsertRange(0, list2);
            objectTable.InsertRange(0, collection);
            return collection.ToArray();
        }

        public static byte[] writeBinaryDate(DateTime obj)
        {
            List<byte> collection = new List<byte>(RegulateNullBytes(BitConverter.GetBytes(PlistDateConverter.ConvertToAppleTimeStamp(obj)), 8));
            collection.Reverse();
            collection.Insert(0, 0x33);
            objectTable.InsertRange(0, collection);
            return collection.ToArray();
        }

        private static byte[] writeBinaryDictionary(Dictionary<string, object> dictionary)
        {
            List<byte> collection = new List<byte>();
            List<byte> list2 = new List<byte>();
            List<int> list3 = new List<int>();
            for (int i = dictionary.Count - 1; i >= 0; i += -1)
            {
                object[] array = new object[(dictionary.Count - 1) + 1];
                dictionary.Values.CopyTo(array, 0);
                composeBinary(RuntimeHelpers.GetObjectValue(array[i]));
                offsetTable.Add(objectTable.Count);
                list3.Add(refCount);
                refCount--;
            }
            for (int j = dictionary.Count - 1; j >= 0; j += -1)
            {
                string[] strArray = new string[(dictionary.Count - 1) + 1];
                dictionary.Keys.CopyTo(strArray, 0);
                composeBinary(strArray[j]);
                offsetTable.Add(objectTable.Count);
                list3.Add(refCount);
                refCount--;
            }
            if (dictionary.Count < 15)
            {
                list2.Add(Convert.ToByte((int) (0xd0 | Convert.ToByte(dictionary.Count))));
            }
            else
            {
                list2.Add(0xdf);
                list2.AddRange(writeBinaryInteger(dictionary.Count, false));
            }
            foreach (int num3 in list3)
            {
                byte[] buffer = RegulateNullBytes(BitConverter.GetBytes(num3), objRefSize);
                Array.Reverse(buffer);
                collection.InsertRange(0, buffer);
            }
            collection.InsertRange(0, list2);
            objectTable.InsertRange(0, collection);
            return collection.ToArray();
        }

        private static byte[] writeBinaryDouble(double value)
        {
            List<byte> collection = new List<byte>(RegulateNullBytes(BitConverter.GetBytes(value), 4));
            while (collection.Count != Math.Pow(2.0, Math.Log((double) collection.Count) / Math.Log(2.0)))
            {
                collection.Add(0);
            }
            int num = 0x20 | ((int) Math.Round(Math.Truncate((double) (Math.Log((double) collection.Count) / Math.Log(2.0)))));
            collection.Reverse();
            collection.Insert(0, Convert.ToByte(num));
            objectTable.InsertRange(0, collection);
            return collection.ToArray();
        }

        private static byte[] writeBinaryInteger(int value, bool write)
        {
            List<byte> collection = new List<byte>(BitConverter.GetBytes((long) value));
            collection = new List<byte>(RegulateNullBytes(collection.ToArray()));
            while (collection.Count != Math.Pow(2.0, Math.Log((double) collection.Count) / Math.Log(2.0)))
            {
                collection.Add(0);
            }
            int num = 0x10 | ((int) Math.Round(Math.Truncate((double) (Math.Log((double) collection.Count) / Math.Log(2.0)))));
            collection.Reverse();
            collection.Insert(0, Convert.ToByte(num));
            if (write)
            {
                objectTable.InsertRange(0, collection);
            }
            return collection.ToArray();
        }

        private static byte[] writeBinaryString(string value, bool head)
        {
            List<byte> collection = new List<byte>();
            List<byte> list2 = new List<byte>();
            foreach (char ch in value.ToCharArray())
            {
                collection.Add(Convert.ToByte(ch));
            }
            if (head)
            {
                if (value.Length < 15)
                {
                    list2.Add(Convert.ToByte((int) (80 | Convert.ToByte(value.Length))));
                }
                else
                {
                    list2.Add(0x5f);
                    list2.AddRange(writeBinaryInteger(collection.Count, false));
                }
            }
            collection.InsertRange(0, list2);
            objectTable.InsertRange(0, collection);
            return collection.ToArray();
        }

        private static void writeDictionaryValues(Dictionary<string, object> dictionary, XmlWriter writer)
        {
            writer.WriteStartElement("dict");
            foreach (string str in dictionary.Keys)
            {
                object objectValue = RuntimeHelpers.GetObjectValue(dictionary[str]);
                writer.WriteElementString("key", str);
                compose(RuntimeHelpers.GetObjectValue(objectValue), writer);
            }
            writer.WriteEndElement();
        }

        public static string writeXml(object value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UTF8Encoding(false);
                settings.ConformanceLevel = ConformanceLevel.Document;
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteDocType("plist", "-//Apple Computer//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
                    writer.WriteStartElement("plist");
                    writer.WriteAttributeString("version", "1.0");
                    compose(RuntimeHelpers.GetObjectValue(value), writer);
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                    return Encoding.UTF8.GetString(stream.ToArray());
                }
            }
        }

        public static void writeXml(object value, Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(writeXml(RuntimeHelpers.GetObjectValue(value)));
            }
        }

        public static void writeXml(object value, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(writeXml(RuntimeHelpers.GetObjectValue(value)));
            }
        }
    }
}

