using DevelApp.JsonSchemaBuilder;
using DevelApp.JsonSchemaBuilder.CodeGeneration;
using DevelApp.Workflow.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ModelBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            DummyClass dummyClass = new DummyClass();
            string prePath = ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar;


            string jsonSchemaString = prePath + "Workflow.Model" + Path.DirectorySeparatorChar + "JsonSchema" + Path.DirectorySeparatorChar + "";
            string assemblyPath = dummyClass.GetType().Assembly.Location;
            string jsonSchemaApplicationRoot = Path.GetFullPath(jsonSchemaString, assemblyPath);
            LoadAllJsonSchemaBuildersAndWriteSchemasToFile(jsonSchemaApplicationRoot);

            string csharpModelString = prePath + "Workflow" + Path.DirectorySeparatorChar + "GeneratedModel" + Path.DirectorySeparatorChar + "";
            string applicationRoot = Path.GetFullPath(csharpModelString, assemblyPath);
            LoadAllJsonSchemaBuildersAndGenerateCSharpCodeToFile(applicationRoot, jsonSchemaApplicationRoot);

        }

        #region Assembly load all schemas and write

        private static void LoadAllJsonSchemaBuildersAndGenerateCSharpCodeToFile(string applicationRoot, string jsonSchemaApplicationRoot)
        {
            CodeGenerator codeGenerator = new CodeGenerator(applicationRoot, jsonSchemaApplicationRoot);
            foreach (Type codeDefinedType in GetInterfaceTypes(typeof(IJsonSchemaDefinition)))
            {
                IJsonSchemaDefinition jsonSchema = GetJsonSchemaInstance(codeDefinedType);
                codeGenerator.Register(jsonSchema);
            }
            codeGenerator.Generate(Code.CSharp);
        }

        private static void LoadAllJsonSchemaBuildersAndWriteSchemasToFile(string pathString)
        {
            foreach (Type codeDefinedType in GetInterfaceTypes(typeof(IJsonSchemaDefinition)))
            {
                IJsonSchemaDefinition jsonSchema = GetJsonSchemaInstance(codeDefinedType);
                jsonSchema.WriteSchemaToFile(pathString);
            }
        }

        private static IEnumerable<Type> GetInterfaceTypes(Type interfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
        }

        private static IJsonSchemaDefinition GetJsonSchemaInstance(Type jsonSchemaType)
        {
            //Create dataowner instance
            IJsonSchemaDefinition jsonSchemaInstance = (IJsonSchemaDefinition)Activator.CreateInstance(jsonSchemaType);

            return jsonSchemaInstance;
        }

        #endregion
    }
}
