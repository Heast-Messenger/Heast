using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerators;

[Generator]
public class PacketSourceGenerator : ISourceGenerator
{
    private static string PacketSourceTemplate => """
                                                  namespace {{namespace_identifier}}
                                                  {
                                                      public partial class {{class_identifier}}
                                                      {
                                                          public {{class_identifier}}(PacketBuf buf)
                                                          {
                                                              {{buffer_read_properties}}
                                                          }
                                                          
                                                          public override void Write(PacketBuf buf)
                                                          {
                                                              {{buffer_write_properties}}
                                                          }
                                                      }
                                                  }
                                                  """;

    private static string BufferReadProperty => "{{property_identifier}} = buf.{{read_action}}();";
    private static string BufferWriteProperty => "buf.{{write_action}}({{property_identifier}});";

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new PacketFinder());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is PacketFinder packetFinder)
        {
            var packets = packetFinder.Packets;

            foreach (var packet in packets)
            {
                var namespaceIdentifier = string.Empty;
                if (packet.Parent is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclarationSyntax)
                {
                    namespaceIdentifier = fileScopedNamespaceDeclarationSyntax.Name.NormalizeWhitespace().ToFullString();
                }

                if (packet.Parent is NamespaceDeclarationSyntax namespaceDeclarationSyntax)
                {
                    namespaceIdentifier = namespaceDeclarationSyntax.Name.NormalizeWhitespace().ToFullString();
                }

                var packetIdentifier = packet.Identifier.NormalizeWhitespace().ToFullString();

                var properties = packet.Members
                    .Where(m => m is PropertyDeclarationSyntax)
                    .Cast<PropertyDeclarationSyntax>();

                var readActions = new List<string>();
                var writeActions = new List<string>();

                foreach (var property in properties)
                {
                    var attributes = property.AttributeLists
                        .SelectMany(a => a.Attributes)
                        .Select(a => a.Name.NormalizeWhitespace().ToFullString());

                    if (attributes.Any(s => s == "DisableAutoGenerate"))
                    {
                        continue;
                    }

                    var propertyType = property.Type.NormalizeWhitespace().ToFullString();
                    var methodName = propertyType switch
                    {
                        "int" => "VarInt",
                        "bool" => "Boolean",
                        "long" => "Long",
                        "double" => "Double",
                        "float" => "Float",
                        "string" => "String",
                        "byte[]" => "ByteArray",
                        "DateTime" => "Timestamp",
                        "Guid" => "Guid",
                        _ => $"Enum<{propertyType}>"
                    };

                    var propertyIdentifier = property.Identifier.NormalizeWhitespace().ToFullString();

                    readActions.Add(BufferReadProperty
                        .Replace("{{property_identifier}}", propertyIdentifier)
                        .Replace("{{read_action}}", $"Read{methodName}"));

                    writeActions.Add(BufferWriteProperty
                        .Replace("{{property_identifier}}", propertyIdentifier)
                        .Replace("{{write_action}}", $"Write{methodName}"));
                }

                var source = PacketSourceTemplate
                    .Replace("{{namespace_identifier}}", namespaceIdentifier)
                    .Replace("{{class_identifier}}", packetIdentifier)
                    .Replace("{{buffer_read_properties}}", string.Join("\n            ", readActions))
                    .Replace("{{buffer_write_properties}}", string.Join("\n            ", writeActions));

                context.AddSource($"{packetIdentifier}.g.cs", source);
            }
        }
    }

    private class PacketFinder : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> Packets { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax packet)
            {
                var attributes = packet.AttributeLists
                    .SelectMany(a => a.Attributes)
                    .Select(a => a.Name.NormalizeWhitespace().ToFullString());

                var baseTypes = packet.BaseList?.Types
                    .Select(t => t.Type.NormalizeWhitespace().ToFullString()) ?? new List<string>();

                var modifiers = packet.Modifiers
                    .Select(m => m.NormalizeWhitespace().ToFullString());

                if (baseTypes.Any(s => s == "AbstractPacket") && modifiers.Any(s => s == "partial") && attributes.All(s => s != "DisableAutoGenerate"))
                {
                    Packets.Add(packet);
                }
            }
        }
    }
}