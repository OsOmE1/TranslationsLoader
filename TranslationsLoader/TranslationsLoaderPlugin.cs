using System.Collections.Generic;
using System.Linq;
using Il2CppInspector.PluginAPI.V100;
using Il2CppInspector.Reflection;

namespace TranslationsLoader
{
    public class TranslationsLoaderPlugin : IPlugin, ILoadPipeline
    {
        public string Id => "translations-loader";

        public string Name => "TranslationsLoader";

        public string Author => "OsOmE1";

        public string Version => "1.0.0";

        public string Description => "Performs deobfuscation with a nameTranslation.txt file";

        private PluginOptionFilePath TranslationsFileOption = new PluginOptionFilePath
        {
            Name = "translations",
            Description = "Path to nameTranslations.txt",
            MustExist = true,
            MustNotExist = false,
            IsFolder = false,
            Required = true
        };

        public List<IPluginOption> Options => new List<IPluginOption> { TranslationsFileOption };

        public void PostProcessTypeModel(TypeModel model, PluginPostProcessTypeModelEventInfo info)
        {
            Translations translations = new Translations(TranslationsFileOption.Value);
            List<TypeInfo> types = model.Types.Where(t => t.Assembly.ShortName == "Assembly-CSharp.dll").ToList();

            foreach(TypeInfo type in types)
            {
                DeobfuscateType(type, translations);
            }

            info.IsDataModified = true;
        }

        private void DeobfuscateType(TypeInfo type, Translations translations)
        {
            if (translations._translations.TryGetValue(type.Name, out string typeTranslation))
            {
                type.Name = typeTranslation;
            }

            foreach (FieldInfo field in type.DeclaredFields)
            {
                if (translations._translations.TryGetValue(field.Name, out string fieldTranslation))
                {
                    field.Name = fieldTranslation;
                }
            }

            foreach (PropertyInfo property in type.DeclaredProperties)
            {
                if (translations._translations.TryGetValue(property.Name, out string propertyTranslation))
                {
                    property.Name = propertyTranslation;
                }
            }

            foreach (MethodInfo method in type.DeclaredMethods)
            {
                if (translations._translations.TryGetValue(method.Name, out string methodTranslation))
                {
                    method.Name = methodTranslation;
                }
            }

            foreach (TypeInfo nestedType in type.DeclaredNestedTypes)
            {
                DeobfuscateType(nestedType, translations);
            }
        }
    }
}
