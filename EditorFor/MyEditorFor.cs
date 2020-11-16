using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EditorFor
{
    public static class MyEditorFor
    {
        public static IHtmlContent MyEditorForModel(this IHtmlHelper helper, object model)
        {
            var content = new HtmlContentBuilder();
            foreach (var str in GetForm(model))
            {
                content.AppendHtml(str);
            }

            return content;
        }
        
        private static readonly Dictionary<Type, string> InputTypes = new Dictionary<Type, string>
        {
            {typeof(int), "number"},
            {typeof(long), "number"},
            {typeof(bool), "checkbox"},
            {typeof(string), "text"}
        };

        public static IEnumerable<string> GetForm(object obj)
        {
            var type = obj.GetType();
            var root = new PropertyNode(type, obj);
            foreach (var str in type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SelectMany(p => Process(new PropertyNode(p, root, p.GetValue(obj)))))
                yield return str;
        }
        
        private static IEnumerable<string> Process(PropertyNode node)
        {
            yield return $"<div class='editor-label'><label for='{node.Property.Name}'>{node.Property.Name}</label></div>";

            if (InputTypes.Keys.Contains(node.Type))
            {
                yield return GetInput(node);
            }
            else if (node.Type.IsEnum)
            {
                yield return GetSelect(node);
            }
            else if (node.Type.IsClass)
            {
                node.Parent.CheckType(node.Type);
                foreach (var str in node
                    .Type
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .SelectMany(p => Process(new PropertyNode(p, node, p.GetValue(node.Value)))))
                    yield return str;
            }
            else
            {
                throw new NotSupportedException("Модель содержит неподдерживаемые свойства");
            }
        }

        private static string GetInput(PropertyNode node)
        {
            return $"<div class='editor-field'>" +
                   $"<input " +
                   $"type='{InputTypes[node.Type]}' " +
                   $"id='{node.Property.Name}' " +
                   $"name='{node.Property.Name}' " +
                   $"value='{node.Value}'" +
                   $"{Checked(node)}>" +
                   $"</div>";
        }

        private static string GetSelect(PropertyNode node)
        {
            return $"<div class='editor-field'>" +
                   $"<select name='{node.Property.Name}'>" +
                   node
                       .Type
                       .GetEnumNames()
                       .Select(option =>
                           $"<option value='{option}'{Selected(node, option)}>{option}</option>")
                       .Aggregate((current, next) => current + next) +
                   $"</select>" +
                   $"</div>";
        }

        private static string Selected(PropertyNode node, string option)
        {
            return node.Value.ToString() == option ? " selected" : "";
        }

        private static string Checked(PropertyNode node)
        {
            return node.Type == typeof(bool) && 
                   (bool) node.Value ? " checked" : "";
        }
    }

    class PropertyNode
    {
        public PropertyInfo Property { get; set; }
        public PropertyNode Parent { get; set; }
        public object Value { get; set; }
        
        public Type Type { get; set; }

        public PropertyNode(Type type, object obj)
        {
            Value = obj;
            Type = obj.GetType();
        }
        
        public PropertyNode(PropertyInfo property, PropertyNode parent, object obj)
        {
            Property = property;
            Parent = parent;
            Value = obj;
            Type = Property.PropertyType;
        }
        
        public void CheckType(Type type)
        {
            if (Type == type)
                throw new NotSupportedException("Произошло зацикливание");

            Parent?.CheckType(type);
        }
    } 
}