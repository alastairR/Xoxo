//
//  This file is part of Diwen.Xbrl.
//
//  Author:
//       John Nordberg <john.nordberg@gmail.com>
//
//  Copyright (c) 2015-2016 John Nordberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace Diwen.Xbrl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml;
    using System.Xml.Serialization;

    public class TypedMemberCollection : Collection<TypedMember>, IEquatable<IList<TypedMember>>
    {
        Instance instanceField;

        [XmlIgnore]
        public Instance Instance
        {
            get { return instanceField; }
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException();
                }
                instanceField = value;
                var dimNs = instanceField.DimensionNamespace;
                var dimPrefix = instanceField.Namespaces.LookupPrefix(dimNs);
                var domNs = instanceField.TypedDomainNamespace;
                var domprefix = instanceField.Namespaces.LookupPrefix(domNs);

                foreach(var item in this)
                {
                    item.Instance = value;
                    if(item.Dimension.Namespace != instanceField.DimensionNamespace)
                    {
                        item.Dimension = new XmlQualifiedName(dimPrefix + ":" + item.Dimension.Name, dimNs);
                    }

                    if(item.Domain.Namespace != instanceField.TypedDomainNamespace)
                    {
                        item.Domain = new XmlQualifiedName(domprefix + ":" + item.Domain.Name, domNs);
                    }
                }
            }
        }

        public TypedMemberCollection()
        {
        }

        public TypedMemberCollection(Instance instance)
            : this()
        {
            Instance = instance;
        }

        public TypedMember Add(string dimension, string domain, string value)
        {

            XmlQualifiedName dim;
            XmlQualifiedName dom;
            if(Instance != null)
            {
                dim = new XmlQualifiedName(dimension, Instance.DimensionNamespace);
                dom = new XmlQualifiedName(domain, Instance.TypedDomainNamespace);
            }
            else
            {
                dim = new XmlQualifiedName(dimension);
                dom = new XmlQualifiedName(domain);
            }

            var typedMember = new TypedMember(dim, dom, value);
            base.Add(typedMember);
            return typedMember;
        }

        #region IEquatable implementation

        public bool Equals(IList<TypedMember> other)
        {
            return this.ContentCompare(other);
        }

        #endregion
    }
}