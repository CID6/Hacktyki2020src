using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RabbitEntityConsumer
{
    public class DeserializedElement
    {
        private XElement Element { get; set; }

        public string Name { get; private set; }
        public List<DeserializedElement> Children { get; private set; }
        public string Value { get; private set; }
        public Dictionary<string, string> Attributes { get; private set; }

        public bool HasSubchildren
        {
            get
            {
                return (Children.Count != 0);
            }
        }

        public DeserializedElement(XElement element)
        {
            Element = element;

            SetName();
            SetAttributes();
            SetValue();
            SetChildren();
        }

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

        public void TurnAttributesIntoChildren()
        {
            foreach (var attribute in Attributes)
            {
                Children.Add(new DeserializedElement(name: attribute.Key, value: attribute.Value));
            }

            Attributes.Clear();
        }

        public void TurnValueIntoChild()
        {
            if (Value != null || Value != "")
            {
                Children.Add(new DeserializedElement(name: Name + "_value", value: Value));
                Value = null;
            }
        }

        protected string NormalizeAttributeValue(string attributeValue)
        {
            string returnString = attributeValue.Replace("\n", "");
            returnString = returnString.Replace("\r", "");
            returnString = returnString.Replace("\t", "");

            return returnString;
        }

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
