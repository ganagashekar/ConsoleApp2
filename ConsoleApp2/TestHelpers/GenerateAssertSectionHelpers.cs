using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.TestHelpers
{
    public class GenerateAssertSectionHelpers
    {

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
        public static string GenerateAssertSection(MethodDeclarationSyntax methodDeclaration, string expectedValue, List<string> parameterNames, int argumentCount)
        {
            string assertCode = "";

            if (methodDeclaration.ReturnType.ToString() != "void")
            {
                //var expectedmain = $"expected{methodDeclaration.Identifier.Text}";
                var expectedmain = $"expected_results";

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

    }
}
