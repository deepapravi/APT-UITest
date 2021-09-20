using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataDrivenFramework.Testcases
{
    public class KS2IndicatorsList : BaseTest
    {

      static  IDictionary<int, string> KS2Indicators = new Dictionary<int, string>();

        public static IDictionary<int, string> VerifyKS2IndicatorsList()
        {

            KS2Indicators.Add(1,"//td[contains(text(),'% Expected standard+ (Re, Wr, Ma)')]");
            KS2Indicators.Add(2,"//td[contains(text(),'% Higher standard (Re, Wr, Ma)')]");
            KS2Indicators.Add(3,"//td[contains(text(),'Average Scaled Score (Re, GPS, Ma)')]");
            KS2Indicators.Add(4,"//td[contains(text(),'Average Scaled Score (Re, Ma)')]");
            KS2Indicators.Add(5,"//td[contains(text(),'% Expected standard+ Reading')]");
            KS2Indicators.Add(6,"//td[contains(text(),'% Higher standard Reading')]");
            KS2Indicators.Add(7,"//td[contains(text(),'Scaled Score Reading')]");
            KS2Indicators.Add(8,"//td[contains(text(),'% Expected standard+ Writing')]");
            KS2Indicators.Add(9,"//td[contains(text(),'% Higher standard Writing')]");
            KS2Indicators.Add(10,"//td[contains(text(),'Scaled Score Writing')]");
            KS2Indicators.Add(11,"//td[contains(text(),'% Expected standard+ Maths')]");
            KS2Indicators.Add(12,"//td[contains(text(),'% Higher standard Maths')]");
            KS2Indicators.Add(13,"//td[contains(text(),'Scaled Score Maths')]");
            KS2Indicators.Add(14,"//td[contains(text(),'% Expected standard+ Grammar, Punctuation & Spelli')]");
            KS2Indicators.Add(15,"//td[contains(text(),'% Higher standard Grammar, Punctuation & Spelling')]");
            KS2Indicators.Add(16,"//td[contains(text(),'Scaled Score Grammar, Punctuation & Spelling')]");
                      
            return KS2Indicators;

        }


    }

        


    
}
