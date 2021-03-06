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
    using System.Globalization;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot(ElementName = "period", Namespace = "http://www.xbrl.org/2003/instance")]
    public class Period : IEquatable<Period>
    {
        static IFormatProvider ic = CultureInfo.InvariantCulture;

        [XmlElement(ElementName = "instant", DataType = "date", Namespace = "http://www.xbrl.org/2003/instance")]
        public DateTime Instant { get; set; }

        [XmlElement(ElementName = "startDate", DataType = "date", Namespace = "http://www.xbrl.org/2003/instance")]
        public DateTime StartDate { get; set; }
        [XmlElement(ElementName = "endDate", DataType = "date", Namespace = "http://www.xbrl.org/2003/instance")]
        public DateTime EndDate { get; set; }

        public Period()
        {

        }

        public Period(DateTime instant)
            : this()
        {
            Instant = instant;
        }

        public Period(int year, int month, int day)
            : this()
        {
            Instant = new DateTime(year, month, day);
        }

        public Period(DateTime startDate, DateTime endDate)
            : this()
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public Period(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
            : this()
        {
            StartDate = new DateTime(startYear, startMonth, startDay);
            EndDate = new DateTime(endYear, endMonth, endDay);
        }

        public bool ShouldSerializeInstant()
        {
            return Instant != null && Instant != DateTime.MinValue;
        }

        public bool ShouldSerializeStartDate()
        {
            return StartDate != null && StartDate != DateTime.MinValue;
        }

        public bool ShouldSerializeEndDate()
        {
            return EndDate != null && EndDate != DateTime.MinValue;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Period;
            return other != null && Equals(other);
        }

        public override string ToString()
        {
            return string.Format(ic, "Instant={0:yyyy-MM-dd}", Instant);
        }

        #region IEquatable implementation

        public bool Equals(Period other)
        {
            return other != null && Instant.Equals(other.Instant);
        }

        public override int GetHashCode()
        {
            return Instant.Date.GetHashCode();
        }

        #endregion
    }
}