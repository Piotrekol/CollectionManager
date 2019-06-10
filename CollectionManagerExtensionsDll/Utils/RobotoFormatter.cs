using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CollectionManagerExtensionsDll.Utils
{

    public static class RobotoFormatter
    {
        //------------------------------------------------------------------
        /// <summary>
        /// http://geekswithblogs.net/NotImpossible/archive/2009/01/19/named-format-strings.aspx
        /// Returns a formated string.
        /// The format may contain references to properties of this object.
        /// To add properties to an output, the format is:
        /// "Random Text={PropertyName}"
        /// If you like to add the characters '{' or '}', you have to mask them
        /// with '{{'/'}}'.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"><c>FormatException</c> in case of invalid format.</exception>
        /// <exception cref="ArgumentNullException"><c>format</c> is null.</exception>
        public static string RobotoFormat(this string format, object @object)
        {
            if (format == null) { throw new ArgumentNullException("format"); }
            if (format.Trim().Length == 0) { return format; }

            Descriptor[] descriptors = GetDescriptors(format);

            string result = format;
            Descriptor descriptor;
            object descriptorValue;

            //could be done with foreach, altough 'for' could be faster.
            for (int i = 0; i < descriptors.Length; i++)
            {
                descriptor = descriptors[i];
                descriptorValue = GetDescriptorValue(@object, descriptor);
                result = result.Replace(
                    string.Concat("{", descriptor.FullDescriptor, "}"),
                    descriptor.ValueToString(descriptorValue));
            }
            return result.Replace(@"{{", "{").Replace(@"}}", "}");
        }

        //---------------------------------------------------------
        private static object GetDescriptorValue(object @object, Descriptor descriptor)
        {
            object result = @object;
            foreach (var name in descriptor.Names)
            {
                if (@object is Dictionary<string, string> dict)
                {
                    return dict[name];
                }

                PropertyInfo descriptionInfo = result.GetType().GetProperty(name);
                if (descriptionInfo == null)
                {
                    throw new FormatException(string.Format("No property named '{0}' found. Invalid format!", descriptor));
                }
                result = descriptionInfo.GetValue(result, null);
            }
            return result;
        }


        //------------------------------------------------------------------
        /// <summary>
        /// Function to 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static Descriptor[] GetDescriptors(string format)
        {
            var list = new List<Descriptor>();
            StringBuilder descriptorBuilder = new StringBuilder(format.Length);

            bool isDescriptor = false;
            char character;
            for (int i = 0; i < format.Length; i++)
            {
                character = format[i];
                //ignore escaped opening/closing braces
                if ((character == '{' && GetNextChar(i, format) == '{') ||
                    (character == '}' && GetNextChar(i, format) == '}'))
                {
                    i++;
                    continue;
                }

                if (isDescriptor || character == '{') //we found beginning
                {
                    isDescriptor = true;
                    if (character != '}')
                    {
                        if (character != '{')
                        {
                            descriptorBuilder.Append(character);
                        }
                    }
                    else if (GetNextChar(i, format) != '}') //found an end (ignoring escaped closing)
                    {
                        var descriptor = new Descriptor(descriptorBuilder.ToString());
                        if (!list.Contains(descriptor)) { list.Add(descriptor); }

                        descriptorBuilder.Remove(0, descriptorBuilder.Length);
                        isDescriptor = false;
                    }
                }
            }
            if (isDescriptor) //found no end
            {
                throw new FormatException("No end for descriptor found. Please use {{ to encode a { in your format.");
            }
            return list.ToArray();
        }

        //---------------------------------------------------------
        private static char GetNextChar(int i, string format)
        {
            if (((i + 1) < format.Length))
            {
                return format[i + 1];
            }
            return ' ';
        }


        //---------------------------------------------------------
        /// <summary>
        /// This class encapsulates the handling of a descriptor.
        /// </summary>
        private class Descriptor : IEquatable<Descriptor>
        {
            public Descriptor(string descriptorValue)
            {
                if (descriptorValue == null) { throw new ArgumentNullException("descriptorValue"); }
                if (descriptorValue.Trim().Length == 0) { throw new ArgumentException("Empty descriptor not allowed.", "descriptorValue"); }

                Format = string.Empty;
                FullDescriptor = descriptorValue;

                string[] values = descriptorValue.Split(':');
                if (values.Length > 0)
                {
                    Names = values[0].Split('.');
                }
                if (values.Length > 1)
                {
                    Format = values[1];
                }
            }

            //---------------------------------------------------------
            public string[] Names { get; private set; }
            public string Format { get; private set; }
            public string FullDescriptor { get; private set; }

            //---------------------------------------------------------
            public bool HasFormat
            {
                get { return !string.IsNullOrEmpty(Format); }
            }

            //---------------------------------------------------------
            public string ValueToString(object value)
            {
                return string.Format(ToFormatString(), value);
            }


            //---------------------------------------------------------
            private string ToFormatString()
            {
                string formatString = "{0}";
                if (HasFormat)
                {
                    formatString = string.Concat("{0:", Format, "}");
                }
                return formatString;
            }

            //---------------------------------------------------------
            public bool Equals(Descriptor other)
            {
                return FullDescriptor.Equals(other.FullDescriptor);
            }
        }
    }

}