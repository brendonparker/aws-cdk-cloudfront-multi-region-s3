using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Constructs;

namespace Cloudfrontmultiregs3;

public class SecondaryRegionStackProps : StackProps
{
    public string BucketArnParameterName { get; set; }
}

public class SecondaryRegionStack : Stack
{
    internal SecondaryRegionStack(Construct scope, string id, SecondaryRegionStackProps props) : base(scope, id, props)
    {
        var bucket = new Bucket(this, "Bucket");
        bucket.ApplyRemovalPolicy(RemovalPolicy.DESTROY);

        new StringParameter(this, "BucketArn", new StringParameterProps
        {
            ParameterName = props.BucketArnParameterName,
            Description = $"CDK managed.",
            StringValue = bucket.BucketArn,
        });
    }
}
