using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBT330Lab2
{
    public class DynamicDBModel
    {
        public List<string> AttributeNames { get; set; }
        public List<string> AttributeContents { get; set; }

        public override string ToString()
        {
            string retString = "";
            for (int i = 0; i < AttributeNames.Count; i++)
            {
                retString += $"{AttributeNames[i]} : {AttributeContents[i]}, ";
            }
            return retString;
        }

    }
}
