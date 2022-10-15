using System.Text;

namespace savefile
{
    public static class SaveFile
    {
        public static bool Save(string path, Action<StreamWriter> HowSaveFile)
        {
            try
            {
                using (var sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    HowSaveFile(sw);
                }
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public static IEnumerable<string> Read(string path)
        {
            var listprocess = new List<string>();
            try
            {                
                using (var sr = new StreamReader(path))
                {
                    string? info;
                    while ((info = sr.ReadLine()) != null)
                    {
                        listprocess.Add(info);
                    }
                }
            }
            catch(Exception e)
            {

            }
            return listprocess as IEnumerable<string>;
           
        }
    }
}