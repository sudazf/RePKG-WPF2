using System.IO;
using System.Text;

namespace RePKG_WPF2.Utility
{
    public class FileReleaseUtility
    {
        public static bool ReleaseTo(string folder)
        {
            var exeFile = Path.Combine(folder, "RePKG.exe");
            var noticeFile = Path.Combine(folder, "THIRD-PARTY-NOTICES.txt");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (Release(Properties.Resources.RePKG, exeFile)
                && Release(Encoding.Default.GetBytes(Properties.Resources.THIRD_PARTY_NOTICES), noticeFile))
            {
                return true;
            }

            return false;
        }

        private static bool Release(byte[] resource, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create)) //开始写入文件流
            {
                fs.Write(resource, 0, resource.Length);
            }

            if (File.Exists(filePath)) //检索文件是否正确被释放
            {
                return true;
            }

            return false;
        }
    }
}
