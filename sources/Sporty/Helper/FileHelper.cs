using Common.Logging;
using Sporty.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Sporty.Helper
{
    public class FileHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FileHelper));

        public static string SaveFile(byte[] fileContent, string filename, string extension, string subPath)
        {
            log.InfoFormat("Call SaveFile filename: {0}, ext: {1}", filename, extension);

            string filePathAndName = GetAttachmentFilePathAndName(filename, subPath);
            string directory = Path.GetDirectoryName(filePathAndName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filename).ToLower();

            for (int i = 1; ; ++i)
            {
                if (!System.IO.File.Exists(filePathAndName))
                    break;

                filePathAndName = Path.Combine(directory, fileNameWithoutExt + "_" + i + extension);
            }
            StreamWriter writer = null;
            try
            {
                File.WriteAllBytes(filePathAndName, fileContent);
            }
            catch (Exception exc)
            {
                log.Error("Can't save file.", exc);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }

            return filePathAndName;
        }
        public static string SaveFile(string fileContent, string filename, string extension, string subPath)
        {
            log.InfoFormat("Call SaveFile filename: {0}, ext: {1}", filename, extension);

            string filePathAndName = GetAttachmentFilePathAndName(filename, subPath);
            string directory = Path.GetDirectoryName(filePathAndName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filename).ToLower();

            for (int i = 1; ; ++i)
            {
                if (!System.IO.File.Exists(filePathAndName))
                    break;

                filePathAndName = Path.Combine(directory, fileNameWithoutExt + "_" + i + extension);
            }
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(filePathAndName);
                writer.Write(fileContent);

            }
            catch (Exception exc)
            {
                log.Error("Can't save file.", exc);
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }

            return filePathAndName;
        }

        public static string GetMaterialFilePathAndName(string filename, string userId)
        {
            string rootFolder = AppConfigHelper.GetWebConfigValue(Constants.RootUploadFolder);

            string fileName = Path.GetFileName(filename).ToLower();
            string filePathAndName = Path.Combine(rootFolder, userId, "Material", fileName);
            return filePathAndName;
        }

        public static string GetAttachmentFilePathAndName(string filename, string subPath)
        {
            string rootFolder = AppConfigHelper.GetWebConfigValue(Constants.RootUploadFolder);

            string fileName = Path.GetFileName(filename).ToLower();
            string filePathAndName = Path.Combine(rootFolder, subPath, fileName);
            return filePathAndName;
        }
    }
}