using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Common.Validations
{
    public abstract class ValidationBase
    {
        public bool IsSuccessful { get; protected set; }
        public List<string> ValidationFailedMessages { get; protected set; }

        public abstract void Validate();

    }
}
