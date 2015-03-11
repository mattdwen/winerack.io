using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace winerack.Validation
{
    public class AtLeastOneItemAttribute : ValidationAttribute
    {
        #region Public Methods

        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null) {
                return list.Count > 0;
            }
            return false;
        }

        #endregion Public Methods
    }
}