using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Okta.SCIM.Server.Exceptions
{
    // Okta Exceptions
    public class DuplicateGroupException : Exception
    {
        public DuplicateGroupException()
        {
        }

        public DuplicateGroupException(string message)
            : base(message)
        {
        }

        public DuplicateGroupException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    public class OnPremUserManagementException : Exception
    {
        public OnPremUserManagementException()
        {
        }

        public OnPremUserManagementException(string message)
            : base(message)
        {
        }

        public OnPremUserManagementException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}