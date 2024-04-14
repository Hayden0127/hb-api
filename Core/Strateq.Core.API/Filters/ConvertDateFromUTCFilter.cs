using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using Strateq.Core.Utilities;
using Strateq.Core.Model;
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace Strateq.Core.API.Filter
{
    public class ConvertDateFromUTCFilter : ActionFilterAttribute
    {
        HttpRequest Request;
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult result)
            {
                Request = context.HttpContext.Request;

                if (result.Value is not ProblemDetails && result.Value is not string)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(result.Value.GetType()))
                    {
                        Type type = result.Value.GetType().GenericTypeArguments[0];
                        PropertyInfo[] input = type.GetProperties();

                        foreach (object v in result.Value as IEnumerable)
                        {
                            Convert(input, v);
                        }
                    }
                    else
                    {
                        Convert(result.Value.GetType().GetProperties(), result.Value);
                    }
                }
            }
        }

        void Convert(IEnumerable<PropertyInfo> input, object obj)
        {
            IEnumerable<PropertyInfo> infos = input
                .Where(p =>
                p.PropertyType != typeof(string) &&
                (p.PropertyType == typeof(DateTime) ||
                p.PropertyType == typeof(DateTime?) ||
                p.PropertyType.IsClass));

            if (infos.Any())
            {
                foreach (PropertyInfo info in infos)
                {
                    if (info.PropertyType == typeof(DateTime) || info.PropertyType == typeof(DateTime?))
                    {
                        DateTime? oldDate = info.GetValue(obj) as DateTime?;
                        TimeZoneInfo sg = TimeZoneInfo.FindSystemTimeZoneById("Singapore");
                        if (oldDate != null)
                        {
                            var newDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)oldDate, sg);
                            info.SetValue(obj, newDate);
                        }
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(info.PropertyType))
                    {
                        Type[] typeResult = info.PropertyType.GenericTypeArguments;
                        Type type = info.PropertyType.GenericTypeArguments[0];
                        IEnumerable list = info.GetValue(obj) as IEnumerable;
                        if (list != null)
                        {
                            foreach (var v in list)
                            {
                                Convert(type.GetProperties(), v);
                            }

                        }
                    }
                    else if (info.GetValue(obj) != null)
                    {
                        Convert(info.PropertyType.GetProperties(), info.GetValue(obj));
                    }
                }
            }
        }
    }
}

