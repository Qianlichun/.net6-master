using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;

namespace Project.QLC.Framework.WebAPI.Filter
{
    /// <summary>
    /// 方法名称：FileUploadFilter
    /// 创 建 人：qianxiaoxiao
    /// 创建时间：2023/8/2 10:33:39
    /// </summary>
    public class FileUploadFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            const string FileUploadContentType = "multipart/form-data";
            if (operation.RequestBody == null || !operation.RequestBody.Content.Any(x =>
            x.Key.Equals(FileUploadContentType, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            if (context.ApiDescription.ParameterDescriptions[0].Type == typeof(IFormCollection))
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Description = "文件上传",
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        {
                            FileUploadContentType,new OpenApiMediaType
                            {
                                Schema=new OpenApiSchema
                                {
                                    Type="object",
                                    Required=new HashSet<string>{ "file"},
                                    Properties=new Dictionary<string,OpenApiSchema>
                                    {
                                        {
                                            "file",new OpenApiSchema()
                                            {
                                                Type="string",
                                                Format="binary"
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}