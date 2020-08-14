using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XMLExportDC
{
    public class DeserializedElement
    {
        /// <summary>
        /// XElement object of the deserialized element.
        /// </summary>
        private XElement Element { get; set; }

        /// <summary>
        /// Element name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Element children
        /// </summary>
        public List<DeserializedElement> Children { get; private set; }
        /// <summary>
        /// Element value
        /// </summary>
        public string Value { get; private set; }
        /// <summary>
        /// Element attributes
        /// </summary>
        public Dictionary<string, string> Attributes { get; private set; }

        public bool HasSubchildren
        {
            get
            {
                return (Children.Count != 0);
            }
        }

        /// <summary>
        /// Deserializes the given XElement
        /// </summary>
        /// <param name="element"></param>
        public DeserializedElement(XElement element)
        {
            Element = element;

            SetName();
            SetAttributes();
            SetValue();
            SetChildren();
        }

        /// <summary>
        /// Creates a new DeserializedElement with the given params.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="value"></param>
        /// <param name="children"></param>
        public DeserializedElement(string name = null, Dictionary<string, string> attributes = null, string value = null, List<DeserializedElement> children = null)
        {
            Name = name;
            Attributes = attributes ?? new Dictionary<string, string>();
            Value = value;
            Children = children ?? new List<DeserializedElement>();
        }

        private void SetName()
        {
            Name = Element.Name.LocalName;
        }

        private void SetAttributes()
        {
            Attributes = new Dictionary<string, string>();
            foreach (XAttribute attribute in Element.Attributes())
            {
                Attributes.Add(attribute.Name.LocalName, NormalizeAttributeValue(attribute.Value));
            }
        }

        private void SetValue()
        {
            IEnumerable<XText> textNodes = Element.Nodes().OfType<XText>();
            if (textNodes.Count() != 0)
            {
                Value = textNodes.First().Value;
            }
        }

        private void SetChildren()
        {
            Children = new List<DeserializedElement>();
            foreach (XElement childElement in Element.Elements())
            {
                Children.Add(new DeserializedElement(childElement));
            }
        }

        /// <summary>
        /// Turns attributes of this element into children elements.
        /// </summary>
        public void TurnAttributesIntoChildren()
        {
            foreach (var attribute in Attributes)
            {
                Children.Add(new DeserializedElement(name: attribute.Key, value: attribute.Value));
            }

            Attributes.Clear();
        }

        /// <summary>
        /// Turns value of this element into a child element.
        /// </summary>
        public void TurnValueIntoChild()
        {
            if (Value != null || Value != "")
            {
                Children.Add(new DeserializedElement(name: Name + "_value", value: Value));
                Value = null;
            }
        }

        /// <summary>
        /// Removes unnecessary characters from the value
        /// </summary>
        /// <param name="attributeValue">Unnormalized value</param>
        /// <returns>Normalized value string</returns>
        protected string NormalizeAttributeValue(string attributeValue)
        {
            string returnString = attributeValue.Replace("\n", "");
            returnString = returnString.Replace("\r", "");
            returnString = returnString.Replace("\t", "");

            return returnString;
        }

        /// <summary>
        /// Finds children of this element with the given element name.
        /// </summary>
        /// <param name="elementName">The given element name.</param>
        /// <returns>Collection of elements with the given element name</returns>
        public IEnumerable<DeserializedElement> FindChildren(string elementName)
        {
            List<DeserializedElement> elementList = new List<DeserializedElement>();

            TraverseTreeAndAddElementsByName(this, elementList, elementName);

            return elementList;
        }

        private void TraverseTreeAndAddElementsByName(DeserializedElement element, List<DeserializedElement> list, string elementName)
        {
            if (element.Name == elementName) list.Add(element);

            foreach (var child in element.Children)
            {
                TraverseTreeAndAddElementsByName(child, list, elementName);
            }
        }
    }
}
