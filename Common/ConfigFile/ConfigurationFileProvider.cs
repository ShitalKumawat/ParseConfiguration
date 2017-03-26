using System.Configuration;

namespace Common
{
    public class ConfigurationFileProvider<T> : IProvider<T> where T : class
    {
        public T Read()
        {
            var sectionName = typeof(T).Name;

            return (T)ConfigurationManager.GetSection(sectionName);
        }


    }
}
