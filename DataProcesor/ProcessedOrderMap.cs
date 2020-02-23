using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcesor
{
    public class ProcessedOrderMap: ClassMap<ProcessedOrder>
    {
        public ProcessedOrderMap()
        {
            AutoMap(CultureInfo.InvariantCulture);

            Map(m => m.Customer).Name("CustomerNumber");
            Map(m => m.Amount).Name("Quantity").TypeConverter<RomanTypeConverter>();
        }
    }
}
