using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System.Linq;
using System.Runtime.Serialization;
using Formatter = Microsoft.CodeAnalysis.Formatting.Formatter;

public static class CodeAligner
{
    public static string AlignAssignments(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        var root = tree.GetCompilationUnitRoot();

        var rewriter = new AssignmentAlignRewriter();
        var newRoot = rewriter.Visit(root);

        // Apply standard formatting after alignment (important!)
        var formattedRoot = Formatter.Format(newRoot, new AdhocWorkspace());

        return formattedRoot.ToFullString();
    }

    private class AssignmentAlignRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        {
            // Handle only single-variable declarations for simplicity in this example.
            // You would need to extend this to handle multiple variable declarations in a single statement
            // if that's a requirement for your use case.  The logic would be similar, but you would
            // iterate through the variables in the declaration.
            if (node.Declaration.Variables.Count != 1) return node;


            // Find assignment expressions within the local declaration
            var assignments = node.DescendantNodes().OfType<AssignmentExpressionSyntax>().ToList();

            if (assignments.Count <= 1) return node; // Nothing to align if there's only one or zero assignments

            int maxLeftLength = assignments.Max(a => a.Left.ToString().Length);

            var newNodes = assignments.Select(assignment =>
            {
                int padding = maxLeftLength - assignment.Left.ToString().Length;

                // Create a new assignment with adjusted whitespace *before* the operator.
                var newAssignment = assignment.WithOperatorToken(
                    assignment.OperatorToken.WithLeadingTrivia(SyntaxFactory.Whitespace(new string(' ', padding + 1)))); // +1 for space after =

                return newAssignment;
            }).ToList();


            // The tricky part:  We need to reconstruct the LocalDeclarationStatement.  We can't just replace the assignments
            // directly.  We replace the *initializer* of the variable declaration with the new assignment.
            var firstVariable = node.Declaration.Variables.First();
            var newVariable = firstVariable.WithInitializer(SyntaxFactory.EqualsValueClause(newNodes[0])); // Assuming only one assignment for now

            var newDeclaration = node.Declaration.WithVariables(SyntaxFactory.SeparatedList<VariableDeclaratorSyntax>(new[] { newVariable }));


            return node.WithDeclaration(newDeclaration);
        }


        // Example of handling multiple variables in a declaration (more complex)
        // public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node)
        // {
        //     // ... (logic to find assignments like before) ...

        //     // ... (calculate maxLeftLength) ...

        //     // ... (create new assignments with padding) ...

        //     // Reconstruct the variable declarators and the declaration.  This part is more involved
        //     // when you have multiple variables. You'd need to match up the new assignments with
        //     // the corresponding variables.
        //     // ...

        //     return node.WithDeclaration(newDeclaration);
        // }
    }



//    public static void Main(string[] args)
//    {
//        string code = @"
//int x = 1;
//double y = 2.0;
//string message = ""hello"";
//bool z = true;";

//        string alignedCode = AlignAssignments(code);
//        Console.WriteLine(alignedCode);



//        string code2 = @"
//int x = 1, y = 2;
//double z = 3.0;";

//        string alignedCode2 = AlignAssignments(code2);
//        Console.WriteLine(alignedCode2);


//    }
}