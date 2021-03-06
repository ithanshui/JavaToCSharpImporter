﻿using CodeConverterCore.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeConverterCore.Model
{
    /// <summary>
    /// Methodendefinition für eine Klasse
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [DebuggerDisplay("{ToString()}")]
    public class TypeContainer
    {
        public TypeContainer() { }

        public TypeContainer(string inName)
        {
            if (inName.EndsWith("[]"))
            {
                IsArray = true;
            }
            else if (inName.Contains("extends") || inName.Contains("<"))
            {
                throw new Exception("Name not allowed for TypeContainer Name");
            }
            Name = inName;
        }
        /// <summary>
        /// Typename (mostly another Class)
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// Global Type of this TypeContainer, can be checkt via Equals (ie. for Interfaces, BaseTypes, Generics)
        /// </summary>
        public BaseType Type { get; set; }

        /// <summary>
        /// List of Generic Sub-Types
        /// </summary>
        [JsonProperty]
        public List<TypeContainer> GenericTypes { get; set; } = new List<TypeContainer>();

        /// <summary>
        /// Extends from Generic Types
        /// </summary>
        public List<TypeContainer> Extends { get; set; } = new List<TypeContainer>();

        /// <summary>
        /// Is it an Array
        /// </summary>
        [JsonProperty]
        public bool IsArray { get; set; }

        /// <summary>
        /// Type as Nullable Type
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Inizialising Array Data
        /// </summary>
        public ICodeEntry ArrayInizialiser { get; set; }

        /// <summary>
        /// Convert String inTo usefull Type
        /// </summary>
        /// <param name="inType"></param>
        public static implicit operator TypeContainer(string inType)
        {
            var tmpContainer = new TypeContainer();
            if (inType.EndsWith("[]"))
            {
                tmpContainer.IsArray = true;
                inType = inType.Substring(0, inType.Length - 2);
            }
            if (inType.Contains("extends"))
            {
                tmpContainer.Name = inType.Split(' ')[0];
                var tmpExtends = inType.Substring(inType.IndexOf("extends"));

                tmpContainer.Extends = tmpExtends.Split(',').Select(inItem => inItem.Trim(' '))
                    .Select(inItem => new TypeContainer(inItem)).ToList();
            }
            else if (inType.Contains("<"))
            {
                tmpContainer.Name = inType.Substring(0, inType.IndexOf("<"));
                var tmpInnerData = inType.Substring(inType.IndexOf("<") + 1, inType.Length - inType.IndexOf("<") - 2);
                throw new Exception("Nope");
            }
            else
            {
                tmpContainer.Name = inType;
            }

            return tmpContainer;
        }

        /// <summary>
        /// Override ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (GenericTypes.Count == 0)
            {
                if (Extends.Count > 0)
                {
                    return $"{Name} extends {string.Join(", ", Extends)}{(IsArray ? $"[{ArrayInizialiser}]" : "")}";
                }
                return $"{Name}{(IsArray ? $"[{ArrayInizialiser}]" : "")}";
            }
            if (Extends.Count > 0)
            {
                return $"{Name}<{string.Join(", ", GenericTypes)} extends {string.Join(", ", Extends)}>{(IsArray ? $"[{ArrayInizialiser}]" : "")}";
            }
            return $"{Name}<{string.Join(", ", GenericTypes)}>{(IsArray ? $"[{ArrayInizialiser}]" : "")}";
        }

        /// <summary>
        /// OVerride Equals to be correct for our usage
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var tmpOther = obj as TypeContainer;
            if (tmpOther == null)
            {
                return obj == null;
            }
            if (tmpOther.Name != Name)
            {
                return false;
            }
            if (tmpOther.GenericTypes.Count != GenericTypes.Count)
            {
                return false;
            }
            for (var tmpI = 0; tmpI < GenericTypes.Count; tmpI++)
            {
                if (tmpOther.GenericTypes[tmpI] != GenericTypes[tmpI])
                {
                    return false;
                }
            }

            if (tmpOther.Extends.Count != Extends.Count)
            {
                return false;
            }
            for (var tmpI = 0; tmpI < Extends.Count; tmpI++)
            {
                if (tmpOther.Extends[tmpI] != Extends[tmpI])
                {
                    return false;
                }
            }
            return true;
        }

        public static TypeContainer Void = new TypeContainer { Type = BaseType.Void, Name = "void" };
    }
}
