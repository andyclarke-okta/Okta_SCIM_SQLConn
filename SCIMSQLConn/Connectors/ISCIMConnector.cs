using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okta.SCIM.Models;
using Okta.SCIM.Server.Exceptions;

namespace SCIMSQLConn.Connectors
{
    // implement this interface to build an Okta connector
    public interface ISCIMConnector
    {
        SCIMUser createUser(SCIMUser user);
        SCIMUser getUser(String id);
        SCIMUserQueryResponse getUsers(PaginationProperties pageProperties, SCIMFilter filter);
        bool deleteUser(String id);
        SCIMUser updateUser(SCIMUser user);
        SCIMGroup createGroup(SCIMGroup group);
        SCIMGroup getGroup(String id);
        SCIMGroupQueryResponse getGroups(PaginationProperties pageProperties);
        bool deleteGroup(String id);
        bool updateGroup(SCIMGroup group);
        ServiceProviderConfiguration getServiceProviderConfig();
    }
}
