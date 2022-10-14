using Amazon.CDK;
using Amazon.CDK.CustomResources;
using Constructs;
using System;
using System.Collections.Generic;

namespace Cloudfrontmultiregs3;

public class SSMParameterReader : AwsCustomResource
{
    public class SSMParameterReaderProps
    {
        public string ParameterName { get; set; }
        public string Region { get; set; }
    }

    public SSMParameterReader(Construct scope, string id, SSMParameterReaderProps props) : base(scope, id, new AwsCustomResourceProps
    {
        OnUpdate = new AwsSdkCall
        {
            Service = "SSM",
            Action = "getParameter",
            Parameters = new Dictionary<string, string>
                {
                    { "Name", props.ParameterName },
                },
            Region = props.Region,
            PhysicalResourceId = PhysicalResourceId.Of(DateTime.Now.ToString()),
        },
        Policy = AwsCustomResourcePolicy.FromSdkCalls(new SdkCallsPolicyOptions
        {
            Resources = new string[]
            {
                    Fn.Join(":", new string[]
                    {
                        "arn",
                        Fn.Ref("AWS::Partition"),
                        "ssm",
                        props.Region,
                        Fn.Ref("AWS::AccountId"),
                        $"parameter/{props.ParameterName}",
                    }),
            },
        }),
    })
    {
    }

    public String GetParameterValue()
    {
        return GetResponseField("Parameter.Value").ToString();
    }
}