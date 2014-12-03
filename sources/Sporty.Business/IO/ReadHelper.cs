using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Sporty.Business.IO
{
    public sealed class ReadHelper
    {
        // Methods
        private ReadHelper()
        {
        }

        public static string[] GetBlockLines(string blockName, string[] fileContentsLines)
        {
            string str = "[" + blockName + "]";
            bool flag = false;
            var list = new ArrayList();
            for (int i = 0; i < fileContentsLines.Length; i++)
            {
                if (!flag && fileContentsLines[i].StartsWith(str))
                {
                    flag = true;
                }
                else
                {
                    if (flag && ((fileContentsLines[i].Length == 0) || fileContentsLines[i].StartsWith("[")))
                    {
                        break;
                    }
                    if (flag)
                    {
                        list.Add(fileContentsLines[i]);
                    }
                }
            }
            return (string[]) list.ToArray(typeof (string));
        }

        public static string GetValueFromBlock(string[] blockLines, string name)
        {
            for (int i = 0; i < blockLines.Length; i++)
            {
                if (blockLines[i].StartsWith(name + "="))
                {
                    return blockLines[i].Substring(name.Length + 1);
                }
            }
            return null;
        }

        public static byte[] ReadFileToByteArray(string filePath)
        {
            byte[] buffer2 = null;
            try
            {
                int num4;
                var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var length = (int) stream.Length;
                var buffer = new byte[length];
                for (int i = 0; i < length; i += num4)
                {
                    int count = length - i;
                    if (count > 0x1000)
                    {
                        count = 0x1000;
                    }
                    num4 = stream.Read(buffer, i, count);
                }
                stream.Close();
                buffer2 = buffer;
            }
            catch (Exception exception)
            {
                //throw new Exception(ResxManager.Instance.GetString("ReadErrorBinary"), exception);
            }
            return buffer2;
        }

        public static string[] ReadFileToLineArray(string filePath)
        {
            string[] strArray = null;
            try
            {
                var reader = new StreamReader(filePath, Encoding.Default, false);
                var list = new ArrayList();
                string str = null;
                while ((str = reader.ReadLine()) != null)
                {
                    list.Add(str);
                }
                reader.Close();
                strArray = (string[]) list.ToArray(typeof (string));
            }
            catch (Exception exception)
            {
                //throw new Exception(ResxManager.Instance.GetString("ReadErrorText"), exception);
            }
            return strArray;
        }
    }
}