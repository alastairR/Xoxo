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
    using System.Linq;
    using System.Collections.Generic;
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot(ElementName = "scenario", Namespace = "http://www.xbrl.org/2003/instance")]
    public class Scenario : IEquatable<Scenario>
    {
        Instance instanceField;

        [XmlIgnore]
        public Instance Instance
        {
            get { return instanceField; }
            set
            {
                instanceField = value;
                ExplicitMembers.Instance = value;
                TypedMembers.Instance = value;
            }
        }

        [XmlElement("explicitMember", Namespace = "http://xbrl.org/2006/xbrldi")]
        public ExplicitMemberCollection ExplicitMembers { get; set; }

        [XmlElement("typedMember", Namespace = "http://xbrl.org/2006/xbrldi")]
        public TypedMemberCollection TypedMembers { get; set; }

        public Scenario()
        {
            ExplicitMembers = new ExplicitMemberCollection();
            TypedMembers = new TypedMemberCollection();
        }

        public Scenario(Instance instance)
        {
            ExplicitMembers = new ExplicitMemberCollection(instance);
            TypedMembers = new TypedMemberCollection(instance);
        }

        public override string ToString()
        {
            var members = new List<string>();
            if(ExplicitMembers != null)
            {
                members.AddRange(ExplicitMembers.Select(m => m.ToString()));
            }
            if(TypedMembers != null)
            {
                members.AddRange(TypedMembers.Select(m => m.ToString()));
            }

            members.Sort();
            return string.Join(", ", members);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Scenario;
            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach(var m in TypedMembers)
            {
                hashCode = 31 * hashCode + m.GetHashCode();
            }
            foreach(var m in ExplicitMembers)
            {
                hashCode = 31 * hashCode + m.GetHashCode();
            }
            return hashCode;
        }

        public ExplicitMember AddExplicitMember(string dimension, string value)
        {
            return ExplicitMembers.Add(dimension, value);
        }

        public TypedMember AddTypedMember(string dimension, string domain, string value)
        {
            return TypedMembers.Add(dimension, domain, value);
        }

        #region IEquatable implementation

        public bool Equals(Scenario other)
        {
            var result = false;
            if(other != null)
            {
                if(ExplicitMembers.Equals(other.ExplicitMembers))
                {
                    result |= TypedMembers.Equals(other.TypedMembers);
                }
            }

            return result;
        }

        #endregion
    }
}