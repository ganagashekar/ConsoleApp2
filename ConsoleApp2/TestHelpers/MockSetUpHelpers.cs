using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.TestHelpers
{
    public static class MockSetUpHelpers
    {
        public static string GetDefaultValue(string typeName)
        {
            //private static Random _random = new Random();
            switch (typeName.ToLower())
            {
                case "int": return new Random().Next(100).ToString(); ;
                case "int32": return new Random().Next(10000).ToString(); ;
                case "int64": return new Random().Next(10000).ToString(); ;
                case "string": return $"\"{GetRandomString(20)}, welcome!\"";
                case "bool": return "false";
                case "double": return "0.0";
                case "DateTime": return "DateTime.Now";
                default: return "null"; // Handle other types as needed
            }
        }

        private static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Characters to use
            var random = new Random(); // Create a Random instance

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateMockSetup(string mockName, string methodName, string TestMethodName, TypeSyntax returnType, params string[] parameterTypes)
        {

            string setupCode = "";
            string returnTypeName = returnType.ToString();
            Assembly assembly;
            assembly = Assembly.LoadFrom("C:\\Users\\ganga\\source\\repos\\ConsoleApp2\\TestProj2\\bin\\Debug\\net8.0\\MyProject.dll");

            var tyepss = assembly.GetTypes();
            var type = tyepss.Where(x => x.Name == returnTypeName).FirstOrDefault();// assembly.GetType("MyProject."+typeName); // Now get the type


            if (returnTypeName == "void")
            {
                setupCode = $"{mockName}.Setup(x => x.{methodName}(";

                // Handle parameters
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    setupCode += $"It.IsAny<{parameterTypes[i]}>()";
                    if (i < parameterTypes.Length - 1)
                    {
                        setupCode += ", ";
                    }
                }

                setupCode += "));\n"; // No Returns for void methods
            }
            else if (returnTypeName.EndsWith("[]")) // Array return type
            {
                string elementTypeName = returnTypeName.Substring(0, returnTypeName.Length - 2);
                string defaultValue = GetDefaultValue(elementTypeName);

                setupCode = $"{mockName}.Setup(x => x.{methodName}(";

                // Handle parameters
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    setupCode += $"It.IsAny<{parameterTypes[i]}>()";
                    if (i < parameterTypes.Length - 1)
                    {
                        setupCode += ", ";
                    }
                }

                setupCode += $")).Returns(new {elementTypeName}[] {{ {defaultValue} }});\n";

            }
            else if (type != null)
            {

                // Type returnTypeType = Type.GetType(returnTypeName);
                if (type != null)
                {

                    string returnObjectName = $"{mockName}Return{methodName}"; // Unique name
                    setupCode += $"{returnTypeName} {returnObjectName} = Activator.CreateInstance<{returnTypeName}>();\n"; // Initialization here

                    string initializationCode = InitializePropertiesRecursivelyHelpers.InitializePropertiesRecursively(returnObjectName, type, tyepss); // Get the initialization code
                    setupCode += initializationCode;
                    string setupCodetest = $@"{mockName}.Setup(x => x.{methodName}(";

                    // Handle parameters
                    for (int i = 0; i < parameterTypes.Length; i++)
                    {
                        setupCodetest += $"It.IsAny<{parameterTypes[i]}>()";
                        if (i < parameterTypes.Length - 1)
                        {
                            setupCodetest += ", ";
                        }
                    }

                    // Lambda now includes the initialization code
                    setupCodetest += $")).Returns(() => {{  return {returnObjectName}; }});\n"; // Initialization code is now *used*

                    // Add the return object to the Arrange section for use in Assert
                    // setupCode += $"{returnTypeName} expected{TestMethodName} = {returnObjectName};\n"; // For use in Assert
                    setupCode += $"{returnTypeName} expected_results = {returnObjectName};\n"; // For use in Assert

                    setupCode += setupCodetest;

                }
            }
            else
            {


                string defaultValue = GetDefaultValue(returnTypeName);

                setupCode = $"mock{mockName}.Setup(x => x.{methodName}(";

                // Handle parameters
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    setupCode += $"It.IsAny<{parameterTypes[i]}>()";
                    if (i < parameterTypes.Length - 1)
                    {
                        setupCode += ", ";
                    }
                }

                setupCode += $")).Returns({defaultValue});\n";
            }

            return setupCode;
        }



    }
}
