using ConsoleApp2.TestHelpers;
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
            string testClassName = $"{className}Tests";

            string unitTestCode = $@"public class {testClassName} {{";
            var methodDeclarations = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var method in methodDeclarations.Where(x => string.IsNullOrEmpty(methodName) || x.Identifier.Text == methodName))
            {
                string testMethodName = $"{method.Identifier.Text}Test";
                //MethodDeclarationSyntax methodDeclaration = methodDeclarations
                //.FirstOrDefault(m => m.Identifier.Text == methodName);

                if (method == null)
                {
                    return "Method not found.";
                }
                else
                {
                    unitTestCode += ArrangeHelper.GenerateArrangeSection(method);
                }
            }

            unitTestCode += "}";

            return unitTestCode;
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


            //string unitTest = CodeAligner.AlignAssignments(GenerateUnitTest(code, className, methodName));
            //Console.WriteLine(unitTest);

            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}Tests.cs");
            //File.WriteAllText(filePath, unitTest);
            //Console.WriteLine($"Unit tests saved to: {filePath}");

            //methodName = "LogData";
            //unitTest = GenerateUnitTest(code, className, methodName);
            //Console.WriteLine(unitTest);


            //methodName = "ProcessDataSummaryData";
            methodName = "";
            string unitTest = CodeAligner.AlignAssignments(GenerateUnitTest(code, className, methodName));
            Console.WriteLine(unitTest);



            //methodName = "Sum";
            //unitTest = GenerateUnitTest(code, className, methodName);
            //Console.WriteLine(unitTest);
            //filePath = Path.Combine(Directory.GetCurrentDirectory(), $"{className}Tests.cs");
            //File.AppendAllText(filePath, unitTest);
            //Console.WriteLine($"Unit tests saved to: {filePath}");

        }
    }
}