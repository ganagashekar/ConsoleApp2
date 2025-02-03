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

            // Check if the method is static
            bool isStatic = methodDeclaration.Modifiers.Any(m => m.Kind() == SyntaxKind.StaticKeyword);

            if (isStatic)
            {
                // Static method call
                actCode += $"var result = {className}.{methodName}(";
            }
            else
            {

                actCode += $"var result={className.ToLower()}Instance.{methodName}(";
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
