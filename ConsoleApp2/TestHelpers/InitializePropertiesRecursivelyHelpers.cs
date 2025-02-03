using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.TestHelpers
{
    public static  class InitializePropertiesRecursivelyHelpers
    {

        public static string InitializePropertiesRecursively(string objectName, Type type, Type[] tyepss, string currentPath = "")
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
                    var defaultValue = MockSetUpHelpers.GetDefaultValue(propertyType.Name);
                    initializationCode += $"{objectName}.{fullPath} = {defaultValue};\n";
                }
            }

            return initializationCode;
        }
    }
}
