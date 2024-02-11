using System.Text.RegularExpressions;
using System.Text;

namespace Mango.Service.ProductApi.Helper
{
    public class Utilities
    {
        public static int PAGE_SIZE = 20;

        private static IWebHostEnvironment _webHostEnvironment;
        public static void InitialHostEnv(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
            {
                Directory.CreateDirectory(path);
            }
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(result))
            {
                var words = str.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    var s = words[i];
                    if (s.Length > 0)
                    {
                        words[i] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static bool IsInteger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            try
            {
                if (String.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                if (!regex.IsMatch(str))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string GetRandomKey(int length = 5)
        {
            string pattern = @"123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return stringBuilder.ToString();
        }
        public static string SEOUrl(string url)
        {
            var result = url.ToLower().Trim();
            result = Regex.Replace(result, "áàạảãâấầậẫăắằặẳẵ", "a");
            result = Regex.Replace(result, "éèẹẻẽêếềệểễ", "e");
            result = Regex.Replace(result, "óòọỏõôốồộổỗơớờợởỡ", "o");
            result = Regex.Replace(result, "úùụủũưứừựửữ", "u");
            result = Regex.Replace(result, "íìịỉĩ", "i");
            result = Regex.Replace(result, "ýỳỵỷỹ", "y");
            result = Regex.Replace(result, "đ", "d");
            result = Regex.Replace(result, "[^a-z0-9-]", "");
            result = Regex.Replace(result, "(-)+", "-");
            return result;
        }

        public static async Task<string> UploadFile(Microsoft.AspNetCore.Http.IFormFile fileThumb, string sDirectory, string newname)
        {
            try
            {
                if (fileThumb != null)
                {
                    if (newname == null) newname = fileThumb.FileName;
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "ProductImages", sDirectory);
                    CreateIfMissing(path);
                    string folderName = Path.Combine("ProductImages", sDirectory);
                    string pathFile = Path.Combine(_webHostEnvironment.WebRootPath, folderName, newname);
                    var supportedType = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExt = System.IO.Path.GetExtension(fileThumb.FileName);
                    if (!supportedType.Contains(fileExt.ToLower())) return null;
                    else
                    {
                        using (var stream = new FileStream(pathFile, FileMode.Create))
                        {
                            await fileThumb.CopyToAsync(stream);
                        }
                        newname = Path.Combine("\\ProductImages", sDirectory, newname);
                        return newname;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    } 
}
