
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SCIMSQLConn
{

using System;
    using System.Collections.Generic;
    
public partial class dept_manager
{

    public string dept_no { get; set; }

    public int emp_no { get; set; }

    public System.DateTime from_date { get; set; }

    public System.DateTime to_date { get; set; }



    public virtual department department { get; set; }

    public virtual employee employee { get; set; }

}

}
