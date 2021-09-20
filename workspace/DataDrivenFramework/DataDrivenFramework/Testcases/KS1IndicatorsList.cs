using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenFramework.Testcases
{
    public class KS1IndicatorsList : BaseTest
    {

        static IDictionary<int, string> KS1Indicators = new Dictionary<int, string>();

        public static IDictionary<int, string> VerifyKS1IndicatorsList()
        {

            KS1Indicators.Add(1, "//td[contains(text(),'% Expected standard+ (Re, Wr, Ma)')]");
            KS1Indicators.Add(2, "//td[contains(text(),'% Higher standard (Re, Wr, Ma)')]");
            KS1Indicators.Add(3, "//td[contains(text(),'% Expected Standard + Reading')]");
            KS1Indicators.Add(4, "//td[contains(text(),'% Higher Standard Reading')]");
            KS1Indicators.Add(5, "//td[contains(text(),'Scaled Score Reading')]");
            KS1Indicators.Add(6, "//td[contains(text(),'% Expected Standard + Maths')]");
            KS1Indicators.Add(7, "//td[contains(text(),'% Higher Standard Maths')]");
            KS1Indicators.Add(8, "//td[contains(text(),'Scaled Score Maths')]");
            KS1Indicators.Add(9, "//td[contains(text(),'% Expected Standard + Writing')]");
            KS1Indicators.Add(10, "//td[contains(text(),'% Higher Standard Writing')]");
            KS1Indicators.Add(11, "//td[contains(text(),'Scaled Score Writing')]");
          
            return KS1Indicators;

        }


    }





}
