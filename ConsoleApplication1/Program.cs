using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Okta.SCIM.Models;
using Newtonsoft.Json;
using System.Linq.Expressions;
using SCIMSQLConn;
using SCIMSQLConn.Connectors;
using CsvHelper;

namespace ConsoleApplication1
{
    class Program
    {

        static List<string> filters =  new List<String>() {
                                         "userName eq \"AamodtAbdelkader@mysql-db.org\"",
                                         "id eq \"455773\"",
                                         "email eq \"bjensen@example.com\" OR email eq \"my-secondary-email@domain.com\"",
                                         "name.givenName eq \"john\"",
                                         "urn:Aokta:Aonprem_app:A1.0:Auser:Acustom:AdepartmentName  eq \"Cloud Service\""
        };

        static void printFilter(SCIMFilter filter)
        {
            if (filter.FilterExpressions != null)
            {
                Console.WriteLine(filter.FilterType);
                foreach (SCIMFilter f in filter.FilterExpressions)
                {
                    printFilter(f);
                }
            }
            else
            {
                Console.WriteLine(filter.FilterAttribute.AttributeName);
                Console.WriteLine(filter.FilterAttribute.SubAttributeName);
                Console.WriteLine(filter.FilterType);
                Console.WriteLine(filter.FilterValue.Replace("\"", ""));
            }
        }

        static Expression CreateExpression(ParameterExpression param, string propertyName)
        {
            Expression body = param;
            foreach (var member in propertyName.Split('.'))
            {
                body = Expression.PropertyOrField(body, member);
            }
            return body;
        }


        static string getAttributeString(SCIMFilterAttribute sfa)
        {
            var x = sfa.SubAttributeName;
            if (x == null)
            {
                return sfa.AttributeName;
            }
            else
            {
                return sfa.AttributeName + "." + x;
            }
        }

        static Expression CreateExpression(SCIMFilter f)
        {

            Console.WriteLine(f.FilterType);
            switch (f.FilterType)
            {
                case SCIMFilterType.EQUALS:
                    Console.WriteLine(getAttributeString(f.FilterAttribute));
                    Console.WriteLine(f.FilterValue);
                    break;
                case SCIMFilterType.OR:
                    break;
                case SCIMFilterType.UNKNOWN:
                    break;
                default:
                    break;
            }
            return null;
        }

        static void Main(string[] args)
        {

            //using (System.IO.StreamReader file = System.IO.File.OpenText(@"mapping.csv"))
            //{
            //    PPOAADToIglooGroupMap map = new PPOAADToIglooGroupMap();

            //        var csv = new CsvReader(file);
            //        var records = csv.GetRecords<PPOAADToIglooGroupMapRecord>();
            //        map.mappings = records.ToList<PPOAADToIglooGroupMapRecord>();
            //        foreach (var rec in map.mappings)
            //        {
            //            Console.WriteLine(rec.GroupName);
            //        }

            //        string v1 = "Accreditation & Eval222";
            //        string v2 = String.Empty;
            //        var query = (from rec in map.mappings
            //                     where rec.Department == v1 && rec.Division == v2
            //                     select rec.SpaceName).FirstOrDefault<string>();
            //        if (query == null)
            //        {
            //            Console.WriteLine("NULL");
            //        }

            //    Console.WriteLine(query);

               
            //    // department = Accreditation & Eval
            //    // division = Affiliate Services Division

                

            //}
            //using (System.IO.StreamReader file = System.IO.File.OpenText(@"10Users.json"))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    SCIMUserQueryResponse response =
            //        (SCIMUserQueryResponse)serializer.Deserialize(file, typeof(SCIMUserQueryResponse));

            //    foreach (SCIMUser u in response.Resources)
            //    {
            //        Console.WriteLine(u.id);
            //    }

            //    foreach (string f in filters)
            //    {
            //        Console.WriteLine();
            //        Console.WriteLine(f);
            //        SCIMFilter filter = SCIMFilter.TryParse(f);
            //        CreateExpression(filter);
            //        Console.WriteLine();
            //    }

                //// u => u.name.givenName == "Alagu"
                //ParameterExpression p = Expression.Parameter(typeof(SCIMUser), "u");
                //Expression m = CreateExpression(p, "name.givenName");
                //// list 
                //Expression m2 = CreateExpression(p, "emails");
                //// extension properties (starts with urn://)

                //Console.WriteLine(m2.Type);
                //Console.WriteLine(m2);
                //ConstantExpression c = Expression.Constant("Alagu", typeof(string));
                //BinaryExpression b = Expression.Equal(m, c);
                //LambdaExpression l = Expression.Lambda<Func<SCIMUser, bool>>(b, p);
                //Console.WriteLine(c);
                //Console.WriteLine(p);
                //Console.WriteLine(m);
                //Console.WriteLine(b);
                //Console.WriteLine(l);
                //Func<SCIMUser, bool> fn = (Func<SCIMUser,bool>) l.Compile();
                //Console.WriteLine(fn);

                //var x = response.Resources.AsQueryable<SCIMUser>();
                //var query = from SCIMUser user in x
                //            where fn(user)
                //            select user;

                //foreach (SCIMUser v in query)
                //{
                //    Console.WriteLine(v.id);
                //}
            //}
        }
    }
}
