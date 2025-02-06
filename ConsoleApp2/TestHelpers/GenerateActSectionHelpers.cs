using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.TestHelpers
{
    public static  class GenerateActSectionHelpers
    {
        public static string GenerateActSection(string className, string methodName, MethodDeclarationSyntax methodDeclaration)
        {

            string actCode = "";
            var parameters = methodDeclaration.ParameterList.Parameters;
            bool isStatic = methodDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.StaticKeyword);
            string returnType = methodDeclaration.ReturnType.ToString(); // Get the return type


            if (returnType != "void") // Check for void return type
            {
                if (isStatic)
                {
                    actCode += $"var result = {className}.{methodName}(";
                }
                else
                {
                    actCode += $"var result = {className.ToLower()}Instance.{methodName}(";
                }
            }
            else // Handle void return type
            {
                if (isStatic)
                {
                    actCode += $"{className}.{methodName}("; // No result variable for void methods
                }
                else
                {
                    actCode += $"{className.ToLower()}Instance.{methodName}(";
                }
            }

            
            bool firstParameterCall = true; // Flag for method call parameters (distinct from constructor parameters)
            foreach (var parameter in parameters)
            {
                if (!firstParameterCall)
                {
                    actCode += ", ";
                }

                actCode += $"Objmock{parameter.Type.ToString()}_{parameter.Identifier.Text}";// parameter.Identifier.Text;
                firstParameterCall = false;
            }

            actCode += ");\n";

            return actCode;


        }
    }
}
