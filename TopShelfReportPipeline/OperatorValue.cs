using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopShelfReportPipeline
{
    public enum OperatorValue
    {
        Blank,
        NotBlank,
        UsePreviousOR,
        LessThan,
        GreaterThan,
        Between,
        LessThanNot,
        GreaterThanNot,
        NotBetween,
        Equals,
        Equals_TextArea,
        Equals_Autocomplete,
        Equals_Select,
        Equals_Multiple,
        EqualsPopup,
        Equals_CheckBoxes,
        NotEquals,
        NotEquals_Select,
        NotEquals_Multiple,
        NotEqualsPopup,
        LessThanField,
        GreaterThanField,
        EqualsField,
        NotEqualsField,
        Like,
        BeginsWith,
        EndsWith,
        NotLike
    }
}
