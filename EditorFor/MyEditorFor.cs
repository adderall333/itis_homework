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
            foreach (var str in type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .SelectMany(p => Process(new PropertyNode(p, obj, new Type[0]))))
                yield return str;
        }
        
        private static IEnumerable<string> Process(PropertyNode node)
        {
            var type = node.Property.PropertyType;
            yield return $"<div class='editor-label'><label for='{node.Property.Name}'>{node.Property.Name}</label></div>";

            if (InputTypes.Keys.Contains(type))
            {
                yield return GetInput(node.Property, node.Parent);
            }
            else if (type.IsEnum)
            {
                yield return GetSelect(node.Property, node.Parent);
            }
            else if (type.IsClass)
            {
                node.CheckType(type);
                node.AddType(type);
                foreach (var str in type
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .SelectMany(p => Process(new PropertyNode(p, node.GetValue(), node
                        .PreviousNodeTypes
                        .Append(node.Property.PropertyType)))))
                    yield return str;
            }
            else
            {
                throw new NotSupportedException("Модель содержит неподдерживаемые свойства");
            }
        }

        private static string GetInput(PropertyInfo property, object obj)
        {
            return $"<div class='editor-field'>" +
                   $"<input " +
                   $"type='{InputTypes[property.PropertyType]}' " +
                   $"id='{property.Name}' " +
                   $"name='{property.Name}' " +
                   $"value='{property.GetValue(obj)}'" +
                   $"{Checked(property, obj)}>" +
                   $"</div>";
        }

        private static string GetSelect(PropertyInfo property, object obj)
        {
            return $"<div class='editor-field'>" +
                   $"<select name='{property.Name}'>" +
                   property
                       .PropertyType
                       .GetEnumNames()
                       .Select(option =>
                           $"<option value='{option}'{Selected(property, obj, option)}>{option}</option>")
                       .Aggregate((current, next) => current + next) +
                   $"</select>" +
                   $"</div>";
        }

        private static string Selected(PropertyInfo property, object obj, string option)
        {
            return property.GetValue(obj).ToString() == option ? " selected" : "";
        }

        private static string Checked(PropertyInfo property, object obj)
        {
            return property.PropertyType == typeof(bool) && 
                   (bool) property.GetValue(obj) ? " checked" : "";
        }
    }

    class PropertyNode
    {
        public HashSet<Type> PreviousNodeTypes { get; set; }
        public PropertyInfo Property { get; set; }
        public object Parent { get; set; }
        

        public PropertyNode(PropertyInfo property, object parent, IEnumerable<Type> previousNodeTypes)
        {
            Property = property;
            Parent = parent;
            PreviousNodeTypes = new HashSet<Type>(previousNodeTypes);
        }
        
        public void CheckType(Type type)
        {
            if (PreviousNodeTypes.Contains(type))
                throw new NotSupportedException("Произошло зацикливание");
        }
        
        public void AddType(Type type)
        {
            PreviousNodeTypes.Add(type);
        }

        public object GetValue()
        {
            return Property.GetValue(Parent);
        }
    } 
}