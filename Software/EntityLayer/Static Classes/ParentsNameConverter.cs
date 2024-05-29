using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EntityLayer.Static_Classes
{
    public class ParentsNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var parents = value as ICollection<Parent>;

            if (parents == null || parents.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder parentsBuilder = new StringBuilder();

            foreach (var parent in parents)
            {
                parentsBuilder.Append($"{parent.FirstName} {parent.LastName}, ");
            }

            if (parentsBuilder.Length > 0)
            {
                parentsBuilder.Length -= 2;
            }

            return parentsBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
