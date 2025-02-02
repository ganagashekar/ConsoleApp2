using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Xunit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;

namespace TestNamespaces
{
    public class UnitTestGenerator
    {
        public static string GenerateUnitTest(string code, string className, string methodName)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            ClassDeclarationSyntax classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                .FirstOrDefault(c => c.Identifier.Text == className);

            if (classDeclaration == null)
            {
                return "Class not found.";
            }

            MethodDeclarationSyntax methodDeclaration = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.Text == methodName);

            if (methodDeclaration == null)
            {
                return "Method not found.";
            }

            string testClassName = $"{className}Tests";
            string testMethodName = $"{methodName}Test";

            string unitTestCode = $@"
public class {testClassName}
{{
    {GenerateArrangeSection(methodDeclaration)}
}}";

            return unitTestCode;
        }

        //    private static string GenerateArrangeSection(MethodDeclarationSyntax methodDeclaration)
        //    {

        //        //// ... Inside GenerateArrangeSection, where you get the type:
        //        //var type = Type.GetType("YourNamespace.Person, YourAssembly"); // Replace with actual namespace and assembly name

        //        //// Example: If your Person class is in the MyProject namespace and MyLibrary assembly:
        //        //var type = Type.GetType("MyProject.Person, MyLibrary");

        //        //var type = Type.GetType("MyProject.Person, TestNamespaces");
        //        string arrangeCode = "";
        //        var parameters = methodDeclaration.ParameterList.Parameters;

        //        var theoryAttribute = methodDeclaration.AttributeLists
        //            .SelectMany(al => al.Attributes)
        //            .FirstOrDefault(a => a.Name.ToString() == "Theory");

        //        if (theoryAttribute != null)
        //        {
        //            // ... (Theory/InlineData handling remains the same - see previous responses)
        //        }

        //        foreach (var parameter in parameters)
        //        {
        //            var typeName = parameter.Type.ToString();

        //            if (!IsInterface(typeName) && !IsPredefinedType(typeName))
        //            {
        //                arrangeCode += $"var {parameter.Identifier.Text} = new {typeName}();\n";

        //                var type = Type.GetType(typeName);
        //                if (type == null)
        //                {
        //                    //type = Type.GetType("TestNamespaces.Person, ConsoleApp2");

        //                    var callingAssembly = Assembly.GetCallingAssembly();
        //                     type = callingAssembly.GetType("Person");
        //                }// Requires fully qualified name if not in same assembly!
        //                else if (type != null)
        //                {
        //                    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //                    foreach (var property in properties)
        //                    {
        //                        var propertyType = property.PropertyType.ToString();
        //                        var defaultValue = GetDefaultValue(propertyType);
        //                        arrangeCode += $"{parameter.Identifier.Text}.{property.Name} = {defaultValue};\n";
        //                    }
        //                }
        //                else
        //                {
        //                    arrangeCode += $"// WARNING: Could not load type {typeName} for property initialization. Ensure the assembly is loaded.\n";
        //                }

        //            }
        //            else
        //            {
        //                arrangeCode += $"var {parameter.Identifier.Text} = {GenerateTestData(parameter.Type)};\n";
        //            }
        //        }

        //        string singleTestMethod = $@"
        //[Fact]
        //public void {methodDeclaration.Identifier.Text}Test()
        //{{
        //    // Arrange
        //    {arrangeCode}

        //    // Act
        //    {GenerateActSection(GetClassName(methodDeclaration), methodDeclaration.Identifier.Text, methodDeclaration)}

        //    // Assert
        //    {GenerateAssertSection(methodDeclaration, null, null, 0)} 
        //}}";

        //        return singleTestMethod;
        //    }
        private static string InitializePropertiesRecursively(string objectName, Type type, Type[] tyepss, string currentPath = "")
        {
            string initializationCode = "";
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                var propertyName = property.Name;

                string fullPath = string.IsNullOrEmpty(currentPath) ? propertyName : $"{currentPath}.{propertyName}";
                var isclasstype = tyepss.Where(x => x.Name == property.Name).FirstOrDefault();
                //if (!IsInterface(propertyType.Name) && !IsPredefinedType(propertyType.Name) && !propertyType.IsArray)
                if (property.MemberType.ToString() == "Property" && isclasstype != null)
                {
                    //var nestedObject = Activator.CreateInstance(propertyType);
                    //initializationCode += $"{objectName}.{fullPath} = {nestedObject};\n";
                    initializationCode += InitializePropertiesRecursively(objectName, isclasstype, tyepss, fullPath); // Recursive call
                }
                //else if (propertyType.IsArray)
                //{
                //    var elementType = propertyType.GetElementType();
                //    if (elementType != null)
                //    {
                //        var arrayInstance = Array.CreateInstance(elementType, 1);
                //        var defaultValue = GetDefaultValue(elementType.Name);
                //        arrayInstance.SetValue(Convert.ChangeType(defaultValue, elementType), 0);
                //        initializationCode += $"{objectName}.{fullPath} = {arrayInstance};\n";
                //    }
                //}
                else
                {
                    var defaultValue = GetDefaultValue(propertyType.Name);
                    initializationCode += $"{objectName}.{fullPath} = {defaultValue};\n";
                }
            }

            return initializationCode;
        }
       
        private static string GenerateMockSetup(string mockName, string methodName, string TestMethodName, TypeSyntax returnType, params string[] parameterTypes)
        {

            string setupCode = "";
            string returnTypeName = returnType.ToString();
            Assembly assembly;
            assembly = Assembly.LoadFrom("C:\\Users\\ganga\\source\\repos\\ConsoleApp2\\TestProj2\\bin\\Debug\\net8.0\\MyProject.dll");

            var tyepss = assembly.GetTypes();
            var type = tyepss.Where(x => x.Name == returnTypeName).FirstOrDefault();// assembly.GetType("MyProject."+typeName); // Now get the type


            if (returnTypeName == "void")
            {
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

                setupCode += "));\n"; // No Returns for void methods
            }
            else if (returnTypeName.EndsWith("[]")) // Array return type
            {
                string elementTypeName = returnTypeName.Substring(0, returnTypeName.Length - 2);
                string defaultValue = GetDefaultValue(elementTypeName);

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

                setupCode += $")).Returns(new {elementTypeName}[] {{ {defaultValue} }});\n";

            }
            else if (type != null)
            {

               // Type returnTypeType = Type.GetType(returnTypeName);
                if (type != null)
                {

                    string returnObjectName = $"{mockName}Return{methodName}"; // Unique name
                    setupCode += $"{returnTypeName} {returnObjectName} = Activator.CreateInstance<{returnTypeName}>();\n"; // Initialization here

                    string initializationCode = InitializePropertiesRecursively(returnObjectName, type, tyepss); // Get the initialization code
                    setupCode += initializationCode;
                    string setupCodetest = $@"
mock{mockName}.Setup(x => x.{methodName}(";

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
                    setupCode += $"{returnTypeName} expected{TestMethodName} = {returnObjectName};\n"; // For use in Assert
                    setupCode += setupCodetest;


                    //                    var lambdaBody = new StringBuilder();

                    //                    // Declare the return object *outside* the lambda (but still within the setup method)
                    //                    lambdaBody.AppendLine($"{returnTypeName} returnObject;");

                    //                    lambdaBody.AppendLine($"returnObject = Activator.CreateInstance<{returnTypeName}>();"); // Initialize inside the lambda

                    //                    string initializationCode = InitializePropertiesRecursively("returnObject", type, tyepss);
                    //                    lambdaBody.Append(initializationCode); // Append the initialization code

                    //                    lambdaBody.AppendLine("return returnObject;");


                    //                    setupCode = $@"
                    //mock{mockName}.Setup(x => x.{methodName}(";

                    //                    // Handle parameters
                    //                    for (int i = 0; i < parameterTypes.Length; i++)
                    //                    {
                    //                        setupCode += $"It.IsAny<{parameterTypes[i]}>()";
                    //                        if (i < parameterTypes.Length - 1)
                    //                        {
                    //                            setupCode += ", ";
                    //                        }
                    //                    }

                    //                    setupCode += $")).Returns(() => {{{lambdaBody.ToString()}}});\n"; // Use the built lambda body


                    //                    // *** Corrected and Improved Lambda Expression using StringBuilder ***
                    //                    var lambdaBody = new StringBuilder();

                    //                    lambdaBody.AppendLine($"{returnTypeName} returnObject = Activator.CreateInstance<{returnTypeName}>();");

                    //                    string initializationCode = InitializePropertiesRecursively("returnObject", type, tyepss);
                    //                    lambdaBody.Append(initializationCode); // Append the initialization code

                    //                    lambdaBody.AppendLine("return returnObject;");


                    //                    setupCode = $@"
                    //mock{mockName}.Setup(x => x.{methodName}(";

                    //                    // Handle parameters
                    //                    for (int i = 0; i < parameterTypes.Length; i++)
                    //                    {
                    //                        setupCode += $"It.IsAny<{parameterTypes[i]}>()";
                    //                        if (i < parameterTypes.Length - 1)
                    //                        {
                    //                            setupCode += ", ";
                    //                        }
                    //                    }

                    //                    setupCode += $")).Returns(() => {{{lambdaBody.ToString()}}});\n"; // Use the built lambda body
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

        private static ConstructorInfo GetConstructor(string className)
        {
            var type = Type.GetType(className);
            if (type != null)
            {
                return type.GetConstructors().FirstOrDefault(); // Get the first constructor (you can add logic to choose a specific one)
            }
            return null;
        }

        private static T CreateAndInitialize<T>(Type type, Type[] tyepss) where T : class
        {
            T returnObject = Activator.CreateInstance<T>();
            InitializePropertiesRecursively("returnObject", type, tyepss);
            return returnObject;
        }
        private static string GenerateArrangeSection(MethodDeclarationSyntax methodDeclaration)
        {
            string arrangeCode = "";
            var parameters = methodDeclaration.ParameterList.Parameters;

            // ... (Theory attribute handling - if applicable)
            Assembly assembly;
            assembly = Assembly.LoadFrom("C:\\Users\\ganga\\source\\repos\\ConsoleApp2\\TestProj2\\bin\\Debug\\net8.0\\MyProject.dll");

            foreach (var parameter in parameters)
            {
                var typeName = parameter.Type.ToString();

                if (IsInterface(typeName))
                {
                    arrangeCode += $"var mock{parameter.Identifier.Text} = new Mock<{typeName}>();\n"; // Corrected Line
                    if (typeName == "IDataService")
                    {
                        

                        // Generate the mock variable name for setup
                        string mockVariableName = $"mock{parameter.Identifier.Text}";
                        if (methodDeclaration.Identifier.Text == "ProcessData")
                        {
                            arrangeCode += GenerateMockSetup(parameter.Identifier.Text, "GetData", methodDeclaration.Identifier.Text, methodDeclaration.ReturnType);
                        }
                        else if (methodDeclaration.Identifier.Text == "LogData")
                        {
                            arrangeCode += GenerateMockSetup(parameter.Identifier.Text, "GetData",methodDeclaration.Identifier.Text, methodDeclaration.ReturnType);
                        }
                        arrangeCode += $"{typeName} {parameter.Identifier.Text} = {mockVariableName}.Object;\n"; // Corrected Line
                    }
                     
                    //arrangeCode += $"var {parameter.Identifier.Text} = mock{parameter.Identifier.Text}.Object;\n";
                    
                }
                else if (!IsPredefinedType(typeName) && !typeName.EndsWith("[]"))
                {
                    typeName = parameter?.Type.ToString();

                    var tyepss = assembly.GetTypes();
                    var type = tyepss.Where(x => x.Name == typeName).FirstOrDefault();// assembly.GetType("MyProject."+typeName); // Now get the type


                    if (!IsInterface(typeName) && !IsPredefinedType(typeName) && !typeName.EndsWith("[]"))
                    {
                        //arrangeCode += $"var {parameter.Identifier.Text} = new {typeName}();\n";

                        ////var type = Type.GetType(typeName);
                        //if (type != null)
                        //{
                        //    arrangeCode += InitializePropertiesRecursively(parameter.Identifier.Text, type, tyepss); // Call the recursive function
                        //}
                        //else
                        //{
                        //    arrangeCode += $"// WARNING: Could not load type {typeName} for property initialization. Ensure the assembly is loaded.\n";
                        //}


                        arrangeCode += $"var mock{parameter.Identifier.Text} = new Mock<{typeName}>();\n";

                        // *** IMPORTANT: Set up mock behavior here ***

                        // Example for IDataService.GetData():


                    }
                    else
                    {
                        arrangeCode += $"var {parameter.Identifier.Text} = {GenerateTestData(parameter.Type)};\n";
                    }
                }
                else
                {
                    arrangeCode += $"var {parameter.Identifier.Text} = {GenerateTestData(parameter.Type)};\n";
                }
            }



            var className = GetClassName(methodDeclaration);  // Or however you get the class name
            if (!methodDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.StaticKeyword))
            {
                arrangeCode += $"\nvar {className.ToLower()}Instance = new {className}(";

                // Add parameters to the constructor call
                bool firstParameter = true;
                foreach (var parameter in parameters)
                {
                    if (!firstParameter)
                    {
                        arrangeCode += ", ";
                    }

                    arrangeCode += parameter.Identifier.Text;
                    firstParameter = false;
                }

                arrangeCode += ");\n\n"; // Close constructor call
            }

            string singleTestMethod = $@"
    [Fact]
    public void {methodDeclaration.Identifier.Text}Test()
    {{
        // Arrange
        {arrangeCode}

        // Act
        {GenerateActSection(GetClassName(methodDeclaration), methodDeclaration.Identifier.Text, methodDeclaration)}

        // Assert
        {GenerateAssertSection(methodDeclaration, null, null, 0)} 
    }}";

            return singleTestMethod;
        }



        private static string GenerateTestData(TypeSyntax type)
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

        private static string GenerateActSection(string className, string methodName, MethodDeclarationSyntax methodDeclaration)
        {

            string actCode = "";
            var parameters = methodDeclaration.ParameterList.Parameters;

            // Check if the method is static
            bool isStatic = methodDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.StaticKeyword);

            if (isStatic)
            {
                // Static method call
                actCode += $"var result = {className}.{methodName}(";
            }
            else
            {
                // Instance method call - use the instance created in Arrange

                // *** KEY CHANGE: Pass constructor parameters to MyService in Act ***
                actCode += $"var result = new {className}(";

                bool firstParameter = true;
                foreach (var parameter in parameters)
                {
                    if (!firstParameter)
                    {
                        actCode += ", ";
                    }

                    actCode += parameter.Identifier.Text;
                    firstParameter = false;
                }
                actCode += $").{methodName}(";  // Call the method on the newly created instance

            }


            bool firstParameterCall = true; // Flag for method call parameters (distinct from constructor parameters)
            foreach (var parameter in parameters)
            {
                if (!firstParameterCall)
                {
                    actCode += ", ";
                }

                actCode += parameter.Identifier.Text;
                firstParameterCall = false;
            }

            actCode += ");\n";

            return actCode;

            //string actCode = "";
            //var parameters = methodDeclaration.ParameterList.Parameters;

            //// Check if the method is static
            //bool isStatic = methodDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.StaticKeyword);

            //if (isStatic)
            //{
            //    // Static method call
            //    actCode += $"var result = {className}.{methodName}(";
            //}
            //else
            //{
            //    // Instance method call - use the instance created in Arrange
            //    actCode += $"var result = {className.ToLower()}Instance.{methodName}("; // Use the instance
            //}


            //bool firstParameter = true;
            //foreach (var parameter in parameters)
            //{
            //    if (!firstParameter)
            //    {
            //        actCode += ", ";
            //    }

            //    actCode += parameter.Identifier.Text;
            //    firstParameter = false;
            //}

            //actCode += ");\n";

            //return actCode;

            //var classDeclaration = methodDeclaration.Parent as ClassDeclarationSyntax;

            //if (classDeclaration != null)
            //{
            //    if (methodDeclaration.ReturnType.ToString() != "void")
            //    {
            //        actCode = $"var result = new {classDeclaration.Identifier.Text}().{methodName}({parameters});\n";
            //    }
            //    else
            //    {
            //        actCode = $"new {classDeclaration.Identifier.Text}().{methodName}({parameters});\n";
            //    }
            //}
            //else
            //{
            //    actCode = "// Error: Could not determine class name.\n";
            //}

            //return actCode;
        }

        private static string GenerateAssertSection(MethodDeclarationSyntax methodDeclaration, string expectedValue, List<string> parameterNames, int argumentCount)
        {
            string assertCode = "";

            if (methodDeclaration.ReturnType.ToString() != "void")
            {
                var expectedmain = $"expected{methodDeclaration.Identifier.Text}";
                if (expectedValue != null)
                {
                    assertCode = $"Assert.Equal({expectedValue}, result);";
                }
                else
                {
                    var returnValueType = methodDeclaration.ReturnType.ToString();

                    if (returnValueType.EndsWith("[]"))
                    {
                        assertCode = $"Assert.True(result.SequenceEqual(new {returnValueType.Substring(0, returnValueType.Length - 2)}[] {{ {GenerateExpectedArrayValues(parameterNames, argumentCount, returnValueType)} }}));";
                    }
                    else
                    {
                        Assembly assembly;
                        assembly = Assembly.LoadFrom("C:\\Users\\ganga\\source\\repos\\ConsoleApp2\\TestProj2\\bin\\Debug\\net8.0\\MyProject.dll");

                        var tyepss = assembly.GetTypes();
                        var type = tyepss.Where(x => x.Name == returnValueType).FirstOrDefault();// assembly.GetType("MyProject."+typeName); // Now get the type
                        if (type != null)
                        {
                            expectedValue = expectedmain;
                        }
                        else
                        {

                            expectedValue = GetExpectedValue(methodDeclaration.ReturnType);
                        }
                        assertCode = $"Assert.Equal({expectedValue}, result);";
                    }
                }

            }

            return assertCode;
        }

        private static string GenerateExpectedArrayValues(List<string> parameterNames, int argumentCount, string returnValueType)
        {
            string expectedValues = "";

            // Example logic (REPLACE WITH YOUR ACTUAL LOGIC - THIS IS JUST A PLACEHOLDER)
            if (argumentCount > 0)
            {
                if (returnValueType == "int[]")
                {
                    if (parameterNames.Count >= 2)
                    {
                        expectedValues = $"{parameterNames[0]} + {parameterNames[1]}"; // Example: a + b
                    }
                    else
                    {
                        expectedValues = "1, 2, 3"; // Default values
                    }
                }
                else if (returnValueType == "string[]")
                {
                    expectedValues = $"\"{parameterNames[0]}1\", \"{parameterNames[0]}2\""; // Example
                }
                // Add more types as needed
            }
            else
            {
                if (returnValueType == "int[]")
                {
                    expectedValues = "1, 2, 3";
                }
                else if (returnValueType == "string[]")
                {
                    expectedValues = "\"Value1\", \"Value2\"";
                }
            }

            return expectedValues;
        }

        private static string GetExpectedValue(TypeSyntax returnType)
        {
            string typeName = returnType.ToString();
            switch (typeName)
            {
                case "int":
                    return "15"; // Example - NEEDS REAL LOGIC
                case "string":
                    return "\"Expected String\""; // Example - NEEDS REAL LOGIC
                case "bool":
                    return "true"; // Example - NEEDS REAL LOGIC
                    return "6.28"; // Example - NEEDS REAL LOGIC
                case "DateTime":
                    return "DateTime.Now"; // Example - NEEDS REAL LOGIC
                default:
                    return "null"; // Placeholder
            }
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

        private static bool IsInterface(string typeName)
        {
            return typeName.StartsWith("I"); // Basic check - USE ROSLYN SEMANTIC MODEL FOR ROBUST CHECKING
        }

        private static bool IsPredefinedType(string typeName)
        {
            return typeName == "int" || typeName == "string" || typeName == "bool" || typeName == "double" || typeName == "DateTime";
        }

        private static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; // Characters to use
            var random = new Random(); // Create a Random instance

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //private static string GetDefaultValue(string typeName)
        //{
        //    //private static Random _random = new Random();
        //    switch (typeName.ToLower())
        //    {
        //        case "int": return new Random().Next(100).ToString();;
        //        case "int": return new Random().Next(100).ToString(); ;
        //        case "string": return "\"\"";
        //        case "bool": return "false";
        //        case "double": return "0.0";
        //        case "DateTime": return "DateTime.Now";
        //        default: return "null"; // Handle other types as needed
        //    }
        //}

        private static string GetDefaultValue(string typeName)
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

        private static AssemblyName GetAssemblyName(string typeName)
        {
            // Very basic.  Needs to be more robust (handle generics, nested types, etc.)
            var parts = typeName.Split('.');
            if (parts.Length > 0)
            {
                return new AssemblyName(parts[0]); // Assumes the root namespace is the assembly name
            }
            return null;
        }


        public static void Main(string[] args)
        {
            //            string code = @"
            //namespace MyProject  // Example namespace
            //{
            //    public class Address
            //    {
            //        public string Street { get; set; }
            //        public int HouseNumber { get; set; }
            //        public string City { get; set; }
            //    }

            //    public class Person
            //    {
            //        public string Name { get; set; }
            //        public int Age { get; set; }
            //        public Address Address { get; set; }

            //        public void UpdateAddress(Address address)
            //        {
            //            this.Address = address;
            //        }
            //    }

            //    public class MyService
            //    {
            //        public void ProcessPerson(Person person)
            //        {
            //            // ... use the person object
            //        }

            //        public void UpdatePersonAddress(Person person)
            //        {
            //            person.UpdateAddress(new Address { Street = ""New Street"" });
            //        }

            //        public int[] GetEvenNumbers(int limit)
            //        {
            //            return Enumerable.Range(1, limit).Where(x => x % 2 == 0).ToArray();
            //        }
            //    }
            //}
            //"; 
            string code = System.IO.File.ReadAllText(@"C:\Users\ganga\source\repos\ConsoleApp2\TestProj2\Class1.cs");

            string className = "MyService";
            string methodName = "ProcessData";

            //string unitTest = GenerateUnitTest(code, className, methodName);
            //Console.WriteLine(unitTest);

            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}Tests.cs");
            //File.WriteAllText(filePath, unitTest);
            //Console.WriteLine($"Unit tests saved to: {filePath}");


            //methodName = "UpdatePersonAddress";
            //unitTest = GenerateUnitTest(code, className, methodName);
            //Console.WriteLine(unitTest);

            //filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}Tests.cs");
            //File.AppendAllText(filePath, unitTest);
            //Console.WriteLine($"Unit tests saved to: {filePath}");

            //methodName = "GetEvenNumbers";
            //unitTest = GenerateUnitTest(code, className, methodName);
            //Console.WriteLine(unitTest);

            //filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}Tests.cs");
            //File.AppendAllText(filePath, unitTest);
            //Console.WriteLine($"Unit tests saved to: {filePath}");


            string unitTest = GenerateUnitTest(code, className, methodName);
            Console.WriteLine(unitTest);

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}Tests.cs");
            File.WriteAllText(filePath, unitTest);
            Console.WriteLine($"Unit tests saved to: {filePath}");

            methodName = "LogData";
            unitTest = GenerateUnitTest(code, className, methodName);
            Console.WriteLine(unitTest);

            filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}Tests.cs");
            File.AppendAllText(filePath, unitTest);
            Console.WriteLine($"Unit tests saved to: {filePath}");

        }
    }
}