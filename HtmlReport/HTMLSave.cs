using System.IO;
using System.Text;

namespace HtmlReport
{
    public static class HTMLSave
    {
        private static string Header = "<!DOCTYPE html><html lang=\"ru\"><head><meta charset = \"UTF-8\"><title>Отчёт</title></head><body>";
        private static string Footer = "</body></html>";
        public static bool SaveInHTMLFile(List<string>? Process, List<string>? Keys, string path)
        {
            try
            {
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.WriteLine(Header);
                    sw.WriteLine("<hr>");
                    sw.WriteLine("Процессы");
                    sw.WriteLine("<hr>");
                    if (Process != null)
                    {
                        foreach (var process in Process)
                        {
                            sw.WriteLine($"<p>{process}</p>");
                        }
                    }
                    sw.WriteLine("<hr>");
                    sw.WriteLine("Нажатые клавиши");
                    sw.WriteLine("<hr>");
                    if(Keys != null)
                    {
                        foreach (var key in Keys)
                        {
                            sw.WriteLine($"<p>{key}</p>");
                        }
                    }
                    sw.WriteLine(Footer);
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }
}