namespace RifleRange
{
    public static class Helper
    {
        public static string GetFileNameWithOutGUID(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && fileName.IndexOf("_") > -1) fileName = fileName.Substring(fileName.IndexOf("_") + 1);

            return fileName;
        }
    }
}