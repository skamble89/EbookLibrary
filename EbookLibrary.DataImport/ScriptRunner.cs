using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EbookLibrary.DataImport
{
    public class ScriptRunner
    {
        public async Task RunScript(string name)
        {
            var types = GetAllScripts();

            if (types.ContainsKey(name))
            {
                var script = types[name];
                var obj = script.GetConstructor(System.Type.EmptyTypes).Invoke(new object[] { }) as IScript;
                await obj.ExecuteAsync();
            }
            else
            {
                throw new ArgumentException(string.Format("Script '{0}' not found.", name));
            }
        }

        #region Private methods
        private Dictionary<string, Type> GetAllScripts()
        {
            var result = new Dictionary<string, Type>();
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            foreach (Type t in types)
            {
                var attrs = t.GetCustomAttributes().ToList();
                attrs.ForEach(x =>
                {
                    var attr = x as ScriptAttribute;
                    if (attr != null)
                    {
                        result.Add(attr.Name, t);
                    }
                });
            }

            return result;
        }
        #endregion
    }
}
