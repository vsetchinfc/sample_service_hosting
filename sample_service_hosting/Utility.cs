namespace VSC
{
    public static class Utility
    {
        public static string GetExecutingAssemblyLocation()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }
    }
}