using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.TestHelpers
{
    public static class ArrangeHelper
    {

        public static string GenerateTestData(TypeSyntax type)
        {
            string typeName = type.ToString();

            switch (typeName)
            {
                case "int":
                    return "10";
                case "string":
                    return "\"TestString\"";
                case "bool":
                    return "true";
                case "double":
                    return "3.14";
                case "DateTime":
                    return "DateTime.Now";
                default:
                    if (typeName.EndsWith("[]"))
                    {
                        string elementType = typeName.Substring(0, typeName.Length - 2);
                        return $"new {elementType}[] {{ {GenerateTestData(SyntaxFactory.ParseTypeName(elementType))} }}";
                    }
                    return "null";
            }
        }
        private static bool IsInterface(string typeName)
        {
            return typeName.StartsWith("I"); // Basic check - USE ROSLYN SEMANTIC MODEL FOR ROBUST CHECKING
        }

        private static bool IsPredefinedType(string typeName)
        {
            return typeName == "int" || typeName == "string" || typeName == "bool" || typeName == "double" || typeName == "DateTime";
        }
        private static string GetClassName(MethodDeclarationSyntax methodDeclaration)
        {
            var parent = methodDeclaration.Parent;
            while (parent != null && !(parent is ClassDeclarationSyntax))
            {
                parent = parent.Parent;
            }

            if (parent is ClassDeclarationSyntax classDeclaration)
            {
                return classDeclaration.Identifier.Text;
            }

            return null;
        }

        private static ConstructorInfo GetConstructor(string className)
        {
            //var type = Type.GetType(className);
            Assembly assembly;
            assembly = Assembly.LoadFrom("C:\\Users\\ganga\\source\\repos\\ConsoleApp2\\TestProj2\\bin\\Debug\\net8.0\\MyProject.dll");

            var tyepss = assembly.GetTypes();
            var type = tyepss.Where(x => x.Name == className).FirstOrDefault();
            if (type != null)
            {
                return type.GetConstructors().FirstOrDefault(); // Get the first constructor (you can add logic to choose a specific one)
            }
            return null;
        }

        public static string GenerateArrangeSection(MethodDeclarationSyntax methodDeclaration)
        {
            string arrangeCode = "";
            var parameters = methodDeclaration.ParameterList.Parameters;
            var className = GetClassName(methodDeclaration);
            var constructor = GetConstructor(className);

            // ... (Theory attribute handling - if applicable)
            Assembly assembly;
            assembly = Assembly.LoadFrom("C:\\Users\\ganga\\source\\repos\\ConsoleApp2\\TestProj2\\bin\\Debug\\net8.0\\MyProject.dll");


            if (constructor != null)
            {
                foreach (var parameter in constructor.GetParameters())
                {
                    var parameterType = parameter.ParameterType;
                    if (parameterType.IsInterface)
                    {
                        arrangeCode += $"Mock<{parameterType.Name}> mock{parameterType.Name} = new Mock<{parameterType.Name}>();\n"; // Declaration and initialization
                    }
                    else
                    {
                        // Handle concrete types - expand as needed
                        if (parameterType == typeof(string))
                        {
                            arrangeCode += $"{parameterType.Name} {parameter.Name} = $\"{parameter.Name}Value\";\n"; // Or some default string value
                        }
                        else if (parameterType == typeof(int))
                        {
                            arrangeCode += $"{parameterType.Name} {parameter.Name} = 123;\n"; // Or some default int value
                        }
                        else
                        {
                            arrangeCode += $"{parameterType.Name} {parameter.Name} = default({parameterType.Name});\n"; // Default value for other types
                        }
                    }
                }




            }
            else
            {
                arrangeCode += $"// WARNING: Could not find a suitable constructor for {className}.\n";
            }

            foreach (var parameter in parameters)
            {
                var typeName = parameter.Type.ToString();

                if (IsInterface(typeName))
                {


                    string mockVariableName = $"mock{typeName}";

                    // *** Correctly pass method parameter types to GenerateMockSetup ***
                    var methodParams = methodDeclaration.ParameterList.Parameters.Select(p => p.Type.ToString()).ToArray();
                    var returnType = methodDeclaration.ReturnType;

                    // *** Dynamic Logic for Mock Setup ***
                    // 1. Get the interface type
                    var interfaceType = Type.GetType(typeName);

                    var tyepss = assembly.GetTypes();
                    var type = tyepss.Where(x => x.Name == typeName).FirstOrDefault();// assembly.GetType("MyProject."+typeName); // Now get the type




                    if (type != null)
                    {
                        // 2. Find the methods of the interface
                        var interfaceMethods = type.GetMethods();

                        // 3. Iterate through the interface methods and generate mock setups
                        foreach (var interfaceMethod in interfaceMethods)
                        {
                            var interfaceMethodParams = interfaceMethod.GetParameters().Select(p => p.ParameterType.Name).ToArray();

                            // Generate mock setup for *each* method of the interface.
                            // You might want to add logic here to *filter* methods if needed.
                            arrangeCode += MockSetUpHelpers.GenerateMockSetup(mockVariableName, interfaceMethod.Name, interfaceMethod.Name, methodDeclaration.ReturnType, interfaceMethodParams);
                        }
                    }
                    else
                    {
                        arrangeCode += $"// WARNING: Could not load type {typeName} for dynamic mock setup.\n";
                    }


                    arrangeCode += $"var Obj{mockVariableName}_{parameter.Identifier.Text} = {mockVariableName}.Object;\n";

                }
                else if (!IsPredefinedType(typeName) && !typeName.EndsWith("[]"))
                {
                    typeName = parameter?.Type.ToString();

                    var tyepss = assembly.GetTypes();
                    var type = tyepss.Where(x => x.Name == typeName).FirstOrDefault();// assembly.GetType("MyProject."+typeName); // Now get the type


                    if (!IsInterface(typeName) && !IsPredefinedType(typeName) && !typeName.EndsWith("[]"))
                    {

                        arrangeCode += $"{typeName} {parameter.Identifier.Text} = new {typeName}();\n";

                        if (type != null)
                        {
                            arrangeCode += InitializePropertiesRecursivelyHelpers.InitializePropertiesRecursively(parameter.Identifier.Text, type, tyepss);
                        }
                        else
                        {
                            arrangeCode += $"// WARNING: Could not load type {typeName} for property initialization. Ensure the assembly is loaded.\n";
                        }




                    }
                    else
                    {
                        arrangeCode += $"var Objmock{typeName}_{parameter.Identifier.Text} = {GenerateTestData(parameter.Type)};\n";
                    }
                }
                else
                {
                    arrangeCode += $"var Objmock{typeName}_{parameter.Identifier.Text} = {GenerateTestData(parameter.Type)};\n";
                }
            }

            // Create the class instance here in Arrange
            arrangeCode += $"\nvar {className.ToLower()}Instance = new {className}(";

            bool firstParameter = true;
            foreach (var parameter in constructor.GetParameters())
            {
                if (!firstParameter)
                {
                    arrangeCode += ", ";
                }

                var parameterType = parameter.ParameterType;
                var parameterName = parameter.Name;

                if (parameterType.IsInterface)
                {
                    arrangeCode += $"mock{parameter.ParameterType.Name}.Object"; // Use the mock object directly
                }
                else
                {
                    arrangeCode += parameter.Name; // Use the concrete instance or value
                }

                firstParameter = false;
            }

            arrangeCode += ");\n\n"; // Close constructor call

            // Mocks for constructor parameters (declared but not initialized here)





            string singleTestMethod = $@"
    [TestMethod()]
    public void {methodDeclaration.Identifier.Text}Test()
    {{
        // Arrange
        {arrangeCode}

        // Act
        {GenerateActSectionHelpers.GenerateActSection(GetClassName(methodDeclaration), methodDeclaration.Identifier.Text, methodDeclaration)}

        // Assert
        {GenerateAssertSectionHelpers.GenerateAssertSection(methodDeclaration, null, null, 0)} 
    }}";

            return singleTestMethod;
        }



    }
}
